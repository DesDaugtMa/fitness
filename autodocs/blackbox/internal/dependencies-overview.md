---
title: "Dependencies Overview"
created: 2026-03-21
type: dependency-overview
visibility: internal
tags: [blackbox, internal, dependencies, packages, runtime]
---

## API Frameworks
- `Microsoft.AspNetCore.App` (built-in ASP.NET Core MVC)
- `AspNet.Security.OAuth.Apple` 10.0.0

## Authentication
- `Microsoft.AspNetCore.Authentication.Cookies` (Core)
- `Microsoft.AspNetCore.Authentication.Google` 10.0.3

## Database / ORM
- `Microsoft.EntityFrameworkCore` 10.0.3
- `Microsoft.EntityFrameworkCore.SqlServer` 10.0.3
- `Microsoft.EntityFrameworkCore.Design` 10.0.3
- `Microsoft.EntityFrameworkCore.Tools` 10.0.3

## Project References
- `Fitness.DataAccess` (DbContext, Modelle, Migrations)

## Build / Codegen
- `Microsoft.VisualStudio.Web.CodeGeneration.Design` 10.0.2

## Known Risks
- Keine transiente HTTP-Client-Retry-Policy für OAuth2 (mittleres Risiko)
- Keine Rate Limiting / Throttling im Web-App Route (mittleres Risiko)

## License Notes
- Microsoft-Pakete unter MIT/Apache-like Lizenz.

## Related Documentation
- [[../internal/sql-server-datastore]]
- [[../internal/google-oauth-outbound]]
- [[../connections-overview]]
- [[../public/fitness-webapp-api]]

[[../index]]
