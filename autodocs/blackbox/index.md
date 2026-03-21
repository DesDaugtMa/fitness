---
type: index
created: 2025-11-13
updated: 2026-03-21
tags: [index, blackbox]
---

# 🔄 Blackbox-Dokumentation

Diese Dokumentation beschreibt alle externen Schnittstellen und Abhängigkeiten des Systems aus einer Blackbox-Perspektive.

## 🌐 Öffentliche Schnittstellen

### REST APIs
- [[public/fitness-webapp-api]]

### GraphQL APIs
_Keine Einträge vorhanden._

### Messaging
_Keine Einträge vorhanden._

## 🔌 Interne Abhängigkeiten

### Datenbanken
- [[internal/sql-server-datastore]]

### Externe Services
- [[internal/google-oauth-outbound]]

### Cache-Systeme
_Keine Einträge vorhanden._

## 📊 Übersichten

- [[public/connections-overview]]
- [[public/user-login-dataflow]]
- [[internal/dependencies-overview]]
- [[internal/risk-register]]
- [[internal/redaction-log]]

## ⚠️ Risiken

- [[internal/risk-register]]

## 🧮 Statistik (Coverage)

- Inbound Interfaces: 100% erfasst (Controller-basiert)
- Outbound Integrations: 100% erfasst (DB, Google OAuth2)
- Datastore: 100% erfasst (SQL Server)

---

## Related

- [[../_meta]] – Globale Konventionen
- [[../features/index]] – Features-Übersicht
- [[../tests/index]] – Test-Dokumentation
- [[../adrs/index]] – Architektur-Entscheidungen
- [[../index]] – Hauptnavigation
