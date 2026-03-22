---
type: adr
number: 001
created: 2026-03-21
updated: 2026-03-22
status: accepted
decision_date: 2026-03-21
area: data
tags: [adr, data, persistence, cmp/entity-framework, dom/persistence, status/active]
related_features: [2026-03-21-user-management, 2026-03-21-workout-tracking, 2026-03-21-admin-features, 2026-03-22-image-management]
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

## Affected Features

This decision impacts all data persistence across the following features:

- [[../features/2026-03-21-user-management]] – User, Role, RegistrationToken, Friendship models
- [[../features/2026-03-21-workout-tracking]] – Exercise, TrainingPlan, WorkoutSession, WorkoutLog models
- [[../features/2026-03-21-admin-features]] – RegistrationToken management
- [[../features/2026-03-22-image-management]] – Image blob storage

## Implementation Details

- **DbContext:** FitnessDbContext (Fitness.DataAccess)
- **Fluent API:** Used for complex relationships (e.g., Friendship composite keys)
- **Migrations:** SQL Server database schema versioning
- **Target Framework:** .NET 10

[[../adrs/index]]
