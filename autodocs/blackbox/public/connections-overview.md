---
title: "System Connections Overview"
created: 2026-03-21
type: overview
visibility: public
tags: [blackbox, public, connections, dependencies, architecture, overview]
---

## C4 Context Diagram

```mermaid
C4Context
    title Fitness App Context
    Enterprise_Boundary(b1, "Fitness App") {
        Person(browser, "User", "Endanwender (Webbrowser)")
        System(webapp, "Fitness WebApp", "ASP.NET Core MVC")
    }
    System_Ext(gAuth, "Google OAuth2", "OAuth2 Provider")
    System_Ext(db, "SQL Server", "Relationale Datenbank")

    Rel(browser, webapp, "HTTPS (Form-POST/GET)")
    Rel(webapp, gAuth, "OAuth2 Authorization Code")
    Rel(webapp, db, "Entity Framework Core / SQL", "TDS / TCP")
```

## Inbound Connections

| Source | Interface | Protocol | Zweck |
|--------|-----------|----------|-------|
| Browser | Web App MVC Endpoints | HTTPS | Nutzer-Authentifikation, Workout-Management, Profile |

## Outbound Connections

| Target System | Protocol | Zweck | Pattern |
|---------------|----------|-------|---------|
| SQL Server (Datenbank) | TDS (TCP) | Persistenz von Users/Exercises/Plans | Datastore
| Google OAuth2 | HTTPS (OAuth2) | Social Login | OAuth2

## Datastore Übersicht

| Datastore | Typ | Zweck | Data Classification |
|-----------|-----|-------|---------------------|
| Fitness Database | rdbms | Benutzer-, Auth-, Workout-Daten | pii, internal |

## Integration-Patterns Zusammenfassung

- MVC Web UI ist primärer Inbound (synchronous request/response).
- Outbound SQL-DB per ORM (Entity Framework).
- Google OAuth2 als externes Auth-Provider.

## Related Documentation

- [[fitness-webapp-api]]
- [[../internal/sql-server-datastore]]
- [[../internal/google-oauth-outbound]]
- [[../internal/dependencies-overview]]
- [[../user-login-dataflow]]

[[../index]]
