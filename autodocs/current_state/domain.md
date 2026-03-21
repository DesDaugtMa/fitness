---
type: "domain"
created: "2025-12-11"
updated: "2025-12-11"
status: "draft"
tags: ["domain", "business", "current_state"]
---

# Domain Overview

Dieses Dokument beschreibt alle fachlichen Domänen des Systems,
ihre Bounded Contexts, Verantwortlichkeiten und zentrale Begriffe.

> **Hinweis:**  
> Dieses Dokument wird schrittweise vom *initializer*, *architect* und später
> vom *current_state_updater* befüllt.

---

## 1. Zweck
<!-- AGENT:PURPOSE -->
Kurzbeschreibung des Domänenmodells auf hoher Ebene.

---

## 2. Hauptdomänen
<!-- AGENT:DOMAINS -->
Liste aller fachlichen Domänen in diesem System.  
Beispielstruktur:

- **Users / Identity**
- **Payments**
- **Notifications**
- **Reporting**

(Platzhalter — Agents füllen diese Liste schrittweise.)

---

## 3. Bounded Contexts
<!-- AGENT:BOUNDED_CONTEXTS -->
Beschreibung der konzeptuellen Kontexte in jeder Domäne.

Format:

### <Context-Name>
- Zweck:
- Verantwortlichkeiten:
- Grenzen:
- Zuordnung zu Features:
  - FR-XXX – …
  - FR-YYY – …

---

## 4. Domänenregeln & Policies
<!-- AGENT:DOMAIN_RULES -->
Regeln, Constraints und Invarianten, die das Verhalten bestimmen.

Beispiele:
- Ein Benutzer muss aktiv sein, um X tun zu dürfen.
- Zahlungen dürfen nie rückdatiert werden.

---

## 5. Beziehungen zwischen Domänen
<!-- AGENT:RELATIONS -->
Hochlevel-Interaktionen zwischen Domänen.

Diagramme (optional):
```mermaid
flowchart LR
  Users --> Payments
