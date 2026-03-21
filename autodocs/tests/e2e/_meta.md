---
type: meta
created: 2025-01-11
tags: [meta, tests, e2e]
---

# E2E-Tests - Metadata

## Zweck
End-to-End Tests dokumentieren komplette User Flows von Anfang bis Ende.

## Konventionen

**Frontmatter Required:**
```yaml
type: test
test_type: e2e
status: passing|failing|skipped
area: <domain>
components: [...]
related_features: [...]
```

**Naming:**
- `test-<feature-name>-<scenario>.md`
- Beispiel: `test-user-login-success.md`

## Best Practices
- Teste kritische Flows (Login, Checkout, etc.)
- Dokumentiere Setup/Teardown Steps
- Verlinke zu Features via `related_features: [...]`
