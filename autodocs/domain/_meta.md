---
type: meta
created: 2025-11-11
tags: [meta, domain, knowledge, business-logic]
---

# /autodocs/domain/_meta.md – Domain Knowledge & Business Logic

## Zweck

Sammelt **fachliches Wissen und Domänen-Modelle**:
- Business-Domänen und Subdomänen
- Ubiquitous Language (DDD-Begriffe)
- Domain-Models und Entities
- Business-Rules und Constraints
- Workflows und Prozesse

Unterschied zu `/features/`: Hier geht es um **was das System fachlich tut**, nicht **wie es implementiert ist**.

## Ordnerstruktur

```
/domain/
├── _meta.md
├── index.md
├── /auth/              # Authentication-Domäne
│   ├── index.md
│   └── concepts.md
├── /payment/           # Payment-Domäne
│   ├── index.md
│   └── workflows.md
└── /reporting/         # Reporting-Domäne
    └── index.md
```

## Dateinamen-Konvention

```
<thema>-<typ>.md
```

- **Thema:** Fachliches Thema
- **Typ:** concepts, workflows, rules, glossary
- **Beispiele:**
  - `auth/authentication-concepts.md`
  - `payment/payment-workflows.md`
  - `reporting/glossary.md`

## Pflicht-Frontmatter

```yaml
---
type: domain
created: YYYY-MM-DD
updated: YYYY-MM-DD         # optional
domain: <domänen-name>
tags: [domain, dom/<name>, ...]
related_adrs: []            # Architektur-Entscheidungen zur Domäne
related_features: []        # Features die Domäne implementieren
stakeholders: []            # Fachexperten, Product Owner
---
```

## Pflicht-Struktur

```markdown
# Domain: [Titel]

## Overview
[Was ist diese Domäne? Scope und Boundaries]

## Ubiquitous Language
| Begriff | Definition |
|---------|------------|
| **Entity** | Beschreibung |
| **Value Object** | Beschreibung |

## Domain Model
[Konzeptionelles Modell, ggf. Diagramm]

## Business Rules
- **Regel 1:** Beschreibung
- **Regel 2:** Beschreibung

## Workflows
[Fachliche Prozesse]

## Constraints
[Fachliche Einschränkungen]

## Related
- [[related-domain]]
- [[related-feature]]
```

## Wann Domain-Doku erstellen?

### ✅ Dokumentiere hier:
- **Fachbegriffe:** Was bedeutet "Rechnung" in diesem Kontext?
- **Business-Rules:** "Bestellung kann nur storniert werden wenn..."
- **Workflows:** "User-Registrierung läuft ab als..."
- **Domain-Models:** Entities, Value Objects, Aggregates
- **Ubiquitous Language:** Gemeinsame Sprache zwischen Dev & Business

### ❌ Nicht hier:
- **Code-Implementierung:** → `/src/` oder `/autodocs/features/`
- **Technische Entscheidungen:** → `/autodocs/adrs/`
- **UI-Designs:** → `/autodocs/ui/`

## Domain-Driven Design (DDD) Integration

Falls DDD genutzt wird:
```markdown
## Bounded Context
[Name und Scope des Bounded Context]

## Aggregates
- **Order Aggregate:** Root = Order, enthält OrderItems
- **User Aggregate:** Root = User, enthält Profile

## Domain Events
- `OrderPlaced` – Wenn Bestellung abgeschickt
- `PaymentReceived` – Wenn Zahlung eingegangen
```

## Beispiel-Snippet

```yaml
# payment/payment-workflows.md Frontmatter
---
type: domain
created: 2025-11-11
domain: payment
tags: [domain, dom/payment, business-logic]
related_adrs: [[../adrs/adr-010-payment-gateway]]
related_features: [[../features/2025-10-15-stripe-integration]]
stakeholders: [Product Owner, Finance Team]
---
```

## Verlinkung

Von Domain-Doku zu anderen Bereichen:
```markdown
## Implementation
Implementiert in [[../features/2025-10-15-payment-processing]].

## Architecture
Payment-Gateway-Wahl dokumentiert in [[../adrs/adr-010-payment-gateway]].

## Tests
Workflow getestet in [[../tests/e2e/checkout-flow]].
```

## Tags-Konvention

- **Domain:** `#domain`, `#dom/<name>`
- **DDD:** `#ddd`, `#bounded-context`, `#aggregate`
- **Stakeholder:** `#stakeholder/product`, `#stakeholder/business`

## Agent-Hinweise

### Automatisierbare Tasks
- **Glossar-Sync:** Begriffe aus Code-Comments extrahieren
- **Workflow-Visualization:** Aus Text Mermaid-Diagramme generieren
- **Cross-Reference:** Features zu Domänen-Konzepten verlinken

---

## For AI Agents: Domain-Specific Rules

**Validation:**
```yaml
validation:
  frontmatter:
    - required: [type, created, domain, tags, stakeholders]
    - domain: must_exist_as_subdirectory
  
  structure:
    - must_have_ubiquitous_language_section: true
    - must_link_to_implementation: true  # Links to features
  
  tags:
    - must_include: "dom/<domain_name>"
    - domain_tag_matches_frontmatter: true
```

**Auto-Fix (Safe):**
- Add missing "dom/<name>" tag based on domain field
- Create index.md for new domain subdirectories
- Add stakeholders field with empty array if missing
- Link to glossary from domain docs

**Auto-Fix (Review Required):**
- Extract domain terms from code comments (/** @domain ... */)
- Generate ubiquitous language table from code
- Create subdirectory for new domain

**Forbidden:**
- Never change business rules without stakeholder review
- Never modify ubiquitous language definitions

**Triggers:**
- New domain mentioned in features → Create domain/index.md
- Business rule in code comment → Extract to domain doc
- New bounded context detected → Create domain structure

**Workflow:**
1. Identify domains from code structure (packages, modules)
2. Extract domain concepts from code (classes, interfaces)
3. Cross-reference features to domain concepts
4. Validate ubiquitous language consistency
5. Link to ADRs for domain architecture decisions
6. Check stakeholders are defined


## Related

- [[index]] – Domain-Übersicht
- [[../index]] – Haupt-Navigation
- [[../features/_meta]] – Wie Domänen implementiert werden
