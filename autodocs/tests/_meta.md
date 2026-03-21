---
type: meta
created: 2025-01-11
tags: [meta, tests, quality, coverage]
---

# /autodocs/tests/_meta.md – Test Documentation

## Zweck

Tests dokumentieren **wie wir Qualität sichern**:
- Unit-Tests (einzelne Funktionen/Module)
- Integration-Tests (Komponenten-Interaktion)
- E2E-Tests (End-to-End User Flows)
- Performance-Tests
- Security-Tests

Jede Test-Dok **muss** mit Features verknüpft sein.

---

## Ordnerstruktur

```
/tests/
├── _meta.md              # Diese Datei
├── index.md              # Test-Übersicht
├── coverage.md           # Coverage-Report
├── /unit/                # Unit-Tests
│   ├── _meta.md
│   └── index.md
├── /e2e/                 # End-to-End-Tests
│   ├── _meta.md
│   └── index.md
├── /integration/         # Integration-Tests (optional)
│   ├── _meta.md
│   └── index.md
└── /performance/         # Performance-Tests (optional)
    ├── _meta.md
    └── index.md
```

---

## Dateinamen-Konvention

```
test-bereich-funktionalitaet.md
```

- **Bereich:** unit, e2e, integration, performance
- **Funktionalität:** Was wird getestet
- **Beispiele:**
  - `unit/test-auth-service.md`
  - `e2e/test-login-flow.md`
  - `integration/test-payment-gateway.md`

---

## Pflicht-Frontmatter

```yaml
---
type: test
test_type: unit | integration | e2e | performance | security
created: YYYY-MM-DD
updated: YYYY-MM-DD         # optional
status: passing | failing | flaky | skipped
area: <bereich>              # z.B. auth, payment, ui
tags: [test, ...]
components: []               # Getestete Komponenten
related_features: []         # Features die getestet werden
test_files: []               # Pfade zu Test-Files im Code
coverage: 0-100              # Coverage-Prozent (optional)
execution_time: <ms>         # Durchschnittliche Laufzeit (optional)
---
```

### Status-Definitionen

| Status | Bedeutung |
|--------|-----------|
| `passing` | Test läuft grün |
| `failing` | Test schlägt fehl |
| `flaky` | Test ist instabil |
| `skipped` | Test ist deaktiviert |

---

## Pflicht-Struktur

```markdown
# Test: Titel

## Was wird getestet?
[Kurzbeschreibung der Funktionalität]

## Tested Feature
- [[../features/YYYY-MM-DD-feature]] – Hauptfeature

## Test Cases

### Test Case 1: Titel
**Given:** [Vorbedingung]  
**When:** [Aktion]  
**Then:** [Erwartetes Ergebnis]

### Test Case 2: Titel
...

## Test Implementation
```path
/src/__tests__/auth-service.test.ts
```

## Coverage
- **Lines:** X%
- **Branches:** Y%
- **Functions:** Z%

## Known Issues
- [Falls der Test flaky ist oder bekannte Probleme hat]

## Maintenance Notes
- [Besonderheiten beim Maintainen]

## Related
- [[related-test]]
- [[related-feature]]
```

---

## Test-Typen im Detail

### Unit-Tests (`/tests/unit/`)
**Fokus:** Einzelne Funktionen/Klassen isoliert testen

**Beispiele:**
- `test-auth-service.md` – Service-Logik
- `test-user-model.md` – Datenmodell-Validierung
- `test-utils.md` – Utility-Funktionen

**Konventionen:**
- Mock alle Dependencies
- Schnelle Ausführung (<100ms)
- Keine Netzwerk/DB-Calls

### Integration-Tests (`/tests/integration/`)
**Fokus:** Zusammenspiel mehrerer Komponenten

**Beispiele:**
- `test-api-database.md` – API + DB
- `test-payment-gateway.md` – Payment + External API

**Konventionen:**
- Reale Dependencies (oder Test-Doubles)
- Setup/Teardown für Ressourcen
- Mittlere Laufzeit (<5s)

### E2E-Tests (`/tests/e2e/`)
**Fokus:** Komplette User Flows

**Beispiele:**
- `test-login-flow.md` – Login → Dashboard
- `test-checkout-process.md` – Warenkorb → Payment → Confirmation

**Konventionen:**
- Browser-Automation (Playwright, Cypress)
- Reales System (oder Staging)
- Längere Laufzeit (5-30s)

### Performance-Tests (`/tests/performance/`)
**Fokus:** Geschwindigkeit, Skalierung

**Beispiele:**
- `test-api-response-time.md`
- `test-concurrent-users.md`

**Konventionen:**
- Baseline-Metriken definieren
- Threshold-Werte setzen
- Load-Testing-Tools (k6, JMeter)

---

## Coverage-Tracking

Siehe `[[coverage]]` für den vollständigen Coverage-Report.

### Coverage-Ziele
- **Unit-Tests:** ≥80%
- **Integration-Tests:** ≥60%
- **E2E-Tests:** Kritische User Flows

### Coverage-Update-Workflow
```bash
# 1. Tests laufen lassen mit Coverage
npm run test:coverage

# 2. Coverage-Report generieren
# (automatisch in CI/CD)

# 3. coverage.md updaten
# (manuell oder via Agent)
```

---

## Verlinkung

### Von Test zu Feature

```markdown
## Tested Feature
Diese Tests validieren [[../features/2025-01-15-user-authentication]].
```

### Von Feature zu Test

```markdown
## Tests
- [[../tests/unit/test-auth-service]] – Unit-Tests
- [[../tests/e2e/test-login-flow]] – E2E-Tests
```

---

## Tags-Konvention

- **Test-Typ:** `#test/unit`, `#test/e2e`, `#test/integration`
- **Status:** `#status/passing`, `#status/flaky`
- **Komponenten:** `#cmp/auth-service`
- **Kritikalität:** `#critical`, `#nice-to-have`
- **Domäne:** `#dom/<bereich>`

---

## Flaky Tests

Wenn ein Test als `flaky` markiert ist:

### Pflicht-Dokumentation
```markdown
## Flaky Test Investigation

### Symptome
- [Wie äußert sich die Instabilität?]

### Mögliche Ursachen
- [ ] Race Condition
- [ ] Timing-Issues
- [ ] External Dependencies
- [ ] Test-Isolation-Problem

### Mitigation
- [Was wurde versucht?]

### Next Steps
- [ ] Weitere Analyse
- [ ] Rewrite Test
- [ ] Skip temporär
```

---

## Test-Maintenance

### Regelmäßige Reviews
- **Wöchentlich:** Flaky Tests analysieren
- **Monatlich:** Coverage-Gaps identifizieren
- **Quarterly:** Test-Suite-Performance optimieren

### Obsolete Tests
Wenn ein Feature deprecated wird:
1. Test-Status auf `skipped` setzen
2. Verweis auf Deprecation-Grund
3. Timeline für Test-Removal

---

## Qualitätskriterien

Gute Test-Doku hat:
- ✅ Klare Test Cases (Given/When/Then)
- ✅ Features sind verlinkt
- ✅ Coverage ist dokumentiert
- ✅ Test-Files sind referenziert
- ✅ Known Issues sind dokumentiert
- ✅ Status ist aktuell

---

## Agent-Hinweise

### Automatisierbare Tasks

- **Coverage-Updates:** Aus CI/CD-Reports
- **Test-Status-Updates:** Aus Test-Runs
- **Flaky-Test-Detection:** Aus Historie
- **Missing-Test-Detection:** Features ohne Tests finden
- **Test-File-Linking:** Automatisch Code-Links setzen

### Agent-Workflow für Test-Doku

```python
def document_test(test_file, test_type, area):
    # 1. Parse Test-File
    test_cases = parse_test_cases(test_file)
    coverage = get_coverage_for_file(test_file)
    
    # 2. Finde related Features
    related_features = find_features_for_area(area)
    
    # 3. Generiere Dateinamen
    filename = f"test-{area}-{slugify(test_file)}.md"
    
    # 4. Nutze Template
    content = render_template('test-template', {
        'test_type': test_type,
        'area': area,
        'test_cases': test_cases,
        'coverage': coverage,
        'related_features': related_features
    })
    
    # 5. Erstelle Datei
    path = f"autodocs/tests/{test_type}/{filename}"
    write_file(path, content)
    
    # 6. Update Coverage-Report
    update_coverage_report(area, coverage)
    
    return path
```

---

## For AI Agents: Test-Specific Rules

**🤖 When you process this directory:**

### Validation Rules
```yaml
test_validation:
  frontmatter_required:
    - test_type: [unit, integration, e2e, performance, security]
    - status: [passing, failing, flaky, skipped]
    - area: string
    - components: array (with #cmp/ tags)
    - related_features: array (bidirectional link required)
    - test_files: array (paths to actual test code)
  
  structure_required:
    - "## Was wird getestet?"
    - "## Tested Feature"
    - "## Test Cases"
    - "## Coverage"
```

### Auto-Fix Actions
```yaml
auto_fix:
  - parse_coverage_reports: "coverage/coverage-final.json"
  - update_coverage_md: "autodocs/tests/coverage.md"
  - link_tests_to_features_bidirectionally
  - extract_component_tags_from_file_paths
  - create_todos_for_missing_tests
  
coverage_goals:
  unit: 80
  integration: 60
  e2e: critical_flows_only
```

### Code-Sync Actions
```yaml
detect_tests:
  patterns:
    - "**/*.test.{ts,tsx,js,jsx}"
    - "**/*.spec.{ts,tsx,js,jsx}"
    - "**/__tests__/**/*"
  
  actions:
    - create_test_doc_if_missing
    - link_to_tested_component
    - update_coverage_table
```

### Coverage Tracking
```yaml
when_coverage_updated:
  - parse_json: "coverage/coverage-final.json"
  - update: "autodocs/tests/coverage.md"
  - for_each_component:
      if_coverage_lt_80: create_todo
  - visualize_trends: true
```

### Workflow
1. Scan for test files in codebase
2. Create test docs if missing
3. Link to features bidirectionally
4. Parse coverage reports
5. Update coverage.md
6. Create TODOs for gaps

---

## Related

- [[index]] – Test-Übersicht
- [[coverage]] – Coverage-Report
- [[../templates/test-template]] – Template für neue Tests
- [[../features/_meta]] – Wie Tests und Features zusammenhängen
- [[../index]] – Haupt-Navigation

[[index]]
