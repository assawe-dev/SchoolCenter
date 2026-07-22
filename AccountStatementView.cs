using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SchoolCenter
{
    public class AccountStatementView : UserControl
    {
        private Panel pnlInput;
        private Label lblStudent;
        private ComboBox cmbStudents;
        private Label lblFromDate;
        private DateTimePicker dtpFromDate;
        private Label lblToDate;
        private DateTimePicker dtpToDate;
        private Button btnFilter;
        private Button btnPrintExport;

        private Panel pnlGrid;
        private DataGridView dgvStatement;

        private Panel pnlFooter;
        private Panel cardTotalCharged;
        private Label lblTotalChargedTitle;
        private Label lblTotalChargedValue;
        private Panel cardTotalPaid;
        private Label lblTotalPaidTitle;
        private Label lblTotalPaidValue;
        private Panel cardFinalBalance;
        private Label lblFinalBalanceTitle;
        private Label lblFinalBalanceValue;

        public event EventHandler DataSaved;

        public AccountStatementView()
        {
            InitializeComponent();
            ThemeHelper.ApplyTheme(this);
            LoadStudents();

            // Default date range: current month
            dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpToDate.Value = DateTime.Now;

            LoadStatement();
        }

        private void InitializeComponent()
        {
            this.pnlInput = new Panel();
            this.lblStudent = new Label();
            this.cmbStudents = new ComboBox();
            this.lblFromDate = new Label();
            this.dtpFromDate = new DateTimePicker();
            this.lblToDate = new Label();
            this.dtpToDate = new DateTimePicker();
            this.btnFilter = new Button();
            this.btnPrintExport = new Button();

            this.pnlGrid = new Panel();
            this.dgvStatement = new DataGridView();

            this.pnlFooter = new Panel();
            this.cardTotalCharged = new Panel();
            this.lblTotalChargedTitle = new Label();
            this.lblTotalChargedValue = new Label();
            this.cardTotalPaid = new Panel();
            this.lblTotalPaidTitle = new Label();
            this.lblTotalPaidValue = new Label();
            this.cardFinalBalance = new Panel();
            this.lblFinalBalanceTitle = new Label();
            this.lblFinalBalanceValue = new Label();

            this.pnlInput.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatement)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.cardTotalCharged.SuspendLayout();
            this.cardTotalPaid.SuspendLayout();
            this.cardFinalBalance.SuspendLayout();
            this.SuspendLayout();

            //
            // AccountStatementView
            //
            this.BackColor = Color.FromArgb(248, 250, 252); // Modern SaaS #F8FAFC
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlInput);
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Cairo", 10F);
            this.Name = "AccountStatementView";
            this.RightToLeft = RightToLeft.Yes;
            this.Size = new Size(820, 600);

            //
            // pnlInput
            //
            this.pnlInput.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right)));
            this.pnlInput.BackColor = Color.White;
            this.pnlInput.Controls.Add(this.btnPrintExport);
            this.pnlInput.Controls.Add(this.btnFilter);
            this.pnlInput.Controls.Add(this.dtpToDate);
            this.pnlInput.Controls.Add(this.lblToDate);
            this.pnlInput.Controls.Add(this.dtpFromDate);
            this.pnlInput.Controls.Add(this.lblFromDate);
            this.cmbStudents.Font = new Font("Cairo", 10F);
            this.pnlInput.Controls.Add(this.cmbStudents);
            this.pnlInput.Controls.Add(this.lblStudent);
            this.pnlInput.Location = new Point(20, 20);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new Size(780, 110);
            this.pnlInput.TabIndex = 0;

            //
            // lblStudent
            //
            this.lblStudent.AutoSize = true;
            this.lblStudent.Font = new Font("Cairo", 10F, FontStyle.Bold);
            this.lblStudent.ForeColor = Color.FromArgb(15, 23, 42);
            this.lblStudent.Location = new Point(680, 20);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new Size(82, 23);
            this.lblStudent.Text = "اختر الطالب:";

            //
            // cmbStudents
            //
            this.cmbStudents.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.cmbStudents.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.cmbStudents.DropDownStyle = ComboBoxStyle.DropDown;
            this.cmbStudents.FormattingEnabled = true;
            this.cmbStudents.Location = new Point(410, 17);
            this.cmbStudents.Name = "cmbStudents";
            this.cmbStudents.Size = new Size(260, 31);
            this.cmbStudents.TabIndex = 1;
            this.cmbStudents.SelectedIndexChanged += new EventHandler(this.CbStudents_SelectedIndexChanged);

            //
            // lblFromDate
            //
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Font = new Font("Cairo", 9.5F, FontStyle.Bold);
            this.lblFromDate.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblFromDate.Location = new Point(310, 20);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new Size(68, 21);
            this.lblFromDate.Text = "من تاريخ:";

            //
            // dtpFromDate
            //
            this.dtpFromDate.CustomFormat = "yyyy/MM/dd";
            this.dtpFromDate.Format = DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new Point(140, 16);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new Size(160, 30);
            this.dtpFromDate.TabIndex = 2;

            //
            // lblToDate
            //
            this.lblToDate.AutoSize = true;
            this.lblToDate.Font = new Font("Cairo", 9.5F, FontStyle.Bold);
            this.lblToDate.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblToDate.Location = new Point(310, 65);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new Size(72, 21);
            this.lblToDate.Text = "إلى تاريخ:";

            //
            // dtpToDate
            //
            this.dtpToDate.CustomFormat = "yyyy/MM/dd";
            this.dtpToDate.Format = DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new Point(140, 61);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new Size(160, 30);
            this.dtpToDate.TabIndex = 3;

            //
            // btnFilter
            //
            this.btnFilter.BackColor = Color.FromArgb(37, 99, 235); // SaaS Blue #2563EB
            this.btnFilter.Cursor = Cursors.Hand;
            this.btnFilter.FlatAppearance.BorderSize = 0;
            this.btnFilter.FlatStyle = FlatStyle.Flat;
            this.btnFilter.Font = new Font("Cairo", 10F, FontStyle.Bold);
            this.btnFilter.ForeColor = Color.White;
            this.btnFilter.Location = new Point(410, 61);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new Size(120, 32);
            this.btnFilter.TabIndex = 4;
            this.btnFilter.Text = "تصفية 🔍";
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += new EventHandler(this.BtnFilter_Click);

            //
            // btnPrintExport
            //
            this.btnPrintExport.BackColor = Color.FromArgb(16, 185, 129); // Modern Green #10B981
            this.btnPrintExport.Cursor = Cursors.Hand;
            this.btnPrintExport.FlatAppearance.BorderSize = 0;
            this.btnPrintExport.FlatStyle = FlatStyle.Flat;
            this.btnPrintExport.Font = new Font("Cairo", 10F, FontStyle.Bold);
            this.btnPrintExport.ForeColor = Color.White;
            this.btnPrintExport.Location = new Point(540, 61);
            this.btnPrintExport.Name = "btnPrintExport";
            this.btnPrintExport.Size = new Size(130, 32);
            this.btnPrintExport.TabIndex = 5;
            this.btnPrintExport.Text = "تصدير كشف حساب";
            this.btnPrintExport.UseVisualStyleBackColor = false;
            this.btnPrintExport.Click += new EventHandler(this.BtnPrintExport_Click);

            //
            // pnlGrid
            //
            this.pnlGrid.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            this.pnlGrid.Controls.Add(this.dgvStatement);
            this.pnlGrid.Location = new Point(20, 145);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new Size(780, 310);
            this.pnlGrid.TabIndex = 1;

            //
            // dgvStatement
            //
            this.dgvStatement.AllowUserToAddRows = false;
            this.dgvStatement.AllowUserToDeleteRows = false;
            this.dgvStatement.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252); // Zebra
            this.dgvStatement.BackgroundColor = Color.White;
            this.dgvStatement.BorderStyle = BorderStyle.None;
            this.dgvStatement.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 99, 235); // #2563EB
            this.dgvStatement.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvStatement.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvStatement.DefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
            this.dgvStatement.DefaultCellStyle.SelectionForeColor = Color.FromArgb(37, 99, 235);
            this.dgvStatement.EnableHeadersVisualStyles = false;
            this.dgvStatement.Dock = DockStyle.Fill;
            this.dgvStatement.Location = new Point(0, 0);
            this.dgvStatement.Name = "dgvStatement";
            this.dgvStatement.ReadOnly = true;
            this.dgvStatement.RowHeadersVisible = false;
            this.dgvStatement.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //
            // pnlFooter
            //
            this.pnlFooter.Anchor = ((AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left) | AnchorStyles.Right)));
            this.pnlFooter.BackColor = Color.Transparent;
            this.pnlFooter.Controls.Add(this.cardFinalBalance);
            this.pnlFooter.Controls.Add(this.cardTotalPaid);
            this.pnlFooter.Controls.Add(this.cardTotalCharged);
            this.pnlFooter.Location = new Point(20, 470);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new Size(780, 110);
            this.pnlFooter.TabIndex = 2;
            this.pnlFooter.Paint += new PaintEventHandler(this.PnlFooter_Paint);

            //
            // cardTotalCharged
            //
            this.cardTotalCharged.BackColor = Color.White;
            this.cardTotalCharged.BorderStyle = BorderStyle.FixedSingle;
            this.cardTotalCharged.Controls.Add(this.lblTotalChargedValue);
            this.cardTotalCharged.Controls.Add(this.lblTotalChargedTitle);
            this.cardTotalCharged.Location = new Point(530, 10);
            this.cardTotalCharged.Name = "cardTotalCharged";
            this.cardTotalCharged.Size = new Size(230, 90);
            this.cardTotalCharged.TabIndex = 0;

            //
            // lblTotalChargedTitle
            //
            this.lblTotalChargedTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTotalChargedTitle.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblTotalChargedTitle.Location = new Point(10, 10);
            this.lblTotalChargedTitle.Name = "lblTotalChargedTitle";
            this.lblTotalChargedTitle.Size = new Size(210, 25);
            this.lblTotalChargedTitle.Text = "إجمالي المطلوب (المستحق)";
            this.lblTotalChargedTitle.TextAlign = ContentAlignment.MiddleRight;

            //
            // lblTotalChargedValue
            //
            this.lblTotalChargedValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTotalChargedValue.ForeColor = Color.FromArgb(15, 23, 42);
            this.lblTotalChargedValue.Location = new Point(10, 45);
            this.lblTotalChargedValue.Name = "lblTotalChargedValue";
            this.lblTotalChargedValue.Size = new Size(210, 35);
            this.lblTotalChargedValue.Text = "0.00 د.ل";
            this.lblTotalChargedValue.TextAlign = ContentAlignment.MiddleRight;

            //
            // cardTotalPaid
            //
            this.cardTotalPaid.BackColor = Color.White;
            this.cardTotalPaid.BorderStyle = BorderStyle.FixedSingle;
            this.cardTotalPaid.Controls.Add(this.lblTotalPaidValue);
            this.cardTotalPaid.Controls.Add(this.lblTotalPaidTitle);
            this.cardTotalPaid.Location = new Point(280, 10);
            this.cardTotalPaid.Name = "cardTotalPaid";
            this.cardTotalPaid.Size = new Size(230, 90);
            this.cardTotalPaid.TabIndex = 1;

            //
            // lblTotalPaidTitle
            //
            this.lblTotalPaidTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTotalPaidTitle.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblTotalPaidTitle.Location = new Point(10, 10);
            this.lblTotalPaidTitle.Name = "lblTotalPaidTitle";
            this.lblTotalPaidTitle.Size = new Size(210, 25);
            this.lblTotalPaidTitle.Text = "إجمالي المدفوع";
            this.lblTotalPaidTitle.TextAlign = ContentAlignment.MiddleRight;

            //
            // lblTotalPaidValue
            //
            this.lblTotalPaidValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTotalPaidValue.ForeColor = Color.FromArgb(16, 185, 129); // Green #10B981
            this.lblTotalPaidValue.Location = new Point(10, 45);
            this.lblTotalPaidValue.Name = "lblTotalPaidValue";
            this.lblTotalPaidValue.Size = new Size(210, 35);
            this.lblTotalPaidValue.Text = "0.00 د.ل";
            this.lblTotalPaidValue.TextAlign = ContentAlignment.MiddleRight;

            //
            // cardFinalBalance
            //
            this.cardFinalBalance.BackColor = Color.White;
            this.cardFinalBalance.BorderStyle = BorderStyle.FixedSingle;
            this.cardFinalBalance.Controls.Add(this.lblFinalBalanceValue);
            this.cardFinalBalance.Controls.Add(this.lblFinalBalanceTitle);
            this.cardFinalBalance.Location = new Point(30, 10);
            this.cardFinalBalance.Name = "cardFinalBalance";
            this.cardFinalBalance.Size = new Size(230, 90);
            this.cardFinalBalance.TabIndex = 2;

            //
            // lblFinalBalanceTitle
            //
            this.lblFinalBalanceTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblFinalBalanceTitle.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblFinalBalanceTitle.Location = new Point(10, 10);
            this.lblFinalBalanceTitle.Name = "lblFinalBalanceTitle";
            this.lblFinalBalanceTitle.Size = new Size(210, 25);
            this.lblFinalBalanceTitle.Text = "الرصيد النهائي المتبقي";
            this.lblFinalBalanceTitle.TextAlign = ContentAlignment.MiddleRight;

            //
            // lblFinalBalanceValue
            //
            this.lblFinalBalanceValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblFinalBalanceValue.ForeColor = Color.FromArgb(239, 68, 68); // Red #EF4444
            this.lblFinalBalanceValue.Location = new Point(10, 45);
            this.lblFinalBalanceValue.Name = "lblFinalBalanceValue";
            this.lblFinalBalanceValue.Size = new Size(210, 35);
            this.lblFinalBalanceValue.Text = "0.00 د.ل";
            this.lblFinalBalanceValue.TextAlign = ContentAlignment.MiddleRight;

            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatement)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.cardTotalCharged.ResumeLayout(false);
            this.cardTotalPaid.ResumeLayout(false);
            this.cardFinalBalance.ResumeLayout(false);
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
                        SELECT StudentID, StudentName
                        FROM Students
                        ORDER BY StudentName ASC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            cmbStudents.DataSource = dt;
                            cmbStudents.DisplayMember = "StudentName";
                            cmbStudents.ValueMember = "StudentID";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadStudents failed in AccountStatementView: " + ex.Message);
            }
        }

        private void CbStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStatement();
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            LoadStatement();
        }

        public void LoadStatement()
        {
            if (cmbStudents.SelectedValue == null)
            {
                lblTotalChargedValue.Text = "0.00 د.ل";
                lblTotalPaidValue.Text = "0.00 د.ل";
                lblFinalBalanceValue.Text = "0.00 د.ل";
                return;
            }

            int studentID;
            if (cmbStudents.SelectedValue is DataRowView)
            {
                try
                {
                    studentID = Convert.ToInt32(((DataRowView)cmbStudents.SelectedValue)["StudentID"]);
                }
                catch
                {
                    return;
                }
            }
            else
            {
                if (!int.TryParse(cmbStudents.SelectedValue.ToString(), out studentID))
                {
                    return;
                }
            }

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Fetch all transactions for this student within date range, ordered by date then ID
                    string query = @"
                        SELECT
                            ft.TransactionDate,
                            ft.TransactionType,
                            ft.Notes,
                            ft.Debit,
                            ft.Credit,
                            u.Username
                        FROM FinancialTransactions ft
                        INNER JOIN Users u ON ft.UserID = u.UserID
                        WHERE ft.StudentID = @StudentID
                          AND ft.TransactionDate >= @FromDate
                          AND ft.TransactionDate <= @ToDate
                        ORDER BY ft.TransactionDate ASC, ft.TransactionID ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Ensure FromDate is at 00:00:00 and ToDate is at 23:59:59
                        DateTime fromDate = dtpFromDate.Value.Date;
                        DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1);

                        cmd.Parameters.AddWithValue("@StudentID", studentID);
                        cmd.Parameters.AddWithValue("@FromDate", fromDate);
                        cmd.Parameters.AddWithValue("@ToDate", toDate);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            // We need to construct a DataTable with a running balance column
                            DataTable displayTable = new DataTable();
                            displayTable.Columns.Add("التاريخ", typeof(string));
                            displayTable.Columns.Add("نوع العملية", typeof(string));
                            displayTable.Columns.Add("البيان / الملاحظات", typeof(string));
                            displayTable.Columns.Add("المطلوب (مدين)", typeof(string));
                            displayTable.Columns.Add("المدفوع (دائن)", typeof(string));
                            displayTable.Columns.Add("الرصيد التراكمي", typeof(string));
                            displayTable.Columns.Add("الموظف", typeof(string));

                            decimal runningBalance = 0;
                            decimal totalCharged = 0;
                            decimal totalPaid = 0;

                            foreach (DataRow row in dt.Rows)
                            {
                                DateTime date = Convert.ToDateTime(row["TransactionDate"]);
                                string type = row["TransactionType"].ToString();
                                string notes = row["Notes"] != DBNull.Value ? row["Notes"].ToString() : "";
                                decimal debit = Convert.ToDecimal(row["Debit"]);
                                decimal credit = Convert.ToDecimal(row["Credit"]);
                                string username = row["Username"] != DBNull.Value ? row["Username"].ToString() : "-";

                                // Map type to Arabic
                                string arabicType = type;
                                if (type == "Fee Charge") arabicType = "رسوم دورة";
                                else if (type == "Payment Receipt") arabicType = "سند قبض";
                                else if (type == "Opening Balance") arabicType = "رصيد سابق";

                                runningBalance += (debit - credit);
                                totalCharged += debit;
                                totalPaid += credit;

                                displayTable.Rows.Add(
                                    date.ToString("yyyy/MM/dd HH:mm"),
                                    arabicType,
                                    notes,
                                    debit.ToString("N2"),
                                    credit.ToString("N2"),
                                    runningBalance.ToString("N2"),
                                    username
                                );
                            }

                            dgvStatement.DataSource = displayTable;

                            // Right align the numerical columns
                            if (dgvStatement.Columns.Count >= 6)
                            {
                                dgvStatement.Columns["المطلوب (مدين)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                dgvStatement.Columns["المدفوع (دائن)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                dgvStatement.Columns["الرصيد التراكمي"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            }

                            dgvStatement.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                            // Set metrics at the bottom
                            lblTotalChargedValue.Text = totalCharged.ToString("N2") + " د.ل";
                            lblTotalPaidValue.Text = totalPaid.ToString("N2") + " د.ل";
                            lblFinalBalanceValue.Text = (totalCharged - totalPaid).ToString("N2") + " د.ل";

                            if ((totalCharged - totalPaid) > 0)
                            {
                                lblFinalBalanceValue.ForeColor = Color.FromArgb(239, 68, 68); // Red
                            }
                            else
                            {
                                lblFinalBalanceValue.ForeColor = Color.FromArgb(16, 185, 129); // Green
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadStatement failed: " + ex.Message);
            }
        }

        private void BtnPrintExport_Click(object sender, EventArgs e)
        {
            if (dgvStatement.Rows.Count == 0)
            {
                MessageBox.Show("لا توجد بيانات متاحة للتصدير.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "ملف اكسل مبسط (*.csv)|*.csv";
            sfd.FileName = string.Format("كشف_حساب_{0}_{1}.csv", cmbStudents.Text.Replace(" ", "_"), DateTime.Now.ToString("yyyy-MM-dd"));

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    // CSV columns header
                    string[] headers = new string[dgvStatement.Columns.Count];
                    for (int i = 0; i < dgvStatement.Columns.Count; i++)
                    {
                        headers[i] = EscapeCsvField(dgvStatement.Columns[i].HeaderText);
                    }
                    sb.AppendLine(string.Join(",", headers));

                    // Write rows
                    foreach (DataGridViewRow row in dgvStatement.Rows)
                    {
                        string[] fields = new string[dgvStatement.Columns.Count];
                        for (int i = 0; i < dgvStatement.Columns.Count; i++)
                        {
                            fields[i] = EscapeCsvField(row.Cells[i].Value != null ? row.Cells[i].Value.ToString() : "");
                        }
                        sb.AppendLine(string.Join(",", fields));
                    }

                    // Append totals summary at the bottom
                    sb.AppendLine();
                    sb.AppendLine(string.Format("{0},,,,,{1}", EscapeCsvField("إجمالي المطلوب"), EscapeCsvField(lblTotalChargedValue.Text)));
                    sb.AppendLine(string.Format("{0},,,,,{1}", EscapeCsvField("إجمالي المدفوع"), EscapeCsvField(lblTotalPaidValue.Text)));
                    sb.AppendLine(string.Format("{0},,,,,{1}", EscapeCsvField("الرصيد النهائي المتبقي"), EscapeCsvField(lblFinalBalanceValue.Text)));

                    // Write with UTF-8 BOM so Excel opens Arabic correctly
                    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, new UTF8Encoding(true)))
                    {
                        sw.Write(sb.ToString());
                    }

                    MessageBox.Show("تم تصدير كشف الحساب بنجاح إلى ملف Excel / CSV.", "تصدير موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ أثناء تصدير كشف الحساب: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void PnlFooter_Paint(object sender, PaintEventArgs e)
        {
            // Optional custom painting of subtle borders or backgrounds for footer
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
