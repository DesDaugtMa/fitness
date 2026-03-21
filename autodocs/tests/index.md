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
- **Status:** 🔴 No tests yet

---

## Test-Bereiche

### [[unit/index|Unit-Tests]]
Isolierte Tests für einzelne Funktionen/Klassen
- **Ziel:** ≥80% Coverage
- **Aktuell:** 0 Tests

### [[e2e/index|E2E-Tests]]
End-to-End Tests für User Flows
- **Ziel:** Alle kritischen Flows
- **Aktuell:** 0 Tests

### [[coverage|Coverage-Report]]
Detaillierte Coverage-Metriken
- **Ziel:** ≥80% Overall
- **Aktuell:** 0%

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
