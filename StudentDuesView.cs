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

        // Payment section (Optional / Instant Payment to update financial balance)
        private GroupBox gbPayment;
        private CheckBox chkInstantPay;
        private Label lblPayAmount;
        private TextBox txtPayAmount;
        private Label lblPayNotes;
        private TextBox txtPayNotes;

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

            this.gbPayment = new GroupBox();
            this.chkInstantPay = new CheckBox();
            this.lblPayAmount = new Label();
            this.txtPayAmount = new TextBox();
            this.lblPayNotes = new Label();
            this.txtPayNotes = new TextBox();

            this.btnSave = new Button();
            this.btnClear = new Button();

            this.pnlGrid = new Panel();
            this.dgvDues = new DataGridView();

            this.pnlInput.SuspendLayout();
            this.gbPayment.SuspendLayout();
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
            this.pnlInput.Controls.Add(this.gbPayment);
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
            // gbPayment
            //
            this.gbPayment.Controls.Add(this.chkInstantPay);
            this.gbPayment.Controls.Add(this.lblPayAmount);
            this.gbPayment.Controls.Add(this.txtPayAmount);
            this.gbPayment.Controls.Add(this.lblPayNotes);
            this.gbPayment.Controls.Add(this.txtPayNotes);
            this.gbPayment.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.gbPayment.ForeColor = Color.FromArgb(44, 62, 80);
            this.gbPayment.Location = new Point(20, 10);
            this.gbPayment.Name = "gbPayment";
            this.gbPayment.Size = new Size(360, 160);
            this.gbPayment.TabStop = false;
            this.gbPayment.Text = "تسجيل سداد مالي (اختياري)";

            //
            // chkInstantPay
            //
            this.chkInstantPay.AutoSize = true;
            this.chkInstantPay.Location = new Point(140, 25);
            this.chkInstantPay.Name = "chkInstantPay";
            this.chkInstantPay.Size = new Size(200, 26);
            this.chkInstantPay.Text = "تسجيل دفعة مسددة الآن";
            this.chkInstantPay.UseVisualStyleBackColor = true;
            this.chkInstantPay.CheckedChanged += new EventHandler(this.ChkInstantPay_CheckedChanged);

            //
            // lblPayAmount
            //
            this.lblPayAmount.AutoSize = true;
            this.lblPayAmount.Enabled = false;
            this.lblPayAmount.Location = new Point(250, 68);
            this.lblPayAmount.Name = "lblPayAmount";
            this.lblPayAmount.Size = new Size(100, 21);
            this.lblPayAmount.Text = "المبلغ المدفوع:";

            //
            // txtPayAmount
            //
            this.txtPayAmount.Enabled = false;
            this.txtPayAmount.Location = new Point(20, 65);
            this.txtPayAmount.Name = "txtPayAmount";
            this.txtPayAmount.Size = new Size(220, 29);

            //
            // lblPayNotes
            //
            this.lblPayNotes.AutoSize = true;
            this.lblPayNotes.Enabled = false;
            this.lblPayNotes.Location = new Point(250, 108);
            this.lblPayNotes.Name = "lblPayNotes";
            this.lblPayNotes.Size = new Size(100, 21);
            this.lblPayNotes.Text = "ملاحظات السداد:";

            //
            // txtPayNotes
            //
            this.txtPayNotes.Enabled = false;
            this.txtPayNotes.Location = new Point(20, 105);
            this.txtPayNotes.Name = "txtPayNotes";
            this.txtPayNotes.Size = new Size(220, 29);

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
            this.gbPayment.ResumeLayout(false);
            this.gbPayment.PerformLayout();
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
                    string query = "SELECT Id, FullName FROM Students ORDER BY FullName ASC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            cbStudent.DataSource = dt;
                            cbStudent.DisplayMember = "FullName";
                            cbStudent.ValueMember = "Id";
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
                    string query = "SELECT Id, CourseName, Cost FROM Courses ORDER BY CourseName ASC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            cbCourse.DataSource = dt;
                            cbCourse.DisplayMember = "CourseName";
                            cbCourse.ValueMember = "Id";
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
                    if (chkInstantPay.Checked)
                    {
                        txtPayAmount.Text = txtDueAmount.Text;
                    }
                }
            }
        }

        private void CbCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            AutoFillCourseCost();
        }

        private void ChkInstantPay_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkInstantPay.Checked;
            lblPayAmount.Enabled = isChecked;
            txtPayAmount.Enabled = isChecked;
            lblPayNotes.Enabled = isChecked;
            txtPayNotes.Enabled = isChecked;

            if (isChecked)
            {
                txtPayAmount.Text = txtDueAmount.Text;
            }
            else
            {
                txtPayAmount.Clear();
                txtPayNotes.Clear();
            }
        }

        public void LoadDues()
        {
            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT
                            d.Id,
                            s.FullName AS [اسم الطالب],
                            c.CourseName AS [اسم الدورة],
                            d.DueAmount AS [قيمة المستحق (د.ل)],
                            d.Notes AS [ملاحظات],
                            d.CreatedAt AS [تاريخ التعيين]
                        FROM StudentDues d
                        INNER JOIN Students s ON d.StudentId = s.Id
                        INNER JOIN Courses c ON d.CourseId = c.Id
                        ORDER BY d.Id DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvDues.DataSource = dt;

                            if (dgvDues.Columns.Contains("Id"))
                                dgvDues.Columns["Id"].Visible = false;

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
            chkInstantPay.Checked = false;
            txtPayAmount.Clear();
            txtPayNotes.Clear();
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

            decimal payAmount = 0;
            if (chkInstantPay.Checked)
            {
                if (!decimal.TryParse(txtPayAmount.Text.Trim(), out payAmount) || payAmount < 0)
                {
                    MessageBox.Show("يرجى إدخال مبلغ مدفوع صحيح.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
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
                            // 1. حفظ المستحق في StudentDues
                            string queryDue = "INSERT INTO StudentDues (StudentId, CourseId, DueAmount, Notes, CreatedAt) VALUES (@StudentId, @CourseId, @DueAmount, @Notes, @CreatedAt)";
                            using (SqlCommand cmdDue = new SqlCommand(queryDue, conn, trans))
                            {
                                cmdDue.Parameters.AddWithValue("@StudentId", cbStudent.SelectedValue);
                                cmdDue.Parameters.AddWithValue("@CourseId", cbCourse.SelectedValue);
                                cmdDue.Parameters.AddWithValue("@DueAmount", dueAmount);
                                cmdDue.Parameters.AddWithValue("@Notes", txtNotes.Text.Trim());
                                cmdDue.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                                cmdDue.ExecuteNonQuery();
                            }

                            // 2. إذا تم تحديد دفع فوري، حفظ الدفع في FinancialTransactions لتحديث رصيد الطالب المالي
                            if (chkInstantPay.Checked && payAmount > 0)
                            {
                                string queryPay = "INSERT INTO FinancialTransactions (StudentId, Amount, TransactionDate, Notes) VALUES (@StudentId, @Amount, @TransactionDate, @Notes)";
                                using (SqlCommand cmdPay = new SqlCommand(queryPay, conn, trans))
                                {
                                    cmdPay.Parameters.AddWithValue("@StudentId", cbStudent.SelectedValue);
                                    cmdPay.Parameters.AddWithValue("@Amount", payAmount);
                                    cmdPay.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                                    cmdPay.Parameters.AddWithValue("@Notes", "سداد فوري عند تعيين دورة: " + txtPayNotes.Text.Trim());
                                    cmdPay.ExecuteNonQuery();
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

                MessageBox.Show("تم تعيين المستحقات المالية وتحديث رصيد الطالب بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
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