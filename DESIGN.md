# UI/UX & Printing Design Guidelines

This document defines the strict UI/UX and Printing standards for the School Center Management System. The Agent must follow these rules for every Form, UserControl, and PrintDocument generated.

---

## 1. Environment Constraints
- **Target Framework:** .NET Framework 4.6 (Visual Studio 2015)
- **Language Level:** C# 6.0
- **UI Technology:** Windows Forms (WinForms)

---

## 2. Color Palette (Flat Design)
Always use these exact colors or their RGB equivalents for all controls:
- **Primary (Sidebar, Headers):** `#2C3E50` (RGB: 44, 62, 80) - Dark Navy
- **Accent/Action (Save, Primary Buttons):** `#3498DB` (RGB: 52, 152, 219) - Modern Blue
- **Success (Payment Receipt, Confirmations):** `#2ECC71` (RGB: 46, 204, 113) - Soft Green
- **Danger/Warning (Fees Charge, Delete):** `#E74C3C` (RGB: 231, 76, 60) - Soft Red
- **Background (Forms, Main Panel):** `#F8F9FA` (RGB: 248, 249, 250) - Off-White
- **Text (Primary):** `#2C3E50` (RGB: 44, 62, 80) - Dark Charcoal
- **Text (Muted/Labels):** `#7F8C8D` (RGB: 127, 140, 141) - Cool Gray

---

## 3. WinForms Control Styling Rules

### Typography
- **Primary Font:** `Segoe UI` (Preferred) or `Tahoma`.
- **Sizes:** 
  - Labels & Inputs: 10pt Regular.
  - Section Headers: 12pt Bold.
  - Main Title: 16pt Bold.

### Buttons (`Button`)
- `FlatStyle` must be set to `Flat`.
- `FlatAppearance.BorderSize` must be `0`.
- `BackColor` should match the Action type (Primary/Success/Danger).
- `ForeColor` must be `Color.White`.
- `Cursor` must be `Cursors.Hand`.

### DataGridView Styling (Modern Grid)
- `EnableHeadersVisualStyles` = `false`.
- `ColumnHeadersDefaultCellStyle`: BackColor = Primary (`#2C3E50`), ForeColor = White, Font = Segoe UI 10pt Bold.
- `DefaultCellStyle`: SelectionBackColor = Accent (`#3498DB`), SelectionForeColor = White.
- `AlternatingRowsDefaultCellStyle`: BackColor = `#F2F4F4` (Light alternating gray for readability).
- `BackgroundColor` = `Color.White`.
- `BorderStyle` = `BorderStyle.None`.
- `RowHeadersVisible` = `false`.

### Layout & Containers
- Main application structure must use a Left or Right Sidebar `Panel` (Primary Color) for navigation.
- Sub-forms should be loaded dynamically inside a central Main `Panel` as Borderless Forms (`FormBorderStyle = None`) or `UserControls` to avoid multiple floating windows.

---

## 4. Printing Standards (System.Drawing.Printing.PrintDocument)

All receipts, invoices, and reports must be rendered programmatically using `PrintDocument` via the `PrintPage` event. The design must look clean, balanced, and premium.

### Design Grid for Receipts & Invoices
- **Alignment:** Support Right-to-Left (RTL) text rendering since the system language is Arabic. Use `StringFormatFlags.DirectionRightToLeft`.
- **Header Section:**
  - Center-aligned or Top-Right bold Center Name (Font size: 16pt Bold).
  - Metadata: Invoice No, Date, and Employee name in 9pt Muted text.
- **Divider Lines:** Use `e.Graphics.DrawLine` with a thin pen (1px, Color: Cool Gray) to separate sections cleanly. Avoid ugly `================` text dividers.
- **Data Table Layout:**
  - Create aligned columns using explicit X coordinates.
  - Header row for items/fees should have a light background rectangle or a bold line above and below.
- **Totals Box:**
  - Net Balance, Amount Paid, or Total Fees should be highlighted in a distinct bounding box (`e.Graphics.DrawRectangle`) or bolded at the bottom right.
- **Footer:**
  - A subtle "Thank you" or system signature centered at the very bottom (Font size: 8pt Italic).

### Print Logic Constraints
- Ensure proper use of `Brush` and `Pen` objects, and ALWAYS dispose of them or wrap them in `using` statements to prevent memory leaks in WinForms.