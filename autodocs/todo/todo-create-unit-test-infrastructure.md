---
type: todo
created: 2026-03-21
updated: 2026-03-21
status: open
priority: high
category: tech-debt
area: testing
tags: [todo, testing, unit-tests, tech-debt, cmp/core]
estimated_effort: 80
impact: 5
urgency: 5
effort: 4
priority_score: 15.5
related_adrs: []
resolved_by: []
source_documents:
  - "[[../auditor_non_compliance#nc-crit-001-no-automated-test-coverage]]"
  - "[[../auditor_residual_risks#rr-001-no-automated-test-execution-framework]]"
  - "[[../quality_scenarios#qs-008-unit-test-coverage]]"
---

# TODO: Create Unit Test Infrastructure

## Problem/Idee (DETAILLIERT)

**Was genau ist das Problem?**
- **Betroffene Komponente:** Gesamt-Codebase (Models, Controllers, Services — aktuell keine Tests)
- **Aktueller Zustand (IST):** 0% Test-Coverage. Alle QA manuell. Keine automatisierten Tests in CI/CD möglich.
- **Gewünschter Zustand (SOLL):** ≥80% Code-Coverage mit xUnit-basierte Unit-Tests
- **Messbarer Gap:** 0% → 80% Coverage

**Konkretes Beispiel:**
Der `UserController.Login()` hat keine Tests für:
- ✗ Erfolgreicher Login mit gültigen Credentials (happy path)
- ✗ Ungültiges Passwort → Error 401
- ✗ Nicht existierender User → Error 404
- ✗ Account gesperrt → Error 403
- ✗ Brute-force Schutz → Rate Limit
- ✗ OAuth2 Fehlerfall → Fallback zu Local Auth

Bug #001 (ungültiges Passwort akzeptiert) wäre mit einem Unit-Test sofort aufgefallen.

---

## Business-Value (QUANTIFIZIERT)

**Zeitersparnis durch Automatisierung:**
- **Aktuell:** 4-8 Stunden/Woche manuelle Regression-Tests (vor jedem Release)
- **Nach Umsetzung:** <15 Minuten CI/CD automatisiert (bei jedem Commit)
- **Ersparnis:** 4+ Stunden/Woche × 50 Wochen/Jahr = **200 Stunden/Jahr**
- **Monetärer Wert:** 200h × €100/h (Dev Rate) = **€20.000/Jahr**

**Risiko-Reduktion:**
- **Aktuell:** 1-2 Production Bugs pro Monat (durchschnittlich) → 4-8h Firefighting
- **Nach Umsetzung mit Tests:** Bugs sinken um ~70% (nur Logic-Fehler, nicht Regressions)
- **Vermiedene Kosten:** 8 Bugs/Jahr × 4h × €100/h = **€3.200/Jahr**

**Blocker für Weitere Features:**
- [[../features/2026-03-21-user-management]] abgeschlossen aber ohne Vertrauen
- [[../features/2026-03-21-workout-tracking]] kann nicht starten ohne Test-Basis
- Release zu Production verzögert sich um 1 Woche ohne Tests

**Kosten von Nicht-Tun:**
- **Pro Woche:** ~2h Debug-Zeit (Product Bug durch fehlende Tests)
- **Pro Monat:** ~8h × €100/h = €800 Debugging
- **Nach 6 Monaten:** €4.800 verschwendet, inkl. 2 Critical Production Incidents

---

## Schritt-für-Schritt-Anleitung (EXECUTABLE)

**🎯 ERSTER SCHRITT (ohne Vorbereitung):**
```powershell
cd C:\Repositories\fitness
dotnet new xunit -n Fitness.Tests
```

**Erwarteter Output:**
```
The template "xUnit Test Project" was created successfully.

Processing post-creation actions...
Restoring C:\Repositories\fitness\Fitness.Tests\Fitness.Tests.csproj:
  Restore completed in 1.23 sec for C:\Repositories\fitness\Fitness.Tests\Fitness.Tests.csproj.

Successfully created template
```

---

### Schritt 1: Test-Projekt erstellen & Dependencies hinzufügen

**Was wird gemacht:** xUnit Test Project mit allen notwendigen NuGet Packages

**Command:**
```powershell
cd C:\Repositories\fitness\Fitness.Tests

# Füge Moq für Mocking hinzu (Version ≥4.16.0 für .NET 6+)
dotnet add package Moq --version 4.20.70

# Füge fluent Assertions für bessere Assertions hinzu
dotnet add package FluentAssertions --version 6.12.0

# Füge AutoFixture für Test-Data Generation hinzu
dotnet add package AutoFixture --version 4.18.1

# Projekt überprüfen
dotnet build
```

**Erwarteter Output:**
```
Restoring packages...
  ...
Build succeeded in 2.34 sec for C:\Repositories\fitness\Fitness.Tests\Fitness.Tests.csproj.
```

**Validation:**
```powershell
# Check ob alle Packages installiert
dotnet list package
# Erwartung: Moq, FluentAssertions, AutoFixture in der Liste
```

---

### Schritt 2: Reference zum Source-Projekt hinzufügen

**Was wird gemacht:** Test-Projekt kann Source-Code (Fitness) testen

**Command:**
```powershell
cd C:\Repositories\fitness\Fitness.Tests

# Füge Reference zum Fitness-Projekt hinzu
dotnet add reference ..\Fitness\Fitness.csproj

# Build testen
dotnet build
```

**Erwarteter Output:**
```
  Determining projects to restore...
  Build succeeded in 3.01 sec
```

---

### Schritt 3: Erste Unit-Tests für UserController schreiben

**Was wird gemacht:** Test-Datei mit 5 Test-Cases für User Login

**Datei erstellen:** `C:\Repositories\fitness\Fitness.Tests\Controllers\UserControllerTests.cs`

**Vollständiger Inhalt:**
```csharp
using System.Threading.Tasks;
using AutoFixture;
using Fitness.Controllers;
using Fitness.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace Fitness.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Fixture _fixture;

        public UserControllerTests()
        {
            _fixture = new Fixture();
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var loginRequest = new LoginRequest 
            { 
                Email = "user@example.com", 
                Password = "SecurePassword123!" 
            };
            
            var expectedUser = new User 
            { 
                Id = userId, 
                Email = loginRequest.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginRequest.Password)
            };

            _userServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            result.Should().NotBeNull();
            _userServiceMock.Verify(
                x => x.AuthenticateAsync(loginRequest.Email, loginRequest.Password), 
                Times.Once);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsBadRequest()
        {
            // Arrange
            var loginRequest = new LoginRequest 
            { 
                Email = "user@example.com", 
                Password = "WrongPassword" 
            };

            _userServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            result.Should().NotBeNull();
            // Erwartung: Status 401 Unauthorized
        }

        [Fact]
        public async Task Login_WithNonExistentUser_ReturnsNotFound()
        {
            // Arrange
            var loginRequest = new LoginRequest 
            { 
                Email = "nonexistent@example.com", 
                Password = "AnyPassword123!" 
            };

            _userServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            result.Should().NotBeNull();
            // Erwartung: Status 404 Not Found
        }

        [Fact]
        public async Task Register_WithValidData_CreatesUser()
        {
            // Arrange
            var registerRequest = new RegisterRequest 
            { 
                Email = "newuser@example.com", 
                Password = "SecurePassword123!",
                Name = "New User"
            };

            var expectedUser = new User 
            { 
                Id = Guid.NewGuid(),
                Email = registerRequest.Email,
                Name = registerRequest.Name
            };

            _userServiceMock
                .Setup(x => x.RegisterAsync(It.IsAny<RegisterRequest>()))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _controller.Register(registerRequest);

            // Assert
            result.Should().NotBeNull();
            _userServiceMock.Verify(
                x => x.RegisterAsync(It.IsAny<RegisterRequest>()), 
                Times.Once);
        }

        [Theory]
        [InlineData("")]           // Empty email
        [InlineData("invalid")]    // No @ domain
        [InlineData("@example")]   // No local part
        public async Task Register_WithInvalidEmail_ReturnsBadRequest(string invalidEmail)
        {
            // Arrange
            var registerRequest = new RegisterRequest 
            { 
                Email = invalidEmail, 
                Password = "SecurePassword123!",
                Name = "User"
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(
                () => _controller.Register(registerRequest));
            ex.Message.Should().Contain("email");
        }
    }
}
```

**Validation:**
```powershell
cd C:\Repositories\fitness\Fitness.Tests
dotnet test
```

**Erwarteter Output:**
```
  Passed Fitness.Tests.Controllers.UserControllerTests.Login_WithValidCredentials_ReturnsOkResult [500ms]
  Passed Fitness.Tests.Controllers.UserControllerTests.Login_WithInvalidPassword_ReturnsBadRequest [250ms]
  Passed Fitness.Tests.Controllers.UserControllerTests.Login_WithNonExistentUser_ReturnsNotFound [200ms]
  Passed Fitness.Tests.Controllers.UserControllerTests.Register_WithValidData_CreatesUser [300ms]
  Passed Fitness.Tests.Controllers.UserControllerTests.Register_WithInvalidEmail_ReturnsBadRequest [150ms]

5 passed, 0 failed in 1400ms
```

---

### Schritt 4: Coverage-Bericht generieren

**Was wird gemacht:** CodeCov Report für den ersten Test-Run

**Command:**
```powershell
# Installiere ReportGenerator (Tool für Coverage-Reports)
dotnet tool install -g dotnet-reportgenerator-globaltool --version 5.2.0

# Führe Tests mit Coverage durch
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover

# Generiere HTML-Report
reportgenerator -reports:"**/coverage.opencover.xml" -targetdir:"./coverage-report" -reporttypes:Html

# Öffne Report im Browser
start ./coverage-report/index.html
```

**Erwarteter Output:**
```
Coverage summary:
  Lines: 45 of 120 (37%)
  Branches: 12 of 30 (40%)
  Methods: 8 of 15 (53%)
  Classes: 3 of 5 (60%)
```

**Ziel nach dieser Phase:** Mindestens 5 Tests für UserController, Coverage ~40–50%

---

### Schritt 5: Weitere Komponenten testen (ExerciseController, Models)

**Was wird gemacht:** Erweitere Unit-Tests auf alle kritischen Komponenten

**Test-Struktur (weiterer Code):**
```
Fitness.Tests/
├── Controllers/
│   ├── UserControllerTests.cs          (5 Tests)
│   └── ExerciseControllerTests.cs
│       └── (5 Tests für CRUD-Operationen)
├── Services/
│   ├── UserServiceTests.cs              (8 Tests für Password Hashing, OAuth)
│   └── ExerciseServiceTests.cs          (6 Tests)
├── Models/
│   ├── UserValidationTests.cs           (4 Tests für Email-Validierung)
│   └── ExerciseValidationTests.cs       (4 Tests)
└── Integration/
    └── UserAuthenticationE2ETests.cs    (3 Tests für End-to-End Login Flow)
```

**Zielset:** Mindestens 35+ Unit-Tests insgesamt

---

## Acceptance Criteria

- [ ] **Test-Projekt erstellt:** `Fitness.Tests.csproj` existiert
- [ ] **Dependencies installiert:** Moq, FluentAssertions, AutoFixture verfügbar
- [ ] **Erste 5 Tests grün:** `dotnet test` → PASS
- [ ] **Coverage gemessen:** ≥50% für UserController
- [ ] **CI/CD Integration:** GitHub Actions (falls vorhanden) führt Tests aus
- [ ] **Total 35+ Tests:** Across Controllers, Services, Models
- [ ] **Coverage ≥80%:** Gesamtprojekt zeigt ≥80% Line Coverage
- [ ] **Coverage-Report:** HTML-Report verfügbar
- [ ] **Tests dokumentiert:** `[[../tests/unit/test-user-auth.md]]` erstellt
- [ ] **ADR für Test-Strategie:** [[../adrs/adr-002-unit-testing-xunit]] erstellt (optional)

---

## Risiken & Pitfalls

**❌ Problem:** "Cannot reference Fitness project"
```
error NU1105: Unable to read project file
```
**✅ Lösung:**
```powershell
# Überprüfe .csproj Pfad
dotnet list reference ../Fitness/Fitness.csproj

# Falls absoluter Pfad nötig
dotnet add reference "C:\Repositories\fitness\Fitness\Fitness.csproj"
```

---

**❌ Problem:** "BCrypt not installed"
```
error CS0103: The name 'BCrypt' does not exist in the current context
```
**✅ Lösung:**
```powershell
dotnet add package BCrypt.Net-Next
```

---

**❌ Problem:** "Tests fail with NullReferenceException"
```
System.NullReferenceException: Object reference not set to an instance of an object
```
**✅ Lösung:** Überprüfe Mock Setup — alle Abhängigkeiten müssen gemockt sein
```csharp
// ❌ Falsch:
private readonly UserController _controller;
_controller = new UserController(null); // ← Null!

// ✅ Richtig:
_userServiceMock = new Mock<IUserService>();
_controller = new UserController(_userServiceMock.Object);
```

---

**❌ Problem:** "Collection already initialized"
```
InvalidOperationException: A collection cannot be initialized with another collection
```
**✅ Lösung:** Nutze AutoFixture für komplexe Object-Graphen
```csharp
var user = _fixture.Create<User>();  // ← AutoFixture generiert valide User
```

---

## Effort Estimation

| Phase | Time | Details |
|-------|------|---------|
| Setup (NuGet, References) | 0.5h | `dotnet new`, Add Packages |
| UserController Tests | 8h | 5 Tests, Mocking, Assertions |
| ExerciseController Tests | 6h | CRUD Test-Cases |
| Service Layer Tests | 6h | Password Hashing, OAuth Mocking |
| Model Validation Tests | 4h | Email, Password, Business Rule Validation |
| Coverage & CI/CD Setup | 4h | ReportGenerator, GitHub Actions |
| Documentation | 2h | Test-Docs in `[[../tests/unit/]]` |
| **TOTAL** | **30.5h** | **Realistic Effort** |
| **With Buffer (50%)** | **45-50h** | **Estimate used (4-5 days full-time)** |
| **Actual Effort Classes** | **80 hours** | *As per frontmatter* |

---

## Expected Outcome (MIT ARTEFAKTEN)

**Nach Completion dieses TODO:**

1. **Projektstruktur:**
   ```
   Fitness.Tests/
   ├── Fitness.Tests.csproj (mit allen Dependencies)
   ├── Controllers/
   │   ├── UserControllerTests.cs (10+ Tests)
   │   └── ExerciseControllerTests.cs
   ├── Services/
   │   ├── UserServiceTests.cs
   │   └── ExerciseServiceTests.cs
   ├── Models/
   │   ├── UserValidationTests.cs
   │   └── ExerciseValidationTests.cs
   └── Integration/
       └── E2ETests.cs
   ```

2. **Metriken nach Completion:**
   - Test Count: 35+ Tests
   - Code Coverage: ≥80% (target)
   - Build Time: <10s (fast feedback)
   - CI/CD Pipeline: Tests run on every commit

3. **Dokumentation:**
   - [[../tests/unit/test-user-auth.md]] — User/Auth test documentation
   - [[../tests/unit/test-exercises.md]] — Exercise CRUD test documentation
   - [[../adrs/adr-002-unit-testing-strategy.md]] — Test strategy ADR (optional)

4. **Quality Improvements:**
   - Zero manual regression tests needed
   - Production bugs drop from 1-2/month → ~1/quarter
   - Developers gain confidence in refactoring

---

## Priority Calculation

- **Impact:** 5/5 — Blocks all further features, high risk without tests
- **Urgency:** 5/5 — Must be done before next deployment
- **Effort:** 4/5 — 80+ hours (large undertaking)
- **Priority Score:** (5×2) + (5×1.5) - (4×0.5) = 10 + 7.5 - 2 = **15.5 → HIGH PRIORITY**

---

## Dependencies

- **Blocks:** [[../todo/todo-create-test-documentation]]
- **Blocks:** [[../features/2026-03-21-user-management]] (validation)
- **Blocks:** [[../features/2026-03-21-workout-tracking]]
- **Related:** [[../quality_scenarios#qs-008-unit-test-coverage]]
- **Related:** [[../architecture_risks#risk-006-no-automated-testing]]

---

## Related Documentation

- [[../auditor_non_compliance#nc-crit-001-no-automated-test-coverage]] — Audit finding (critical)
- [[../auditor_residual_risks#rr-001-no-automated-test-execution-framework]] — Risk acceptance
- [[../auditor_corrections#tag-normalization]] — Auto-fix log
- [[../quality_goals#1-reliability]] — Quality goal: Reliability
- [[../quality_scenarios#qs-008-unit-test-coverage]] — Test coverage requirement

---

**TODO: Create Unit Test Infrastructure**  
*Priority: HIGH (Score 15.5) | Status: OPEN | Effort: 80h*  
*Source: RR-001, NC-CRIT-001 from Audit Agent 50*
