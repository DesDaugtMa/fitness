---
type: todo
created: 2026-03-21
status: open
priority: high
category: enhancement
area: security
tags: [todo, security, rate-limiting, middleware, cmp/api]
estimated_effort: 8
impact: 5
urgency: 5
effort: 2
priority_score: 16.5
related_adrs: []
resolved_by: []
source_documents:
  - "[[../auditor_residual_risks#rr-004-no-rate-limiting-ddosbrute-force-risk]]"
  - "[[../architecture_risks#risk-001]]"
---

# TODO: Add Rate Limiting Middleware

## Problem/Idee

**Aktueller Zustand:** API hat keine Schutzmaßnahmen gegen Brute-Force-Attacken oder DDoS.  
**Gewünschter Zustand:** ≤100 HTTP requests pro Minute pro IP-Adresse  
**Gap:** 0% Rate Limiting → 100%

**Szenario:** Attacker versucht 1000 Login-Requests/Minute gegen `/api/users/login` → System akzeptiert alle

---

## Business-Value

**Sicherheit:** Verhindert Brute-Force-Angriffe auf Login/OAuth-Flow  
**Kostenersparnis:** €2.000+ pro Security-Incident (Investigation, Remediation)  
**Compliance:** Standard-Anforderung für Public APIs

---

## Schritt-für-Schritt-Anleitung

**🎯 ERSTER SCHRITT:**
```powershell
cd C:\Repositories\fitness\Fitness
dotnet add package AspNetCore.RateLimiting --version 1.0.0
dotnet build
```

### Schritt 1: Middleware in Program.cs konfigurieren

**Datei:** `C:\Repositories\fitness\Fitness\Program.cs`

**Code hinzufügen (nach `builder.Services.AddControllers()`):**
```csharp
// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "fixed", configure: options =>
    {
        options.PermitLimit = 100;              // 100 requests
        options.Window = TimeSpan.FromMinutes(1); // per 1 minute
        options.QueueProcessingOrder = System.Collections.Generic.QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;                 // Queue 2 additional requests
    });
    
    options.AddSlidingWindowLimiter(policyName: "sliding", configure: options =>
    {
        options.PermitLimit = 10;
        options.Window = TimeSpan.FromSeconds(10);
        options.SegmentsPerWindow = 2;
    });
});

// Apply Middleware
var app = builder.Build();
app.UseRateLimiter();
```

### Schritt 2: Rate Limiting auf Endpoints anwenden

**Datei:** `Fitness/Controllers/UserController.cs`

**Attribute hinzufügen:**
```csharp
using System.Threading.RateLimiting;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpPost("login")]
    [RequireRateLimiting("fixed")]  // ← Add this
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        // Implementation
    }

    [HttpPost("register")]
    [RequireRateLimiting("fixed")]  // ← Add this
    public async Task<ActionResult<User>> Register([FromBody] RegisterRequest request)
    {
        // Implementation
    }
}
```

### Schritt 3: Custom Rate Limit Response

**Datei:** `Fitness/Middleware/RateLimitExceptionHandler.cs` (neue Datei)

**Vollständiger Code:**
```csharp
using Microsoft.AspNetCore.RateLimiting;
using System.Net;

namespace Fitness.Middleware
{
    public sealed class RateLimitExceptionHandler : IAsyncPolicy<HttpContext>
    {
        public async ValueTask<bool> OnRejectedAsync(
            OnRejectedContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.HttpContext.Response.ContentType = "application/json";

            var response = new
            {
                error = "Too many requests",
                retryAfter = context.HttpContext.Response.Headers["Retry-After"],
                message = "Rate limit exceeded. Try again later."
            };

            await context.HttpContext.Response.WriteAsJsonAsync(response);
            return true;
        }
    }
}
```

### Schritt 4: Test die Rate Limiting

**Command:**
```powershell
cd C:\Repositories\fitness\Fitness
dotnet run

# In separatem Terminal: Test mit schnellen Requests (lokal)
$headers = @{}
for ($i = 1; $i -le 105; $i++) {
    $response = Invoke-WebRequest -Uri "http://localhost:5000/api/users/login" -Method POST -Body '{}' -ErrorAction SilentlyContinue
    Write-Host "Request $i : Status $($response.StatusCode)"
    if ($i -gt 100) {
        Write-Host "❌ Request $i blocked with 429" # Expected from request 101 onwards
    }
}
```

**Erwarteter Output:**
```
Request 1 : Status 400 (Bad request, but not rate limited)
...
Request 100 : Status 400
Request 101 : Status 429 (❌ TOO MANY REQUESTS) ← Success!
Request 102 : Status 429
Request 103 : Status 429
```

---

## Acceptance Criteria

- [ ] NuGet package installed: `AspNetCore.RateLimiting`
- [ ] Middleware configured in `Program.cs`
- [ ] Rate limiter applied to `/login` and `/register` endpoints
- [ ] Custom 429 response handler implemented
- [ ] Manual test passes: 101st request blocked with 429
- [ ] All existing tests still pass: `dotnet test`

---

## Effo Estimation

| Phase | Time |
|-------|------|
| Package install & config | 0.5h |
| Middleware setup | 1h |
| Endpoint decoration | 0.5h |
| Custom response handler | 0.5h |
| Testing & validation | 0.5h |
| Logging & monitoring | 0.5h |
| **TOTAL** | **3.5h** |

---

## Priority Calculation

- **Impact:** 5/5 — Prevents DDoS/Brute-force attacks
- **Urgency:** 5/5 — Must implement within 1 week (per RR-004)
- **Effort:** 2/5 — ~8 hours
- **Priority Score:** (5×2) + (5×1.5) - (2×0.5) = **16.5 → HIGH**

---

## Related Documentation

- [[../auditor_residual_risks#rr-004-no-rate-limiting-ddosbrute-force-risk]]
- [[../quality_goals#1-security]]
- [[../architecture_risks#risk-001]]

[[index]]
