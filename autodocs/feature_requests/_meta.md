## type: meta created: 2025-12-09 tags: [meta, feature-request, requirements, traceability]

## 🏗️ /autodocs/featurerequests/\_meta.md – Feature Requests

### Zweck

Feature Requests dokumentieren alle **gewünschten Anforderungen** (funktionale und nicht-funktionale) an das Windsurf- und Antigravitations-Projekt, um eine lückenlose **Nachverfolgbarkeit (Traceability)** zu ermöglichen.

Sie definieren:
  - **Was** das Feature tun soll.
  - **Wie** der Erfolg gemessen wird (Acceptance Criteria).
  - **Wo** es im Code implementiert und getestet wird.

Feature Requests sind **änderbar**, solange der Status nicht `completed` ist.

-----

### Dateinamen-Konvention

```
XXX_feature-kurzer-sprechender-titel.md
```

  - **XXX:** Dreistellige, fortlaufende Nummer (`001`, `002`, `003`, ...)
  - **Titel:** kebab-case, beschreibend (z.B. `windload-calculation`, `anti-gravity-stabilization`)
  - **Beispiel:**
      - `001_windsurf-setup-optimierung.md`
      - `002_anti-gravity-altitude-control.md`

-----

### Pflicht-Frontmatter

```yaml
---
type: feature_request
number: XXX                    # Feature-Nummer (z.B. 001)
created: YYYY-MM-DD
updated: YYYY-MM-DD            # Datum der letzten Anpassung
status: requested | in_progress | blocked | rejected | completed
priority: high | medium | low   # Priorität für die Umsetzung
domain: <bereich>              # z.B. windsurf, anti_gravity, ui, api
tags: [feature, ...]
related_adrs: []               # Links zu relevanten Architektur-Entscheidungen (ADR-Nummern)
related_code: []               # Relative Pfade zu Haupt-Implementierungsdateien
---
```

#### Status-Definitionen

| Status | Bedeutung | Agenten-Aktion |
|--------|-----------|----------------|
| `requested` | Anforderung ist definiert, wartet auf Planung. | Agent prüft auf fehlende Tests/Code. |
| `in_progress` | Aktuell in Entwicklung/Implementierung. | Agent generiert/refaktorisiert Code und Tests. |
| `blocked` | Kann aufgrund externer/interner Abhängigkeiten nicht fortgesetzt werden. | Agent stoppt die Implementierung. |
| `rejected` | Feature wird nicht umgesetzt (mit Begründung). | Agent löscht zugehörigen Code/Tests (falls existent). |
| `completed` | Implementiert, getestet, erfüllt alle Acceptance Criteria. | **Immutable** – Agent darf Tests nur minimal anpassen. |

-----

### Pflicht-Struktur

Jeder Feature Request **muss** folgende Sections enthalten:

```markdown
# FR-XXX: Titel

## Status & Priorität
[Aktueller Status und Priorität]

## Beschreibung
- Was soll das Feature ermöglichen?
- Welchen Nutzen stiftet es?
- Welche Benutzergruppe profitiert davon?

## Akzeptanzkriterien (Acceptance Criteria)
- **Klare, überprüfbare** Punkte, wann das Feature als erfolgreich gilt.
- Sollten direkt 1:1 in Unit Tests überführt werden können.

## Verlinkte Assets (Traceability)

### Unit Tests
- Liste der **relativen Pfade** zu den primären Testdateien, die dieses Feature abdecken.
- *Beispiel:* `[[../../src/tests/windsurf_logic/test_setup_check.py]]`

### Implementierung
- Relative Pfade zu den wichtigsten Implementierungsdateien.

## Abhängigkeiten
- Andere Features, die vorausgesetzt werden.
- Welche ADRs (Architecture Decision Records) beeinflussen dies?

## Begründung der Ablehnung (falls status: rejected)
- Warum wird das Feature nicht umgesetzt?

## Agenten-Hinweise
- Spezifische Anweisungen für den Agenten zur Implementierung (z.B. Verwendung eines bestimmten Entwurfsmusters).
```

-----

### Agent-Workflow für neue Features

Der Agent soll diese Dokumentation nutzen, um **TDD** (Test-Driven Development) und die **Traceability** zu gewährleisten.

1.  **Neues Feature (`requested`):** Der Agent erstellt die Feature-Datei (FR-XXX) und generiert im nächsten Schritt die passenden **Unit Test-Dateien** als **textuelle Platzhalter** im TDD-Zyklus (RED).
2.  **Fehler im Test:** Wenn ein Test fehlschlägt, **MUSS** der Agent zuerst die zugehörige Feature-Datei (FR-XXX) konsultieren, um die Akzeptanzkriterien zu prüfen.
3.  **Änderung der Tests:** Eine Anpassung der Testlogik ist nur zulässig, wenn die Änderung in der FR-XXX-Datei vorgenommen und der Status entsprechend dokumentiert wird. **Completed**-Features sind geschützt.

-----

### Für AI Agents: Feature-Specific Rules

**🤖 When you process this directory:**

#### Validation Rules

```yaml
feature_validation:
  filename: "XXX_feature-name.md where XXX is 001, 002, etc."
  frontmatter_required:
    - number: integer (must match filename)
    - status: [requested, in_progress, blocked, rejected, completed]
    - priority: [high, medium, low]
  
  structure_required:
    - "## Akzeptanzkriterien (Acceptance Criteria)"
    - "## Verlinkte Assets (Traceability)"
    - "### Unit Tests"
```

#### Auto-Fix Actions

```yaml
auto_fix:
  - find_next_feature_number_automatically
  - link_to_related_tests_if_mentioned_in_description
  - add_feature_id_to_corresponding_test_files
  - **create_test_file_placeholders_for_red_step** # NEU: Agent erstellt die leere Testdatei mit Platzhalter
  
forbidden:
  - never_change_status_completed_without_human_review
  - never_modify_acceptance_criteria_after_completion
```

#### Workflow (Test-Check)

1.  Lese Test-Datei (z.B. `test_sail_logic.py`).
2.  Extrahiere die **Feature-ID** (z.B. `FEATURE_ID = "001"`).
3.  Lade die zugehörige Feature-Request-Datei (`001_...md`).
4.  **Prüfe:** Passt der Testcase noch zu den Akzeptanzkriterien? Nur wenn ja, ist der Test gültig.

-----

[[index]]

-----

