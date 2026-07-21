using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Printing;

namespace SchoolCenter
{
    public class PaymentsView : UserControl
    {
        private Panel pnlInput;
        private Label lblStudent;
        private ComboBox cbStudent;
        private Label lblAmount;
        private TextBox txtAmount;
        private Label lblDate;
        private DateTimePicker dtpDate;
        private Label lblNotes;
        private TextBox txtNotes;
        private Button btnSave;
        private Button btnPrint;

        private Panel pnlSearch;
        private Label lblSearch;
        private TextBox txtSearch;

        private Panel pnlGrid;
        private DataGridView dgvFinancials;

        private int lastTransactionID = -1;
        private string printStudentName = "";
        private decimal printAmount = 0m;
        private DateTime printDate = DateTime.Now;
        private string printNotes = "";

        public event EventHandler DataSaved;

        public PaymentsView()
        {
            InitializeComponent();
            ThemeHelper.ApplyTheme(this);
            LoadStudentsList();
            LoadTransactions();
        }

        private void InitializeComponent()
        {
            this.pnlInput = new Panel();
            this.lblStudent = new Label();
            this.cbStudent = new ComboBox();
            this.lblAmount = new Label();
            this.txtAmount = new TextBox();
            this.lblDate = new Label();
            this.dtpDate = new DateTimePicker();
            this.lblNotes = new Label();
            this.txtNotes = new TextBox();
            this.btnSave = new Button();
            this.btnPrint = new Button();

            this.pnlSearch = new Panel();
            this.lblSearch = new Label();
            this.txtSearch = new TextBox();

            this.pnlGrid = new Panel();
            this.dgvFinancials = new DataGridView();

            this.pnlInput.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinancials)).BeginInit();
            this.SuspendLayout();

            //
            // PaymentsView
            //
            this.BackColor = Color.FromArgb(248, 249, 250); // Off-White
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlInput);
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 10F);
            this.Name = "PaymentsView";
            this.RightToLeft = RightToLeft.Yes;
            this.Size = new Size(820, 600);

            //
            // pnlInput
            //
            this.pnlInput.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.pnlInput.BackColor = Color.White;
            this.pnlInput.Controls.Add(this.btnPrint);
            this.pnlInput.Controls.Add(this.btnSave);
            this.pnlInput.Controls.Add(this.txtNotes);
            this.pnlInput.Controls.Add(this.lblNotes);
            this.pnlInput.Controls.Add(this.dtpDate);
            this.pnlInput.Controls.Add(this.lblDate);
            this.pnlInput.Controls.Add(this.txtAmount);
            this.pnlInput.Controls.Add(this.lblAmount);
            this.pnlInput.Controls.Add(this.cbStudent);
            this.pnlInput.Controls.Add(this.lblStudent);
            this.pnlInput.Location = new Point(20, 20);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new Size(780, 160);
            this.pnlInput.TabIndex = 0;

            //
            // lblStudent
            //
            this.lblStudent.AutoSize = true;
            this.lblStudent.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblStudent.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblStudent.Location = new Point(680, 20);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new Size(82, 23);
            this.lblStudent.Text = "اختر الطالب:";

            //
            // cbStudent
            //
            this.cbStudent.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStudent.FormattingEnabled = true;
            this.cbStudent.Location = new Point(410, 17);
            this.cbStudent.Name = "cbStudent";
            this.cbStudent.Size = new Size(260, 31);

            //
            // lblAmount
            //
            this.lblAmount.AutoSize = true;
            this.lblAmount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblAmount.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblAmount.Location = new Point(310, 20);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new Size(94, 23);
            this.lblAmount.Text = "قيمة السداد:";

            //
            // txtAmount
            //
            this.txtAmount.Location = new Point(110, 17);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new Size(190, 30);

            //
            // lblDate
            //
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblDate.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblDate.Location = new Point(680, 65);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new Size(93, 23);
            this.lblDate.Text = "تاريخ السداد:";

            //
            // dtpDate
            //
            this.dtpDate.CustomFormat = "dd/MM/yyyy";
            this.dtpDate.Format = DateTimePickerFormat.Custom;
            this.dtpDate.Location = new Point(410, 62);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new Size(260, 30);

            //
            // lblNotes
            //
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblNotes.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblNotes.Location = new Point(310, 65);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new Size(76, 23);
            this.lblNotes.Text = "ملاحظات:";

            //
            // txtNotes
            //
            this.txtNotes.Location = new Point(110, 62);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new Size(190, 30);

            //
            // btnSave
            //
            this.btnSave.BackColor = Color.FromArgb(46, 204, 113); // Soft Green for success/confirm
            this.btnSave.Cursor = Cursors.Hand;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(410, 110);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(140, 35);
            this.btnSave.Text = "تسجيل الإيصال";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new EventHandler(this.BtnSave_Click);

            //
            // btnPrint
            //
            this.btnPrint.BackColor = Color.FromArgb(52, 152, 219); // Action Blue
            this.btnPrint.Cursor = Cursors.Hand;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatStyle = FlatStyle.Flat;
            this.btnPrint.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnPrint.ForeColor = Color.White;
            this.btnPrint.Location = new Point(250, 110);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(140, 35);
            this.btnPrint.Text = "طباعة آخر إيصال 🖨";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new EventHandler(this.BtnPrint_Click);

            //
            // pnlSearch
            //
            this.pnlSearch.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.pnlSearch.BackColor = Color.White;
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Location = new Point(20, 195);
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
            this.lblSearch.Text = "بحث سريع باسم الطالب أو البيان:";

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
            this.pnlGrid.Controls.Add(this.dgvFinancials);
            this.pnlGrid.Location = new Point(20, 260);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new Size(780, 320);
            this.pnlGrid.TabIndex = 2;

            //
            // dgvFinancials
            //
            this.dgvFinancials.AllowUserToAddRows = false;
            this.dgvFinancials.AllowUserToDeleteRows = false;
            this.dgvFinancials.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 244, 244);
            this.dgvFinancials.BackgroundColor = Color.White;
            this.dgvFinancials.BorderStyle = BorderStyle.None;
            this.dgvFinancials.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            this.dgvFinancials.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvFinancials.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvFinancials.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            this.dgvFinancials.DefaultCellStyle.SelectionForeColor = Color.White;
            this.dgvFinancials.EnableHeadersVisualStyles = false;
            this.dgvFinancials.Dock = DockStyle.Fill;
            this.dgvFinancials.Location = new Point(0, 0);
            this.dgvFinancials.Name = "dgvFinancials";
            this.dgvFinancials.ReadOnly = true;
            this.dgvFinancials.RowHeadersVisible = false;
            this.dgvFinancials.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinancials)).EndInit();
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
                LoadTransactions();
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
                            t.TransactionID AS [رقم الحركة],
                            s.StudentName AS [اسم الطالب],
                            t.Credit AS [قيمة السداد],
                            t.TransactionDate AS [تاريخ السداد],
                            t.Notes AS [ملاحظات]
                        FROM FinancialTransactions t
                        INNER JOIN Students s ON t.StudentID = s.StudentID
                        WHERE t.TransactionType = 'Payment Receipt'
                          AND (s.StudentName LIKE @Filter OR t.Notes LIKE @Filter)
                        ORDER BY t.TransactionID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Filter", "%" + filterText + "%");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvFinancials.DataSource = dt;
                            dgvFinancials.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Search failed: " + ex.Message);
            }
        }

        public void LoadStudentsList()
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
                            cbStudent.DataSource = dt;
                            cbStudent.DisplayMember = "StudentName";
                            cbStudent.ValueMember = "StudentID";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadStudentsList failed: " + ex.Message);
            }
        }

        public void LoadTransactions()
        {
            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT
                            t.TransactionID AS [رقم الحركة],
                            s.StudentName AS [اسم الطالب],
                            t.Credit AS [قيمة السداد],
                            t.TransactionDate AS [تاريخ السداد],
                            t.Notes AS [ملاحظات]
                        FROM FinancialTransactions t
                        INNER JOIN Students s ON t.StudentID = s.StudentID
                        WHERE t.TransactionType = 'Payment Receipt'
                        ORDER BY t.TransactionID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvFinancials.DataSource = dt;
                            dgvFinancials.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadTransactions failed: " + ex.Message);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cbStudent.SelectedValue == null)
            {
                MessageBox.Show("يرجى اختيار الطالب أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal amount;
            if (!decimal.TryParse(txtAmount.Text.Trim(), out amount) || amount <= 0)
            {
                MessageBox.Show("يرجى إدخال مبلغ صحيح أكبر من الصفر.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
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
                            // 1. Insert into FinancialTransactions
                            // TransactionType = 'Payment Receipt'
                            // Debit = 0.00, Credit = Paid Amount
                            // UserID = 1 (default admin)
                            string queryTx = @"
                                INSERT INTO FinancialTransactions (StudentID, TransactionType, Debit, Credit, TransactionDate, Notes, UserID)
                                VALUES (@StudentID, 'Payment Receipt', 0.00, @Credit, @TransactionDate, @Notes, @UserID);
                                SELECT SCOPE_IDENTITY();";

                            int newTxID = -1;
                            using (SqlCommand cmd = new SqlCommand(queryTx, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@StudentID", cbStudent.SelectedValue);
                                cmd.Parameters.AddWithValue("@Credit", amount);
                                cmd.Parameters.AddWithValue("@TransactionDate", dtpDate.Value);
                                cmd.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(txtNotes.Text) ? "إيصال سداد رسوم" : txtNotes.Text.Trim());
                                cmd.Parameters.AddWithValue("@UserID", 1); // seeded admin

                                object scalarResult = cmd.ExecuteScalar();
                                if (scalarResult != null && scalarResult != DBNull.Value)
                                {
                                    newTxID = Convert.ToInt32(scalarResult);
                                }
                            }

                            if (newTxID != -1)
                            {
                                // Calculate current treasury balance to append to TreasuryLog
                                string queryBalance = @"
                                    SELECT ISNULL((SELECT SUM(Amount) FROM TreasuryLog WHERE ActionType = 'Deposit'), 0) -
                                           ISNULL((SELECT SUM(Amount) FROM TreasuryLog WHERE ActionType = 'Withdrawal'), 0)";
                                decimal currentBal = 0m;
                                using (SqlCommand cmdBal = new SqlCommand(queryBalance, conn, trans))
                                {
                                    object res = cmdBal.ExecuteScalar();
                                    if (res != null && res != DBNull.Value)
                                    {
                                        currentBal = Convert.ToDecimal(res);
                                    }
                                }

                                decimal newBalance = currentBal + amount;

                                // 2. Append deposit entry into TreasuryLog
                                string queryLog = @"
                                    INSERT INTO TreasuryLog (TransactionID, Amount, ActionType, CurrentBalance, LogDate, Notes)
                                    VALUES (@TransactionID, @Amount, 'Deposit', @CurrentBalance, @LogDate, @Notes)";
                                using (SqlCommand cmdLog = new SqlCommand(queryLog, conn, trans))
                                {
                                    cmdLog.Parameters.AddWithValue("@TransactionID", newTxID);
                                    cmdLog.Parameters.AddWithValue("@Amount", amount);
                                    cmdLog.Parameters.AddWithValue("@CurrentBalance", newBalance);
                                    cmdLog.Parameters.AddWithValue("@LogDate", dtpDate.Value);
                                    cmdLog.Parameters.AddWithValue("@Notes", "إيداع تلقائي لقيمة إيصال رقم " + newTxID);
                                    cmdLog.ExecuteNonQuery();
                                }

                                lastTransactionID = newTxID;
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

                MessageBox.Show("تم تسجيل الإيصال المالي وتحديث الخزينة بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAmount.Clear();
                txtNotes.Clear();
                LoadTransactions();
                OnDataSaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تسجيل الإيصال: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (lastTransactionID == -1)
            {
                MessageBox.Show("لا يوجد إيصال مسجل حديثاً لطباعته.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // جلب تفاصيل الإيصال الأخير للطباعة
            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT
                            s.StudentName,
                            t.Credit,
                            t.TransactionDate,
                            t.Notes
                        FROM FinancialTransactions t
                        INNER JOIN Students s ON t.StudentID = s.StudentID
                        WHERE t.TransactionID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", lastTransactionID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                printStudentName = reader["StudentName"].ToString();
                                printAmount = Convert.ToDecimal(reader["Credit"]);
                                printDate = Convert.ToDateTime(reader["TransactionDate"]);
                                printNotes = reader["Notes"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("تعذر العثور على بيانات الإيصال.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }

                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(this.PrintReceiptPage);
                PrintPreviewDialog ppd = new PrintPreviewDialog();
                ppd.Document = pd;
                ppd.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء محاولة الطباعة: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintReceiptPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int startX = e.PageBounds.Width - 50; // هامش أيمن
            int startY = 50;

            // خطوط الفرشاة والخطوط
            Font fontHeader = new Font("Segoe UI", 16, FontStyle.Bold);
            Font fontSub = new Font("Segoe UI", 10, FontStyle.Regular);
            Font fontBodyBold = new Font("Segoe UI", 11, FontStyle.Bold);
            Font fontBody = new Font("Segoe UI", 11, FontStyle.Regular);
            Font fontFooter = new Font("Segoe UI", 8, FontStyle.Italic);

            SolidBrush brushPrimary = new SolidBrush(Color.FromArgb(44, 62, 80));
            SolidBrush brushText = new SolidBrush(Color.FromArgb(44, 62, 80));
            SolidBrush brushMuted = new SolidBrush(Color.FromArgb(127, 140, 141));
            Pen penGray = new Pen(Color.FromArgb(127, 140, 141), 1);

            StringFormat sfRight = new StringFormat(StringFormatFlags.DirectionRightToLeft);
            sfRight.Alignment = StringAlignment.Near;

            StringFormat sfCenter = new StringFormat(StringFormatFlags.DirectionRightToLeft);
            sfCenter.Alignment = StringAlignment.Center;

            // 1. ترويسة الإيصال
            g.DrawString("منظومة مركز الدورات التعليمية", fontHeader, brushPrimary, e.PageBounds.Width / 2, startY, sfCenter);
            startY += 40;
            g.DrawString("إيصال سداد رسوم دراسية (إيصال قبض)", fontSub, brushMuted, e.PageBounds.Width / 2, startY, sfCenter);
            startY += 30;

            // رسم خط فاصل رمادي أنيق
            g.DrawLine(penGray, 50, startY, e.PageBounds.Width - 50, startY);
            startY += 25;

            // 2. معلومات الفاتورة/الإيصال الأساسية
            g.DrawString("رقم الإيصال: " + lastTransactionID, fontBodyBold, brushText, startX, startY, sfRight);
            startY += 25;
            g.DrawString("تاريخ السداد: " + printDate.ToString("dd/MM/yyyy HH:mm"), fontBody, brushText, startX, startY, sfRight);
            startY += 35;

            // رسم خط فاصل آخر
            g.DrawLine(penGray, 50, startY, e.PageBounds.Width - 50, startY);
            startY += 25;

            // 3. محتوى الإيصال والتفاصيل
            g.DrawString("استلمنا من الطالب/الطالبة: " + printStudentName, fontBodyBold, brushText, startX, startY, sfRight);
            startY += 30;
            g.DrawString("مبلغاً وقدره: " + printAmount.ToString("N2") + " د.ل (دينار ليبي)", fontBodyBold, brushText, startX, startY, sfRight);
            startY += 30;
            g.DrawString("ملاحظات: " + (string.IsNullOrEmpty(printNotes) ? "لا يوجد" : printNotes), fontBody, brushText, startX, startY, sfRight);
            startY += 45;

            // 4. إطار لتأكيد الدفع المالي الإجمالي
            Rectangle totalRect = new Rectangle(50, startY, e.PageBounds.Width - 100, 50);
            g.DrawRectangle(penGray, totalRect);
            g.DrawString("صافي القيمة المقبوضة: " + printAmount.ToString("N2") + " د.ل", fontBodyBold, brushPrimary, e.PageBounds.Width / 2, startY + 12, sfCenter);
            startY += 100;

            // 5. التوقيع والتحية الختامية
            g.DrawString("توقيع المحاسب / الإدارة: .............................", fontBodyBold, brushText, startX, startY, sfRight);
            startY += 60;

            g.DrawLine(penGray, 50, startY, e.PageBounds.Width - 50, startY);
            startY += 10;
            g.DrawString("شكراً لتعاملكم معنا - منظومة الإدارة الذكية لمركز الدورات", fontFooter, brushMuted, e.PageBounds.Width / 2, startY, sfCenter);

            // تخلص من الكائنات لتفادي تسريب الذاكرة
            fontHeader.Dispose();
            fontSub.Dispose();
            fontBodyBold.Dispose();
            fontBody.Dispose();
            fontFooter.Dispose();
            brushPrimary.Dispose();
            brushText.Dispose();
            brushMuted.Dispose();
            penGray.Dispose();
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