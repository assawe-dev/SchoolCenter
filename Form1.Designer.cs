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
            this.btnPayments = new System.Windows.Forms.Button();
            this.btnBalanceReport = new System.Windows.Forms.Button();
            this.btnStudentDues = new System.Windows.Forms.Button();
            this.btnCourses = new System.Windows.Forms.Button();
            this.btnStudents = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.statusPanel = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.logoPanel = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.mainContentPanel = new System.Windows.Forms.Panel();
            this.homeViewPanel = new System.Windows.Forms.Panel();
            this.pnlWarning = new System.Windows.Forms.Panel();
            this.lblWarningText = new System.Windows.Forms.Label();
            this.welcomePanel = new System.Windows.Forms.Panel();
            this.lblWelcomeDesc = new System.Windows.Forms.Label();
            this.lblWelcomeTitle = new System.Windows.Forms.Label();
            this.btnRefreshData = new System.Windows.Forms.Button();
            this.cardTreasury = new System.Windows.Forms.Panel();
            this.lblCardTreasuryValue = new System.Windows.Forms.Label();
            this.lblCardTreasuryTitle = new System.Windows.Forms.Label();
            this.cardDebts = new System.Windows.Forms.Panel();
            this.lblCardDebtsTitle = new System.Windows.Forms.Label();
            this.lblCardDebtsValue = new System.Windows.Forms.Label();
            this.cardStudents = new System.Windows.Forms.Panel();
            this.lblCardStudentsValue = new System.Windows.Forms.Label();
            this.lblCardStudentsTitle = new System.Windows.Forms.Label();
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
            this.welcomePanel.SuspendLayout();
            this.cardTreasury.SuspendLayout();
            this.cardDebts.SuspendLayout();
            this.cardStudents.SuspendLayout();
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
            this.sidebarPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.sidebarPanel.Controls.Add(this.btnSettings);
            this.sidebarPanel.Controls.Add(this.btnUsers);
            this.sidebarPanel.Controls.Add(this.btnPayments);
            this.sidebarPanel.Controls.Add(this.btnBalanceReport);
            this.sidebarPanel.Controls.Add(this.btnStudentDues);
            this.sidebarPanel.Controls.Add(this.btnCourses);
            this.sidebarPanel.Controls.Add(this.btnStudents);
            this.sidebarPanel.Controls.Add(this.btnHome);
            this.sidebarPanel.Controls.Add(this.statusPanel);
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
            this.btnSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Location = new System.Drawing.Point(0, 430);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(240, 50);
            this.btnSettings.TabIndex = 8;
            this.btnSettings.Text = "إعدادات الاتصال";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.BtnSettings_Click);
            //
            // btnUsers
            //
            this.btnUsers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUsers.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUsers.FlatAppearance.BorderSize = 0;
            this.btnUsers.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsers.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnUsers.ForeColor = System.Drawing.Color.White;
            this.btnUsers.Location = new System.Drawing.Point(0, 380);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Size = new System.Drawing.Size(240, 50);
            this.btnUsers.TabIndex = 7;
            this.btnUsers.Text = "إدارة المستخدمين والصلاحيات";
            this.btnUsers.UseVisualStyleBackColor = true;
            this.btnUsers.Click += new System.EventHandler(this.BtnUsers_Click);
            //
            // btnPayments
            //
            this.btnPayments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPayments.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPayments.FlatAppearance.BorderSize = 0;
            this.btnPayments.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnPayments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayments.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPayments.ForeColor = System.Drawing.Color.White;
            this.btnPayments.Location = new System.Drawing.Point(0, 330);
            this.btnPayments.Name = "btnPayments";
            this.btnPayments.Size = new System.Drawing.Size(240, 50);
            this.btnPayments.TabIndex = 6;
            this.btnPayments.Text = "إيصالات السداد والخزينة";
            this.btnPayments.UseVisualStyleBackColor = true;
            this.btnPayments.Click += new System.EventHandler(this.BtnPayments_Click);
            //
            // btnBalanceReport
            //
            this.btnBalanceReport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBalanceReport.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBalanceReport.FlatAppearance.BorderSize = 0;
            this.btnBalanceReport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnBalanceReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBalanceReport.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnBalanceReport.ForeColor = System.Drawing.Color.White;
            this.btnBalanceReport.Location = new System.Drawing.Point(0, 280);
            this.btnBalanceReport.Name = "btnBalanceReport";
            this.btnBalanceReport.Size = new System.Drawing.Size(240, 50);
            this.btnBalanceReport.TabIndex = 5;
            this.btnBalanceReport.Text = "تقرير الأرصدة والديون";
            this.btnBalanceReport.UseVisualStyleBackColor = true;
            this.btnBalanceReport.Click += new System.EventHandler(this.BtnBalanceReport_Click);
            //
            // btnStudentDues
            //
            this.btnStudentDues.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStudentDues.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnStudentDues.FlatAppearance.BorderSize = 0;
            this.btnStudentDues.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnStudentDues.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStudentDues.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnStudentDues.ForeColor = System.Drawing.Color.White;
            this.btnStudentDues.Location = new System.Drawing.Point(0, 230);
            this.btnStudentDues.Name = "btnStudentDues";
            this.btnStudentDues.Size = new System.Drawing.Size(240, 50);
            this.btnStudentDues.TabIndex = 4;
            this.btnStudentDues.Text = "تعيين المستحقات المالية";
            this.btnStudentDues.UseVisualStyleBackColor = true;
            this.btnStudentDues.Click += new System.EventHandler(this.BtnStudentDues_Click);
            //
            // btnCourses
            //
            this.btnCourses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCourses.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCourses.FlatAppearance.BorderSize = 0;
            this.btnCourses.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnCourses.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCourses.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCourses.ForeColor = System.Drawing.Color.White;
            this.btnCourses.Location = new System.Drawing.Point(0, 180);
            this.btnCourses.Name = "btnCourses";
            this.btnCourses.Size = new System.Drawing.Size(240, 50);
            this.btnCourses.TabIndex = 3;
            this.btnCourses.Text = "إدارة الدورات";
            this.btnCourses.UseVisualStyleBackColor = true;
            this.btnCourses.Click += new System.EventHandler(this.BtnCourses_Click);
            //
            // btnStudents
            //
            this.btnStudents.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStudents.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnStudents.FlatAppearance.BorderSize = 0;
            this.btnStudents.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnStudents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStudents.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnStudents.ForeColor = System.Drawing.Color.White;
            this.btnStudents.Location = new System.Drawing.Point(0, 130);
            this.btnStudents.Name = "btnStudents";
            this.btnStudents.Size = new System.Drawing.Size(240, 50);
            this.btnStudents.TabIndex = 2;
            this.btnStudents.Text = "إدارة الطلاب";
            this.btnStudents.UseVisualStyleBackColor = true;
            this.btnStudents.Click += new System.EventHandler(this.BtnStudents_Click);
            //
            // btnHome
            //
            this.btnHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHome.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Location = new System.Drawing.Point(0, 80);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(240, 50);
            this.btnHome.TabIndex = 1;
            this.btnHome.Text = "الرئيسية";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.BtnHome_Click);
            //
            // statusPanel
            //
            this.statusPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(49)))), ((int)(((byte)(63)))));
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
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(240, 80);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "حالة الاتصال: متصل";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // logoPanel
            //
            this.logoPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(49)))), ((int)(((byte)(63)))));
            this.logoPanel.Controls.Add(this.lblLogo);
            this.logoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.logoPanel.Location = new System.Drawing.Point(0, 0);
            this.logoPanel.Name = "logoPanel";
            this.logoPanel.Size = new System.Drawing.Size(240, 80);
            this.logoPanel.TabIndex = 0;
            //
            // lblLogo
            //
            this.lblLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(0, 0);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(240, 80);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "منظومة المركز";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // mainContentPanel
            //
            //
            // headerPanel
            //
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.btnLogout);
            this.headerPanel.Controls.Add(this.lblUserInfo);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(860, 50);
            this.headerPanel.TabIndex = 0;
            //
            // lblUserInfo
            //
            this.lblUserInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUserInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblUserInfo.Location = new System.Drawing.Point(500, 12);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(340, 25);
            this.lblUserInfo.TabIndex = 0;
            this.lblUserInfo.Text = "المستخدم الحالي: -";
            this.lblUserInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // btnLogout
            //
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(20, 10);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(120, 30);
            this.btnLogout.TabIndex = 1;
            this.btnLogout.Text = "تسجيل الخروج 🚪";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.BtnLogout_Click);
            //
            // mainContentPanel
            //
            this.mainContentPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
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
            this.homeViewPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.homeViewPanel.Controls.Add(this.pnlWarning);
            this.homeViewPanel.Controls.Add(this.welcomePanel);
            this.homeViewPanel.Controls.Add(this.btnRefreshData);
            this.homeViewPanel.Controls.Add(this.cardTreasury);
            this.homeViewPanel.Controls.Add(this.cardDebts);
            this.homeViewPanel.Controls.Add(this.cardStudents);
            this.homeViewPanel.Controls.Add(this.lblHomeTitle);
            this.homeViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.homeViewPanel.Location = new System.Drawing.Point(0, 0);
            this.homeViewPanel.Name = "homeViewPanel";
            this.homeViewPanel.Size = new System.Drawing.Size(860, 700);
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
            // welcomePanel
            //
            this.welcomePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.welcomePanel.BackColor = System.Drawing.Color.White;
            this.welcomePanel.Controls.Add(this.lblWelcomeDesc);
            this.welcomePanel.Controls.Add(this.lblWelcomeTitle);
            this.welcomePanel.Location = new System.Drawing.Point(20, 400);
            this.welcomePanel.Name = "welcomePanel";
            this.welcomePanel.Size = new System.Drawing.Size(820, 280);
            this.welcomePanel.TabIndex = 4;
            //
            // lblWelcomeDesc
            //
            this.lblWelcomeDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWelcomeDesc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblWelcomeDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblWelcomeDesc.Location = new System.Drawing.Point(20, 60);
            this.lblWelcomeDesc.Name = "lblWelcomeDesc";
            this.lblWelcomeDesc.Size = new System.Drawing.Size(780, 200);
            this.lblWelcomeDesc.TabIndex = 1;
            this.lblWelcomeDesc.Text = "تتيح لك هذه المنظومة تتبع سجلات الطلاب والحركات المالية من سداد للرسوم وإيداع في الخزينة بشكل فوري وسهل. يمكنك استخدام القائمة الجانبية للتنقل بين شاشات المنظومة المختلفة.";
            //
            // lblWelcomeTitle
            //
            this.lblWelcomeTitle.AutoSize = true;
            this.lblWelcomeTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblWelcomeTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblWelcomeTitle.Location = new System.Drawing.Point(20, 20);
            this.lblWelcomeTitle.Name = "lblWelcomeTitle";
            this.lblWelcomeTitle.Size = new System.Drawing.Size(309, 28);
            this.lblWelcomeTitle.TabIndex = 0;
            this.lblWelcomeTitle.Text = "مرحباً بك في منظومة مركز الدورات";
            //
            // btnRefreshData
            //
            this.btnRefreshData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnRefreshData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefreshData.FlatAppearance.BorderSize = 0;
            this.btnRefreshData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshData.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRefreshData.ForeColor = System.Drawing.Color.White;
            this.btnRefreshData.Location = new System.Drawing.Point(20, 340);
            this.btnRefreshData.Name = "btnRefreshData";
            this.btnRefreshData.Size = new System.Drawing.Size(160, 40);
            this.btnRefreshData.TabIndex = 3;
            this.btnRefreshData.Text = "تحديث البيانات 🗘";
            this.btnRefreshData.UseVisualStyleBackColor = false;
            this.btnRefreshData.Click += new System.EventHandler(this.BtnRefreshData_Click);
            //
            // cardTreasury
            //
            this.cardTreasury.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cardTreasury.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.cardTreasury.Controls.Add(this.lblCardTreasuryValue);
            this.cardTreasury.Controls.Add(this.lblCardTreasuryTitle);
             this.cardTreasury.Location = new System.Drawing.Point(300, 180);
            this.cardTreasury.Name = "cardTreasury";
             this.cardTreasury.Size = new System.Drawing.Size(260, 140);
            this.cardTreasury.TabIndex = 2;
            //
            // lblCardTreasuryValue
            //
            this.lblCardTreasuryValue.AutoSize = true;
            this.lblCardTreasuryValue.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblCardTreasuryValue.ForeColor = System.Drawing.Color.White;
            this.lblCardTreasuryValue.Location = new System.Drawing.Point(20, 60);
            this.lblCardTreasuryValue.Name = "lblCardTreasuryValue";
            this.lblCardTreasuryValue.Size = new System.Drawing.Size(161, 60);
            this.lblCardTreasuryValue.TabIndex = 1;
            this.lblCardTreasuryValue.Text = "0.00 د.ل";
            //
            // lblCardTreasuryTitle
            //
            this.lblCardTreasuryTitle.AutoSize = true;
            this.lblCardTreasuryTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCardTreasuryTitle.ForeColor = System.Drawing.Color.White;
            this.lblCardTreasuryTitle.Location = new System.Drawing.Point(20, 20);
            this.lblCardTreasuryTitle.Name = "lblCardTreasuryTitle";
            this.lblCardTreasuryTitle.Size = new System.Drawing.Size(176, 28);
            this.lblCardTreasuryTitle.TabIndex = 0;
            this.lblCardTreasuryTitle.Text = "رصيد الخزينة الحالي";
            //
             // cardDebts
             //
             this.cardDebts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
             this.cardDebts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
             this.cardDebts.Controls.Add(this.lblCardDebtsValue);
             this.cardDebts.Controls.Add(this.lblCardDebtsTitle);
             this.cardDebts.Location = new System.Drawing.Point(20, 180);
             this.cardDebts.Name = "cardDebts";
             this.cardDebts.Size = new System.Drawing.Size(260, 140);
             this.cardDebts.TabIndex = 3;
             //
             // lblCardDebtsValue
             //
             this.lblCardDebtsValue.AutoSize = true;
             this.lblCardDebtsValue.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
             this.lblCardDebtsValue.ForeColor = System.Drawing.Color.White;
             this.lblCardDebtsValue.Location = new System.Drawing.Point(20, 60);
             this.lblCardDebtsValue.Name = "lblCardDebtsValue";
             this.lblCardDebtsValue.Size = new System.Drawing.Size(161, 60);
             this.lblCardDebtsValue.TabIndex = 1;
             this.lblCardDebtsValue.Text = "0.00 د.ل";
             //
             // lblCardDebtsTitle
             //
             this.lblCardDebtsTitle.AutoSize = true;
             this.lblCardDebtsTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
             this.lblCardDebtsTitle.ForeColor = System.Drawing.Color.White;
             this.lblCardDebtsTitle.Location = new System.Drawing.Point(20, 20);
             this.lblCardDebtsTitle.Name = "lblCardDebtsTitle";
             this.lblCardDebtsTitle.Size = new System.Drawing.Size(176, 28);
             this.lblCardDebtsTitle.TabIndex = 0;
             this.lblCardDebtsTitle.Text = "إجمالي الديون المستحقة";
             //
            // cardStudents
            //
             this.cardStudents.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cardStudents.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.cardStudents.Controls.Add(this.lblCardStudentsValue);
            this.cardStudents.Controls.Add(this.lblCardStudentsTitle);
             this.cardStudents.Location = new System.Drawing.Point(580, 180);
            this.cardStudents.Name = "cardStudents";
             this.cardStudents.Size = new System.Drawing.Size(260, 140);
            this.cardStudents.TabIndex = 1;
            //
            // lblCardStudentsValue
            //
            this.lblCardStudentsValue.AutoSize = true;
            this.lblCardStudentsValue.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblCardStudentsValue.ForeColor = System.Drawing.Color.White;
            this.lblCardStudentsValue.Location = new System.Drawing.Point(20, 60);
            this.lblCardStudentsValue.Name = "lblCardStudentsValue";
            this.lblCardStudentsValue.Size = new System.Drawing.Size(50, 60);
            this.lblCardStudentsValue.TabIndex = 1;
            this.lblCardStudentsValue.Text = "0";
            //
            // lblCardStudentsTitle
            //
            this.lblCardStudentsTitle.AutoSize = true;
            this.lblCardStudentsTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCardStudentsTitle.ForeColor = System.Drawing.Color.White;
            this.lblCardStudentsTitle.Location = new System.Drawing.Point(20, 20);
            this.lblCardStudentsTitle.Name = "lblCardStudentsTitle";
            this.lblCardStudentsTitle.Size = new System.Drawing.Size(127, 28);
            this.lblCardStudentsTitle.TabIndex = 0;
            this.lblCardStudentsTitle.Text = "إجمالي الطلاب";
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
            this.RightToLeftLayout = true;
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
            this.welcomePanel.ResumeLayout(false);
            this.welcomePanel.PerformLayout();
            this.cardTreasury.ResumeLayout(false);
            this.cardTreasury.PerformLayout();
            this.cardDebts.ResumeLayout(false);
            this.cardDebts.PerformLayout();
            this.cardStudents.ResumeLayout(false);
            this.cardStudents.PerformLayout();
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
        private System.Windows.Forms.Panel homeViewPanel;
        private System.Windows.Forms.Label lblHomeTitle;
        private System.Windows.Forms.Panel cardStudents;
        private System.Windows.Forms.Label lblCardStudentsTitle;
        private System.Windows.Forms.Label lblCardStudentsValue;
        private System.Windows.Forms.Panel cardTreasury;
        private System.Windows.Forms.Label lblCardTreasuryTitle;
        private System.Windows.Forms.Label lblCardTreasuryValue;
        private System.Windows.Forms.Panel cardDebts;
        private System.Windows.Forms.Label lblCardDebtsTitle;
        private System.Windows.Forms.Label lblCardDebtsValue;
        private System.Windows.Forms.Button btnRefreshData;
        private System.Windows.Forms.Panel welcomePanel;
        private System.Windows.Forms.Label lblWelcomeTitle;
        private System.Windows.Forms.Label lblWelcomeDesc;
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