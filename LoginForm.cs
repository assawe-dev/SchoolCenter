using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SchoolCenter
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Ensure database is initialized and seeded before trying to log in
                DbConnectionManager.InitializeDatabase();
                ThemeHelper.ApplyTheme(this);

                // Load dynamic branding settings
                Tuple<string, Image> settings = SettingsService.GetSettings();
                lblCenterHeader.Text = settings.Item1;
              
                if (settings.Item2 != null)
                {
                    picCenterLogo.Image = settings.Item2;
                }
                else
                {
                    // Fallback to default emoji symbol rendered into an image to satisfy requirement perfectly
                    picCenterLogo.Image = CreateDefaultLogoImage();
                }

                txtUsername.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تهيئة قاعدة البيانات: " + ex.Message, "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Image CreateDefaultLogoImage()
        {
            Bitmap bmp = new Bitmap(200, 100);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(System.Drawing.Color.Transparent);
                using (Font font = new Font("Segoe UI", 48F))
                {
                    g.DrawString("🏫", font, System.Drawing.Brushes.Black, new PointF(40, 10));
                }
            }
            return bmp;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "يرجى إدخال اسم المستخدم وكلمة المرور";
                lblError.Visible = true;
                return;
            }

            try
            {
                string connectionString = DbConnectionManager.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Query both user details and their granular permissions
                    string query = @"
                        SELECT u.UserID, u.Username, u.Role, u.IsActive,
                               p.CanManageStudents, p.CanManageCourses, p.CanAssignDues,
                               p.CanReceivePayments, p.CanViewReports, p.CanManageUsers
                        FROM Users u
                        LEFT JOIN UserPermissions p ON u.UserID = p.UserID
                        WHERE u.Username = @user AND u.PasswordHash = @pass AND u.IsActive = 1";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", username);
                        cmd.Parameters.AddWithValue("@pass", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate UserSession
                                UserSession.CurrentUserID = Convert.ToInt32(reader["UserID"]);
                                UserSession.Username = reader["Username"].ToString();
                                UserSession.Role = reader["Role"].ToString();

                                // Set permissions (fallback to defaults if NULL)
                                UserSession.CanManageStudents = reader["CanManageStudents"] != DBNull.Value ? Convert.ToBoolean(reader["CanManageStudents"]) : true;
                                UserSession.CanManageCourses = reader["CanManageCourses"] != DBNull.Value ? Convert.ToBoolean(reader["CanManageCourses"]) : true;
                                UserSession.CanAssignDues = reader["CanAssignDues"] != DBNull.Value ? Convert.ToBoolean(reader["CanAssignDues"]) : true;
                                UserSession.CanReceivePayments = reader["CanReceivePayments"] != DBNull.Value ? Convert.ToBoolean(reader["CanReceivePayments"]) : true;
                                UserSession.CanViewReports = reader["CanViewReports"] != DBNull.Value ? Convert.ToBoolean(reader["CanViewReports"]) : false;
                                UserSession.CanManageUsers = reader["CanManageUsers"] != DBNull.Value ? Convert.ToBoolean(reader["CanManageUsers"]) : false;

                                // Hide this login form and show the main Form1
                                this.Hide();
                                Form1 mainForm = new Form1();
                                mainForm.ShowDialog();

                                // When mainForm returns:
                                if (UserSession.CurrentUserID == 0)
                                {
                                    // User logged out
                                    this.Show();
                                    txtUsername.Clear();
                                    txtPassword.Clear();
                                    lblError.Visible = false;
                                    txtUsername.Focus();
                                }
                                else
                                {
                                    // User closed application directly
                                    this.Close();
                                }
                            }
                            else
                            {
                                lblError.Text = "اسم المستخدم أو كلمة المرور غير صحيحة";
                                lblError.Visible = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء محاولة تسجيل الدخول: " + ex.Message, "خطأ في الاتصال", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExitApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void BtnTogglePassword_Click(object sender, EventArgs e)
        {
            if (txtPassword.UseSystemPasswordChar)
            {
                txtPassword.UseSystemPasswordChar = false;
                btnTogglePassword.Text = "🙈";
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
                btnTogglePassword.Text = "👁️";
            }
        }
    }
}
