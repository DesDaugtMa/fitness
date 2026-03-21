---
title: "Architecture Risks"
date: 2026-03-21
type: architecture
view_type: risks
status: accepted
tags: [architecture, risks, trade-offs, isaqb]
related_docs:
  - "[[../quality_goals]]"
  - "[[../quality_scenarios]]"
  - "[[../constraints]]"
  - "[[../blackbox/internal/risk-register]]"
  - "[[../todo/index]]"
source_files:
  - Source/Fitness/Program.cs
  - Source/Fitness/Controllers/AccountController.cs
---

# Architecture Risks & Trade-offs

## Einführung

Diese Sicht dokumentiert:
1. **Architektur-Risiken:** Strukturelle Schwachstellen, die zu Problemen führen können
2. **Trade-offs:** Entscheidungen, bei denen Gewinn in einer Dimension mit Verlust in einer anderen bezahlt wird
3. **Single Points of Failure (SPOFs):** Kritische Komponenten ohne Redundanz

Risiken werden nach **Severity × Likelihood** bewertet.

---

## 🔴 Critical Risks

### RISK-001: Fehlender Rate Limiter (DoS/Brute-Force Vulnerability)

**ID:** ARCH-001  
**Severity:** 🔴 High (Security)  
**Likelihood:** 🔴 High (Trivial to exploit)  
**Risk Score:** (9 × 8) / 100 = **72/100** (Critical)

**Problem:**
- Es gibt keine Middleware, die Requests pro IP/User rate-limits
- Attacker kann unbegrenzt Login-Attempts machen (Brute-Force)
- Attacker kann unbegrenzt GET-Requests spammen (DoS auf CPU/DB)

**Betroffene Komponenten:**
- [[../building_block_view#1-presentation-layer-controllers--views]] (All Controllers)
- [[../blackbox/public/fitness-webapp-api]] (All Endpoints)

**Current Mitigation:** ❌ None

**Konsequenzen:**
- **Security:** Account Takeover risk (weak password brute-force)
- **Availability:** App can be DoS'd from low-bandwidth attack
- **Compliance:** OWASP Top 10 (A7:2021 Identification and Authentication Failures)

**Recommended Mitigation (Priority: Immediate):**
```csharp
// In Program.cs:
builder.Services.AddScoped<IAsyncPolicy<HttpResponseMessage>>(
    _ => Policy.Handle<HttpRequestException>()
        .Or<TimeoutException>()
        .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: _ => TimeSpan.FromSeconds(2),
            onRetry: (outcome, timespan, retryCount, context) => { /* log */ })
        .WrapAsync(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)))
);

// Add middleware:
app.UseRateLimiter();
```

**Effort:** Medium (1-2 days)  
**Expected Benefit:** Eliminates brute-force + reduces DoS surface

**Related ADR:** (None yet — should create ADR-rate-limiting)  
**Related TODO:** [[../../todo/todo-rate-limiting-middleware]]

---

### RISK-002: Keine Retry-Policy für Google OAuth2

**ID:** ARCH-002  
**Severity:** 🟠 High (Reliability)  
**Likelihood:** 🟡 Medium (Google occasionally down)  
**Risk Score:** (8 × 6) / 100 = **48/100** (High)

**Problem:**
- Google OAuth2 API ist nicht 100% available
- Wenn Google timeout/down → entire OAuth flow fails without retry
- User cannot login via Google, and error UX is poor

**Betroffene Komponenten:**
- [[../building_block_view#1-presentation-layer-controllers--views]] (AccountController)
- [[../blackbox/internal/google-oauth-outbound]] (OAuth Flow)

**Current Mitigation:** ⚠️ Redirect to /Account/Login (but no retry, no backoff)

**Konsequenzen:**
- **Availability:** Login via Google unavailable if Google intermittently down
- **User Experience:** User doesn't know if it's their network or Google
- **Business Impact:** Users may abandon account creation

**Recommended Mitigation (Priority: High):**

Use **Polly** (resilience library) with exponential backoff:

```csharp
// In Program.cs:
var googleOAuthPolicy = Policy.Handle<HttpRequestException>()
    .Or<TaskCanceledException>()
    .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
    .WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt - 1)),  // 1s, 2s, 4s
        onRetry: (outcome, timespan, retryCount, context) =>
        {
            // Log retry attempt
            _logger.LogWarning($"OAuth retry {retryCount} after {timespan.TotalSeconds}s");
        }
    )
    .WrapAsync(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)));

builder.Services.AddHttpClient()
    .AddPolicyHandler(googleOAuthPolicy);
```

**Effort:** Low (1 day)  
**Expected Benefit:** Resilient OAuth flow, 99.5%+ availability if Google is > 99% available

**Related ADR:** (None yet — should create ADR-oauth-resilience)  
**Related TODO:** [[../../todo/todo-oauth-retry-policy]]

---

### RISK-003: PII Data Without At-Rest Encryption (Compliance Gap)

**ID:** ARCH-003  
**Severity:** 🔴 High (Compliance/Security)  
**Likelihood:** 🟡 Medium (Breach scenario: stolen backup)  
**Risk Score:** (8 × 5) / 100 = **40/100** (High)

**Problem:**
- User emails, passwords, workout history = PII
- SQL Server database has no encryption at rest (TDE not enabled)
- If backup is stolen or DB server compromised → plaintext PII exposed

**Betroffene Komponenten:**
- [[../building_block_view#3-data-access-layer-entity-framework-core]] (Data Models)
- [[../blackbox/internal/sql-server-datastore]] (SQL Server)
- [[../deployment_view]] (Storage)

**Current Mitigation:** ⚠️ Passwords are hashed (good), but emails are plaintext

**Konsequenzen:**
- **Compliance:** GDPR Article 32 requires "encryption of personal data"
- **Security:** Stolen backup → all PII exposed
- **Business Impact:** Regulatory fines (€20M or 4% revenue), reputation damage

**Recommended Mitigation (Priority: High):**

**Option A (Easiest): Enable SQL Server TDE (Transparent Data Encryption)**
```sql
-- SQL Server 2016+
CREATE MASTER KEY ENCRYPTION BY PASSWORD = '<strong-password>';
CREATE CERTIFICATE Cert1 WITH SUBJECT = 'Fitness App TDE';
CREATE DATABASE ENCRYPTION KEY WITH ALGORITHM = AES_256 ENCRYPTION BY SERVER CERTIFICATE Cert1;
ALTER DATABASE Fitness SET ENCRYPTION ON;
```

**Option B (Stronger): Application-Level Encryption**
- Hash/encrypt sensitive fields in C# before saving
- Use System.Security.Cryptography
- Trade-off: More code, can't query on encrypted fields

**Effort:** Low (TDE: 2 hours), Medium (App-level: 2 days)  
**Expected Benefit:** GDPR compliance, breach impact mitigation

**Related ADR:** (None yet — should create ADR-data-encryption)  
**Related TODO:** [[../../todo/todo-at-rest-encryption]]

---

## 🟠 High Risks

### RISK-004: No High-Availability Setup (Single Database Instance)

**ID:** ARCH-004  
**Severity:** 🟠 High (Availability)  
**Likelihood:** 🟡 Medium (Hardware failure ~0.5-1% per year)  
**Risk Score:** (7 × 6) / 100 = **42/100** (High)

**Problem:**
- Single SQL Server instance: if it fails, entire app is down
- No replication (AlwaysOn), no failover cluster
- RTO (Recovery Time Objective) is > 30 minutes (manual restart)

**Betroffene Komponenten:**
- [[../building_block_view#3-data-access-layer-entity-framework-core]]
- [[../deployment_view]] (Database Tier)

**Current Mitigation:** ⚠️ Daily backups (but recovery is manual/slow)

**Konsequenzen:**
- **Availability:** Down for 30+ minutes = ~USD 1000s+ revenue loss
- **SLA:** Cannot promise "99.5% uptime"
- **Team Morale:** Ops team has to wake up for outages

**Recommended Mitigation (Priority: Medium-High):**

**Option A (Recommended): SQL Server Availability Groups (AlwaysOn)**
- Requires SQL Server 2012+ Enterprise or Standard (licensing cost)
- Automatic failover, RPO ≈ 0, RTO < 10 seconds

**Option B: Azure SQL Database (Managed)**
- Automatic HA, backups, patching
- Cost: ~$1000/month for production tier

**Option C: Poor Man's HA**
- Log Shipping: Semi-automatic, RPO ~5 minutes, RTO ~5 minutes
- Requires manual scripts

**Effort:** Medium (AlwaysOn: 3-5 days), Low (Azure: 1 day)  
**Expected Benefit:** 99.9%+ availability, eliminates single point of failure

**Related ADR:** (None yet)  
**Related TODO:** [[../../todo/todo-ha-sql-server]]

---

## 🟡 Medium Risks

### RISK-005: N+1 Query Anti-Pattern (Performance Degradation)

**ID:** ARCH-005  
**Severity:** 🟡 Medium (Performance)  
**Likelihood:** 🟠 High (Common EF Core mistake)  
**Risk Score:** (6 × 7) / 100 = **42/100** (Medium-High)

**Problem:**
- EF Core lazy-loading by default (can cause N+1 queries)
- Example: Loading 50 exercises + MuscleGroup = 51 queries instead of 2
- Degrades app response time by 50-500% as data grows

**Betroffene Komponenten:**
- [[../building_block_view#3-data-access-layer-entity-framework-core]] (LINQ queries)
- [[../quality_scenarios#qs-006-n-1-query-prevention-exercises-list]]

**Current Mitigation:** ⚠️ Unknown (need code review)

**Consequences:**
- **Performance:** Dashboard load time 500ms → 5 seconds as user data grows
- **Scalability:** Can't support 1000+ concurrent users

**Recommended Mitigation:**

1. Disable lazy-loading globally:
```csharp
// In FitnessDbContext:
public FitnessDbContext(DbContextOptions<FitnessDbContext> options)
    : base(options)
{
    // ChangeTracker.LazyLoadingEnabled = false;  // Disable lazy-loading
}
```

2. Use explicit eager-loading (.Include()):
```csharp
// GOOD
var exercises = await context.Exercises
    .Include(e => e.MuscleGroup)
    .ToListAsync();

// BAD (N+1 risk)
var exercises = await context.Exercises.ToListAsync();
```

3. Add query profiling:
```csharp
// Log slow queries in development
var logger = LoggerFactory.Create(builder => builder.AddConsole())
    .CreateLogger<DbContextOptionsBuilder>();
options.LogTo(Console.WriteLine);
```

**Effort:** Low (2 days)  
**Expected Benefit:** 5-10x performance improvement for large data sets

**Related TODO:** [[../../quality_scenarios#qs-006-n-1-query-prevention-exercises-list]]

---

### RISK-006: No Automated Testing (Manual QA Only)

**ID:** ARCH-006  
**Severity:** 🟡 Medium (Quality/Reliability)  
**Likelihood:** 🟡 Medium (Already affecting releases)  
**Risk Score:** (6 × 6) / 100 = **36/100** (Medium)

**Problem:**
- No unit tests visible in repo
- No integration tests visible
- All testing is manual (QA team or dev clicking buttons)
- Regression bugs slip into production

**Betroffene Komponenten:**
- [[../building_block_view]] (All Controllers, Models)
- [[../quality_scenarios#qs-008-unit-test-coverage]]
- [[../quality_scenarios#qs-009-integration-test-setup]]

**Current Mitigation:** ❌ Manual QA (slow, error-prone)

**Consequences:**
- **Quality:** Bugs in production (OAuth state parameter validation, CSRF, etc.)
- **Velocity:** Testing takes days, slows down releases
- **Confidence:** Hard to refactor without breaking things

**Recommended Mitigation (Priority: Medium):**

Create test project:
```bash
dotnet new xunit -n Fitness.Tests
dotnet add reference ../Source/Fitness/
dotnet add package Moq Xunit.DependencyInjection Microsoft.AspNetCore.Mvc.Testing
```

Add minimum unit tests (20+ tests):
- AccountController: Register, Login, Logout
- ExercisesController: Create, Read, Update, Delete
- Password Hashing

**Effort:** Medium (1-2 weeks)  
**Expected Benefit:** 80%+ fewer regressions, safer refactoring

**Related ADR:** (None yet)  
**Related TODO:** [[../../todo/todo-unit-test-setup]], [[../../todo/todo-integration-tests]]

---

## 📊 Risk Matrix (Severity vs Likelihood)

```
SEVERITY ↑
   High  │ RISK-001 RISK-002 │ RISK-003 RISK-004
         │ (72)    (48)      │ (40)    (42)
   Med   │ RISK-005 RISK-006 │
         │ (42)    (36)      │
   Low   │                   │
         └───────────────────┴─────── LIKELIHOOD →
           Low      Med      High
```

---

## Trade-offs & Strategic Decisions

### Trade-off 1: Monolith vs. Microservices

**Current Choice:** Monolith (ASP.NET Core MVC)

| Aspect | Monolith | Microservices |
|---|---|---|
| **Development Speed** | ✅ Fast | ❌ Slow (distributed complexity) |
| **Deployment** | ✅ Simple | ❌ Complex (kubernetes, etc.) |
| **Scalability** | ⚠️ Vertical only | ✅ Horizontal (but overkill for this size) |
| **Ops Complexity** | ✅ Simple | ❌ High |

**Decision Rationale:** Team is small, feature velocity is critical. Monolith is correct choice for now.

**Future Consideration:** If system grows to 100+ microservices or team > 20 people, consider SPA frontend + Microservices backend.

---

### Trade-off 2: Server-Side Rendering vs. SPA

**Current Choice:** Server-Side Rendering (Razor MVC)

| Aspect | SSR | SPA (React) |
|---|---|---|
| **Development Speed** | ✅ Fast | ⚠️ Medium (complex build) |
| **User Experience** | ⚠️ Full-page reloads | ✅ Instant (no reloads) |
| **Mobile Support** | ⚠️ So-so (responsive CSS helps) | ✅ Native-like UX |
| **SEO** | ✅ Native | ⚠️ Requires SSR layer |

**Decision Rationale:** Current choice is pragmatic for a growing team. SSR is acceptable for web-based workout tracking app (not real-time).

**Future Consideration:** If UX demands improve (real-time notifications, offline-first), rebuild as React SPA.

---

## Mitigation Roadmap (Prioritized)

### 🚨 Immediate (This Sprint)

1. **RISK-001:** Add rate limiter (IP-based, 100 req/min per IP)
2. **RISK-002:** Add OAuth retry with exponential backoff

### 👀 High Priority (Next 2 Weeks)

3. **RISK-003:** Enable SQL Server TDE (encryption at rest)
4. **RISK-006:** Create unit test baseline (20+ tests)

### 📋 Medium Priority (Next 4 Weeks)

5. **RISK-005:** Fix N+1 queries (code review + .Include() cleanup)
6. **RISK-004:** Evaluate HA options (AlwaysOn vs Azure SQL)

### 🔮 Future (Next Quarter)

7. Add integration tests
8. Implement distributed caching (Redis)
9. Horizontal scaling plan

---

## Related Documentation

- [[../quality_goals]] — Policy-level quality objectives
- [[../quality_scenarios]] — Test scenarios linked to risks
- [[../constraints]] — Constraints that drive risk levels
- [[../blackbox/internal/risk-register]] — Blackbox risk inventory
- [[../todo/index]] — Actionable TODOs for risk mitigation

---

## Navigation

[[index]] — Architektur-Übersicht
[[constraints]] — Constraints that shape risks
[[quality_goals]] — Quality goals & trade-offs
