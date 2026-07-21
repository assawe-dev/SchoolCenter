using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SchoolCenter
{
    public class StudentsView : UserControl
    {
        private Panel pnlInput;
        private Label lblName;
        private TextBox txtName;
        private Label lblGuardianName;
        private TextBox txtGuardianName;
        private Label lblGuardianPhone;
        private TextBox txtGuardianPhone;
        private Label lblNotes;
        private TextBox txtNotes;
        private Label lblOpeningBalance;
        private TextBox txtOpeningBalance;
        private Label lblBalanceType;
        private ComboBox cmbBalanceType;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;

        private Panel pnlSearch;
        private Label lblSearch;
        private TextBox txtSearch;

        private DataGridView dgvStudents;
        private Panel pnlGrid;

        private int selectedStudentID = -1;

        public event EventHandler DataSaved;

        public StudentsView()
        {
            InitializeComponent();
            ThemeHelper.ApplyTheme(this);
            LoadStudents();
        }

        private void InitializeComponent()
        {
            this.pnlInput = new Panel();
            this.lblName = new Label();
            this.txtName = new TextBox();
            this.lblGuardianName = new Label();
            this.txtGuardianName = new TextBox();
            this.lblGuardianPhone = new Label();
            this.txtGuardianPhone = new TextBox();
            this.lblNotes = new Label();
            this.txtNotes = new TextBox();
            this.lblOpeningBalance = new Label();
            this.txtOpeningBalance = new TextBox();
            this.lblBalanceType = new Label();
            this.cmbBalanceType = new ComboBox();
            this.btnAdd = new Button();
            this.btnUpdate = new Button();
            this.btnDelete = new Button();
            this.btnClear = new Button();

            this.pnlSearch = new Panel();
            this.lblSearch = new Label();
            this.txtSearch = new TextBox();

            this.pnlGrid = new Panel();
            this.dgvStudents = new DataGridView();

            this.pnlInput.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            this.SuspendLayout();

            //
            // StudentsView
            //
            this.BackColor = Color.FromArgb(248, 250, 252); // Modern SaaS #F8FAFC
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlInput);
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 10F);
            this.Name = "StudentsView";
            this.RightToLeft = RightToLeft.Yes;
            this.Size = new Size(820, 600);

            //
            // pnlInput
            //
            this.pnlInput.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.pnlInput.BackColor = Color.White;
            this.pnlInput.Controls.Add(this.btnClear);
            this.pnlInput.Controls.Add(this.btnDelete);
            this.pnlInput.Controls.Add(this.btnUpdate);
            this.pnlInput.Controls.Add(this.btnAdd);
            this.pnlInput.Controls.Add(this.txtNotes);
            this.pnlInput.Controls.Add(this.lblNotes);
            this.pnlInput.Controls.Add(this.txtGuardianPhone);
            this.pnlInput.Controls.Add(this.lblGuardianPhone);
            this.pnlInput.Controls.Add(this.txtGuardianName);
            this.pnlInput.Controls.Add(this.lblGuardianName);
            this.pnlInput.Controls.Add(this.txtName);
            this.pnlInput.Controls.Add(this.lblName);
            this.pnlInput.Controls.Add(this.txtOpeningBalance);
            this.pnlInput.Controls.Add(this.lblOpeningBalance);
            this.pnlInput.Controls.Add(this.cmbBalanceType);
            this.pnlInput.Controls.Add(this.lblBalanceType);
            this.pnlInput.Location = new Point(20, 20);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new Size(780, 230);
            this.pnlInput.TabIndex = 0;

            //
            // lblName
            //
            this.lblName.AutoSize = true;
            this.lblName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblName.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblName.Location = new Point(650, 20);
            this.lblName.Name = "lblName";
            this.lblName.Size = new Size(110, 23);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "اسم الطالب:";

            //
            // txtName
            //
            this.txtName.Location = new Point(410, 17);
            this.txtName.Name = "txtName";
            this.txtName.Size = new Size(230, 30);
            this.txtName.TabIndex = 2;

            //
            // lblGuardianName
            //
            this.lblGuardianName.AutoSize = true;
            this.lblGuardianName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblGuardianName.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblGuardianName.Location = new Point(290, 20);
            this.lblGuardianName.Name = "lblGuardianName";
            this.lblGuardianName.Size = new Size(115, 23);
            this.lblGuardianName.TabIndex = 3;
            this.lblGuardianName.Text = "اسم ولي الأمر:";

            //
            // txtGuardianName
            //
            this.txtGuardianName.Location = new Point(40, 17);
            this.txtGuardianName.Name = "txtGuardianName";
            this.txtGuardianName.Size = new Size(240, 30);
            this.txtGuardianName.TabIndex = 4;

            //
            // lblGuardianPhone
            //
            this.lblGuardianPhone.AutoSize = true;
            this.lblGuardianPhone.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblGuardianPhone.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblGuardianPhone.Location = new Point(650, 65);
            this.lblGuardianPhone.Name = "lblGuardianPhone";
            this.lblGuardianPhone.Size = new Size(115, 23);
            this.lblGuardianPhone.TabIndex = 5;
            this.lblGuardianPhone.Text = "رقم ولي الأمر:";

            //
            // txtGuardianPhone
            //
            this.txtGuardianPhone.Location = new Point(410, 62);
            this.txtGuardianPhone.Name = "txtGuardianPhone";
            this.txtGuardianPhone.Size = new Size(230, 30);
            this.txtGuardianPhone.TabIndex = 6;

            //
            // lblNotes
            //
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblNotes.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblNotes.Location = new Point(290, 65);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new Size(110, 23);
            this.lblNotes.TabIndex = 7;
            this.lblNotes.Text = "ملاحظات أخرى:";

            //
            // txtNotes
            //
            this.txtNotes.Location = new Point(40, 62);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new Size(240, 30);
            this.txtNotes.TabIndex = 8;

            //
            // lblOpeningBalance
            //
            this.lblOpeningBalance.AutoSize = true;
            this.lblOpeningBalance.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblOpeningBalance.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblOpeningBalance.Location = new Point(650, 110);
            this.lblOpeningBalance.Name = "lblOpeningBalance";
            this.lblOpeningBalance.Size = new Size(115, 23);
            this.lblOpeningBalance.TabIndex = 9;
            this.lblOpeningBalance.Text = "الرصيد السابق:";

            //
            // txtOpeningBalance
            //
            this.txtOpeningBalance.Location = new Point(410, 107);
            this.txtOpeningBalance.Name = "txtOpeningBalance";
            this.txtOpeningBalance.Size = new Size(230, 30);
            this.txtOpeningBalance.TabIndex = 10;
            this.txtOpeningBalance.Text = "0.00";

            //
            // lblBalanceType
            //
            this.lblBalanceType.AutoSize = true;
            this.lblBalanceType.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblBalanceType.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblBalanceType.Location = new Point(290, 110);
            this.lblBalanceType.Name = "lblBalanceType";
            this.lblBalanceType.Size = new Size(115, 23);
            this.lblBalanceType.TabIndex = 11;
            this.lblBalanceType.Text = "نوع الرصيد:";

            //
            // cmbBalanceType
            //
            this.cmbBalanceType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbBalanceType.FormattingEnabled = true;
            this.cmbBalanceType.Items.AddRange(new object[] {
            "مدين / Debit",
            "دائن / Credit"});
            this.cmbBalanceType.Location = new Point(40, 107);
            this.cmbBalanceType.Name = "cmbBalanceType";
            this.cmbBalanceType.Size = new Size(240, 31);
            this.cmbBalanceType.TabIndex = 12;

            //
            // btnAdd
            //
            this.btnAdd.BackColor = Color.FromArgb(52, 152, 219); // Modern Action Blue
            this.btnAdd.Cursor = Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Location = new Point(540, 170);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(120, 35);
            this.btnAdd.TabIndex = 13;
            this.btnAdd.Text = "إضافة طالب";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new EventHandler(this.BtnAdd_Click);

            //
            // btnUpdate
            //
            this.btnUpdate.BackColor = Color.FromArgb(46, 204, 113); // Soft Success Green
            this.btnUpdate.Cursor = Cursors.Hand;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = FlatStyle.Flat;
            this.btnUpdate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnUpdate.ForeColor = Color.White;
            this.btnUpdate.Location = new Point(410, 170);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new Size(120, 35);
            this.btnUpdate.TabIndex = 14;
            this.btnUpdate.Text = "تعديل البيانات";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new EventHandler(this.BtnUpdate_Click);

            //
            // btnDelete
            //
            this.btnDelete.BackColor = Color.FromArgb(231, 76, 60); // Soft Danger Red
            this.btnDelete.Cursor = Cursors.Hand;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Location = new Point(280, 170);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(120, 35);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "حذف";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new EventHandler(this.BtnDelete_Click);

            //
            // btnClear
            //
            this.btnClear.BackColor = Color.FromArgb(127, 140, 141); // Gray
            this.btnClear.Cursor = Cursors.Hand;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnClear.ForeColor = Color.White;
            this.btnClear.Location = new Point(150, 170);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(120, 35);
            this.btnClear.TabIndex = 16;
            this.btnClear.Text = "جديد / مسح";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new EventHandler(this.BtnClear_Click);

            //
            // pnlSearch
            //
            this.pnlSearch.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.pnlSearch.BackColor = Color.White;
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Location = new Point(20, 265);
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
            this.lblSearch.Location = new Point(600, 13);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new Size(160, 23);
            this.lblSearch.Text = "بحث سريع بالاسم أو الهاتف:";

            //
            // txtSearch
            //
            this.txtSearch.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.txtSearch.Location = new Point(40, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(550, 30);
            this.txtSearch.TextChanged += new EventHandler(this.TxtSearch_TextChanged);

            //
            // pnlGrid
            //
            this.pnlGrid.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.pnlGrid.Controls.Add(this.dgvStudents);
            this.pnlGrid.Location = new Point(20, 330);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new Size(780, 250);
            this.pnlGrid.TabIndex = 2;

            //
            // dgvStudents
            //
            this.dgvStudents.AllowUserToAddRows = false;
            this.dgvStudents.AllowUserToDeleteRows = false;
            this.dgvStudents.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            this.dgvStudents.BackgroundColor = Color.White;
            this.dgvStudents.BorderStyle = BorderStyle.None;
            this.dgvStudents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 99, 235); // #2563EB
            this.dgvStudents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvStudents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvStudents.DefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
            this.dgvStudents.DefaultCellStyle.SelectionForeColor = Color.FromArgb(37, 99, 235);
            this.dgvStudents.EnableHeadersVisualStyles = false;
            this.dgvStudents.Dock = DockStyle.Fill;
            this.dgvStudents.Location = new Point(0, 0);
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.ReadOnly = true;
            this.dgvStudents.RowHeadersVisible = false;
            this.dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvStudents.CellClick += new DataGridViewCellEventHandler(this.DgvStudents_CellClick);

            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            this.ResumeLayout(false);
        }

        public void LoadStudents()
        {
            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT
                            StudentID,
                            StudentName AS [اسم الطالب],
                            GuardianName AS [اسم ولي الأمر],
                            ParentPhone AS [رقم هاتف ولي الأمر],
                            Notes AS [ملاحظات أخرى],
                            RegistrationDate AS [تاريخ التسجيل]
                        FROM Students
                        ORDER BY StudentID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvStudents.DataSource = dt;

                            if (dgvStudents.Columns.Contains("StudentID"))
                                dgvStudents.Columns["StudentID"].Visible = false;

                            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadStudents failed: " + ex.Message);
            }
        }

        private void ClearInputs()
        {
            txtName.Clear();
            txtGuardianName.Clear();
            txtGuardianPhone.Clear();
            txtNotes.Clear();
            txtOpeningBalance.Text = "0.00";
            cmbBalanceType.SelectedIndex = -1;
            selectedStudentID = -1;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtGuardianPhone.Text))
            {
                MessageBox.Show("يرجى ملء جميع الحقول المطلوبة (اسم الطالب ورقم هاتف ولي الأمر).", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal openingBalanceAmount = 0;
            if (!string.IsNullOrWhiteSpace(txtOpeningBalance.Text))
            {
                if (!decimal.TryParse(txtOpeningBalance.Text.Trim(), out openingBalanceAmount) || openingBalanceAmount < 0)
                {
                    MessageBox.Show("يرجى إدخال مبلغ رصيد سابق صحيح.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (openingBalanceAmount > 0 && cmbBalanceType.SelectedIndex == -1)
            {
                MessageBox.Show("يرجى اختيار نوع الرصيد (مدين أو دائن) لأن القيمة المدخلة أكبر من الصفر.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            int newStudentID = -1;
                            string query = "INSERT INTO Students (StudentName, GuardianName, ParentPhone, RegistrationDate, Notes) VALUES (@StudentName, @GuardianName, @ParentPhone, @RegistrationDate, @Notes); SELECT SCOPE_IDENTITY();";
                            using (SqlCommand cmd = new SqlCommand(query, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@StudentName", txtName.Text.Trim());
                                cmd.Parameters.AddWithValue("@GuardianName", txtGuardianName.Text.Trim());
                                cmd.Parameters.AddWithValue("@ParentPhone", txtGuardianPhone.Text.Trim());
                                cmd.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@Notes", txtNotes.Text.Trim());
                                object result = cmd.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                {
                                    newStudentID = Convert.ToInt32(result);
                                }
                            }

                            if (openingBalanceAmount > 0 && newStudentID != -1)
                            {
                                decimal debit = 0;
                                decimal credit = 0;
                                if (cmbBalanceType.SelectedIndex == 0) // Debit
                                {
                                    debit = openingBalanceAmount;
                                }
                                else // Credit
                                {
                                    credit = openingBalanceAmount;
                                }

                                string queryTx = @"
                                    INSERT INTO FinancialTransactions (StudentID, TransactionType, Debit, Credit, TransactionDate, Notes, UserID)
                                    VALUES (@StudentID, 'Opening Balance', @Debit, @Credit, @TransactionDate, @Notes, @UserID)";
                                using (SqlCommand cmdTx = new SqlCommand(queryTx, conn, trans))
                                {
                                    cmdTx.Parameters.AddWithValue("@StudentID", newStudentID);
                                    cmdTx.Parameters.AddWithValue("@Debit", debit);
                                    cmdTx.Parameters.AddWithValue("@Credit", credit);
                                    cmdTx.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                                    cmdTx.Parameters.AddWithValue("@Notes", "رصيد افتتاح سابق");
                                    cmdTx.Parameters.AddWithValue("@UserID", 1); // seeded default admin
                                    cmdTx.ExecuteNonQuery();
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

                MessageBox.Show("تم إضافة الطالب وحفظ الرصيد السابق بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
                LoadStudents();
                OnDataSaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء إضافة الطالب: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedStudentID == -1)
            {
                MessageBox.Show("يرجى تحديد طالب من الجدول لتعديل بياناته.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtGuardianPhone.Text))
            {
                MessageBox.Show("يرجى ملء جميع الحقول المطلوبة (الاسم ورقم الهاتف).", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal openingBalanceAmount = 0;
            if (!string.IsNullOrWhiteSpace(txtOpeningBalance.Text))
            {
                if (!decimal.TryParse(txtOpeningBalance.Text.Trim(), out openingBalanceAmount) || openingBalanceAmount < 0)
                {
                    MessageBox.Show("يرجى إدخال مبلغ رصيد سابق صحيح.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (openingBalanceAmount > 0 && cmbBalanceType.SelectedIndex == -1)
            {
                MessageBox.Show("يرجى اختيار نوع الرصيد (مدين أو دائن) لأن القيمة المدخلة أكبر من الصفر.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            // Update student details
                            string query = "UPDATE Students SET StudentName = @StudentName, GuardianName = @GuardianName, ParentPhone = @ParentPhone, Notes = @Notes WHERE StudentID = @StudentID";
                            using (SqlCommand cmd = new SqlCommand(query, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@StudentName", txtName.Text.Trim());
                                cmd.Parameters.AddWithValue("@GuardianName", txtGuardianName.Text.Trim());
                                cmd.Parameters.AddWithValue("@ParentPhone", txtGuardianPhone.Text.Trim());
                                cmd.Parameters.AddWithValue("@Notes", txtNotes.Text.Trim());
                                cmd.Parameters.AddWithValue("@StudentID", selectedStudentID);
                                cmd.ExecuteNonQuery();
                            }

                            // Update or Insert Opening Balance in FinancialTransactions
                            // First check if an 'Opening Balance' record already exists for this StudentID
                            string checkQuery = "SELECT TransactionID FROM FinancialTransactions WHERE StudentID = @StudentID AND TransactionType = 'Opening Balance'";
                            int txId = -1;
                            using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn, trans))
                            {
                                checkCmd.Parameters.AddWithValue("@StudentID", selectedStudentID);
                                object res = checkCmd.ExecuteScalar();
                                if (res != null && res != DBNull.Value)
                                {
                                    txId = Convert.ToInt32(res);
                                }
                            }

                            if (openingBalanceAmount > 0)
                            {
                                decimal debit = 0;
                                decimal credit = 0;
                                if (cmbBalanceType.SelectedIndex == 0) // Debit
                                {
                                    debit = openingBalanceAmount;
                                }
                                else // Credit
                                {
                                    credit = openingBalanceAmount;
                                }

                                if (txId != -1)
                                {
                                    // Update existing opening balance
                                    string updateTxQuery = "UPDATE FinancialTransactions SET Debit = @Debit, Credit = @Credit, Notes = @Notes WHERE TransactionID = @TransactionID";
                                    using (SqlCommand updateTxCmd = new SqlCommand(updateTxQuery, conn, trans))
                                    {
                                        updateTxCmd.Parameters.AddWithValue("@Debit", debit);
                                        updateTxCmd.Parameters.AddWithValue("@Credit", credit);
                                        updateTxCmd.Parameters.AddWithValue("@Notes", "رصيد افتتاح سابق");
                                        updateTxCmd.Parameters.AddWithValue("@TransactionID", txId);
                                        updateTxCmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    // Insert new opening balance
                                    string insertTxQuery = @"
                                        INSERT INTO FinancialTransactions (StudentID, TransactionType, Debit, Credit, TransactionDate, Notes, UserID)
                                        VALUES (@StudentID, 'Opening Balance', @Debit, @Credit, @TransactionDate, @Notes, @UserID)";
                                    using (SqlCommand insertTxCmd = new SqlCommand(insertTxQuery, conn, trans))
                                    {
                                        insertTxCmd.Parameters.AddWithValue("@StudentID", selectedStudentID);
                                        insertTxCmd.Parameters.AddWithValue("@Debit", debit);
                                        insertTxCmd.Parameters.AddWithValue("@Credit", credit);
                                        insertTxCmd.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                                        insertTxCmd.Parameters.AddWithValue("@Notes", "رصيد افتتاح سابق");
                                        insertTxCmd.Parameters.AddWithValue("@UserID", 1); // seeded default admin
                                        insertTxCmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                // If amount is 0, delete the existing opening balance if it exists
                                if (txId != -1)
                                {
                                    string deleteTxQuery = "DELETE FROM FinancialTransactions WHERE TransactionID = @TransactionID";
                                    using (SqlCommand deleteTxCmd = new SqlCommand(deleteTxQuery, conn, trans))
                                    {
                                        deleteTxCmd.Parameters.AddWithValue("@TransactionID", txId);
                                        deleteTxCmd.ExecuteNonQuery();
                                    }
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

                MessageBox.Show("تم تعديل بيانات الطالب والرصيد السابق بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
                LoadStudents();
                OnDataSaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تعديل البيانات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedStudentID == -1)
            {
                MessageBox.Show("يرجى تحديد طالب من الجدول لحذفه.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("هل أنت متأكد من حذف هذا الطالب نهائياً؟ سيتم حذف جميع الحركات المالية التابعة له أيضاً.", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string connectionString = DbConnectionManager.GetConnectionString();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM Students WHERE StudentID = @StudentID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@StudentID", selectedStudentID);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("تم حذف الطالب بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    LoadStudents();
                    OnDataSaved();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطأ أثناء عملية الحذف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        public void SetSearchText(string text)
        {
            txtSearch.Text = text;
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(filterText))
            {
                LoadStudents();
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
                            StudentID,
                            StudentName AS [اسم الطالب],
                            GuardianName AS [اسم ولي الأمر],
                            ParentPhone AS [رقم هاتف ولي الأمر],
                            Notes AS [ملاحظات أخرى],
                            RegistrationDate AS [تاريخ التسجيل]
                        FROM Students
                        WHERE StudentName LIKE @Filter OR ParentPhone LIKE @Filter OR GuardianName LIKE @Filter
                        ORDER BY StudentID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Filter", "%" + filterText + "%");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvStudents.DataSource = dt;

                            if (dgvStudents.Columns.Contains("StudentID"))
                                dgvStudents.Columns["StudentID"].Visible = false;

                            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Search failed: " + ex.Message);
            }
        }

        private void dgvStudents_CellClick_NoId(DataGridViewRow row)
        {
            txtName.Text = row.Cells["اسم الطالب"].Value.ToString();
            txtGuardianName.Text = row.Cells["اسم ولي الأمر"].Value.ToString();
            txtGuardianPhone.Text = row.Cells["رقم هاتف ولي الأمر"].Value.ToString();
            txtNotes.Text = row.Cells["ملاحظات أخرى"].Value.ToString();

            // Fetch opening balance if any
            txtOpeningBalance.Text = "0.00";
            cmbBalanceType.SelectedIndex = -1;
            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Debit, Credit FROM FinancialTransactions WHERE StudentID = @StudentID AND TransactionType = 'Opening Balance'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", selectedStudentID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal debit = Convert.ToDecimal(reader["Debit"]);
                                decimal credit = Convert.ToDecimal(reader["Credit"]);
                                if (debit > 0)
                                {
                                    txtOpeningBalance.Text = debit.ToString("0.00");
                                    cmbBalanceType.SelectedIndex = 0; // Debit
                                }
                                else if (credit > 0)
                                {
                                    txtOpeningBalance.Text = credit.ToString("0.00");
                                    cmbBalanceType.SelectedIndex = 1; // Credit
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Fetch opening balance failed: " + ex.Message);
            }
        }

        private void DgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
                selectedStudentID = Convert.ToInt32(row.Cells["StudentID"].Value);
                dgvStudents_CellClick_NoId(row);
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