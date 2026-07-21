using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SchoolCenter
{
    public class CoursesView : UserControl
    {
        private Panel pnlInput;
        private Label lblCourseName;
        private TextBox txtCourseName;
        private Label lblCourseCost;
        private TextBox txtCourseCost;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;

        private DataGridView dgvCourses;
        private Panel pnlGrid;

        private TableLayoutPanel tlpInput;

        private int selectedCourseID = -1;

        public event EventHandler DataSaved;

        public CoursesView()
        {
            InitializeComponent();
            LoadCourses();
        }

        private void InitializeComponent()
        {
            this.pnlInput = new Panel();
            this.tlpInput = new TableLayoutPanel();

            this.lblCourseName = new Label();
            this.txtCourseName = new TextBox();
            this.lblCourseCost = new Label();
            this.txtCourseCost = new TextBox();

            this.btnAdd = new Button();
            this.btnUpdate = new Button();
            this.btnDelete = new Button();
            this.btnClear = new Button();

            this.pnlGrid = new Panel();
            this.dgvCourses = new DataGridView();

            this.pnlInput.SuspendLayout();
            this.tlpInput.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourses)).BeginInit();
            this.SuspendLayout();

            //
            // CoursesView
            //
            this.BackColor = Color.FromArgb(248, 249, 250); // Off-White
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlInput);
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 10F);
            this.Name = "CoursesView";
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
            this.pnlInput.Size = new Size(820, 160);
            this.pnlInput.TabIndex = 0;

            //
            // tlpInput
            //
            this.tlpInput.ColumnCount = 4;
            this.tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            this.tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            this.tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            this.tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            this.tlpInput.RowCount = 2;
            this.tlpInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            this.tlpInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));

            this.tlpInput.Controls.Add(this.lblCourseName, 0, 0);
            this.tlpInput.Controls.Add(this.txtCourseName, 1, 0);
            this.tlpInput.Controls.Add(this.lblCourseCost, 2, 0);
            this.tlpInput.Controls.Add(this.txtCourseCost, 3, 0);

            // Button flow layout
            FlowLayoutPanel flpButtons = new FlowLayoutPanel();
            flpButtons.FlowDirection = FlowDirection.RightToLeft;
            flpButtons.Controls.Add(this.btnAdd);
            flpButtons.Controls.Add(this.btnUpdate);
            flpButtons.Controls.Add(this.btnDelete);
            flpButtons.Controls.Add(this.btnClear);
            flpButtons.Dock = DockStyle.Fill;
            flpButtons.Margin = new Padding(0);

            this.tlpInput.Controls.Add(flpButtons, 0, 1);
            this.tlpInput.SetColumnSpan(flpButtons, 4);

            this.tlpInput.Dock = DockStyle.Fill;
            this.tlpInput.Location = new Point(0, 0);
            this.tlpInput.Name = "tlpInput";
            this.tlpInput.Padding = new Padding(15);
            this.tlpInput.TabIndex = 0;

            //
            // lblCourseName
            //
            this.lblCourseName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.lblCourseName.AutoSize = true;
            this.lblCourseName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblCourseName.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblCourseName.Location = new Point(675, 28);
            this.lblCourseName.Name = "lblCourseName";
            this.lblCourseName.Size = new Size(112, 23);
            this.lblCourseName.Text = "اسم الدورة:";
            this.lblCourseName.TextAlign = ContentAlignment.MiddleLeft;

            //
            // txtCourseName
            //
            this.txtCourseName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtCourseName.Location = new Point(413, 25);
            this.txtCourseName.Name = "txtCourseName";
            this.txtCourseName.Size = new Size(256, 30);
            this.txtCourseName.TabIndex = 0;
            this.txtCourseName.KeyDown += new KeyEventHandler(this.Input_KeyDown);

            //
            // lblCourseCost
            //
            this.lblCourseCost.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.lblCourseCost.AutoSize = true;
            this.lblCourseCost.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblCourseCost.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblCourseCost.Location = new Point(295, 28);
            this.lblCourseCost.Name = "lblCourseCost";
            this.lblCourseCost.Size = new Size(112, 23);
            this.lblCourseCost.Text = "تكلفة الدورة:";
            this.lblCourseCost.TextAlign = ContentAlignment.MiddleLeft;

            //
            // txtCourseCost
            //
            this.txtCourseCost.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtCourseCost.Location = new Point(18, 25);
            this.txtCourseCost.Name = "txtCourseCost";
            this.txtCourseCost.Size = new Size(271, 30);
            this.txtCourseCost.TabIndex = 1;
            this.txtCourseCost.KeyDown += new KeyEventHandler(this.Input_KeyDown);

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
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "إضافة دورة";
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
            this.btnUpdate.TabIndex = 3;
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
            this.btnDelete.TabIndex = 4;
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
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "مسح حقول";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new EventHandler(this.BtnClear_Click);

            //
            // pnlGrid
            //
            this.pnlGrid.Controls.Add(this.dgvCourses);
            this.pnlGrid.Dock = DockStyle.Fill;
            this.pnlGrid.Location = new Point(0, 160);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new Padding(15);
            this.pnlGrid.Size = new Size(820, 440);
            this.pnlGrid.TabIndex = 1;

            //
            // dgvCourses
            //
            this.dgvCourses.AllowUserToAddRows = false;
            this.dgvCourses.AllowUserToDeleteRows = false;
            this.dgvCourses.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 244, 244);
            this.dgvCourses.BackgroundColor = Color.White;
            this.dgvCourses.BorderStyle = BorderStyle.None;
            this.dgvCourses.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            this.dgvCourses.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvCourses.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvCourses.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            this.dgvCourses.DefaultCellStyle.SelectionForeColor = Color.White;
            this.dgvCourses.EnableHeadersVisualStyles = false;
            this.dgvCourses.Dock = DockStyle.Fill;
            this.dgvCourses.Location = new Point(15, 15);
            this.dgvCourses.Name = "dgvCourses";
            this.dgvCourses.ReadOnly = true;
            this.dgvCourses.RowHeadersVisible = false;
            this.dgvCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCourses.CellClick += new DataGridViewCellEventHandler(this.DgvCourses_CellClick);

            this.pnlInput.ResumeLayout(false);
            this.tlpInput.ResumeLayout(false);
            this.tlpInput.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourses)).EndInit();
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

        public void LoadCourses()
        {
            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT
                            CourseID AS [Id],
                            CourseName AS [اسم الدورة],
                            Cost AS [تكلفة الدورة (د.ل)]
                        FROM Courses
                        ORDER BY CourseID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvCourses.DataSource = dt;

                            if (dgvCourses.Columns.Contains("Id"))
                                dgvCourses.Columns["Id"].Visible = false;

                            dgvCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadCourses failed: " + ex.Message);
            }
        }

        private void ClearInputs()
        {
            txtCourseName.Clear();
            txtCourseCost.Clear();
            selectedCourseID = -1;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCourseName.Text))
            {
                MessageBox.Show("يرجى إدخال اسم الدورة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal cost;
            if (!decimal.TryParse(txtCourseCost.Text.Trim(), out cost) || cost < 0)
            {
                MessageBox.Show("يرجى إدخال تكلفة صحيحة للدورة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Courses (CourseName, Cost) VALUES (@CourseName, @Cost)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseName", txtCourseName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Cost", cost);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("تم إضافة الدورة بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
                LoadCourses();
                OnDataSaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء إضافة الدورة: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedCourseID == -1)
            {
                MessageBox.Show("يرجى تحديد دورة من الجدول لتعديل بياناتها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCourseName.Text))
            {
                MessageBox.Show("يرجى إدخال اسم الدورة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal cost;
            if (!decimal.TryParse(txtCourseCost.Text.Trim(), out cost) || cost < 0)
            {
                MessageBox.Show("يرجى إدخال تكلفة صحيحة للدورة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Courses SET CourseName = @CourseName, Cost = @Cost WHERE CourseID = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseName", txtCourseName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Cost", cost);
                        cmd.Parameters.AddWithValue("@Id", selectedCourseID);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("تم تعديل بيانات الدورة بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
                LoadCourses();
                OnDataSaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تعديل الدورة: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedCourseID == -1)
            {
                MessageBox.Show("يرجى تحديد دورة من الجدول لحذفها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("هل أنت متأكد من حذف هذه الدورة نهائياً؟ سيتم حذف جميع تسجيلات المستحقات المتعلقة بها أيضاً.", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string connectionString = DbConnectionManager.GetConnectionString();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM Courses WHERE CourseID = @Id";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Id", selectedCourseID);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("تم حذف الدورة بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    LoadCourses();
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

        private void DgvCourses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCourses.Rows[e.RowIndex];
                selectedCourseID = Convert.ToInt32(row.Cells["Id"].Value);
                txtCourseName.Text = row.Cells["اسم الدورة"].Value.ToString();
                txtCourseCost.Text = row.Cells["تكلفة الدورة (د.ل)"].Value.ToString();
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