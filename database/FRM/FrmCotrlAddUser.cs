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
using System.Xml.Linq;

namespace database.FRM
{
    public partial class FrmCotrlAddUser : UserControl
    {
        SqlConnection Connection;
        public FrmCotrlAddUser()
        {
            InitializeComponent();
            Connection = new SqlConnection("Server=DELL-5430\\SQLEXPRESS;Database = StoreX; Integrated Security = true;");
        }
        public void FillUserData(string id, string name, string phone, string address)
        {
            txtAddUserIDControl.Text = id;
            txtAddUserNameControl.Text = name;
            txtAddUserPhoneControl.Text = phone;
            txtAddUserAddressControl.Text = address;
        }
        private void btn_AddUser_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(txtAddUserIDControl.Text) ||
                    string.IsNullOrWhiteSpace(txtAddUserNameControl.Text) ||
                    string.IsNullOrWhiteSpace(txtAddUserPhoneControl.Text) ||
                    string.IsNullOrWhiteSpace(txtAddUserAddressControl.Text))
                {
                    MessageBox.Show("Please enter complete information!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                if (CheckCustomerExists(txtAddUserIDControl.Text))
                {
                    DialogResult result = MessageBox.Show(
                        "There is currently a customer with the same ID, do you want to update?",
                        "Confirm update",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        UpdateCustomer();
                    }
                    else
                    {
                        return; 
                    }
                }
                else
                {
   
                    string query = @"INSERT INTO [dbo].[Customers] 
                            (CustomerID, CustumerNAME, PhoneOfCustomers, AddressOfCustomers)
                            VALUES (@ID, @Name, @Phone, @Address)";

                    Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, Connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", txtAddUserIDControl.Text);
                        cmd.Parameters.AddWithValue("@Name", txtAddUserNameControl.Text);
                        cmd.Parameters.AddWithValue("@Phone", txtAddUserPhoneControl.Text);
                        cmd.Parameters.AddWithValue("@Address", txtAddUserAddressControl.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Customer added successfully!", "Notification",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

    
                btn_ClearUser_Click(sender, e);

      
                if (Parent is Form parentForm)
                {
                    foreach (Control control in parentForm.Controls)
                    {
                       
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
        }

        private bool CheckCustomerExists(string customerId)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Customers WHERE CustomerID = @ID", Connection))
                {
                    cmd.Parameters.AddWithValue("@ID", customerId);
                    Connection.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
        }


        private void UpdateCustomer()
        {
            try
            {
                string query = @"UPDATE [dbo].[Customers] 
                           SET CustumerNAME = @Name, 
                               PhoneOfCustomers = @Phone, 
                               AddressOfCustomers = @Address
                           WHERE CustomerID = @ID";

                Connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@ID", txtAddUserIDControl.Text);
                    cmd.Parameters.AddWithValue("@Name", txtAddUserNameControl.Text);
                    cmd.Parameters.AddWithValue("@Phone", txtAddUserPhoneControl.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddUserAddressControl.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer update successful!", "Notification",
                        MessageBoxButtons.OK, MessageBoxIcon.Information );
                    if (Parent is Form parentForm)
                    {
                        foreach (Control control in parentForm.Controls)
                        {
                           
                        }
                    }
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while updating: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
        }



        private void btn_ClearUser_Click(object sender, EventArgs e)
        {
            txtAddUserIDControl.Clear();
            txtAddUserNameControl.Clear();
            txtAddUserPhoneControl.Clear();
            txtAddUserAddressControl.Clear();
        }





        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                BringToFront();
            }
        }

        private void btnExitFromAddUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            txtAddUserIDControl.Clear();
            txtAddUserNameControl.Clear();
            txtAddUserPhoneControl.Clear();
            txtAddUserAddressControl.Clear();

        }
    }
}
