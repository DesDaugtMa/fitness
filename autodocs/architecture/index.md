---
type: index
created: 2025-11-13
updated: 2026-03-21
tags: [index, architecture]
---

# 🏗️ Architecture Documentation (iSAQB-Compliant)

**Complete software architecture documentation for the Fitness WebApp** — 9 comprehensive views following iSAQB standards.

---

## 📌 Quick Start

**New to this architecture?** Start here:

1. [[context_view]] — "What is this system?" (System context, users, dependencies)
2. [[building_block_view]] — "How is it structured?" (Components, layers, code mapping)
3. [[runtime_view]] — "How does it behave?" (User journeys, sequence diagrams)
4. [[quality_goals]] — "What matters?" (Top 5 quality goals, trade-offs)

**For more details:** [[#-architecture-views]] below.

---

## 🎯 Core Facts

| Aspect | Details |
|--------|---------|
| **System Type** | Monolithic ASP.NET Core MVC Web App |
| **Stack** | .NET 10, C#, SQL Server, Google OAuth2 |
| **Layers** | Presentation (Controllers) → Domain → Data Access (EF Core) |
| **Users** | Fitness enthusiasts managing workouts |
| **Team Size** | Small (< 10 developers) |
| **Deployment** | Single cloud VM or on-prem |

---

## 🔄 Architecture Views (Complete)

### ✅ 1. Context View
**[[context_view]]** — System context and external dependencies

- C4 Level 1 diagram (system in environment)
- User roles: Fitness users
- External systems: SQL Server, Google OAuth2
- Quality concerns from context perspective

### ✅ 2. Building Block View (C4 L2/L3)
**[[building_block_view]]** — Internal structure and components

- 7 Controllers: AccountController, ExercisesController, HomeController, AdminController, etc.
- 6 Domain Models: User, Exercise, Workout, MuscleGroup, Image, RegistrationToken
- Entity Framework Core data access layer
- Layered monolith architecture (3-tier)
- Code-to-component mapping

### ✅ 3. Runtime View
**[[runtime_view]]** — Important scenarios and behavior at runtime

- 5 detailed user journey scenarios with sequence diagrams:
  1. User Registration & Local Authentication
  2. Google OAuth2 Login Flow
  3. Exercise CRUD Operations
  4. Error Handling (Unauthorized/Forbidden)
  5. Workout Session Tracking
- Performance and quality attributes per scenario

### ✅ 4. Deployment View
**[[deployment_view]]** — Infrastructure, deployment, and operations

- Application tier (load balancer, N WebApp instances)
- Database tier (SQL Server, backup strategy)
- Network topology (HTTPS, internal VLANs)
- Environment configuration (Dev, Staging, Prod)
- Storage and disaster recovery strategy
- Monitoring recommendations

### ✅ 5. Quality Goals
**[[quality_goals]]** — Key quality objectives (prioritized)

**Top 5 Goals:**
1. **Security** — Protect user credentials & PII (🔴 Critical)
2. **Availability** — System accessible 24/7 (🟠 High)
3. **Performance** — Fast responses < 1s (🟠 High)
4. **Maintainability** — Code is readable & changeable (🟡 Medium)
5. **Testability** — Easy to write unit & integration tests (🟡 Medium)

- Stakeholder perspectives
- Trade-offs and strategic decisions

### ✅ 6. Quality Scenarios
**[[quality_scenarios]]** — Testable acceptance criteria for quality goals

**8 Scenarios:**
- QS-001: Password hashing (PBKDF2) ✅ Implemented
- QS-002: OAuth2 token validation ⚠️ Partial (retry missing)
- QS-003: Anti-CSRF protection ✅ Implemented
- QS-005: Login performance P95 < 500ms ❓ Unknown
- QS-006: N+1 query prevention ⚠️ Needs review
- QS-004: Database failover (HA) ❌ Missing
- QS-008: Unit test coverage ≥ 80% ❌ No tests
- QS-009: Integration test setup ⚠️ Unclear

### ✅ 7. Constraints
**[[constraints]]** — Non-negotiable boundaries shaping architecture

**12 Key Constraints:**
- Technical: .NET 10, SQL Server, EF Core, Server-side Razor
- Organizational: Small team, limited DevOps budget
- Legal: GDPR compliance (if EU users)
- External: Google OAuth2, browser compatibility, network latency

### ✅ 8. Architecture Risks
**[[architecture_risks]]** — Known risks and mitigation strategies

**6 Risks (prioritized by Severity × Likelihood):**

| Risk | Severity | Status | Mitigation |
|------|----------|--------|-----------|
| **ARCH-001** No rate limiter (DoS) | 🔴 Critical | 🚨 Open | Add IP-based rate limiting (1-2 days) |
| **ARCH-002** No OAuth retry policy | 🔴 Critical | 🚨 Open | Add Polly retry with backoff (1 day) |
| **ARCH-003** No at-rest encryption | 🔴 High | 🔴 Open | Enable TDE or app-level encryption (2h-2d) |
| **ARCH-004** No HA (single DB) | 🟠 High | 🔴 Open | SQL Server AlwaysOn or Azure SQL (3-5d) |
| **ARCH-005** N+1 queries | 🟡 Medium | ⚠️ Code review | Use .Include(), disable lazy-loading (2d) |
| **ARCH-006** No automated tests | 🟡 Medium | ❌ Missing | Create test project + 20+ tests (1-2w) |

See [[architecture_risks]] for detailed mitigation roadmap.

### ✅ 9. Architecture Mapping
**[[architecture_mapping]]** — Code ↔ Docs cross-reference

- Maps source code paths to building blocks
- Links controllers to features and tests
- Domain models with database schema
- Feature → Test bidirectional mapping
- Used by updater agent for synchronization

---

## 📊 Quality Goals Summary

```
🔴 CRITICAL  ──┐
               │ ARCH-001: No rate limiter
               │ ARCH-002: No OAuth retry
🔴 HIGH      ──┼─ ARCH-003: No encryption (GDPR)
               │ ARCH-004: No HA
🟡 MEDIUM    ──┤ ARCH-005: N+1 queries
               │ ARCH-006: No tests
🟢 LOW       ──┘
```

**Status:** 5 critical/high issues identified, prioritized mitigation roadmap created

---

## 🔗 Related Documentation

### Features & Requirements
- [[../features/2026-03-21-user-management]] — User registration, authentication, OAuth2
- [[../features/2026-03-21-workout-tracking]] — Exercise and workout management

### Architecture Decisions
- [[../adrs/adr-001-use-entity-framework]] — Why Entity Framework Core?
- [[../adrs/adr-001-documentation-system]] — Why this documentation system?

### Testing
- [[../tests/index]] — Test documentation (⚠️ Currently **NO TESTS**)
  - TODO: Create test-auth, test-exercises, test-workouts projects

### Backlog & Questions
- [[../todo/index]] — Actionable tasks from architecture analysis
- [[../questions/index]] — Open design questions

### External Interfaces
- [[../blackbox/index]] — External APIs and dependencies
- [[../blackbox/public/fitness-webapp-api]] — REST/MVC endpoints
- [[../blackbox/internal/risk-register]] — Blackbox-level risks

---

## 📈 Architecture Maturity Assessment

| Dimension | Maturity | Score | Evidence |
|-----------|----------|-------|----------|
| **Context Understanding** | Excellent | ⭐⭐⭐⭐ | Complete C4 context + blackbox docs |
| **Building Block Clarity** | Excellent | ⭐⭐⭐⭐ | 7 controllers, 6 models, clear layers |
| **Runtime Behavior** | Excellent | ⭐⭐⭐⭐ | 5 detailed sequence diagrams |
| **Quality Goals** | Excellent | ⭐⭐⭐⭐ | 5 prioritized goals with metrics |
| **Decision Documentation** | Poor | ⭐ | Only 2 ADRs; many decisions undocumented |
| **Risk Management** | Good | ⭐⭐⭐ | 6 risks identified + mitigation roadmap |
| **Testing & Validation** | Critical Gap | ⭐ | **NO automated tests** (all manual) |
| **Implementation Docs** | Good | ⭐⭐⭐ | Code mapping exists, some gaps |

**Overall: ⭐⭐⭐ Good foundational architecture, critical gaps in testing**

---

## 🚀 Immediate Action Items (Priority)

🚨 **This Sprint:**
- [ ] Add rate limiter middleware (ARCH-001)
- [ ] Add OAuth return policy with backoff (ARCH-002)

👀 **Next 2 Weeks:**
- [ ] Enable SQL Server TDE encryption at rest (ARCH-003)
- [ ] Create unit test baseline (20+ tests) (ARCH-006)

📋 **Next 4 Weeks:**
- [ ] Fix N+1 queries with code review (ARCH-005)
- [ ] Evaluate SQL Server HA options (ARCH-004)

See [[architecture_risks#mitigation-roadmap-prioritized]] for detailed timeline & effort estimates.

---

## 📚 How to Use This Documentation

### For Developers
1. Start with [[context_view]] to understand the big picture
2. Review [[building_block_view]] to find component responsibilities
3. Check [[architecture_mapping]] before adding/modifying code
4. Read [[quality_goals]] & [[quality_scenarios]] before writing tests

### For Architects
1. Study [[constraints]] for decision context
2. Review [[architecture_risks]] for known issues
3. Use [[architecture_mapping]] for impact analysis
4. Propose new ADRs using [[../adrs/_meta]] template

### For Operations
1. Reference [[deployment_view]] for infrastructure setup
2. Check [[architecture_risks]] for incident handling
3. Review [[quality_scenarios]] for monitoring baselines

### For Security & Compliance
1. Focus on [[quality_goals#1-security]] and security scenarios
2. Track [[architecture_risks#risk-001-risk-003]] mitigation
3. Plan [[constraints#cc-001-gdpr]] compliance roadmap

---

## 🔗 Navigation

**← [[../index]]** — Back to main autodocs

**Sibling Sections:**
- [[../features/index]] — Feature documentation
- [[../adrs/index]] — Architecture Decision Records
- [[../tests/index]] — Test documentation
- [[../todo/index]] — TODO backlog
- [[../blackbox/index]] — Blackbox (external interfaces)
- [[../questions/index]] — Open design questions

---

**Generated by AutoDocs 40 · Architect Agent** — iSAQB-compliant, production-ready.

*Last Updated: 2026-03-21 · Version: 1.0*
