---
type: guide
created: 2025-01-11
tags: [guide, tags, taxonomy, conventions]
---

# 🏷️ Tag-Taxonomie – Vollständige Tag-Übersicht

## Überblick

Tags ermöglichen **automatische Verlinkung** und **Filterung** in Obsidian.

---

## Tag-Hierarchie

### Dokumenttypen

```
#adr              - Architecture Decision Record
#feature          - Implementiertes Feature
#test             - Test-Dokumentation
#todo             - Offene Aufgabe
#guide            - How-To Guide
#meta             - Meta-Dokumentation
```

### Test-Typen

```
#test/unit        - Unit-Tests
#test/integration - Integration-Tests
#test/e2e         - End-to-End-Tests
#test/performance - Performance-Tests
#test/security    - Security-Tests
```

### Komponenten

```
#cmp/<name>       - Komponenten-spezifisch
  #cmp/auth-service
  #cmp/user-model
  #cmp/pdf-renderer
  #cmp/api-gateway
```

### Domänen

```
#dom/<bereich>    - Domänen-spezifisch
  #dom/auth
  #dom/payment
  #dom/reporting
  #dom/admin
```

### Status

```
#status/planned
#status/in-progress
#status/active
#status/released
#status/deprecated
#status/superseded
```

### Priorität

```
#prio/high
#prio/medium
#prio/low
```

### Kategorien (TODOs)

```
#tech-debt
#enhancement
#bug
#research
```

### Technologien

```
#tech/<name>
  #tech/postgresql
  #tech/react
  #tech/kubernetes
  #tech/graphql
```

### Impact

```
#impact/high
#impact/medium
#impact/low
```

### Special Tags

```
#breaking-change  - Breaking Changes
#security         - Security-relevant
#critical         - Kritisch
#deprecated       - Veraltet
#experimental     - Experimentell
```

---

## Verwendung in Obsidian

### Tag-Suche

```
# Alle ADRs mit hohem Impact
tag:#adr AND tag:#impact/high

# Alle Auth-bezogenen Docs
tag:#dom/auth

# Alle TODOs mit hoher Prio
tag:#todo AND tag:#prio/high

# Alle Breaking Changes
tag:#breaking-change
```

### Tag-Graph

Obsidian visualisiert automatisch:
- Welche Komponenten am meisten dokumentiert sind
- Welche Bereiche am meisten TODOs haben
- Dependency-Graphen via Komponenten-Tags

---

## Tag-Kombinationen (Beispiele)

```yaml
# ADR für Auth mit hohem Impact
tags: [adr, architecture, auth, dom/auth, impact/high]

# Feature mit Breaking Change
tags: [feature, api, breaking-change, cmp/api-gateway]

# High-Priority Tech Debt
tags: [todo, tech-debt, prio/high, cmp/legacy-code]

# E2E-Test für kritischen Flow
tags: [test, test/e2e, critical, dom/payment]
```

---

## Related

- [[../index]]
- [[agent-workflow]]

[[../index]]
