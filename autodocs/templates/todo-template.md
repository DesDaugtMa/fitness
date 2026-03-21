---
type: todo
created: YYYY-MM-DD
status: open
priority: medium
category: enhancement
area: <bereich>
tags: [todo, ...]
estimated_effort: <hours>
impact: <1-5>
urgency: <1-5>
effort: <1-5>
priority_score: <calculated>
---

# TODO: [Titel]

## Problem/Idee (DETAILLIERT)

**Was genau ist das Problem?**
- Betroffene Datei/Komponente/Feature: `pfad/zur/datei`
- Aktueller Zustand (IST): [Konkret messbar, z.B. "0% Coverage", "2h/Woche manuell"]
- Gewünschter Zustand (SOLL): [Konkret messbar, z.B. "80% Coverage", "0h/Woche automatisiert"]
- Messbarer Gap: [Zahlen, z.B. "45% → 80%"]

**Konkretes Beispiel:**
[Zeige ein echtes Beispiel des Problems, z.B. "User-Login in `src/auth/login.tsx` hat keine Tests, Bug #123 wäre mit Tests verhindert worden"]

---

## Business-Value (QUANTIFIZIERT)

**Warum sollten wir das machen?**

**Zeitersparnis:**
- Aktuell: [X Stunden/Woche für Y]
- Nach Umsetzung: [Z Stunden/Woche]
- Ersparnis: [X-Z Stunden/Woche]
- ROI: [Nach N Wochen bei M Stunden Setup]

**ODER Risiko-Reduktion:**
- Aktuelles Risiko: [Konkretes Szenario, z.B. "Production-Bug alle 2 Wochen → 4h Firefighting"]
- Nach Mitigation: [Reduziertes Risiko mit Zahlen]
- Vermiedene Kosten: [Stunden/Euro]

**ODER Blocker für:**
- [[Feature X]] kann nicht starten ohne dies
- [[Feature Y]] wartet darauf
- Business-Impact: [Konkret, z.B. "Release verzögert sich um 2 Wochen"]

**Kosten von Nicht-Tun:**
- Pro Woche: [X Stunden verschwendet ODER Y Risiko]
- Pro Monat: [Hochgerechnet]
- Nach 6 Monaten: [Breaking Point]

---

## Schritt-für-Schritt-Anleitung (EXECUTABLE)

**🎯 ERSTER SCHRITT (ohne Vorbereitung):**
```bash
# Dieser Command ist der ERSTE konkrete Schritt
[Command hier - copy-paste-ready]
```

### Schritt 1: Setup/Vorbereitung

**Was wird gemacht:** [Kurze Beschreibung]

**Command:**
```bash
# Vollständiger Command mit allen Parametern
cd /path/to/project
npm install --save-dev package@^1.2.3
```

**Erwarteter Output:**
```
added 1 package, and audited 234 packages in 2s
```

**Validation:**
```bash
# Prüfe dass es funktioniert hat
npm list package
# Erwartung: package@1.2.3
```

---

### Schritt 2: Konfiguration

**Was wird gemacht:** [Beschreibung]

**Datei erstellen:** `pfad/zur/config.json`

**Vollständiger Inhalt:**
```json
{
  "key": "value",
  "setting": true,
  "nested": {
    "option": "complete-example"
  }
}
```

**Validation:**
```bash
# Prüfe Config-Syntax
cat pfad/zur/config.json | jq .
# Erwartung: Gültiges JSON ohne Errors
```

---

### Schritt 3: Implementierung

**Was wird gemacht:** [Beschreibung]

**Datei:** `src/example/implementation.ts`

**Vollständiger Code:** (KEIN '...' oder Platzhalter!)
```typescript
// Vollständiges, lauffähiges Beispiel
export function exampleFunction(input: string): string {
  // Kompletter Code, nicht nur Skeleton
  const result = input.toUpperCase();
  return result;
}

// Mit Tests direkt im Code-Comment:
// exampleFunction("hello") === "HELLO"
```

**Validation:**
```bash
# Kompiliere und prüfe
npx tsc --noEmit src/example/implementation.ts
# Erwartung: Exit Code 0 (keine Fehler)
```

---

### Schritt 4: Testing

**Test-File:** `src/example/implementation.test.ts`

**Vollständiger Test-Code:**
```typescript
import { exampleFunction } from './implementation';

describe('exampleFunction', () => {
  it('should convert to uppercase', () => {
    expect(exampleFunction('hello')).toBe('HELLO');
  });
  
  // Weitere Test-Cases vollständig ausgeschrieben
});
```

**Tests ausführen:**
```bash
npm test -- implementation.test.ts
# Erwartung: PASS  src/example/implementation.test.ts
```

---

### Schritt 5: Integration/Deployment

**Was wird gemacht:** [Beschreibung]

**Commands:**
```bash
# Build
npm run build

# Deploy (mit konkreten Parametern)
./deploy.sh --environment=staging --version=1.2.3

# Verify
curl -s https://staging.example.com/health | jq .status
# Erwartung: {"status":"ok"}
```

---

### Schritt 6: Dokumentation aktualisieren

**Datei:** `autodocs/features/feature-xyz.md`

**Zu ändernde Zeilen:** 45-67

**Alt:**
```markdown
[Alter Text der ersetzt wird]
```

**Neu:**
```markdown
[Vollständiger neuer Text - kein "siehe oben" oder Verweise]
```

**Command:**
```bash
# Verify dass Änderung korrekt
git diff autodocs/features/feature-xyz.md
```

---

## Technische Details (VOLLSTÄNDIG)

**Benötigte Tools & Versionen:**
- Node.js: `^18.0.0` (Check: `node --version`)
- npm: `^9.0.0` (Check: `npm --version`)
- Package X: `^1.2.3` (Install: `npm install X@^1.2.3`)

**Alle npm/pip/etc. Packages:**
```bash
# Vollständige Install-Commands
npm install --save-dev \
  package-a@^1.0.0 \
  package-b@^2.0.0 \
  package-c@^3.0.0
```

**Konfigurationsdateien (vollständig):**

`config/example.yml`:
```yaml
# Vollständiger Inhalt, nicht nur Ausschnitt
setting1: value1
setting2: value2
nested:
  option: true
```

**Environment-Variablen:**
```bash
# .env (mit PLATZHALTERN, keine echten Secrets!)
API_KEY=your_api_key_here  # Ersetze mit echtem Key
DATABASE_URL=postgresql://user:pass@host:5432/db
```

**Dateipfade (alle betroffenen Files):**
- Source: `src/components/Example.tsx`
- Tests: `src/components/Example.test.tsx`
- Config: `config/example.json`
- Docs: `autodocs/features/example-feature.md`

---

## Acceptance Criteria (TESTBAR)

Jedes Kriterium hat einen konkreten Test-Command!

- [ ] **Kriterium 1:** File existiert
  ```bash
  # Test-Command
  ls -la src/components/Example.tsx
  # Erwartung: Exit Code 0, File existiert
  ```

- [ ] **Kriterium 2:** Tests laufen durch
  ```bash
  # Test-Command
  npm test -- Example.test.tsx
  # Erwartung: PASS, alle Tests grün
  ```

- [ ] **Kriterium 3:** Coverage > 80%
  ```bash
  # Test-Command
  npm test -- --coverage Example.tsx | grep "All files" | awk '{print $10}' | sed 's/%//'
  # Erwartung: Output > 80
  ```

- [ ] **Kriterium 4:** Build erfolgreich
  ```bash
  # Test-Command
  npm run build
  # Erwartung: Exit Code 0, kein Error in Output
  ```

- [ ] **Kriterium 5:** Dokumentation aktualisiert
  ```bash
  # Test-Command
  grep "neue Feature-Beschreibung" autodocs/features/example-feature.md
  # Erwartung: String gefunden
  ```

---

## Validation (WIE PRÜFEN)

**Nach Umsetzung - Komplette Validation:**

### Unit-Tests
```bash
npm test -- --coverage
# Erwartung: 
# - Alle Tests grün (PASS)
# - Coverage > 80% für geänderte Files
```

### Integration-Tests
```bash
npm run test:integration
# Erwartung: Feature funktioniert end-to-end
```

### Manuelle Checks
1. **UI-Check:** Öffne `http://localhost:3000/example`
   - Erwartung: [Screenshot oder genaue Beschreibung was zu sehen sein sollte]
2. **API-Check:** `curl -X POST http://localhost:3000/api/example -d '{"test":true}'`
   - Erwartung: `{"status":"success","data":{...}}`

### CI/CD-Check
```bash
# GitHub Actions Status
gh run list --workflow=ci.yml --limit=1
# Erwartung: Status = completed, conclusion = success
```

---

## Dependencies (KONKRET)

**NPM/Pip/etc. Packages:**
```bash
npm install --save-dev \
  @types/jest@^29.0.0 \
  jest@^29.0.0 \
  ts-jest@^29.0.0
```

**System-Tools:**
- Docker: `>= 20.0` (Check: `docker --version`)
- Git: `>= 2.30` (Check: `git --version`)

**Andere TODOs (Prerequisites):**
- [[todo-other-task]] muss VORHER erledigt sein
- [[todo-another-task]] kann parallel laufen

**Zugriffe/Permissions:**
- GitHub: Admin-Rechte auf Repo `org/repo`
- AWS: Deploy-Rechte auf `staging` Environment
- API-Key: OpenAI API Key in Environment-Variable `OPENAI_API_KEY`

**Externe Services:**
- Service X muss erreichbar sein (Check: `curl https://api.service-x.com/health`)

---

## Risiken & Pitfalls (MIT LÖSUNGEN)

### Häufige Fehler

**❌ Problem:** "Module not found" nach npm install
**✅ Lösung:** 
```bash
# Cache leeren und neu installieren
rm -rf node_modules package-lock.json
npm install
```

**❌ Problem:** Tests schlagen fehl mit "Cannot find module"
**✅ Lösung:** 
```bash
# Jest-Config prüfen
cat jest.config.js
# Stelle sicher dass moduleNameMapper korrekt ist
```

### Breaking-Change-Risiken

**Risiko:** Änderung könnte bestehende Feature X brechen
**Mitigation:** 
1. Regression-Tests laufen lassen: `npm run test:regression`
2. Feature-Flag nutzen: `FEATURE_NEW_BEHAVIOR=true`
3. Rollback-Plan bereit (siehe unten)

### Rollback-Strategie

**Falls es schief geht:**
```bash
# 1. Git Rollback
git revert HEAD --no-edit
git push

# 2. Cache leeren
npm cache clean --force

# 3. Vorherige Version deployen
./deploy.sh --environment=staging --version=1.2.2

# 4. Verify
curl https://staging.example.com/health
```

**Erwartung nach Rollback:** Alles funktioniert wie vorher

---

## Effort Estimation

**Total: X hours**

**Breakdown:**
- Setup (Tool-Installation, Config): Yh
  - npm install: 0.25h
  - Config-Files erstellen: 0.5h
- Implementierung (Code schreiben): Zh
  - Core-Logik: 2h
  - Error-Handling: 0.5h
- Testing (Test schreiben + ausführen): Ah
  - Unit-Tests: 1h
  - Integration-Tests: 0.5h
- Documentation (Doku aktualisieren): Bh
- Review (Code-Review, Anpassungen): Ch
  - Self-Review: 0.25h
  - Team-Review: 0.5h (waiting time nicht eingerechnet)

**Worst-Case (+50% Buffer):** X * 1.5 = [Berechnung]h

**Best-Case (-20%):** X * 0.8 = [Berechnung]h

---

## Expected Outcome (ARTEFAKTE MIT PFADEN)

**Nach Umsetzung existieren diese Files/Änderungen:**

1. **Datei:** `src/components/Example.tsx`
   - Status: Neu erstellt
   - Größe: ~150 LOC
   - Coverage: > 80%
   - Inhalt: Vollständige Example-Component mit TypeScript

2. **Datei:** `src/components/Example.test.tsx`
   - Status: Neu erstellt
   - Anzahl Tests: 8 Test-Cases
   - Coverage: 85%

3. **Datei:** `autodocs/features/example-feature.md`
   - Status: Aktualisiert (Zeilen 45-67 geändert)
   - Neu: Beschreibung der Example-Komponente

4. **Datei:** `autodocs/tests/coverage.md`
   - Status: Aktualisiert (neue Zeile für Example)
   - Coverage-Entry: `Example | 85% | 8 tests`

5. **GitHub PR:**
   - Status: Merged in `main`
   - CI-Pipeline: ✅ Grün
   - Approvals: 2+ Team-Members

**Metriken nach Umsetzung:**
- Coverage: 78% → 82% (gesamt)
- Build-Time: unverändert (~2min)
- Test-Suite: +8 Tests, +5s Runtime

---

## Priority Calculation

- **Impact:** X/5 – [Begründung mit konkreten Zahlen]
- **Urgency:** Y/5 – [Begründung mit Deadline/Blocker]
- **Effort:** Z/5 – [Begründung mit Stunden]
- **Priority Score:** (X * 2) + (Y * 1.5) - (Z * 0.5) = [Berechnung] → **HIGH/MEDIUM/LOW**

---

## Related

- [[../index]] – TODO-Index
- [[source-document]] – Woher kam dieses TODO
- [[related-feature]] – Verwandtes Feature
- [[related-adr]] – Relevanter ADR

---

## Status & Notes

**Status:** open
**Assigned:** [Optional]
**Started:** [Datum wenn in-progress]
**Completed:** [Datum wenn completed]

**Notes während Umsetzung:**
- [Datum]: [Notiz über Fortschritt/Probleme]

[[../index]]
