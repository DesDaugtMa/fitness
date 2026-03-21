---
type: index
created: 2026-03-17
tags: [updater, maintenance, index]
---

# Updater — Betriebsdaten

Diese Collection enthält die automatisch generierten Betriebsdateien des AutoDocs Updater Agents.

## Dateien

| Datei | Status | Beschreibung |
|---|---|---|
| [[updater/state]] | auto-generated | Persistenter State (letzter Commit, Zeitstempel) |
| [[updater/state_backup]] | auto-generated | Backup des vorherigen States |
| [[updater/log]] | auto-generated | Append-only Log aller Updater-Läufe |
| [[updater/report]] | auto-generated | Bericht des letzten Updater-Laufs |

> **Hinweis:** Diese Dateien werden vom Updater Agent automatisch erstellt und gepflegt. Manuelle Änderungen werden beim nächsten Lauf überschrieben (außer `log.md` — der ist append-only).

## Letzter Lauf

Siehe [[updater/report]] für den aktuellen Status.

[[index]]
