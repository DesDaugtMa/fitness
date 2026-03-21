---
type: meta
created: 2025-11-11
tags: [meta, ci, cd, deployment, automation]
---

# /autodocs/ci/_meta.md – CI/CD & Deployment Documentation

## Zweck

Dokumentiert **Build, Test, Deploy und Operations**:
- CI/CD-Pipelines (GitHub Actions, GitLab CI, etc.)
- Deployment-Strategien und Rollout-Procedures
- Environment-Konfigurationen (dev, staging, prod)
- Monitoring, Logging, Observability
- Incident-Response und Rollback-Procedures

## Dateinamen-Konvention

```
<thema>-<umgebung>.md
```

- **Thema:** pipeline, deployment, monitoring, rollback
- **Umgebung:** dev, staging, prod, all (optional)
- **Beispiele:**
  - `pipeline-github-actions.md`
  - `deployment-production.md`
  - `monitoring-observability.md`
  - `rollback-procedures.md`

## Pflicht-Frontmatter

```yaml
---
type: ci
created: YYYY-MM-DD
updated: YYYY-MM-DD         # optional
environment: dev | staging | prod | all
tags: [ci, cd, deployment, ...]
related_adrs: []            # ADRs für Deployment-Strategien
related_features: []        # Features die Deploy betreffen
pipeline_files: []          # Pfade zu .yml/.yaml Pipeline-Files
---
```

## Pflicht-Struktur

```markdown
# CI/CD: [Titel]

## Overview
[Was wird hier beschrieben?]

## Pipeline/Process
[Beschreibung des Ablaufs]

## Configuration
[Wo liegen Config-Files? Wichtige Settings?]
⚠️ **Secrets:** Nutze Platzhalter, keine echten Keys!

## Environments
| Environment | URL | Purpose |
|-------------|-----|---------|
| Dev | - | Lokale Entwicklung |
| Staging | - | Pre-Prod Testing |
| Production | - | Live System |

## Monitoring & Alerts
[Wie überwachen wir? Wo sind Dashboards?]

## Rollback Procedure
[Wie rollen wir zurück bei Problemen?]

## Known Issues
[Bekannte Probleme oder Einschränkungen]

## Related
- [[related-adr]]
- [[related-feature]]
```

## Wann CI/CD-Doku erstellen?

### ✅ Dokumentiere:
- **Pipeline-Setup:** Initiale CI/CD-Konfiguration
- **Deployment-Strategien:** Blue/Green, Canary, Rolling
- **Environment-Setup:** Wie werden Umgebungen konfiguriert?
- **Monitoring-Setup:** Observability-Stack
- **Incident-Procedures:** Rollback, Hotfix-Deployment

### ❌ Nicht hier:
- **Code-Implementierung:** → Haupt-Projekt `/src/`
- **Feature-Details:** → `/autodocs/features/`

## Sicherheit & Secrets

⚠️ **KRITISCH: Keine Secrets committen!**

### Erlaubte Dokumentation:
```markdown
## Environment Variables

Benötigte Secrets (in CI/CD-Tool hinterlegen):
- `DATABASE_URL` – PostgreSQL Connection String
- `API_KEY` – External API Key
- `DEPLOY_TOKEN` – Deploy-Authentifizierung

Siehe `.env.example` für Format.
```

### NICHT erlaubt:
```markdown
❌ DATABASE_URL=postgres://user:password123@prod.db...
❌ API_KEY=sk-abc123def456...
```

## Beispiel-Snippet

```yaml
# pipeline-github-actions.md Frontmatter
---
type: ci
created: 2025-11-11
environment: all
tags: [ci, github-actions, automation]
related_adrs: [[../adrs/adr-005-cicd-strategy]]
pipeline_files: [.github/workflows/ci.yml, .github/workflows/deploy.yml]
---
```

## Verlinkung

Von CI-Doku zu anderen Bereichen:
```markdown
## Decision Basis
Pipeline-Strategie basiert auf [[../adrs/adr-005-cicd-strategy]].

## Deployed Features
Deployment von [[../features/2025-11-10-new-api]] erfolgt via Blue/Green.

## Tests
E2E-Tests aus [[../tests/e2e/login-flow]] laufen vor Production-Deploy.
```

## Tags-Konvention

- **Bereich:** `#ci`, `#cd`, `#deployment`, `#monitoring`
- **Umgebung:** `#env/dev`, `#env/staging`, `#env/prod`
- **Tools:** `#tool/github-actions`, `#tool/docker`, `#tool/kubernetes`

## Agent-Hinweise

### Automatisierbare Tasks
- **Pipeline-Status-Updates:** Aus CI/CD-Runs
- **Deployment-Logs:** Automatisch in Doku verlinken
- **Environment-Drift-Detection:** Config-Abweichungen erkennen

---

## For AI Agents: CI/CD-Specific Rules

**Validation:**
```yaml
validation:
  frontmatter:
    - required: [type, created, environment, tags, pipeline_files]
    - environment: [dev, staging, prod, all]
  
  secrets:
    - no_real_secrets_in_docs: true
    - use_placeholders: true
    - forbidden_patterns:
        - "password\\s*=\\s*['\"][^'\"]+['\"]"
        - "token\\s*=\\s*[A-Za-z0-9]{20,}"
        - "api[_-]?key\\s*=\\s*sk-"
  
  pipeline_files:
    - verify_files_exist: true
    - check_yaml_syntax: true
```

**Auto-Fix (Safe):**
- Add missing environment field (default: "all")
- Normalize environment to lowercase
- Add missing pipeline_files array
- Replace found secrets with placeholders

**Auto-Fix (Review Required):**
- Create environment-specific docs for shared configs
- Update pipeline file references after file moves

**Forbidden:**
- Never commit actual secrets or credentials
- Never modify rollback procedures without review

**Triggers:**
- New .yml/.yaml file in .github/workflows/ → Create CI doc
- Deployment failure → Suggest incident doc
- New environment detected → Create environment doc

**Workflow:**
1. Scan for pipeline files (.github/workflows/**, .gitlab-ci.yml, etc.)
2. Validate referenced pipeline_files exist
3. Check for secrets in documentation
4. Link to related ADRs about deployment strategy
5. Update environment table if new env detected


## Related

- [[index]] – CI/CD-Übersicht
- [[../index]] – Haupt-Navigation
- [[../adrs/_meta]] – ADRs für Infrastruktur-Entscheidungen
