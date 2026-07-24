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

        private void LoadBranding()
        {
            try
            {
                Tuple<string, Image> settings = SettingsService.GetSettings();
                lblLogo.Text = "⚡ " + settings.Item1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadBranding failed: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Set automatically maximized upon load
                this.WindowState = FormWindowState.Maximized;

                // Explicitly set Right-to-Left layout flow and docking
                this.RightToLeftLayout = false;
                sidebarPanel.Dock = DockStyle.Right;
                mainContentPanel.Dock = DockStyle.Fill;

                // Ensure warning panel is properly parented, docked, and z-ordered
                pnlWarning.Parent = homeViewPanel;
                pnlWarning.Dock = DockStyle.Top;
                pnlWarning.BringToFront();

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

                // Load dynamic branding
                LoadBranding();

                // Wire up data saved event handlers to keep everything in sync
                uSettingsView.DataSaved += (s, ev) => {
                    LoadBranding();
                };
                uStudentsView.DataSaved += (s, ev) => {
                    LoadDashboardData();
                    uStudentDuesView.LoadStudents();
                    uBalanceReportView.LoadReport();
                    uPaymentsView.LoadStudentsList();
                    uPaymentsView.LoadTransactions();
                    uAccountStatementView.LoadStudents();
                    uAccountStatementView.LoadStatement();
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
                    uAccountStatementView.LoadStatement();
                };
                uBalanceReportView.DataSaved += (s, ev) => {
                    LoadDashboardData();
                };
                uPaymentsView.DataSaved += (s, ev) => {
                    LoadDashboardData();
                    uBalanceReportView.LoadReport();
                    uStudentDuesView.LoadDues();
                    uAccountStatementView.LoadStatement();
                };
                uUsersView.DataSaved += (s, ev) => {
                    LoadDashboardData();
                };

                // Display currently logged-in user information
                lblUserInfo.Text = UserSession.Username;
                lblRoleBadge.Text = UserSession.Role == "Admin" ? "مدير النظام" : "المحاسب المالي";
                if (!string.IsNullOrEmpty(UserSession.Username) && UserSession.Username.Length > 0)
                {
                    lblAvatarChar.Text = UserSession.Username.Substring(0, 1);
                }

                // Apply role-based screen-level permissions
                btnStudents.Visible = UserSession.CanManageStudents;
                btnCourses.Visible = UserSession.CanManageCourses;
                btnStudentDues.Visible = UserSession.CanAssignDues;
                btnPayments.Visible = UserSession.CanReceivePayments;
                btnBalanceReport.Visible = UserSession.CanViewReports;
                btnAccountStatement.Visible = UserSession.CanViewReports;
                btnUsers.Visible = (UserSession.Role == "Admin" && UserSession.CanManageUsers);
                btnSettings.Visible = (UserSession.Role == "Admin");

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
                pnlWarning.Dock = DockStyle.Top;
                pnlWarning.BringToFront();
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

                    // 2. Populate Doughnut Chart (System.Windows.Forms.DataVisualization.Charting.Chart)
                    // We will display a beautiful distribution of financial operation types / registration status
                    string chartQuery = @"
                        SELECT
                            CASE
                                WHEN TransactionType = 'Fee Charge' THEN N'دين مستحق'
                                WHEN TransactionType = 'Payment Receipt' THEN N'سداد دفعة'
                                WHEN TransactionType = 'Opening Balance' THEN N'رصيد افتتاح سابق'
                                ELSE TransactionType
                            END AS TxType,
                            COUNT(*) AS [Count]
                        FROM FinancialTransactions
                        GROUP BY TransactionType";

                    DataTable chartDt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(chartQuery, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(chartDt);
                        }
                    }

                    // Setup Chart
                    chartStatusDistribution.Series.Clear();
                    chartStatusDistribution.ChartAreas.Clear();
                    chartStatusDistribution.Legends.Clear();

                    var chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea("MainArea");
                    chartArea.BackColor = Color.White;
                    chartStatusDistribution.ChartAreas.Add(chartArea);

                    var legend = new System.Windows.Forms.DataVisualization.Charting.Legend("MainLegend");
                    legend.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
                    legend.Alignment = StringAlignment.Center;
                    legend.Font = ThemeHelper.CreateFont(8.5F);
                    chartStatusDistribution.Legends.Add(legend);

                    var series = new System.Windows.Forms.DataVisualization.Charting.Series("StatusSeries");
                    series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
                    series["PieLabelStyle"] = "Disabled"; // Keep label values inside the legend to look clean
                    series.BorderColor = Color.White;
                    series.BorderWidth = 2;

                    // Set standard colors
                    Color[] paletteColors = {
                        ThemeHelper.ColorCard3Bg, // Amber
                        ThemeHelper.ColorCard2Bg, // Teal
                        ThemeHelper.ColorCard1Bg, // Blue
                        Color.Purple
                    };

                    int pIdx = 0;
                    foreach (DataRow row in chartDt.Rows)
                    {
                        string txType = row["TxType"].ToString();
                        double count = Convert.ToDouble(row["Count"]);

                        var point = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                        point.YValues = new double[] { count };
                        point.LegendText = txType + " (" + count + ")";
                        point.Color = paletteColors[pIdx % paletteColors.Length];
                        series.Points.Add(point);
                        pIdx++;
                    }

                    // Fallback sample data if empty to display gorgeous presentation
                    if (chartDt.Rows.Count == 0)
                    {
                        string[] labels = { "نشط", "مكتمل", "موقوف", "دائن" };
                        double[] values = { 12, 8, 3, 2 };
                        Color[] colors = { ThemeHelper.ColorCard1Bg, ThemeHelper.ColorCard2Bg, ThemeHelper.ColorCard4Bg, ThemeHelper.ColorCard3Bg };
                        for (int i = 0; i < labels.Length; i++)
                        {
                            var point = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                            point.YValues = new double[] { values[i] };
                            point.LegendText = labels[i] + " (" + values[i] + ")";
                            point.Color = colors[i];
                            series.Points.Add(point);
                        }
                    }

                    chartStatusDistribution.Series.Add(series);

                    // Format columns modes
                    dgvRecentTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

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

                    // Setup custom pill-badge cell formatting
                    dgvRecentTransactions.CellFormatting -= DgvRecentTransactions_CellFormatting;
                    dgvRecentTransactions.CellFormatting += DgvRecentTransactions_CellFormatting;
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

        private void DgvRecentTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvRecentTransactions.Columns[e.ColumnIndex].Name == "العملية")
            {
                if (e.Value != null)
                {
                    string status = e.Value.ToString();
                    if (status == "سداد دفعة" || status == "صالح" || status == "سداد مدفوع")
                    {
                        e.CellStyle.BackColor = ThemeHelper.ColorPillGreenBg;
                        e.CellStyle.ForeColor = ThemeHelper.ColorPillGreenText;
                        e.CellStyle.SelectionBackColor = ThemeHelper.ColorPillGreenBg;
                        e.CellStyle.SelectionForeColor = ThemeHelper.ColorPillGreenText;
                    }
                    else if (status == "تعيين دورة" || status == "مخالف" || status == "دين مستحق")
                    {
                        e.CellStyle.BackColor = ThemeHelper.ColorPillRedBg;
                        e.CellStyle.ForeColor = ThemeHelper.ColorPillRedText;
                        e.CellStyle.SelectionBackColor = ThemeHelper.ColorPillRedBg;
                        e.CellStyle.SelectionForeColor = ThemeHelper.ColorPillRedText;
                    }
                    else if (status == "رصيد افتتاح سابق" || status == "معلق")
                    {
                        e.CellStyle.BackColor = ThemeHelper.ColorPillYellowBg;
                        e.CellStyle.ForeColor = ThemeHelper.ColorPillYellowText;
                        e.CellStyle.SelectionBackColor = ThemeHelper.ColorPillYellowBg;
                        e.CellStyle.SelectionForeColor = ThemeHelper.ColorPillYellowText;
                    }
                }
            }
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
            accountStatementViewPanel.Visible = (activePanel == accountStatementViewPanel);
            settingsViewPanel.Visible = (activePanel == settingsViewPanel);

            // Dynamically update the header bar page title and page subtitle
            if (activePanel == homeViewPanel)
            {
                lblPageTitle.Text = "لوحة التحكم";
                lblPageSubtitle.Text = "نظرة عامة على النظام المالي والتعليمي";
            }
            else if (activePanel == studentsViewPanel)
            {
                lblPageTitle.Text = "إدارة الطلاب";
                lblPageSubtitle.Text = "إضافة وتعديل بيانات الطلاب ومتابعة حساباتهم";
            }
            else if (activePanel == coursesViewPanel)
            {
                lblPageTitle.Text = "إدارة الدورات";
                lblPageSubtitle.Text = "تكوين وتحديث قائمة الدورات التعليمية وتكاليفها";
            }
            else if (activePanel == studentDuesViewPanel)
            {
                lblPageTitle.Text = "تعيين المستحقات";
                lblPageSubtitle.Text = "ربط الطلاب بالدورات وتسجيل الرسوم المترتبة عليهم";
            }
            else if (activePanel == paymentsViewPanel)
            {
                lblPageTitle.Text = "إيصالات السداد";
                lblPageSubtitle.Text = "تحصيل الرسوم وتسجيل سندات القبض والإيداع المالي";
            }
            else if (activePanel == balanceReportViewPanel)
            {
                lblPageTitle.Text = "تقارير الأرصدة";
                lblPageSubtitle.Text = "كشف بالديون والالتزامات المالية المتراكمة وتصديرها";
            }
            else if (activePanel == usersViewPanel)
            {
                lblPageTitle.Text = "إدارة المستخدمين";
                lblPageSubtitle.Text = "إدارة حسابات الموظفين ومنح وتعديل الصلاحيات";
            }
            else if (activePanel == accountStatementViewPanel)
            {
                lblPageTitle.Text = "كشف حساب طالب";
                lblPageSubtitle.Text = "عرض تفصيلي للعمليات المالية والحركات السابقة للطالب";
            }
            else if (activePanel == settingsViewPanel)
            {
                lblPageTitle.Text = "إعدادات الهوية";
                lblPageSubtitle.Text = "تخصيص شعار واسم مركز الدورات التعليمية";
            }
        }

        /// <summary>
        /// Highlights the currently active sidebar navigation button.
        /// </summary>
        private void SetActiveButton(Button activeBtn)
        {
            Color lightText = Color.FromArgb(241, 245, 249);  // #F1F5F9
            Color hoverBg = Color.FromArgb(51, 65, 85);       // #334155

            Button[] buttons = { btnHome, btnStudents, btnCourses, btnStudentDues, btnBalanceReport, btnPayments, btnUsers, btnSettings, btnAccountStatement };
            foreach (var btn in buttons)
            {
                if (btn == null) continue;
                btn.BackColor = Color.Transparent;
                btn.ForeColor = lightText;
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

        private void BtnAccountStatement_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnAccountStatement);
            ShowPanel(accountStatementViewPanel);
            uAccountStatementView.LoadStudents();
            uAccountStatementView.LoadStatement();
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
            ShowPanel(settingsViewPanel);
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