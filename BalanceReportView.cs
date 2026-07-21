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

        private DataGridView dgvReport;
        private Panel pnlGrid;

        private Panel pnlSummary;
        private Label lblTotalDebtTitle;
        private Label lblTotalDebtValue;

        public event EventHandler DataSaved;

        public BalanceReportView()
        {
            InitializeComponent();
            LoadReport();
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new Panel();
            this.lblTitle = new Label();
            this.btnExport = new Button();

            this.pnlGrid = new Panel();
            this.dgvReport = new DataGridView();

            this.pnlSummary = new Panel();
            this.lblTotalDebtTitle = new Label();
            this.lblTotalDebtValue = new Label();

            this.pnlHeader.SuspendLayout();
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
            this.Controls.Add(this.pnlHeader);
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 10F);
            this.Name = "BalanceReportView";
            this.RightToLeft = RightToLeft.Yes;
            this.Size = new Size(820, 600);

            //
            // pnlHeader
            //
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
            this.lblTotalDebtTitle.AutoSize = true;
            this.lblTotalDebtTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.lblTotalDebtTitle.ForeColor = Color.White;
            this.lblTotalDebtTitle.Location = new Point(500, 22);
            this.lblTotalDebtTitle.Name = "lblTotalDebtTitle";
            this.lblTotalDebtTitle.Size = new Size(251, 30);
            this.lblTotalDebtTitle.Text = "مجموع الديون المستحقة:";

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
            // pnlGrid
            //
            this.pnlGrid.Controls.Add(this.dgvReport);
            this.pnlGrid.Location = new Point(20, 100);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new Size(780, 390);
            this.pnlGrid.TabIndex = 1;

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
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.pnlSummary.ResumeLayout(false);
            this.pnlSummary.PerformLayout();
            this.ResumeLayout(false);
        }

        public void LoadReport()
        {
            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // تقرير شامل يحسب:
                    // - مجموع المستحقات لكل طالب (Total Dues) من جدول StudentDues
                    // - مجموع المدفوعات لكل طالب (Paid Amount) من جدول FinancialTransactions
                    // - المتبقي عليه كديون (Remaining Debt = Total Dues - Paid Amount)
                    string query = @"
                        SELECT
                            s.FullName AS [اسم الطالب],
                            s.GuardianPhone AS [رقم هاتف ولي الأمر],
                            COALESCE(STUFF((
                                SELECT DISTINCT ', ' + c.CourseName
                                FROM StudentDues d
                                INNER JOIN Courses c ON d.CourseId = c.Id
                                WHERE d.StudentId = s.Id
                                FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, ''), N'غير مسجل') AS [الدورات المنتسب إليها],
                            COALESCE((SELECT SUM(DueAmount) FROM StudentDues WHERE StudentId = s.Id), 0) AS [إجمالي المستحقات],
                            COALESCE((SELECT SUM(Amount) FROM FinancialTransactions WHERE StudentId = s.Id), 0) AS [إجمالي المدفوعات],
                            (COALESCE((SELECT SUM(DueAmount) FROM StudentDues WHERE StudentId = s.Id), 0) -
                             COALESCE((SELECT SUM(Amount) FROM FinancialTransactions WHERE StudentId = s.Id), 0)) AS [الديون المتبقية]
                        FROM Students s
                        ORDER BY s.FullName ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvReport.DataSource = dt;

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
            decimal totalOutstanding = 0m;
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (row.Cells["الديون المتبقية"].Value != DBNull.Value)
                {
                    totalOutstanding += Convert.ToDecimal(row.Cells["الديون المتبقية"].Value);
                }
            }
            lblTotalDebtValue.Text = totalOutstanding.ToString("N2") + " د.ل";
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
                    string[] columnNames = new string[dgvReport.Columns.Count];
                    for (int i = 0; i < dgvReport.Columns.Count; i++)
                    {
                        columnNames[i] = EscapeCsvField(dgvReport.Columns[i].HeaderText);
                    }
                    sb.AppendLine(string.Join(",", columnNames));

                    // كتابة البيانات
                    foreach (DataGridViewRow row in dgvReport.Rows)
                    {
                        string[] fields = new string[dgvReport.Columns.Count];
                        for (int i = 0; i < dgvReport.Columns.Count; i++)
                        {
                            fields[i] = EscapeCsvField(row.Cells[i].Value != null ? row.Cells[i].Value.ToString() : "");
                        }
                        sb.AppendLine(string.Join(",", fields));
                    }

                    // إضافة سطر المجموع في نهاية ملف الـ CSV ليتطابق مع الـ Summary Panel
                    sb.AppendLine();
                    sb.AppendLine(EscapeCsvField("مجموع الديون المستحقة") + ",,,,," + EscapeCsvField(lblTotalDebtValue.Text));

                    // كتابة الملف بترميز UTF-8 مع الـ BOM
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
            // إذا كان الحقل يحتوي على فواصل، علامات اقتباس، أو سطور جديدة، نقوم بإحاطته بالاقتباسات ودبلجة الاقتباسات الموجودة
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