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
    public partial class FrmControlCustomerID : UserControl
    {
        SqlConnection Connection;

        public string OrderId { get; private set; }
        public string CustomerId { get; private set; }

        public event EventHandler CustomerIDAdd;

        public FrmControlCustomerID()
        {
            InitializeComponent();
            Connection = new SqlConnection("Server=DELL-5430\\SQLEXPRESS;Database  = StoreX; Integrated Security = true;");
        }


        private void FillCustomerIDComboBox()
        {
            try
            {

                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }


                string query = " SELECT CONCAT([CustumerNAME],'  ID: ',[CustomerID]) as 'Customer ID' FROM [Customers]";
                SqlCommand command = new SqlCommand(query, Connection);
                SqlDataReader reader = command.ExecuteReader();


                cboAddCustomerID.Items.Clear();

                while (reader.Read())
                {
                    cboAddCustomerID.Items.Add(reader["Customer ID"].ToString());
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while filling the Customer combo box: {ex.Message}", "Error",
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

        private void FrmControlCustomerID_Load(object sender, EventArgs e)
        {
            Connection.Open();
            FillCustomerIDComboBox();

        }


        private void txtAddOrderID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtAddOrderID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtAddOrderID.Text) || !string.IsNullOrEmpty(cboAddCustomerID.Text))
                {


                    CustomerId = cboAddCustomerID.Text;
                    OrderId = txtAddOrderID.Text;

                    CustomerIDAdd?.Invoke(this, EventArgs.Empty);

                    ParentForm?.Close();


                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                CustomerIDAdd?.Invoke(this, EventArgs.Empty);

                ParentForm?.Close();
            }
        }
    }
}
