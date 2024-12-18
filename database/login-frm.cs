using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;
using BCrypt.Net;

namespace database
{


    public partial class login_frm : Form
    {
        SqlConnection conn = new SqlConnection("Server=DELL-5430\\SQLEXPRESS;Database  = StoreX; Integrated Security = true;");

        /*public string UserPassID { get; private set; }*/
        public string usernameLog { get; private set; }
        public string passwordLog { get; private set; }


        public event EventHandler UserPassAdded;


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        public login_frm()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            }
            txt_Password.UseSystemPasswordChar = true;

        }

        private void login_frm_Load(object sender, EventArgs e)
        {

        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btn_Login_Click(object sender, EventArgs e)
        {

            try
            {
                usernameLog = txt_UserName.Text.Trim();
                passwordLog = txt_Password.Text;

                if (string.IsNullOrEmpty(usernameLog) || string.IsNullOrEmpty(passwordLog))
                {
                    MessageBox.Show("Please enter full login information!");
                    return;
                }

                string query = "SELECT PasswordOfEmployees, RoleOfEmployees, EmployeeID FROM Employees WHERE UsernameOfEmployees = @Username";

                using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar) { Value = usernameLog });
                        MessageBox.Show(usernameLog);
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHash = reader["PasswordOfEmployees"].ToString();
                                string userRole = reader["RoleOfEmployees"]?.ToString() ?? "default";
                                string userPassID = reader["EmployeeID"]?.ToString();

                                if (BCrypt.Net.BCrypt.Verify(passwordLog, storedHash))
                                {
                                    GlobalVariables.CurrentEmployeeID = userPassID;

                                    Manager formMain = new Manager(userRole);
                                    formMain.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    MessageBox.Show("Incorrect username or password!");
                                    txt_UserName.Clear();
                                    txt_Password.Clear();
                                    txt_UserName.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("User does not exist!");
                                txt_UserName.Clear();
                                txt_Password.Clear();
                                txt_UserName.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }





        }



        


private void SavePassword(string username, string password)
    {
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        string query = "INSERT INTO Employees (UsernameOfEmployees, PasswordOfEmployees) VALUES (@Username, @Password)";
        using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar) { Value = username });
                cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar) { Value = hashedPassword });

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }



        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox2.Checked)
            {
                txt_Password.UseSystemPasswordChar = false;
            }
            else
            {
                txt_Password.UseSystemPasswordChar = true;
            }
        }
    }

    public static class GlobalVariables
    {
        public static string CurrentEmployeeID { get; set; }
    }
}
