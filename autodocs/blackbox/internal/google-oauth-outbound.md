---
title: "Google OAuth2 Integration"
created: 2026-03-21
type: outbound-integration
visibility: internal
target_system: Google OAuth2
protocol: https
library: "Microsoft.AspNetCore.Authentication.Google 10.0.3"
tags: [blackbox, internal, outbound, oauth2, google, auth]
---

## Overview

Die App nutzt Google OAuth2 als optionalen Authentifizierungsprovider. Nutzer können sich über Google anmelden, der Flow wird in `AccountController` implementiert.

## Target System

- Provider: Google Identity Platform
- Endpoints: `https://accounts.google.com/o/oauth2/auth`, `https://oauth2.googleapis.com/token`

## Connection Details

- Protocol: HTTPS/TLS
- Base URL: `https://accounts.google.com` und `https://oauth2.googleapis.com`

## Authentication

- OAuth2 Authorization Code Flow (handler durch `Microsoft.AspNetCore.Authentication.Google`).
- ClientId/ClientSecret in `Config/AppSettings.GoogleAuthSettings`.

## Resilience

- Retry-Logik nicht explizit implementiert.
- Fehlertoleranz: Bei Fehlschlag Redirect zu `/Account/Login`.

## Error Handling

- Ungültiger Token oder Auth-Fehler: Redirect `/Account/Login` oder `/Account/AccessDenied`.
- 5xx von Google wird als Login-Failure behandelt.

## Monitoring

- Keine App-spezifischen Metriken. Empfohlen: OAuth-Erfolgsrate + Fehlerzählung.

## Dependencies

- Microsoft.AspNetCore.Authentication.Google 10.0.3

## Source References

- `Source/Fitness/Program.cs:30-90`
- `Source/Fitness/Controllers/AccountController.cs:1-330`

## Related Documentation

- Public: [[../public/connections-overview]]
- Public: [[../public/user-login-dataflow]]
- Internal: [[../internal/sql-server-datastore]]

[[../index]]
