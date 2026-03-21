---
type: guide
created: 2025-01-11
tags: [guide, agent, workflow, automation]
---

# 🤖 Agent-Workflow – Wie Agents mit diesem System arbeiten

## Überblick

Dieser Guide beschreibt **wie AI-Agents deterministisch mit diesem Dokumentationssystem arbeiten**.

---

## Agent-Einstieg: Initiales Setup

### 1. System verstehen

```python
def initialize_agent():
    # 1. Lies globale Regeln
    rules = read_file("autodocs/_meta.md")
    
    # 2. Lies Bereichs-Meta
    adr_rules = read_file("autodocs/adrs/_meta.md")
    feature_rules = read_file("autodocs/features/_meta.md")
    test_rules = read_file("autodocs/tests/_meta.md")
    todo_rules = read_file("autodocs/todo/_meta.md")
    
    # 3. Parse Tag-Taxonomie
    tags = parse_tag_taxonomy(rules)
    
    # 4. Index alle Docs
    docs = index_all_docs("autodocs/")
    
    return AgentContext(rules, docs, tags)
```

### 2. Entscheidungsbaum

```
Neue Information?
├─ Architektur-Entscheidung? → create_adr()
├─ Implementierte Änderung? → create_feature()
├─ Test-Dokumentation? → create_test()
├─ Offene Aufgabe? → create_todo()
├─ Domänen-Wissen? → create_domain_doc()
└─ Prozess/How-To? → create_guide()
```

---

## Workflow 1: ADR erstellen

```python
def create_adr(title, context, decision, area):
    """Erstellt neue Architecture Decision Record"""
    
    # 1. Finde nächste ADR-Nummer
    existing_adrs = glob("autodocs/adrs/adr-*.md")
    numbers = [extract_number(f) for f in existing_adrs]
    next_num = max(numbers) + 1 if numbers else 1
    
    # 2. Generiere Dateinamen
    slug = slugify(title)
    filename = f"autodocs/adrs/adr-{next_num:03d}-{slug}.md"
    
    # 3. Generiere Tags
    tags = ["adr", "architecture", area]
    if "security" in title.lower():
        tags.append("security")
    
    # 4. Nutze Template
    content = render_template("autodocs/templates/adr-template.md", {
        "number": next_num,
        "title": title,
        "context": context,
        "decision": decision,
        "area": area,
        "tags": tags,
        "date": today()
    })
    
    # 5. Schreibe Datei
    write_file(filename, content)
    
    # 6. Update Index
    update_adr_index(next_num, title, area)
    
    # 7. Link zu verwandten Docs
    link_related_docs(filename, find_related(title, context))
    
    return filename
```

---

## Workflow 2: Feature dokumentieren

```python
def create_feature(title, commits, area):
    """Dokumentiert implementiertes Feature"""
    
    # 1. Generiere Dateinamen
    date = today()
    slug = slugify(title)
    filename = f"autodocs/features/{date}-{slug}.md"
    
    # 2. Analysiere Commits
    changed_files = get_changed_files(commits)
    components = identify_components(changed_files)
    
    # 3. Finde verwandte Docs
    related_adrs = find_adrs_for_area(area)
    related_tests = find_tests_for_components(components)
    resolved_todos = find_todos_for_title(title)
    
    # 4. Generiere Tags
    tags = ["feature", area]
    tags.extend([f"cmp/{c}" for c in components])
    
    # 5. Prüfe Breaking Changes
    breaking = is_breaking_change(commits)
    
    # 6. Nutze Template
    content = render_template("autodocs/templates/feature-template.md", {
        "title": title,
        "area": area,
        "commits": commits,
        "components": components,
        "related_adrs": related_adrs,
        "related_tests": related_tests,
        "breaking_change": breaking,
        "tags": tags,
        "date": date
    })
    
    # 7. Schreibe Datei
    write_file(filename, content)
    
    # 8. Update verwandte Docs
    if resolved_todos:
        mark_todos_completed(resolved_todos, filename)
    
    # 9. Update Changelog (wenn released)
    update_changelog(title, commits)
    
    return filename
```

---

## Workflow 3: Tests dokumentieren

```python
def create_test_doc(test_file, test_type, area):
    """Dokumentiert Test"""
    
    # 1. Parse Test-File
    test_cases = parse_test_cases(test_file)
    coverage = get_coverage(test_file)
    
    # 2. Finde getestetes Feature
    related_features = find_features_for_area(area)
    
    # 3. Generiere Dateinamen
    slug = slugify(os.path.basename(test_file))
    filename = f"autodocs/tests/{test_type}/test-{area}-{slug}.md"
    
    # 4. Generiere Tags
    tags = ["test", f"test/{test_type}", area]
    
    # 5. Nutze Template
    content = render_template("autodocs/templates/test-template.md", {
        "title": os.path.basename(test_file),
        "test_type": test_type,
        "area": area,
        "test_cases": test_cases,
        "coverage": coverage,
        "related_features": related_features,
        "test_files": [test_file],
        "tags": tags,
        "date": today()
    })
    
    # 6. Schreibe Datei
    write_file(filename, content)
    
    # 7. Update Coverage-Report
    update_coverage_report(area, coverage)
    
    # 8. Link zu Features
    link_tests_to_features(filename, related_features)
    
    return filename
```

---

## Workflow 4: TODO erstellen/abschließen

```python
def create_todo(title, problem, priority, category):
    """Erstellt neues TODO"""
    
    # 1. Prüfe Duplikate
    similar = find_similar_todos(title, problem)
    if similar:
        print(f"Ähnliche TODOs: {similar}")
    
    # 2. Generiere Dateinamen
    slug = slugify(title)
    filename = f"autodocs/todo/todo-{slug}.md"
    
    # 3. Generiere Tags
    tags = ["todo", category, f"prio/{priority}"]
    
    # 4. Nutze Template
    content = render_template("autodocs/templates/todo-template.md", {
        "title": title,
        "problem": problem,
        "priority": priority,
        "category": category,
        "tags": tags,
        "date": today()
    })
    
    # 5. Schreibe Datei
    write_file(filename, content)
    
    # 6. Update TODO-Index
    update_todo_index(priority, category)
    
    return filename

def complete_todo(todo_file, resolution_doc):
    """Markiert TODO als completed"""
    
    # 1. Update Frontmatter
    update_frontmatter(todo_file, {
        "status": "completed",
        "resolved_by": [resolution_doc],
        "updated": today()
    })
    
    # 2. Füge Resolution-Section hinzu
    append_section(todo_file, f"""
## Resolution

Gelöst durch [[{resolution_doc}]]

Datum: {today()}
""")
    
    # 3. Update TODO-Index
    update_todo_index_status(todo_file, "completed")
```

---

## Automatisierbare Tasks

### 1. Changelog-Generierung

```python
def update_changelog():
    """Generiert Changelog aus Conventional Commits"""
    
    # 1. Parse Git-History
    commits = get_commits_since_last_tag()
    
    # 2. Gruppiere nach Type
    features = [c for c in commits if c.type == "feat"]
    fixes = [c for c in commits if c.type == "fix"]
    breaking = [c for c in commits if c.breaking]
    
    # 3. Update changelog.md
    changelog = f"""
## [{next_version()}] - {today()}

### Added
{format_commits(features)}

### Fixed
{format_commits(fixes)}

### Breaking Changes
{format_commits(breaking)}
"""
    
    prepend_to_file("autodocs/changelog.md", changelog)
```

### 2. Coverage-Report-Update

```python
def update_coverage_report():
    """Update Coverage-Report aus Test-Run"""
    
    # 1. Parse Coverage-JSON
    coverage = parse_coverage_json("coverage/coverage-final.json")
    
    # 2. Berechne Metriken
    overall = coverage.overall
    by_area = coverage.by_area
    trends = calculate_trends(coverage, historical)
    
    # 3. Update coverage.md
    update_coverage_doc({
        "overall": overall,
        "by_area": by_area,
        "trends": trends,
        "updated": today()
    })
```

### 3. Broken-Link-Check

```python
def check_broken_links():
    """Findet kaputte interne Links"""
    
    # 1. Parse alle Docs
    docs = glob("autodocs/**/*.md")
    
    # 2. Extrahiere Links
    links = []
    for doc in docs:
        content = read_file(doc)
        links.extend(extract_wikilinks(content))
    
    # 3. Validiere Links
    broken = []
    for link in links:
        target = resolve_link(link)
        if not file_exists(target):
            broken.append((link, target))
    
    # 4. Report
    if broken:
        print(f"🔴 {len(broken)} broken links:")
        for src, target in broken:
            print(f"  - {src} → {target}")
```

### 4. Tag-Konsistenz-Validierung

```python
def validate_tags():
    """Prüft Tag-Konsistenz"""
    
    # 1. Parse alle Tags
    all_tags = set()
    for doc in glob("autodocs/**/*.md"):
        frontmatter = parse_frontmatter(doc)
        all_tags.update(frontmatter.get("tags", []))
    
    # 2. Lade erlaubte Tags
    allowed = load_tag_taxonomy()
    
    # 3. Finde ungültige Tags
    invalid = all_tags - allowed
    
    # 4. Report
    if invalid:
        print(f"🟡 {len(invalid)} ungültige Tags:")
        for tag in invalid:
            print(f"  - #{tag}")
```

---

## Best Practices für Agents

### DO's ✅

1. **Immer Meta lesen:** Starte mit `_meta.md` im relevanten Bereich
2. **Templates nutzen:** Kopiere Templates, fülle sie aus
3. **Tags generieren:** Automatisch aus Kontext ableiten
4. **Links setzen:** Bidirektional zwischen verwandten Docs
5. **Validieren:** Prüfe Frontmatter, Links, Tags vor dem Schreiben

### DON'Ts ❌

1. **Keine Duplikate:** Prüfe ob ähnliche Docs existieren
2. **Keine kaputten Links:** Validiere alle Wikilinks
3. **Keine inkonsistenten Tags:** Nutze definierte Taxonomie
4. **Keine leeren Docs:** Mindestinhalt aus Templates
5. **Nicht überschreiben:** Ergänze, lösche nicht

---

## Hilfsfunktionen

```python
# Slugify für Dateinamen
def slugify(text):
    return text.lower().replace(" ", "-").replace("_", "-")

# Wikilink-Resolution
def resolve_link(link):
    # [[../features/my-feature]] → autodocs/features/my-feature.md
    # [[my-feature]] → suche in allen Bereichen
    pass

# Tag-Extraktion
def extract_tags(text):
    return re.findall(r'#([\w-/]+)', text)

# Frontmatter-Parsing
def parse_frontmatter(file):
    content = read_file(file)
    match = re.match(r'^---\n(.*?)\n---', content, re.DOTALL)
    if match:
        return yaml.safe_load(match.group(1))
    return {}
```

---

## Related

- [[../index]] – Haupt-Navigation
- [[tag-taxonomy]] – Tag-Übersicht
- [[../templates/_meta]] – Templates
- [[../adrs/_meta]] – ADR-Regeln
- [[../features/_meta]] – Feature-Regeln

[[../index]]
