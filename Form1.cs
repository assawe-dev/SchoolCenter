using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolCenter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // استدعاء الكود ليتحقق من وجود الملف أو يصنعه
                string connectionString = DbConnectionManager.GetConnectionString();
            }
            catch (System.IO.FileNotFoundException ex)
            {
                // هذه الرسالة ستظهر فقط أول مرة عندما يصنع المنظومة الملف النصي
                MessageBox.Show(ex.Message, "تنبيه إعداد السيرفر", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // إغلاق المنظومة تلقائياً ليذهب المستخدم ويعدل الملف النصي
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ غير متوقع: " + ex.Message);
            }
        }
    }
}
