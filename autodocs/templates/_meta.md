---
type: meta
created: 2025-01-11
tags: [meta, templates]
---

# /autodocs/templates/_meta.md – Wiederverwendbare Templates

## Zweck

Templates für konsistente Dokumentation:
- ADR-Template
- Feature-Template
- Test-Template
- TODO-Template
- Guide-Template

---

## Nutzung

```bash
# Kopiere Template
cp autodocs/templates/adr-template.md autodocs/adrs/adr-XXX-titel.md

# Fülle aus
# Passe Frontmatter an
```

---

## For AI Agents: Template-Specific Rules

**Validation:**
```yaml
validation:
  templates:
    - must_contain_frontmatter_placeholders: true
    - must_have_template_suffix: "-template.md"
    - placeholders_format: "{{VARIABLE_NAME}}"
  
  required_templates:
    - adr-template.md
    - feature-template.md
    - test-template.md
    - todo-template.md
```

**Auto-Fix (Safe):**
- Ensure all templates have standard placeholders
- Add missing template fields from _meta.md examples
- Normalize placeholder format to {{UPPERCASE}}

**Auto-Fix (Review Required):**
- Create new templates for new document types
- Update templates when frontmatter schemas change

**Forbidden:**
- Never delete existing templates without migration plan
- Never remove required placeholders

**Usage by AI:**
When creating new documents:
1. Copy appropriate template
2. Replace all {{PLACEHOLDERS}} with actual values
3. Remove optional sections marked as "optional"
4. Fill in current date for {{CREATED}}
5. Generate appropriate filename

**Template Inheritance:**
- All templates should follow structure in autodocs/_meta.md
- Templates can reference each other for common sections
- Keep templates generic (boilerplate-ready)

**Workflow:**
1. When new document type needed → Create template
2. When creating doc → Use template, replace placeholders
3. Validate created doc has no remaining {{PLACEHOLDERS}}
4. Log template usage for statistics

## Related

- [[index]] – Templates-Übersicht
- [[../index]] – Haupt-Navigation
