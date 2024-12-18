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
    public partial class AdminFrmCustomer : UserControl
    {
        SqlConnection Connection;
        private FunctionFrm functionFrm;

        private string selectedAdminIDCustomer;
        private string selectedAdminNameCustomer;
        private string selectedAdminPhoneCustomer;
        private string selectedAdminAddressCustomer;

        private Form functionFormWrapper;

        public AdminFrmCustomer()
        {
            InitializeComponent();
            Connection = new SqlConnection("Server=DELL-5430\\SQLEXPRESS;Database  = StoreX; Integrated Security = true;");

        }

        public void FillData()
        {
            String query = "SELECT CustomerID as \"    ID\"\r\n      , CustumerNAME as \"     Name\"\r\n      , PhoneOfCustomers as \"     Phone\"\r\n      , AddressOfCustomers as \"     Address\"\r\n  FROM Customers";
            DataTable tb = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(query, Connection);
            ad.Fill(tb);
            dgvwAdminFrmCustomer.DataSource = tb;
            Connection.Close();
        }


        private void clearTextBox()
        {
            txtFrmAddAddressCus.Clear();
            txtFrmAddIDCus.Clear();
            txtFrmAddNameCus.Clear();
            txtFrmAddPhoneCus.Clear();
        }
        private void FillTextBoxWithSelectData(object sender, EventArgs e)
        {
            txtFrmAddAddressCus.Text = selectedAdminAddressCustomer;
            txtFrmAddIDCus.Text = selectedAdminIDCustomer;
            txtFrmAddNameCus.Text = selectedAdminNameCustomer;
            txtFrmAddPhoneCus.Text = selectedAdminPhoneCustomer;
        }

        private void AdminFrmCustomer_Load(object sender, EventArgs e)
        {
            Connection.Open();
            FillData();
        }

        private void btnClearCustomerAdmin_Click(object sender, EventArgs e)
        {
            clearTextBox();
        }

        private void btnAddCustomerAdmin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFrmAddIDCus.Text) ||
                string.IsNullOrEmpty(txtFrmAddNameCus.Text) ||
                string.IsNullOrEmpty(txtFrmAddPhoneCus.Text) ||
                string.IsNullOrEmpty(txtFrmAddAddressCus.Text))
            {
                MessageBox.Show("Please fill in all information!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                Connection.Open();
                string checkQuery = "SELECT COUNT(*) FROM Customers WHERE CustomerID = @CustomerID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, Connection);
                checkCmd.Parameters.AddWithValue("@CustomerID", txtFrmAddIDCus.Text);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count > 0)
                {

                    DialogResult result = MessageBox.Show(
                        "This Customer id already exists in the database. Do you want to update?",
                        "Confirm",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {

                        UpdateCustomer();
                    }
                }
                else
                {
                    InsertCustomer();
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


        private void InsertCustomer()
        {
            string insertQuery = @"INSERT INTO [Customers] 
                    VALUES (
                    @CustomerID,
                    @CustumerNAME,
                    @PhoneOfCustomers,
                    @AddressOfCustomers
                    )";

            using (SqlCommand cmd = new SqlCommand(insertQuery, Connection))
            {
                cmd.Parameters.AddWithValue("@CustomerID", txtFrmAddIDCus.Text);
                cmd.Parameters.AddWithValue("@CustumerNAME", txtFrmAddNameCus.Text);
                cmd.Parameters.AddWithValue("@PhoneOfCustomers", txtFrmAddPhoneCus.Text);
                cmd.Parameters.AddWithValue("@AddressOfCustomers", txtFrmAddAddressCus.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Added Prodcut successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            clearTextBox();
        }

        private void UpdateCustomer()
        {
            string updateQuery = @"UPDATE [Customers] SET 
           
            [CustumerNAME] = @CustumerNAME, 
            [PhoneOfCustomers] = @PhoneOfCustomers, 
            [AddressOfCustomers] = @AddressOfCustomers

            WHERE CustomerID = @CustomerID";

            using (SqlCommand cmd = new SqlCommand(updateQuery, Connection))
            {
                cmd.Parameters.AddWithValue("@CustomerID", txtFrmAddIDCus.Text);
                cmd.Parameters.AddWithValue("@CustumerNAME", txtFrmAddNameCus.Text);
                cmd.Parameters.AddWithValue("@PhoneOfCustomers", txtFrmAddPhoneCus.Text);
                cmd.Parameters.AddWithValue("@AddressOfCustomers", txtFrmAddAddressCus.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Employee updated successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            clearTextBox();
        }

        private void dgvwAdminFrmCustomer_MouseDown(object sender, MouseEventArgs e)
        {
            if (dgvwAdminFrmCustomer.CurrentRow != null)
            {

                selectedAdminIDCustomer = dgvwAdminFrmCustomer.CurrentRow.Cells["    ID"].Value?.ToString();
                selectedAdminNameCustomer = dgvwAdminFrmCustomer.CurrentRow.Cells["     Name"].Value?.ToString();
                selectedAdminPhoneCustomer = dgvwAdminFrmCustomer.CurrentRow.Cells["     Phone"].Value?.ToString();
                selectedAdminAddressCustomer = dgvwAdminFrmCustomer.CurrentRow.Cells["     Address"].Value?.ToString();
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
                    functionFrm.DeleteButtonClicked += (s, args) => DeleteCustomer();
                    functionFrm.EditButtonClicked += FillTextBoxWithSelectData;
                    functionFormWrapper.Controls.Add(functionFrm);


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
                    functionFormWrapper.Close();
                }
            }
        }


        private void DeleteCustomer()
        {
            if (string.IsNullOrEmpty(selectedAdminIDCustomer))
            {
                MessageBox.Show("Please select an employee to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete Product: {selectedAdminNameCustomer}?",
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



                                string updateNullProductQuery = "UPDATE [Orders] SET [CustomerID] = NULL WHERE [CustomerID] = @CustomerID;";

                                using (SqlCommand updateCmd = new SqlCommand(updateNullProductQuery, conn, transaction))
                                {
                                    updateCmd.Parameters.AddWithValue("@CustomerID", selectedAdminIDCustomer);
                                    updateCmd.ExecuteNonQuery();
                                }



                    
                                string deleteEmployeeQuery = "DELETE FROM [StoreX].[dbo].[Customers] WHERE CustomerID = @CustomerID;";
                                using (SqlCommand deleteEmpCmd = new SqlCommand(deleteEmployeeQuery, conn, transaction))
                                {
                                    deleteEmpCmd.Parameters.AddWithValue("@CustomerID", selectedAdminIDCustomer);
                                    int rowsAffected = deleteEmpCmd.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        transaction.Commit();
                                        MessageBox.Show("Customer deleted successfully!", "Success",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        FillData();
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        MessageBox.Show("No Customer was deleted. Please try again.", "Error",
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

        private void btnSeaschBtnAdminCus_Click(object sender, EventArgs e)
        {

            try
            {
                string searchValue = btnSeaschBtnAdminCustomer.Text.Trim();
                String baseQuery = "SELECT [CustomerID] as \"     ID\",[CustumerNAME] as \"     Name\",[PhoneOfCustomers] as \"     Phone\",[AddressOfCustomers] as \"     Address\" FROM [StoreX].[dbo].[Customers]  where ";
                string whereClause = "";
                if (btnReaschIDCus.Checked)
                {
                    whereClause = "[CustomerID] = @searchValue";
                }
                else if (btnReaschNameCus.Checked)
                {
                    whereClause = "[CustumerNAME] LIKE @searchValue + '%'";
                }
                else if (btnReaschPhoneCus.Checked)
                {
                    whereClause = "[PhoneOfCustomers] LIKE @searchValue + '%'";
                }
                else if (btnAddCustomerAdmin.Checked)
                {
                    whereClause = "[AddressOfCustomers] = LIKE @searchValue + '%'";
                }
                else
                {
                    whereClause = "[CustumerNAME] LIKE @searchValue + '%'";
                }

                string finalQuery = baseQuery + whereClause;
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                using (SqlCommand cmd = new SqlCommand(finalQuery, Connection))
                {
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    dgvwAdminFrmCustomer.DataSource = dt;

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

        private void btnResetBtnAdminCus_Click(object sender, EventArgs e)
        {
            btnReaschIDCus.Checked = false;
            btnReaschNameCus.Checked = false;
            btnReaschPhoneCus.Checked = false;
            btnReaschAddressCus.Checked = false;
            FillData();
        }
    }
}
