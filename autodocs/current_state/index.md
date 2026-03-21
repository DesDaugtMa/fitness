Alles klar — hier ist **das komplette, final überarbeitete `index.md`**, sauber, vollständig, und **einsatzbereit zum Kopieren**.
Ich habe **nur** ergänzt, was notwendig war, damit Agents korrekt mit ausgelagerten Detail-/Topic-Dateien umgehen können — ohne den Rest zu verändern oder zu vermischen.

**→ Dies ist dein finales `autodocs/current_state/index.md`.**
**→ Einfach 1:1 reinkopieren.**

---

```markdown
---
type: "current_state_index"
created: "2025-12-11"
updated: "2025-12-12"
version: "1.1.0"
status: "active" # active | deprecated
tags: ["current_state", "index", "ssot"]
---

# Current System State – Index

Dies ist der **zentrale Einstiegspunkt** für alle Agents und Menschen, die den aktuellen
Systemzustand verstehen oder ändern wollen.

- **Single Source of Truth:** Alles in `autodocs/current_state/` gilt als verbindlich.
- **Startpunkt:** Jeder Agent MUSS dieses Dokument zuerst lesen, bevor er andere
  Dateien in `current_state/` öffnet.
- **Flache Struktur:** Alle Dateien in `current_state/` liegen direkt in diesem Ordner.
  Es dürfen **keine Unterordner** angelegt werden.
- **Kein Wildwuchs:** Neue Dateien, Umbenennungen und Löschungen in `current_state`
  dürfen nur über den dafür vorgesehenen `current_state_updater`-Agent erfolgen.

---

## 1. Regeln für Agents

### 1.1 Allgemeine Regeln

- `current_state/index.md` ist immer die **erste Datei**, die ein Agent lesen muss.
- Alle in diesem Dokument referenzierten Dateien gelten als **offiziell**.
- Informationen außerhalb von `autodocs/current_state/` (z. B. `autodocs/architecture`,
  `adrs`, `features`, etc.) gelten als **Quellen** oder **Historie**, aber nicht als
  verbindliche Wahrheit.

### 1.2 Schreibrechte

- **Nur der `current_state_updater`-Agent** darf Inhalte in `autodocs/current_state/*.md`
  dauerhaft ändern oder löschen oder neue Dateien anlegen.
- Andere Agents (z. B. `new_request`, `implementation`, `e2e_tester`, `initializer`,
  `architect`, `auditor`) dürfen:
  - `current_state` **lesen**,
  - aber **nicht** direkt darin schreiben (außer in explizit erlaubten Initial-Migrationsphasen).
- Fachliche Änderungen am Systemzustand dürfen ausschließlich erfolgen über:
  1. einen Feature-Request oder Bug-Report (`autodocs/feature_requests/**`)
  2. anschließend den `current_state_updater`.

### 1.3 Dateistruktur

- Es dürfen nur die hier gelisteten Dateien in `autodocs/current_state/` existieren.
- Neue Dateien dürfen nur angelegt werden, wenn:
  - sie in diesem `index.md` ergänzt werden und
  - sie vom `current_state_updater` erzeugt oder freigegeben werden.

### 1.4 Umgang mit großen Logikblöcken (Detail-/Topic-Dateien)

Um lange Hauptdateien übersichtlich zu halten, dürfen Inhalte ausgelagert werden:

- Der `current_state_updater` darf große Abschnitte in **Detail-/Topic-Dateien** auslagern.
- Namenskonvention:

```

<hauptdokument>_<topic>.md

````

Beispiele:
- `architecture_auth.md`
- `architecture_payments.md`
- `domain_billing.md`
- `features_reporting.md`

- **In der Hauptdatei MUSS ein sichtbarer Verweis stehen**, z. B.:

```markdown
<!-- DETAIL: architecture_auth -->
Details zur Authentifizierungsarchitektur siehe: `architecture_auth.md`
````

* Jede neue Detail-Datei MUSS:

  * ein vollständiges Frontmatter besitzen,
  * hier im Index unter Abschnitt **2. Dateien im `current_state`** eingetragen werden.

* Agents dürfen **niemals Inhalte unsichtbar verschieben**:

  * Jede Auslagerung muss im Hauptdokument kommentiert und im Index verlinkt werden.

---

## 2. Dateien im `current_state` (flache Struktur)

Alle Dateien liegen direkt in `autodocs/current_state/`:

### Hauptdokumente

* `index.md` – **dieses Dokument**, zentrale Navigation und Regeln.
* `domain.md` – Fachliche Domänen, Bounded Contexts, Verantwortlichkeiten.
* `architecture.md` – Technische Architektur, Bausteine, Schichten, Integrationen.
* `features.md` – Wichtige Features/Capabilities, inkl. Referenzen auf FR-IDs.
* `runtime_flows.md` – Wichtige Abläufe/User Journeys (z. B. Login, Checkout).
* `data_model.md` – Zentrale Datenstrukturen, Aggregate, Events.
* `quality.md` – Qualitätsziele, nicht-funktionale Anforderungen, relevante Szenarien.
* `decision_log_index.md` – Übersicht wichtiger Entscheidungen (z. B. ADRs, verlinkt).
* `glossary.md` – Begriffe, Definitionen, Abkürzungen.

### Detail-/Topic-Dateien (werden nur angelegt, wenn nötig)

Diese Liste wird durch den `current_state_updater` erweitert. Beispiele:

* `architecture_auth.md` – Detail: Authentifizierungs-Architektur
* `architecture_payments.md` – Detail: Zahlungsarchitektur
* `domain_billing.md` – Detail: Billing-Domäne
* `features_reporting.md` – Detail: Reporting-Features

> Jede existierende Detail-Datei MUSS in dieser Liste stehen.
> Jede neue Datei MUSS zuerst hier eingetragen werden, bevor sie gültig ist.

---

## 3. Frontmatter-Konventionen für alle Dateien in `current_state`

Jede Datei in `autodocs/current_state/` (einschließlich `index.md` und aller Detail-Dateien) MUSS
ein Frontmatter im YAML-Format enthalten:

```yaml
---
type: "<domain|domain_detail|architecture|architecture_detail|features|features_detail|runtime_flows|runtime_flows_detail|data_model|data_model_detail|quality|decision_log_index|glossary|current_state_index>"
created: "YYYY-MM-DD"
updated: "YYYY-MM-DD"
status: "draft|stable|deprecated"
tags: ["tag1", "tag2", "..."]
---
```

* **`created`** – Erstellt am
* **`updated`** – Zuletzt geändert
* **`status`** – `draft`, `stable`, `deprecated`
* Detail-Dateien verwenden die entsprechenden `*_detail` Typen.

Der `current_state_updater` ist verantwortlich dafür, `updated`-Felder konsistent zu aktualisieren.

