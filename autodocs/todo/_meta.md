---
type: meta
created: 2025-01-11
tags: [meta, todo, backlog, tech-debt]
---

# /autodocs/todo/_meta.md – TODOs & Backlog

## Zweck

TODOs dokumentieren **offene Aufgaben**:
- Tech Debt (Refactoring, Cleanup)
- Feature-Ideen (noch nicht spruchreif)
- Bugs (priorisiert)
- Improvements (Nice-to-Have)
- Research-Topics (Spikes)

TODOs sind **temporär** – sie werden zu Features/ADRs oder verworfen.

---

## Dateinamen-Konvention

```
todo-kurzer-titel.md
```

- **Beispiele:**
  - `todo-refactor-auth-layer.md`
  - `todo-add-dark-mode.md`
  - `todo-research-graphql.md`

---

## Pflicht-Frontmatter

```yaml
---
type: todo
created: YYYY-MM-DD
updated: YYYY-MM-DD
status: open | in-progress | completed | cancelled
priority: high | medium | low
category: tech-debt | enhancement | bug | research
area: <bereich>
tags: [todo, ...]
estimated_effort: <hours/days>  # optional
assigned_to: <person>           # optional
related_adrs: []                # falls entscheidungsrelevant
resolved_by: []                 # Feature/ADR die TODO löst
---
```

### Status-Definitionen

| Status | Bedeutung |
|--------|-----------|
| `open` | Noch nicht begonnen |
| `in-progress` | In Arbeit |
| `completed` | Erledigt (via Feature/ADR) |
| `cancelled` | Nicht mehr relevant |

### Category-Definitionen

| Category | Bedeutung |
|----------|-----------|
| `tech-debt` | Code-Qualität, Refactoring |
| `enhancement` | Verbesserung bestehender Features |
| `bug` | Fehler beheben |
| `research` | Spike, Investigation |

---

## Pflicht-Struktur

```markdown
# TODO: Titel

## Problem/Idee
[Was ist das Problem oder die Idee?]

## Motivation
[Warum sollten wir das machen?]

## Proposed Solution (optional)
[Grober Lösungsansatz]

## Acceptance Criteria
- [ ] Kriterium 1
- [ ] Kriterium 2

## Effort Estimation
- **Time:** X hours/days
- **Complexity:** Low/Medium/High

## Dependencies
- Abhängig von: [[other-todo]]
- Blockiert: [[feature-xyz]]

## Notes
[Weitere Gedanken, Links, Referenzen]

## Resolution (bei completed/cancelled)
- Gelöst durch: [[feature-YYYY-MM-DD-title]]
- Grund für Cancel: [...]

## Related
- [[related-todo]]
- [[related-adr]]
```

---

## Workflow

### 1. TODO erstellen

```bash
# Neue Idee/Problem
autodocs/todo/todo-meine-idee.md

# Status: open
# Priority setzen
```

### 2. Priorisieren

```bash
# Wöchentliches TODO-Review
# Priority/Effort aktualisieren
# Grooming
```

### 3. Umsetzen

```bash
# Status: in-progress
# Feature-Doc/ADR erstellen
# Implementieren
```

### 4. Abschließen

```bash
# Status: completed
# resolved_by: [[feature-xyz]] setzen
# TODO bleibt als Historie
```

---

## Priorisierung

### High Priority 🔴
- Sicherheitslücken
- Kritische Bugs
- Blocker für Features

### Medium Priority 🟡
- Tech Debt mit Impact
- Nice-to-Have Features
- Performance-Improvements

### Low Priority 🟢
- Cosmetic Issues
- Future Ideas
- Research ohne Dringlichkeit

---

## Tech Debt Management

### Definition
Code/Architektur-Entscheidungen die **kurzfristig ok**, aber **langfristig problematisch** sind.

### Tracking
```markdown
## Tech Debt Score
- **Impact:** High/Medium/Low (Was passiert wenn wir es nicht fixen?)
- **Effort:** High/Medium/Low (Wie aufwändig ist die Lösung?)
- **Urgency:** High/Medium/Low (Wie dringlich?)

→ Priorität = Funktion(Impact, Effort, Urgency)
```

---

## Tags-Konvention

- **Kategorie:** `#todo`, `#tech-debt`, `#enhancement`, `#bug`, `#research`
- **Priorität:** `#prio/high`, `#prio/medium`, `#prio/low`
- **Status:** `#status/open`, `#status/completed`
- **Bereich:** `#dom/<bereich>`, `#cmp/<component>`

---

## Qualitätskriterien

Gutes TODO hat:
- ✅ Klare Problem-/Ideen-Beschreibung
- ✅ Motivation ist nachvollziehbar
- ✅ Priority ist gesetzt
- ✅ Acceptance Criteria definiert
- ✅ Effort ist geschätzt
- ✅ Bei Completion: Resolution dokumentiert

---

## Agent-Hinweise

### Automatisierbare Tasks

- **Stale-TODO-Detection:** Alte TODOs ohne Update
- **Priority-Suggestion:** Basierend auf Impact/Effort
- **Auto-Linking:** TODOs zu Features/ADRs verlinken
- **Completion-Tracking:** Status-Updates aus Features
- **Burndown-Charts:** TODO-Progress visualisieren

### Agent-Workflow

```python
def create_todo(title, problem, priority, category):
    # 1. Generiere Dateinamen
    filename = f"todo-{slugify(title)}.md"
    
    # 2. Suche ähnliche TODOs (Duplikate?)
    similar = find_similar_todos(title, problem)
    
    # 3. Nutze Template
    content = render_template('todo-template', {
        'title': title,
        'problem': problem,
        'priority': priority,
        'category': category
    })
    
    # 4. Erstelle Datei
    write_file(f"autodocs/todo/{filename}", content)
    
    # 5. Update Index
    update_todo_index(priority, category)
    
    return filename
```

---

## For AI Agents: TODO-Specific Rules

**🤖 When you process this directory:**

### Validation Rules
```yaml
todo_validation:
  filename: "todo-kebab-case.md"
  frontmatter_required:
    - status: [open, in-progress, completed, cancelled]
    - priority: [high, medium, low]
    - category: [tech-debt, enhancement, bug, research]
    - area: string
  
  lifecycle:
    - if_status_completed: must_have_resolved_by
    - if_older_than_90days_and_open: mark_as_stale
```

### Auto-Fix Actions
```yaml
auto_fix:
  - extract_todos_from_code_comments
  - detect_stale_todos_gt_90days
  - link_completed_to_features
  - prioritize_by_severity
  
code_smell_detection:
  scan: "src/**/*.{ts,tsx,js,jsx}"
  triggers:
    - function_length_gt_50: priority_medium
    - cyclomatic_complexity_gt_10: priority_high
    - missing_tests_coverage_lt_80: priority_medium
    - todo_fixme_comments: priority_low
```

### Lifecycle Management
```yaml
workflow:
  - detect_completed_todos
  - check_if_linked_to_feature_or_adr
  - if_not_linked: warn_user
  - if_stale: suggest_cancellation_or_update
```

### Code-Sync Actions
```yaml
extract_from_code:
  patterns:
    - "// TODO:"
    - "// FIXME:"
    - "/* TODO:"
  
  actions:
    - create_todo_doc
    - link_to_source_file
    - set_priority_from_context
```

### Workflow
1. Scan codebase for TODO/FIXME comments
2. Detect code smells (long functions, high complexity)
3. Create TODO docs for each
4. Check for stale TODOs (>90 days)
5. Verify completed TODOs link to features
6. Archive completed & linked TODOs

---

## Related

- [[index]] – TODO-Übersicht
- [[../templates/todo-template]] – Template
- [[../features/_meta]] – Workflow TODO → Feature
- [[../index]] – Haupt-Navigation

[[index]]
