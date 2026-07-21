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

        private TableLayoutPanel tlpInput;

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
            this.tlpInput = new TableLayoutPanel();

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
            this.tlpInput.SuspendLayout();
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

            this.tlpInput.Controls.Add(this.lblStudent, 0, 0);
            this.tlpInput.Controls.Add(this.cbStudent, 1, 0);
            this.tlpInput.Controls.Add(this.lblCourse, 2, 0);
            this.tlpInput.Controls.Add(this.cbCourse, 3, 0);
            this.tlpInput.Controls.Add(this.lblDueAmount, 0, 1);
            this.tlpInput.Controls.Add(this.txtDueAmount, 1, 1);
            this.tlpInput.Controls.Add(this.lblNotes, 2, 1);
            this.tlpInput.Controls.Add(this.txtNotes, 3, 1);

            // Button flow panel
            FlowLayoutPanel flpButtons = new FlowLayoutPanel();
            flpButtons.FlowDirection = FlowDirection.RightToLeft;
            flpButtons.Controls.Add(this.btnSave);
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
            // lblStudent
            //
            this.lblStudent.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.lblStudent.AutoSize = true;
            this.lblStudent.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblStudent.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblStudent.Location = new Point(675, 26);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new Size(112, 23);
            this.lblStudent.Text = "اختر الطالب:";
            this.lblStudent.TextAlign = ContentAlignment.MiddleLeft;

            //
            // cbStudent
            //
            this.cbStudent.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.cbStudent.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStudent.FormattingEnabled = true;
            this.cbStudent.Location = new Point(413, 22);
            this.cbStudent.Name = "cbStudent";
            this.cbStudent.Size = new Size(256, 31);
            this.cbStudent.TabIndex = 0;

            //
            // lblCourse
            //
            this.lblCourse.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.lblCourse.AutoSize = true;
            this.lblCourse.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblCourse.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblCourse.Location = new Point(295, 26);
            this.lblCourse.Name = "lblCourse";
            this.lblCourse.Size = new Size(112, 23);
            this.lblCourse.Text = "اختر الدورة:";
            this.lblCourse.TextAlign = ContentAlignment.MiddleLeft;

            //
            // cbCourse
            //
            this.cbCourse.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.cbCourse.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbCourse.FormattingEnabled = true;
            this.cbCourse.Location = new Point(18, 22);
            this.cbCourse.Name = "cbCourse";
            this.cbCourse.Size = new Size(271, 31);
            this.cbCourse.TabIndex = 1;
            this.cbCourse.SelectedIndexChanged += new EventHandler(this.CbCourse_SelectedIndexChanged);

            //
            // lblDueAmount
            //
            this.lblDueAmount.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.lblDueAmount.AutoSize = true;
            this.lblDueAmount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblDueAmount.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblDueAmount.Location = new Point(675, 71);
            this.lblDueAmount.Name = "lblDueAmount";
            this.lblDueAmount.Size = new Size(112, 23);
            this.lblDueAmount.Text = "تكلفة الدورة:";
            this.lblDueAmount.TextAlign = ContentAlignment.MiddleLeft;

            //
            // txtDueAmount
            //
            this.txtDueAmount.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtDueAmount.Location = new Point(413, 67);
            this.txtDueAmount.Name = "txtDueAmount";
            this.txtDueAmount.ReadOnly = true;
            this.txtDueAmount.Size = new Size(256, 30);
            this.txtDueAmount.TabIndex = 2;
            this.txtDueAmount.TabStop = false;

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
            this.lblNotes.Text = "ملاحظات الدورة:";
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
            // btnSave
            //
            this.btnSave.BackColor = Color.FromArgb(52, 152, 219); // Modern Action Blue
            this.btnSave.Cursor = Cursors.Hand;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(160, 38);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "تعيين المستحق المالي";
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
            this.btnClear.Location = new Point(169, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(120, 38);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "مسح الحقول";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new EventHandler(this.BtnClear_Click);

            //
            // pnlGrid
            //
            this.pnlGrid.Controls.Add(this.dgvDues);
            this.pnlGrid.Dock = DockStyle.Fill;
            this.pnlGrid.Location = new Point(0, 195);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new Padding(15);
            this.pnlGrid.Size = new Size(820, 405);
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
            this.dgvDues.Location = new Point(15, 15);
            this.dgvDues.Name = "dgvDues";
            this.dgvDues.ReadOnly = true;
            this.dgvDues.RowHeadersVisible = false;
            this.dgvDues.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.pnlInput.ResumeLayout(false);
            this.tlpInput.ResumeLayout(false);
            this.tlpInput.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDues)).EndInit();
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
                    string query = @"
                        SELECT
                            ft.TransactionID AS [رقم المستحق],
                            s.StudentName AS [اسم الطالب],
                            ft.Notes AS [اسم الدورة / ملاحظات],
                            ft.Debit AS [قيمة المستحق (د.ل)],
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
                int currentUserID = DbConnectionManager.GetDefaultUserID();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Insert directly into FinancialTransactions (TransactionType = 'Fee Charge', Debit = cost, Credit = 0.00)
                    string queryDue = @"
                        INSERT INTO FinancialTransactions (StudentID, TransactionType, Debit, Credit, TransactionDate, Notes, UserID)
                        VALUES (@StudentID, 'Fee Charge', @Debit, 0.00, @TransactionDate, @Notes, @UserID)";

                    using (SqlCommand cmdDue = new SqlCommand(queryDue, conn))
                    {
                        cmdDue.Parameters.AddWithValue("@StudentID", cbStudent.SelectedValue);
                        cmdDue.Parameters.AddWithValue("@Debit", dueAmount);
                        cmdDue.Parameters.AddWithValue("@TransactionDate", DateTime.Now);

                        string courseInfo = cbCourse.Text;
                        string finalNotes = "رسوم الدورة: " + courseInfo + (string.IsNullOrEmpty(txtNotes.Text) ? "" : " - " + txtNotes.Text.Trim());
                        cmdDue.Parameters.AddWithValue("@Notes", finalNotes);
                        cmdDue.Parameters.AddWithValue("@UserID", currentUserID);
                        cmdDue.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("تم تعيين المستحقات المالية (رسوم الدورة) بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
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