using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SchoolCenter
{
    public class SettingsView : UserControl
    {
        private Panel pnlContainer;
        private Label lblTitle;
        private Label lblCenterName;
        private TextBox txtCenterName;
        private Label lblLogo;
        private PictureBox picLogoPreview;
        private Button btnBrowseLogo;
        private Button btnSaveSettings;

        public event EventHandler DataSaved;

        public SettingsView()
        {
            InitializeComponent();
            ThemeHelper.ApplyTheme(this);
            LoadCurrentSettings();
        }

        private void InitializeComponent()
        {
            this.pnlContainer = new Panel();
            this.lblTitle = new Label();
            this.lblCenterName = new Label();
            this.txtCenterName = new TextBox();
            this.lblLogo = new Label();
            this.picLogoPreview = new PictureBox();
            this.btnBrowseLogo = new Button();
            this.btnSaveSettings = new Button();

            this.pnlContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogoPreview)).BeginInit();
            this.SuspendLayout();

            //
            // SettingsView
            //
            this.BackColor = Color.FromArgb(248, 250, 252); // Modern background #F8FAFC
            this.Controls.Add(this.pnlContainer);
            this.Dock = DockStyle.Fill;
            this.Font = new Font("Cairo", 10F);
            this.Name = "SettingsView";
            this.RightToLeft = RightToLeft.Yes;
            this.Size = new Size(820, 600);

            //
            // pnlContainer
            //
            this.pnlContainer.Anchor = AnchorStyles.None;
            this.pnlContainer.BackColor = Color.White;
            this.pnlContainer.BorderStyle = BorderStyle.FixedSingle;
            this.pnlContainer.Controls.Add(this.btnSaveSettings);
            this.pnlContainer.Controls.Add(this.btnBrowseLogo);
            this.pnlContainer.Controls.Add(this.picLogoPreview);
            this.pnlContainer.Controls.Add(this.lblLogo);
            this.pnlContainer.Controls.Add(this.txtCenterName);
            this.pnlContainer.Controls.Add(this.lblCenterName);
            this.pnlContainer.Controls.Add(this.lblTitle);
            this.pnlContainer.Location = new Point(160, 50);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new Size(500, 500);
            this.pnlContainer.TabIndex = 0;

            //
            // lblTitle
            //
            this.lblTitle.Font = new Font("Cairo", 16F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
            this.lblTitle.Location = new Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(460, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "إعدادات الهوية والشعار";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            //
            // lblCenterName
            //
            this.lblCenterName.AutoSize = true;
            this.lblCenterName.Font = new Font("Cairo", 10.5F, FontStyle.Bold);
            this.lblCenterName.ForeColor = Color.FromArgb(15, 23, 42);
            this.lblCenterName.Location = new Point(360, 90);
            this.lblCenterName.Name = "lblCenterName";
            this.lblCenterName.Size = new Size(120, 26);
            this.lblCenterName.TabIndex = 1;
            this.lblCenterName.Text = "اسم الجهة / المركز:";

            //
            // txtCenterName
            //
            this.txtCenterName.BackColor = Color.FromArgb(248, 250, 252);
            this.txtCenterName.BorderStyle = BorderStyle.FixedSingle;
            this.txtCenterName.Font = new Font("Cairo", 11F);
            this.txtCenterName.Location = new Point(40, 125);
            this.txtCenterName.Name = "txtCenterName";
            this.txtCenterName.Size = new Size(420, 35);
            this.txtCenterName.TabIndex = 2;

            //
            // lblLogo
            //
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new Font("Cairo", 10.5F, FontStyle.Bold);
            this.lblLogo.ForeColor = Color.FromArgb(15, 23, 42);
            this.lblLogo.Location = new Point(360, 185);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new Size(95, 26);
            this.lblLogo.TabIndex = 3;
            this.lblLogo.Text = "شعار المؤسسة:";

            //
            // picLogoPreview
            //
            this.picLogoPreview.BackColor = Color.FromArgb(248, 250, 252);
            this.picLogoPreview.BorderStyle = BorderStyle.FixedSingle;
            this.picLogoPreview.Location = new Point(40, 220);
            this.picLogoPreview.Name = "picLogoPreview";
            this.picLogoPreview.Size = new Size(240, 140);
            this.picLogoPreview.SizeMode = PictureBoxSizeMode.Zoom;
            this.picLogoPreview.TabIndex = 4;
            this.picLogoPreview.TabStop = false;

            //
            // btnBrowseLogo
            //
            this.btnBrowseLogo.BackColor = Color.FromArgb(100, 116, 139); // Muted slate gray
            this.btnBrowseLogo.Cursor = Cursors.Hand;
            this.btnBrowseLogo.FlatAppearance.BorderSize = 0;
            this.btnBrowseLogo.FlatStyle = FlatStyle.Flat;
            this.btnBrowseLogo.Font = new Font("Cairo", 9.5F, FontStyle.Bold);
            this.btnBrowseLogo.ForeColor = Color.White;
            this.btnBrowseLogo.Location = new Point(295, 220);
            this.btnBrowseLogo.Name = "btnBrowseLogo";
            this.btnBrowseLogo.Size = new Size(165, 35);
            this.btnBrowseLogo.TabIndex = 5;
            this.btnBrowseLogo.Text = "اختيار شعار جديد";
            this.btnBrowseLogo.UseVisualStyleBackColor = false;
            this.btnBrowseLogo.Click += new EventHandler(this.BtnBrowseLogo_Click);

            //
            // btnSaveSettings
            //
            this.btnSaveSettings.BackColor = Color.FromArgb(37, 99, 235); // Accent blue #2563EB
            this.btnSaveSettings.Cursor = Cursors.Hand;
            this.btnSaveSettings.FlatAppearance.BorderSize = 0;
            this.btnSaveSettings.FlatStyle = FlatStyle.Flat;
            this.btnSaveSettings.Font = new Font("Cairo", 11F, FontStyle.Bold);
            this.btnSaveSettings.ForeColor = Color.White;
            this.btnSaveSettings.Location = new Point(40, 410);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new Size(420, 45);
            this.btnSaveSettings.TabIndex = 6;
            this.btnSaveSettings.Text = "حفظ الإعدادات";
            this.btnSaveSettings.UseVisualStyleBackColor = false;
            this.btnSaveSettings.Click += new EventHandler(this.BtnSaveSettings_Click);

            this.pnlContainer.ResumeLayout(false);
            this.pnlContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogoPreview)).EndInit();
            this.ResumeLayout(false);
        }

        private void LoadCurrentSettings()
        {
            try
            {
                Tuple<string, Image> settings = SettingsService.GetSettings();
                txtCenterName.Text = settings.Item1;
                if (settings.Item2 != null)
                {
                    picLogoPreview.Image = settings.Item2;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadCurrentSettings failed: " + ex.Message);
            }
        }

        private void BtnBrowseLogo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "ملفات الصور|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.ico";
                ofd.Title = "اختر شعار الجهة الجديد";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Load image to preview
                        picLogoPreview.Image = Image.FromFile(ofd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("فشل تحميل الصورة المحددة: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            string centerName = txtCenterName.Text.Trim();
            if (string.IsNullOrEmpty(centerName))
            {
                MessageBox.Show("يرجى إدخال اسم الجهة أو المركز.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SettingsService.SaveSettings(centerName, picLogoPreview.Image);
                MessageBox.Show("تم حفظ الإعدادات وهوية الجهة بنجاح.", "نجاح العملية", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnDataSaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ أثناء الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
