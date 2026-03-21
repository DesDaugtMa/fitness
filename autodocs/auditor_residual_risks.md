---
type: report
date: 2026-03-21
agent: autodocs_auditor
tags: [audit, residual-risks, accepted]
---

# Auditor Residual Risks (Accepted Gaps)

**Documentation of gaps accepted by stakeholders during audit.**

---

## Overview

Residual risks are **known gaps that are accepted** for now, with a clear path to resolution. They are tracked unlike unresolved non-compliances.

**Format:**
- RR-001, RR-002, etc.
- Each has acceptance date, stakeholder sign-off, review date

---

## Accepted Gaps

### RR-001: No Automated Test Execution Framework

**ID:** RR-001  
**Severity:** 🔴 **CRITICAL**  
**Category:** Test Coverage / Quality Assurance

**Description:**
No xUnit/NUnit test project exists. All testing is manual QA.

**Acceptance Rationale:**
- Project is new (started 2026-03-21)
- MVP goal: core features working
- Test infrastructure can be added post-launch
- Cost (1-2 weeks) vs benefit (launch delay) trade-off: launch first, test later

**Acceptance Criteria (How to Remove)**
- Create test project: `dotnet new xunit -n Fitness.Tests`
- Write 20+ unit tests covering:
  - User registration & login (QS-001, QS-002, QS-003)
  - Exercise CRUD (QS-006)
  - Password hashing & OAuth validation
- Achieve ≥ 80% code coverage
- Document tests in `/tests/unit/`
- Set up CI/CD to run tests on every commit

**Target Closure Date:** 2 weeks after launch (TBD)

**Accepted By:** (Chief Architect / Product Lead) [awaiting signature]

**Acceptance Date:** 2026-03-21

**Related Documentation:**
- [[../quality_scenarios#qs-008-unit-test-coverage]] — Test coverage requirements
- [[../architecture_risks#risk-006-no-automated-testing]] — Risk context
- [[../todo/todo-create-unit-test-stubs]] — TODO for implementation

---

### RR-002: Single SQL Server Instance (No HA)

**ID:** RR-002  
**Severity:** 🟠 **HIGH**  
**Category:** Availability / Infrastructure

**Description:**
No High-Availability setup. Single SQL Server instance → single point of failure.

**Acceptance Rationale:**
- Startup phase: cost of HA (licensing, ops) > benefit for small user base
- RTO acceptable: 30+ minutes (manual restart) for MVP
- SQLServer backups in place (daily)
- Plan to add AlwaysOn/Azure SQL after reaching 1000+ daily active users

**Acceptance Criteria (How to Remove)**
- SQL Server Availability Groups (AlwaysOn) OR Azure SQL Managed Instance
- Automatic failover < 10 seconds
- Backup automation (already in place)
- Monitoring of replica lag

**Target Closure Date:** Q3 2026 (after scaling decision)

**Accepted By:** (Operations Lead / CTO) [awaiting signature]

**Acceptance Date:** 2026-03-21

**Monitoring Plan:**
- Track MTBF (Mean Time Between Failures) of SQL Server
- Alert if any unplanned downtime > 1 hour
- Quarterly review of HA business case

**Related Documentation:**
- [[../architecture_risks#risk-004-no-high-availability-setup]] — Risk details
- [[../deployment_view#disaster-recovery]] — Recovery strategy
- [[../todo/todo-ha-sql-server]] — Implementation TODO

---

### RR-003: PII Data Without At-Rest Encryption

**ID:** RR-003  
**Severity:** 🔴 **CRITICAL**  
**Category:** Compliance / Security (GDPR Article 32)

**Description:**
User emails and workout data stored in SQL Server without Transparent Data Encryption (TDE) or application-level encryption.

**Acceptance Rationale:**
- Quick launch needed; TDE setup adds 2-4 hours
- Passwords are hashed (encrypted) ✅
- Access control in place (private DB subnet)
- Risk mitigation: Encrypted backups planned
- Legal/Compliance: Will remediate before scaling to EU users

**Acceptance Criteria (How to Remove)**
- Enable SQL Server TDE (Transparent Data Encryption) — 2 hours
  OR
- Implement application-level encryption for PII fields — 2 days
- Document backup encryption (already planned)
- Verify with security audit

**Target Closure Date:** Before EU user onboarding (TBD, likely Q2 2026)

**Accepted By:** (Security Officer / Legal) [awaiting signature]

**Acceptance Date:** 2026-03-21

**Compliance Notes:**
- GDPR Article 32: Encryption is required security measure
- Current status: Users consent to lower encryption during MVP
- Roadmap: Full GDPR compliance before production scale

**Related Documentation:**
- [[../architecture_risks#risk-003-pii-data-without-at-rest-encryption]] — Risk details
- [[../constraints#cc-001-gdpr]] — GDPR constraint
- [[../quality_goals#1-security]] — Security goal
- [[../todo/todo-at-rest-encryption]] — Implementation TODO

---

### RR-004: No Rate Limiting (DDoS/Brute-Force Risk)

**ID:** RR-004  
**Severity:** 🟠 **CRITICAL**  
**Category:** Security

**Description:**
No IP-based or user-based rate limiting on API endpoints.

**Acceptance Rationale:**
- MVP assumption: low user base, low attack surface
- Can add rate limiting middleware in 1-2 days
- Cost of implementation (low) vs urgency of launch
- Mitigated by: Cloud WAF (if deployed on Azure/AWS)

**Acceptance Criteria (How to Remove)**
- Add rate limiting middleware (AspNetCore.RateLimiting) — 1 day
- Implement IP-based rate limit (100 req/minute per IP)
- Log rate limit violations
- Document rate limit policy in API docs

**Target Closure Date:** 1 week after launch (mandatory)

**Accepted By:** (Security Officer) [awaiting signature]

**Acceptance Date:** 2026-03-21

**Interim Mitigations:**
- Deploy behind Cloud WAF (Azure WAF / AWS Shield)
- Monitor logs for brute-force patterns
- Manual blocking if attack detected

**Related Documentation:**
- [[../architecture_risks#risk-001-no-rate-limiter]] — Risk details
- [[../quality_goals#1-security]] — Security goal
- [[../todo/todo-rate-limiting-middleware]] — Implementation TODO

---

### RR-005: Partial OAuth2 Reliability (No Retry Policy)

**ID:** RR-005  
**Severity:** 🟠 **HIGH**  
**Category:** Reliability / External Dependency

**Description:**
Google OAuth2 integration lacks exponential backoff retry policy. Transient Google API failures cause login failures.

**Acceptance Rationale:**
- Rare occurrence (Google > 99.9% uptime)
- Fallback exists: local username/password login
- Implementation (Polly library retry) is 1 day effort and low risk
- Better to ship and iterate

**Acceptance Criteria (How to Remove)**
- Integrate Polly retry policy with exponential backoff (1s, 2s, 4s)
- Max 3 retries per OAuth request
- Log retry attempts for monitoring
- Alert if retry rate > 5% (indicator of persistent Google outage)

**Target Closure Date:** 1 week after launch (high priority)

**Accepted By:** (Tech Lead) [awaiting signature]

**Acceptance Date:** 2026-03-21

**Interim Mitigations:**
- Users can fall back to local login if OAuth fails
- Monitoring of Google API status (manual check of status.google.com)

**Related Documentation:**
- [[../architecture_risks#risk-002-keine-retry-backoff-für-google-oauth2-verbindung]] — Risk details
- [[../todo/todo-oauth-retry-policy]] — Implementation TODO

---

### RR-006: Incomplete Architecture Decision Record (ADR) Coverage

**ID:** RR-006  
**Severity:** 🟠 **MAJOR**  
**Category:** Documentation / Architecture

**Description:**
Only 2 ADRs exist (001, 001-doc-system). Missing ADRs for major risks:
- Rate limiting strategy
- Encryption policy
- HA strategy
- OAuth resilience

**Acceptance Rationale:**
- Documentation-only gap (no code impact)
- ADRs planned for sprint after launch
- Current architecture decisions are documented in [[../architecture/index.md]]
- Can create ADRs retroactively after decision is finalized

**Acceptance Criteria (How to Remove)**
- Create ADR-002: Rate Limiting Strategy (after implementation)
- Create ADR-003: Data Encryption Policy (after implementation)
- Create ADR-004: High Availability Architecture (during HA planning)
- Create ADR-005: OAuth2 Resilience Patterns (after implementation)
- Each ADR should follow [[../templates/adr-template.md]]

**Target Closure Date:** Q2 2026 (after risk mitigation implementation)

**Accepted By:** (Chief Architect) [awaiting signature]

**Acceptance Date:** 2026-03-21

**Related Documentation:**
- [[../architecture/architecture_risks.md]] — Risk context for missing ADRs
- [[../adrs/_meta.md]] — ADR naming convention
- [[../templates/adr-template.md]] — ADR template

---

## Summary Table

| ID | Issue | Severity | Target Closure | Status |
|---|---|---|---|---|
| RR-001 | No test framework | 🔴 Critical | 2 weeks | Accepted |
| RR-002 | No HA (single DB) | 🟠 High | Q3 2026 | Accepted |
| RR-003 | No PII encryption | 🔴 Critical | Before EU scale | Accepted |
| RR-004 | No rate limiting | 🟠 Critical | 1 week | Accepted |
| RR-005 | OAuth no retry | 🟠 High | 1 week | Accepted |
| RR-006 | Incomplete ADRs | 🟠 Major | Q2 2026 | Accepted |

---

## Risk Trend

```
Severity vs Time to Closure

🔴 2 Critical Issues (Testing, Encryption)  ──── Must fix in 1-2 weeks
🟠 4 High/Major Issues                      ──── Should fix in 1-4 weeks
```

---

## Acceptance Sign-offs (Awaiting)

- [ ] Chief Architect: RR-001, RR-006
- [ ] Tech Lead: RR-004, RR-005
- [ ] Operations Lead / CTO: RR-002
- [ ] Security Officer: RR-003, RR-004
- [ ] Product Lead: All RRs (business case approval)

**Note:** Once all stakeholders sign off, risk management plan is locked.

---

## Monitoring & Review Schedule

**Weekly Checks (First 4 weeks):**
- Any actual incidents related to RR-001, RR-004, RR-005?
- User complaints about OAuth login failures?
- Any attempted brute-force attacks?

**Monthly Review:**
- Have RR-001, RR-004, RR-005 been mitigated?
- Any movement toward RR-002, RR-003, RR-006?

**Quarterly Review:**
- Re-assess business case for each RR
- Update target closure dates if needed

---

## Related Documentation

- [[../auditor_report.md]] — Main audit report
- [[../auditor_non_compliance.md]] — Non-compliance details
- [[../architecture_risks.md]] — Architecture risk context
- [[../quality_goals.md]] — Quality goals

---

**Residual Risks (Accepted Gaps)**  
*Last Updated: 2026-03-21*  
*Status: Awaiting Stakeholder Sign-offs*
