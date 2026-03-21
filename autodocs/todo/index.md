---
type: index
created: 2026-03-21
updated: 2026-03-21
tags: [index, todo, backlog, prioritized]
---

# 📋 TODOs & Backlog (Konsolidiert)

**Priorisierte TODO-Liste aus Audit Agent 50 (Auditor) & vorherigen Agenten.**

---

## Executive Summary

- **Total TODOs:** 8
- **Open:** 8
- **In Progress:** 0
- **Completed:** 0
- **Blocked:** 1 (waiting for test infrastructure)

**Gesamtaufwand:** ~170 Stunden  
**Geschätzter Wert:** €92.7k (Time savings + Risk reduction + GDPR compliance)

**Critical Path:** [[#todo-create-unit-test-infrastructure]] → [[#todo-create-test-documentation]]

---

## 🔴 HIGH PRIORITY TODOs (Score ≥13)

**Diese sollten in den nächsten 2-4 Wochen begonnen werden.**

### 1. [[todo-create-unit-test-infrastructure|TODO: Create Unit Test Infrastructure]]
- **Priority Score:** 15.5 | **Effort:** 80h | **Impact:** 5/5 | **Status:** OPEN
- **Blocker für:** [[todo-create-test-documentation]]
- **Business Value:** €20k/year (time savings) + risk mitigation
- **First Command:** `dotnet new xunit -n Fitness.Tests`

### 2. [[todo-add-rate-limiting-middleware|TODO: Add Rate Limiting Middleware]]
- **Priority Score:** 16.5 (HIGHEST!) | **Effort:** 8h | **Impact:** 5/5 | **Status:** OPEN
- **Urgency:** 5/5 (1-week deadline)
- **Business Value:** €2k (prevent DDoS breach)

### 3. [[todo-oauth2-retry-policy|TODO: Implement OAuth2 Retry Policy]]
- **Priority Score:** 13 | **Effort:** 8h | **Impact:** 4/5 | **Status:** OPEN
- **Business Value:** €3.2k/year (fewer support tickets)

### 4. [[todo-enable-pii-encryption-tde|TODO: Enable PII Data Encryption (TDE)]]
- **Priority Score:** 15.5 | **Effort:** 2h | **Impact:** 5/5 | **Status:** OPEN
- **Quick Win:** Only 2 hours! GDPR compliance
- **Business Value:** €50k (prevent GDPR fine)

### 5. [[todo-create-missing-architecture-adrs|TODO: Create Missing Architecture ADRs]]
- **Priority Score:** 11 | **Effort:** 24h | **Impact:** 4/5 | **Status:** OPEN
- **Creates:** ADR-002 (Testing), ADR-003 (Rate Limiting), ADR-004 (Encryption), ADR-005 (HA)
- **Business Value:** €5k (onboarding efficiency)

---

## 🟠 MEDIUM PRIORITY TODOs (Score 8-12)

### 6. [[todo-plan-sql-server-ha-setup|TODO: Plan SQL Server High Availability Setup]]
- **Priority Score:** 9 | **Effort:** 24h (planning) | **Impact:** 4/5 | **Status:** OPEN
- **Timeline:** Q3 2026 (post-launch)
- **Business Value:** €10k (prevent downtime)

### 7. [[todo-create-test-documentation|TODO: Create Test Documentation Stubs]]
- **Priority Score:** 8 | **Effort:** 16h | **Impact:** 3/5 | **Status:** 🔴 BLOCKED
- **Blocked By:** [[todo-create-unit-test-infrastructure]]
- **Business Value:** €2k (dev efficiency)

---

## 🟢 LOW PRIORITY TODOs (Score <8)

### 8. [[todo-fix-tag-format-inconsistencies|TODO: Fix Tag Format Inconsistencies]]
- **Priority Score:** 6.5 | **Effort:** 0.5h | **Impact:** 2/5 | **Status:** OPEN
- **Quick Win:** 30 minutes! No blockers.
- **Business Value:** €0.5k (automation wins)

---

## 📊 Statistiken & Breakdown

| Priority | Count | Hours | Value | % |
|----------|-------|-------|-------|---|
| 🔴 HIGH | 5 | 120 | €92k | 71% |
| 🟠 MEDIUM | 2 | 40 | — | 24% |
| 🟢 LOW | 1 | 0.5 | — | <1% |
| **TOTAL** | **8** | **160.5** | **€92.7k** | **100%** |

---

## 🎯 Recommended Execution Order

1. **Sprint 1 (Weeks 1-2):** Unit Tests + Quick Wins
   - [[todo-create-unit-test-infrastructure]] (80h) ← CRITICAL
   - [[todo-add-rate-limiting-middleware]] (8h) ← Parallel
   - [[todo-enable-pii-encryption-tde]] (2h) ← QUICK WIN
   - [[todo-fix-tag-format-inconsistencies]] (0.5h) ← QUICK WIN

2. **Sprint 2 (Weeks 3-4):** Resilience + Documentation
   - [[todo-oauth2-retry-policy]] (8h)
   - [[todo-create-missing-architecture-adrs]] (24h)
   - [[todo-create-test-documentation]] (16h) ← Unblocked

3. **Sprint 3 (Q2):** Planning
   - [[todo-plan-sql-server-ha-setup]] (24h)

---

## 💰 Business Value: €92.7k

- **Time Savings:** €20.7k/year (test automation)
- **Compliance:** €50k (GDPR fines prevented)
- **Reliability:** €17k (fewer incidents)
- **Knowledge:** €5k (onboarding)

**ROI: €579/hour** 📈

---

## 🚀 Quick Wins (2.5 hours, start today!)

1. [[todo-enable-pii-encryption-tde]] - 2h, €50k value
2. [[todo-fix-tag-format-inconsistencies]] - 0.5h, 100% compliance

---

## 📌 Related Documentation

- [[./todo_consolidation_report.md]] — Full consolidation report & analysis
- [[../auditor_report.md]] — Audit findings (92/100 score)
- [[../auditor_non_compliance.md]] — 7 non-compliances
- [[../auditor_residual_risks.md]] — 6 accepted risks

---

**Status: ✅ READY FOR EXECUTION**  
*Agent 60 · TodoLister · 2026-03-21*

[[_meta]] | [[../index]]
