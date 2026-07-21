using System;
using System.Data;
using System.Data.SqlClient;
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
            ThemeHelper.ApplyTheme(this);
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
                // Explicitly set Right-to-Left layout flow and docking
                this.RightToLeftLayout = false;
                sidebarPanel.Dock = DockStyle.Right;
                mainContentPanel.Dock = DockStyle.Fill;

                // Bind paint events to draw nice subtle borders
                headerPanel.Paint += DrawHeaderBottomBorder;
                cardStudents.Paint += DrawCardBorder;
                cardCourses.Paint += DrawCardBorder;
                cardTreasury.Paint += DrawCardBorder;
                cardDebts.Paint += DrawCardBorder;
                pnlDashboardRight.Paint += DrawCardBorder;
                pnlDashboardLeft.Paint += DrawCardBorder;

                // Verify or create the external DB configuration file.
                string connectionString = DbConnectionManager.GetConnectionString();

                // Initialize Database and Tables if they don't exist
                DbConnectionManager.InitializeDatabase();

                // Wire up data saved event handlers to keep everything in sync
                uStudentsView.DataSaved += (s, ev) => {
                    LoadDashboardData();
                    uStudentDuesView.LoadStudents();
                    uBalanceReportView.LoadReport();
                    uPaymentsView.LoadStudentsList();
                    uPaymentsView.LoadTransactions();
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
                    uPaymentsView.LoadTransactions();
                };
                uBalanceReportView.DataSaved += (s, ev) => {
                    LoadDashboardData();
                };
                uPaymentsView.DataSaved += (s, ev) => {
                    LoadDashboardData();
                    uBalanceReportView.LoadReport();
                    uStudentDuesView.LoadDues();
                };
                uUsersView.DataSaved += (s, ev) => {
                    LoadDashboardData();
                };

                // Display currently logged-in user information
                lblUserInfo.Text = string.Format("مرحباً بك، {0}", UserSession.Username);
                lblRoleBadge.Text = UserSession.Role == "Admin" ? "مسؤول النظام 🎓" : "المحاسب المالي 💰";

                // Apply role-based screen-level permissions
                btnStudents.Visible = UserSession.CanManageStudents;
                btnCourses.Visible = UserSession.CanManageCourses;
                btnStudentDues.Visible = UserSession.CanAssignDues;
                btnPayments.Visible = UserSession.CanReceivePayments;
                btnBalanceReport.Visible = UserSession.CanViewReports;
                btnUsers.Visible = (UserSession.Role == "Admin" && UserSession.CanManageUsers);

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
                lblStatus.ForeColor = Color.FromArgb(16, 185, 129); // Success Green #10B981

                FinancialService financialService = new FinancialService();
                int totalStudents = financialService.GetTotalStudents();
                int totalCourses = financialService.GetTotalCourses();
                decimal treasuryBalance = financialService.GetCurrentTreasuryBalance();
                decimal totalOutstandingDebts = financialService.GetTotalOutstandingDebts();

                lblCardStudentsValue.Text = totalStudents.ToString();
                lblCardCoursesValue.Text = totalCourses.ToString();
                lblCardTreasuryValue.Text = treasuryBalance.ToString("N2") + " د.ل";
                lblCardDebtsValue.Text = totalOutstandingDebts.ToString("N2") + " د.ل";

                LoadRecentActivity();
            }
            catch (Exception ex)
            {
                // Display database load exceptions gracefully within the UI.
                pnlWarning.Visible = true;
                lblWarningText.Text = "تنبيه: تعذر الاتصال بقاعدة البيانات. يرجى التحقق من ملف الإعدادات db_config.txt.\n" + ex.Message;
                lblStatus.Text = "حالة الاتصال: غير متصل";
                lblStatus.ForeColor = Color.FromArgb(239, 68, 68); // Danger Red #EF4444

                lblCardStudentsValue.Text = "غير متوفر";
                lblCardCoursesValue.Text = "غير متوفر";
                lblCardTreasuryValue.Text = "غير متوفر";
                lblCardDebtsValue.Text = "غير متوفر";
            }
        }

        private void LoadRecentActivity()
        {
            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. Fetch recent transactions/registrations
                    string recentQuery = @"
                        SELECT TOP 7
                            s.StudentName AS [الطالب],
                            CASE
                                WHEN t.TransactionType = 'Fee Charge' THEN N'تعيين دورة'
                                WHEN t.TransactionType = 'Payment Receipt' THEN N'سداد دفعة'
                                WHEN t.TransactionType = 'Opening Balance' THEN N'رصيد افتتاح سابق'
                                ELSE t.TransactionType
                            END AS [العملية],
                            CASE
                                WHEN t.Debit > 0 THEN t.Debit
                                ELSE t.Credit
                            END AS [القيمة (د.ل)],
                            FORMAT(t.TransactionDate, 'dd/MM HH:mm') AS [الوقت]
                        FROM FinancialTransactions t
                        INNER JOIN Students s ON t.StudentID = s.StudentID
                        ORDER BY t.TransactionID DESC";

                    using (SqlCommand cmd = new SqlCommand(recentQuery, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvRecentTransactions.DataSource = dt;
                        }
                    }

                    // 2. Fetch active courses list & enrollments/cost
                    string courseQuery = @"
                        SELECT TOP 7
                            CourseName AS [اسم الدورة],
                            Cost AS [السعر المعتمد (د.ل)]
                        FROM Courses
                        ORDER BY CourseID DESC";

                    using (SqlCommand cmd = new SqlCommand(courseQuery, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvCourseDistribution.DataSource = dt;
                        }
                    }

                    // Format columns modes
                    dgvRecentTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvCourseDistribution.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // Set custom column weights and minimum widths to prevent timestamp and value text truncation
                    if (dgvRecentTransactions.Columns.Count >= 4)
                    {
                        dgvRecentTransactions.Columns[0].FillWeight = 120; // الطالب
                        dgvRecentTransactions.Columns[1].FillWeight = 90;  // العملية
                        dgvRecentTransactions.Columns[2].FillWeight = 90;  // القيمة (د.ل)
                        dgvRecentTransactions.Columns[3].FillWeight = 100; // الوقت

                        dgvRecentTransactions.Columns[0].MinimumWidth = 100;
                        dgvRecentTransactions.Columns[1].MinimumWidth = 80;
                        dgvRecentTransactions.Columns[2].MinimumWidth = 80;
                        dgvRecentTransactions.Columns[3].MinimumWidth = 90;
                    }

                    // Format grids
                    StyleDashboardGrid(dgvRecentTransactions);
                    StyleDashboardGrid(dgvCourseDistribution);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadRecentActivity failed: " + ex.Message);
            }
        }

        private void StyleDashboardGrid(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Header styling
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(15, 23, 42);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(241, 245, 249);
            dgv.ColumnHeadersHeight = 35;

            // Row styling
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255); // light blue selection
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(37, 99, 235);
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(15, 23, 42);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
        }

        private void DrawCardBorder(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(Color.FromArgb(226, 232, 240), 1))
            {
                e.Graphics.DrawRectangle(p, 0, 0, ((Panel)sender).Width - 1, ((Panel)sender).Height - 1);
            }
        }

        private void DrawHeaderBottomBorder(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(Color.FromArgb(226, 232, 240), 1))
            {
                e.Graphics.DrawLine(p, 0, headerPanel.Height - 1, headerPanel.Width, headerPanel.Height - 1);
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
            paymentsViewPanel.Visible = (activePanel == paymentsViewPanel);
            usersViewPanel.Visible = (activePanel == usersViewPanel);
        }

        /// <summary>
        /// Highlights the currently active sidebar navigation button.
        /// </summary>
        private void SetActiveButton(Button activeBtn)
        {
            Color darkText = Color.FromArgb(51, 65, 85);      // #334155
            Color hoverBg = Color.FromArgb(226, 232, 240);    // #E2E8F0

            Button[] buttons = { btnHome, btnStudents, btnCourses, btnStudentDues, btnBalanceReport, btnPayments, btnUsers, btnSettings };
            foreach (var btn in buttons)
            {
                if (btn == null) continue;
                btn.BackColor = Color.Transparent;
                btn.ForeColor = darkText;
                btn.FlatAppearance.MouseOverBackColor = hoverBg;
            }

            // Highlight the selected button with the Accent blue color (#2563EB)
            if (activeBtn != null)
            {
                activeBtn.BackColor = Color.FromArgb(37, 99, 235); // #2563EB
                activeBtn.ForeColor = Color.White;
                activeBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            }
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

        private void BtnPayments_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnPayments);
            ShowPanel(paymentsViewPanel);
            uPaymentsView.LoadStudentsList();
            uPaymentsView.LoadTransactions();
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

        private void BtnUsers_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnUsers);
            ShowPanel(usersViewPanel);
            uUsersView.LoadUsers();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            UserSession.Clear();
            this.Close();
        }

        private void BtnRefreshData_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        private void TxtGlobalSearch_Enter(object sender, EventArgs e)
        {
            if (txtGlobalSearch.Text == "بحث سريع في المنظومة...")
            {
                txtGlobalSearch.Text = "";
                txtGlobalSearch.ForeColor = Color.FromArgb(15, 23, 42); // #0F172A
            }
        }

        private void TxtGlobalSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGlobalSearch.Text))
            {
                txtGlobalSearch.Text = "بحث سريع في المنظومة...";
                txtGlobalSearch.ForeColor = Color.FromArgb(100, 116, 139); // #64748B
            }
        }

        private void TxtGlobalSearch_TextChanged(object sender, EventArgs e)
        {
            string searchVal = txtGlobalSearch.Text.Trim();
            if (searchVal == "بحث سريع في المنظومة...")
                searchVal = "";

            uStudentsView.SetSearchText(searchVal);
            uCoursesView.SetSearchText(searchVal);
            uPaymentsView.SetSearchText(searchVal);
            uBalanceReportView.SetSearchText(searchVal);
            uUsersView.SetSearchText(searchVal);
        }

        private void BtnQuickAddStudent_Click(object sender, EventArgs e)
        {
            // Switch to students tab and focus student name
            SetActiveButton(btnStudents);
            ShowPanel(studentsViewPanel);
            uStudentsView.LoadStudents();

            // Find txtName control in uStudentsView to focus it!
            var txtNameCtrl = uStudentsView.Controls.Find("txtName", true);
            if (txtNameCtrl.Length > 0 && txtNameCtrl[0] is TextBox)
            {
                txtNameCtrl[0].Focus();
            }
        }

        private void BtnQuickPayment_Click(object sender, EventArgs e)
        {
            // Switch to payments tab and focus amount textbox
            SetActiveButton(btnPayments);
            ShowPanel(paymentsViewPanel);
            uPaymentsView.LoadStudentsList();
            uPaymentsView.LoadTransactions();

            var txtAmountCtrl = uPaymentsView.Controls.Find("txtAmount", true);
            if (txtAmountCtrl.Length > 0 && txtAmountCtrl[0] is TextBox)
            {
                txtAmountCtrl[0].Focus();
            }
        }
    }
}