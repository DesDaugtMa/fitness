---
type: index
created: 2025-01-11
updated: 2025-11-11
tags: [ci, cd, deployment, automation, index]
---

# ⚙️ CI/CD & Deployment

> **Für Agents:** Lies zuerst `[[_meta]]` für CI/CD-Dokumentations-Konventionen.

## Überblick

CI/CD-Dokumentation:

- Build-Pipelines (GitHub Actions, GitLab CI, etc.)
- Deployment-Strategien (Blue/Green, Canary, Rolling)
- Environments (dev, staging, prod)
- Monitoring & Observability
- Rollback-Procedures & Incident-Response

---

## Pipelines

_Noch keine Pipelines dokumentiert._

**Beispiel-Struktur:**

- `pipeline-github-actions.md` – Haupt-CI/CD-Pipeline
- `deployment-production.md` – Production-Deployment-Strategie
- `monitoring-observability.md` – Monitoring-Stack
- `rollback-procedures.md` – Emergency-Rollback-Guide

---

## Environments

| Environment | URL | Purpose | Status |
|-------------|-----|---------|--------|
| **Development** | `localhost` | Lokale Entwicklung | 🟢 Active |
| **Staging** | - | Pre-Production Testing | ⚪ Planned |
| **Production** | - | Live System | ⚪ Planned |

---

## Deployment-Strategie

_Noch nicht definiert_

**Mögliche Strategien:**

- **Rolling Deployment:** Schrittweises Update
- **Blue/Green:** Parallele Environments
- **Canary:** Gradual Rollout mit Monitoring

→ Entscheidung via ADR dokumentieren: `[[../adrs/adr-XXX-deployment-strategy]]`

---

## Monitoring & Alerts

_Noch nicht eingerichtet_

**Geplante Tools:**

- **Logging:** (z.B. ELK, Loki)
- **Metrics:** (z.B. Prometheus, Grafana)
- **Tracing:** (z.B. Jaeger, OpenTelemetry)
- **Alerts:** (z.B. PagerDuty, Slack)

---

## Sicherheit

⚠️ **Keine Secrets in Dokumentation!**

- Alle Credentials in CI/CD-Tool hinterlegen
- `.env.example` für Format-Referenz
- Platzhalter verwenden: `DATABASE_URL=your_db_url_here`

---

## Wie dokumentiere ich CI/CD?

1. **Erstelle Datei:**

   ```bash
   autodocs/ci/pipeline-github-actions.md
   ```

2. **Beschreibe:**
   - Pipeline-Schritte
   - Environment-Variables (Platzhalter!)
   - Deployment-Prozess
   - Rollback-Procedure

3. **Verlinke:**
   - ADRs für Strategie-Entscheidungen
   - Features die deployed werden
   - Tests die in Pipeline laufen

---

## Related

- [[_meta]] – CI/CD-Dokumentations-Konventionen
- [[../adrs/_meta]] – ADRs für Infrastruktur-Entscheidungen
- [[../features/_meta]] – Features die deployed werden
- [[../tests/_meta]] – Tests in CI/CD
- [[../index]] – Haupt-Navigation

[[../index]]
