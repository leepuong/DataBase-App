using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace database.FRM
{
    public partial class AdminFrmEmployees : UserControl
    {
        SqlConnection Connection;
        private FunctionFrm functionFrm;
        private string selectedEmployeeID;
        private string selectedEmployeeName;
        private string selectedUsername;
        private string selectedPassword;
        private string selectedRole;
        private string selectedEmail;
        private string selectedPhone;
        private string selectedAddress;

        private Form functionFormWrapper;



        public AdminFrmEmployees()
        {
            InitializeComponent();
            Connection = new SqlConnection("Server=DELL-5430\\SQLEXPRESS;Database  = StoreX; Integrated Security = true;");
            if (functionFormWrapper != null)
            {
                FunctionFrm functionFrm = functionFormWrapper.Controls.OfType<FunctionFrm>().FirstOrDefault();
                if (functionFrm != null)
                {
                    functionFrm.EditButtonClicked += FillTextBoxesWithSelectedEmployee;
                }
            }
        }
        private void FillTextBoxesWithSelectedEmployee(object sender, EventArgs e)
        {
    
            txtIDEmp.Text = selectedEmployeeID;
            txtNameEmp.Text = selectedEmployeeName;
            txtUserNameEmp.Text = selectedUsername;
            txtPassEmp.Text = selectedPassword;
            txtEmailEmp.Text = selectedEmail;
            txtPhoneEmp.Text = selectedPhone;
            txtAddressEmp.Text = selectedAddress;
        }
        public void FillData()
        {
            String query = "SELECT  EmployeeID as '     ID'\r\n      ,EmployeeNAME as '     Name'\r\n      ,UsernameOfEmployees as  '     User name'\r\n      ,ShowPass as '     Password'\r\n      ,RoleOfEmployees as '     Role'\r\n      ,EmailOfEmployees as '     Email'\r\n      ,PhoneOfEmployees as '     Phone'\r\n      ,AddressOfEmployees as '     Address'\r\n      \r\n  FROM Employees";
            DataTable tb = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(query, Connection);
            ad.Fill(tb);
            dgvwAdminFrmEmployees.DataSource = tb;
            Connection.Close();
        }






        private void dgvwAdminFrmEmployees_MouseDown(object sender, MouseEventArgs e)
        {


            if (dgvwAdminFrmEmployees.CurrentRow != null)
            {
         
                selectedEmployeeID = dgvwAdminFrmEmployees.CurrentRow.Cells["     ID"].Value?.ToString();
                selectedEmployeeName = dgvwAdminFrmEmployees.CurrentRow.Cells["     Name"].Value?.ToString();
                selectedUsername = dgvwAdminFrmEmployees.CurrentRow.Cells["     User name"].Value?.ToString();
                selectedPassword = dgvwAdminFrmEmployees.CurrentRow.Cells["     Password"].Value?.ToString();
                selectedRole = dgvwAdminFrmEmployees.CurrentRow.Cells["     Role"].Value?.ToString();
                selectedEmail = dgvwAdminFrmEmployees.CurrentRow.Cells["     Email"].Value?.ToString();
                selectedPhone = dgvwAdminFrmEmployees.CurrentRow.Cells["     Phone"].Value?.ToString();
                selectedAddress = dgvwAdminFrmEmployees.CurrentRow.Cells["     Address"].Value?.ToString();
            }
            if (e.Button == MouseButtons.Right)
            {

                if (functionFormWrapper == null || functionFormWrapper.IsDisposed)
                {
 
                    functionFormWrapper = new Form
                    {
                        FormBorderStyle = FormBorderStyle.None, 
                        StartPosition = FormStartPosition.Manual,
                        TopMost = true,
                        Size = new Size(60, 80)
                    };

  
                    FunctionFrm functionFrm = new FunctionFrm();
                    functionFrm.DeleteButtonClicked += (s, args) => DeleteEmployee();
                    functionFrm.EditButtonClicked += FillTextBoxesWithSelectedEmployee;
                    functionFormWrapper.Controls.Add(functionFrm);
                    FillRoleComboBox();
                    functionFrm.Dock = DockStyle.Fill;
                    functionFormWrapper.ClientSize = functionFrm.Size;
                }


                functionFormWrapper.Location = Cursor.Position;

     
                if (!functionFormWrapper.Visible)
                {
                    functionFormWrapper.Show();
                }
                else
                {
            
                    functionFormWrapper.BringToFront();
                }
            }

            if (e.Button == MouseButtons.Left)
            {
          
                if (functionFormWrapper != null && functionFormWrapper.Visible)
                {
                    FillRoleComboBox();
                    functionFormWrapper.Close();
                }
            }

        }





        private void InsertEmployee()
        {

            if (!ValidatePassword(txtPassEmp.Text))
            {
                return; 
            }


            string insertQuery = @"INSERT INTO Employees 
            (EmployeeID, EmployeeName, UsernameOfEmployees, ShowPass, RoleOfEmployees, 
            EmailOfEmployees, PhoneOfEmployees, AddressOfEmployees, PasswordOfEmployees) 
            VALUES 
            (@EmployeeID, @EmployeeName, @Username, @ShowPass, @Role, 
            @Email, @Phone, @Address, @Password )";

            using (SqlCommand cmd = new SqlCommand(insertQuery, Connection))
            {
                cmd.Parameters.AddWithValue("@EmployeeID", txtIDEmp.Text);
                cmd.Parameters.AddWithValue("@EmployeeName", txtNameEmp.Text);
                cmd.Parameters.AddWithValue("@Username", txtUserNameEmp.Text);
                cmd.Parameters.AddWithValue("@Role", cboRoleEmp.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmailEmp.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhoneEmp.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddressEmp.Text);
                cmd.Parameters.AddWithValue("@ShowPass", txtPassEmp.Text);
                string passSalt = txtPassEmp.Text;

                cmd.Parameters.AddWithValue("@Password", BCrypt.Net.BCrypt.HashPassword(passSalt, 13));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Added employee successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            cboRoleEmp.Items.Clear();
            txtIDEmp.Clear();
            txtNameEmp.Clear();
            txtUserNameEmp.Clear();
            txtEmailEmp.Clear();
            txtPassEmp.Clear();
            txtPhoneEmp.Clear();
            txtAddressEmp.Clear();
        }

        private bool ValidatePassword(string password)
        {

            if (password.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            bool hasUpperCase = false;
            bool hasSpecialChar = false;
            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    hasUpperCase = true;
                }

                if (!char.IsLetterOrDigit(c))
                {
                    hasSpecialChar = true;
                }
            }



            if (!hasUpperCase)
            {
                MessageBox.Show("Password must contain at least one uppercase character!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            if (!hasSpecialChar)
            {
                MessageBox.Show("Password must contain at least one special character!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void UpdateEmployee()
        {

            if (!ValidatePassword(txtPassEmp.Text))
            {
                return; 
            }


            string updateQuery = @"UPDATE Employees SET 
            EmployeeName = @EmployeeName, 
            UsernameOfEmployees = @Username, 
            ShowPass = @ShowPass, 
            RoleOfEmployees = @Role, 
            EmailOfEmployees = @Email, 
            PhoneOfEmployees = @Phone, 
            AddressOfEmployees = @Address, 
            PasswordOfEmployees =@Password
            WHERE EmployeeID = @EmployeeID";

            using (SqlCommand cmd = new SqlCommand(updateQuery, Connection))
            {
                cmd.Parameters.AddWithValue("@EmployeeID", txtIDEmp.Text);
                cmd.Parameters.AddWithValue("@EmployeeName", txtNameEmp.Text);
                cmd.Parameters.AddWithValue("@Username", txtUserNameEmp.Text);
                cmd.Parameters.AddWithValue("@ShowPass", txtPassEmp.Text);
                cmd.Parameters.AddWithValue("@Role", cboRoleEmp.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmailEmp.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhoneEmp.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddressEmp.Text);

                string passSalt = txtPassEmp.Text;

                cmd.Parameters.AddWithValue("@Password", BCrypt.Net.BCrypt.HashPassword(passSalt, 13));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Employee updated successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            cboRoleEmp.Items.Clear();
            txtIDEmp.Clear();
            txtNameEmp.Clear();
            txtUserNameEmp.Clear();
            txtEmailEmp.Clear();
            txtPassEmp.Clear();
            txtPhoneEmp.Clear();
            txtAddressEmp.Clear();
        }

        private void DeleteEmployee()
        {
            if (string.IsNullOrEmpty(selectedEmployeeID))
            {
                MessageBox.Show("Please select an employee to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

 
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete employee: {selectedEmployeeName}?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection("Server=DELL-5430\\SQLEXPRESS;Database = StoreX; Integrated Security = true;"))
                    {
                        conn.Open();
                        using (SqlTransaction transaction = conn.BeginTransaction())
                        {
                            try
                            {
                      
                                string updateOrdersQuery = "UPDATE Orders SET EmployeeID = NULL WHERE EmployeeID = @EmployeeID";
                                string deleteEmployeeQuery = "DELETE FROM Employees WHERE EmployeeID = @EmployeeID";


           
                                using (SqlCommand updateCmd = new SqlCommand(updateOrdersQuery, conn, transaction))
                                {
                                    updateCmd.Parameters.AddWithValue("@EmployeeID", selectedEmployeeID);
                                    updateCmd.ExecuteNonQuery();
                                }

                                using (SqlCommand deleteEmpCmd = new SqlCommand(deleteEmployeeQuery, conn, transaction))
                                {
                                    deleteEmpCmd.Parameters.AddWithValue("@EmployeeID", selectedEmployeeID);
                                    int rowsAffected = deleteEmpCmd.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        transaction.Commit();
                                        MessageBox.Show("Employee deleted successfully!", "Success",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        FillData();
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        MessageBox.Show("No employee was deleted. Please try again.", "Error",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        

        private void AdminFrmEmployees_Load(object sender, EventArgs e)
        {
            Connection.Open();
            FillData();

            FillRoleComboBox();
        }
        private void FillRoleComboBox()
        {
            try
            {
    
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }


                string query = "SELECT DISTINCT RoleOfEmployees FROM Employees";
                SqlCommand command = new SqlCommand(query, Connection);
                SqlDataReader reader = command.ExecuteReader();

        
                cboRoleEmp.Items.Clear();
                cboReaschRoleEmp.Items.Clear();

    
                while (reader.Read())
                {
                    cboRoleEmp.Items.Add(reader["RoleOfEmployees"].ToString());
                    cboReaschRoleEmp.Items.Add(reader["RoleOfEmployees"].ToString());
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while filling the role combo box: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
      
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
            }
        }

        private void btn_AddEmployees_Click(object sender, EventArgs e)
        {



   
            if (string.IsNullOrEmpty(txtIDEmp.Text) ||
                string.IsNullOrEmpty(txtNameEmp.Text) ||
                string.IsNullOrEmpty(txtUserNameEmp.Text) ||
                string.IsNullOrEmpty(txtPassEmp.Text) ||
                string.IsNullOrEmpty(cboRoleEmp.Text))
            {
                MessageBox.Show("Please fill in all information!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM Employees WHERE EmployeeID = @EmployeeID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, Connection);
                checkCmd.Parameters.AddWithValue("@EmployeeID", txtIDEmp.Text);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
          
                    DialogResult result = MessageBox.Show(
                        "This employee id already exists in the database. Do you want to update?",
                        "Confirm",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
               
                        UpdateEmployee();   
                    }
                }
                else
                {
  
                    InsertEmployee();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
                FillData();
            }
        }

        private void btn_ClearEmployees_Click(object sender, EventArgs e)
        {
            cboRoleEmp.Items.Clear();
            txtIDEmp.Clear();
            txtNameEmp.Clear();
            txtUserNameEmp.Clear();
            txtEmailEmp.Clear();
            txtPassEmp.Clear();
            txtPhoneEmp.Clear();
            txtAddressEmp.Clear();
            FillRoleComboBox();
        }



        private void btnResetBtnEmp_Click(object sender, EventArgs e)
        {
            try
            {
     
                cboReaschRoleEmp.Items.Clear();
                txtReaschBoxEmp.Clear();
                btnReaschAddressEmp.Checked = false;
                btnReaschEmailEmp.Checked = false;
                btnReaschIDEmp.Checked = false;
                btnReaschNameEmp.Checked = false;
                btnReaschPhoneEmp.Checked = false;

    
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

         
                FillData();

     
                FillRoleComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resetting data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
            }
        }

        private void btnSeaschBtnEmp_Click(object sender, EventArgs e)
        {
            try
            {
                string searchValue = txtReaschBoxEmp.Text.Trim();
                string baseQuery = @"SELECT  EmployeeID as '     ID'
                          ,EmployeeNAME as '     Name'
                          ,UsernameOfEmployees as  '     User name'
                          ,ShowPass as '     Password'
                          ,RoleOfEmployees as '     Role'
                          ,EmailOfEmployees as '     Email'
                          ,PhoneOfEmployees as '     Phone'
                          ,AddressOfEmployees as '     Address'
                          FROM Employees WHERE ";

                string whereClause = "";

           
                if (btnReaschIDEmp.Checked)
                {
                    whereClause = "[EmployeeID] = @searchValue";
                }
                else if (btnReaschNameEmp.Checked)
                {
                    whereClause = "[EmployeeNAME] LIKE @searchValue + '%'";
                }
                else if (btnReaschPhoneEmp.Checked)
                {
                    whereClause = "[PhoneOfEmployees] LIKE @searchValue + '%'";
                }
                else if (btnReaschEmailEmp.Checked)
                {
                    whereClause = "[EmailOfEmployees] LIKE @searchValue + '%'";
                }
                else if (btnReaschAddressEmp.Checked)
                {
                    whereClause = "[AddressOfEmployees] LIKE @searchValue + '%'";
                }
                else
                {
     
                    whereClause = "[EmployeeNAME] LIKE @searchValue + '%'";
                }

      
                if (!string.IsNullOrEmpty(cboReaschRoleEmp.Text))
                {
                    whereClause += " AND [RoleOfEmployees] = @roleValue";
                }

                string finalQuery = baseQuery + whereClause;

                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                using (SqlCommand cmd = new SqlCommand(finalQuery, Connection))
                {
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    if (!string.IsNullOrEmpty(cboReaschRoleEmp.Text))
                    {
                        cmd.Parameters.AddWithValue("@roleValue", cboReaschRoleEmp.Text);
                    }

                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    dgvwAdminFrmEmployees.DataSource = dt;

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No results found!", "Message",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
            }
        }
    }
}
