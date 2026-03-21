---
id: Q-0009
type: question
category: betrieb
status: open
priority: high
source_document: autodocs/blackbox/internal/sql-server-datastore.md
source_code: Source/Fitness/DataAccess/FitnessDbContext.cs
tags: [question, betrieb, datastore, pii, prio/high]
created: 2026-03-21
last_updated: 2026-03-21
related_docs:
  - "[[../../blackbox/internal/sql-server-datastore]]"
---

# Q-0009: Warum wurde SQL Server als primärer Datastore gewählt und wie werden PII-Risiken adressiert?

## Kontext
Das Blackbox-Dokument spezifiziert SQL Server + Klassifizierungen, aber keine TDE/Encryptionslösung.

## Frage
> ❓ Warum wurde SQL Server ausgewählt und welche Operations-/Compliance-Maßnahmen gab es für PII + Backup/Recovery?

## Erwartete Antwortfelder
- Auswahlentscheidung gegenüber anderen DBs
- Backup/Recovery-Strategie
- Verschlüsselungs- und Zugriffsmaßnahmen

## Quell-Kontext
**Ursprungsdokument:** [[../../blackbox/internal/sql-server-datastore]]
**Fundstelle:** Gesamtes Dokument

## Verwandte Fragen
- [[../sicherheit/Q-0010-google-oauth-security-flow]]

## Status
**Status:** closed
**Bearbeiter:** Alexander Friedrich
**Antwort:** Da es unter Windows am leichtesten für mich umzusetzen war.
**Abgeschlossen:** —

[[index]]
