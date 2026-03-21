---
title: "Redaction Log"
created: 2026-03-21
type: redaction-log
visibility: internal
tags: [blackbox, internal, security, redaction, meta]
---

### RED-001
- **Original Location**: `Source/Fitness/Config/appsettings.json` – Datenbank-Verbindung und Google-Credentials.
- **Redacted Info Type**: secret
- **Original (Beschreibung)**: Verbindung zu SQL Server (`Default`), `ClientId`, `ClientSecret`.
- **Placeholder Used**: `Server=<db-host>;Database=<db-name>;User Id=<user>;Password=<password>;` / `GoogleAuthSettings:ClientId=<client-id>, ClientSecret=<client-secret>`
- **Reason**: security
- **Affected Documents**: [[../internal/sql-server-datastore]], [[../internal/google-oauth-outbound]]
- **Date**: 2026-03-21

### RED-002
- **Original Location**: `Source/Fitness/Config/appsettings.Production.json` – Produktions-Flags.
- **Redacted Info Type**: environment-specific
- **Original (Beschreibung)**: `AllowedHosts`, `Logging`.
- **Placeholder Used**: `AllowedHosts=*</>`
- **Reason**: security
- **Affected Documents**: [[../internal/sql-server-datastore]]
- **Date**: 2026-03-21

[[../index]]
