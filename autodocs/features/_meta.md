---
type: meta
created: 2025-01-11
tags: [meta, features, changes, implementation]
---

# /autodocs/features/_meta.md – Feature & Change Documentation

## Zweck

Features dokumentieren **implementierte Änderungen** am System:
- Neue Features (funktional/nicht-funktional)
- Größere Refactorings
- Breaking Changes
- Deprecations

Jedes Feature **muss** mit Tests und/oder ADRs verknüpft sein.

---

## Dateinamen-Konvention

```
YYYY-MM-DD-kurzer-sprechender-titel.md
```

- **YYYY-MM-DD:** Implementierungsdatum (oder Merge-Datum)
- **Titel:** kebab-case, beschreibend
- **Beispiele:**
  - `2025-01-15-user-authentication-system.md`
  - `2025-02-03-migrate-to-postgresql.md`
  - `2025-03-20-add-dark-mode-ui.md`

---

## Pflicht-Frontmatter

```yaml
---
type: feature
created: YYYY-MM-DD
updated: YYYY-MM-DD           # optional
status: planned | in-progress | released | deprecated
area: <bereich>                # z.B. auth, ui, data, api
tags: [feature, ...]
components: []                 # z.B. [auth-service, user-model]
related_adrs: []               # ADRs die diese Änderung begründen
related_tests: []              # Tests die diese Änderung validieren
related_todos: []              # TODOs die dadurch erledigt wurden
commits: []                    # Git commit SHAs oder Messages
tickets: []                    # Jira/Linear/GitHub Issue IDs
breaking_change: false         # true wenn Breaking Change
---
```

### Status-Definitionen

| Status | Bedeutung |
|--------|-----------|
| `planned` | Geplant, noch nicht begonnen |
| `in-progress` | In Entwicklung |
| `released` | Produktiv deployed |
| `deprecated` | Veraltet, wird entfernt |

---

## Pflicht-Struktur

```markdown
# Feature: Titel

## Kurzbeschreibung
[1-3 Sätze: Was wurde gemacht?]

## Motivation
[Warum war diese Änderung nötig?]

## Änderungen

### Code
- [Komponente X]: Beschreibung
- [Modul Y]: Beschreibung

### API Changes (falls relevant)
- Neue Endpoints
- Geänderte Endpoints
- Deprecated Endpoints

### Database Changes (falls relevant)
- Neue Tables/Schemas
- Migrationen
- Data Transformations

### UI Changes (falls relevant)
- Neue Screens/Components
- Geänderte Flows
- UX-Improvements

## Tests
- [[test-unit-xyz]] – Unit-Tests
- [[test-e2e-abc]] – E2E-Tests
- Coverage: X%

## Decision Basis
- Folgt [[adr-xxx]] – Architektur-Entscheidung
- Löst [[todo-xyz]] – TODO

## Migration Guide (bei Breaking Changes)
[Wie upgraden bestehende Nutzer/Systeme?]

## Rollout Plan (optional)
- [ ] Stage 1: ...
- [ ] Stage 2: ...

## Known Issues / Limitations
- [Wenn es bekannte Einschränkungen gibt]

## Related
- [[related-feature]]
- [[related-adr]]
- [[related-test]]

## References
- Commit: `abc123def`
- PR: #123
- Ticket: PROJ-456
```

---

## Wann ein Feature dokumentieren?

### ✅ Dokumentiere als Feature:

- **Neue Funktionalität:** Login-System, Export-Feature
- **Größere Refactorings:** Code-Strukturänderungen
- **Performance-Improvements:** Signifikante Optimierungen
- **Breaking Changes:** API-Änderungen, Migrations
- **Deprecations:** Features die entfernt werden
- **Security-Fixes:** (wenn nicht geheim)
- **UI-Changes:** Neue/geänderte User Interfaces

### ❌ Nicht als Feature dokumentieren:

- **Kleine Bug-Fixes:** Außer sie sind signifikant
- **Typos, Code-Cleanup:** Minimale Änderungen
- **Dependency-Updates:** Außer mit Breaking Changes
- **Config-Tweaks:** Außer sie ändern Verhalten

---

## Workflow

### 1. Planung

```bash
# Feature dokumentieren sobald klar was gebaut wird
autodocs/features/YYYY-MM-DD-feature-name.md

# Status: planned
# Related ADR verlinken (wenn vorhanden)
```

### 2. Entwicklung

```bash
# Status auf in-progress setzen
# Während Entwicklung: Tests verlinken
# Commits dokumentieren
```

### 3. Release

```bash
# Status auf released setzen
# Changelog-Entry erstellen (automatisch aus Commits)
# Known Issues dokumentieren
```

### 4. Deprecation (später)

```bash
# Wenn Feature veraltet: status: deprecated
# Successor-Feature verlinken
# Timeline für Removal setzen
```

---

## Verlinkung

### Von Feature zu anderen Docs

```markdown
## Decision Basis
Diese Implementierung folgt [[../adrs/adr-005-authentication-strategy]].

## Tests
- [[../tests/unit/auth-service]] – Unit-Tests
- [[../tests/e2e/login-flow]] – E2E-Tests

## Resolved TODOs
- [[../todo/implement-oauth]] – Abgeschlossen
```

### Von anderen Docs zu Feature

```markdown
# In ADR
## Implementation
Implementiert in [[../features/2025-01-15-user-authentication]].

# In Test
## Tested Feature
Testet [[../features/2025-01-15-user-authentication]].
```

---

## Tags-Konvention

- **Bereich:** `#feature`, `#auth`, `#ui`, `#api`, `#data`
- **Status:** `#status/released`, `#status/deprecated`
- **Komponenten:** `#cmp/auth-service`, `#cmp/user-model`
- **Breaking:** `#breaking-change`
- **Domäne:** `#dom/<bereich>`

---

## Breaking Changes

Wenn `breaking_change: true`:

### Pflicht-Zusätze
- **Migration Guide:** Wie upgraden?
- **Deprecation Notice:** Was wird entfernt?
- **Timeline:** Wann wird entfernt?
- **Changelog-Entry:** Prominent markieren

### Template für Breaking Change

```markdown
## ⚠️ Breaking Change

### Was ändert sich?
[Beschreibung]

### Migration Guide
**Vorher:**
```code
// Alte Implementierung
```

**Nachher:**
```code
// Neue Implementierung
```

### Timeline
- **2025-01-15:** Neue Version released (backward-compatible)
- **2025-03-15:** Deprecation Warning
- **2025-06-15:** Old API removed
```

---

## Beispiele

### Feature-Kategorien

1. **User-Facing Features**
   - `2025-01-20-add-export-to-pdf.md`
   - `2025-02-05-dark-mode-support.md`

2. **Developer-Facing Features**
   - `2025-01-18-add-graphql-api.md`
   - `2025-02-10-implement-caching-layer.md`

3. **Infrastructure Changes**
   - `2025-01-25-migrate-to-kubernetes.md`
   - `2025-03-01-add-monitoring-stack.md`

4. **Performance Improvements**
   - `2025-02-12-optimize-query-performance.md`
   - `2025-03-05-lazy-loading-images.md`

---

## Qualitätskriterien

Gute Feature-Doku hat:
- ✅ Klarer Titel & Beschreibung
- ✅ Motivation ist nachvollziehbar
- ✅ Tests sind verlinkt
- ✅ ADRs sind verlinkt (falls vorhanden)
- ✅ Commits/Tickets sind referenziert
- ✅ Breaking Changes sind klar markiert
- ✅ Tags sind konsistent

---

## Agent-Hinweise

### Automatisierbare Tasks

- **Feature-Stub-Generierung:** Aus Commits neue Feature-Docs generieren
- **Test-Coverage-Check:** Prüfen ob alle Features Tests haben
- **ADR-Link-Suggestion:** Automatisch passende ADRs vorschlagen
- **Changelog-Generierung:** Aus Feature-Docs Changelog erstellen
- **Status-Updates:** Aus Git/CI Status automatisch updaten

### Agent-Workflow für neue Features

```python
def document_feature(title, commits, area):
    # 1. Generiere Dateinamen
    date = datetime.now().strftime("%Y-%m-%d")
    filename = f"{date}-{slugify(title)}.md"
    
    # 2. Parse Commits für Kontext
    related_tests = find_tests_for_commits(commits)
    related_adrs = find_adrs_for_area(area)
    
    # 3. Nutze Template
    content = render_template('feature-template', {
        'title': title,
        'area': area,
        'commits': commits,
        'related_tests': related_tests,
        'related_adrs': related_adrs
    })
    
    # 4. Erstelle Datei
    write_file(f"autodocs/features/{filename}", content)
    
    # 5. Update Index
    update_feature_index(date, title)
    
    # 6. Update Changelog (wenn released)
    if status == 'released':
        update_changelog(title, commits)
    
    return filename
```

---

## For AI Agents: Feature-Specific Rules

**🤖 When you process this directory:**

### Validation Rules
```yaml
feature_validation:
  filename: "YYYY-MM-DD-kebab-case.md"
  frontmatter_required:
    - created: YYYY-MM-DD (must match filename date)
    - status: [planned, in-progress, released, deprecated]
    - area: string
    - breaking_change: boolean
    - related_tests: array (must have at least 1 if status=released)
    - related_adrs: array (if architecture change)
  
  structure_required:
    - "## Kurzbeschreibung"
    - "## Motivation"
    - "## Änderungen"
    - "## Tests"
```

### Auto-Fix Actions
```yaml
auto_fix:
  - extract_features_from_commits
  - link_to_tests_automatically
  - update_changelog_if_released
  - add_breaking_change_tag
  - link_to_relevant_adrs
  
sync_with_code:
  - scan: "src/**/*.{ts,tsx,js,jsx}"
  - detect_new_components: true
  - create_stubs_for_undocumented: true
  - link_test_files: "**/*.{test,spec}.{ts,js}"
```

### Bidirectional Linking
```yaml
enforce:
  feature_to_test: required_if_status_released
  test_to_feature: always_required
  feature_to_adr: required_if_architecture_change
  adr_to_feature: optional
```

### Workflow
1. Scan git commits for new features (conventional commits)
2. Create feature doc if missing
3. Link to tests bidirectionally
4. Update changelog.md if status=released
5. Verify breaking changes are marked

---

## Related

- [[index]] – Feature-Übersicht
- [[../index]] – Haupt-Navigation
- [[../templates/feature-template]] – Template für neue Features
- [[../adrs/_meta]] – Wie Features und ADRs zusammenhängen

[[index]]
