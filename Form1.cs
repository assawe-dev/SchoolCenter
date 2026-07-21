using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SchoolCenter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Prevents window and control flickering on load and during transitions.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Verify or create the external DB configuration file.
                string connectionString = DbConnectionManager.GetConnectionString();

                // Initialize Database and Tables if they don't exist
                DbConnectionManager.InitializeDatabase();

                // Wire up data saved event handlers to keep everything in sync
                uStudentsView.DataSaved += (s, ev) => {
                    LoadDashboardData();
                    uStudentDuesView.LoadStudents();
                    uBalanceReportView.LoadReport();
                };
                uCoursesView.DataSaved += (s, ev) => {
                    LoadDashboardData();
                    uStudentDuesView.LoadCourses();
                    uBalanceReportView.LoadReport();
                };
                uStudentDuesView.DataSaved += (s, ev) => {
                    LoadDashboardData();
                    uBalanceReportView.LoadReport();
                    uStudentsView.LoadStudents();
                };
                uBalanceReportView.DataSaved += (s, ev) => {
                    LoadDashboardData();
                };

                // Initialize the Home View as the active view
                SetActiveButton(btnHome);
                ShowPanel(homeViewPanel);

                // Load database stats dynamically
                LoadDashboardData();
            }
            catch (FileNotFoundException ex)
            {
                // This warning message is displayed on first run when the config file is auto-generated.
                MessageBox.Show(ex.Message, "تنبيه إعداد السيرفر", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ غير متوقع أثناء تحميل البيانات: " + ex.Message, "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadDashboardData();
            }
        }

        /// <summary>
        /// Loads student count and treasury balance dynamically. Handled with graceful error fallback.
        /// </summary>
        private void LoadDashboardData()
        {
            try
            {
                pnlWarning.Visible = false;
                lblStatus.Text = "حالة الاتصال: متصل";
                lblStatus.ForeColor = Color.FromArgb(46, 204, 113); // Soft Success Green

                FinancialService financialService = new FinancialService();
                int totalStudents = financialService.GetTotalStudents();
                decimal treasuryBalance = financialService.GetCurrentTreasuryBalance();

                lblCardStudentsValue.Text = totalStudents.ToString();
                lblCardTreasuryValue.Text = treasuryBalance.ToString("N2") + " د.ل";
            }
            catch (Exception ex)
            {
                // Display database load exceptions gracefully within the UI.
                pnlWarning.Visible = true;
                lblWarningText.Text = "تنبيه: تعذر الاتصال بقاعدة البيانات. يرجى التحقق من ملف الإعدادات db_config.txt.\n" + ex.Message;
                lblStatus.Text = "حالة الاتصال: غير متصل";
                lblStatus.ForeColor = Color.FromArgb(231, 76, 60); // Soft Danger Red

                lblCardStudentsValue.Text = "غير متوفر";
                lblCardTreasuryValue.Text = "غير متوفر";
            }
        }

        /// <summary>
        /// Switches panels inside the Main Content Panel.
        /// </summary>
        private void ShowPanel(Panel activePanel)
        {
            homeViewPanel.Visible = (activePanel == homeViewPanel);
            studentsViewPanel.Visible = (activePanel == studentsViewPanel);
            coursesViewPanel.Visible = (activePanel == coursesViewPanel);
            studentDuesViewPanel.Visible = (activePanel == studentDuesViewPanel);
            balanceReportViewPanel.Visible = (activePanel == balanceReportViewPanel);
        }

        /// <summary>
        /// Highlights the currently active sidebar navigation button.
        /// </summary>
        private void SetActiveButton(Button activeBtn)
        {
            // Reset all buttons to default sidebar dark color
            btnHome.BackColor = Color.FromArgb(44, 62, 80);
            btnStudents.BackColor = Color.FromArgb(44, 62, 80);
            btnCourses.BackColor = Color.FromArgb(44, 62, 80);
            btnStudentDues.BackColor = Color.FromArgb(44, 62, 80);
            btnBalanceReport.BackColor = Color.FromArgb(44, 62, 80);
            btnSettings.BackColor = Color.FromArgb(44, 62, 80);

            // Highlight the selected button with the Accent blue color
            activeBtn.BackColor = Color.FromArgb(52, 152, 219); // #3498DB
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnHome);
            ShowPanel(homeViewPanel);
            LoadDashboardData();
        }

        private void BtnStudents_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnStudents);
            ShowPanel(studentsViewPanel);
            uStudentsView.LoadStudents();
        }

        private void BtnCourses_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnCourses);
            ShowPanel(coursesViewPanel);
            uCoursesView.LoadCourses();
        }

        private void BtnStudentDues_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnStudentDues);
            ShowPanel(studentDuesViewPanel);
            uStudentDuesView.LoadStudents();
            uStudentDuesView.LoadCourses();
            uStudentDuesView.LoadDues();
        }

        private void BtnBalanceReport_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnBalanceReport);
            ShowPanel(balanceReportViewPanel);
            uBalanceReportView.LoadReport();
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnSettings);

            // Open the external db_config.txt file dynamically in Notepad for convenience.
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db_config.txt");
            if (File.Exists(configPath))
            {
                try
                {
                    System.Diagnostics.Process.Start("notepad.exe", configPath);
                }
                catch
                {
                    MessageBox.Show("يرجى تعديل إعدادات الاتصال مباشرة من ملف 'db_config.txt' الموجود في مجلد المنظومة الرئيسي.",
                        "إعدادات الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("ملف إعدادات الاتصال 'db_config.txt' غير موجود. يرجى إعادة تشغيل المنظومة ليتم إنشاؤه تلقائياً.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnRefreshData_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
        }
    }
}