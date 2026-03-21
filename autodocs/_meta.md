---
type: meta
created: 2025-01-11
version: 1.0.0
tags: [meta, docs, conventions, agent-guide]
---

# /autodocs/_meta.md – Globale Doku-Konventionen

## Zweck
Zentrale Regeln für dieses Doku-System – ein **Prototyp einer neuen, vernetzten Dokumentationsart**.
Es verknüpft Architektur-Entscheidungen (ADRs), Features, Tests und TODOs und ist für **Menschen und Agents** optimiert.

## Leitbild
- **Klarheit** vor Umfang: kurze, präzise Einträge mit Links
- **Nachvollziehbarkeit:** Jede Änderung ist test- und/oder entscheidungsgestützt
- **Vernetzung:** Backlinks & Tags ermöglichen Navigation und Automation
- **Agent-Kompatibilität:** Jede Ebene hat _meta-Regeln, damit Agents deterministisch arbeiten
- **Selbsterklärend:** Die Struktur dokumentiert sich selbst

## Pflicht-Konventionen

### Frontmatter (YAML)
Jede Datei **muss** YAML-Frontmatter enthalten:
```yaml
---
type: <adr|feature|test|todo|meta|index|guide>
created: YYYY-MM-DD
updated: YYYY-MM-DD  # optional
status: <siehe Typ-spezifische Meta>
tags: [tag1, tag2, ...]
---
```

### Datumsformat
Immer `YYYY-MM-DD` (ISO 8601)

### Backlinks
Jede Datei endet mit `[[index]]` oder dem passenden Parent-Link.

### Tags-Konvention
- **Kleinschreibung** immer
- **Komponenten:** `#cmp/<kebab-case-name>` (z.B. `#cmp/pdf-renderer`)
- **Domänen:** `#dom/<bereich>` (z.B. `#dom/translation`, `#dom/scoring`)
- **Tests:** `#test`, `#test/unit`, `#test/e2e`, `#test/integration`
- **Status:** `#status/planned`, `#status/active`, `#status/deprecated`
- **Priorität:** `#prio/high`, `#prio/medium`, `#prio/low`

### Verlinkung
- **Obsidian-Style:** `[[pfad/zum/file]]` oder `[[dateiname]]` (ohne `.md`)
- **Relative Links bevorzugt:** `[[../features/feature-name]]`
- **Cross-References:** Nutze `## Related` Sections am Ende jeder Datei

### Dateinamen
- **Kebab-case** für alle Dateinamen (außer `_meta.md`, `index.md`)
- **Präfixe:** `adr-XXX-`, `YYYY-MM-DD-` wo spezifiziert
- **Sprechend:** Namen sollen Inhalt beschreiben

## Änderungen & Governance

### ADRs
- **Niemals löschen**, nur `status: superseded`
- Bei Änderung: neue ADR erstellen, alte referenzieren

### _meta.md Dateien
- Nur via ADR ändern
- Versionieren bei Breaking Changes

### Changelog
- Automatisch aus Conventional Commits generiert
- Format: [Keep a Changelog](https://keepachangelog.com/)

### Conventional Commits
Standard für alle Commits:
- `feat:` – neue Features
- `fix:` – Bugfixes
- `docs:` – Dokumentation
- `refactor:` – Code-Umstrukturierung
- `test:` – Tests
- `chore:` – Tooling, Build

## Agent-Hinweise

### Workflow für Agents
1. **Immer zuerst:** Lies alle `_meta.md` Dateien im relevanten Bereich
2. **Frontmatter prüfen:** Validiere vor dem Schreiben
3. **Tags generieren:** Automatisch aus Kontext ableiten
4. **Links setzen:** Bidirektionale Verlinkung zwischen verwandten Docs
5. **Templates nutzen:** Siehe `[[templates/_meta]]`

### Automatisierbare Aufgaben
- Changelog-Generierung aus Commits
- Coverage-Report-Updates
- Broken-Link-Checks
- Tag-Konsistenz-Validierung
- Cross-Reference-Graphen

### Entscheidungsbaum für neue Dokumentation
```
Neue Information?
├─ Architekturentscheidung? → ADR
├─ Implementierte Änderung? → Feature
├─ Test-Dokumentation? → Tests
├─ Offene Aufgabe/Idee? → TODO
├─ Domänen-Wissen? → Domain
└─ Prozess/Workflow? → Guides
```

## Sicherheit
- **Keine Secrets** in Dokumentation
- **Keine Credentials** committen
- Verweis auf sichere Speicherorte (z.B. `.env.example`)

## Qualitätskriterien
- ✅ Jede Datei hat valides Frontmatter
- ✅ Alle Links sind gültig
- ✅ Tags folgen Konvention
- ✅ Mindestens ein Backlink
- ✅ Datum im ISO-Format

## For AI Agents: Auto-Validation & Execution

**🤖 When you read this file, you understand the entire system.**

### Entry Point
This is your primary entry point. Read this file first to understand all conventions.

### Validation Rules (Auto-Execute)
```yaml
validation:
  frontmatter:
    - every_file_must_have: [type, created, tags]
    - date_format: YYYY-MM-DD
    - tags_lowercase: true
  
  structure:
    - all_files_end_with: "[[index]] or [[../index]]"
    - required_directories: [adrs, features, tests, todo, templates, guides]
    - each_directory_has: [_meta.md, index.md]
  
  links:
    - no_broken_wikilinks: true
    - relative_paths_only: true
  
  naming:
    - adrs: "adr-XXX-kebab-case.md"
    - features: "YYYY-MM-DD-kebab-case.md"
    - kebab_case_everywhere: true
```

### Auto-Fix Policy (Execute These)
```yaml
auto_fix:
  safe:  # Apply immediately
    - add_missing_frontmatter_fields
    - normalize_tags_to_lowercase
    - add_missing_backlinks
    - fix_date_format_to_iso
  
  review_required:  # Log for human review
    - create_new_directories
    - delete_files
  
  forbidden:  # Never do
    - change_user_content
    - modify_adr_decisions
    - delete_meta_files
```

### Workflow
1. Read this file (you are here)
2. Read each `_meta.md` in subdirectories
3. Validate all files against rules above
4. Apply safe auto-fixes
5. Report findings to `/autodocs/review_report.md`
6. Log changes to `/autodocs/review_history.log`

### Next Steps
- Read `[[index]]` for navigation overview
- Read `[[guides/agent-workflow]]` for detailed agent instructions
- Read subdirectory `_meta.md` files for specific rules

---

## Related
- [[index]] – Haupt-Navigation
- [[templates/_meta]] – Wiederverwendbare Templates
- [[guides/agent-workflow]] – Detaillierter Agent-Guide
- [[guides/tag-taxonomy]] – Vollständige Tag-Übersicht

---
*Diese Datei ist der Kern des Dokumentationssystems. Änderungen nur via ADR!*
