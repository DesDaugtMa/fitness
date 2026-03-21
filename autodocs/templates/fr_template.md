---
feature_id: "FR-XXX"
title: "<Wird vom Agenten ersetzt>"
status: "requested" # requested | waiting_for_input | in_progress | completed | rejected
created: "<DATE>"
updated: "<DATE>"
replaces: null # Falls dieses Feature eine alte Logik ersetzt
---

# Feature Request: {{TITLE}}

Dieses Dokument wird vom **New Request Agent** erzeugt und dient als gemeinsame Arbeitsgrundlage  
für Produkt, Entwickler, Testautomation und weitere Agents.

---

# 1. 🔍 Zusammenfassung
<!-- AGENT:SUMMARY -->
Kurze Beschreibung des Ziels und der Motivation des Features.

---

# 2. 📜 Anforderungen & Akzeptanzkriterien
<!-- AGENT:REQUIREMENTS -->
Liste aller funktionalen und nicht-funktionalen Anforderungen.

## 2.1 Funktionale Anforderungen
- [ ] …

## 2.2 Nicht-funktionale Anforderungen
- [ ] …

## 2.3 Akzeptanzkriterien
- [ ] GIVEN …
- [ ] WHEN …
- [ ] THEN …

---

# 3. ❓ Offene Fragen
<!-- AGENT:QUESTIONS -->
Hier trägt der Agent alle offenen Punkte ein, die vor der Umsetzung geklärt werden müssen.

Beispiele:
- [Feature-Agent] Frage: …
- [Requester] Antwort: …

---

# 4. ⚠️ 🚨 KONFLIKT & ENTSCHEIDUNG ERFORDERLICH
<!-- AGENT:CONFLICTS -->
Falls Konflikte gegen den aktuellen Systemzustand existieren  
(z. B. alte Regeln, Domain-Constraints, Architekturentscheidungen).

**Wird nur vom Agenten befüllt, falls Konflikte auftreten.**

Format:
- **Konfliktbeschreibung:** …
- **Betroffene Stelle:** …
- **Vorschlag:** …
- **Erforderliche Entscheidung:** `[ ] Accepted`

---

# 5. 🧭 Technische Planung (High-Level)
<!-- AGENT:TECHNICAL_PLANNING -->
Ziel dieses Abschnitts ist es, dem Coding-Agent einen klaren Rahmen zu geben.

## 5.1 Betroffene Domänen
- …

## 5.2 Überblick über geplante Änderungen
- …

## 5.3 Risiken & besondere Hinweise
- …

---

# 6. 🧪 Teststrategie
<!-- AGENT:TEST_STRATEGY -->
Dieser Abschnitt wird vom Planungs-/Teststrategie-Agent oder Teil des New Request Agents vorbereitet.

## 6.1 Unit-Test-Scope
- …

## 6.2 E2E-Test-Scope (Playwright)
- …

## 6.3 Testdaten / Mocking / Constraints
- …

---

# 7. 🏗️ Implementierungsnotizen (Coding-Agent)
<!-- AGENT:CODING_NOTES -->
Dieser Bereich wird vom Coding-Agent während der Umsetzung erweitert.

## 7.1 Implementierte Komponenten
- …

## 7.2 Offene Implementierungsfragen
- …

## 7.3 Unit-Tests
### 7.3.1 Geschriebene Tests
- …

### 7.3.2 Ergebnisse letzter Lauf
Status: ✔️ / ❌  
Ergebnisdetails: …

---

# 8. 🌐 E2E-Tests & QA (E2E-Agent)
<!-- AGENT:E2E -->
Dieser Bereich wird ausschließlich vom E2E-/Playwright-Agent gepflegt.

## 8.1 E2E-Testfälle
- …

## 8.2 Locator-Strategie & Self-Healing-Notizen
- …

## 8.3 Testergebnisse
Status: ✔️ / ❌  
Fehlerübersicht:
- …

## 8.4 Offene QA-Punkte
- …

---

# 9. 🔗 Verlinkte Assets
<!-- AGENT:LINKS -->
Der New Request Agent trägt hier alle Stubs und Dateien ein.

- **Feature-Request Dokument:** `autodocs/feature_requests/fr-XXX_title.md`
- **Unit-Test Stubs:** `tests/stubs/test_XXX_feature.py`
- **Code Stubs:** `src/stubs/feature_XXX.py`
- Weitere Dateien:
  - …

---

# 10. 📘 Historie & Kontext
<!-- AGENT:HISTORY -->
- Frühere Features, die relevant sind:
  - FR-…
- Falls dieses Feature eine alte Logik ersetzt:
  - `replaces: FR-OLD`

---

# 11. 🏁 Abschlussentscheidung
<!-- HUMAN:FINAL_DECISION -->
Wird vom Requester oder Product-Verantwortlichen getroffen.

## 11.1 Erfüllung der Akzeptanzkriterien
- [ ] Alle funktionalen Anforderungen erfüllt  
- [ ] Unit-Tests vollständig & grün  
- [ ] E2E-Tests vollständig & grün  
- [ ] Keine Konflikte mehr offen  
- [ ] Dokumentation aktualisiert  

## 11.2 Finale Entscheidung
**✔️ ACCEPTED** / **❌ REJECTED**

Kommentar:
…

---

# 12. 📜 Audit-Log
<!-- AGENT:AUDIT -->
Zeitgestempelte Einträge aller Agenten:

Beispiel:
- `[2025-12-11 14:03] [New-Request-Agent] Initiales Dokument erzeugt`
- `[2025-12-11 15:12] [Coding-Agent] Unit-Test-Stubs aktualisiert`
- `[2025-12-11 16:40] [E2E-Agent] Locator für login_button geheilt`
