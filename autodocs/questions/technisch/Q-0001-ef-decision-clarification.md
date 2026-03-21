---
id: Q-0001
type: question
category: technisch
status: open
priority: high
source_document: autodocs/adrs/adr-001-use-entity-framework.md
source_code: Source/Fitness.DataAccess/FitnessDbContext.cs
tags: [question, technisch, ef, db, prio/high]
created: 2026-03-21
last_updated: 2026-03-21
related_docs:
  - "[[../../adrs/adr-001-use-entity-framework]]"
---

# Q-0001: Warum wurde Entity Framework für Datenzugriff als ADR gewählt?

## Kontext
Die ADR beschreibt die Wahl von EF Core gegenüber Dapper und NHibernate.

## Frage
> ❓ Warum wurde Entity Framework Core statt alternativer ORMs gewählt? Welche Anforderungen und Alternativen wurden gewichtet?

## Erwartete Antwortfelder
- Performance-/Wartbarkeitserwägungen
- Entwicklerkenntnisse und Teamkompetenzen
- Migrations- und Technologie-Strategie

## Quell-Kontext
**Ursprungsdokument:** [[../../adrs/adr-001-use-entity-framework]]
**Fundstelle:** Gesamtes Dokument

## Verwandte Fragen
- [[../technisch/Q-0011-dependencies-overview-question]]

## Status
**Status:** closed
**Bearbeiter:** Alexander Friedrich
**Antwort:** EF Core wurde gewählt, da ich mich als Entwickler am besten damit auskenne. Es ist hauptsächlich aus Bequemlichkeit im Microsoft-Umfeld von mir gewählt worden.
**Abgeschlossen:** —

[[index]]
