using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SchoolCenter
{
    public class UsersView : UserControl
    {
        private Panel pnlInput;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Label lblRole;
        private ComboBox cmbRole;
        private CheckBox chkIsActive;

        // Permissions Checkboxes
        private GroupBox grpPermissions;
        private CheckBox chkCanManageStudents;
        private CheckBox chkCanManageCourses;
        private CheckBox chkCanAssignDues;
        private CheckBox chkCanReceivePayments;
        private CheckBox chkCanViewReports;
        private CheckBox chkCanManageUsers;

        private Button btnSave;
        private Button btnUpdate;
        private Button btnClear;

        private Panel pnlSearch;
        private Label lblSearch;
        private TextBox txtSearch;

        private Panel pnlGrid;
        private DataGridView dgvUsers;

        private int selectedUserID = -1;

        public event EventHandler DataSaved;

        public UsersView()
        {
            InitializeComponent();
            ThemeHelper.ApplyTheme(this);
            LoadUsers();
        }

        private void InitializeComponent()
        {
            this.pnlInput = new Panel();
            this.lblUsername = new Label();
            this.txtUsername = new TextBox();
            this.lblPassword = new Label();
            this.txtPassword = new TextBox();
            this.lblRole = new Label();
            this.cmbRole = new ComboBox();
            this.chkIsActive = new CheckBox();

            this.grpPermissions = new GroupBox();
            this.chkCanManageStudents = new CheckBox();
            this.chkCanManageCourses = new CheckBox();
            this.chkCanAssignDues = new CheckBox();
            this.chkCanReceivePayments = new CheckBox();
            this.chkCanViewReports = new CheckBox();
            this.chkCanManageUsers = new CheckBox();

            this.btnSave = new Button();
            this.btnUpdate = new Button();
            this.btnClear = new Button();

            this.pnlSearch = new Panel();
            this.lblSearch = new Label();
            this.txtSearch = new TextBox();

            this.pnlGrid = new Panel();
            this.dgvUsers = new DataGridView();

            this.pnlInput.SuspendLayout();
            this.grpPermissions.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();

            //
            // UsersView
            //
            this.BackColor = Color.FromArgb(248, 249, 250); // Off-White
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlInput);
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 10F);
            this.Name = "UsersView";
            this.RightToLeft = RightToLeft.Yes;
            this.Size = new Size(820, 600);

            //
            // pnlInput
            //
            this.pnlInput.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            this.pnlInput.BackColor = Color.White;
            this.pnlInput.Controls.Add(this.lblUsername);
            this.pnlInput.Controls.Add(this.txtUsername);
            this.pnlInput.Controls.Add(this.lblPassword);
            this.pnlInput.Controls.Add(this.txtPassword);
            this.pnlInput.Controls.Add(this.lblRole);
            this.pnlInput.Controls.Add(this.cmbRole);
            this.pnlInput.Controls.Add(this.chkIsActive);
            this.pnlInput.Controls.Add(this.grpPermissions);
            this.pnlInput.Controls.Add(this.btnSave);
            this.pnlInput.Controls.Add(this.btnUpdate);
            this.pnlInput.Controls.Add(this.btnClear);
            this.pnlInput.Location = new Point(20, 20);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new Size(780, 250);
            this.pnlInput.TabIndex = 0;

            //
            // lblUsername
            //
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblUsername.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblUsername.Location = new Point(640, 20);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new Size(125, 23);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "اسم المستخدم:";

            //
            // txtUsername
            //
            this.txtUsername.Location = new Point(410, 17);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new Size(220, 30);
            this.txtUsername.TabIndex = 0;

            //
            // lblPassword
            //
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblPassword.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblPassword.Location = new Point(640, 65);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new Size(94, 23);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "كلمة المرور:";

            //
            // txtPassword
            //
            this.txtPassword.Location = new Point(410, 62);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new Size(220, 30);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.UseSystemPasswordChar = true;

            //
            // lblRole
            //
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblRole.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblRole.Location = new Point(640, 110);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new Size(59, 23);
            this.lblRole.TabIndex = 2;
            this.lblRole.Text = "الدور:";

            //
            // cmbRole
            //
            this.cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Items.AddRange(new object[] {
            "Admin",
            "Accountant",
            "Receptionist"});
            this.cmbRole.Location = new Point(410, 107);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new Size(220, 31);
            this.cmbRole.TabIndex = 2;

            //
            // chkIsActive
            //
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = CheckState.Checked;
            this.chkIsActive.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.chkIsActive.ForeColor = Color.FromArgb(44, 62, 80);
            this.chkIsActive.Location = new Point(553, 155);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new Size(77, 27);
            this.chkIsActive.TabIndex = 3;
            this.chkIsActive.Text = "نشط؟";
            this.chkIsActive.UseVisualStyleBackColor = true;

            //
            // grpPermissions
            //
            this.grpPermissions.Controls.Add(this.chkCanManageStudents);
            this.grpPermissions.Controls.Add(this.chkCanManageCourses);
            this.grpPermissions.Controls.Add(this.chkCanAssignDues);
            this.grpPermissions.Controls.Add(this.chkCanReceivePayments);
            this.grpPermissions.Controls.Add(this.chkCanViewReports);
            this.grpPermissions.Controls.Add(this.chkCanManageUsers);
            this.grpPermissions.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.grpPermissions.ForeColor = Color.FromArgb(44, 62, 80);
            this.grpPermissions.Location = new Point(20, 10);
            this.grpPermissions.Name = "grpPermissions";
            this.grpPermissions.Size = new Size(370, 220);
            this.grpPermissions.TabIndex = 4;
            this.grpPermissions.TabStop = false;
            this.grpPermissions.Text = "الصلاحيات التفصيلية (Permissions)";

            //
            // chkCanManageStudents
            //
            this.chkCanManageStudents.AutoSize = true;
            this.chkCanManageStudents.Checked = true;
            this.chkCanManageStudents.CheckState = CheckState.Checked;
            this.chkCanManageStudents.Font = new Font("Segoe UI", 9F);
            this.chkCanManageStudents.Location = new Point(190, 35);
            this.chkCanManageStudents.Name = "chkCanManageStudents";
            this.chkCanManageStudents.Size = new Size(160, 24);
            this.chkCanManageStudents.TabIndex = 4;
            this.chkCanManageStudents.Text = "إدارة بيانات الطلاب";
            this.chkCanManageStudents.UseVisualStyleBackColor = true;

            //
            // chkCanManageCourses
            //
            this.chkCanManageCourses.AutoSize = true;
            this.chkCanManageCourses.Checked = true;
            this.chkCanManageCourses.CheckState = CheckState.Checked;
            this.chkCanManageCourses.Font = new Font("Segoe UI", 9F);
            this.chkCanManageCourses.Location = new Point(190, 75);
            this.chkCanManageCourses.Name = "chkCanManageCourses";
            this.chkCanManageCourses.Size = new Size(160, 24);
            this.chkCanManageCourses.TabIndex = 5;
            this.chkCanManageCourses.Text = "إدارة الدورات التدريبية";
            this.chkCanManageCourses.UseVisualStyleBackColor = true;

            //
            // chkCanAssignDues
            //
            this.chkCanAssignDues.AutoSize = true;
            this.chkCanAssignDues.Checked = true;
            this.chkCanAssignDues.CheckState = CheckState.Checked;
            this.chkCanAssignDues.Font = new Font("Segoe UI", 9F);
            this.chkCanAssignDues.Location = new Point(190, 115);
            this.chkCanAssignDues.Name = "chkCanAssignDues";
            this.chkCanAssignDues.Size = new Size(160, 24);
            this.chkCanAssignDues.TabIndex = 6;
            this.chkCanAssignDues.Text = "إضافة المستحقات المالية";
            this.chkCanAssignDues.UseVisualStyleBackColor = true;

            //
            // chkCanReceivePayments
            //
            this.chkCanReceivePayments.AutoSize = true;
            this.chkCanReceivePayments.Checked = true;
            this.chkCanReceivePayments.CheckState = CheckState.Checked;
            this.chkCanReceivePayments.Font = new Font("Segoe UI", 9F);
            this.chkCanReceivePayments.Location = new Point(15, 35);
            this.chkCanReceivePayments.Name = "chkCanReceivePayments";
            this.chkCanReceivePayments.Size = new Size(165, 24);
            this.chkCanReceivePayments.TabIndex = 7;
            this.chkCanReceivePayments.Text = "سندات القبض والخزينة";
            this.chkCanReceivePayments.UseVisualStyleBackColor = true;

            //
            // chkCanViewReports
            //
            this.chkCanViewReports.AutoSize = true;
            this.chkCanViewReports.Font = new Font("Segoe UI", 9F);
            this.chkCanViewReports.Location = new Point(15, 75);
            this.chkCanViewReports.Name = "chkCanViewReports";
            this.chkCanViewReports.Size = new Size(165, 24);
            this.chkCanViewReports.TabIndex = 8;
            this.chkCanViewReports.Text = "تقرير الأرصدة والتصدير";
            this.chkCanViewReports.UseVisualStyleBackColor = true;

            //
            // chkCanManageUsers
            //
            this.chkCanManageUsers.AutoSize = true;
            this.chkCanManageUsers.Font = new Font("Segoe UI", 9F);
            this.chkCanManageUsers.Location = new Point(15, 115);
            this.chkCanManageUsers.Name = "chkCanManageUsers";
            this.chkCanManageUsers.Size = new Size(165, 24);
            this.chkCanManageUsers.TabIndex = 9;
            this.chkCanManageUsers.Text = "إدارة حسابات المستخدمين";
            this.chkCanManageUsers.UseVisualStyleBackColor = true;

            //
            // btnSave
            //
            this.btnSave.BackColor = Color.FromArgb(52, 152, 219); // Modern Blue
            this.btnSave.Cursor = Cursors.Hand;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(640, 195);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(120, 35);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "حفظ";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new EventHandler(this.BtnSave_Click);

            //
            // btnUpdate
            //
            this.btnUpdate.BackColor = Color.FromArgb(46, 204, 113); // Soft Success Green
            this.btnUpdate.Cursor = Cursors.Hand;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = FlatStyle.Flat;
            this.btnUpdate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnUpdate.ForeColor = Color.White;
            this.btnUpdate.Location = new Point(510, 195);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new Size(120, 35);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "تعديل";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new EventHandler(this.BtnUpdate_Click);

            //
            // btnClear
            //
            this.btnClear.BackColor = Color.FromArgb(127, 140, 141); // Gray
            this.btnClear.Cursor = Cursors.Hand;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnClear.ForeColor = Color.White;
            this.btnClear.Location = new Point(410, 195);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(94, 35);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "مسح";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new EventHandler(this.BtnClear_Click);

            //
            // pnlSearch
            //
            this.pnlSearch.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            this.pnlSearch.BackColor = Color.White;
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Location = new Point(20, 285);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new Size(780, 50);
            this.pnlSearch.TabIndex = 1;

            //
            // lblSearch
            //
            this.lblSearch.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblSearch.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblSearch.Location = new Point(620, 13);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new Size(140, 23);
            this.lblSearch.Text = "بحث باسم أو الدور:";

            //
            // txtSearch
            //
            this.txtSearch.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            this.txtSearch.Location = new Point(30, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(580, 30);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new EventHandler(this.TxtSearch_TextChanged);

            //
            // pnlGrid
            //
            this.pnlGrid.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            this.pnlGrid.Controls.Add(this.dgvUsers);
            this.pnlGrid.Location = new Point(20, 350);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new Size(780, 230);
            this.pnlGrid.TabIndex = 2;

            //
            // dgvUsers
            //
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 244, 244);
            this.dgvUsers.BackgroundColor = Color.White;
            this.dgvUsers.BorderStyle = BorderStyle.None;
            this.dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            this.dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            this.dgvUsers.DefaultCellStyle.SelectionForeColor = Color.White;
            this.dgvUsers.EnableHeadersVisualStyles = false;
            this.dgvUsers.Dock = DockStyle.Fill;
            this.dgvUsers.Location = new Point(0, 0);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.CellClick += new DataGridViewCellEventHandler(this.DgvUsers_CellClick);

            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.grpPermissions.ResumeLayout(false);
            this.grpPermissions.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);
        }

        public void LoadUsers()
        {
            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT
                            UserID AS [الرقم الآلي],
                            Username AS [اسم المستخدم],
                            Role AS [الدور الوظيفي],
                            CASE WHEN IsActive = 1 THEN N'نشط' ELSE N'غير نشط' END AS [الحالة]
                        FROM Users
                        ORDER BY UserID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvUsers.DataSource = dt;

                            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadUsers failed: " + ex.Message);
            }
        }

        private void ClearInputs()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            cmbRole.SelectedIndex = -1;
            chkIsActive.Checked = true;

            // Reset Permissions Checkboxes
            chkCanManageStudents.Checked = true;
            chkCanManageCourses.Checked = true;
            chkCanAssignDues.Checked = true;
            chkCanReceivePayments.Checked = true;
            chkCanViewReports.Checked = false;
            chkCanManageUsers.Checked = false;

            selectedUserID = -1;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("يرجى إدخال اسم المستخدم، كلمة المرور، وتحديد الدور.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if username already exists
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @user";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim());
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("اسم المستخدم هذا مسجل مسبقاً في النظام.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            int newUserID = -1;
                            string queryUser = @"
                                INSERT INTO Users (Username, PasswordHash, Role, IsActive)
                                VALUES (@user, @pass, @role, @active);
                                SELECT SCOPE_IDENTITY();";

                            using (SqlCommand cmdUser = new SqlCommand(queryUser, conn, trans))
                            {
                                cmdUser.Parameters.AddWithValue("@user", txtUsername.Text.Trim());
                                cmdUser.Parameters.AddWithValue("@pass", txtPassword.Text.Trim());
                                cmdUser.Parameters.AddWithValue("@role", cmbRole.SelectedItem.ToString());
                                cmdUser.Parameters.AddWithValue("@active", chkIsActive.Checked ? 1 : 0);

                                object res = cmdUser.ExecuteScalar();
                                if (res != null && res != DBNull.Value)
                                {
                                    newUserID = Convert.ToInt32(res);
                                }
                            }

                            if (newUserID != -1)
                            {
                                string queryPerm = @"
                                    INSERT INTO UserPermissions (UserID, CanManageStudents, CanManageCourses, CanAssignDues, CanReceivePayments, CanViewReports, CanManageUsers)
                                    VALUES (@UserID, @CanStudents, @CanCourses, @CanDues, @CanPayments, @CanReports, @CanUsers)";

                                using (SqlCommand cmdPerm = new SqlCommand(queryPerm, conn, trans))
                                {
                                    cmdPerm.Parameters.AddWithValue("@UserID", newUserID);
                                    cmdPerm.Parameters.AddWithValue("@CanStudents", chkCanManageStudents.Checked ? 1 : 0);
                                    cmdPerm.Parameters.AddWithValue("@CanCourses", chkCanManageCourses.Checked ? 1 : 0);
                                    cmdPerm.Parameters.AddWithValue("@CanDues", chkCanAssignDues.Checked ? 1 : 0);
                                    cmdPerm.Parameters.AddWithValue("@CanPayments", chkCanReceivePayments.Checked ? 1 : 0);
                                    cmdPerm.Parameters.AddWithValue("@CanReports", chkCanViewReports.Checked ? 1 : 0);
                                    cmdPerm.Parameters.AddWithValue("@CanUsers", chkCanManageUsers.Checked ? 1 : 0);

                                    cmdPerm.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }

                MessageBox.Show("تم إضافة المستخدم وصلاحياته بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
                LoadUsers();
                OnDataSaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء إضافة المستخدم: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedUserID == -1)
            {
                MessageBox.Show("يرجى تحديد مستخدم من الجدول لتعديله.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text) || cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("يرجى إدخال اسم المستخدم وتحديد الدور.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if username is used by another user
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @user AND UserID <> @UserID";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim());
                        checkCmd.Parameters.AddWithValue("@UserID", selectedUserID);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("اسم المستخدم هذا مستخدم بالفعل من قبل مستخدم آخر.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            // Update User table details
                            string queryUser;
                            bool updatePassword = !string.IsNullOrWhiteSpace(txtPassword.Text);

                            if (updatePassword)
                            {
                                queryUser = "UPDATE Users SET Username = @user, PasswordHash = @pass, Role = @role, IsActive = @active WHERE UserID = @UserID";
                            }
                            else
                            {
                                queryUser = "UPDATE Users SET Username = @user, Role = @role, IsActive = @active WHERE UserID = @UserID";
                            }

                            using (SqlCommand cmdUser = new SqlCommand(queryUser, conn, trans))
                            {
                                cmdUser.Parameters.AddWithValue("@user", txtUsername.Text.Trim());
                                if (updatePassword)
                                {
                                    cmdUser.Parameters.AddWithValue("@pass", txtPassword.Text.Trim());
                                }
                                cmdUser.Parameters.AddWithValue("@role", cmbRole.SelectedItem.ToString());
                                cmdUser.Parameters.AddWithValue("@active", chkIsActive.Checked ? 1 : 0);
                                cmdUser.Parameters.AddWithValue("@UserID", selectedUserID);

                                cmdUser.ExecuteNonQuery();
                            }

                            // Update or insert permissions
                            string queryCheckPerm = "SELECT COUNT(*) FROM UserPermissions WHERE UserID = @UserID";
                            int permCount = 0;
                            using (SqlCommand cmdCheckPerm = new SqlCommand(queryCheckPerm, conn, trans))
                            {
                                cmdCheckPerm.Parameters.AddWithValue("@UserID", selectedUserID);
                                permCount = Convert.ToInt32(cmdCheckPerm.ExecuteScalar());
                            }

                            if (permCount > 0)
                            {
                                string queryUpdatePerm = @"
                                    UPDATE UserPermissions
                                    SET CanManageStudents = @CanStudents,
                                        CanManageCourses = @CanCourses,
                                        CanAssignDues = @CanDues,
                                        CanReceivePayments = @CanPayments,
                                        CanViewReports = @CanReports,
                                        CanManageUsers = @CanUsers
                                    WHERE UserID = @UserID";

                                using (SqlCommand cmdUpdatePerm = new SqlCommand(queryUpdatePerm, conn, trans))
                                {
                                    cmdUpdatePerm.Parameters.AddWithValue("@CanStudents", chkCanManageStudents.Checked ? 1 : 0);
                                    cmdUpdatePerm.Parameters.AddWithValue("@CanCourses", chkCanManageCourses.Checked ? 1 : 0);
                                    cmdUpdatePerm.Parameters.AddWithValue("@CanDues", chkCanAssignDues.Checked ? 1 : 0);
                                    cmdUpdatePerm.Parameters.AddWithValue("@CanPayments", chkCanReceivePayments.Checked ? 1 : 0);
                                    cmdUpdatePerm.Parameters.AddWithValue("@CanReports", chkCanViewReports.Checked ? 1 : 0);
                                    cmdUpdatePerm.Parameters.AddWithValue("@CanUsers", chkCanManageUsers.Checked ? 1 : 0);
                                    cmdUpdatePerm.Parameters.AddWithValue("@UserID", selectedUserID);

                                    cmdUpdatePerm.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                string queryInsertPerm = @"
                                    INSERT INTO UserPermissions (UserID, CanManageStudents, CanManageCourses, CanAssignDues, CanReceivePayments, CanViewReports, CanManageUsers)
                                    VALUES (@UserID, @CanStudents, @CanCourses, @CanDues, @CanPayments, @CanReports, @CanUsers)";

                                using (SqlCommand cmdInsertPerm = new SqlCommand(queryInsertPerm, conn, trans))
                                {
                                    cmdInsertPerm.Parameters.AddWithValue("@UserID", selectedUserID);
                                    cmdInsertPerm.Parameters.AddWithValue("@CanStudents", chkCanManageStudents.Checked ? 1 : 0);
                                    cmdInsertPerm.Parameters.AddWithValue("@CanCourses", chkCanManageCourses.Checked ? 1 : 0);
                                    cmdInsertPerm.Parameters.AddWithValue("@CanDues", chkCanAssignDues.Checked ? 1 : 0);
                                    cmdInsertPerm.Parameters.AddWithValue("@CanPayments", chkCanReceivePayments.Checked ? 1 : 0);
                                    cmdInsertPerm.Parameters.AddWithValue("@CanReports", chkCanViewReports.Checked ? 1 : 0);
                                    cmdInsertPerm.Parameters.AddWithValue("@CanUsers", chkCanManageUsers.Checked ? 1 : 0);

                                    cmdInsertPerm.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }

                MessageBox.Show("تم تعديل بيانات المستخدم وصلاحياته بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
                LoadUsers();
                OnDataSaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تعديل المستخدم: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(filterText))
            {
                LoadUsers();
                return;
            }

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT
                            UserID AS [الرقم الآلي],
                            Username AS [اسم المستخدم],
                            Role AS [الدور الوظيفي],
                            CASE WHEN IsActive = 1 THEN N'نشط' ELSE N'غير نشط' END AS [الحالة]
                        FROM Users
                        WHERE Username LIKE @Filter OR Role LIKE @Filter
                        ORDER BY UserID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Filter", "%" + filterText + "%");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvUsers.DataSource = dt;
                            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Search users failed: " + ex.Message);
            }
        }

        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                selectedUserID = Convert.ToInt32(row.Cells["الرقم الآلي"].Value);
                txtUsername.Text = row.Cells["اسم المستخدم"].Value.ToString();
                txtPassword.Clear(); // Leave password field clear for security or to update it

                string role = row.Cells["الدور الوظيفي"].Value.ToString();
                if (cmbRole.Items.Contains(role))
                {
                    cmbRole.SelectedItem = role;
                }
                else
                {
                    cmbRole.SelectedIndex = -1;
                }

                string status = row.Cells["الحالة"].Value.ToString();
                chkIsActive.Checked = (status == "نشط");

                // Fetch permissions for this selected user
                try
                {
                    string connectionString = DbConnectionManager.GetConnectionString();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = @"
                            SELECT CanManageStudents, CanManageCourses, CanAssignDues,
                                   CanReceivePayments, CanViewReports, CanManageUsers
                            FROM UserPermissions
                            WHERE UserID = @UserID";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserID", selectedUserID);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    chkCanManageStudents.Checked = Convert.ToBoolean(reader["CanManageStudents"]);
                                    chkCanManageCourses.Checked = Convert.ToBoolean(reader["CanManageCourses"]);
                                    chkCanAssignDues.Checked = Convert.ToBoolean(reader["CanAssignDues"]);
                                    chkCanReceivePayments.Checked = Convert.ToBoolean(reader["CanReceivePayments"]);
                                    chkCanViewReports.Checked = Convert.ToBoolean(reader["CanViewReports"]);
                                    chkCanManageUsers.Checked = Convert.ToBoolean(reader["CanManageUsers"]);
                                }
                                else
                                {
                                    // Default permissions fallback if not found
                                    chkCanManageStudents.Checked = true;
                                    chkCanManageCourses.Checked = true;
                                    chkCanAssignDues.Checked = true;
                                    chkCanReceivePayments.Checked = true;
                                    chkCanViewReports.Checked = false;
                                    chkCanManageUsers.Checked = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Fetch permissions failed: " + ex.Message);
                }
            }
        }

        protected virtual void OnDataSaved()
        {
            if (DataSaved != null)
            {
                DataSaved(this, EventArgs.Empty);
            }
        }
    }
}
