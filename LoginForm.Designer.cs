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
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.pnlLoginCard = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.pnlBackground.SuspendLayout();
            this.pnlLoginCard.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlBackground
            //
            this.pnlBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42))))); // Dark Slate Navy #0F172A
            this.pnlBackground.Controls.Add(this.pnlLoginCard);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(480, 500);
            this.pnlBackground.TabIndex = 0;
            //
            // pnlLoginCard
            //
            this.pnlLoginCard.BackColor = System.Drawing.Color.White;
            this.pnlLoginCard.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pnlLoginCard.Controls.Add(this.lblError);
            this.pnlLoginCard.Controls.Add(this.btnExit);
            this.pnlLoginCard.Controls.Add(this.btnLogin);
            this.pnlLoginCard.Controls.Add(this.txtPassword);
            this.pnlLoginCard.Controls.Add(this.lblPassword);
            this.pnlLoginCard.Controls.Add(this.txtUsername);
            this.pnlLoginCard.Controls.Add(this.lblUsername);
            this.pnlLoginCard.Controls.Add(this.lblSubtitle);
            this.pnlLoginCard.Controls.Add(this.lblTitle);
            this.pnlLoginCard.Location = new System.Drawing.Point(40, 40);
            this.pnlLoginCard.Name = "pnlLoginCard";
            this.pnlLoginCard.Size = new System.Drawing.Size(400, 420);
            this.pnlLoginCard.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.Font = new System.Drawing.Font("Cairo", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(360, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "تسجيل الدخول";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblSubtitle
            //
            this.lblSubtitle.Font = new System.Drawing.Font("Cairo", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSubtitle.Location = new System.Drawing.Point(20, 60);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(360, 25);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "يرجى إدخال اسم المستخدم وكلمة المرور للوصول";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblUsername
            //
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Cairo", 10F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblUsername.Location = new System.Drawing.Point(260, 105);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(114, 23);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "اسم المستخدم:";
            //
            // txtUsername
            //
            this.txtUsername.Font = new System.Drawing.Font("Cairo", 11F);
            this.txtUsername.Location = new System.Drawing.Point(30, 135);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(340, 32);
            this.txtUsername.TabIndex = 0;
            //
            // lblPassword
            //
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Cairo", 10F, System.Drawing.FontStyle.Bold);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblPassword.Location = new System.Drawing.Point(280, 185);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(94, 23);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "كلمة المرور:";
            //
            // txtPassword
            //
            this.txtPassword.Font = new System.Drawing.Font("Cairo", 11F);
            this.txtPassword.Location = new System.Drawing.Point(30, 215);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(340, 32);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtPassword_KeyDown);
            //
            // btnLogin
            //
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235))))); // Accent Blue #2563EB
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Cairo", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(30, 305);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(340, 40);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "دخول";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            //
            // btnExit
            //
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139))))); // Gray #64748B
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Cairo", 10F, System.Drawing.FontStyle.Bold);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(30, 355);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(340, 35);
            this.btnExit.TabIndex = 4;
            this.btnExit.TabStop = false;
            this.btnExit.Text = "إغلاق المنظومة";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            //
            // lblError
            //
            this.lblError.Font = new System.Drawing.Font("Cairo", 9F, System.Drawing.FontStyle.Bold);
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68))))); // Soft red #EF4444
            this.lblError.Location = new System.Drawing.Point(30, 260);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(340, 35);
            this.lblError.TabIndex = 5;
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblError.Visible = false;
            //
            // LoginForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 500);
            this.Controls.Add(this.pnlBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "منظومة مركز الدورات - تسجيل الدخول";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.pnlBackground.ResumeLayout(false);
            this.pnlLoginCard.ResumeLayout(false);
            this.pnlLoginCard.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Panel pnlLoginCard;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblError;
    }
}