---
title: "Fitness Webapp HTTP API"
created: 2026-03-21
type: api
visibility: public
protocol: rest
authentication: cookie
base_path: / (MVC-Routing)
version: "1.0"
tags: [blackbox, public, api, inbound, rest, mvc, auth]
related_features:
  - "[[2026-03-21-user-management]]"
  - "[[2026-03-21-workout-tracking]]"
related_docs:
  - "[[../connections-overview]]"
  - "[[../user-login-dataflow]]"
  - "[[../internal/sql-server-datastore]]"
source_files:
  - Source/Fitness/Controllers/AccountController.cs:1-330
  - Source/Fitness/Controllers/ExercisesController.cs:1-250
  - Source/Fitness/Controllers/HomeController.cs:1-60
---

## Overview

Diese ASP.NET Core Web-App stellt ein MVC-basiertes HTTP-Interface zur Verfügung. Externe Clients (Browser) interagieren über Forms und Cookie-basierte Sessions mit Endpoints für Registrierung, Login, Workout-Management und UI-Views.

## Authentication

- Cookie-basierte Authentifizierung (`CookieAuthenticationDefaults.AuthenticationScheme`).
- Schutz gegen CSRF durch `[ValidateAntiForgeryToken]` bei POST-Endpoints.
- Google OAuth2 über `/Account/GoogleLogin` und `/Account/GoogleResponse`.

## Endpoints

| Method | Path | Description | Auth Required |
|--------|------|-------------|---------------|
| GET | /Account/Register/{id?} | Registrierungsseite | No |
| POST | /Account/Register | Benutzerregistrierung | No |
| GET | /Account/Login | Login-Seite | No |
| POST | /Account/Login | Login-Submit | No |
| POST | /Account/Logout | Logout | Yes |
| GET | /Account/GoogleLogin | Google OAuth Start | No |
| GET | /Account/GoogleResponse | Google OAuth Callback | No |
| GET | /Exercises | Liste der Übungen | Yes |
| GET | /Exercises/Details/{id} | Übungsdetails | Yes |
| GET | /Exercises/Create | Übung anlegen (Form) | Yes |
| POST | /Exercises/Create | Übung anlegen (Persist) | Yes |
| GET | /Exercises/Edit/{id} | Übung editieren (Form) | Yes |
| POST | /Exercises/Edit/{id} | Übung aktualisieren | Yes |
| GET | /Exercises/Delete/{id} | Übung löschen (Confirm) | Yes |
| POST | /Exercises/Delete/{id} | Übung löschen | Yes |
| GET | /Home/Index | Dashboard | Yes |
| GET | /Home/Privacy | Datenschutz | Yes |

## Data Formats

- HTML Forms für standardmäßige Aktionen.
- JSON-Payloads werden serverseitig nicht als API-Contract genutzt, sondern MVC-Model-Binding aus Formularfeldern.
- Session-Cookies: `AspNetCore.Cookies` (Name konfiguriert durch ASP.NET Standard).

## Error Handling

- `404` bei ungültigen IDs.
- `400`/Form-Revalidation bei ungültigen Modellen (ModelState errors).
- 500er Fehler mit `Home/Error` im Produktivmodus.

## Rate Limits

- Keine expliziten Rate Limits im Code. Sollte nachträglich mit Middleware ergänzt werden.

## Versioning

- Semantisches Versioning ist nicht als API-Versionierung implementiert (Single Version für UI-Route).

## Related Documentation

- Feature: [[2026-03-21-user-management]]
- Feature: [[2026-03-21-workout-tracking]]
- Architecture: [[adr-001-use-entity-framework]]
- Internal Datastore: [[../internal/sql-server-datastore]]

[[../index]]
