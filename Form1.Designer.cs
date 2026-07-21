namespace SchoolCenter
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sidebarPanel = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnUsers = new System.Windows.Forms.Button();
            this.lblCatSettings = new System.Windows.Forms.Label();
            this.btnPayments = new System.Windows.Forms.Button();
            this.btnBalanceReport = new System.Windows.Forms.Button();
            this.btnStudentDues = new System.Windows.Forms.Button();
            this.lblCatFinance = new System.Windows.Forms.Label();
            this.btnCourses = new System.Windows.Forms.Button();
            this.btnStudents = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.lblCatMain = new System.Windows.Forms.Label();
            this.statusPanel = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.logoPanel = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.lblRoleBadge = new System.Windows.Forms.Label();
            this.txtGlobalSearch = new System.Windows.Forms.TextBox();
            this.btnQuickAddStudent = new System.Windows.Forms.Button();
            this.btnQuickPayment = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.mainContentPanel = new System.Windows.Forms.Panel();
            this.homeViewPanel = new System.Windows.Forms.Panel();
            this.pnlWarning = new System.Windows.Forms.Panel();
            this.lblWarningText = new System.Windows.Forms.Label();
            this.btnRefreshData = new System.Windows.Forms.Button();
            this.cardTreasury = new System.Windows.Forms.Panel();
            this.lblCardTreasuryValue = new System.Windows.Forms.Label();
            this.lblCardTreasuryTitle = new System.Windows.Forms.Label();
            this.lblCardTreasuryTag = new System.Windows.Forms.Label();
            this.cardDebts = new System.Windows.Forms.Panel();
            this.lblCardDebtsTitle = new System.Windows.Forms.Label();
            this.lblCardDebtsValue = new System.Windows.Forms.Label();
            this.lblCardDebtsTag = new System.Windows.Forms.Label();
            this.cardStudents = new System.Windows.Forms.Panel();
            this.lblCardStudentsValue = new System.Windows.Forms.Label();
            this.lblCardStudentsTitle = new System.Windows.Forms.Label();
            this.lblCardStudentsTag = new System.Windows.Forms.Label();
            this.cardCourses = new System.Windows.Forms.Panel();
            this.lblCardCoursesValue = new System.Windows.Forms.Label();
            this.lblCardCoursesTitle = new System.Windows.Forms.Label();
            this.lblCardCoursesTag = new System.Windows.Forms.Label();
            this.pnlDashboardRight = new System.Windows.Forms.Panel();
            this.lblDashboardRightTitle = new System.Windows.Forms.Label();
            this.dgvRecentTransactions = new System.Windows.Forms.DataGridView();
            this.pnlDashboardLeft = new System.Windows.Forms.Panel();
            this.lblDashboardLeftTitle = new System.Windows.Forms.Label();
            this.dgvCourseDistribution = new System.Windows.Forms.DataGridView();
            this.lblHomeTitle = new System.Windows.Forms.Label();
            this.studentsViewPanel = new System.Windows.Forms.Panel();
            this.coursesViewPanel = new System.Windows.Forms.Panel();
            this.studentDuesViewPanel = new System.Windows.Forms.Panel();
            this.balanceReportViewPanel = new System.Windows.Forms.Panel();
            this.usersViewPanel = new System.Windows.Forms.Panel();

            this.paymentsViewPanel = new System.Windows.Forms.Panel();
            this.uPaymentsView = new SchoolCenter.PaymentsView();

            this.uStudentsView = new SchoolCenter.StudentsView();
            this.uCoursesView = new SchoolCenter.CoursesView();
            this.uStudentDuesView = new SchoolCenter.StudentDuesView();
            this.uBalanceReportView = new SchoolCenter.BalanceReportView();
            this.uUsersView = new SchoolCenter.UsersView();

            this.sidebarPanel.SuspendLayout();
            this.statusPanel.SuspendLayout();
            this.logoPanel.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.mainContentPanel.SuspendLayout();
            this.homeViewPanel.SuspendLayout();
            this.pnlWarning.SuspendLayout();
            this.cardTreasury.SuspendLayout();
            this.cardDebts.SuspendLayout();
            this.cardStudents.SuspendLayout();
            this.cardCourses.SuspendLayout();
            this.pnlDashboardRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentTransactions)).BeginInit();
            this.pnlDashboardLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourseDistribution)).BeginInit();
            this.studentsViewPanel.SuspendLayout();
            this.coursesViewPanel.SuspendLayout();
            this.studentDuesViewPanel.SuspendLayout();
            this.balanceReportViewPanel.SuspendLayout();
            this.paymentsViewPanel.SuspendLayout();
            this.usersViewPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sidebarPanel
            //
            this.sidebarPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.sidebarPanel.Controls.Add(this.statusPanel);
            this.sidebarPanel.Controls.Add(this.btnSettings);
            this.sidebarPanel.Controls.Add(this.btnUsers);
            this.sidebarPanel.Controls.Add(this.lblCatSettings);
            this.sidebarPanel.Controls.Add(this.btnBalanceReport);
            this.sidebarPanel.Controls.Add(this.btnPayments);
            this.sidebarPanel.Controls.Add(this.btnStudentDues);
            this.sidebarPanel.Controls.Add(this.lblCatFinance);
            this.sidebarPanel.Controls.Add(this.btnCourses);
            this.sidebarPanel.Controls.Add(this.btnStudents);
            this.sidebarPanel.Controls.Add(this.btnHome);
            this.sidebarPanel.Controls.Add(this.lblCatMain);
            this.sidebarPanel.Controls.Add(this.logoPanel);
            this.sidebarPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.sidebarPanel.Location = new System.Drawing.Point(860, 0);
            this.sidebarPanel.Name = "sidebarPanel";
            this.sidebarPanel.Size = new System.Drawing.Size(240, 700);
            this.sidebarPanel.TabIndex = 0;
            //
            // btnSettings
            //
            this.btnSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnSettings.Location = new System.Drawing.Point(0, 430);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(240, 45);
            this.btnSettings.TabIndex = 8;
            this.btnSettings.Text = "إعدادات الاتصال ⚙️";
            this.btnSettings.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.BtnSettings_Click);
            //
            // btnUsers
            //
            this.btnUsers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUsers.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUsers.FlatAppearance.BorderSize = 0;
            this.btnUsers.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsers.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnUsers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnUsers.Location = new System.Drawing.Point(0, 380);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Size = new System.Drawing.Size(240, 45);
            this.btnUsers.TabIndex = 7;
            this.btnUsers.Text = "حسابات وصلاحيات 👤";
            this.btnUsers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUsers.UseVisualStyleBackColor = true;
            this.btnUsers.Click += new System.EventHandler(this.BtnUsers_Click);
            //
            // btnPayments
            //
            this.btnPayments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPayments.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPayments.FlatAppearance.BorderSize = 0;
            this.btnPayments.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnPayments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayments.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPayments.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnPayments.Location = new System.Drawing.Point(0, 330);
            this.btnPayments.Name = "btnPayments";
            this.btnPayments.Size = new System.Drawing.Size(240, 45);
            this.btnPayments.TabIndex = 6;
            this.btnPayments.Text = "إيصالات السداد 💳";
            this.btnPayments.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPayments.UseVisualStyleBackColor = true;
            this.btnPayments.Click += new System.EventHandler(this.BtnPayments_Click);
            //
            // btnBalanceReport
            //
            this.btnBalanceReport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBalanceReport.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBalanceReport.FlatAppearance.BorderSize = 0;
            this.btnBalanceReport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnBalanceReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBalanceReport.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnBalanceReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnBalanceReport.Location = new System.Drawing.Point(0, 280);
            this.btnBalanceReport.Name = "btnBalanceReport";
            this.btnBalanceReport.Size = new System.Drawing.Size(240, 45);
            this.btnBalanceReport.TabIndex = 5;
            this.btnBalanceReport.Text = "الأرصدة والديون 📊";
            this.btnBalanceReport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBalanceReport.UseVisualStyleBackColor = true;
            this.btnBalanceReport.Click += new System.EventHandler(this.BtnBalanceReport_Click);
            //
            // btnStudentDues
            //
            this.btnStudentDues.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStudentDues.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnStudentDues.FlatAppearance.BorderSize = 0;
            this.btnStudentDues.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnStudentDues.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStudentDues.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnStudentDues.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnStudentDues.Location = new System.Drawing.Point(0, 230);
            this.btnStudentDues.Name = "btnStudentDues";
            this.btnStudentDues.Size = new System.Drawing.Size(240, 45);
            this.btnStudentDues.TabIndex = 4;
            this.btnStudentDues.Text = "تعيين المستحقات 💸";
            this.btnStudentDues.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStudentDues.UseVisualStyleBackColor = true;
            this.btnStudentDues.Click += new System.EventHandler(this.BtnStudentDues_Click);
            //
            // btnCourses
            //
            this.btnCourses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCourses.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCourses.FlatAppearance.BorderSize = 0;
            this.btnCourses.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnCourses.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCourses.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCourses.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnCourses.Location = new System.Drawing.Point(0, 180);
            this.btnCourses.Name = "btnCourses";
            this.btnCourses.Size = new System.Drawing.Size(240, 45);
            this.btnCourses.TabIndex = 3;
            this.btnCourses.Text = "إدارة الدورات 📚";
            this.btnCourses.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCourses.UseVisualStyleBackColor = true;
            this.btnCourses.Click += new System.EventHandler(this.BtnCourses_Click);
            //
            // btnStudents
            //
            this.btnStudents.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStudents.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnStudents.FlatAppearance.BorderSize = 0;
            this.btnStudents.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnStudents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStudents.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnStudents.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnStudents.Location = new System.Drawing.Point(0, 130);
            this.btnStudents.Name = "btnStudents";
            this.btnStudents.Size = new System.Drawing.Size(240, 45);
            this.btnStudents.TabIndex = 2;
            this.btnStudents.Text = "إدارة الطلاب 👥";
            this.btnStudents.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStudents.UseVisualStyleBackColor = true;
            this.btnStudents.Click += new System.EventHandler(this.BtnStudents_Click);
            //
            // lblCatMain
            //
            this.lblCatMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCatMain.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCatMain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCatMain.Location = new System.Drawing.Point(0, 75);
            this.lblCatMain.Name = "lblCatMain";
            this.lblCatMain.Padding = new System.Windows.Forms.Padding(0, 15, 15, 5);
            this.lblCatMain.Size = new System.Drawing.Size(240, 40);
            this.lblCatMain.Text = "القائمة الرئيسية";
            this.lblCatMain.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            //
            // lblCatFinance
            //
            this.lblCatFinance.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCatFinance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCatFinance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCatFinance.Location = new System.Drawing.Point(0, 260);
            this.lblCatFinance.Name = "lblCatFinance";
            this.lblCatFinance.Padding = new System.Windows.Forms.Padding(0, 15, 15, 5);
            this.lblCatFinance.Size = new System.Drawing.Size(240, 40);
            this.lblCatFinance.Text = "الإدارة والمالية";
            this.lblCatFinance.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            //
            // lblCatSettings
            //
            this.lblCatSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCatSettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCatSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCatSettings.Location = new System.Drawing.Point(0, 445);
            this.lblCatSettings.Name = "lblCatSettings";
            this.lblCatSettings.Padding = new System.Windows.Forms.Padding(0, 15, 15, 5);
            this.lblCatSettings.Size = new System.Drawing.Size(240, 40);
            this.lblCatSettings.Text = "الإعدادات";
            this.lblCatSettings.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            //
            // btnHome
            //
            this.btnHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHome.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnHome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnHome.Location = new System.Drawing.Point(0, 80);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(240, 45);
            this.btnHome.TabIndex = 1;
            this.btnHome.Text = "الرئيسية 🏠";
            this.btnHome.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.BtnHome_Click);
            //
            // statusPanel
            //
            this.statusPanel.BackColor = System.Drawing.Color.Transparent;
            this.statusPanel.Controls.Add(this.lblStatus);
            this.statusPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusPanel.Location = new System.Drawing.Point(0, 620);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Size = new System.Drawing.Size(240, 80);
            this.statusPanel.TabIndex = 5;
            //
            // lblStatus
            //
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(240, 80);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "حالة الاتصال: متصل";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // logoPanel
            //
            this.logoPanel.BackColor = System.Drawing.Color.Transparent;
            this.logoPanel.Controls.Add(this.lblLogo);
            this.logoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.logoPanel.Location = new System.Drawing.Point(0, 0);
            this.logoPanel.Name = "logoPanel";
            this.logoPanel.Size = new System.Drawing.Size(240, 75);
            this.logoPanel.TabIndex = 0;
            //
            // lblLogo
            //
            this.lblLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblLogo.Location = new System.Drawing.Point(0, 0);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(240, 75);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "🎓 منظومة المركز التعليمي";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // mainContentPanel
            //
            //
            // headerPanel
            //
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.btnLogout);
            this.headerPanel.Controls.Add(this.btnQuickPayment);
            this.headerPanel.Controls.Add(this.btnQuickAddStudent);
            this.headerPanel.Controls.Add(this.txtGlobalSearch);
            this.headerPanel.Controls.Add(this.lblRoleBadge);
            this.headerPanel.Controls.Add(this.lblUserInfo);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(860, 75);
            this.headerPanel.TabIndex = 0;
            //
            // lblUserInfo
            //
            this.lblUserInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUserInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblUserInfo.Location = new System.Drawing.Point(580, 15);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(260, 25);
            this.lblUserInfo.TabIndex = 0;
            this.lblUserInfo.Text = "مرحباً بك، -";
            this.lblUserInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblRoleBadge
            //
            this.lblRoleBadge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRoleBadge.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.lblRoleBadge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblRoleBadge.Location = new System.Drawing.Point(580, 42);
            this.lblRoleBadge.Name = "lblRoleBadge";
            this.lblRoleBadge.Size = new System.Drawing.Size(260, 22);
            this.lblRoleBadge.TabIndex = 1;
            this.lblRoleBadge.Text = "مسؤول النظام / محاسب";
            this.lblRoleBadge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // txtGlobalSearch
            //
            this.txtGlobalSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtGlobalSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.txtGlobalSearch.Location = new System.Drawing.Point(365, 22);
            this.txtGlobalSearch.Name = "txtGlobalSearch";
            this.txtGlobalSearch.Size = new System.Drawing.Size(200, 30);
            this.txtGlobalSearch.TabIndex = 2;
            this.txtGlobalSearch.Text = "بحث سريع في المنظومة...";
            this.txtGlobalSearch.TextChanged += new System.EventHandler(this.TxtGlobalSearch_TextChanged);
            this.txtGlobalSearch.Enter += new System.EventHandler(this.TxtGlobalSearch_Enter);
            this.txtGlobalSearch.Leave += new System.EventHandler(this.TxtGlobalSearch_Leave);
            //
            // btnQuickAddStudent
            //
            this.btnQuickAddStudent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnQuickAddStudent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuickAddStudent.FlatAppearance.BorderSize = 0;
            this.btnQuickAddStudent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuickAddStudent.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnQuickAddStudent.ForeColor = System.Drawing.Color.White;
            this.btnQuickAddStudent.Location = new System.Drawing.Point(235, 21);
            this.btnQuickAddStudent.Name = "btnQuickAddStudent";
            this.btnQuickAddStudent.Size = new System.Drawing.Size(110, 32);
            this.btnQuickAddStudent.TabIndex = 3;
            this.btnQuickAddStudent.Text = "+ طالب جديد";
            this.btnQuickAddStudent.UseVisualStyleBackColor = false;
            this.btnQuickAddStudent.Click += new System.EventHandler(this.BtnQuickAddStudent_Click);
            //
            // btnQuickPayment
            //
            this.btnQuickPayment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnQuickPayment.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuickPayment.FlatAppearance.BorderSize = 0;
            this.btnQuickPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuickPayment.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnQuickPayment.ForeColor = System.Drawing.Color.White;
            this.btnQuickPayment.Location = new System.Drawing.Point(120, 21);
            this.btnQuickPayment.Name = "btnQuickPayment";
            this.btnQuickPayment.Size = new System.Drawing.Size(100, 32);
            this.btnQuickPayment.TabIndex = 4;
            this.btnQuickPayment.Text = "+ سند قبض";
            this.btnQuickPayment.UseVisualStyleBackColor = false;
            this.btnQuickPayment.Click += new System.EventHandler(this.BtnQuickPayment_Click);
            //
            // btnLogout
            //
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(25, 21);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(80, 32);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "خروج 🚪";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.BtnLogout_Click);
            //
            // mainContentPanel
            //
            this.mainContentPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.mainContentPanel.Controls.Add(this.homeViewPanel);
            this.mainContentPanel.Controls.Add(this.studentsViewPanel);
            this.mainContentPanel.Controls.Add(this.coursesViewPanel);
            this.mainContentPanel.Controls.Add(this.studentDuesViewPanel);
            this.mainContentPanel.Controls.Add(this.balanceReportViewPanel);
            this.mainContentPanel.Controls.Add(this.paymentsViewPanel);
            this.mainContentPanel.Controls.Add(this.usersViewPanel);
            this.mainContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContentPanel.Location = new System.Drawing.Point(0, 50);
            this.mainContentPanel.Name = "mainContentPanel";
            this.mainContentPanel.Size = new System.Drawing.Size(860, 650);
            this.mainContentPanel.TabIndex = 1;
            //
            // homeViewPanel
            //
            this.homeViewPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252))))); // #F8FAFC
            this.homeViewPanel.Controls.Add(this.pnlWarning);
            this.homeViewPanel.Controls.Add(this.pnlDashboardLeft);
            this.homeViewPanel.Controls.Add(this.pnlDashboardRight);
            this.homeViewPanel.Controls.Add(this.cardTreasury);
            this.homeViewPanel.Controls.Add(this.cardDebts);
            this.homeViewPanel.Controls.Add(this.cardCourses);
            this.homeViewPanel.Controls.Add(this.cardStudents);
            this.homeViewPanel.Controls.Add(this.lblHomeTitle);
            this.homeViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.homeViewPanel.Location = new System.Drawing.Point(0, 0);
            this.homeViewPanel.Name = "homeViewPanel";
            this.homeViewPanel.Size = new System.Drawing.Size(860, 650);
            this.homeViewPanel.TabIndex = 0;
            //
            // pnlWarning
            //
            this.pnlWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlWarning.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.pnlWarning.Controls.Add(this.lblWarningText);
            this.pnlWarning.Location = new System.Drawing.Point(20, 80);
            this.pnlWarning.Name = "pnlWarning";
            this.pnlWarning.Size = new System.Drawing.Size(820, 80);
            this.pnlWarning.TabIndex = 5;
            this.pnlWarning.Visible = false;
            //
            // lblWarningText
            //
            this.lblWarningText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWarningText.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblWarningText.ForeColor = System.Drawing.Color.White;
            this.lblWarningText.Location = new System.Drawing.Point(0, 0);
            this.lblWarningText.Name = "lblWarningText";
            this.lblWarningText.Size = new System.Drawing.Size(820, 80);
            this.lblWarningText.TabIndex = 0;
            this.lblWarningText.Text = "تنبيه: تعذر الاتصال بقاعدة البيانات. يرجى التحقق من إعدادات الاتصال في ملف db_config.txt.";
            this.lblWarningText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // cardStudents
            //
            this.cardStudents.BackColor = System.Drawing.Color.White;
            this.cardStudents.Controls.Add(this.lblCardStudentsTag);
            this.cardStudents.Controls.Add(this.lblCardStudentsValue);
            this.cardStudents.Controls.Add(this.lblCardStudentsTitle);
            this.cardStudents.Location = new System.Drawing.Point(650, 80);
            this.cardStudents.Name = "cardStudents";
            this.cardStudents.Size = new System.Drawing.Size(190, 140);
            this.cardStudents.TabIndex = 1;
            //
            // lblCardStudentsTitle
            //
            this.lblCardStudentsTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCardStudentsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCardStudentsTitle.Location = new System.Drawing.Point(15, 20);
            this.lblCardStudentsTitle.Name = "lblCardStudentsTitle";
            this.lblCardStudentsTitle.Size = new System.Drawing.Size(160, 24);
            this.lblCardStudentsTitle.TabIndex = 0;
            this.lblCardStudentsTitle.Text = "إجمالي الطلاب 👥";
            this.lblCardStudentsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblCardStudentsValue
            //
            this.lblCardStudentsValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblCardStudentsValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblCardStudentsValue.Location = new System.Drawing.Point(15, 50);
            this.lblCardStudentsValue.Name = "lblCardStudentsValue";
            this.lblCardStudentsValue.Size = new System.Drawing.Size(160, 45);
            this.lblCardStudentsValue.TabIndex = 1;
            this.lblCardStudentsValue.Text = "0";
            this.lblCardStudentsValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblCardStudentsTag
            //
            this.lblCardStudentsTag.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCardStudentsTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblCardStudentsTag.Location = new System.Drawing.Point(15, 102);
            this.lblCardStudentsTag.Name = "lblCardStudentsTag";
            this.lblCardStudentsTag.Size = new System.Drawing.Size(160, 24);
            this.lblCardStudentsTag.TabIndex = 2;
            this.lblCardStudentsTag.Text = "طالب مسجل";
            this.lblCardStudentsTag.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // cardCourses
            //
            this.cardCourses.BackColor = System.Drawing.Color.White;
            this.cardCourses.Controls.Add(this.lblCardCoursesTag);
            this.cardCourses.Controls.Add(this.lblCardCoursesValue);
            this.cardCourses.Controls.Add(this.lblCardCoursesTitle);
            this.cardCourses.Location = new System.Drawing.Point(440, 80);
            this.cardCourses.Name = "cardCourses";
            this.cardCourses.Size = new System.Drawing.Size(190, 140);
            this.cardCourses.TabIndex = 2;
            //
            // lblCardCoursesTitle
            //
            this.lblCardCoursesTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCardCoursesTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCardCoursesTitle.Location = new System.Drawing.Point(15, 20);
            this.lblCardCoursesTitle.Name = "lblCardCoursesTitle";
            this.lblCardCoursesTitle.Size = new System.Drawing.Size(160, 24);
            this.lblCardCoursesTitle.TabIndex = 0;
            this.lblCardCoursesTitle.Text = "الدورات النشطة 📚";
            this.lblCardCoursesTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblCardCoursesValue
            //
            this.lblCardCoursesValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblCardCoursesValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblCardCoursesValue.Location = new System.Drawing.Point(15, 50);
            this.lblCardCoursesValue.Name = "lblCardCoursesValue";
            this.lblCardCoursesValue.Size = new System.Drawing.Size(160, 45);
            this.lblCardCoursesValue.TabIndex = 1;
            this.lblCardCoursesValue.Text = "0";
            this.lblCardCoursesValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblCardCoursesTag
            //
            this.lblCardCoursesTag.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCardCoursesTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lblCardCoursesTag.Location = new System.Drawing.Point(15, 102);
            this.lblCardCoursesTag.Name = "lblCardCoursesTag";
            this.lblCardCoursesTag.Size = new System.Drawing.Size(160, 24);
            this.lblCardCoursesTag.TabIndex = 2;
            this.lblCardCoursesTag.Text = "دورة تدريبية";
            this.lblCardCoursesTag.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // cardDebts
            //
            this.cardDebts.BackColor = System.Drawing.Color.White;
            this.cardDebts.Controls.Add(this.lblCardDebtsTag);
            this.cardDebts.Controls.Add(this.lblCardDebtsValue);
            this.cardDebts.Controls.Add(this.lblCardDebtsTitle);
            this.cardDebts.Location = new System.Drawing.Point(230, 80);
            this.cardDebts.Name = "cardDebts";
            this.cardDebts.Size = new System.Drawing.Size(190, 140);
            this.cardDebts.TabIndex = 3;
            //
            // lblCardDebtsTitle
            //
            this.lblCardDebtsTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCardDebtsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCardDebtsTitle.Location = new System.Drawing.Point(15, 20);
            this.lblCardDebtsTitle.Name = "lblCardDebtsTitle";
            this.lblCardDebtsTitle.Size = new System.Drawing.Size(160, 24);
            this.lblCardDebtsTitle.TabIndex = 0;
            this.lblCardDebtsTitle.Text = "إجمالي الديون 📊";
            this.lblCardDebtsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblCardDebtsValue
            //
            this.lblCardDebtsValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblCardDebtsValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblCardDebtsValue.Location = new System.Drawing.Point(15, 50);
            this.lblCardDebtsValue.Name = "lblCardDebtsValue";
            this.lblCardDebtsValue.Size = new System.Drawing.Size(160, 45);
            this.lblCardDebtsValue.TabIndex = 1;
            this.lblCardDebtsValue.Text = "0.00 د.ل";
            this.lblCardDebtsValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblCardDebtsTag
            //
            this.lblCardDebtsTag.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCardDebtsTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.lblCardDebtsTag.Location = new System.Drawing.Point(15, 102);
            this.lblCardDebtsTag.Name = "lblCardDebtsTag";
            this.lblCardDebtsTag.Size = new System.Drawing.Size(160, 24);
            this.lblCardDebtsTag.TabIndex = 2;
            this.lblCardDebtsTag.Text = "ديون متراكمة ⚠️";
            this.lblCardDebtsTag.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // cardTreasury
            //
            this.cardTreasury.BackColor = System.Drawing.Color.White;
            this.cardTreasury.Controls.Add(this.lblCardTreasuryTag);
            this.cardTreasury.Controls.Add(this.lblCardTreasuryValue);
            this.cardTreasury.Controls.Add(this.lblCardTreasuryTitle);
            this.cardTreasury.Location = new System.Drawing.Point(20, 80);
            this.cardTreasury.Name = "cardTreasury";
            this.cardTreasury.Size = new System.Drawing.Size(190, 140);
            this.cardTreasury.TabIndex = 4;
            //
            // lblCardTreasuryTitle
            //
            this.lblCardTreasuryTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCardTreasuryTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCardTreasuryTitle.Location = new System.Drawing.Point(15, 20);
            this.lblCardTreasuryTitle.Name = "lblCardTreasuryTitle";
            this.lblCardTreasuryTitle.Size = new System.Drawing.Size(160, 24);
            this.lblCardTreasuryTitle.TabIndex = 0;
            this.lblCardTreasuryTitle.Text = "رصيد الخزينة 💳";
            this.lblCardTreasuryTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblCardTreasuryValue
            //
            this.lblCardTreasuryValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblCardTreasuryValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblCardTreasuryValue.Location = new System.Drawing.Point(15, 50);
            this.lblCardTreasuryValue.Name = "lblCardTreasuryValue";
            this.lblCardTreasuryValue.Size = new System.Drawing.Size(160, 45);
            this.lblCardTreasuryValue.TabIndex = 1;
            this.lblCardTreasuryValue.Text = "0.00 د.ل";
            this.lblCardTreasuryValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblCardTreasuryTag
            //
            this.lblCardTreasuryTag.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblCardTreasuryTag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.lblCardTreasuryTag.Location = new System.Drawing.Point(15, 102);
            this.lblCardTreasuryTag.Name = "lblCardTreasuryTag";
            this.lblCardTreasuryTag.Size = new System.Drawing.Size(160, 24);
            this.lblCardTreasuryTag.TabIndex = 2;
            this.lblCardTreasuryTag.Text = "الرصيد المتاح ✅";
            this.lblCardTreasuryTag.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // pnlDashboardRight
            //
            this.pnlDashboardRight.BackColor = System.Drawing.Color.White;
            this.pnlDashboardRight.Controls.Add(this.dgvRecentTransactions);
            this.pnlDashboardRight.Controls.Add(this.lblDashboardRightTitle);
            this.pnlDashboardRight.Location = new System.Drawing.Point(440, 240);
            this.pnlDashboardRight.Name = "pnlDashboardRight";
            this.pnlDashboardRight.Size = new System.Drawing.Size(400, 390);
            this.pnlDashboardRight.TabIndex = 5;
            //
            // lblDashboardRightTitle
            //
            this.lblDashboardRightTitle.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblDashboardRightTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblDashboardRightTitle.Location = new System.Drawing.Point(15, 15);
            this.lblDashboardRightTitle.Name = "lblDashboardRightTitle";
            this.lblDashboardRightTitle.Size = new System.Drawing.Size(370, 25);
            this.lblDashboardRightTitle.TabIndex = 0;
            this.lblDashboardRightTitle.Text = "آخر العمليات والنشاط المالي 🧾";
            this.lblDashboardRightTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // dgvRecentTransactions
            //
            this.dgvRecentTransactions.AllowUserToAddRows = false;
            this.dgvRecentTransactions.AllowUserToDeleteRows = false;
            this.dgvRecentTransactions.BackgroundColor = System.Drawing.Color.White;
            this.dgvRecentTransactions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRecentTransactions.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.dgvRecentTransactions.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.dgvRecentTransactions.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.dgvRecentTransactions.EnableHeadersVisualStyles = false;
            this.dgvRecentTransactions.Location = new System.Drawing.Point(15, 50);
            this.dgvRecentTransactions.Name = "dgvRecentTransactions";
            this.dgvRecentTransactions.ReadOnly = true;
            this.dgvRecentTransactions.RowHeadersVisible = false;
            this.dgvRecentTransactions.Size = new System.Drawing.Size(370, 325);
            this.dgvRecentTransactions.TabIndex = 1;
            //
            // pnlDashboardLeft
            //
            this.pnlDashboardLeft.BackColor = System.Drawing.Color.White;
            this.pnlDashboardLeft.Controls.Add(this.dgvCourseDistribution);
            this.pnlDashboardLeft.Controls.Add(this.lblDashboardLeftTitle);
            this.pnlDashboardLeft.Location = new System.Drawing.Point(20, 240);
            this.pnlDashboardLeft.Name = "pnlDashboardLeft";
            this.pnlDashboardLeft.Size = new System.Drawing.Size(400, 390);
            this.pnlDashboardLeft.TabIndex = 6;
            //
            // lblDashboardLeftTitle
            //
            this.lblDashboardLeftTitle.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblDashboardLeftTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblDashboardLeftTitle.Location = new System.Drawing.Point(15, 15);
            this.lblDashboardLeftTitle.Name = "lblDashboardLeftTitle";
            this.lblDashboardLeftTitle.Size = new System.Drawing.Size(370, 25);
            this.lblDashboardLeftTitle.TabIndex = 0;
            this.lblDashboardLeftTitle.Text = "دليل الدورات والتكلفة النشطة 📖";
            this.lblDashboardLeftTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // dgvCourseDistribution
            //
            this.dgvCourseDistribution.AllowUserToAddRows = false;
            this.dgvCourseDistribution.AllowUserToDeleteRows = false;
            this.dgvCourseDistribution.BackgroundColor = System.Drawing.Color.White;
            this.dgvCourseDistribution.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCourseDistribution.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.dgvCourseDistribution.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.dgvCourseDistribution.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.dgvCourseDistribution.EnableHeadersVisualStyles = false;
            this.dgvCourseDistribution.Location = new System.Drawing.Point(15, 50);
            this.dgvCourseDistribution.Name = "dgvCourseDistribution";
            this.dgvCourseDistribution.ReadOnly = true;
            this.dgvCourseDistribution.RowHeadersVisible = false;
            this.dgvCourseDistribution.Size = new System.Drawing.Size(370, 325);
            this.dgvCourseDistribution.TabIndex = 1;
            //
            // lblHomeTitle
            //
            this.lblHomeTitle.AutoSize = true;
            this.lblHomeTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHomeTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblHomeTitle.Location = new System.Drawing.Point(20, 20);
            this.lblHomeTitle.Name = "lblHomeTitle";
            this.lblHomeTitle.Size = new System.Drawing.Size(262, 37);
            this.lblHomeTitle.TabIndex = 0;
            this.lblHomeTitle.Text = "الرئيسية - لوحة التحكم";
            //
            // studentsViewPanel
            //
            this.studentsViewPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.studentsViewPanel.Controls.Add(this.uStudentsView);
            this.studentsViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.studentsViewPanel.Location = new System.Drawing.Point(0, 0);
            this.studentsViewPanel.Name = "studentsViewPanel";
            this.studentsViewPanel.Size = new System.Drawing.Size(860, 700);
            this.studentsViewPanel.TabIndex = 1;
            this.studentsViewPanel.Visible = false;
            //
            // uStudentsView
            //
            this.uStudentsView.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // coursesViewPanel
            //
            this.coursesViewPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.coursesViewPanel.Controls.Add(this.uCoursesView);
            this.coursesViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.coursesViewPanel.Location = new System.Drawing.Point(0, 0);
            this.coursesViewPanel.Name = "coursesViewPanel";
            this.coursesViewPanel.Size = new System.Drawing.Size(860, 700);
            this.coursesViewPanel.TabIndex = 2;
            this.coursesViewPanel.Visible = false;
            //
            // uCoursesView
            //
            this.uCoursesView.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // studentDuesViewPanel
            //
            this.studentDuesViewPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.studentDuesViewPanel.Controls.Add(this.uStudentDuesView);
            this.studentDuesViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.studentDuesViewPanel.Location = new System.Drawing.Point(0, 0);
            this.studentDuesViewPanel.Name = "studentDuesViewPanel";
            this.studentDuesViewPanel.Size = new System.Drawing.Size(860, 700);
            this.studentDuesViewPanel.TabIndex = 3;
            this.studentDuesViewPanel.Visible = false;
            //
            // uStudentDuesView
            //
            this.uStudentDuesView.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // balanceReportViewPanel
            //
            this.balanceReportViewPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.balanceReportViewPanel.Controls.Add(this.uBalanceReportView);
            this.balanceReportViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.balanceReportViewPanel.Location = new System.Drawing.Point(0, 0);
            this.balanceReportViewPanel.Name = "balanceReportViewPanel";
            this.balanceReportViewPanel.Size = new System.Drawing.Size(860, 700);
            this.balanceReportViewPanel.TabIndex = 4;
            this.balanceReportViewPanel.Visible = false;
            //
            // uBalanceReportView
            //
            this.uBalanceReportView.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // paymentsViewPanel
            //
            this.paymentsViewPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.paymentsViewPanel.Controls.Add(this.uPaymentsView);
            this.paymentsViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paymentsViewPanel.Location = new System.Drawing.Point(0, 0);
            this.paymentsViewPanel.Name = "paymentsViewPanel";
            this.paymentsViewPanel.Size = new System.Drawing.Size(860, 700);
            this.paymentsViewPanel.TabIndex = 5;
            this.paymentsViewPanel.Visible = false;
            //
            // uPaymentsView
            //
            this.uPaymentsView.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // usersViewPanel
            //
            this.usersViewPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.usersViewPanel.Controls.Add(this.uUsersView);
            this.usersViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usersViewPanel.Location = new System.Drawing.Point(0, 0);
            this.usersViewPanel.Name = "usersViewPanel";
            this.usersViewPanel.Size = new System.Drawing.Size(860, 650);
            this.usersViewPanel.TabIndex = 6;
            this.usersViewPanel.Visible = false;
            //
            // uUsersView
            //
            this.uUsersView.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.mainContentPanel);
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.sidebarPanel);
            this.Name = "Form1";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = false;
            this.Text = "منظومة مركز الدورات التعليمية - لوحة التحكم";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.sidebarPanel.ResumeLayout(false);
            this.statusPanel.ResumeLayout(false);
            this.logoPanel.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.mainContentPanel.ResumeLayout(false);
            this.homeViewPanel.ResumeLayout(false);
            this.homeViewPanel.PerformLayout();
            this.pnlWarning.ResumeLayout(false);
            this.cardTreasury.ResumeLayout(false);
            this.cardTreasury.PerformLayout();
            this.cardDebts.ResumeLayout(false);
            this.cardDebts.PerformLayout();
            this.cardStudents.ResumeLayout(false);
            this.cardStudents.PerformLayout();
            this.cardCourses.ResumeLayout(false);
            this.cardCourses.PerformLayout();
            this.pnlDashboardRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentTransactions)).EndInit();
            this.pnlDashboardLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourseDistribution)).EndInit();
            this.studentsViewPanel.ResumeLayout(false);
            this.coursesViewPanel.ResumeLayout(false);
            this.studentDuesViewPanel.ResumeLayout(false);
            this.balanceReportViewPanel.ResumeLayout(false);
            this.paymentsViewPanel.ResumeLayout(false);
            this.usersViewPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel sidebarPanel;
        private System.Windows.Forms.Panel logoPanel;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblCatMain;
        private System.Windows.Forms.Label lblCatFinance;
        private System.Windows.Forms.Label lblCatSettings;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnStudents;
        private System.Windows.Forms.Button btnCourses;
        private System.Windows.Forms.Button btnStudentDues;
        private System.Windows.Forms.Button btnBalanceReport;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Panel statusPanel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel mainContentPanel;
        private System.Windows.Forms.Label lblRoleBadge;
        private System.Windows.Forms.TextBox txtGlobalSearch;
        private System.Windows.Forms.Button btnQuickAddStudent;
        private System.Windows.Forms.Button btnQuickPayment;
        private System.Windows.Forms.Panel homeViewPanel;
        private System.Windows.Forms.Label lblHomeTitle;
        private System.Windows.Forms.Panel cardStudents;
        private System.Windows.Forms.Label lblCardStudentsTitle;
        private System.Windows.Forms.Label lblCardStudentsValue;
        private System.Windows.Forms.Label lblCardStudentsTag;
        private System.Windows.Forms.Panel cardCourses;
        private System.Windows.Forms.Label lblCardCoursesTitle;
        private System.Windows.Forms.Label lblCardCoursesValue;
        private System.Windows.Forms.Label lblCardCoursesTag;
        private System.Windows.Forms.Panel cardTreasury;
        private System.Windows.Forms.Label lblCardTreasuryTitle;
        private System.Windows.Forms.Label lblCardTreasuryValue;
        private System.Windows.Forms.Label lblCardTreasuryTag;
        private System.Windows.Forms.Panel cardDebts;
        private System.Windows.Forms.Label lblCardDebtsTitle;
        private System.Windows.Forms.Label lblCardDebtsValue;
        private System.Windows.Forms.Label lblCardDebtsTag;
        private System.Windows.Forms.Panel pnlDashboardRight;
        private System.Windows.Forms.Label lblDashboardRightTitle;
        private System.Windows.Forms.DataGridView dgvRecentTransactions;
        private System.Windows.Forms.Panel pnlDashboardLeft;
        private System.Windows.Forms.Label lblDashboardLeftTitle;
        private System.Windows.Forms.DataGridView dgvCourseDistribution;
        private System.Windows.Forms.Button btnRefreshData;
        private System.Windows.Forms.Panel pnlWarning;
        private System.Windows.Forms.Label lblWarningText;
        private System.Windows.Forms.Panel studentsViewPanel;
        private System.Windows.Forms.Panel coursesViewPanel;
        private System.Windows.Forms.Panel studentDuesViewPanel;
        private System.Windows.Forms.Panel balanceReportViewPanel;
        private System.Windows.Forms.Panel paymentsViewPanel;
        private System.Windows.Forms.Panel usersViewPanel;
        private System.Windows.Forms.Button btnPayments;
        private System.Windows.Forms.Button btnUsers;

        private SchoolCenter.StudentsView uStudentsView;
        private SchoolCenter.CoursesView uCoursesView;
        private SchoolCenter.StudentDuesView uStudentDuesView;
        private SchoolCenter.BalanceReportView uBalanceReportView;
        private SchoolCenter.PaymentsView uPaymentsView;
        private SchoolCenter.UsersView uUsersView;
    }
}