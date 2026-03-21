---
type: todo
created: 2026-03-21
status: open
priority: high
category: enhancement
area: infrastructure
tags: [todo, ha, planning, sql-server, infrastructure]
estimated_effort: 24
impact: 4
urgency: 2
effort: 4
priority_score: 9
related_adrs: []
resolved_by: []
source_documents:
  - "[[../auditor_residual_risks#rr-002-single-sql-server-instance-no-ha]]"
  - "[[../architecture_risks#risk-004]]"
  - "[[../deployment_view#disaster-recovery]]"
---

# TODO: Plan & Design SQL Server High Availability Setup

## Problem/Idee

**Ausgangsituation:** Single SQL Server Instance, single point of failure. RTO ~30 minutes (manual restart)  
**Ziel (Q3 2026):** High Availability mit <10s Failover, 99.95% Uptime  
**Meilenstein:** Planning & Cost Analysis Q2, Implementation Q3

**Szenario:** Server-Crash am Black Friday. Mit HA: Automatisches failover in 10s. Ohne HA: 30+ Minutes Downtime = Lost Sales

---

## Business-Value

- **Availability:** 99.5% (aktuell) → 99.95% (mit HA) = 3.7 Stunden weniger Downtime/Jahr
- **Revenue Impact:** Bei ~€100/Minute Umsatzverlust = €370 Ersparnis/Jahr
- **Customer Trust:** SLA-fähig
- **Competitive:** Standard für Produktions-Apps

---

## Schritt-für-Schritt-Anleitung

### Schritt 1: Evaluate Optionen (Planning Phase)

**Option A: Azure SQL Managed Instance** (Empfohlen für Cloud-First)

**Pros:**
- ✅ Zero-downtime patching
- ✅ Automatic failover <10s
- ✅ Geo-redundancy available
- ✅ Managed by Azure (no ops burden)

**Cons:**
- ❌ Higher cost (~€200-400/month for HA)
- ❌ Requires Azure account & cloud setup

**Effort to Implement:** ~3-5 days (migration, testing)

---

**Option B: SQL Server AlwaysOn Availability Groups** (On-Premises)

**Pros:**
- ✅ Full control (on-premises)
- ✅ Supports multiple replicas (up to 9)
- ✅ Read-only secondary replicas

**Cons:**
- ❌ Requires 2+ servers (hardware cost)
- ❌ Complex setup & maintenance
- ❌ ~5-10 days implementation

**Effort to Implement:** ~5-10 days (infrastructure, testing)

---

**Option C: Doccker + Kubernetes** (Future-Proof)

**Pros:**
- ✅ Scalable, cloud-agnostic
- ✅ Auto-healing & failover
- ✅ Containers deployable to any cloud

**Cons:**
- ❌ Major refactor (containerization, orchestration)
- ❌ 2-3 weeks effort

**Effort:** ~2-3 weeks

---

### Schritt 2: Cost Analysis (Decision Criteria)

| Option | Monthly Cost | Setup Cost | Ops Effort | Failover Time |
|--------|--------------|-----------|-----------|---------------|
| **Current** (Single) | €50 | €0 | Low | ∞ (manual) |
| **Azure MI (HA)** | €200-400 | €2k | Very Low | <10s |
| **AlwaysOn** | €50 (existing servers) | €5k | High | <10s |
| **Kubernetes** | €100-200 | €10k | Medium | <5s |

**Recommendation:** Start with Azure MI (best ROI for startup phase)

### Schritt 3: Create Migration Plan (Document)

**Datei:** `autodocs/architecture/ha-migration-plan.md`

```markdown
# SQL Server HA Migration Plan (Q3 2026)

## Phase 1: Planning (2 weeks, Q2)
- [ ] Evaluate Azure MI vs AlwaysOn vs K8s
- [ ] Estimate cost (€200-400/month Azure)
- [ ] Get stakeholder approval
- [ ] Create detailed migration plan

## Phase 2: Infrastructure Setup (1 week)
- [ ] Provision Azure MI (if chosen)
- [ ] Setup monitoring & alerting
- [ ] Configure dr backup strategy

## Phase 3: Data Migration (3-5 days)
- [ ] Backup current database
- [ ] Restore to new HA instance
- [ ] Verify data integrity
- [ ] Cutover to new instance

## Phase 4: Testing & Validation (3-5 days)
- [ ] Failover tests
- [ ] Load tests
- [ ] Disaster recovery drills
- [ ] Customer acceptance test

## Phase 5: Rollback Plan (Prepared)
- [ ] Rollback to single-instance if needed
- [ ] Communication plan
- [ ] Rollback runbook

---

**Owner:** Operations Lead / Cloud Architect  
**Timeline:** Q3 2026  
**Budget:** €2k setup + €200-400/month  
**Success Criteria:** 99.95% uptime, <10s failover RTO
```

### Schritt 4: Setup Prerequisite Monitoring

**Command (Setup CloudWatch / Azure Monitor):**

```powershell
# If using Azure SQL MI, setup monitoring now
# (Prerequisite for HA is understanding current state)

# Query current database health
sqlcmd -S localhost -d Fitness -Q "
SELECT 
    DB_NAME() as DatabaseName,
    DATABASEPROPERTYEX(DB_NAME(), 'IsInStandby') as IsReadOnly,
    (SELECT COUNT(*) FROM sys.tables) as TableCount
FROM sys.databases
"
```

---

## Acceptance Criteria

- [ ] Planning document created: `ha-migration-plan.md`
- [ ] Cost analysis completed (compare all 3 options)
- [ ] Stakeholder approval obtained (decision logged)
- [ ] Monitoring setup (baseline metrics)
- [ ] Migration runbook drafted
- [ ] Risk assessment completed

---

## Effort Estimation

| Phase | Time |
|-------|------|
| Option Evaluation | 4h |
| Cost Analysis & Comparison | 4h |
| Migration Plan Document | 4h |
| Baseline Monitoring Setup | 4h |
| Risk Assessment | 4h |
| Stakeholder Review | 2h |
| **TOTAL (Planning)** | **22 hours** |
| **Implementation (Q3)** | **40-60 hours** (not in scope) |

---

## Priority Calculation

- **Impact:** 4/5 — Operational reliability improvement
- **Urgency:** 2/5 — Medium-term (Q3 after MVP launch)
- **Effort:** 4/5 — 24 hours planning, 40-60 hours implementation
- **Priority Score:** (4×2) + (2×1.5) - (4×0.5) = **9 → MEDIUM**

---

## Timeline

- ✅ Q1 2026: MVP Launch (single instance acceptable)
- ⏳ Q2 2026: Planning & Approval (THIS TODO)
- 🎯 Q3 2026: Implementation & Cutover

---

## Related Documentation

- [[../auditor_residual_risks#rr-002-single-sql-server-instance-no-ha]]
- [[../architecture/deployment_view#disaster-recovery]]
- [[../templates/adr-template]] (for ADR-005)

[[index]]
