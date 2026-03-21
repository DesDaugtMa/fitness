---
type: index
created: 2025-01-11
updated: 2025-11-11
tags: [domain, knowledge, business-logic, index]
---

# 🎨 Domain-Wissen

> **Für Agents:** Lies zuerst `[[_meta]]` für Domain-Dokumentations-Konventionen.

## Überblick

Fachliches Wissen über:

- Business-Domänen und Subdomänen
- Domain-Models (Entities, Value Objects, Aggregates)
- Ubiquitous Language (DDD-Begriffe)
- Business-Rules und Constraints
- Workflows und Prozesse

**Unterschied zu Features:** Hier dokumentieren wir **was** das System fachlich tut, nicht **wie** es technisch implementiert ist.

---

## Domänen

_Noch keine Domänen dokumentiert._

**Beispiel-Struktur:**

```
/domain/
├── /auth/
│   ├── index.md
│   ├── authentication-concepts.md
│   └── authorization-rules.md
├── /payment/
│   ├── index.md
│   ├── payment-workflows.md
│   └── glossary.md
└── /reporting/
    ├── index.md
    └── metrics-definitions.md
```

---

## Ubiquitous Language

| Begriff | Domäne | Definition |
|---------|--------|------------|
| _Noch keine Begriffe definiert_ | - | - |

→ Fachbegriffe hier zentral dokumentieren

---

## Domain-Driven Design (DDD)

Falls DDD verwendet wird:

### Bounded Contexts

_Noch nicht definiert_

### Aggregates

_Noch nicht definiert_

### Domain Events

_Noch nicht definiert_

---

## Wie dokumentiere ich Domain-Wissen?

1. **Erstelle Domänen-Ordner:**

   ```bash
   mkdir autodocs/domain/auth
   ```

2. **Erstelle Domänen-Doc:**

   ```bash
   autodocs/domain/auth/authentication-concepts.md
   ```

3. **Beschreibe fachlich:**
   - **Konzepte:** Was bedeutet "Session"? Was ist "2FA"?
   - **Business-Rules:** "User muss Email verifizieren bevor..."
   - **Workflows:** "Login-Prozess: 1. Credentials, 2. 2FA, 3. Session"

4. **Verlinke technische Umsetzung:**
   - Features: `[[../features/2025-10-15-implement-2fa]]`
   - ADRs: `[[../adrs/adr-008-authentication-strategy]]`
   - Tests: `[[../tests/e2e/login-flow]]`

---

## Stakeholder

Domänen-Wissen wird in Zusammenarbeit mit folgenden Stakeholdern erstellt:

- **Product Owner:** Fachliche Anforderungen
- **Domain Experts:** Fachexperten aus Business-Bereichen
- **Developers:** Technische Umsetzung

---

## Related

- [[_meta]] – Domain-Dokumentations-Konventionen
- [[../features/_meta]] – Wie Domänen implementiert werden
- [[../adrs/_meta]] – Architektur-Entscheidungen
- [[../index]] – Haupt-Navigation

[[../index]]
