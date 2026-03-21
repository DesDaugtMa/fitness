---
id: Q-0006
type: question
category: technisch
status: open
priority: medium
source_document: autodocs/blackbox/public/fitness-webapp-api.md
source_code: Source/Fitness/Controllers/AccountController.cs
tags: [question, technisch, api, auth, prio/medium]
created: 2026-03-21
last_updated: 2026-03-21
related_docs:
  - "[[../../blackbox/public/fitness-webapp-api]]"
---

# Q-0006: Warum wurde die WebApp API als MVC-Form-basierte Schnittstelle implementiert?

## Kontext
Das Blackbox-Doc beschreibt viele MVC-Endpunkte, keine reine API-Versionierung und keine JSON-first API.

## Frage
> ❓ Warum wurde MVC Form-Post/GET + Cookie-Auth statt API-first/SPA-Architektur gewählt?

## Erwartete Antwortfelder
- Legacy-/Team-Gründe
- Sicherheit/CSRF-Handling
- Datenformat-Entscheidungen

## Quell-Kontext
**Ursprungsdokument:** [[../../blackbox/public/fitness-webapp-api]]
**Fundstelle:** Gesamtes Dokument

## Verwandte Fragen
- [[../betrieb/Q-0009-sql-server-datastore-requirements]]

## Status
**Status:** closed
**Bearbeiter:** Alexander Friedrich
**Antwort:** Da es so von ASP.NET und EF Core mit dem Codegenerator erstellt wurde.
**Abgeschlossen:** —

[[index]]
