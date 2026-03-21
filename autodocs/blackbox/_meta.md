---
type: meta
created: 2025-11-13
version: 1.0.0
tags: [meta, docs, conventions, blackbox]
---

# /autodocs/blackbox/_meta.md – Blackbox-Dokumentation

## Zweck
Dokumentation aller externen Schnittstellen (inbound/outbound) und Abhängigkeiten aus einer Blackbox-Perspektive. Diese Dokumentation fokussiert auf die Außensicht des Systems, ohne interne Implementierungsdetails zu enthüllen.

## Struktur
- **public/** - Öffentliche API-Dokumente für externe Konsumenten
- **internal/** - Interne Abhängigkeiten und Schnittstellen für das Entwicklungsteam
- **index.md** - Übersicht aller Schnittstellen

## Pflicht-Konventionen

### Frontmatter (YAML)
Jede Datei **muss** folgendes Frontmatter enthalten:
```yaml
---
type: blackbox
created: YYYY-MM-DD
updated: YYYY-MM-DD  # optional
interface_type: inbound|outbound|datastore|external_system
status: active|deprecated|planned
visibility: public|internal
tags: [tag1, tag2, ...]
---
```

### Spezifische Tags
- **Protokoll:** `#protocol/rest`, `#protocol/graphql`, `#protocol/grpc`, etc.
- **Technologie:** `#tech/postgres`, `#tech/redis`, `#tech/kafka`, etc.
- **Klassifizierung:** `#data/sensitive`, `#data/personal`, `#data/public`, etc.
- **Risiko:** `#risk/high`, `#risk/medium`, `#risk/low`

### Sicherheit
- **Keine Secrets** in Dokumentation
- **Kein Zugangsdaten** für externe Systeme
- **Abstrakte Platzhalter** für sensible Parameter

## Required Sections
1. **Overview** - Kurzbeschreibung der Schnittstelle
2. **Schema** - Datenvertragsformat (JSON Schema, OpenAPI, etc.)
3. **Dependencies** - Abhängigkeiten zu anderen Systemen
4. **Security** - Authentifizierung, Autorisierung, Verschlüsselung
5. **Error Handling** - Fehlerszenarien und -behandlung
6. **Related Documentation** - Links zu verwandten Dokumenten

## Verlinkungen
- Jede Schnittstelle muss mit zugehörigen Features verlinkt sein
- Jede Schnittstelle muss mit relevanten Tests verlinkt sein
- Jede Schnittstelle muss mit ADRs verlinkt sein, die sie betreffen

---

## Related
- [[index]] – Blackbox-Übersicht
- [[../index]] – Haupt-Navigation
- [[../features/_meta]] – Features-Dokumentation
- [[../tests/_meta]] – Test-Dokumentation
