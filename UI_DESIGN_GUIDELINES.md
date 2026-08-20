# UI_DESIGN_GUIDELINES.md

## UI Design Style

- Build Arabic-first, right-to-left business interfaces.
- Be careful with WinForms RTL mirroring: when `RightToLeft` / `RightToLeftLayout` is enabled, `MiddleRight` may render visually on the left. Align by visual result, not by property name, and use a shared RTL-safe helper or established project pattern when possible.
- Prefer practical admin/system screens over decorative or marketing-style layouts.
- Keep screens clear, dense, and efficient for repeated daily use.
- Avoid overloaded CRUD screens: a single form should not expose add/edit inputs, search filters, result grids, print/export actions, and unrelated management actions all at once. Keep the primary task visible, move secondary workflows behind tabs/dialogs/panels, and preserve dense but readable Arabic business layouts.
- Support both light mode and dark mode from the start.
- Do not hard-code one-off colors inside forms/components when a theme system exists.
- Use theme tokens/helpers for background, surface, alternate surface, text, muted text, border, input, selection, primary, success, warning, and danger colors.
- Treat shell sidebars and navigation trees as a dedicated navigation surface, not as a normal form surface. Provide distinct colors for navigation background, alternate background, hover, selected row, border, primary text, and muted text. In light mode, the navigation surface may use a deep brand/navigation color so the app does not feel washed out; in dark mode, it must remain visibly layered and avoid flat pure black. Keep selected and hover states readable in both modes.
- Make dark mode readable and calm: preserve contrast, avoid pure black-heavy screens unless already used by the project, and verify grids, inputs, disabled states, selections, and totals remain legible.
- Use a consistent structure when suitable: header/title area, filters/search area, action toolbar, main content grid/table, and optional totals/status area.
- Header and section titles must be visually aligned with their content area, not positioned by guesswork. In RTL WinForms screens, form titles, subtitles, section headers, and description text must share the same visual right edge as the fields, grids, or action area below them. Use `TableLayoutPanel`, `Dock`, padding, and consistent columns instead of manual `Location` values, and verify the final visual alignment because `RightToLeftLayout` can mirror coordinates and make property-based alignment misleading.
- Prioritize readable data tables: clear headers, stable row heights, alternating row backgrounds when useful, strong numeric alignment, and visible selection states.
- Use restrained colors with semantic meaning: primary actions, secondary actions, danger, success, warning, and disabled states.
- Use a consistent semantic button color system: `Primary` for the main create/save workflow such as `+ جديد`, `✓ حفظ`, and `✎ تعديل` when it is the primary action on the screen; `Danger` for destructive or reversal actions such as `حذف`, `إلغاء الاعتماد`, `إلغاء عملية`, or irreversible cancel actions; `Warning` for risky but recoverable actions such as `تراجع`, `إرجاع`, `إلغاء مؤقت`, or actions requiring user attention; `Info` for discovery/output actions such as `بحث`, `عرض تقرير`, `تصفية`, and `استعراض`; and `Neutral` for navigation and utility actions such as `إغلاق`, `رجوع`, `طباعة`, `تصدير`, and secondary buttons. Button colors must come from the active theme, not hard-coded per form, and disabled buttons must use muted theme colors while remaining readable in light and dark mode.
- Do not make operational screens look like a rainbow of button colors. Use filled color only for the primary action, dangerous/destructive actions, or the one action that needs immediate attention; use neutral or outline styling for secondary actions, printing, exporting, navigation, and repeated utility commands.
- Buttons may use short text symbols to improve scanning, but symbols must be consistent across the project and verified in RTL layouts. Prefer simple symbols such as `+ جديد`, `✓ حفظ`, `× إغلاق`, `↻ تحديث`, `... اختيار`, `<< السابق`, `التالي >>`. Do not use decorative or inconsistent symbols, and do not let symbols replace clear Arabic button text for important actions.
- Reuse the project's existing theme, fonts, colors, controls, and layout helpers before creating new styling.
- Use layout containers such as panels, table layouts, docking, and anchoring so screens remain stable across sizes.
- Keep Arabic labels concise and preserve existing wording unless a wording change is requested.
- Avoid exposing English technical terms, AI jargon, internal architecture names, database terms, code identifiers, or implementation details in user-facing UI unless the business users already know and use that term.
- For mixed Arabic and English text in headers or status messages, avoid exposing technical identifiers such as `ERP_Core` unless necessary. Prefer Arabic business wording, and place technical details in a secondary details area or support log instead of the main title/header.
- Keep user-facing text practical and restrained. Avoid verbose AI-style explanations, promotional wording, exaggerated claims, or decorative copy; use short Arabic business labels that help the user complete the task.
- Add loading, empty, validation, and error states for data-heavy screens.
- Avoid oversized hero sections, decorative cards, gradients, unnecessary icons, and visual noise in operational screens.
- Before changing a UI pattern, consider its effect on usability, readability, existing workflows, dark mode, and consistency with the rest of the project.

## Enter Navigation Rule

For data-entry screens, support `Enter` as next-field navigation using the existing project pattern. Respect `TabIndex`, avoid breaking default Enter actions, and skip multiline text, grids, dropdown interactions, and confirmation dialogs.
