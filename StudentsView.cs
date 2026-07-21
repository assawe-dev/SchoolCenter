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
        private Label lblParentPhone;
        private TextBox txtParentPhone;
        private Label lblNotes;
        private TextBox txtNotes;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;

        private Panel pnlSearch;
        private Label lblSearch;
        private TextBox txtSearch;

        private DataGridView dgvStudents;
        private Panel pnlGrid;

        private TableLayoutPanel tlpInput;

        private int selectedStudentID = -1;

        public event EventHandler DataSaved;

        public StudentsView()
        {
            InitializeComponent();
            LoadStudents();
        }

        private void InitializeComponent()
        {
            this.pnlInput = new Panel();
            this.tlpInput = new TableLayoutPanel();

            this.lblName = new Label();
            this.txtName = new TextBox();
            this.lblGuardianName = new Label();
            this.txtGuardianName = new TextBox();
            this.lblParentPhone = new Label();
            this.txtParentPhone = new TextBox();
            this.lblNotes = new Label();
            this.txtNotes = new TextBox();

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
            this.tlpInput.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            this.SuspendLayout();

            //
            // StudentsView
            //
            this.BackColor = Color.FromArgb(248, 249, 250); // Off-White
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
            this.pnlInput.BackColor = Color.White;
            this.pnlInput.Controls.Add(this.tlpInput);
            this.pnlInput.Dock = DockStyle.Top;
            this.pnlInput.Location = new Point(0, 0);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new Size(820, 195);
            this.pnlInput.TabIndex = 0;

            //
            // tlpInput
            //
            this.tlpInput.ColumnCount = 4;
            this.tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            this.tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            this.tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            this.tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            this.tlpInput.RowCount = 3;
            this.tlpInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            this.tlpInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            this.tlpInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));

            this.tlpInput.Controls.Add(this.lblName, 0, 0);
            this.tlpInput.Controls.Add(this.txtName, 1, 0);
            this.tlpInput.Controls.Add(this.lblGuardianName, 2, 0);
            this.tlpInput.Controls.Add(this.txtGuardianName, 3, 0);
            this.tlpInput.Controls.Add(this.lblParentPhone, 0, 1);
            this.tlpInput.Controls.Add(this.txtParentPhone, 1, 1);
            this.tlpInput.Controls.Add(this.lblNotes, 2, 1);
            this.tlpInput.Controls.Add(this.txtNotes, 3, 1);

            // Button flow panel
            FlowLayoutPanel flpButtons = new FlowLayoutPanel();
            flpButtons.FlowDirection = FlowDirection.RightToLeft;
            flpButtons.Controls.Add(this.btnAdd);
            flpButtons.Controls.Add(this.btnUpdate);
            flpButtons.Controls.Add(this.btnDelete);
            flpButtons.Controls.Add(this.btnClear);
            flpButtons.Dock = DockStyle.Fill;
            flpButtons.Margin = new Padding(0);

            this.tlpInput.Controls.Add(flpButtons, 0, 2);
            this.tlpInput.SetColumnSpan(flpButtons, 4);

            this.tlpInput.Dock = DockStyle.Fill;
            this.tlpInput.Location = new Point(0, 0);
            this.tlpInput.Name = "tlpInput";
            this.tlpInput.Padding = new Padding(15);
            this.tlpInput.TabIndex = 0;

            //
            // lblName
            //
            this.lblName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.lblName.AutoSize = true;
            this.lblName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblName.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblName.Location = new Point(675, 26);
            this.lblName.Name = "lblName";
            this.lblName.Size = new Size(112, 23);
            this.lblName.Text = "اسم الطالب:";
            this.lblName.TextAlign = ContentAlignment.MiddleLeft;

            //
            // txtName
            //
            this.txtName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtName.Location = new Point(413, 22);
            this.txtName.Name = "txtName";
            this.txtName.Size = new Size(256, 30);
            this.txtName.TabIndex = 0;
            this.txtName.KeyDown += new KeyEventHandler(this.Input_KeyDown);

            //
            // lblGuardianName
            //
            this.lblGuardianName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.lblGuardianName.AutoSize = true;
            this.lblGuardianName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblGuardianName.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblGuardianName.Location = new Point(295, 26);
            this.lblGuardianName.Name = "lblGuardianName";
            this.lblGuardianName.Size = new Size(112, 23);
            this.lblGuardianName.Text = "اسم ولي الأمر:";
            this.lblGuardianName.TextAlign = ContentAlignment.MiddleLeft;

            //
            // txtGuardianName
            //
            this.txtGuardianName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtGuardianName.Location = new Point(18, 22);
            this.txtGuardianName.Name = "txtGuardianName";
            this.txtGuardianName.Size = new Size(271, 30);
            this.txtGuardianName.TabIndex = 1;
            this.txtGuardianName.KeyDown += new KeyEventHandler(this.Input_KeyDown);

            //
            // lblParentPhone
            //
            this.lblParentPhone.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.lblParentPhone.AutoSize = true;
            this.lblParentPhone.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblParentPhone.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblParentPhone.Location = new Point(675, 71);
            this.lblParentPhone.Name = "lblParentPhone";
            this.lblParentPhone.Size = new Size(112, 23);
            this.lblParentPhone.Text = "هاتف ولي الأمر:";
            this.lblParentPhone.TextAlign = ContentAlignment.MiddleLeft;

            //
            // txtParentPhone
            //
            this.txtParentPhone.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtParentPhone.Location = new Point(413, 67);
            this.txtParentPhone.Name = "txtParentPhone";
            this.txtParentPhone.Size = new Size(256, 30);
            this.txtParentPhone.TabIndex = 2;
            this.txtParentPhone.KeyDown += new KeyEventHandler(this.Input_KeyDown);

            //
            // lblNotes
            //
            this.lblNotes.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblNotes.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblNotes.Location = new Point(295, 71);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new Size(112, 23);
            this.lblNotes.Text = "ملاحظات أخرى:";
            this.lblNotes.TextAlign = ContentAlignment.MiddleLeft;

            //
            // txtNotes
            //
            this.txtNotes.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtNotes.Location = new Point(18, 67);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new Size(271, 30);
            this.txtNotes.TabIndex = 3;
            this.txtNotes.KeyDown += new KeyEventHandler(this.Input_KeyDown);

            //
            // btnAdd
            //
            this.btnAdd.BackColor = Color.FromArgb(52, 152, 219); // Modern Action Blue
            this.btnAdd.Cursor = Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Location = new Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(120, 38);
            this.btnAdd.TabIndex = 4;
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
            this.btnUpdate.Location = new Point(129, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new Size(120, 38);
            this.btnUpdate.TabIndex = 5;
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
            this.btnDelete.Location = new Point(255, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(120, 38);
            this.btnDelete.TabIndex = 6;
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
            this.btnClear.Location = new Point(381, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(120, 38);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "جديد / مسح";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new EventHandler(this.BtnClear_Click);

            //
            // pnlSearch
            //
            this.pnlSearch.BackColor = Color.White;
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Dock = DockStyle.Top;
            this.pnlSearch.Location = new Point(0, 195);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new Size(820, 50);
            this.pnlSearch.TabIndex = 1;

            //
            // lblSearch
            //
            this.lblSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblSearch.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblSearch.Location = new Point(630, 13);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new Size(165, 23);
            this.lblSearch.Text = "بحث سريع بالاسم أو الهاتف:";

            //
            // txtSearch
            //
            this.txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
            this.txtSearch.Location = new Point(18, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(600, 30);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TabStop = false;
            this.txtSearch.TextChanged += new EventHandler(this.TxtSearch_TextChanged);

            //
            // pnlGrid
            //
            this.pnlGrid.Controls.Add(this.dgvStudents);
            this.pnlGrid.Dock = DockStyle.Fill;
            this.pnlGrid.Location = new Point(0, 245);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new Padding(15);
            this.pnlGrid.Size = new Size(820, 355);
            this.pnlGrid.TabIndex = 2;

            //
            // dgvStudents
            //
            this.dgvStudents.AllowUserToAddRows = false;
            this.dgvStudents.AllowUserToDeleteRows = false;
            this.dgvStudents.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 244, 244);
            this.dgvStudents.BackgroundColor = Color.White;
            this.dgvStudents.BorderStyle = BorderStyle.None;
            this.dgvStudents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            this.dgvStudents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvStudents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvStudents.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            this.dgvStudents.DefaultCellStyle.SelectionForeColor = Color.White;
            this.dgvStudents.EnableHeadersVisualStyles = false;
            this.dgvStudents.Dock = DockStyle.Fill;
            this.dgvStudents.Location = new Point(15, 15);
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.ReadOnly = true;
            this.dgvStudents.RowHeadersVisible = false;
            this.dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvStudents.CellClick += new DataGridViewCellEventHandler(this.DgvStudents_CellClick);

            this.pnlInput.ResumeLayout(false);
            this.tlpInput.ResumeLayout(false);
            this.tlpInput.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            this.ResumeLayout(false);
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
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
                            StudentID AS [Id],
                            StudentName AS [اسم الطالب],
                            GuardianName AS [اسم ولي الأمر],
                            ParentPhone AS [رقم هاتف ولي الأمر],
                            RegistrationDate AS [تاريخ التسجيل],
                            Notes AS [ملاحظات أخرى]
                        FROM Students
                        ORDER BY StudentID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvStudents.DataSource = dt;

                            if (dgvStudents.Columns.Contains("Id"))
                                dgvStudents.Columns["Id"].Visible = false;

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
            txtParentPhone.Clear();
            txtNotes.Clear();
            selectedStudentID = -1;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtParentPhone.Text))
            {
                MessageBox.Show("يرجى ملء جميع الحقول المطلوبة (اسم الطالب ورقم هاتف ولي الأمر).", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        INSERT INTO Students (StudentName, GuardianName, ParentPhone, RegistrationDate, Notes)
                        VALUES (@StudentName, @GuardianName, @ParentPhone, @RegistrationDate, @Notes)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentName", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@GuardianName", txtGuardianName.Text.Trim());
                        cmd.Parameters.AddWithValue("@ParentPhone", txtParentPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(txtNotes.Text) ? (object)DBNull.Value : txtNotes.Text.Trim());
                        cmd.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("تم إضافة الطالب بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtParentPhone.Text))
            {
                MessageBox.Show("يرجى ملء جميع الحقول المطلوبة (الاسم ورقم الهاتف).", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        UPDATE Students
                        SET StudentName = @StudentName, GuardianName = @GuardianName, ParentPhone = @ParentPhone, Notes = @Notes
                        WHERE StudentID = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentName", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@GuardianName", txtGuardianName.Text.Trim());
                        cmd.Parameters.AddWithValue("@ParentPhone", txtParentPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(txtNotes.Text) ? (object)DBNull.Value : txtNotes.Text.Trim());
                        cmd.Parameters.AddWithValue("@Id", selectedStudentID);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("تم تعديل بيانات الطالب بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            DialogResult dialogResult = MessageBox.Show("هل أنت متأكد من حذف هذا الطالب نهائياً؟ سيتم حذف جميع الرسوم والمستحقات والمدفوعات التابعة له أيضاً.", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string connectionString = DbConnectionManager.GetConnectionString();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM Students WHERE StudentID = @Id";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Id", selectedStudentID);
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
                            StudentID AS [Id],
                            StudentName AS [اسم الطالب],
                            GuardianName AS [اسم ولي الأمر],
                            ParentPhone AS [رقم هاتف ولي الأمر],
                            RegistrationDate AS [تاريخ التسجيل],
                            Notes AS [ملاحظات أخرى]
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

                            if (dgvStudents.Columns.Contains("Id"))
                                dgvStudents.Columns["Id"].Visible = false;

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

        private void DgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
                selectedStudentID = Convert.ToInt32(row.Cells["Id"].Value);
                txtName.Text = row.Cells["اسم الطالب"].Value.ToString();
                txtGuardianName.Text = row.Cells["اسم ولي الأمر"].Value.ToString();
                txtParentPhone.Text = row.Cells["رقم هاتف ولي الأمر"].Value.ToString();
                txtNotes.Text = row.Cells["ملاحظات أخرى"].Value.ToString();
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