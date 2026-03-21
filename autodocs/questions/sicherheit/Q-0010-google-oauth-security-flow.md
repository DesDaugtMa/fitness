---
id: Q-0010
type: question
category: sicherheit
status: open
priority: high
source_document: autodocs/blackbox/internal/google-oauth-outbound.md
source_code: Source/Fitness/Controllers/AccountController.cs
tags: [question, sicherheit, oauth2, auth, prio/high]
created: 2026-03-21
last_updated: 2026-03-21
related_docs:
  - "[[../../blackbox/internal/google-oauth-outbound]]"
---

# Q-0010: Warum wurde Google OAuth2 verwendet und wie sind Security-Controls geplant?

## Kontext
Das Dokument beschreibt Google OAuth2 flow, aber keine fertige Resilience-/Security-Politik.

## Frage
> ❓ Warum wurde Google OAuth2 gewählt, und welche Controls (Retry, Throttling, Claims-Validation) sind vorgesehen?

## Erwartete Antwortfelder
- Gründe für Google OAuth2
- Sicherheitsaspekte (Token-Handling, AuthCookies, OpenRedirect)
- Verbesserungsmöglichkeiten (Retry/Backoff, Monitoring)

## Quell-Kontext
**Ursprungsdokument:** [[../../blackbox/internal/google-oauth-outbound]]
**Fundstelle:** Gesamtes Dokument

## Verwandte Fragen
- [[../betrieb/Q-0009-sql-server-datastore-requirements]]

## Status
**Status:** closed
**Bearbeiter:** Alexander Friedrich
**Antwort:** Da viele Benutzer wahrscheinlich einen Google Account haben, um sich so einfacher einloggen zu können und keinen neuen Account erstellen zu müssen. Alle Controls können gerne noch umgesetzt werden.
**Abgeschlossen:** —

[[index]]
