---
type: meta
created: 2026-03-17
tags: [meta, updater, maintenance]
---

# /autodocs/updater/_meta.md

## Zweck

Diese Collection enthält alle Betriebsdateien des **AutoDocs Updater Agents**. Sie werden automatisch vom Updater erzeugt und gepflegt — **niemals manuell bearbeiten**.

## Dateien

| Datei | Beschreibung | Schreibmodus |
|---|---|---|
| `state.json` | Persistenter State: letzter Commit-Hash, Branch, Zeitstempel | Überschreiben |
| `state_backup.json` | Backup vor jedem Lauf | Überschreiben |
| `log.md` | Append-only Log aller verarbeiteten Commits | Nur anhängen (oben) |
| `report.md` | Zusammenfassung des letzten Updater-Laufs | Überschreiben |

## Regeln

- **Keine manuellen Edits** — alle Dateien werden vom Updater verwaltet
- `log.md` ist **append-only**: neueste Einträge oben, bestehende Einträge nie löschen
- `report.md` wird bei jedem Lauf vollständig neu geschrieben
- `state_backup.json` dient der Wiederherstellung bei korruptem State
- Kein Frontmatter erforderlich für `state.json` und `state_backup.json` (JSON)
- `log.md` und `report.md` benötigen kein Obsidian-Frontmatter

## Nicht in diesem Ordner

Dokumentations-Inhalte (Features, Tests, ADRs) gehören **nicht** hierher — nur Updater-Betriebsdaten.
