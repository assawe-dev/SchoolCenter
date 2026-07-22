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

        private Panel pnlSearch;
        private Label lblSearch;
        private TextBox txtSearch;

        private DataGridView dgvCourses;
        private Panel pnlGrid;

        private int selectedCourseID = -1;

        public event EventHandler DataSaved;

        public CoursesView()
        {
            InitializeComponent();
            ThemeHelper.ApplyTheme(this);
            LoadCourses();
        }

        private void InitializeComponent()
        {
            this.pnlInput = new Panel();
            this.lblCourseName = new Label();
            this.txtCourseName = new TextBox();
            this.lblCourseCost = new Label();
            this.txtCourseCost = new TextBox();
            this.btnAdd = new Button();
            this.btnUpdate = new Button();
            this.btnDelete = new Button();
            this.btnClear = new Button();

            this.pnlSearch = new Panel();
            this.lblSearch = new Label();
            this.txtSearch = new TextBox();

            this.pnlGrid = new Panel();
            this.dgvCourses = new DataGridView();

            this.pnlInput.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourses)).BeginInit();
            this.SuspendLayout();

            //
            // CoursesView
            //
            this.BackColor = Color.FromArgb(248, 250, 252); // Modern SaaS #F8FAFC
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlInput);
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Cairo", 10F);
            this.Name = "CoursesView";
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
            this.pnlInput.Controls.Add(this.txtCourseCost);
            this.pnlInput.Controls.Add(this.lblCourseCost);
            this.pnlInput.Controls.Add(this.txtCourseName);
            this.pnlInput.Controls.Add(this.lblCourseName);
            this.pnlInput.Location = new Point(20, 20);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new Size(780, 150);
            this.pnlInput.TabIndex = 0;

            //
            // lblCourseName
            //
            this.lblCourseName.AutoSize = true;
            this.lblCourseName.Font = new Font("Cairo", 10F, FontStyle.Bold);
            this.lblCourseName.ForeColor = Color.FromArgb(15, 23, 42);
            this.lblCourseName.Location = new Point(650, 25);
            this.lblCourseName.Name = "lblCourseName";
            this.lblCourseName.Size = new Size(110, 23);
            this.lblCourseName.Text = "اسم الدورة:";

            //
            // txtCourseName
            //
            this.txtCourseName.Location = new Point(370, 22);
            this.txtCourseName.Name = "txtCourseName";
            this.txtCourseName.Size = new Size(270, 30);

            //
            // lblCourseCost
            //
            this.lblCourseCost.AutoSize = true;
            this.lblCourseCost.Font = new Font("Cairo", 10F, FontStyle.Bold);
            this.lblCourseCost.ForeColor = Color.FromArgb(15, 23, 42);
            this.lblCourseCost.Location = new Point(240, 25);
            this.lblCourseCost.Name = "lblCourseCost";
            this.lblCourseCost.Size = new Size(115, 23);
            this.lblCourseCost.Text = "تكلفة الدورة:";

            //
            // txtCourseCost
            //
            this.txtCourseCost.Location = new Point(40, 22);
            this.txtCourseCost.Name = "txtCourseCost";
            this.txtCourseCost.Size = new Size(190, 30);

            //
            // btnAdd
            //
            this.btnAdd.BackColor = Color.FromArgb(37, 99, 235); // Accent Blue #2563EB
            this.btnAdd.Cursor = Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.Font = new Font("Cairo", 10F, FontStyle.Bold);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Location = new Point(520, 85);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(120, 35);
            this.btnAdd.Text = "إضافة دورة";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new EventHandler(this.BtnAdd_Click);

            //
            // btnUpdate
            //
            this.btnUpdate.BackColor = Color.FromArgb(16, 185, 129); // Success green #10B981
            this.btnUpdate.Cursor = Cursors.Hand;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = FlatStyle.Flat;
            this.btnUpdate.Font = new Font("Cairo", 10F, FontStyle.Bold);
            this.btnUpdate.ForeColor = Color.White;
            this.btnUpdate.Location = new Point(390, 85);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new Size(120, 35);
            this.btnUpdate.Text = "تعديل البيانات";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new EventHandler(this.BtnUpdate_Click);

            //
            // btnDelete
            //
            this.btnDelete.BackColor = Color.FromArgb(239, 68, 68); // Soft danger red #EF4444
            this.btnDelete.Cursor = Cursors.Hand;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Cairo", 10F, FontStyle.Bold);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Location = new Point(260, 85);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(120, 35);
            this.btnDelete.Text = "حذف";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new EventHandler(this.BtnDelete_Click);

            //
            // btnClear
            //
            this.btnClear.BackColor = Color.FromArgb(100, 116, 139); // Muted gray #64748B
            this.btnClear.Cursor = Cursors.Hand;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.Font = new Font("Cairo", 10F, FontStyle.Bold);
            this.btnClear.ForeColor = Color.White;
            this.btnClear.Location = new Point(130, 85);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(120, 35);
            this.btnClear.Text = "مسح حقول";
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
            this.pnlSearch.Location = new Point(20, 185);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new Size(780, 50);
            this.pnlSearch.TabIndex = 1;

            //
            // lblSearch
            //
            this.lblSearch.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new Font("Cairo", 10F, FontStyle.Bold);
            this.lblSearch.ForeColor = Color.FromArgb(15, 23, 42);
            this.lblSearch.Location = new Point(620, 13);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new Size(140, 23);
            this.lblSearch.Text = "بحث سريع بالاسم:";

            //
            // txtSearch
            //
            this.txtSearch.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.txtSearch.Location = new Point(40, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(570, 30);
            this.txtSearch.TextChanged += new EventHandler(this.TxtSearch_TextChanged);

            //
            // pnlGrid
            //
            this.pnlGrid.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.pnlGrid.Controls.Add(this.dgvCourses);
            this.pnlGrid.Location = new Point(20, 250);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new Size(780, 330);
            this.pnlGrid.TabIndex = 2;

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
            this.dgvCourses.Location = new Point(0, 0);
            this.dgvCourses.Name = "dgvCourses";
            this.dgvCourses.ReadOnly = true;
            this.dgvCourses.RowHeadersVisible = false;
            this.dgvCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCourses.CellClick += new DataGridViewCellEventHandler(this.DgvCourses_CellClick);

            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourses)).EndInit();
            this.ResumeLayout(false);
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
                LoadCourses();
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
                            CourseID,
                            CourseName AS [اسم الدورة],
                            Cost AS [تكلفة الدورة (د.ل)]
                        FROM Courses
                        WHERE CourseName LIKE @Filter
                        ORDER BY CourseID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Filter", "%" + filterText + "%");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvCourses.DataSource = dt;

                            if (dgvCourses.Columns.Contains("CourseID"))
                                dgvCourses.Columns["CourseID"].Visible = false;

                            dgvCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Search failed: " + ex.Message);
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
                            CourseID,
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

                            if (dgvCourses.Columns.Contains("CourseID"))
                                dgvCourses.Columns["CourseID"].Visible = false;

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
                    string query = "UPDATE Courses SET CourseName = @CourseName, Cost = @Cost WHERE CourseID = @CourseID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseName", txtCourseName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Cost", cost);
                        cmd.Parameters.AddWithValue("@CourseID", selectedCourseID);
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

            DialogResult dialogResult = MessageBox.Show("هل أنت متأكد من حذف هذه الدورة نهائياً؟ سيتم حذف جميع تسجيلات الحركات المتعلقة بها أيضاً.", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string connectionString = DbConnectionManager.GetConnectionString();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM Courses WHERE CourseID = @CourseID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@CourseID", selectedCourseID);
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
                selectedCourseID = Convert.ToInt32(row.Cells["CourseID"].Value);
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