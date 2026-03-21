---
type: meta
created: 2025-11-13
version: 1.0.0
tags: [meta, docs, conventions, questions]
---

# /autodocs/questions/_meta.md – Fragen-Sammlung

## Zweck

Zentrale Sammlung aller offenen Fragen zum Projekt. Fragen werden kategorisiert, priorisiert und mit relevanten Dokumenten und Code-Stellen verknüpft. Die Fragen-Sammlung dient der Nachvollziehbarkeit und hilft, Lücken im Verständnis oder in der Dokumentation zu schließen.

## Struktur

- **fachlich/** - Fachliche Fragen (Domain, Anforderungen)
- **technisch/** - Technische Fragen (Implementation, Architektur)
- **sicherheit/** - Sicherheitsrelevante Fragen
- **betrieb/** - Betriebsrelevante Fragen (Deployment, Monitoring)
- **unklar/** - Noch nicht kategorisierte Fragen
- **index.md** - Übersicht und Statistik aller offenen Fragen

## Pflicht-Konventionen

### Frontmatter (YAML)

Jede Datei **muss** folgendes Frontmatter enthalten:
```yaml
---
type: question
created: YYYY-MM-DD
updated: YYYY-MM-DD  # optional
category: fachlich|technisch|sicherheit|betrieb|unklar
status: open|in_progress|answered|closed
priority: high|medium|low
source_document: Pfad zum Ursprungsdokument
source_code: Pfad zur relevanten Code-Stelle (optional)
tags: [tag1, tag2, ...]
---
```

### Spezifische Tags

- **Kategorie:** `#question/fachlich`, `#question/technisch`, `#question/sicherheit`, `#question/betrieb`
- **Priority:** `#prio/high`, `#prio/medium`, `#prio/low`
- **Status:** `#status/open`, `#status/in-progress`, `#status/answered`, `#status/closed`

### Benennungskonvention

- Format: `Q-{id}-{slug}.md`
- Beispiel: `Q-042-authentication-flow-unclear.md`

## Required Sections

1. **Question** - Die eigentliche Frage klar formuliert
2. **Context** - Kontext und Hintergrund der Frage
3. **Implications** - Welche Auswirkungen hat die Unklarheit?
4. **Potential Answers** - Mögliche Antworten (falls bekannt)
5. **Related Documentation** - Links zu verwandten Dokumenten
6. **Source References** - Verweise auf Quellen der Frage

## Verlinkungen

- Jede Frage muss mit ihrem Ursprungsdokument bidirektional verlinkt sein
- Jede Frage sollte mit relevanten Code-Stellen verlinkt sein (wo möglich)
- Verwandte Fragen sollten miteinander verlinkt sein

---

## Related

- [[index]] – Fragen-Übersicht
- [[../index]] – Haupt-Navigation
- [[../todo/_meta]] – TODOs-Dokumentation
- [[../adrs/_meta]] – Architektur-Entscheidungs-Dokumentation
