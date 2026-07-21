using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SchoolCenter
{
    public class StudentDuesView : UserControl
    {
        private Panel pnlInput;
        private Label lblStudent;
        private ComboBox cbStudent;
        private Label lblCourse;
        private ComboBox cbCourse;
        private Label lblDueAmount;
        private TextBox txtDueAmount;
        private Label lblNotes;
        private TextBox txtNotes;

        private Button btnSave;
        private Button btnClear;

        private DataGridView dgvDues;
        private Panel pnlGrid;

        public event EventHandler DataSaved;

        public StudentDuesView()
        {
            InitializeComponent();
            LoadStudents();
            LoadCourses();
            LoadDues();
        }

        private void InitializeComponent()
        {
            this.pnlInput = new Panel();
            this.lblStudent = new Label();
            this.cbStudent = new ComboBox();
            this.lblCourse = new Label();
            this.cbCourse = new ComboBox();
            this.lblDueAmount = new Label();
            this.txtDueAmount = new TextBox();
            this.lblNotes = new Label();
            this.txtNotes = new TextBox();

            this.btnSave = new Button();
            this.btnClear = new Button();

            this.pnlGrid = new Panel();
            this.dgvDues = new DataGridView();

            this.pnlInput.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDues)).BeginInit();
            this.SuspendLayout();

            //
            // StudentDuesView
            //
            this.BackColor = Color.FromArgb(248, 249, 250); // Off-White
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlInput);
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 10F);
            this.Name = "StudentDuesView";
            this.RightToLeft = RightToLeft.Yes;
            this.Size = new Size(820, 600);

            //
            // pnlInput
            //
            this.pnlInput.BackColor = Color.White;
            this.pnlInput.Controls.Add(this.btnClear);
            this.pnlInput.Controls.Add(this.btnSave);
            this.pnlInput.Controls.Add(this.txtNotes);
            this.pnlInput.Controls.Add(this.lblNotes);
            this.pnlInput.Controls.Add(this.txtDueAmount);
            this.pnlInput.Controls.Add(this.lblDueAmount);
            this.pnlInput.Controls.Add(this.cbCourse);
            this.pnlInput.Controls.Add(this.lblCourse);
            this.pnlInput.Controls.Add(this.cbStudent);
            this.pnlInput.Controls.Add(this.lblStudent);
            this.pnlInput.Location = new Point(20, 20);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new Size(780, 240);
            this.pnlInput.TabIndex = 0;

            //
            // lblStudent
            //
            this.lblStudent.AutoSize = true;
            this.lblStudent.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblStudent.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblStudent.Location = new Point(650, 18);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new Size(110, 23);
            this.lblStudent.Text = "اختر الطالب:";

            //
            // cbStudent
            //
            this.cbStudent.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStudent.FormattingEnabled = true;
            this.cbStudent.Location = new Point(410, 15);
            this.cbStudent.Name = "cbStudent";
            this.cbStudent.Size = new Size(230, 31);

            //
            // lblCourse
            //
            this.lblCourse.AutoSize = true;
            this.lblCourse.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblCourse.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblCourse.Location = new Point(650, 58);
            this.lblCourse.Name = "lblCourse";
            this.lblCourse.Size = new Size(110, 23);
            this.lblCourse.Text = "اختر الدورة:";

            //
            // cbCourse
            //
            this.cbCourse.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbCourse.FormattingEnabled = true;
            this.cbCourse.Location = new Point(410, 55);
            this.cbCourse.Name = "cbCourse";
            this.cbCourse.Size = new Size(230, 31);
            this.cbCourse.SelectedIndexChanged += new EventHandler(this.CbCourse_SelectedIndexChanged);

            //
            // lblDueAmount
            //
            this.lblDueAmount.AutoSize = true;
            this.lblDueAmount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblDueAmount.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblDueAmount.Location = new Point(650, 98);
            this.lblDueAmount.Name = "lblDueAmount";
            this.lblDueAmount.Size = new Size(110, 23);
            this.lblDueAmount.Text = "قيمة المستحق:";

            //
            // txtDueAmount
            //
            this.txtDueAmount.Location = new Point(410, 95);
            this.txtDueAmount.Name = "txtDueAmount";
            this.txtDueAmount.Size = new Size(230, 30);
            this.txtDueAmount.ReadOnly = true; // Auto-filled from course cost

            //
            // lblNotes
            //
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblNotes.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblNotes.Location = new Point(650, 138);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new Size(110, 23);
            this.lblNotes.Text = "ملاحظات الدورة:";

            //
            // txtNotes
            //
            this.txtNotes.Location = new Point(410, 135);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new Size(230, 30);

            //
            // btnSave
            //
            this.btnSave.BackColor = Color.FromArgb(52, 152, 219); // Modern Action Blue
            this.btnSave.Cursor = Cursors.Hand;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(410, 185);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(160, 38);
            this.btnSave.Text = "حفظ المستحق والمالية";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new EventHandler(this.BtnSave_Click);

            //
            // btnClear
            //
            this.btnClear.BackColor = Color.FromArgb(127, 140, 141); // Gray
            this.btnClear.Cursor = Cursors.Hand;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnClear.ForeColor = Color.White;
            this.btnClear.Location = new Point(230, 185);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(160, 38);
            this.btnClear.Text = "مسح الحقول";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new EventHandler(this.BtnClear_Click);

            //
            // pnlGrid
            //
            this.pnlGrid.Controls.Add(this.dgvDues);
            this.pnlGrid.Location = new Point(20, 280);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new Size(780, 300);
            this.pnlGrid.TabIndex = 1;

            //
            // dgvDues
            //
            this.dgvDues.AllowUserToAddRows = false;
            this.dgvDues.AllowUserToDeleteRows = false;
            this.dgvDues.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 244, 244);
            this.dgvDues.BackgroundColor = Color.White;
            this.dgvDues.BorderStyle = BorderStyle.None;
            this.dgvDues.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            this.dgvDues.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvDues.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvDues.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            this.dgvDues.DefaultCellStyle.SelectionForeColor = Color.White;
            this.dgvDues.EnableHeadersVisualStyles = false;
            this.dgvDues.Dock = DockStyle.Fill;
            this.dgvDues.Location = new Point(0, 0);
            this.dgvDues.Name = "dgvDues";
            this.dgvDues.ReadOnly = true;
            this.dgvDues.RowHeadersVisible = false;
            this.dgvDues.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDues)).EndInit();
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
                    string query = "SELECT StudentID, StudentName FROM Students ORDER BY StudentName ASC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            cbStudent.DataSource = dt;
                            cbStudent.DisplayMember = "StudentName";
                            cbStudent.ValueMember = "StudentID";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadStudents failed: " + ex.Message);
            }
        }

        public void LoadCourses()
        {
            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT CourseID, CourseName, Cost FROM Courses ORDER BY CourseName ASC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            cbCourse.DataSource = dt;
                            cbCourse.DisplayMember = "CourseName";
                            cbCourse.ValueMember = "CourseID";
                        }
                    }
                }
                AutoFillCourseCost();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadCourses failed: " + ex.Message);
            }
        }

        private void AutoFillCourseCost()
        {
            if (cbCourse.SelectedItem != null)
            {
                DataRowView drv = cbCourse.SelectedItem as DataRowView;
                if (drv != null)
                {
                    txtDueAmount.Text = Convert.ToDecimal(drv["Cost"]).ToString("0.00");
                }
            }
        }

        private void CbCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            AutoFillCourseCost();
        }

        public void LoadDues()
        {
            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // In our new schema, financial assignments/dues are records in FinancialTransactions
                    // with TransactionType = 'Fee Charge'
                    string query = @"
                        SELECT
                            ft.TransactionID,
                            s.StudentName AS [اسم الطالب],
                            ft.Debit AS [قيمة المستحق (د.ل)],
                            ft.Notes AS [ملاحظات],
                            ft.TransactionDate AS [تاريخ التعيين]
                        FROM FinancialTransactions ft
                        INNER JOIN Students s ON ft.StudentID = s.StudentID
                        WHERE ft.TransactionType = 'Fee Charge'
                        ORDER BY ft.TransactionID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvDues.DataSource = dt;

                            if (dgvDues.Columns.Contains("TransactionID"))
                                dgvDues.Columns["TransactionID"].Visible = false;

                            dgvDues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadDues failed: " + ex.Message);
            }
        }

        private void ClearInputs()
        {
            if (cbStudent.Items.Count > 0) cbStudent.SelectedIndex = 0;
            if (cbCourse.Items.Count > 0) cbCourse.SelectedIndex = 0;
            txtNotes.Clear();
            AutoFillCourseCost();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cbStudent.SelectedValue == null)
            {
                MessageBox.Show("يرجى اختيار طالب أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbCourse.SelectedValue == null)
            {
                MessageBox.Show("يرجى اختيار دورة أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal dueAmount;
            if (!decimal.TryParse(txtDueAmount.Text.Trim(), out dueAmount) || dueAmount < 0)
            {
                MessageBox.Show("يرجى إدخال مبلغ مستحق صحيح.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // 1. Save record by inserting into FinancialTransactions
                    // TransactionType = 'Fee Charge'
                    // Debit = Course Cost
                    // Credit = 0.00
                    // UserID = Current Logged-in User ID (Since there's no auth, we use UserID = 1 as default)
                    string queryPay = @"
                        INSERT INTO FinancialTransactions (StudentID, TransactionType, Debit, Credit, TransactionDate, Notes, UserID)
                        VALUES (@StudentID, 'Fee Charge', @Debit, 0.00, @TransactionDate, @Notes, @UserID)";
                    using (SqlCommand cmd = new SqlCommand(queryPay, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", cbStudent.SelectedValue);
                        cmd.Parameters.AddWithValue("@Debit", dueAmount);
                        cmd.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(txtNotes.Text) ? "تعيين دورة: " + cbCourse.Text : txtNotes.Text.Trim());
                        cmd.Parameters.AddWithValue("@UserID", 1); // Default admin user seeded
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("تم تعيين المستحقات المالية بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
                LoadDues();
                OnDataSaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء حفظ المستحقات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
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