using System;
using System.Drawing;
using System.Windows.Forms;

namespace SchoolCenter
{
    public static class ThemeHelper
    {
        private static string _detectedFontFamilyName = null;

        public static string GetFontFamilyName()
        {
            if (_detectedFontFamilyName != null)
                return _detectedFontFamilyName;

            if (IsFontInstalled("Cairo"))
            {
                _detectedFontFamilyName = "Cairo";
            }
            else if (IsFontInstalled("Segoe UI"))
            {
                _detectedFontFamilyName = "Segoe UI";
            }
            else
            {
                _detectedFontFamilyName = "Tahoma";
            }

            return _detectedFontFamilyName;
        }

        private static bool IsFontInstalled(string fontName)
        {
            using (Font testFont = new Font(fontName, 8))
            {
                return string.Equals(fontName, testFont.Name, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public static Font CreateFont(float size, FontStyle style = FontStyle.Regular)
        {
            return new Font(GetFontFamilyName(), size, style);
        }

        // Global Theme Colors
        public static readonly Color ColorSidebarBg = Color.FromArgb(15, 23, 42);       // #0F172A
        public static readonly Color ColorSidebarActive = Color.FromArgb(51, 65, 85);   // #334155
        public static readonly Color ColorAccent = Color.FromArgb(37, 99, 235);         // #2563EB
        public static readonly Color ColorMainBg = Color.FromArgb(248, 250, 252);       // #F8FAFC
        public static readonly Color ColorCardBg = Color.White;                         // #FFFFFF
        public static readonly Color ColorBorder = Color.FromArgb(226, 232, 240);       // #E2E8F0
        public static readonly Color ColorTextPrimary = Color.FromArgb(15, 23, 42);     // #0F172A
        public static readonly Color ColorTextMuted = Color.FromArgb(100, 116, 139);    // #64748B

        // Pill / Status Colors
        public static readonly Color ColorPillGreenBg = Color.FromArgb(222, 247, 236);  // #DEF7EC
        public static readonly Color ColorPillGreenText = Color.FromArgb(3, 84, 63);    // #03543F
        public static readonly Color ColorPillRedBg = Color.FromArgb(253, 232, 232);    // #FDE8E8
        public static readonly Color ColorPillRedText = Color.FromArgb(155, 28, 28);    // #9B1C1C
        public static readonly Color ColorPillYellowBg = Color.FromArgb(254, 240, 138); // #FEF08A
        public static readonly Color ColorPillYellowText = Color.FromArgb(113, 63, 18);  // #713F12

        // Vibrant Metric Cards Colors
        public static readonly Color ColorCard1Bg = Color.FromArgb(30, 58, 138);       // #1E3A8A Blue
        public static readonly Color ColorCard2Bg = Color.FromArgb(13, 148, 136);      // #0D9488 Teal
        public static readonly Color ColorCard3Bg = Color.FromArgb(217, 119, 6);       // #D97706 Amber
        public static readonly Color ColorCard4Bg = Color.FromArgb(220, 38, 38);       // #DC2626 Red

        public static void ApplyTheme(Control control)
        {
            if (control == null) return;

            // Apply font while keeping original size and style
            float currentSize = 10F;
            FontStyle currentStyle = FontStyle.Regular;
            if (control.Font != null)
            {
                currentSize = control.Font.Size;
                currentStyle = control.Font.Style;
            }
            control.Font = CreateFont(currentSize, currentStyle);

            // Special handling for common WinForms controls
            if (control is Form)
            {
                Form frm = control as Form;
                frm.RightToLeftLayout = false;
                frm.BackColor = ColorMainBg;
            }
            else if (control is Button)
            {
                Button btn = control as Button;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Cursor = Cursors.Hand;
            }
            else if (control is DataGridView)
            {
                DataGridView dgv = control as DataGridView;
                dgv.EnableHeadersVisualStyles = false;
                dgv.BackgroundColor = ColorCardBg;
                dgv.BorderStyle = BorderStyle.None;
                dgv.RowHeadersVisible = false;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

                if (dgv.ColumnHeadersDefaultCellStyle != null)
                {
                    float hSize = dgv.ColumnHeadersDefaultCellStyle.Font != null ? dgv.ColumnHeadersDefaultCellStyle.Font.Size : 10F;
                    FontStyle hStyle = dgv.ColumnHeadersDefaultCellStyle.Font != null ? dgv.ColumnHeadersDefaultCellStyle.Font.Style : FontStyle.Bold;
                    dgv.ColumnHeadersDefaultCellStyle.Font = CreateFont(hSize, hStyle);
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorAccent;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                }
                if (dgv.DefaultCellStyle != null)
                {
                    float dSize = dgv.DefaultCellStyle.Font != null ? dgv.DefaultCellStyle.Font.Size : 10F;
                    FontStyle dStyle = dgv.DefaultCellStyle.Font != null ? dgv.DefaultCellStyle.Font.Style : FontStyle.Regular;
                    dgv.DefaultCellStyle.Font = CreateFont(dSize, dStyle);
                    dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
                    dgv.DefaultCellStyle.SelectionForeColor = ColorAccent;
                }
            }

            // Apply RTL settings
            control.RightToLeft = RightToLeft.Yes;

            // Apply to all children recursively
            foreach (Control child in control.Controls)
            {
                ApplyTheme(child);
            }
        }
    }
}