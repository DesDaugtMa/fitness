---
type: adr
number: 001
created: 2025-01-11
decision_date: 2025-01-11
updated: 2025-01-11
status: accepted
area: documentation
tags: [adr, documentation, meta, obsidian, agent-friendly]
related_features: []
related_tests: []
supersedes: []
superseded_by: []
---

# ADR-001: Obsidian-Compatible Agent-Friendly Documentation System

## Status

✅ **Accepted** – 2025-01-11

This is a meta-ADR: It documents the decision to use this documentation system.

---

## Kontext

### Problem
Code-Projekte benötigen Dokumentation, die:
- **Für Menschen lesbar** ist (Entwickler, neue Teammitglieder)
- **Für Agents/LLMs verständlich** ist (automatische Code-Generierung, Refactoring)
- **Skalierbar** ist (von kleinen bis zu großen Projekten)
- **Vernetzt** ist (Architektur ↔ Features ↔ Tests ↔ TODOs)
- **In verschiedene Projekte kopierbar** ist (Wiederverwendbarkeit)

### Bisherige Ansätze & Probleme

#### Klassische README-Only-Ansätze
- ❌ Monolithische README.md wird schnell unübersichtlich
- ❌ Keine Strukturierung von Entscheidungen vs. Features vs. Tests
- ❌ Schwer navigierbar
- ❌ Keine Verlinkung zwischen Dokumenten

#### Wiki-Systeme (Confluence, Notion)
- ❌ Externes Tool erforderlich
- ❌ Nicht im Git-Repository versioniert
- ❌ Nicht Obsidian-kompatibel
- ❌ Agents können nicht direkt zugreifen

#### Docusaurus / MkDocs
- ❌ Komplex zu setup und maintainen
- ❌ Overkill für viele Projekte
- ❌ Fokus auf "Produktdokumentation", nicht "Entwicklerdokumentation"
- ❌ Keine native Obsidian-Kompatibilität

#### Docstrings / Inline-Code-Comments
- ❌ Architekturentscheidungen nicht dokumentiert
- ❌ Cross-Cutting-Concerns schwer darstellbar
- ❌ Keine Verlinkung zwischen Code-Bereichen

### Constraints
- **Git-basiert:** Alles im Repository, versioniert
- **Plain Markdown:** Keine proprietären Formate
- **Obsidian-kompatibel:** YAML-Frontmatter, `[[Wikilinks]]`, `#tags`
- **Agent-freundlich:** Klare Struktur, parsbare Metadaten
- **Zero-Setup:** Sofort nutzbar ohne Build-Tools
- **Kopierbar:** Struktur in jedes Projekt übertragbar

---

## Entscheidung

Wir nutzen ein **strukturiertes `/autodocs/`-Verzeichnis** mit:

### 1. Ordnerstruktur nach Dokumentationstyp

```
/autodocs/
├── _meta.md              # Globale Konventionen
├── index.md              # Entry-Point
├── changelog.md          # Versions-Historie
├── /adrs/                # Architecture Decision Records
├── /features/            # Implementierte Features
├── /tests/               # Test-Dokumentation
│   ├── /unit/
│   └── /e2e/
├── /todo/                # Backlog & Tech Debt
├── /domain/              # Domänen-Wissen
├── /ui/                  # UI/UX
├── /ci/                  # CI/CD
├── /agents/              # Agent-Konfigurationen
├── /templates/           # Wiederverwendbare Templates
└── /guides/              # How-To Guides
```

### 2. Meta-System mit `_meta.md` pro Ordner

Jeder Ordner hat eine `_meta.md` Datei, die definiert:
- **Zweck** des Ordners
- **Dateinamen-Konventionen**
- **Pflicht-Frontmatter**
- **Mindestinhalt**
- **Verlinkungsregeln**
- **Tags-Konvention**
- **Agent-Workflows**

### 3. YAML-Frontmatter in jeder Datei

```yaml
---
type: <adr|feature|test|todo|meta|guide>
created: YYYY-MM-DD
updated: YYYY-MM-DD
status: <typ-spezifisch>
tags: [tag1, tag2, ...]
---
```

### 4. Obsidian-Style-Verlinkung

- **Wikilinks:** `[[pfad/zur/datei]]` oder `[[dateiname]]`
- **Tags:** `#tag`, `#cmp/component-name`, `#dom/domain`
- **Backlinks:** Jede Datei linkt zurück zum Parent

### 5. Tag-Taxonomie für automatische Verknüpfung

- **Komponenten:** `#cmp/<name>`
- **Domänen:** `#dom/<bereich>`
- **Tests:** `#test/unit`, `#test/e2e`
- **Status:** `#status/active`, `#status/deprecated`
- **Priorität:** `#prio/high`, `#prio/medium`, `#prio/low`

### 6. Bi-direktionale Verlinkung

- Features referenzieren ADRs: "Diese Implementierung folgt [[adr-001]]"
- ADRs referenzieren Features: "Implementiert in [[feature-xyz]]"
- Tests referenzieren Features: "Testet [[feature-xyz]]"
- TODOs werden zu Features: "Umgesetzt in [[feature-abc]]"

### 7. Template-System

Wiederverwendbare Templates in `/autodocs/templates/` für:
- ADRs
- Features
- Tests
- TODOs
- Guides

---

## Alternativen

### Alternative 1: Docusaurus
**Pro:**
- Schöne Web-UI
- Suche, Navigation, Versionierung out-of-the-box

**Contra:**
- Komplexer Setup
- Build-Step erforderlich
- Nicht Obsidian-kompatibel
- Overkill für interne Doku

**Warum nicht gewählt:**  
Zu komplex für den Anwendungsfall. Wir brauchen keine öffentliche Produktdoku, sondern interne Entwicklerdoku.

### Alternative 2: Confluence/Notion
**Pro:**
- Kollaboration-Features
- WYSIWYG-Editor
- Kommentare, Mentions

**Contra:**
- Nicht im Git
- Kostenpflichtig
- Lock-in
- Agents können nicht direkt zugreifen

**Warum nicht gewählt:**  
Externe Abhängigkeit, nicht git-basiert, nicht agent-friendly.

### Alternative 3: README.md + ADR-Folder (minimalistisch)
**Pro:**
- Einfach
- Schnell
- Keine zusätzliche Struktur

**Contra:**
- Keine Struktur für Features, Tests, TODOs
- Keine Verlinkung
- Nicht skalierbar

**Warum nicht gewählt:**  
Funktioniert nur für kleine Projekte. Bei Wachstum wird es chaotisch.

### Alternative 4: Nur Docstrings + Generated API Docs
**Pro:**
- Code und Doku nah beieinander
- Automatisch generierbar

**Contra:**
- Keine Architekturentscheidungen
- Keine Cross-Cutting-Concerns
- Keine Verlinkung zwischen Konzepten

**Warum nicht gewählt:**  
Docstrings sind wichtig, aber nicht ausreichend für Architektur-Doku.

---

## Konsequenzen

### Positiv ✅

#### Für Menschen
- **Klare Navigation:** Jeder weiß, wo welche Info liegt
- **Obsidian-Power:** Graph-View, Backlinks, Search
- **Git-versioniert:** Doku und Code in Sync
- **Kein Lock-in:** Plain Markdown, überall nutzbar

#### Für Agents
- **Strukturiert parsbar:** YAML-Frontmatter, klare Ordner
- **Selbsterklärend:** `_meta.md` definiert Regeln
- **Automatisierbar:** Changelog, Coverage, Link-Checks
- **Deterministisch:** Klare Konventionen → vorhersagbares Verhalten

#### Für Projekte
- **Skalierbar:** Von klein bis groß nutzbar
- **Kopierbar:** Struktur in jedes Projekt übertragbar
- **Zero-Setup:** Sofort nutzbar
- **Zukunftssicher:** Plain Markdown lebt ewig

### Negativ ❌

#### Initiale Lernkurve
- Entwickler müssen Konventionen lernen
- **Mitigation:** Gute Templates, Guides, Examples

#### Maintenance-Aufwand
- `_meta.md` müssen aktuell gehalten werden
- Verlinkungen können brechen
- **Mitigation:** Automatische Broken-Link-Checks, Linter

#### Keine Build-in-Features
- Keine automatische Suche (außer in Obsidian)
- Keine Versionierungs-UI
- **Mitigation:** Obsidian als optionaler Client, Git für Versionierung

#### Obsidian nicht Pflicht, aber empfohlen
- System funktioniert auch ohne Obsidian
- Aber: Graph-View, Backlinks nur in Obsidian
- **Mitigation:** Dokumentation ist auch ohne Obsidian nutzbar (plain text)

### Neutral 🔄

#### Markdown-Fokus
- Diagramme via Mermaid (text-basiert)
- Keine fancy Visualisierungen
- Trade-off: Einfachheit vs. Fancy-UI

#### Entwickler-Verantwortung
- Jeder muss dokumentieren
- Keine automatische Generierung (yet)
- Trade-off: Manuelle Arbeit vs. Qualität

---

## Umsetzung

### Phase 1: Foundation ✅
- [x] Ordnerstruktur anlegen
- [x] `_meta.md` für jeden Bereich
- [x] Templates erstellen
- [x] Beispiel-Dateien (diese ADR!)

### Phase 2: Adoption (laufend)
- [ ] Team-Training
- [ ] Erste Features dokumentieren
- [ ] Erste Tests dokumentieren
- [ ] TODOs migrieren

### Phase 3: Automation (geplant)
- [ ] Changelog aus Commits generieren
- [ ] Coverage-Reports automatisch updaten
- [ ] Broken-Link-Checker
- [ ] Tag-Konsistenz-Linter

---

## Compliance & Validation

### Success Metrics
- ✅ Alle neuen Features haben Feature-Docs
- ✅ Alle ADRs sind vollständig ausgefüllt
- ✅ Test-Coverage ist dokumentiert
- ✅ Keine Broken Links
- ✅ Tags sind konsistent

### Review-Prozess
- **Bei PR:** Prüfe ob relevante Docs aktualisiert wurden
- **Monatlich:** Review von TODOs und veralteten Docs
- **Quarterly:** ADR-Audit (sind sie noch aktuell?)

---

## Related

- [[../index]] – Hauptnavigation
- [[../features/]] – Zukünftige Feature-Implementierungen
- [[../guides/agent-workflow]] – Wie Agents mit diesem System arbeiten
- [[../templates/adr-template]] – Template für neue ADRs

---

## References

- [Architecture Decision Records (ADRs)](https://adr.github.io/)
- [Obsidian](https://obsidian.md/)
- [Keep a Changelog](https://keepachangelog.com/)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Zettelkasten Method](https://zettelkasten.de/)

---

**Decision Maker:** System Designer  
**Stakeholders:** Developers, Agents, Future Maintainers

[[../index]]
