---
type: meta
created: 2025-01-11
tags: [meta, adr, architecture, decision-records]
---

# /autodocs/adrs/_meta.md – Architecture Decision Records

## Zweck

ADRs (Architecture Decision Records) dokumentieren **wichtige Architektur-Entscheidungen** mit:
- **Kontext:** Warum mussten wir entscheiden?
- **Entscheidung:** Was haben wir gewählt?
- **Konsequenzen:** Was bedeutet das technisch/organisatorisch?
- **Alternativen:** Was haben wir nicht gewählt und warum?

ADRs sind **immutable** – einmal geschrieben, werden sie nie gelöscht, nur als `superseded` markiert.

---

## Dateinamen-Konvention

```
adr-XXX-kurzer-sprechender-titel.md
```

- **XXX:** Dreistellige, fortlaufende Nummer (`001`, `002`, `003`, ...)
- **Titel:** kebab-case, beschreibend
- **Beispiele:**
  - `adr-001-use-postgresql-for-persistence.md`
  - `adr-002-adopt-microservices-architecture.md`
  - `adr-003-implement-event-sourcing.md`

---

## Pflicht-Frontmatter

```yaml
---
type: adr
number: XXX                    # ADR-Nummer (z.B. 001)
created: YYYY-MM-DD
updated: YYYY-MM-DD            # optional, bei Ergänzungen
status: proposed | accepted | rejected | superseded | deprecated
decision_date: YYYY-MM-DD      # wann entschieden wurde
area: <bereich>                # z.B. architecture, security, data, infrastructure
tags: [adr, architecture, ...]
related_features: []           # Links zu Features
related_tests: []              # Links zu Tests
supersedes: []                 # Liste von ADR-Nummern, die überholt werden
superseded_by: []              # ADR-Nummer, die diese ersetzt (bei status: superseded)
---
```

### Status-Definitionen

| Status | Bedeutung |
|--------|-----------|
| `proposed` | Zur Diskussion vorgeschlagen |
| `accepted` | Akzeptiert und umgesetzt/in Umsetzung |
| `rejected` | Abgelehnt (mit Begründung) |
| `superseded` | Durch neuere ADR ersetzt |
| `deprecated` | Veraltet, wird aber noch genutzt |

---

## Pflicht-Struktur

Jede ADR **muss** folgende Sections enthalten:

```markdown
# ADR-XXX: Titel

## Status
[Status + ggf. Link auf superseding ADR]

## Kontext
- Was ist die Situation?
- Welches Problem lösen wir?
- Welche Constraints gibt es?
- Welche Stakeholder sind betroffen?

## Entscheidung
- Was haben wir entschieden?
- Wie sieht die Lösung aus?
- Warum ist das die beste Wahl?

## Alternativen
- Welche anderen Optionen gab es?
- Warum wurden sie nicht gewählt?

## Konsequenzen

### Positiv
- Was verbessert sich?
- Welche Vorteile entstehen?

### Negativ
- Welche Nachteile/Trade-offs?
- Was wird komplexer?

### Neutral
- Was ändert sich sonst?

## Umsetzung
- Welche konkreten Schritte sind nötig?
- Timeline/Milestone-Plan (optional)

## Compliance & Validation
- Wie prüfen wir, ob die Entscheidung erfolgreich war?
- Welche Metriken/Tests validieren das?

## Related
- [[related-feature]]
- [[related-test]]
- [[related-adr]]

## References
- Externe Links, Papers, Blog-Posts
```

---

## Wann eine ADR schreiben?

### ✅ Schreibe eine ADR für:

- **Architektur-Änderungen:** Monolith → Microservices
- **Technologie-Entscheidungen:** Datenbank-Wahl, Framework-Wechsel
- **Sicherheits-Entscheidungen:** Auth-Strategie, Encryption
- **Infrastruktur:** Cloud-Provider, Deployment-Strategie
- **Breaking Changes:** API-Versionierung, Datenmodell-Migration
- **Nicht-funktionale Anforderungen:** Performance, Skalierung, Observability

### ❌ Keine ADR für:

- **Routine-Implementierungen:** Standard CRUD-Features
- **Bug-Fixes:** Außer sie erfordern Architektur-Änderungen
- **Refactorings:** Außer sie ändern fundamentale Patterns
- **Konfiguration:** Außer strategisch wichtig

→ Bei Unsicherheit: Lieber eine ADR schreiben!

---

## Workflow

### 1. Proposal-Phase

```bash
# Erstelle neue ADR
autodocs/adrs/adr-XXX-titel.md

# Frontmatter mit status: proposed
# Diskussion via PR/Comments
```

### 2. Decision-Phase

```bash
# Nach Diskussion: Status auf accepted/rejected setzen
# decision_date hinzufügen
# Commit mit: docs: accept ADR-XXX <title>
```

### 3. Implementation-Phase

```bash
# Feature-Docs erstellen und ADR verlinken
# Tests schreiben und ADR verlinken
# Bei Breaking Change: Changelog updaten
```

### 4. Evolution-Phase

```bash
# Wenn ADR überholt: neue ADR schreiben
# Alte ADR: status: superseded
# superseded_by: <neue-nummer> setzen
```

---

## Verlinkung

### Von ADR zu anderen Docs

```markdown
## Related
- [[../features/2025-01-11-implement-postgres]] – Implementierung
- [[../tests/unit/database-connection]] – Tests
- [[adr-042-database-strategy]] – Übergeordnete Strategie
```

### Von anderen Docs zu ADR

```markdown
## Decision Basis
Diese Implementierung folgt [[../adrs/adr-001-use-postgresql]].
```

---

## Tags-Konvention

Nutze konsistente Tags für Filterbarkeit:

- **Bereich:** `#adr`, `#architecture`, `#security`, `#data`, `#infrastructure`
- **Status:** `#status/accepted`, `#status/superseded`
- **Technologie:** `#tech/postgresql`, `#tech/kubernetes`, `#tech/react`
- **Impact:** `#impact/high`, `#impact/medium`, `#impact/low`
- **Domain:** `#dom/<dein-bereich>`

---

## Beispiele

### Starter-ADRs (für neues Projekt)

1. **ADR-001:** Dokumentations-Strategie (Meta!)
2. **ADR-002:** Architektur-Pattern (Monolith/Microservices/Modular Monolith)
3. **ADR-003:** Primary Tech-Stack (Sprache, Framework)
4. **ADR-004:** Persistenz-Strategie (Datenbank-Wahl)
5. **ADR-005:** Deployment-Strategie (Cloud, On-Prem, Hybrid)

---

## Qualitätskriterien

Gute ADR checkt:
- ✅ Nummer ist eindeutig und fortlaufend
- ✅ Status ist klar definiert
- ✅ Kontext ist nachvollziehbar (auch in 2 Jahren)
- ✅ Entscheidung ist begründet
- ✅ Alternativen sind dokumentiert
- ✅ Konsequenzen sind vollständig (pos/neg/neutral)
- ✅ Related-Links sind gesetzt
- ✅ Tags sind konsistent

---

## Agent-Hinweise

### Automatisierbare Tasks

- **ADR-Nummerierung:** Automatisch nächste freie Nummer finden
- **Status-Validierung:** Prüfen ob Status valide ist
- **Link-Check:** Alle Related-Links verifizieren
- **Supersede-Chain:** Visualisierung von ADR-Evolution
- **Impact-Analyse:** Welche Features/Tests sind betroffen?

### Agent-Workflow für neue ADR

```python
def create_adr(title, context, area):
    # 1. Finde nächste freie Nummer
    next_num = find_next_adr_number()
    
    # 2. Generiere Dateinamen
    filename = f"adr-{next_num:03d}-{slugify(title)}.md"
    
    # 3. Nutze Template
    content = render_template('adr-template', {
        'number': next_num,
        'title': title,
        'area': area,
        'context': context
    })
    
    # 4. Erstelle Datei
    write_file(f"autodocs/adrs/{filename}", content)
    
    # 5. Update Index
    update_adr_index(next_num, title)
    
    return filename
```

---

## For AI Agents: ADR-Specific Rules

**🤖 When you process this directory:**

### Validation Rules
```yaml
adr_validation:
  filename: "adr-XXX-kebab-case.md where XXX is 001, 002, etc."
  frontmatter_required:
    - number: integer (must match filename)
    - status: [proposed, accepted, rejected, superseded, deprecated]
    - decision_date: YYYY-MM-DD (required if status=accepted)
    - area: string
    - supersedes: array (if this ADR replaces others)
    - superseded_by: array (if status=superseded)
  
  structure_required:
    - "## Status"
    - "## Kontext"
    - "## Entscheidung"
    - "## Alternativen"
    - "## Konsequenzen"
```

### Auto-Fix Actions
```yaml
auto_fix:
  - find_next_adr_number_automatically
  - add_decision_date_if_status_accepted
  - link_to_related_features_if_mentioned
  - update_index_md_with_new_adr
  
forbidden:
  - never_change_status_without_human
  - never_modify_decision_section
  - never_delete_adr (only supersede)
```

### When to Create New ADR
Trigger ADR creation if:
- Breaking changes detected in code
- New architecture pattern introduced
- Major technology decision needed
- Security-relevant changes

### Workflow
1. Validate all ADR files against schema
2. Check numbering is sequential
3. Verify supersedes/superseded_by chains
4. Update index.md if new ADRs found
5. Link to implementing features

---

## Related

- [[index]] – ADR-Übersicht mit Visualisierung
- [[../index]] – Haupt-Navigation
- [[../templates/adr-template]] – Template für neue ADRs
- [[../guides/when-to-write-adr]] – Entscheidungshilfe

[[index]]
