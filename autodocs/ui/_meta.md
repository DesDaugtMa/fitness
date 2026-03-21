---
type: meta
created: 2025-11-11
tags: [meta, ui, ux, design, frontend]
---

# /autodocs/ui/_meta.md – UI/UX Documentation

## Zweck

Dokumentiert **User Interface und User Experience**:
- Design-System und Component-Library
- UI-Components (Buttons, Forms, etc.)
- User Flows und Wireframes
- Accessibility Guidelines (WCAG, ARIA)
- Responsive Design Patterns
- Design-Tokens (Colors, Typography, Spacing)

## Ordnerstruktur

```
/ui/
├── _meta.md
├── index.md
├── design-system.md
├── /components/
│   ├── index.md
│   ├── button.md
│   └── form.md
├── /flows/
│   ├── index.md
│   └── checkout-flow.md
└── /accessibility/
    └── guidelines.md
```

## Dateinamen-Konvention

```
<component-name>.md
<flow-name>-flow.md
```

- **Component:** button, input, modal, nav
- **Flow:** checkout-flow, login-flow
- **Beispiele:**
  - `components/button.md`
  - `flows/checkout-flow.md`
  - `design-system.md`

## Pflicht-Frontmatter

```yaml
---
type: ui
created: YYYY-MM-DD
updated: YYYY-MM-DD         # optional
ui_type: component | flow | guideline | design-token
tags: [ui, ux, ...]
components: []              # Genutzte Components
related_features: []        # Features die UI nutzen
figma_link: ""              # Link zu Figma/Design-Tool (optional)
accessibility: A | AA | AAA # WCAG-Level (optional)
---
```

## Pflicht-Struktur (Component)

```markdown
# Component: [Name]

## Overview
[Was ist diese Component? Wann wird sie genutzt?]

## Variants
- **Primary:** Beschreibung
- **Secondary:** Beschreibung

## Props/API
| Prop | Type | Default | Description |
|------|------|---------|-------------|
| `variant` | string | 'primary' | Button-Variante |

## Usage Example
\`\`\`jsx
<Button variant="primary">Click me</Button>
\`\`\`

## Accessibility
- **Keyboard:** Enter/Space triggern Click
- **Screen Reader:** Label wird vorgelesen
- **ARIA:** `role="button"`

## Design Tokens
- Color: `--color-primary`
- Spacing: `--space-md`

## Related
- [[related-component]]
- [[related-flow]]
```

## Pflicht-Struktur (Flow)

```markdown
# User Flow: [Name]

## Overview
[Beschreibung des User Flows]

## Steps
1. **Step 1:** User öffnet Checkout
2. **Step 2:** User füllt Formular aus
3. **Step 3:** User bestätigt Bestellung

## Wireframes
[Link zu Figma oder eingebettetes Diagramm]

## Components Used
- [[components/button]]
- [[components/form]]

## Edge Cases
- Was passiert bei Fehler?
- Was wenn User abbricht?

## Related
- [[related-feature]]
- [[related-test]]
```

## Wann UI-Doku erstellen?

### ✅ Dokumentiere:
- **Design-System:** Zentrale Component-Library
- **Neue Components:** Jede wiederverwendbare UI-Component
- **User Flows:** Wichtige User Journeys
- **Accessibility-Guidelines:** WCAG-Compliance
- **Design-Tokens:** Farben, Typography, Spacing

### ❌ Nicht hier:
- **Code-Implementierung:** → Haupt-Projekt `/src/components/`
- **Backend-Logik:** → `/autodocs/features/`
- **Technische Architektur:** → `/autodocs/adrs/`

## Design-Tokens

Zentrale Design-Variablen dokumentieren:
```markdown
## Colors
- `--color-primary`: #3B82F6
- `--color-secondary`: #64748B

## Typography
- `--font-family-base`: 'Inter', sans-serif
- `--font-size-base`: 16px

## Spacing
- `--space-xs`: 4px
- `--space-sm`: 8px
- `--space-md`: 16px
```

## Accessibility-Checkliste

Jede Component sollte dokumentieren:
- ✅ **Keyboard-Navigation:** Funktioniert ohne Maus
- ✅ **Screen Reader:** Labels und ARIA-Attributes
- ✅ **Contrast-Ratio:** WCAG AA (mindestens 4.5:1)
- ✅ **Focus-States:** Sichtbare Focus-Indikatoren
- ✅ **Responsive:** Mobile-friendly

## Beispiel-Snippet

```yaml
# components/button.md Frontmatter
---
type: ui
created: 2025-11-11
ui_type: component
tags: [ui, component, button]
related_features: [[../features/2025-10-20-new-design-system]]
figma_link: "https://figma.com/file/..."
accessibility: AA
---
```

## Verlinkung

Von UI-Doku zu anderen Bereichen:
```markdown
## Implementation
Implementiert in [[../features/2025-10-20-design-system]].

## Tests
E2E-Tests in [[../tests/e2e/button-interactions]].

## Used In
- [[flows/checkout-flow]]
- [[flows/login-flow]]
```

## Tags-Konvention

- **Typ:** `#ui`, `#ux`, `#component`, `#design-system`
- **Accessibility:** `#a11y`, `#wcag-aa`, `#wcag-aaa`
- **Responsive:** `#responsive`, `#mobile-first`
- **Component-Tags:** `#cmp/button`, `#cmp/form`

## Agent-Hinweise

### Automatisierbare Tasks
- **Component-Inventory:** Alle Components aus Code extrahieren
- **Accessibility-Audit:** WCAG-Compliance prüfen
- **Design-Token-Sync:** Tokens aus CSS/SCSS extrahieren
- **Screenshot-Generation:** Automatische Component-Screenshots

---

## For AI Agents: UI-Specific Rules

**Validation:**
```yaml
validation:
  frontmatter:
    - required: [type, created, ui_type, tags]
    - ui_type: [component, flow, guideline, design-token]
    - accessibility: [A, AA, AAA]  # Optional but recommended
  
  structure:
    - components_must_have_usage_example: true
    - flows_must_have_steps: true
    - components_must_document_accessibility: true
  
  component_sync:
    - scan_paths: ["src/components/**/*.tsx", "src/components/**/*.jsx"]
    - match_to_docs: "autodocs/ui/components/<component-name>.md"
```

**Auto-Fix (Safe):**
- Extract component name from file path
- Create component stub from template
- Add missing #cmp/<component> tag
- Link components used in flows
- Add empty accessibility section if missing

**Auto-Fix (Review Required):**
- Generate props table from TypeScript interfaces
- Extract design tokens from CSS/SCSS variables
- Create user flow docs from page navigation

**Forbidden:**
- Never modify design decisions without designer review
- Never change accessibility guidelines

**Triggers:**
- New component file in src/components/ → Create component doc
- Component missing props documentation → Create TODO
- Accessibility section empty → Create TODO for a11y audit
- New design token in CSS → Update design-system.md

**Workflow:**
1. Scan src/components/ for React/Vue components
2. Create autodocs/ui/components/<name>.md if missing
3. Extract props from TypeScript interfaces
4. Check for accessibility attributes (aria-*, role)
5. Link to related features that use component
6. Validate design token references
7. Generate component inventory in index.md


## Related

- [[index]] – UI-Übersicht
- [[../index]] – Haupt-Navigation
- [[../features/_meta]] – Feature-Implementierungen
- [[../tests/_meta]] – UI-Testing
