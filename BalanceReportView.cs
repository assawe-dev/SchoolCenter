using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SchoolCenter
{
    public class BalanceReportView : UserControl
    {
        private Panel pnlHeader;
        private Label lblTitle;
        private Button btnExport;

        private Panel pnlSearch;
        private Label lblSearch;
        private TextBox txtSearch;

        private DataGridView dgvReport;
        private Panel pnlGrid;

        private Panel pnlSummary;
        private Label lblTotalDebtTitle;
        private Label lblTotalDebtValue;

        public event EventHandler DataSaved;

        public BalanceReportView()
        {
            InitializeComponent();
            ThemeHelper.ApplyTheme(this);
            LoadReport();
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new Panel();
            this.lblTitle = new Label();
            this.btnExport = new Button();

            this.pnlSearch = new Panel();
            this.lblSearch = new Label();
            this.txtSearch = new TextBox();

            this.pnlGrid = new Panel();
            this.dgvReport = new DataGridView();

            this.pnlSummary = new Panel();
            this.lblTotalDebtTitle = new Label();
            this.lblTotalDebtValue = new Label();

            this.pnlHeader.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.pnlSummary.SuspendLayout();
            this.SuspendLayout();

            //
            // BalanceReportView
            //
            this.BackColor = Color.FromArgb(248, 249, 250); // Off-White
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlSummary);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlHeader);
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 10F);
            this.Name = "BalanceReportView";
            this.RightToLeft = RightToLeft.Yes;
            this.Size = new Size(820, 600);

            //
            // pnlHeader
            //
            this.pnlHeader.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.pnlHeader.BackColor = Color.White;
            this.pnlHeader.Controls.Add(this.btnExport);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Location = new Point(20, 20);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new Size(780, 65);
            this.pnlHeader.TabIndex = 0;

            //
            // lblTitle
            //
            this.lblTitle.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblTitle.Location = new Point(500, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(250, 30);
            this.lblTitle.Text = "تقرير أرصدة وديون الطلاب الشامل";

            //
            // btnExport
            //
            this.btnExport.BackColor = Color.FromArgb(46, 204, 113); // Success Green
            this.btnExport.Cursor = Cursors.Hand;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = FlatStyle.Flat;
            this.btnExport.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnExport.ForeColor = Color.White;
            this.btnExport.Location = new Point(20, 13);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new Size(180, 38);
            this.btnExport.Text = "تصدير إلى Excel 📥";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new EventHandler(this.BtnExport_Click);

            //
            // pnlSummary
            //
            this.pnlSummary.Anchor = ((AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.pnlSummary.BackColor = Color.FromArgb(231, 76, 60); // Danger/Warning Color
            this.pnlSummary.Controls.Add(this.lblTotalDebtValue);
            this.pnlSummary.Controls.Add(this.lblTotalDebtTitle);
            this.pnlSummary.Location = new Point(20, 505);
            this.pnlSummary.Name = "pnlSummary";
            this.pnlSummary.Size = new Size(780, 75);
            this.pnlSummary.TabIndex = 2;

            //
            // lblTotalDebtTitle
            //
            this.lblTotalDebtTitle.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.lblTotalDebtTitle.AutoSize = true;
            this.lblTotalDebtTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.lblTotalDebtTitle.ForeColor = Color.White;
            this.lblTotalDebtTitle.Location = new Point(500, 22);
            this.lblTotalDebtTitle.Name = "lblTotalDebtTitle";
            this.lblTotalDebtTitle.Size = new Size(251, 30);
            this.lblTotalDebtTitle.Text = "إجمالي الديون المستحقة الكلية:";

            //
            // lblTotalDebtValue
            //
            this.lblTotalDebtValue.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTotalDebtValue.ForeColor = Color.White;
            this.lblTotalDebtValue.Location = new Point(20, 15);
            this.lblTotalDebtValue.Name = "lblTotalDebtValue";
            this.lblTotalDebtValue.Size = new Size(350, 45);
            this.lblTotalDebtValue.Text = "0.00 د.ل";
            this.lblTotalDebtValue.TextAlign = ContentAlignment.MiddleLeft;

            //
            // pnlSearch
            //
            this.pnlSearch.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.pnlSearch.BackColor = Color.White;
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Location = new Point(20, 100);
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
            this.lblSearch.Location = new Point(540, 13);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new Size(220, 23);
            this.lblSearch.Text = "بحث سريع باسم الطالب أو الهاتف:";

            //
            // txtSearch
            //
            this.txtSearch.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.txtSearch.Location = new Point(40, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(490, 30);
            this.txtSearch.TextChanged += new EventHandler(this.TxtSearch_TextChanged);

            //
            // pnlGrid
            //
            this.pnlGrid.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.pnlGrid.Controls.Add(this.dgvReport);
            this.pnlGrid.Location = new Point(20, 165);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new Size(780, 325);
            this.pnlGrid.TabIndex = 2;

            //
            // dgvReport
            //
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 244, 244);
            this.dgvReport.BackgroundColor = Color.White;
            this.dgvReport.BorderStyle = BorderStyle.None;
            this.dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            this.dgvReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvReport.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            this.dgvReport.DefaultCellStyle.SelectionForeColor = Color.White;
            this.dgvReport.EnableHeadersVisualStyles = false;
            this.dgvReport.Dock = DockStyle.Fill;
            this.dgvReport.Location = new Point(0, 0);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.pnlSummary.ResumeLayout(false);
            this.pnlSummary.PerformLayout();
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
                LoadReport();
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
                            s.StudentName AS [اسم الطالب],
                            s.GuardianName AS [اسم ولي الأمر],
                            s.ParentPhone AS [رقم هاتف ولي الأمر],
                            ISNULL(SUM(ft.Debit), 0) AS [إجمالي المستحقات],
                            ISNULL(SUM(ft.Credit), 0) AS [إجمالي المدفوعات],
                            (ISNULL(SUM(ft.Debit), 0) - ISNULL(SUM(ft.Credit), 0)) AS [الديون المتبقية]
                        FROM Students s
                        LEFT JOIN FinancialTransactions ft ON s.StudentID = ft.StudentID
                        WHERE s.StudentName LIKE @Filter OR s.ParentPhone LIKE @Filter OR s.GuardianName LIKE @Filter
                        GROUP BY s.StudentID, s.StudentName, s.GuardianName, s.ParentPhone
                        ORDER BY s.StudentName ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Filter", "%" + filterText + "%");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvReport.DataSource = dt;

                            if (dgvReport.Columns.Contains("StudentID"))
                                dgvReport.Columns["StudentID"].Visible = false;

                            dgvReport.Columns["إجمالي المستحقات"].DefaultCellStyle.Format = "N2";
                            dgvReport.Columns["إجمالي المدفوعات"].DefaultCellStyle.Format = "N2";
                            dgvReport.Columns["الديون المتبقية"].DefaultCellStyle.Format = "N2";

                            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                    }
                }

                // Recalculate total outstanding debts based on filtered datagrid rows
                decimal totalOutstanding = 0m;
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    if (row.Cells["الديون المتبقية"].Value != null && row.Cells["الديون المتبقية"].Value != DBNull.Value)
                    {
                        totalOutstanding += Convert.ToDecimal(row.Cells["الديون المتبقية"].Value);
                    }
                }
                lblTotalDebtValue.Text = totalOutstanding.ToString("N2") + " د.ل";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Search failed: " + ex.Message);
            }
        }

        public void LoadReport()
        {
            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Using the exact specified T-SQL query for the DataGridView reporting
                    string query = @"
                        SELECT
                            s.StudentID,
                            s.StudentName AS [اسم الطالب],
                            s.GuardianName AS [اسم ولي الأمر],
                            s.ParentPhone AS [رقم هاتف ولي الأمر],
                            ISNULL(SUM(ft.Debit), 0) AS [إجمالي المستحقات],
                            ISNULL(SUM(ft.Credit), 0) AS [إجمالي المدفوعات],
                            (ISNULL(SUM(ft.Debit), 0) - ISNULL(SUM(ft.Credit), 0)) AS [الديون المتبقية]
                        FROM Students s
                        LEFT JOIN FinancialTransactions ft ON s.StudentID = ft.StudentID
                        GROUP BY s.StudentID, s.StudentName, s.GuardianName, s.ParentPhone
                        ORDER BY s.StudentName ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvReport.DataSource = dt;

                            if (dgvReport.Columns.Contains("StudentID"))
                                dgvReport.Columns["StudentID"].Visible = false;

                            dgvReport.Columns["إجمالي المستحقات"].DefaultCellStyle.Format = "N2";
                            dgvReport.Columns["إجمالي المدفوعات"].DefaultCellStyle.Format = "N2";
                            dgvReport.Columns["الديون المتبقية"].DefaultCellStyle.Format = "N2";

                            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                    }
                }

                CalculateTotalOutstandingDebts();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadReport failed: " + ex.Message);
            }
        }

        private void CalculateTotalOutstandingDebts()
        {
            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Using the exact specified T-SQL query for the Total System Debt Label
                    string query = "SELECT ISNULL(SUM(Debit), 0) - ISNULL(SUM(Credit), 0) FROM FinancialTransactions";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        decimal totalOutstanding = 0m;
                        if (result != null && result != DBNull.Value)
                        {
                            totalOutstanding = Convert.ToDecimal(result);
                        }
                        lblTotalDebtValue.Text = totalOutstanding.ToString("N2") + " د.ل";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CalculateTotalOutstandingDebts failed: " + ex.Message);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvReport.Rows.Count == 0)
            {
                MessageBox.Show("لا توجد بيانات متاحة للتصدير.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "ملف اكسل مبسط (*.csv)|*.csv";
            sfd.FileName = "تقرير_الديون_والأرصدة_" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    // كتابة ترويسة الأعمدة مع دعم UTF-8 BOM لكي يتعرف عليها Excel باللغة العربية بشكل صحيح
                    string[] columnNames = new string[dgvReport.Columns.Count - 1]; // Exclude StudentID
                    int colIndex = 0;
                    for (int i = 0; i < dgvReport.Columns.Count; i++)
                    {
                        if (dgvReport.Columns[i].Name == "StudentID") continue;
                        columnNames[colIndex++] = EscapeCsvField(dgvReport.Columns[i].HeaderText);
                    }
                    sb.AppendLine(string.Join(",", columnNames));

                    // كتابة البيانات
                    foreach (DataGridViewRow row in dgvReport.Rows)
                    {
                        string[] fields = new string[dgvReport.Columns.Count - 1];
                        colIndex = 0;
                        for (int i = 0; i < dgvReport.Columns.Count; i++)
                        {
                            if (dgvReport.Columns[i].Name == "StudentID") continue;
                            fields[colIndex++] = EscapeCsvField(row.Cells[i].Value != null ? row.Cells[i].Value.ToString() : "");
                        }
                        sb.AppendLine(string.Join(",", fields));
                    }

                    // إضافة سطر المجموع في نهاية ملف الـ CSV ليتطابق مع الـ Summary Panel
                    sb.AppendLine();
                    sb.AppendLine(EscapeCsvField("إجمالي الديون المستحقة الكلية") + ",,,," + EscapeCsvField(lblTotalDebtValue.Text));

                    // كتابة الملف بترميز UTF-8 مع الـ BOM لضمان الدعم الكامل للغة العربية في Excel
                    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, new UTF8Encoding(true)))
                    {
                        sw.Write(sb.ToString());
                    }

                    MessageBox.Show("تم تصدير التقرير بنجاح إلى ملف Excel / CSV.", "تصدير موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ أثناء التصدير: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field)) return "";
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            }
            return field;
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