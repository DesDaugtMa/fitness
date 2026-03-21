---
title: "Fitness Database (SQL Server)"
created: 2026-03-21
type: datastore
visibility: internal
datastore_type: rdbms
technology: sql-server-2022
data_classification: pii
tags: [blackbox, internal, datastore, sql-server, rdbms, pii]
---

## Overview

Die Anwendung nutzt SQL Server als primären Persistent-Store für Benutzer-, Auth- und Workout-Daten. Entity Framework Core verwaltet ORM-Mappings.

## Technology

- SQL Server (TDS over TCP)
- Entity Framework Core 10.0.3
- DbContext: `FitnessDbContext`
- Connection String: `AppSettings.ConnectionStrings.Default` (in Config/appsettings.json / User Secrets)

## Schema Overview

Haupttabellen:
- `Users` (Id, Email, DisplayName, PasswordHash, RoleId, etc.)
- `Exercises` (Id, CreatorId, Name, Description, ImageId, etc.)
- `Images` (Blob-Daten, MimeType, UploaderId)
- `Roles`, `RegistrationTokens`, `Friendships`, etc.

## Access Patterns

- Reads: hoch (Login, Workout-Listen)
- Writes: mittel (User-Registrierung, Übungserstellung)
- Transaktionen: EF SaveChanges (insbesondere bei Insert+Relations)

## Data Classification

- PII: `Users.Email`, `Users.DisplayName`
- Sensitiv: `Users.PasswordHash`
- Internal: `WorkoutLog`, `TrainingPlan`

## Data Volume & Retention

- Schätzung: 10k+ Nutzer, 100k+ Workout-Records
- Retention: keine automatische Löschung im Code (manuell/cron möglich)

## Backup and Recovery

Nicht im Code verankert; wird extern über DB-Backup-Policies sichergestellt.

## Scalability

Aktuell Single-Instance, potenziell Read-Replicas möglich.

## Connection Details

- Connection String: `Server=<db-host>;Database=<db-name>;User Id=<user>;Password=<password>;` (Placeholder)
- Pool: Standard EF Core Pooling
- SSL: optimal via SQL Server TLS

## Security

- Encryption in transit empfohlen (TLS)
- Kontrollierte DB-Accounts und least privilege

## Source References

- `Source/Fitness.DataAccess/FitnessDbContext.cs`
- `Source/Fitness/Config/appsettings.json`
- `Source/Fitness/Config/appsettings.Development.json`

## Related Documentation

- Feature: [[2026-03-21-user-management]]
- ADR: [[adr-001-use-entity-framework]]
- Public: [[../public/connections-overview]]

[[../index]]
