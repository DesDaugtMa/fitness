---
title: "User Login Flow"
created: 2026-03-21
type: dataflow
visibility: public
flow_type: synchronous
tags: [blackbox, public, dataflow, sequence, authentication, login]
---

## Scenario

Ein Endbenutzer meldet sich an, entweder per E-Mail/Passwort oder Google OAuth2. Ziel ist ein erfolgreiches Session-Cookie-Login und Redirect zum Dashboard.

## Actors

- Benutzer (Browser)
- Fitness WebApp (ASP.NET Core MVC)
- SQL Server (User-Daten)
- Google OAuth2 Provider (optional)

## Flow Diagram

```mermaid
sequenceDiagram
    participant User
    participant WebApp
    participant DB
    participant Google

    User->>WebApp: GET /Account/Login
    WebApp-->>User: Login-Formular
    User->>WebApp: POST /Account/Login (email,password)
    WebApp->>DB: SELECT user by email
    DB-->>WebApp: User row
    WebApp->>WebApp: verify password hash
    alt success
      WebApp->>WebApp: create auth claims
      WebApp-->>User: Set-Cookie + redirect /Home/Index
    else failure
      WebApp-->>User: Login page + ModelState error
    end

    User->>WebApp: GET /Account/GoogleLogin?token=...
    WebApp->>Google: OAuth2 auth request
    Google-->>User: redirect to consent
    User->>Google: consent
    Google-->>WebApp: callback /Account/GoogleResponse
    WebApp->>Google: token/claims (internal)
    WebApp->>DB: find/create user
    WebApp-->>User: Set-Cookie + redirect /Home/Index
```
```

## Steps

1. GET /Account/Login lädt die Login-Ansicht.
2. POST /Account/Login überprüft Credentials im DB.
3. Bei Erfolg: Cookie-Session wird angelegt, Browser wird auf /Home/Index weitergeleitet.
4. Google-Flow: /Account/GoogleLogin startet OAuth2, /Account/GoogleResponse validiert und erstellt User/Token in DB.

## Error Paths

- Ungültige E-Mail/Passwort → Fehler auf Login-Form.
- Abgelaufener/ungültiger Registrierungstoken → Redirect /Account/Login.
- DB-Verbindungsfehler → Error-Seite (/Home/Error).

## Related APIs

- [[fitness-webapp-api]]
- [[../connections-overview]]
- [[../internal/google-oauth-outbound]]

[[../index]]
