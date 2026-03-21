---
id: Q-0004
type: question
category: fachlich
status: open
priority: high
source_document: autodocs/features/2026-03-21-workout-tracking.md
source_code: Source/Fitness/Controllers/ExercisesController.cs
tags: [question, fachlich, workout, tracking, prio/high]
created: 2026-03-21
last_updated: 2026-03-21
related_docs:
  - "[[../../features/2026-03-21-workout-tracking]]"
---

# Q-0004: Warum wurde das Workout Tracking Feature so implementiert?

## Kontext
Workout Tracking umfasst Übungen, Sessions, Pläne und Logs. Die Implementierung nutzt EF-Modelle + MVC-Formular-Flows.

## Frage
> ❓ Warum wurde Workout Tracking als bestehende Architektur mit MVC-Forms und EF umgesetzt? Welche Nutzerprobleme zielt es ab?

## Erwartete Antwortfelder
- Ziel-Nutzer-Szenarien
- Gründe für DB-Struktur / Model-Design
- Trade-offs gegen andere Ansätze (WebAPI, SPA)

## Quell-Kontext
**Ursprungsdokument:** [[../../features/2026-03-21-workout-tracking]]
**Fundstelle:** Gesamtes Dokument

## Verwandte Fragen
- [[../technisch/Q-0007-system-connections-intent]]

## Status
**Status:** closed
**Bearbeiter:** Alexander Friedrich
**Antwort:** Weil ich kein Angular-Native bin. Ich habe es so umgesetzt, da ich mich in ASP.NET gut auskenne.
**Abgeschlossen:** —

[[index]]
