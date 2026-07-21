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
        private Label lblNotes;
        private TextBox txtNotes;
        private Button btnSave;
        private Button btnPrint;
        private Button btnClear;

        private Panel pnlGrid;
        private DataGridView dgvPayments;

        private TableLayoutPanel tlpLayout;

        private int lastTransactionID = -1;
        private string printStudentName = "";
        private decimal printAmount = 0m;
        private DateTime printDate = DateTime.Now;
        private string printNotes = "";

        public event EventHandler DataSaved;

        public PaymentsView()
        {
            InitializeComponent();
            LoadStudentsList();
            LoadPayments();
        }

        private void InitializeComponent()
        {
            this.pnlInput = new Panel();
            this.tlpLayout = new TableLayoutPanel();

            this.lblStudent = new Label();
            this.cbStudent = new ComboBox();
            this.lblAmount = new Label();
            this.txtAmount = new TextBox();
            this.lblNotes = new Label();
            this.txtNotes = new TextBox();

            this.btnSave = new Button();
            this.btnPrint = new Button();
            this.btnClear = new Button();

            this.pnlGrid = new Panel();
            this.dgvPayments = new DataGridView();

            this.pnlInput.SuspendLayout();
            this.tlpLayout.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayments)).BeginInit();
            this.SuspendLayout();

            //
            // PaymentsView
            //
            this.BackColor = Color.FromArgb(248, 249, 250); // Off-White Background
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlInput);
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Segoe UI", 10F);
            this.Name = "PaymentsView";
            this.RightToLeft = RightToLeft.Yes;
            this.Size = new Size(820, 600);

            //
            // pnlInput
            //
            this.pnlInput.BackColor = Color.White;
            this.pnlInput.Controls.Add(this.tlpLayout);
            this.pnlInput.Dock = DockStyle.Top;
            this.pnlInput.Location = new Point(0, 0);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new Size(820, 185);
            this.pnlInput.TabIndex = 0;

            //
            // tlpLayout
            //
            this.tlpLayout.ColumnCount = 6;
            this.tlpLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            this.tlpLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            this.tlpLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            this.tlpLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            this.tlpLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            this.tlpLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));

            this.tlpLayout.RowCount = 3;
            this.tlpLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            this.tlpLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            this.tlpLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));

            this.tlpLayout.Controls.Add(this.lblStudent, 0, 0);
            this.tlpLayout.Controls.Add(this.cbStudent, 1, 0);
            this.tlpLayout.Controls.Add(this.lblAmount, 2, 0);
            this.tlpLayout.Controls.Add(this.txtAmount, 3, 0);
            this.tlpLayout.Controls.Add(this.lblNotes, 0, 1);
            this.tlpLayout.Controls.Add(this.txtNotes, 1, 1);

            // Span Notes text box over several columns
            this.tlpLayout.SetColumnSpan(this.txtNotes, 5);

            // Action buttons flow in the third row
            FlowLayoutPanel flpButtons = new FlowLayoutPanel();
            flpButtons.FlowDirection = FlowDirection.RightToLeft;
            flpButtons.Controls.Add(this.btnSave);
            flpButtons.Controls.Add(this.btnPrint);
            flpButtons.Controls.Add(this.btnClear);
            flpButtons.Dock = DockStyle.Fill;
            flpButtons.Margin = new Padding(0);

            this.tlpLayout.Controls.Add(flpButtons, 0, 2);
            this.tlpLayout.SetColumnSpan(flpButtons, 6);

            this.tlpLayout.Dock = DockStyle.Fill;
            this.tlpLayout.Location = new Point(0, 0);
            this.tlpLayout.Name = "tlpLayout";
            this.tlpLayout.Padding = new Padding(15);
            this.tlpLayout.TabIndex = 0;

            //
            // lblStudent
            //
            this.lblStudent.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.lblStudent.AutoSize = true;
            this.lblStudent.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblStudent.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblStudent.Location = new Point(715, 28);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new Size(88, 23);
            this.lblStudent.Text = "اختر الطالب:";
            this.lblStudent.TextAlign = ContentAlignment.MiddleLeft;

            //
            // cbStudent
            //
            this.cbStudent.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.cbStudent.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStudent.FormattingEnabled = true;
            this.cbStudent.Location = new Point(545, 24);
            this.cbStudent.Name = "cbStudent";
            this.cbStudent.Size = new Size(164, 31);
            this.cbStudent.TabIndex = 0;

            //
            // lblAmount
            //
            this.lblAmount.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.lblAmount.AutoSize = true;
            this.lblAmount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblAmount.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblAmount.Location = new Point(455, 28);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new Size(88, 23);
            this.lblAmount.Text = "مبلغ السداد:";
            this.lblAmount.TextAlign = ContentAlignment.MiddleLeft;

            //
            // txtAmount
            //
            this.txtAmount.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtAmount.Location = new Point(285, 25);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new Size(164, 30);
            this.txtAmount.TabIndex = 1;
            this.txtAmount.KeyDown += new KeyEventHandler(this.Input_KeyDown);

            //
            // lblNotes
            //
            this.lblNotes.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblNotes.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblNotes.Location = new Point(715, 78);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new Size(88, 23);
            this.lblNotes.Text = "ملاحظات:";
            this.lblNotes.TextAlign = ContentAlignment.MiddleLeft;

            //
            // txtNotes
            //
            this.txtNotes.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtNotes.Location = new Point(18, 75);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new Size(691, 30);
            this.txtNotes.TabIndex = 2;
            this.txtNotes.KeyDown += new KeyEventHandler(this.Input_KeyDown);

            //
            // btnSave
            //
            this.btnSave.BackColor = Color.FromArgb(46, 204, 113); // Soft Success Green
            this.btnSave.Cursor = Cursors.Hand;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(150, 38);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "حفظ وإيداع الخزينة";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new EventHandler(this.BtnSave_Click);

            //
            // btnPrint
            //
            this.btnPrint.BackColor = Color.FromArgb(52, 152, 219); // Accent Blue
            this.btnPrint.Cursor = Cursors.Hand;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatStyle = FlatStyle.Flat;
            this.btnPrint.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnPrint.ForeColor = Color.White;
            this.btnPrint.Location = new Point(159, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(150, 38);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "طباعة الإيصال 🖨";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new EventHandler(this.BtnPrint_Click);

            //
            // btnClear
            //
            this.btnClear.BackColor = Color.FromArgb(127, 140, 141); // Cool Gray
            this.btnClear.Cursor = Cursors.Hand;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnClear.ForeColor = Color.White;
            this.btnClear.Location = new Point(315, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(120, 38);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "مسح حقول";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new EventHandler(this.BtnClear_Click);

            //
            // pnlGrid
            //
            this.pnlGrid.Controls.Add(this.dgvPayments);
            this.pnlGrid.Dock = DockStyle.Fill;
            this.pnlGrid.Location = new Point(0, 185);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new Padding(15);
            this.pnlGrid.Size = new Size(820, 415);
            this.pnlGrid.TabIndex = 1;

            //
            // dgvPayments
            //
            this.dgvPayments.AllowUserToAddRows = false;
            this.dgvPayments.AllowUserToDeleteRows = false;
            this.dgvPayments.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 244, 244);
            this.dgvPayments.BackgroundColor = Color.White;
            this.dgvPayments.BorderStyle = BorderStyle.None;
            this.dgvPayments.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            this.dgvPayments.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvPayments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvPayments.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            this.dgvPayments.DefaultCellStyle.SelectionForeColor = Color.White;
            this.dgvPayments.EnableHeadersVisualStyles = false;
            this.dgvPayments.Dock = DockStyle.Fill;
            this.dgvPayments.Location = new Point(15, 15);
            this.dgvPayments.Name = "dgvPayments";
            this.dgvPayments.ReadOnly = true;
            this.dgvPayments.RowHeadersVisible = false;
            this.dgvPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.pnlInput.ResumeLayout(false);
            this.tlpLayout.ResumeLayout(false);
            this.tlpLayout.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayments)).EndInit();
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

        public void LoadStudentsList()
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
                System.Diagnostics.Debug.WriteLine("LoadStudentsList failed: " + ex.Message);
            }
        }

        public void LoadPayments()
        {
            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT
                            ft.TransactionID AS [رقم المعاملة],
                            s.StudentName AS [اسم الطالب],
                            ft.Credit AS [المبلغ المدفوع (د.ل)],
                            ft.TransactionDate AS [تاريخ السداد],
                            ft.Notes AS [ملاحظات]
                        FROM FinancialTransactions ft
                        INNER JOIN Students s ON ft.StudentID = s.StudentID
                        WHERE ft.TransactionType = 'Payment Receipt'
                        ORDER BY ft.TransactionID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvPayments.DataSource = dt;
                            dgvPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadPayments failed: " + ex.Message);
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
                int currentUserID = DbConnectionManager.GetDefaultUserID();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. Insert into FinancialTransactions (Debit = 0, Credit = amount, TransactionType = 'Payment Receipt')
                            string queryTx = @"
                                INSERT INTO FinancialTransactions (StudentID, TransactionType, Debit, Credit, TransactionDate, Notes, UserID)
                                VALUES (@StudentID, 'Payment Receipt', 0.00, @Amount, @TransactionDate, @Notes, @UserID);
                                SELECT SCOPE_IDENTITY();";

                            int newTxID = -1;
                            using (SqlCommand cmdTx = new SqlCommand(queryTx, conn, trans))
                            {
                                cmdTx.Parameters.AddWithValue("@StudentID", cbStudent.SelectedValue);
                                cmdTx.Parameters.AddWithValue("@Amount", amount);
                                cmdTx.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                                cmdTx.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(txtNotes.Text) ? "سداد رسوم" : txtNotes.Text.Trim());
                                cmdTx.Parameters.AddWithValue("@UserID", currentUserID);

                                object scalarResult = cmdTx.ExecuteScalar();
                                if (scalarResult != null && scalarResult != DBNull.Value)
                                {
                                    newTxID = Convert.ToInt32(scalarResult);
                                }
                            }

                            // 2. Fetch current Treasury balance to compute the new running balance
                            decimal currentBalance = 0m;
                            string queryBalance = "SELECT TOP 1 CurrentBalance FROM TreasuryLog ORDER BY LogID DESC";
                            using (SqlCommand cmdBalance = new SqlCommand(queryBalance, conn, trans))
                            {
                                object balResult = cmdBalance.ExecuteScalar();
                                if (balResult != null && balResult != DBNull.Value)
                                {
                                    currentBalance = Convert.ToDecimal(balResult);
                                }
                            }

                            decimal newRunningBalance = currentBalance + amount;

                            // 3. Insert into TreasuryLog (ActionType = 'Deposit')
                            string queryLog = @"
                                INSERT INTO TreasuryLog (TransactionID, Amount, ActionType, CurrentBalance, LogDate, Notes)
                                VALUES (@TransactionID, @Amount, 'Deposit', @CurrentBalance, @LogDate, @Notes)";
                            using (SqlCommand cmdLog = new SqlCommand(queryLog, conn, trans))
                            {
                                cmdLog.Parameters.AddWithValue("@TransactionID", newTxID);
                                cmdLog.Parameters.AddWithValue("@Amount", amount);
                                cmdLog.Parameters.AddWithValue("@CurrentBalance", newRunningBalance);
                                cmdLog.Parameters.AddWithValue("@LogDate", DateTime.Now);
                                cmdLog.Parameters.AddWithValue("@Notes", "إيداع دفعة سداد الطالب: " + cbStudent.Text);
                                cmdLog.ExecuteNonQuery();
                            }

                            trans.Commit();
                            lastTransactionID = newTxID;
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }

                MessageBox.Show("تم حفظ دفعة السداد وإيداعها بالخزينة بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAmount.Clear();
                txtNotes.Clear();
                LoadPayments();
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

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT
                            s.StudentName,
                            ft.Credit,
                            ft.TransactionDate,
                            ft.Notes
                        FROM FinancialTransactions ft
                        INNER JOIN Students s ON ft.StudentID = s.StudentID
                        WHERE ft.TransactionID = @ID";

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
            int startX = e.PageBounds.Width - 50;
            int startY = 50;

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

            g.DrawString("منظومة مركز الدورات التعليمية", fontHeader, brushPrimary, e.PageBounds.Width / 2, startY, sfCenter);
            startY += 40;
            g.DrawString("إيصال سداد رسوم دراسية (إيصال قبض)", fontSub, brushMuted, e.PageBounds.Width / 2, startY, sfCenter);
            startY += 30;

            g.DrawLine(penGray, 50, startY, e.PageBounds.Width - 50, startY);
            startY += 25;

            g.DrawString("رقم الإيصال: " + lastTransactionID, fontBodyBold, brushText, startX, startY, sfRight);
            startY += 25;
            g.DrawString("تاريخ السداد: " + printDate.ToString("dd/MM/yyyy HH:mm"), fontBody, brushText, startX, startY, sfRight);
            startY += 35;

            g.DrawLine(penGray, 50, startY, e.PageBounds.Width - 50, startY);
            startY += 25;

            g.DrawString("استلمنا من الطالب/الطالبة: " + printStudentName, fontBodyBold, brushText, startX, startY, sfRight);
            startY += 30;
            g.DrawString("مبلغاً وقدره: " + printAmount.ToString("N2") + " د.ل (دينار ليبي)", fontBodyBold, brushText, startX, startY, sfRight);
            startY += 30;
            g.DrawString("ملاحظات: " + (string.IsNullOrEmpty(printNotes) ? "لا يوجد" : printNotes), fontBody, brushText, startX, startY, sfRight);
            startY += 45;

            Rectangle totalRect = new Rectangle(50, startY, e.PageBounds.Width - 100, 50);
            g.DrawRectangle(penGray, totalRect);
            g.DrawString("صافي القيمة المقبوضة: " + printAmount.ToString("N2") + " د.ل", fontBodyBold, brushPrimary, e.PageBounds.Width / 2, startY + 12, sfCenter);
            startY += 100;

            g.DrawString("توقيع المحاسب / الإدارة: .............................", fontBodyBold, brushText, startX, startY, sfRight);
            startY += 60;

            g.DrawLine(penGray, 50, startY, e.PageBounds.Width - 50, startY);
            startY += 10;
            g.DrawString("شكراً لتعاملكم معنا - منظومة الإدارة الذكية لمركز الدورات", fontFooter, brushMuted, e.PageBounds.Width / 2, startY, sfCenter);

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

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (cbStudent.Items.Count > 0) cbStudent.SelectedIndex = 0;
            txtAmount.Clear();
            txtNotes.Clear();
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