---
type: meta
created: 2025-01-11
tags: [meta, tests, unit]
---

# Unit-Tests - Metadata

## Zweck
Unit Tests dokumentieren Tests für einzelne Funktionen/Klassen/Module.

## Konventionen

**Frontmatter Required:**
```yaml
type: test
test_type: unit
status: passing|failing|skipped
area: <domain>
components: [...]
related_features: [...]
```

**Naming:**
- `test-<module>-<function>.md`
- Beispiel: `test-auth-validateToken.md`

## Best Practices
- Isoliere Tests (keine Abhängigkeiten)
- Dokumentiere Edge Cases
- ≥80% Coverage anstreben
