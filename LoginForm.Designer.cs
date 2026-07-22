namespace SchoolCenter
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpSplit = new System.Windows.Forms.TableLayoutPanel();
            this.pnlBrand = new System.Windows.Forms.Panel();
            this.pnlBrandCenter = new System.Windows.Forms.Panel();
            this.lblBrandIcon = new System.Windows.Forms.Label();
            this.lblBrandTitle = new System.Windows.Forms.Label();
            this.lblBrandSubtitle = new System.Windows.Forms.Label();
            this.pnlLoginContainer = new System.Windows.Forms.Panel();
            this.pnlLoginCard = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.pnlPasswordWrapper = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnTogglePassword = new System.Windows.Forms.Button();
            this.pnlOptions = new System.Windows.Forms.Panel();
            this.chkRememberMe = new System.Windows.Forms.CheckBox();
            this.lnkForgotPassword = new System.Windows.Forms.LinkLabel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.tlpSplit.SuspendLayout();
            this.pnlBrand.SuspendLayout();
            this.pnlBrandCenter.SuspendLayout();
            this.pnlLoginContainer.SuspendLayout();
            this.pnlLoginCard.SuspendLayout();
            this.pnlPasswordWrapper.SuspendLayout();
            this.pnlOptions.SuspendLayout();
            this.SuspendLayout();
            //
            // tlpSplit
            //
            this.tlpSplit.ColumnCount = 2;
            this.tlpSplit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSplit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSplit.Controls.Add(this.pnlBrand, 0, 0);
            this.tlpSplit.Controls.Add(this.pnlLoginContainer, 1, 0);
            this.tlpSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSplit.Location = new System.Drawing.Point(0, 0);
            this.tlpSplit.Name = "tlpSplit";
            this.tlpSplit.RowCount = 1;
            this.tlpSplit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSplit.Size = new System.Drawing.Size(950, 600);
            this.tlpSplit.TabIndex = 0;
            //
            // pnlBrand
            //
            this.pnlBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252))))); // Subtle off-white #F8FAFC
            this.pnlBrand.Controls.Add(this.pnlBrandCenter);
            this.pnlBrand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBrand.Location = new System.Drawing.Point(478, 3);
            this.pnlBrand.Name = "pnlBrand";
            this.pnlBrand.Size = new System.Drawing.Size(469, 594);
            this.pnlBrand.TabIndex = 0;
            //
            // pnlBrandCenter
            //
            this.pnlBrandCenter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlBrandCenter.Controls.Add(this.lblBrandIcon);
            this.pnlBrandCenter.Controls.Add(this.lblBrandTitle);
            this.pnlBrandCenter.Controls.Add(this.lblBrandSubtitle);
            this.pnlBrandCenter.Location = new System.Drawing.Point(34, 122);
            this.pnlBrandCenter.Name = "pnlBrandCenter";
            this.pnlBrandCenter.Size = new System.Drawing.Size(400, 350);
            this.pnlBrandCenter.TabIndex = 0;
            //
            // lblBrandIcon
            //
            this.lblBrandIcon.Font = new System.Drawing.Font("Segoe UI", 64F);
            this.lblBrandIcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblBrandIcon.Location = new System.Drawing.Point(0, 10);
            this.lblBrandIcon.Name = "lblBrandIcon";
            this.lblBrandIcon.Size = new System.Drawing.Size(400, 120);
            this.lblBrandIcon.TabIndex = 0;
            this.lblBrandIcon.Text = "🏫";
            this.lblBrandIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblBrandTitle
            //
            this.lblBrandTitle.Font = new System.Drawing.Font("Cairo", 18F, System.Drawing.FontStyle.Bold);
            this.lblBrandTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblBrandTitle.Location = new System.Drawing.Point(0, 145);
            this.lblBrandTitle.Name = "lblBrandTitle";
            this.lblBrandTitle.Size = new System.Drawing.Size(400, 50);
            this.lblBrandTitle.TabIndex = 1;
            this.lblBrandTitle.Text = "منظومة مركز الدورات التعليمية";
            this.lblBrandTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblBrandSubtitle
            //
            this.lblBrandSubtitle.Font = new System.Drawing.Font("Cairo", 10.5F);
            this.lblBrandSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblBrandSubtitle.Location = new System.Drawing.Point(0, 205);
            this.lblBrandSubtitle.Name = "lblBrandSubtitle";
            this.lblBrandSubtitle.Size = new System.Drawing.Size(400, 60);
            this.lblBrandSubtitle.TabIndex = 2;
            this.lblBrandSubtitle.Text = "النظام الموحد لإدارة الموارد المالية والتعليمية";
            this.lblBrandSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // pnlLoginContainer
            //
            this.pnlLoginContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42))))); // Dark Slate Navy #0F172A
            this.pnlLoginContainer.Controls.Add(this.pnlLoginCard);
            this.pnlLoginContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLoginContainer.Location = new System.Drawing.Point(3, 3);
            this.pnlLoginContainer.Name = "pnlLoginContainer";
            this.pnlLoginContainer.Size = new System.Drawing.Size(469, 594);
            this.pnlLoginContainer.TabIndex = 1;
            //
            // pnlLoginCard
            //
            this.pnlLoginCard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlLoginCard.BackColor = System.Drawing.Color.White;
            this.pnlLoginCard.Controls.Add(this.lblError);
            this.pnlLoginCard.Controls.Add(this.btnExit);
            this.pnlLoginCard.Controls.Add(this.btnLogin);
            this.pnlLoginCard.Controls.Add(this.pnlOptions);
            this.pnlLoginCard.Controls.Add(this.pnlPasswordWrapper);
            this.pnlLoginCard.Controls.Add(this.lblPassword);
            this.pnlLoginCard.Controls.Add(this.txtUsername);
            this.pnlLoginCard.Controls.Add(this.lblUsername);
            this.pnlLoginCard.Controls.Add(this.lblSubtitle);
            this.pnlLoginCard.Controls.Add(this.lblTitle);
            this.pnlLoginCard.Location = new System.Drawing.Point(44, 47);
            this.pnlLoginCard.Name = "pnlLoginCard";
            this.pnlLoginCard.Size = new System.Drawing.Size(380, 500);
            this.pnlLoginCard.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.Font = new System.Drawing.Font("Cairo", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTitle.Location = new System.Drawing.Point(15, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(350, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "تسجيل الدخول";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblSubtitle
            //
            this.lblSubtitle.Font = new System.Drawing.Font("Cairo", 9F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSubtitle.Location = new System.Drawing.Point(15, 65);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(350, 25);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "أدخل بيانات الاعتماد للوصول إلى المنظومة";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblUsername
            //
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Cairo", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblUsername.Location = new System.Drawing.Point(250, 105);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(113, 24);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "👤 اسم المستخدم:";
            //
            // txtUsername
            //
            this.txtUsername.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsername.Font = new System.Drawing.Font("Cairo", 10.5F);
            this.txtUsername.Location = new System.Drawing.Point(20, 132);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(340, 34);
            this.txtUsername.TabIndex = 0;
            //
            // lblPassword
            //
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Cairo", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblPassword.Location = new System.Drawing.Point(265, 182);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(98, 24);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "🔑 كلمة المرور:";
            //
            // pnlPasswordWrapper
            //
            this.pnlPasswordWrapper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlPasswordWrapper.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPasswordWrapper.Controls.Add(this.txtPassword);
            this.pnlPasswordWrapper.Controls.Add(this.btnTogglePassword);
            this.pnlPasswordWrapper.Location = new System.Drawing.Point(20, 210);
            this.pnlPasswordWrapper.Name = "pnlPasswordWrapper";
            this.pnlPasswordWrapper.Size = new System.Drawing.Size(340, 36);
            this.pnlPasswordWrapper.TabIndex = 1;
            //
            // txtPassword
            //
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Font = new System.Drawing.Font("Cairo", 10.5F);
            this.txtPassword.Location = new System.Drawing.Point(40, 4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(295, 27);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtPassword_KeyDown);
            //
            // btnTogglePassword
            //
            this.btnTogglePassword.BackColor = System.Drawing.Color.Transparent;
            this.btnTogglePassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTogglePassword.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTogglePassword.FlatAppearance.BorderSize = 0;
            this.btnTogglePassword.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTogglePassword.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTogglePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTogglePassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnTogglePassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnTogglePassword.Location = new System.Drawing.Point(0, 0);
            this.btnTogglePassword.Name = "btnTogglePassword";
            this.btnTogglePassword.Size = new System.Drawing.Size(35, 34);
            this.btnTogglePassword.TabIndex = 0;
            this.btnTogglePassword.TabStop = false;
            this.btnTogglePassword.Text = "👁️";
            this.btnTogglePassword.UseVisualStyleBackColor = false;
            this.btnTogglePassword.Click += new System.EventHandler(this.BtnTogglePassword_Click);
            //
            // pnlOptions
            //
            this.pnlOptions.Controls.Add(this.chkRememberMe);
            this.pnlOptions.Controls.Add(this.lnkForgotPassword);
            this.pnlOptions.Location = new System.Drawing.Point(20, 255);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(340, 30);
            this.pnlOptions.TabIndex = 4;
            //
            // chkRememberMe
            //
            this.chkRememberMe.AutoSize = true;
            this.chkRememberMe.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkRememberMe.Font = new System.Drawing.Font("Cairo", 9F);
            this.chkRememberMe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.chkRememberMe.Location = new System.Drawing.Point(258, 0);
            this.chkRememberMe.Name = "chkRememberMe";
            this.chkRememberMe.Size = new System.Drawing.Size(82, 30);
            this.chkRememberMe.TabIndex = 0;
            this.chkRememberMe.Text = "تذكرني";
            this.chkRememberMe.UseVisualStyleBackColor = true;
            //
            // lnkForgotPassword
            //
            this.lnkForgotPassword.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lnkForgotPassword.AutoSize = true;
            this.lnkForgotPassword.Dock = System.Windows.Forms.DockStyle.Left;
            this.lnkForgotPassword.Font = new System.Drawing.Font("Cairo", 9F);
            this.lnkForgotPassword.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkForgotPassword.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.lnkForgotPassword.Location = new System.Drawing.Point(0, 0);
            this.lnkForgotPassword.Name = "lnkForgotPassword";
            this.lnkForgotPassword.Size = new System.Drawing.Size(126, 23);
            this.lnkForgotPassword.TabIndex = 1;
            this.lnkForgotPassword.TabStop = true;
            this.lnkForgotPassword.Text = "نسيت كلمة المرور؟";
            //
            // btnLogin
            //
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42))))); // Dark solid button #0F172A
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Cairo", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(20, 340);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(340, 42);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "دخول";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            //
            // btnExit
            //
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139))))); // Muted Gray #64748B
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Cairo", 10F, System.Drawing.FontStyle.Bold);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(20, 395);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(340, 36);
            this.btnExit.TabIndex = 3;
            this.btnExit.TabStop = false;
            this.btnExit.Text = "إغلاق المنظومة";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            //
            // lblError
            //
            this.lblError.Font = new System.Drawing.Font("Cairo", 9F, System.Drawing.FontStyle.Bold);
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68))))); // Soft red #EF4444
            this.lblError.Location = new System.Drawing.Point(20, 290);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(340, 40);
            this.lblError.TabIndex = 5;
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblError.Visible = false;
            //
            // LoginForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 600);
            this.Controls.Add(this.tlpSplit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "منظومة مركز الدورات التعليمية - تسجيل الدخول";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.tlpSplit.ResumeLayout(false);
            this.pnlBrand.ResumeLayout(false);
            this.pnlBrandCenter.ResumeLayout(false);
            this.pnlLoginContainer.ResumeLayout(false);
            this.pnlLoginCard.ResumeLayout(false);
            this.pnlLoginCard.PerformLayout();
            this.pnlPasswordWrapper.ResumeLayout(false);
            this.pnlPasswordWrapper.PerformLayout();
            this.pnlOptions.ResumeLayout(false);
            this.pnlOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSplit;
        private System.Windows.Forms.Panel pnlBrand;
        private System.Windows.Forms.Panel pnlBrandCenter;
        private System.Windows.Forms.Label lblBrandIcon;
        private System.Windows.Forms.Label lblBrandTitle;
        private System.Windows.Forms.Label lblBrandSubtitle;
        private System.Windows.Forms.Panel pnlLoginContainer;
        private System.Windows.Forms.Panel pnlLoginCard;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Panel pnlPasswordWrapper;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnTogglePassword;
        private System.Windows.Forms.Panel pnlOptions;
        private System.Windows.Forms.CheckBox chkRememberMe;
        private System.Windows.Forms.LinkLabel lnkForgotPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblError;
    }
}
