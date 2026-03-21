# Copilot Workspace Instructions — AutoDocs

This repository uses **AutoDocs** — a self-documenting, Obsidian-compatible documentation system.
All agents and rules are in `.agents/`. All generated documentation is in `autodocs/`.

**Entry point:** Always read `autodocs/_meta.md` first, then each subdirectory's `_meta.md` for specific rules.
When running any AutoDocs agent, read the full `.agent.md` file before starting.

---

## Agent Execution Order

For initial project documentation, run in this order:

1. `.agents/setup/10-initializer.agent.md`
2. `.agents/setup/20-blackbox.agent.md`
3. `.agents/setup/30-questionnaire.agent.md`
4. `.agents/setup/40-architect.agent.md`
5. `.agents/setup/50-auditor.agent.md`
6. `.agents/setup/60-todolister.agent.md`

For ongoing maintenance after code changes: `.agents/maintenance/updater.agent.md`

---

## Your Tasks (Maintenance Mode)

1. Validate structure against `autodocs/MANIFEST.json`
2. Check all `_meta.md` rules are followed
3. Apply auto-fixes as defined in each `_meta.md`
4. Sync with code changes (extract components, link tests, detect smells)
5. Report findings to `autodocs/review_report.md`
6. Log all changes to `autodocs/review_history.log`

**Always remember:**
- Never modify user-written content (only structure/metadata)
- Never modify source code
- Only write files inside `autodocs/`
- Always use relative `[[wikilinks]]`
- Always log changes
- When in doubt, create a TODO instead of guessing

---

## Validation Rules

### Frontmatter
Every file must have frontmatter with: `type`, `created`, `tags`. Dates: `YYYY-MM-DD`. Tags: lowercase.

### Required Structure

Files that must exist:
- `autodocs/_meta.md`
- `autodocs/index.md`
- `autodocs/changelog.md`

Directories that must exist (each needs `_meta.md` and `index.md`):
- `autodocs/adrs`
- `autodocs/features`
- `autodocs/tests`
- `autodocs/todo`
- `autodocs/templates`
- `autodocs/guides`

### Naming Conventions
- ADRs: `adr-XXX-kebab-case.md`
- Features: `YYYY-MM-DD-kebab-case.md`
- Tests: `test-area-name.md`
- TODOs: `todo-kebab-case.md`
- General: `kebab-case`

### Links
No broken wikilinks. Relative paths only. Bidirectional link rules:
- Feature → Test: required if released
- Test → Feature: always required
- ADR → Feature: optional
- Feature → ADR: required if architectural decision

### Tags
Lowercase only. Prefixes: `cmp/` (component), `dom/` (domain), `status/`, `prio/`. See `autodocs/guides/tag-taxonomy.md`.

---

## Auto-Fix Policy

**Apply immediately (safe):**
- `add_missing_frontmatter_fields`
- `normalize_tags_to_lowercase`
- `add_missing_backlinks`
- `fix_date_format_to_iso`
- `create_index_entries_for_new_docs`
- `link_tests_to_features_bidirectionally`
- `extract_component_tags_from_paths`

**Log and suggest only (review required):**
- `create_new_directories`
- `mark_todos_as_stale`
- `deprecate_features`
- `supersede_adrs`

**Never do these (forbidden):**
- `change_user_written_content`
- `modify_adr_decision_sections`
- `delete_meta_files`
- `commit_secrets_or_keys`

---

## Code Sync

Source directories to watch: `src`, `lib`, `components`, `app`

**Component detection** (`.tsx`, `.jsx`, `components/**/*.ts/js`) → create stubs in `autodocs/ui/components/` using `autodocs/templates/component-template.md`

**Test detection** (`*.test.ts`, `*.spec.ts`, `__tests__/**`) → link bidirectionally, auto-create test docs

**Code smell detection** → create TODO in docs:
- Function length > 50 lines → priority `medium`
- Cyclomatic complexity > 10 → priority `high`
- Test coverage < 80% → priority `medium`
- TODO/FIXME comments → priority `low`

**Coverage tracking:** Read `coverage/coverage-final.json`, update `autodocs/tests/coverage.md`.
Goals: unit 80%, integration 60%, e2e critical flows. Create TODO if below threshold.

---

## Workflows

**On file change:** identify changed files → validate → apply safe fixes → update related docs → log

**On commit:** extract conventional commit info → check if feature/ADR docs needed → update changelog if released → sync code to docs

**On test run:** parse coverage → update coverage.md → identify gaps → create TODOs for gaps

**On demand audit:** read all `_meta.md` files → validate entire structure → check all links → verify bidirectional links → generate review report → apply auto-fixes → update dependency graph

---

## Reporting

**Review report** → `autodocs/review_report.md` (sections: executive summary, metrics, validation results, auto-fixes applied, recommendations)

**Audit trail** → `autodocs/review_history.log` (append-only: timestamp, action, file, reason, confidence)

**Metrics tracked:** documentation completeness, link integrity, tag consistency, test coverage, todo health

---

## Confidence Scoring

| Threshold | Action |
|---|---|
| ≥ 0.90 | Auto-commit |
| ≥ 0.70 | Create PR |
| ≥ 0.50 | Log for review |

---

## Security

Abort and warn if detected in content:
- `api_key = "..."` or similar assignment patterns
- `password = "..."`
- `secret = "..."`
- Tokens with 20+ base64 chars

Allowed placeholders: `your_key_here`, `{{API_KEY}}`, `$API_KEY`, `REPLACE_WITH_ACTUAL_KEY`

---

## Templates

Located in `autodocs/templates/`. Available: `adr-template.md`, `feature-template.md`, `test-template.md`, `todo-template.md`.
Always copy and fill placeholders — never write from scratch.

---

## Project Context Placeholders

Update these after copying AutoDocs into a project:
- `{{PROJECT_TYPE}}` — e.g. web-app, api, library, cli
- `{{STACK}}` — e.g. React + TypeScript
- `{{ARCHITECTURE}}` — e.g. monolith, microservices

Defaults: `source_root: src`, `test_root: tests`, `docs_root: autodocs`