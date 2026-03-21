---
id: Q-0003
type: question
category: fachlich
status: open
priority: high
source_document: autodocs/features/2026-03-21-user-management.md
source_code: Source/Fitness/Controllers/AccountController.cs
tags: [question, fachlich, auth, user-management, prio/high]
created: 2026-03-21
last_updated: 2026-03-21
related_docs:
  - "[[../../features/2026-03-21-user-management]]"
---

# Q-0003: Warum wurde das User Management Feature so implementiert?

## Kontext
Das Feature-Dokument beschreibt User-Registrierung, Login, Rollen und die DB-Modelle. Es gibt noch keine Erklärung, warum diese Architektur als Standard gewählt wurde.

## Frage
> ❓ Warum wurde das User Management Feature genau so umgesetzt (EF, Claims, Registrierungstoken)? Welche fachlichen Anforderungen wurden dadurch erfüllt?

## Erwartete Antwortfelder
- Liste der Geschäftsanforderungen, die erfüllt werden
- Warum Entity Framework und Cookie-basiertes Auth verwendet werden
- Warum die Token-basierte Registrierung implementiert ist

## Quell-Kontext
**Ursprungsdokument:** [[../../features/2026-03-21-user-management]]
**Fundstelle:** Gesamtes Dokument

## Verwandte Fragen
- [[../technisch/Q-0006-fitness-webapp-api-design]]

## Status
**Status:** closed
**Bearbeiter:** Alexander Friedrich
**Antwort:** Einfach weil ich es so gelernt habe. Es ist mein Stil. Es ist natürlich möglich, das es so nicht der beste Weg ist.
**Abgeschlossen:** —

[[index]]
