---
type: meta
created: 2025-11-13
version: 1.0.0
tags: [meta, docs, conventions, guides]
---

# /autodocs/guides/_meta.md – How-To-Guides und Best Practices

## Zweck

Sammlung von How-To-Guides, Best Practices und Anleitungen für verschiedene Aspekte des Projekts. Diese Guides helfen bei der Einarbeitung neuer Team-Mitglieder und dokumentieren wiederkehrende Workflows und bewährte Vorgehensweisen.

## Struktur

- **tag-taxonomy.md** - Vollständige Übersicht aller Tags und ihrer Bedeutungen
- **agent-workflow.md** - Workflow für AI-Agents zur Dokumentationspflege
- **development.md** - Entwicklungs-Guidelines
- **testing.md** - Test-Guidelines
- **deployment.md** - Deployment-Guidelines
- **index.md** - Übersicht aller Guides

## Pflicht-Konventionen

### Frontmatter (YAML)

Jede Datei **muss** folgendes Frontmatter enthalten:
```yaml
---
type: guide
created: YYYY-MM-DD
updated: YYYY-MM-DD  # optional
audience: developers|ops|product|all
status: draft|accepted|deprecated
tags: [tag1, tag2, ...]
---
```

### Spezifische Tags

- **Zielgruppe:** `#audience/developers`, `#audience/ops`, `#audience/product`
- **Bereich:** `#area/development`, `#area/testing`, `#area/deployment`
- **Schwierigkeitsgrad:** `#level/beginner`, `#level/intermediate`, `#level/advanced`

### Format

- **Klare Schritte** - Nummerierte Listen für sequentielle Schritte
- **Beispiele** - Praxisnahe Code-Beispiele
- **Troubleshooting** - Häufige Probleme und Lösungen

## Required Sections

1. **Introduction** - Kurze Einführung und Zweck des Guides
2. **Prerequisites** - Voraussetzungen
3. **Step-by-Step Guide** - Detaillierte Anleitung
4. **Examples** - Praxisbeispiele
5. **Troubleshooting** - Häufige Probleme und Lösungen
6. **Related Documentation** - Links zu verwandten Dokumenten

## Verlinkungen

- Jeder Guide sollte mit relevanten Features, ADRs oder Tests verlinkt sein
- Die tag-taxonomy.md sollte eine vollständige Übersicht aller Tags bieten
- Jeder Guide sollte auf relevante externe Ressourcen verweisen

---

## Related

- [[index]] – Guides-Übersicht
- [[../index]] – Haupt-Navigation
- [[../templates/_meta]] – Templates für neue Dokumente
