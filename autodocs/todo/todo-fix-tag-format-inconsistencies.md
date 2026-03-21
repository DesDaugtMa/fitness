---
type: todo
created: 2026-03-21
status: open
priority: low
category: tech-debt
area: documentation
tags: [todo, tag-format, frontmatter, tech-debt, cmp/docs]
estimated_effort: 0.5
impact: 2
urgency: 2
effort: 1
priority_score: 6.5
related_adrs: []
resolved_by: []
source_documents:
  - "[[../auditor_non_compliance#nc-min-001-tag-format-inconsistency-prefix]]"
  - "[[../auditor_corrections#tag-normalization]]"
---

# TODO: Fix Tag Format Inconsistencies

## Problem/Idee

**Ausgangsituation:** 12 Dokumentdateien nutzen nicht-standardisiertes Tag-Format `[#feature, #auth]` statt `[feature, auth]`  
**Standard (per [[../guides/tag-taxonomy.md]]):** keine `#`-Zeichen in Tag-Werten  
**Ziel:** 100% Tag-Format Compliance  
**Gap:** 86% konforme Tags → 100%

**Szenario:** Tag-Index-Generator scheitert wegen `#`-Präfix, Tag-Search-Tools funktionieren nicht

---

## Business-Value

- **Machine Readability:** Tag-Parser-Tools funktionieren korrekt
- **Consistency:** Alle 85 Dokumente nutzen selbes Format
- **Effort:** Nur 30 Minuten (automatisiert oder manuell)

---

## Schritt-für-Schritt-Anleitung

**🎯 ERSTER SCHRITT:** (Automated Search & Replace)

```powershell
cd C:\Repositories\fitness\autodocs

# 1. Finde alle Dateien mit "#"-Präfix in Tags
$files = Get-ChildItem -Recurse -Filter "*.md" | Select-String -Pattern 'tags: \[#[a-z]'

# Ergebnis sollte 12 Dateiena zeigen:
# - features/2026-03-21-user-management.md
# - features/2026-03-21-workout-tracking.md
# - blackbox/public/fitness-webapp-api.md
# - ... (9 mehr)

Write-Host "Found $($files.Count) files with incorrect tag format"
```

### Schritt 1: Automated Search & Replace

**Option A: PowerShell (Alle Dateien gleichzeitig)**

```powershell
$files = Get-ChildItem -Recurse -Filter "*.md" -Path "."

foreach ($file in $files) {
    $content = Get-Content -Path $file.FullName -Raw
    
    # Replace all occurrences of [#tag with [tag
    $newContent = $content -replace 'tags: \[(#[a-z])', 'tags: [$1' -replace 'tags: \[(#[a-z][^]]*)', 'tags: [$($($_ -replace "#", "")))'
    
    # Better: Use this simpler pattern
    $newContent = $content -replace '(tags: \[)#([a-z])', '$1$2'  # Remove # from first tag
    $newContent = $newContent -replace '(, )#([a-z])', '$1$2'    # Remove # from subsequent tags
    
    # Write if content changed
    if ($content -ne $newContent) {
        Set-Content -Path $file.FullName -Value $newContent
        Write-Host "✓ Fixed: $($file.Name)"
    }
}

Write-Host "Tag normalization complete!"
```

**Validation:**
```powershell
# Verify: No more tags with #
Get-ChildItem -Recurse -Filter "*.md" | Select-String -Pattern 'tags: \[#' 

# Should return: No matches found ✓
```

### Schritt 2: Manual Verification (Einzelne Dateien)

**Vor:**
```yaml
---
type: feature
tags: [#feature, #auth, #security]
---
```

**Nach:**
```yaml
---
type: feature
tags: [feature, auth, security]
---
```

**Dateien zu überprüfen (12 total):**
1. `features/2026-03-21-user-management.md`
2. `features/2026-03-21-workout-tracking.md`
3. `blackbox/public/fitness-webapp-api.md`
4. `blackbox/blackbox-report.md`
5. `todo/todo-add-unit-tests.md`
6. `architecture/quality_goals.md`
7. `architecture/quality_scenarios.md`
8. `architecture/constraints.md`
9. `architecture/architecture_risks.md`
10. `architecture/architecture_mapping.md`
11. `blackbox/internal/risk-register.md`
12. `questions/technisch/Q-0001-ef-decision-clarification.md`

---

## Acceptance Criteria

- [ ] All tags validated: No more `#` prefix in tag values
- [ ] Files checked: All 12 affected files updated
- [ ] Validation passed: `Get-ChildItem -Recurse -Filter "*.md" | Select-String "tags: \[#"` returns 0 matches
- [ ] No content loss: Only tags changed, no other content affected

---

## Effort Estimation

| Phase | Time |
|-------|------|
| Search & Replace (automated) | 0.25h |
| Manual Verification | 0.15h |
| Validation | 0.1h |
| **TOTAL** | **0.5 hours (30 minutes)** |

---

## Priority Calculation

- **Impact:** 2/5 — Low impact (cosmetic bug, machine readability)
- **Urgency:** 2/5 — Can do anytime
- **Effort:** 1/5 — 30 minutes
- **Priority Score:** (2×2) + (2×1.5) - (1×0.5) = **6.5 → LOW**

---

## Quick Win

This is a **Quick Win** — Very high ROI (0.5h effort for 100% tag compliance)

---

[[index]]
