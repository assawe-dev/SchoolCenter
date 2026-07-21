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
        private Label lblPhone;
        private TextBox txtPhone;
        private Label lblCourse;
        private ComboBox cbCourse;
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
            LoadCourses();
            LoadStudents();
        }

        private void InitializeComponent()
        {
            this.pnlInput = new Panel();
            this.lblName = new Label();
            this.txtName = new TextBox();
            this.lblPhone = new Label();
            this.txtPhone = new TextBox();
            this.lblCourse = new Label();
            this.cbCourse = new ComboBox();
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
            this.BackColor = Color.FromArgb(248, 249, 250); // Off-White background
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
            this.pnlInput.BorderStyle = BorderStyle.None;
            this.pnlInput.Controls.Add(this.btnClear);
            this.pnlInput.Controls.Add(this.btnDelete);
            this.pnlInput.Controls.Add(this.btnUpdate);
            this.pnlInput.Controls.Add(this.btnAdd);
            this.pnlInput.Controls.Add(this.cbCourse);
            this.pnlInput.Controls.Add(this.lblCourse);
            this.pnlInput.Controls.Add(this.txtPhone);
            this.pnlInput.Controls.Add(this.lblPhone);
            this.pnlInput.Controls.Add(this.txtName);
            this.pnlInput.Controls.Add(this.lblName);
            this.pnlInput.Location = new Point(20, 20);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new Size(780, 140);
            this.pnlInput.TabIndex = 0;

            //
            // lblName
            //
            this.lblName.AutoSize = true;
            this.lblName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblName.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblName.Location = new Point(680, 20);
            this.lblName.Name = "lblName";
            this.lblName.Size = new Size(80, 23);
            this.lblName.Text = "اسم الطالب:";

            //
            // txtName
            //
            this.txtName.Location = new Point(440, 17);
            this.txtName.Name = "txtName";
            this.txtName.Size = new Size(230, 30);

            //
            // lblPhone
            //
            this.lblPhone.AutoSize = true;
            this.lblPhone.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblPhone.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblPhone.Location = new Point(340, 20);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new Size(85, 23);
            this.lblPhone.Text = "رقم الهاتف:";

            //
            // txtPhone
            //
            this.txtPhone.Location = new Point(110, 17);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new Size(220, 30);

            //
            // lblCourse
            //
            this.lblCourse.AutoSize = true;
            this.lblCourse.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblCourse.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblCourse.Location = new Point(680, 65);
            this.lblCourse.Name = "lblCourse";
            this.lblCourse.Size = new Size(83, 23);
            this.lblCourse.Text = "الدورة المسجل بها:";

            //
            // cbCourse
            //
            this.cbCourse.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbCourse.FormattingEnabled = true;
            this.cbCourse.Location = new Point(110, 62);
            this.cbCourse.Name = "cbCourse";
            this.cbCourse.Size = new Size(560, 31);

            //
            // btnAdd
            //
            this.btnAdd.BackColor = Color.FromArgb(52, 152, 219); // Action Blue
            this.btnAdd.Cursor = Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Location = new Point(570, 100);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(100, 32);
            this.btnAdd.Text = "إضافة طالب";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new EventHandler(this.BtnAdd_Click);

            //
            // btnUpdate
            //
            this.btnUpdate.BackColor = Color.FromArgb(46, 204, 113); // Soft Green
            this.btnUpdate.Cursor = Cursors.Hand;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = FlatStyle.Flat;
            this.btnUpdate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnUpdate.ForeColor = Color.White;
            this.btnUpdate.Location = new Point(460, 100);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new Size(100, 32);
            this.btnUpdate.Text = "تعديل البيانات";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new EventHandler(this.BtnUpdate_Click);

            //
            // btnDelete
            //
            this.btnDelete.BackColor = Color.FromArgb(231, 76, 60); // Soft Red
            this.btnDelete.Cursor = Cursors.Hand;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Location = new Point(350, 100);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(100, 32);
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
            this.btnClear.Location = new Point(240, 100);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(100, 32);
            this.btnClear.Text = "جديد / مسح";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new EventHandler(this.BtnClear_Click);

            //
            // pnlSearch
            //
            this.pnlSearch.BackColor = Color.White;
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Location = new Point(20, 175);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new Size(780, 50);
            this.pnlSearch.TabIndex = 1;

            //
            // lblSearch
            //
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblSearch.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblSearch.Location = new Point(650, 13);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new Size(110, 23);
            this.lblSearch.Text = "بحث بالاسم أو الهاتف:";

            //
            // txtSearch
            //
            this.txtSearch.Location = new Point(110, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(530, 30);
            this.txtSearch.TextChanged += new EventHandler(this.TxtSearch_TextChanged);

            //
            // pnlGrid
            //
            this.pnlGrid.Controls.Add(this.dgvStudents);
            this.pnlGrid.Location = new Point(20, 240);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new Size(780, 340);
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

        private void LoadCourses()
        {
            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT CourseID, CourseName + ' - (' + CAST(CourseFees AS NVARCHAR) + ' د.ل)' AS Display FROM Courses";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            cbCourse.DataSource = dt;
                            cbCourse.DisplayMember = "Display";
                            cbCourse.ValueMember = "CourseID";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل الدورات: " + ex.Message);
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
                            s.StudentID,
                            s.FullName AS [اسم الطالب],
                            s.Phone AS [رقم الهاتف],
                            c.CourseName AS [الدورة المسجل بها],
                            s.RegistrationDate AS [تاريخ التسجيل],
                            COALESCE(SUM(t.Amount), 0) AS [المبلغ المدفوع],
                            s.CourseID
                        FROM Students s
                        INNER JOIN Courses c ON s.CourseID = c.CourseID
                        LEFT JOIN FinancialTransactions t ON s.StudentID = t.StudentID
                        GROUP BY s.StudentID, s.FullName, s.Phone, c.CourseName, s.RegistrationDate, s.CourseID
                        ORDER BY s.StudentID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvStudents.DataSource = dt;

                            if (dgvStudents.Columns.Contains("StudentID"))
                                dgvStudents.Columns["StudentID"].Visible = false;
                            if (dgvStudents.Columns.Contains("CourseID"))
                                dgvStudents.Columns["CourseID"].Visible = false;

                            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Graceful fallback if database is not configured or offline
                System.Diagnostics.Debug.WriteLine("LoadStudents failed: " + ex.Message);
            }
        }

        private void ClearInputs()
        {
            txtName.Clear();
            txtPhone.Clear();
            if (cbCourse.Items.Count > 0) cbCourse.SelectedIndex = 0;
            selectedStudentID = -1;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("يرجى ملء جميع الحقول المطلوبة (الاسم ورقم الهاتف).", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbCourse.SelectedValue == null)
            {
                MessageBox.Show("يرجى اختيار دورة تعليمية أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Students (FullName, Phone, CourseID, RegistrationDate) VALUES (@FullName, @Phone, @CourseID, @RegistrationDate)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@CourseID", cbCourse.SelectedValue);
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

            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("يرجى ملء جميع الحقول المطلوبة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Students SET FullName = @FullName, Phone = @Phone, CourseID = @CourseID WHERE StudentID = @StudentID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@CourseID", cbCourse.SelectedValue);
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

            DialogResult dialogResult = MessageBox.Show("هل أنت متأكد من حذف هذا الطالب نهائياً؟ سيتم حذف جميع المدفوعات التابعة له أيضاً.", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                            s.StudentID,
                            s.FullName AS [اسم الطالب],
                            s.Phone AS [رقم الهاتف],
                            c.CourseName AS [الدورة المسجل بها],
                            s.RegistrationDate AS [تاريخ التسجيل],
                            COALESCE(SUM(t.Amount), 0) AS [المبلغ المدفوع],
                            s.CourseID
                        FROM Students s
                        INNER JOIN Courses c ON s.CourseID = c.CourseID
                        LEFT JOIN FinancialTransactions t ON s.StudentID = t.StudentID
                        WHERE s.FullName LIKE @Filter OR s.Phone LIKE @Filter
                        GROUP BY s.StudentID, s.FullName, s.Phone, c.CourseName, s.RegistrationDate, s.CourseID
                        ORDER BY s.StudentID DESC";

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
                            if (dgvStudents.Columns.Contains("CourseID"))
                                dgvStudents.Columns["CourseID"].Visible = false;

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
                selectedStudentID = Convert.ToInt32(row.Cells["StudentID"].Value);
                txtName.Text = row.Cells["اسم الطالب"].Value.ToString();
                txtPhone.Text = row.Cells["رقم الهاتف"].Value.ToString();

                if (row.Cells["CourseID"].Value != DBNull.Value)
                {
                    cbCourse.SelectedValue = row.Cells["CourseID"].Value;
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