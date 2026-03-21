---
type: changelog
created: 2025-01-11
updated: 2024-11-14
tags: [docs, changelog, versioning]
---

# 📜 CHANGELOG

> **Note:** Dieser Changelog wird später automatisch aus [Conventional Commits](https://www.conventionalcommits.org/) generiert.

## Format

Wir folgen [Keep a Changelog](https://keepachangelog.com/de/1.0.0/) und [Semantic Versioning](https://semver.org/lang/de/).

### Typen von Änderungen

- **Added** – neue Features
- **Changed** – Änderungen an bestehender Funktionalität
- **Deprecated** – bald zu entfernende Features
- **Removed** – entfernte Features
- **Fixed** – Bugfixes
- **Security** – Sicherheits-Patches

---

## [Unreleased]

### Added

_Keine neuen Features._
  - Erste Feature-Dokumentation als Präzedenzfall
  - Demonstriert Workflow: Feature → ADR → (Tests pending)
  - Validiert Feature-Template in der Praxis
- Initiale Dokumentationsstruktur
- Agent-freundliches Meta-System
- Obsidian-kompatible Verlinkung
- Template-System für alle Dokumentationstypen

---

## [1.0.0] - 2025-01-11

### Added
- 🎉 Initiales Setup des Dokumentationssystems
- 📁 Ordnerstruktur mit ADRs, Features, Tests, TODOs
- 🏷️ Tag-Taxonomie für automatische Verlinkung
- 🤖 Agent-Workflows und Guides
- 📝 Template-System
- 🔗 Bi-direktionale Verlinkung (Obsidian-Style)

### Documentation
- Globale Konventionen in `_meta.md`
- Hauptindex mit Navigation
- Alle Bereichs-spezifischen `_meta.md` Dateien
- Agent-Workflow-Guide
- Tag-Taxonomie-Übersicht

---

## Versioning Schema

```
MAJOR.MINOR.PATCH

MAJOR: Breaking Changes in Struktur/Konventionen
MINOR: Neue Features, Bereiche, Templates
PATCH: Bugfixes, Verbesserungen, Klarstellungen
```

---

## Automation

Später wird dieser Changelog automatisch generiert via:

```bash
# Aus Git-Historie
conventional-changelog -p angular -i CHANGELOG.md -s

# Oder via GitHub Actions
# Siehe: [[ci/changelog-automation]]
```

---

## Related

- [[index]] – Hauptnavigation
- [[_meta]] – Konventionen
- [[guides/conventional-commits]] – Commit-Format-Guide

[[index]]
