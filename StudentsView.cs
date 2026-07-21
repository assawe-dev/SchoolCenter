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
            this.pnlInput.Location = new Point(20, 20);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new Size(780, 180);
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
            this.lblName.Text = "اسم الطالب:";

            //
            // txtName
            //
            this.txtName.Location = new Point(410, 17);
            this.txtName.Name = "txtName";
            this.txtName.Size = new Size(230, 30);

            //
            // lblGuardianName
            //
            this.lblGuardianName.AutoSize = true;
            this.lblGuardianName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblGuardianName.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblGuardianName.Location = new Point(290, 20);
            this.lblGuardianName.Name = "lblGuardianName";
            this.lblGuardianName.Size = new Size(115, 23);
            this.lblGuardianName.Text = "اسم ولي الأمر:";

            //
            // txtGuardianName
            //
            this.txtGuardianName.Location = new Point(40, 17);
            this.txtGuardianName.Name = "txtGuardianName";
            this.txtGuardianName.Size = new Size(240, 30);

            //
            // lblGuardianPhone
            //
            this.lblGuardianPhone.AutoSize = true;
            this.lblGuardianPhone.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblGuardianPhone.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblGuardianPhone.Location = new Point(650, 65);
            this.lblGuardianPhone.Name = "lblGuardianPhone";
            this.lblGuardianPhone.Size = new Size(115, 23);
            this.lblGuardianPhone.Text = "رقم ولي الأمر:";

            //
            // txtGuardianPhone
            //
            this.txtGuardianPhone.Location = new Point(410, 62);
            this.txtGuardianPhone.Name = "txtGuardianPhone";
            this.txtGuardianPhone.Size = new Size(230, 30);

            //
            // lblNotes
            //
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblNotes.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblNotes.Location = new Point(290, 65);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new Size(110, 23);
            this.lblNotes.Text = "ملاحظات أخرى:";

            //
            // txtNotes
            //
            this.txtNotes.Location = new Point(40, 62);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new Size(240, 30);

            //
            // btnAdd
            //
            this.btnAdd.BackColor = Color.FromArgb(52, 152, 219); // Modern Action Blue
            this.btnAdd.Cursor = Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Location = new Point(540, 120);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(120, 35);
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
            this.btnUpdate.Location = new Point(410, 120);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new Size(120, 35);
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
            this.btnDelete.Location = new Point(280, 120);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(120, 35);
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
            this.btnClear.Location = new Point(150, 120);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(120, 35);
            this.btnClear.Text = "جديد / مسح";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new EventHandler(this.BtnClear_Click);

            //
            // pnlSearch
            //
            this.pnlSearch.BackColor = Color.White;
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Location = new Point(20, 215);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new Size(780, 50);
            this.pnlSearch.TabIndex = 1;

            //
            // lblSearch
            //
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
            this.txtSearch.Location = new Point(40, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(550, 30);
            this.txtSearch.TextChanged += new EventHandler(this.TxtSearch_TextChanged);

            //
            // pnlGrid
            //
            this.pnlGrid.Controls.Add(this.dgvStudents);
            this.pnlGrid.Location = new Point(20, 280);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new Size(780, 300);
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
            selectedStudentID = -1;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtGuardianPhone.Text))
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
                    string query = "INSERT INTO Students (StudentName, GuardianName, ParentPhone, RegistrationDate, Notes) VALUES (@StudentName, @GuardianName, @ParentPhone, @RegistrationDate, @Notes)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentName", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@GuardianName", txtGuardianName.Text.Trim());
                        cmd.Parameters.AddWithValue("@ParentPhone", txtGuardianPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Notes", txtNotes.Text.Trim());
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

            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtGuardianPhone.Text))
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
                    string query = "UPDATE Students SET StudentName = @StudentName, GuardianName = @GuardianName, ParentPhone = @ParentPhone, Notes = @Notes WHERE StudentID = @StudentID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentName", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@GuardianName", txtGuardianName.Text.Trim());
                        cmd.Parameters.AddWithValue("@ParentPhone", txtGuardianPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Notes", txtNotes.Text.Trim());
                        cmd.Parameters.AddWithValue("@StudentID", selectedStudentID);
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