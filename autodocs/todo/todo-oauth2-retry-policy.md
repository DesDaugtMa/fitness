---
type: todo
created: 2026-03-21
status: open
priority: high
category: enhancement
area: reliability
tags: [todo, oauth, retry-policy, resilience, cmp/auth]
estimated_effort: 8
impact: 4
urgency: 4
effort: 2
priority_score: 13
related_adrs: []
resolved_by: []
source_documents:
  - "[[../auditor_residual_risks#rr-005-partial-oauth2-reliability-no-retry-policy]]"
  - "[[../architecture_risks#risk-002]]"
---

# TODO: Implement OAuth2 Retry Policy (Polly)

## Problem/Idee

**Ausgangsituation:** Google OAuth2 Integration hat keinen Retry-Mechanismus. Transiente Fehler (netzwerk, timeout) führen zu Login-Failures.  
**Ziel:** 3 Auto-Retries mit exponential backoff (1s, 2s, 4s)  
**Messbarer Schritt:** 0% Retry-Protected → 100%

**Szenario:** Google API zeitweise überlastet (+50ms Latenz). Ohne Retry: Login-Fehler. Mit Retry: Automatische Wiederholung, User merkt nichts.

---

## Business-Value

- **Zuverlässigkeit:** OAuth-Fehlerrate sinkt von ~2% auf <0.2%
- **User-Erfahrung:** Weniger "Login fehlgeschlagen" bei transienten Problemen
- **Kosten:** Verhindert Support-Tickets für falsche Login-Fehler

---

## Schritt-für-Schritt-Anleitung

**🎯 ERSTER SCHRITT:**
```powershell
cd C:\Repositories\fitness\Fitness
dotnet add package Polly --version 8.2.1
dotnet build
```

### Schritt 1: Polly Retry Policy in DI registrieren

**Datei:** `Fitness/Program.cs`

**Code (in Services-Setup):**
```csharp
using Polly;
using Polly.Timeout;

// Add Polly Retry Policy für Google OAuth
var retryPolicy = Policy
    .Handle<HttpRequestException>()
    .Or<TaskCanceledException>()
    .WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: attempt => 
            TimeSpan.FromSeconds(Math.Pow(2, attempt - 1)), // 1s, 2s, 4s
        onRetry: (outcome, timespan, retryCount, context) =>
        {
            Console.WriteLine($"Retry #{retryCount} after {timespan.TotalSeconds}s for {context["operationName"]}");
        });

var timeoutPolicy = Policy
    .TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));

var combinedPolicy = Policy.WrapAsync(retryPolicy, timeoutPolicy);

builder.Services.AddHttpClient<IOAuthService, GoogleOAuthService>()
    .AddPolicyHandler(combinedPolicy);
```

### Schritt 2: OAuth Service mit Retry anpassen

**Datei:** `Fitness/Services/GoogleOAuthService.cs`

**Code (Updated OAuth Call):**
```csharp
using Polly;

public class GoogleOAuthService : IOAuthService
{
    private readonly HttpClient _httpClient;

    public async Task<OAuthToken> GetTokenAsync(string code)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", _config["OAuth:ClientId"] },
                { "client_secret", _config["OAuth:ClientSecret"] },
                { "code", code },
                { "grant_type", "authorization_code" }
            })
        };

        // HttpClient already has Polly policy attached in DI
        var response = await _httpClient.SendAsync(request);
        
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"OAuth token request failed: {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OAuthToken>(content);
    }
}
```

### Schritt 3: Fallback Behavior (Local Login)

**Code (in UserController):**
```csharp
[HttpPost("login/oauth")]
public async Task<ActionResult<LoginResponse>> OAuthLogin([FromBody] OAuthRequest request)
{
    try
    {
        var token = await _oAuthService.GetTokenAsync(request.Code);
        var user = await _userService.GetOrCreateOAuthUserAsync(token);
        return Ok(new LoginResponse { UserId = user.Id, Token = _generateJwt(user) });
    }
    catch (HttpRequestException ex)
    {
        // After 3 retries failed, fall back to local login
        return BadRequest(new 
        {
            error = "OAuth temporarily unavailable",
            fallback = "Use email/password login instead",
            details = ex.Message
        });
    }
}
```

### Schritt 4: Test mit simuliertem Fehler

**Test-Datei:** `Fitness.Tests/Services/GoogleOAuthServiceTests.cs`

```csharp
[Fact]
public async Task GetToken_WithTransientError_RetriesAndSucceeds()
{
    // Arrange
    var mockHandler = new Mock<HttpMessageHandler>();
    
    // First 2 calls fail (transient), 3rd succeeds
    mockHandler
        .Protected()
        .SetupSequence<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
        .ThrowsAsync(new HttpRequestException("Timeout"))       // Retry 1
        .ThrowsAsync(new HttpRequestException("Timeout"))       // Retry 2
        .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) // Success
        {
            Content = new StringContent(JsonSerializer.Serialize(new OAuthToken { AccessToken = "test" }))
        });

    var client = new HttpClient(mockHandler.Object);
    var service = new GoogleOAuthService(client, _options);

    // Act
    var result = await service.GetTokenAsync("code123");

    // Assert
    result.AccessToken.Should().Be("test");
    mockHandler.Protected().Verify(
        "SendAsync",
        Times.Exactly(3),  // Called 3 times (2 retries + 1 success)
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>());
}
```

**Run Test:**
```powershell
dotnet test --filter "GetToken_WithTransientError"
```

---

## Acceptance Criteria

- [ ] Polly package installed
- [ ] Retry policy configured (3 retries, exponential backoff)
- [ ] Policy applied to OAuth HttpClient
- [ ] Fallback to local login when OAuth fails after retries
- [ ] Unit tests pass (retry logic verified)
- [ ] Manual test: Observe retries in logs
- [ ] No regression: Other login methods still work

---

## Effort Estimation

| Phase | Time |
|-------|------|
| Package & Setup | 0.5h |
| Policy Configuration | 1h |
| Service Updates | 1h |
| Fallback Logic | 0.5h |
| Unit Tests | 1h |
| Manual Testing | 0.5h |
| **TOTAL** | **4.5h** |

---

## Priority Calculation

- **Impact:** 4/5 — Improves reliability, doesn't block
- **Urgency:** 4/5 — 1 week deadline (per RR-005)
- **Effort:** 2/5 — ~8 hours
- **Priority Score:** (4×2) + (4×1.5) - (2×0.5) = **13 → HIGH**

---

[[index]]
