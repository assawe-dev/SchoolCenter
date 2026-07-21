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

            // Special handling for DataGridView cells/headers
            DataGridView dgv = control as DataGridView;
            if (dgv != null)
            {
                if (dgv.ColumnHeadersDefaultCellStyle != null)
                {
                    float hSize = dgv.ColumnHeadersDefaultCellStyle.Font != null ? dgv.ColumnHeadersDefaultCellStyle.Font.Size : 10F;
                    FontStyle hStyle = dgv.ColumnHeadersDefaultCellStyle.Font != null ? dgv.ColumnHeadersDefaultCellStyle.Font.Style : FontStyle.Bold;
                    dgv.ColumnHeadersDefaultCellStyle.Font = CreateFont(hSize, hStyle);
                }
                if (dgv.DefaultCellStyle != null)
                {
                    float dSize = dgv.DefaultCellStyle.Font != null ? dgv.DefaultCellStyle.Font.Size : 10F;
                    FontStyle dStyle = dgv.DefaultCellStyle.Font != null ? dgv.DefaultCellStyle.Font.Style : FontStyle.Regular;
                    dgv.DefaultCellStyle.Font = CreateFont(dSize, dStyle);
                }
                if (dgv.AlternatingRowsDefaultCellStyle != null)
                {
                    float aSize = dgv.AlternatingRowsDefaultCellStyle.Font != null ? dgv.AlternatingRowsDefaultCellStyle.Font.Size : 10F;
                    FontStyle aStyle = dgv.AlternatingRowsDefaultCellStyle.Font != null ? dgv.AlternatingRowsDefaultCellStyle.Font.Style : FontStyle.Regular;
                    dgv.AlternatingRowsDefaultCellStyle.Font = CreateFont(aSize, aStyle);
                }
            }

            // Apply RTL settings
            control.RightToLeft = RightToLeft.Yes;

            Form form = control as Form;
            if (form != null)
            {
                form.RightToLeftLayout = false;
            }

            // Apply to all children recursively
            foreach (Control child in control.Controls)
            {
                ApplyTheme(child);
            }
        }
    }
}