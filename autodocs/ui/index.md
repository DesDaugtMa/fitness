---
type: index
created: 2025-01-11
updated: 2025-11-11
tags: [ui, ux, design, frontend, index]
---

# 🎨 UI/UX Documentation

> **Für Agents:** Lies zuerst `[[_meta]]` für UI-Dokumentations-Konventionen.

## Überblick

UI/UX-Dokumentation:

- Design-System & Component-Library
- UI-Components (Buttons, Forms, Modals, etc.)
- User Flows & Wireframes
- Accessibility Guidelines (WCAG)
- Responsive Design Patterns
- Design-Tokens (Colors, Typography, Spacing)

---

## Components

_Noch keine UI-Components dokumentiert._

**Beispiel-Struktur:**

```
/ui/
├── design-system.md
├── /components/
│   ├── button.md
│   ├── form.md
│   └── modal.md
├── /flows/
│   ├── checkout-flow.md
│   └── login-flow.md
└── /accessibility/
    └── wcag-guidelines.md
```

---

## Design-System

_Noch nicht definiert_

**Typische Inhalte:**

- **Component-Library:** Wiederverwendbare UI-Bausteine
- **Design-Tokens:** Zentrale Design-Variablen
- **Grid-System:** Layout-Raster
- **Spacing-Scale:** Abstände-System
- **Typography-Scale:** Font-Größen und Weights

---

## Design-Tokens

_Noch nicht definiert_

**Beispiel:**

| Token | Value | Usage |
|-------|-------|-------|
| `--color-primary` | `#3B82F6` | Primär-Aktionen |
| `--space-md` | `16px` | Standard-Abstand |
| `--font-size-base` | `16px` | Body-Text |

---

## User Flows

_Noch keine Flows dokumentiert._

**Wichtige Flows:**

- Login-Flow
- Checkout-Flow
- Onboarding-Flow
- Settings-Management

→ Dokumentiere kritische User Journeys mit Wireframes/Screenshots

---

## Accessibility

_Noch nicht definiert_

**WCAG-Compliance-Ziel:**

- **Level A:** Minimum (Pflicht)
- **Level AA:** Standard (Empfohlen) ✅
- **Level AAA:** Optimal (Nice-to-have)

**Checkliste:**

- [ ] Keyboard-Navigation funktioniert
- [ ] Screen-Reader-kompatibel
- [ ] Contrast-Ratio ≥4.5:1 (WCAG AA)
- [ ] Focus-States sichtbar
- [ ] Responsive Design

---

## Wie dokumentiere ich UI?

### Component dokumentieren

1. **Erstelle Datei:**

   ```bash
   autodocs/ui/components/button.md
   ```

2. **Beschreibe:**
   - **Variants:** Primary, Secondary, Danger
   - **Props/API:** Welche Properties?
   - **Usage:** Code-Beispiel
   - **Accessibility:** Keyboard, Screen Reader, ARIA

3. **Verlinke:**
   - Features: `[[../features/2025-10-20-design-system]]`
   - Tests: `[[../tests/e2e/button-interactions]]`

### User Flow dokumentieren

1. **Erstelle Datei:**

   ```bash
   autodocs/ui/flows/checkout-flow.md
   ```

2. **Beschreibe Schritte:**
   - Step 1: User öffnet Warenkorb
   - Step 2: User gibt Adresse ein
   - Step 3: User wählt Payment
   - Step 4: Confirmation

3. **Wireframes hinzufügen:**
   - Link zu Figma
   - Oder eingebettete Mermaid-Diagramme

---

## Design-Tools

**Empfohlene Tools:**

- **Figma:** Design & Prototyping
- **Storybook:** Component-Development
- **Chromatic:** Visual Regression Testing

---

## Related

- [[_meta]] – UI-Dokumentations-Konventionen
- [[../features/_meta]] – Feature-Implementierungen
- [[../tests/_meta]] – UI-Testing (E2E)
- [[../index]] – Haupt-Navigation

[[../index]]
