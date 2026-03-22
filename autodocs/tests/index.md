---
type: index
created: 2025-01-11
updated: 2025-01-11
tags: [test, quality, index, overview]
---

# 🧪 Test Documentation

> **Für Agents:** Lies zuerst `[[_meta]]` für Test-Dokumentations-Konventionen.

## Überblick

Hier dokumentieren wir **wie wir Qualität sichern**:
- **[[unit/index|Unit-Tests]]** – Einzelne Funktionen/Module
- **[[e2e/index|E2E-Tests]]** – End-to-End User Flows
- **[[coverage|Coverage]]** – Test-Abdeckung & Metriken

---

## Quick Stats

- **Total Tests:** 0
- **Coverage:** 0%
- **Status:** 🔴 Test documentation created (implementation pending)

---

## Test-Bereiche

### [[unit/index|Unit-Tests]]
Isolierte Tests für einzelne Funktionen/Klassen
- **Ziel:** ≥80% Coverage
- **Aktuell:** 0 Tests (Documentation: 4 docs created)
- Test docs: [[unit/test-user-management|User Management]], [[unit/test-workout-tracking|Workout Tracking]], [[unit/test-admin|Admin]], [[unit/test-image-management|Image Management]]

### [[e2e/index|E2E-Tests]]
End-to-End Tests für User Flows
- **Ziel:** Alle kritischen Flows
- **Aktuell:** 0 Tests (To be created)

### [[coverage|Coverage-Report]]
Detaillierte Coverage-Metriken
- **Ziel:** ≥80% Overall
- **Aktuell:** 0%

---

## Test Documentation Overview

| Area | Status | Feature Link |
|------|--------|--------------|
| User Management | 📝 Documented | [[../features/2026-03-21-user-management]] |
| Workout Tracking | 📝 Documented | [[../features/2026-03-21-workout-tracking]] |
| Admin Features | 📝 Documented | [[../features/2026-03-21-admin-features]] |
| Image Management | 📝 Documented | [[../features/2026-03-22-image-management]] |

---

## Wie dokumentiere ich einen Test?

1. **Erstelle Test-Datei:**
   ```bash
   autodocs/tests/unit/test-my-feature.md
   # oder
   autodocs/tests/e2e/test-my-flow.md
   ```

2. **Nutze Template:**
   - `[[../templates/test-template]]`

3. **Verlinke Feature:**
   - Setze `related_features` im Frontmatter

4. **Update Coverage:**
   - `[[coverage]]` aktualisieren

---

## Related

- [[_meta]] – Test-Konventionen
- [[coverage]] – Coverage-Report
- [[unit/index]] – Unit-Tests
- [[e2e/index]] – E2E-Tests
- [[../templates/test-template]] – Template
- [[../index]] – Haupt-Navigation

[[../index]]
