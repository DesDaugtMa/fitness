---
title: "Constraints"
date: 2026-03-21
type: architecture
view_type: constraints
status: accepted
tags: [architecture, constraints, boundaries, isaqb]
related_docs:
  - "[[../quality_goals]]"
  - "[[../deployment_view]]"
  - "[[../architecture_risks]]"
  - "[[../adrs/index]]"
source_files:
  - Source/Fitness/Fitness.csproj
  - Source/Fitness/Program.cs
  - Source/Fitness/Config/AppSettings.cs
---

# Constraints (Randbedingungen)

## Einführung

Constraints sind **unveränderliche oder schwer änderbare Grenzen**, die die Architektur-Entscheidungen einschränken oder dirigieren. Sie stammen aus Technologie, Organisation, Geschäft oder Compliance.

---

## 🏗️ Technische Constraints

### TC-001: .NET 10 / ASP.NET Core Monolith

**Kategorie:** Technology Stack  
**Verhandelbar:** ⚠️ Mittelfristig (Rewrite wäre major effort)  
**Status:** Fixed (seit Project-Start)

**Beschreibung:**  
Das System ist in C# mit ASP.NET Core MVC 10 gebaut. Dies schränkt die Wahl anderer Sprachen ein.

**Implikationen:**
- ✅ Productive: Schnelle Entwicklung in C#
- ⚠️ Limited Ecosystem: Nur .NET-kompatible Libraries
- ⚠️ Deployment Windows: Muss .NET 10 Runtime haben

**Related ADRs:**
- [[../adrs/adr-001-documentation-system]] (Meta, aber relevant)

---

### TC-002: SQL Server als Primary Datastore

**Kategorie:** Database Technology  
**Verhandelbar:** ❌ Nein (Massive Migration Cost)  
**Status:** Fixed (seit Project-Start)

**Beschreibung:**  
Einzig Datenbank ist SQL Server. EF Core ist das ORM. Keine NoSQL, keine Polyglot Persistence.

**Implikationen:**
- ✅ Mature: SQL Server ist stabil, ACID-konform
- ⚠️ Scaling: Vertical scaling (bigger hardware) einfacher als horizontal (sharding)
- ⚠️ Licensing: SQL Server listet kosten; Open Source Alternative = PostgreSQL (aber migration effort)

**Related ADRs:**
- [[../adrs/adr-001-use-entity-framework]]

**Workaround:** Migrate to PostgreSQL + EF Core (supported, aber 2-4 Wochen effort)

---

### TC-003: Entity Framework Core (ORM)

**Kategorie:** Data Access Framework  
**Verhandelbar:** ⚠️ Langfristig (Rewrite data layer)  
**Status:** Fixed (seit Project-Start)

**Beschreibung:**  
Datenbank-Zugriff läuft über EF Core, nicht Raw SQL / Dapper.

**Implikationen:**
- ✅ Rapid Development: Migrations, DbContext abstraction
- ⚠️ Performance: ORM overhead; N+1 queries if not careful (see [[../quality_scenarios#qs-006]])
- ⚠️ Flexibility: Some complex queries need LINQ or raw SQL fallback

**Related ADRs:**
- [[../adrs/adr-001-use-entity-framework]]

---

### TC-004: MVC Rendering (Server-Side)

**Kategorie:** UI Architecture  
**Verhandelbar:** ⚠️ Langfristig (rebuild as SPA)  
**Status:** Fixed (seit Project-Start)

**Beschreibung:**  
User Interface wird mit Razor Templates (.cshtml) server-seitig gerendert. **Kein** React/Vue/Angular Frontend.

**Implikationen:**
- ✅ Simpler: No JavaScript build pipeline, no API versioning
- ⚠️ Interactivity: Limited (full-page reload needed for updates)
- ⚠️ Mobile-Unfriendly: Responsive CSS can help, but not optimal for mobile-first

**Related ADRs:**
- None yet (but future ADR if rebuilding as SPA)

---

## 👥 Organisatorische Constraints

### OC-001: Small Team (< 10 Developers)

**Kategorie:** Team Size & Expertise  
**Verhandelbar:** Nein (reality constraint)  
**Status:** Current Context

**Beschreibung:**  
Projekt wird von < 10 C# Entwicklern betrieben. Keine separate DevOps, Security, Database teams.

**Implikationen:**
- ⚠️ Knowledge Bus Factor: If 1 person knows OAuth setup, system has risk
- ⚠️ Limited Automation: Tests, CI/CD may not be fully automated
- ✅ Fast Decision-Making: No bureaucracy, can pivot quickly

**Mitigation:**
- Good documentation (this project!)
- Pair programming on critical components
- Runbooks for operations tasks

---

### OC-002: Limited DevOps / Infrastructure Budget

**Kategorie:** Infrastructure & Operations  
**Verhandelbar:** Schwer (Business decision)  
**Status:** Current Context

**Beschreibung:**  
Keine großen Cloud-Infrastruktur-Investitionen. Deployment vermutlich on-prem oder minimal-Cloud (single VM).

**Implikationen:**
- ⚠️ No Auto-Scaling: Manual scaling effort
- ⚠️ No Managed Services: SQL Server self-hosted, not Azure SQL
- ⚠️ Limited Monitoring: No Datadog, New Relic; maybe just Windows Event Logs

**Mitigation:**
- Use free/open-source tools: ELK Stack, Prometheus
- Automated backups (scripts, not fancy services)
- Health checks & restart policies (powershell scripts)

---

## 📋 Compliance & Legal Constraints

### CC-001: GDPR (User Data Privacy)

**Kategorie:** Regulatory  
**Verhandelbar:** ❌ Nein (Legal Requirement)  
**Status:** In Scope (for any EU users)

**Beschreibung:**  
Wenn App auf EU-Benutzer zugreift, müssen GDPR-Anforderungen erfüllt sein:
- Right to be forgotten
- Data subject access requests
- Data processing agreements

**Implikationen:**
- ⚠️ User Data: Must be encrypted at rest (GAP-003)
- ⚠️ Audit Logs: Track modifications (who, when, what)
- ⚠️ Consent: Explicit opt-in for non-essential cookies
- ⚠️ Data Residency: May need EU-hosted database

**Related Risks:**
- [[../architecture_risks#risk-003-no-at-rest-encryption]]

**Mitigation:**
- Implement audit logging
- Add data deletion feature
- Privacy policy documentation
- GDPR Audit (external consultant)

---

### CC-002: Google OAuth2 Third-Party

**Kategorie:** Dependency  
**Verhandelbar:** ⚠️ Mittelfristig (switch to other provider)  
**Status:** Current Dependency

**Beschreibung:**  
System hängt von Google OAuth2 API ab. Keine Fallback-Option wenn Google down ist.

**Implikationen:**
- ⚠️ Availability: If Google is down, social login is down
- ⚠️ Terms of Service: Google kann API ändern, Terms ändern
- ⚠️ Rate Limits: Daily/monthly quota auf API calls (not yet enforced in code)

**Related Risks:**
- [[../architecture_risks#risk-002-oauth-retry-missing]]

**Mitigation:**
- Keep local authentication as primary option
- Add retry + backoff for OAuth (see GAP-002)
- Monitor Google API status

---

## 🔒 Security Constraints

### SC-001: Password Security Standards

**Kategorie:** Authentication Security  
**Verhandelbar:** ❌ Nein (Industry Standard)  
**Status:** Implemented

**Beschreibung:**  
Passwörter müssen mit NIST-konformen Algorithmen gehashed werden:
- PBKDF2 (recommended) ✅
- bcrypt ✅
- Argon2 ✅

**Implikationen:**
- ✅ ASP.NET Core IPasswordHasher<T> uses PBKDF2 (2x iteration default, not 600k)
- ⚠️ Need to increase iteration count for resistance against GPU attacks

**Current Implementation:** ⚠️ PBKDF2 with lower iteration count (verify in code)

---

### SC-002: HTTPS Required for Client Communication

**Kategorie:** Transport Security  
**Verhandelbar:** ❌ Nein (Best Practice)  
**Status:** Not yet enforced

**Beschreibung:**  
Alle public endpoints müssen HTTPS sein (encrypt in-transit).

**Implikations:**
- Certificate management (Let's Encrypt, wildcard cert)
- HSTS headers (force HTTPS, block HTTP)
- TLS version >= 1.2

**Current Implementation:** ⚠️ Recommend enforcing

**Mitigation:**
- Add `app.UseHttpsRedirection()` in Program.cs
- Add HSTS header middleware

---

## 🌍 External Constraints

### EC-001: Browser Compatibility

**Kategorie:** Client Platform  
**Verhandelbar:** ⚠️ Mittelfristig (React SPA = better mobile)  
**Status:** Current (Razor MVC)

**Beschreibung:**  
Muss funktionieren auf:
- Chrome (latest)
- Edge (latest)
- Firefox (latest)
- iOS Safari (latest)
- Android Chrome (latest)

**Implikations:**
- No IE11 support needed ✅
- CSS Grid / Flexbox OK ✅
- ES6+ JavaScript OK (with transpilation if needed)

---

### EC-002: Network Latency (User Location)

**Kategorie:** Network / Performance  
**Verhandelbar:** Nein (Reality constraint)  
**Status:** Current

**Beschreibung:**  
Benutzer können global verteilt sein → varying latency:
- EU: ~20-50ms
- US: ~80-120ms
- Asia: ~150-300ms

**Implikations:**
- ⚠️ P95 latency must account for network (not just server time)
- ⚠️ Large payloads are slow (minimize JSON responses)
- ✅ Server-side rendering reduces JS payload (bonus for slow networks)

---

## 📊 Constraint Matrix

| Constraint | Category | Type | Impact | Mitigation |
|---|---|---|---|---|
| **TC-001** | Tech Stack | Fixed | High | N/A (accepted) |
| **TC-002** | Database | Fixed | High | PostgreSQL migration (if needed) |
| **TC-003** | ORM | Fixed | Medium | Careful query design, monitoring |
| **TC-004** | UI Architecture | Fixed | Medium | Rebuild as SPA (future) |
| **OC-001** | Team Size | Reality | Medium | Good documentation, knowledge sharing |
| **OC-002** | DevOps Budget | Reality | Medium | Automation, open-source tools |
| **CC-001** | GDPR | Legal | High | Audit, encryption, deletion features |
| **CC-002** | Google Auth | Dependency | Medium | Local auth fallback, retry logic |
| **SC-001** | Password Sec | Standard | High | Verify PBKDF2 config |
| **SC-002** | HTTPS | Best Practice | High | HSTS + redirect middleware |
| **EC-001** | Browser Compat | Reality | Low | Modern stack (no IE11) |
| **EC-002** | Network Latency | Reality | Medium | Minimize payload, optimize server |

---

## Related Documentation

- [[../quality_goals]] — Quality Goals influenced by constraints
- [[../architecture_risks]] — Risks stemming from constraints
- [[../deployment_view]] — Infrastructure constraints
- [[../adrs/index]] — Technology decisions

---

## Navigation

[[index]] — Architektur-Übersicht
[[architecture_risks]] — Risks & Trade-offs
[[deployment_view]] — Infrastructure
