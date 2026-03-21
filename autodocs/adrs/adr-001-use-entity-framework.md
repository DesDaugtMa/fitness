---
type: adr
number: 001
created: 2026-03-21
status: accepted
decision_date: 2026-03-21
area: data
tags: [adr, data, persistence]
related_features: []
related_tests: []
---

# ADR-001: Use Entity Framework for Data Access

## Status
accepted

## Kontext
Das Projekt benötigt eine ORM für Datenbankzugriff. Alternativen: Dapper, NHibernate.

## Entscheidung
Entity Framework Core für .NET 10.

## Alternativen
- Dapper: Schneller, aber mehr Boilerplate.

## Konsequenzen
Positiv: Produktivität. Negativ: Abhängigkeit von EF.

[[../adrs/index]]
