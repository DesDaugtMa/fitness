---
type: meta
created: 2025-11-13
version: 1.0.0
tags: [meta, docs, conventions, architecture]
---

# /autodocs/architecture/_meta.md – Architektur-Dokumentation

## Zweck

Vollständige Dokumentation der Softwarearchitektur nach iSAQB-Prinzipien. Hier werden verschiedene Architektur-Sichten, Qualitätsziele und das Mapping zwischen Architektur-Bausteinen und Code abgelegt.

## Struktur

- **context_view.md** - Systemkontext-Sicht (C4-Level-1)
- **building_block_view.md** - Bausteinsicht (C4-Level-2/3)
- **runtime_view.md** - Laufzeitsicht für wichtige Szenarien
- **deployment_view.md** - Infrastruktur- und Deployment-Sicht
- **quality_goals.md** - Qualitätsziele und ihre Priorisierung
- **quality_scenarios.md** - Konkrete Qualitätsszenarien
- **constraints.md** - Technische und organisatorische Randbedingungen
- **architecture_mapping.md** - Mapping zwischen Code und Architektur-Bausteinen
- **architecture_risks.md** - Architekturrisiken und Trade-offs
- **index.md** - Übersicht der Architektur-Dokumentation

## Pflicht-Konventionen

### Frontmatter (YAML)

Jede Datei **muss** folgendes Frontmatter enthalten:
```yaml
---
type: architecture
created: YYYY-MM-DD
updated: YYYY-MM-DD  # optional
view: context|building_block|runtime|deployment|quality|constraints
status: draft|accepted|deprecated
tags: [tag1, tag2, ...]
---
```

### Spezifische Tags

- **Sicht:** `#view/context`, `#view/building-block`, `#view/runtime`, `#view/deployment`
- **Qualität:** `#quality/performance`, `#quality/security`, `#quality/maintainability`, etc.
- **Treiber:** `#driver/business`, `#driver/technical`, `#driver/regulatory`
- **Stil:** `#style/layered`, `#style/microservices`, `#style/event-driven`, etc.

### Diagramme

- **Mermaid** bevorzugt (in-place rendered)
- **C4-Notation** für Architekturdiagramme
- **Sequenzdiagramme** für Runtime-View
- **UML** für detaillierte Darstellungen

## Required Sections

1. **Introduction** - Kurze Einführung und Kontext
2. **Architecture View** - Spezifische Sicht (je nach Dokumenttyp)
3. **Rationale** - Begründung für Entscheidungen in dieser Sicht
4. **Quality Concerns** - Relevante Qualitätsattribute für diese Sicht
5. **Related Documentation** - Links zu verwandten Dokumenten

## Verlinkungen

- Jede Architektur-Sicht muss mit relevanten ADRs verlinkt sein
- Jede Architektur-Sicht muss mit betroffenen Features verlinkt sein
- Architecture_mapping.md muss Verknüpfungen zu konkretem Code enthalten

---

## Related

- [[index]] – Architektur-Übersicht
- [[../index]] – Haupt-Navigation
- [[../adrs/_meta]] – Architektur-Entscheidungs-Dokumentation
- [[../features/_meta]] – Features-Dokumentation
