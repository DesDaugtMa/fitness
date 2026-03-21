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

## Status

✅ **All 9 required views completed (2026-03-21):**
- ✅ context_view.md (C4 Context, system boundaries)
- ✅ building_block_view.md (C4 L2/L3, layers, components)
- ✅ runtime_view.md (5 scenarios, sequence diagrams)
- ✅ deployment_view.md (infrastructure, HA, backup strategy)
- ✅ quality_goals.md (5 goals, stakeholder perspectives)
- ✅ quality_scenarios.md (8 testable scenarios)
- ✅ constraints.md (12 constraints, impact analysis)
- ✅ architecture_risks.md (6 risks, mitigation roadmap)
- ✅ architecture_mapping.md (code ↔ docs, bidirectional)
- ✅ index.md (comprehensive navigation)

## Coverage Summary

| Dimension | Status | Evidence |
|-----------|--------|----------|
| Context Clarity | ✅ Complete | C4 context diagram, system boundaries defined |
| Building Blocks | ✅ Complete | 7 controllers, 6 models, 3 layers documented |
| Runtime Behavior | ✅ Complete | 5 scenarios, 5 sequence diagrams |
| Quality Goals | ✅ Complete | 5 goals, 8 scenarios, priority matrix |
| Decision Docs | ⚠️ Partial | 2 ADRs exist; recommend 5+ more |
| Risk Analysis | ✅ Complete | 6 risks identified, mitigation roadmap |
| Code Mapping | ✅ Complete | All major components mapped to code |
| Test Coverage | ❌ CRITICAL GAP | 0 automated tests; all manual QA |

**Next Agent: 50-auditor** (validation against _meta rules, compliance report)

## Related

- [[index]] – Architektur-Übersicht
- [[../index]] – Haupt-Navigation
- [[../adrs/_meta]] – Architektur-Entscheidungs-Dokumentation
- [[../features/_meta]] – Features-Dokumentation
