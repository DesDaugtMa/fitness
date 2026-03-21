---
title: "Blackbox Risk Register"
created: 2026-03-21
type: risk-register
visibility: internal
tags: [blackbox, internal, risk, security, reliability]
---

### RISK-001: Fehlender Rate-Limiter für public-endpoints
- **Kategorie**: Security/Reliability
- **Severity**: Medium
- **Likelihood**: High
- **Risk Score**: Medium × High
- **Beschreibung**: Es gibt keine throttling-/rate-limiting-Middleware in der Web-App, dadurch sind Brute-Force-Angriffe und DoS möglich.
- **Betroffene Komponenten**: [[../public/fitness-webapp-api]]
- **Impact**: erhöhte Ressourcennutzung, Account Lockouts, Verfügbarkeit.
- **Aktuelle Maßnahmen**: keine
- **Empfohlene Mitigation**: IP-basierte Rate Limits in ASP.NET Core Middleware implementieren.
- **Status**: Open
- **Related TODOs**: [[../todo/todo-add-unit-tests]]
- **Related ADRs**: [[../adrs/adr-001-use-entity-framework]]

### RISK-002: Keine Retry/Backoff für Google OAuth2-Verbindung
- **Kategorie**: Reliability
- **Severity**: Medium
- **Likelihood**: Medium
- **Risk Score**: Medium × Medium
- **Beschreibung**: Wenn Google OAuth2 vorübergehend nicht verfügbar ist, wird der Flow ohne Backoff fehlschlagen.
- **Betroffene Komponenten**: [[../internal/google-oauth-outbound]]
- **Impact**: Login-Ausfall für Google-User.
- **Aktuelle Maßnahmen**: Redirect zu Login.
- **Empfohlene Mitigation**: Implementiere Policy mit `IHttpClientFactory` + Polly.
- **Status**: Open
- **Related TODOs**: [[../todo/add-oauth-retry-policy]]
- **Related ADRs**: [[../adrs/adr-001-use-entity-framework]]

### RISK-003: PII in DB ohne Verschlüsselung at rest (konzeptionell)
- **Kategorie**: Compliance
- **Severity**: Medium
- **Likelihood**: Medium
- **Risk Score**: Medium × Medium
- **Beschreibung**: User-Emails und DisplayNames gelten als PII, und Datenbankverschlüsselung ist aktuell nicht dokumentiert.
- **Betroffene Komponenten**: [[../internal/sql-server-datastore]]
- **Impact**: Compliance-/Datenschutzrisiko bei Datenleck.
- **Aktuelle Maßnahmen**: Anwendung vertraut auf Infrastruktur.
- **Empfohlene Mitigation**: Transparent Data Encryption (TDE) oder Anwendungsschicht-Verschlüsselung.
- **Status**: Open
- **Related TODOs**: [[../todo/add-data-encryption]]
- **Related ADRs**: [[../adrs/adr-001-use-entity-framework]]

[[../index]]
