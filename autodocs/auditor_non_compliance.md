---
type: report
date: 2026-03-21
agent: autodocs_auditor
version: 2.1
tags: [audit, non-compliance, findings]
---

# Auditor Non-Compliance Report

**Detailed list of all compliance findings from Auditor Review.**

---

## Summary

- **Total Issues Found:** 7
- **Critical:** 1 (test coverage gap)
- **Major:** 2 (missing test docs, architecture doc gaps)
- **Minor:** 4 (tag formatting, missing meta files)

---

## Critical Issues

### NC-CRIT-001: No Automated Test Coverage

**Severity:** 🔴 **CRITICAL**

**Category:** Test Coverage

**Finding:**
- No unit test files exist in codebase
- No test documentation in `/tests/unit/`
- No e2e test documentation in `/tests/e2e/`
- Manual QA only (per blackbox-report.md)
- Violates [[../quality_scenarios#qs-008-unit-test-coverage]] acceptance criteria

**Affected Components:**
- [[../architecture/building_block_view]] (Controllers, Models)
- [[../features/2026-03-21-user-management]]
- [[../features/2026-03-21-workout-tracking]]

**Impact:** High
- Cannot validate architecture quality
- Risk of regressions in production
- No CI/CD automation possible

**Root Cause:**
Project initialized without test infrastructure.

**Recommended Mitigation:**
1. Create test project: `dotnet new xunit -n Fitness.Tests`
2. Add 20+ unit tests (min 80% coverage of controllers, models)
3. Document tests in [[../tests/unit/test-auth.md]], [[../tests/unit/test-exercises.md]], etc.
4. Set up CI/CD to run tests on every commit

**Effort:** 1-2 weeks

**Status:** Open → TODO created

**Related ADR:** None (create new ADR for test strategy)

**Related Todo:** [[../todo/todo-create-unit-test-stubs]]

---

## Major Issues

### NC-MAJ-001: Missing Test Documentation Files

**Severity:** 🟠 **MAJOR**

**Category:** Documentation Completeness

**Finding:**
- `/tests/unit/` has `_meta.md` and `index.md` but no individual test docs
- `/tests/e2e/` has `_meta.md` and `index.md` but no individual test docs
- Per [[../tests/_meta.md]], each test should have a doc (e.g., `test-auth-service.md`)
- Expected: ~10 test documentation files
- Actual: 0

**Affected Directory:**
- `autodocs/tests/unit/`
- `autodocs/tests/e2e/`

**Impact:** Medium
- Missing documentation of test coverage
- Cannot trace test ↔ feature mapping
- Makes onboarding harder

**Root Cause:**
Tests don't exist yet, so documentation can't exist.

**Recommended Mitigation:**
1. Once tests are written, create test doc stubs
2. Use [[../templates/test-template.md]] as template
3. Link bidirectionally: test-doc → feature, feature → test-doc

**Effort:** 2-3 days (after tests exist)

**Status:** Open (depends on NC-CRIT-001)

**Related Docs:**
- [[../tests/_meta.md]] — Test naming conventions
- [[../templates/test-template.md]] — Test doc template
- [[../architecture/architecture_mapping#feature--test-mapping-bidirectional]]

---

### NC-MAJ-002: Incomplete Architecture Risk Documentation

**Severity:** 🟠 **MAJOR**

**Category:** Decision Documentation

**Finding:**
- ADR collection has only 2 ADRs (001, 001-documentation-system)
- Per [[../architecture/quality_goals.md]], 6 architecture risks identified
- Expected: ADRs for each major risk (rate limiting, encryption, HA, etc.)
- Gap: 4-5 ADRs missing

**Affected ADRs (Missing):**
- ADR-002: Rate Limiting Strategy (see [[../architecture/architecture_risks#arch-001]])
- ADR-003: Data Encryption Policy (see [[../architecture/architecture_risks#arch-003]])
- ADR-004: High Availability Strategy (see [[../architecture/architecture_risks#arch-004]])
- ADR-005: OAuth2 Resilience Pattern (see [[../architecture/architecture_risks#arch-002]])

**Impact:** Medium
- Decisions undocumented
- Risk of repeating debates
- New team members can't understand rationale

**Root Cause:**
Focused on features; architectural decisions deferred.

**Recommended Mitigation:**
1. Create ADR-002 through ADR-005 using [[../templates/adr-template.md]]
2. Link to relevant architecture risks and quality goals
3. Schedule architecture review meeting to decide on gaps

**Effort:** 3-5 days

**Status:** Open

**Related Docs:**
- [[../architecture/architecture_risks.md]] — Risk context
- [[../templates/adr-template.md]] — ADR template
- [[../adrs/_meta.md]] — ADR naming convention

---

## Minor Issues

### NC-MIN-001: Tag Format Inconsistency ("#" Prefix)

**Severity:** 🟡 **MINOR**

**Category:** Tag Formatting

**Finding:**
- 12 files use non-standard tag format: `[#feature, #auth, #security]`
- Standard format (per [[../guides/tag-taxonomy.md]]): `[feature, auth, security]`
- "#" should NOT be in tag values (only in markdown for headings)

**Affected Files (12 total):**
1. `features/2026-03-21-user-management.md` line 5
2. `features/2026-03-21-workout-tracking.md` line 5
3. `blackbox/public/fitness-webapp-api.md` line 8
4. `blackbox/blackbox-report.md` line 6
5. `todo/todo-add-unit-tests.md` line 7
6. Plus 7 others...

**Impact:** Low
- Parsing confusion (minor)
- Tag index generation affected (if machine-readable)
- Inconsistent with rest of docs (73 correct files)

**Root Cause:**
Copy-paste error from markdown heading syntax.

**Recommended Mitigation:**
Remove "#" from all tag values. Auto-fix available.

**Effort:** 15 minutes (manual), or automated via search-replace

**Status:** Ready for auto-fix

**Related Docs:**
- [[../guides/tag-taxonomy.md]] — Tag standards

---

### NC-MIN-002: Missing `_meta.md` in Blackbox Subdirectories

**Severity:** 🟡 **MINOR**

**Category:** Structure Consistency

**Finding:**
- Directory `/autodocs/blackbox/public/` exists but has no `_meta.md`
- Directory `/autodocs/blackbox/internal/` exists but has no `_meta.md`
- Per [[../architecture/_meta.md]], all subdirectories should have `_meta.md`
- Only parent `/autodocs/blackbox/` has `_meta.md`

**Expected Files:**
- `autodocs/blackbox/public/_meta.md` (missing)
- `autodocs/blackbox/internal/_meta.md` (missing)

**Impact:** Low
- Breaks consistency pattern
- No documented conventions for public vs internal docs
- Minor tooling issue

**Root Cause:**
Blackbox documentation created without full scaffolding.

**Recommended Mitigation:**
1. Create `blackbox/public/_meta.md` with public interface rules
2. Create `blackbox/internal/_meta.md` with internal/risk rules
3. Link from parent `blackbox/_meta.md`

**Effort:** 1-2 hours

**Status:** Open

**Related Docs:**
- [[../blackbox/_meta.md]] — Parent metadata

---

### NC-MIN-003: Missing `updated` Field in Feature Docs

**Severity:** 🟡 **MINOR**

**Category:** Frontmatter Completeness

**Finding:**
- File: `features/2026-03-21-user-management.md`
- Missing optional `updated` field
- Per [[../features/_meta.md]], `updated` is recommended for tracking changes

**Impact:** Low
- Optional field (not required)
- Minor: no way to track when doc was last edited
- Easy fix

**Recommended Mitigation:**
Add `updated: 2026-03-21` to frontmatter.

**Effort:** 2 minutes

**Status:** Ready for auto-fix

---

### NC-MIN-004: Inconsistent `date` vs `created` Field

**Severity:** 🟡 **MINOR**

**Category:** Frontmatter Standardization

**Finding:**
- File: `auditor_report.md` uses `date: 2026-03-21`
- Standard field (per all _meta.md): `created: YYYY-MM-DD`
- Minor inconsistency

**Impact:** Very Low
- Only affects report files
- Machine parsing should handle both
- Cosmetic issue

**Recommended Mitigation:**
Standardize to `created:` field across all docs.

**Effort:** 1 hour

**Status:** Low priority

---

## Summary Table

| ID | Issue | Category | Severity | Files | Effort | Status |
|---|---|---|---|---|---|---|
| NC-CRIT-001 | No automated tests | Testing | 🔴 Critical | N/A (code) | 1-2w | Open |
| NC-MAJ-001 | Missing test docs | Docs | 🟠 Major | 0 files | 3d | Depends on CRIT-001 |
| NC-MAJ-002 | Missing ADRs | Architecture | 🟠 Major | 4-5 needed | 3-5d | Open |
| NC-MIN-001 | Tag "#" prefix | Format | 🟡 Minor | 12 files | 15m | Fixable |
| NC-MIN-002 | Missing _meta.md | Structure | 🟡 Minor | 2 dirs | 1-2h | Fixable |
| NC-MIN-003 | Missing `updated` | Frontmatter | 🟡 Minor | 1 file | 2m | Fixable |
| NC-MIN-004 | Date field inconsistency | Frontmatter | 🟡 Minor | 1 file | 1h | Low priority |

---

## Linked Risks (from [[../architecture/architecture_risks.md]])

These non-compliances directly relate to architecture risks:

| Risk | NC Issue | Connection |
|---|---|---|
| [[../architecture/architecture_risks#arch-001]] (No rate limiter) | NC-MAJ-002 | Missing ADR-002 for rate limiting strategy |
| [[../architecture/architecture_risks#arch-003]] (No encryption) | NC-MAJ-002 | Missing ADR-003 for encryption policy |
| [[../architecture/architecture_risks#arch-006]] (No tests) | NC-CRIT-001 | Critical gap = security & quality risk |

---

## Disposition

### Ready for Auto-Fix (Next Sprint)
- NC-MIN-001: Tag normalization
- NC-MIN-003: Add `updated` field

### Requires Development Work
- NC-CRIT-001: Create test project (1-2 weeks)
- NC-MAJ-001: Write tests then document them
- NC-MAJ-002: Create 4-5 ADRs (3-5 days)
- NC-MIN-002: Create _meta.md stubs (1-2 hours)

---

## Related Documentation

- [[../auditor_report.md]] — Main audit report
- [[../auditor_corrections.md]] — Auto-fixes applied log
- [[../architecture/architecture_risks.md]] — Architecture risks
- [[../quality_scenarios.md]] — Quality testing scenarios

---

**Generated by AutoDocs 50 · Auditor Agent**  
*Date: 2026-03-21 · Confidence: Strong (95%)*
