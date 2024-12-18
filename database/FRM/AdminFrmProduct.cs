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
    public partial class AdminFrmProduct : UserControl
    {
        SqlConnection Connection;
        private FunctionFrm functionFrm;

        private string selectedAdminIDProduct;
        private string selectedAdminNameProduct;
        private string selectedAdminPriceProduct;
        private string selectedAdminCategoryProduct;
        private string selectedAdminQuantityProduct;
        private string selectedAdminOriginProduct;
        private string selectedAdminDiscountProduct;
        private Form functionFormWrapper;
        public AdminFrmProduct()
        {
            InitializeComponent();
            Connection = new SqlConnection("Server=DELL-5430\\SQLEXPRESS;Database  = StoreX; Integrated Security = true;");
            if (functionFormWrapper != null)
            {
                FunctionFrm functionFrm = functionFormWrapper.Controls.OfType<FunctionFrm>().FirstOrDefault();
                if (functionFrm != null)
                {
                    functionFrm.EditButtonClicked += FillTextBoxWithSelectData;
                }
            }
        }

        private void FillTextBoxWithSelectData(object sender, EventArgs e)
        {


            cboFrmAddCategoryProduct.Text = selectedAdminCategoryProduct;
            txtFrmAddDiscountProduct.Text = selectedAdminDiscountProduct;
            txtFrmAddQuantityProduct.Text = selectedAdminQuantityProduct;
            txtFrmAddIdProduct.Text = selectedAdminIDProduct;
            txtFrmAddNameProduct.Text = selectedAdminNameProduct;
            cboFrmAddOriginProduct.Text = selectedAdminOriginProduct;
            txtFrmAddPriceProduct.Text = selectedAdminPriceProduct;

        }

        public void FillData()
        {
            String query = "SELECT " +
                "[ProductID] as \"     ID\"," +
                "[ProductNAME] as \"     Name\"," +
                "[PriceOfProducts] as \"     Price\"," +
                "[QuantityOfProducts] as \"     Quantity\"," +
                "[Category].CategoryNAME as \"     Category\"," +
                "[Supplies].SupplierNAME as \"     Origin\"," +
                "[DiscountOfProducts] as \"     Discount\" " +
                "FROM [StoreX].[dbo].[Product]" +
                "INNER JOIN [Supplies] ON [Product].[SupplierID] = [Supplies].[SupplierID]" +
                "INNER JOIN [Category] ON [Product].[CategoryID] = [Category].[CategoryID]"+
                "where Product.[ClearCheck] = 1;";
            DataTable tb = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(query, Connection);
            ad.Fill(tb);
            dgvwAdminFrmProduct.DataSource = tb;
            Connection.Close();
        }

        private void DeleteProduct()
        {
            if (string.IsNullOrEmpty(selectedAdminIDProduct))
            {
                MessageBox.Show("Please select an employee to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            DialogResult result = MessageBox.Show(
                $"product {selectedAdminNameProduct} will be out of business if you delete",
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

                                string updateClearCheckQuery = "UPDATE [StoreX].[dbo].[Product] SET ClearCheck = 0 WHERE ProductID = @ProductID;";

                                using (SqlCommand updateCmd = new SqlCommand(updateClearCheckQuery, conn, transaction))
                                {
                                    updateCmd.Parameters.AddWithValue("@ProductID", selectedAdminIDProduct);
                                    int rowsAffected = updateCmd.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        transaction.Commit();
                                        MessageBox.Show("Prodcut deleted successfully!", "Success",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        FillData();
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        MessageBox.Show("No Product was deleted. Please try again.", "Error",
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

        private void Account_frm_Load(object sender, EventArgs e)
        {
            Connection.Open();

            FillData();
            FillCategoryComboBox();
            FillOriginComboBox();
        }

        private void btn_ClearAdminProduct_Click(object sender, EventArgs e)
        {
            cboFrmAddCategoryProduct.Items.Clear();
            txtFrmAddDiscountProduct.Clear();
            txtFrmAddQuantityProduct.Clear();
            txtFrmAddIdProduct.Clear();
            txtFrmAddNameProduct.Clear();
            cboFrmAddOriginProduct.Items.Clear();
            txtFrmAddPriceProduct.Clear();
            FillCategoryComboBox();
            FillOriginComboBox();

        }

        private void btnSeaschBtnAdminProduct_Click(object sender, EventArgs e)
        {
            try
            {
                string searchValue = txtReaschBoxAdminProduct.Text.Trim();
                String baseQuery = "SELECT " +
                    "[ProductID] as \"     ID\"," +
                    "[ProductNAME] as \"     Name\"," +
                    "[PriceOfProducts] as \"     Price\"," +
                    "[QuantityOfProducts] as \"     Quantity\"," +
                    "[Category].CategoryNAME as \"     Category\"," +
                    "[Supplies].SupplierNAME as \"     Origin\"," +
                    "[DiscountOfProducts] as \"     Discount\" " +
                    "FROM [StoreX].[dbo].[Product]" +
                    "INNER JOIN [Supplies] ON [Product].[SupplierID] = [Supplies].[SupplierID]" +
                    "INNER JOIN [Category] ON [Product].[CategoryID] = [Category].[CategoryID] " +
                    "where Product.[ClearCheck] = 1 and ";
                string whereClause = "";
                if (btnReaschIDAdminProduct.Checked)
                {
                    whereClause = "[ProductID] = @searchValue";
                }
                else if (btnReaschNameAdminProduct.Checked)
                {
                    whereClause = "[ProductNAME] LIKE @searchValue + '%'";
                }
                else if (btnReaschPriceAdminProduct.Checked)
                {
                    whereClause = "[PriceOfProducts] LIKE @searchValue + '%'";
                }
                else if (btnReaschQuantityAdminProduct.Checked)
                {
                    whereClause = "[QuantityOfProducts] = @searchValue";
                }
                else if (btnReaschCategoryAdminProduct.Checked)
                {
                    whereClause = "[Category].CategoryNAME LIKE @searchValue + '%'";
                }
                else if (btnReaschOriginAdminProduct.Checked)
                {
                    whereClause = "[Supplies].SupplierNAME LIKE @searchValue + '%'";
                }
                else if (btnReaschDiscountAdminProduct.Checked)
                {
                    whereClause = "[DiscountOfProducts] LIKE @searchValue + '%'";
                }
                else
                {
                    whereClause = "[ProductNAME] LIKE @searchValue + '%'";
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
                    dgvwAdminFrmProduct.DataSource = dt;

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

        private void FillCategoryComboBox()
        {
            try
            {

                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }


                string categoryquery = "select CategoryName from [Category]";

                SqlCommand command = new SqlCommand(categoryquery, Connection);
                SqlDataReader reader = command.ExecuteReader();

   
                cboFrmAddCategoryProduct.Items.Clear();


                while (reader.Read())
                {
                    cboFrmAddCategoryProduct.Items.Add(reader["CategoryNAME"].ToString());

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

        private void FillOriginComboBox()
        {
            try
            {
       
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

        

                string originquery = "select SupplierNAME from [Supplies]";

                SqlCommand command = new SqlCommand(originquery, Connection);

                SqlDataReader reader = command.ExecuteReader();


       
                cboFrmAddOriginProduct.Items.Clear();



                while (reader.Read())
                {
                    cboFrmAddOriginProduct.Items.Add(reader["SupplierNAME"].ToString());

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

        private void btnResetBtnAdminProduct_Click(object sender, EventArgs e)
        {
            try
            {
                txtReaschBoxAdminProduct.Clear();
                btnReaschIDAdminProduct.Checked = false;
                btnReaschNameAdminProduct.Checked = false;
                btnReaschQuantityAdminProduct.Checked = false;
                btnReaschCategoryAdminProduct.Checked = false;
                btnReaschOriginAdminProduct.Checked = false;
                btnReaschDiscountAdminProduct.Checked = false;
                btnReaschPriceAdminProduct.Checked = false;


                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

  
                FillData();
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
        
        //_
        private void btn_AddAdminProduct_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFrmAddIdProduct.Text) ||
                string.IsNullOrEmpty(txtFrmAddNameProduct.Text) ||
                string.IsNullOrEmpty(txtFrmAddPriceProduct.Text) ||
                string.IsNullOrEmpty(cboFrmAddOriginProduct.Text) ||
                string.IsNullOrEmpty(txtFrmAddQuantityProduct.Text) ||
                string.IsNullOrEmpty(txtFrmAddDiscountProduct.Text) ||
                string.IsNullOrEmpty(cboFrmAddCategoryProduct.Text))
            {
                MessageBox.Show("Please fill in all information!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                Connection.Open();

                
                string checkQuery = "SELECT COUNT(*) FROM Product WHERE ProductID = @ProductID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, Connection);
                checkCmd.Parameters.AddWithValue("@ProductID", txtFrmAddIdProduct.Text);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                   
                    DialogResult result = MessageBox.Show(
                        "This Product id already exists in the database. Do you want to update?",
                        "Confirm",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        
                        UpdateProduct();
                    }
                }
                else
                {
                    
                    InsertProduct();
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

        private void dgvwAdminFrmProduct_MouseDown(object sender, MouseEventArgs e)
        {
            FillCategoryComboBox();
            FillOriginComboBox();
            if (dgvwAdminFrmProduct.CurrentRow != null)
            {
   
                selectedAdminIDProduct = dgvwAdminFrmProduct.CurrentRow.Cells["     ID"].Value?.ToString();
                selectedAdminNameProduct = dgvwAdminFrmProduct.CurrentRow.Cells["     Name"].Value?.ToString();
                selectedAdminPriceProduct = dgvwAdminFrmProduct.CurrentRow.Cells["     Price"].Value?.ToString();
                selectedAdminQuantityProduct = dgvwAdminFrmProduct.CurrentRow.Cells["     Quantity"].Value?.ToString();
                selectedAdminCategoryProduct = dgvwAdminFrmProduct.CurrentRow.Cells["     Category"].Value?.ToString();
                selectedAdminOriginProduct = dgvwAdminFrmProduct.CurrentRow.Cells["     Origin"].Value?.ToString();
                selectedAdminDiscountProduct = dgvwAdminFrmProduct.CurrentRow.Cells["     Discount"].Value?.ToString();
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
                    functionFrm.DeleteButtonClicked += (s, args) => DeleteProduct();
                    functionFrm.EditButtonClicked += FillTextBoxWithSelectData;
                    functionFormWrapper.Controls.Add(functionFrm);
                    FillCategoryComboBox();
                    FillOriginComboBox();

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

        private bool checkBussine()
        {
            string checkBuss = "SELECT ClearCheck FROM Product WHERE ProductID = @ProductID";
            SqlCommand checkCmd = new SqlCommand(checkBuss, Connection);
            checkCmd.Parameters.AddWithValue("@ProductID", txtFrmAddIdProduct.Text);

            using (SqlDataReader reader = checkCmd.ExecuteReader())
            {
                if (reader.Read()) 
                {
                    int clearCheckValue = Convert.ToInt32(reader["ClearCheck"]);

                    if (clearCheckValue == 1)
                    {
                        return true;
                    }
                    else if (clearCheckValue == 0)
                    {
                        MessageBox.Show("The product has been discontinued and you cannot update this product");
                        return false;
                    }
                }

            }
            return true;



        }


        private bool ValidateProduct()
        {
  
            float price;
            if (!float.TryParse(txtFrmAddPriceProduct.Text, out price))
            {
                MessageBox.Show("Invalid price format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (price < 0)
            {
                MessageBox.Show("Product price must be greater than or equal to 0!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            float quantity;
            if (!float.TryParse(txtFrmAddQuantityProduct.Text, out quantity))
            {
                MessageBox.Show("Invalid quantity format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (quantity < 0)
            {
                MessageBox.Show("Product quantity must be greater than or equal to 0!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            float discount;
            if (!float.TryParse(txtFrmAddDiscountProduct.Text, out discount))
            {
                MessageBox.Show("Invalid discount format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (discount < 0 || discount > 100)
            {
                MessageBox.Show("Discount must be between 0 and 100!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            

            return true;
        }


        private void InsertProduct()
        {
            if (!ValidateProduct())
            {
                return;
            }

            string insertQuery = @"INSERT INTO [Product] 
                    VALUES (@ProductID,(SELECT SupplierID FROM [dbo].[Supplies] WHERE SupplierNAME = @Origin),@ProductName,@Price,@Quantity,(SELECT CategoryID FROM [Category] WHERE CategoryNAME = @Category),@Discount, 1)";

            using (SqlCommand cmd = new SqlCommand(insertQuery, Connection))
            {
                cmd.Parameters.AddWithValue("@ProductID", txtFrmAddIdProduct.Text);
                cmd.Parameters.AddWithValue("@ProductName", txtFrmAddNameProduct.Text);
                cmd.Parameters.AddWithValue("@Price", txtFrmAddPriceProduct.Text);
                cmd.Parameters.AddWithValue("@Quantity", txtFrmAddQuantityProduct.Text);
                cmd.Parameters.AddWithValue("@Discount", txtFrmAddDiscountProduct.Text);
                cmd.Parameters.AddWithValue("@Category", cboFrmAddCategoryProduct.Text);
                cmd.Parameters.AddWithValue("@Origin", cboFrmAddOriginProduct.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Added Prodcut successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            cboFrmAddCategoryProduct.Items.Clear();
            cboFrmAddOriginProduct.Items.Clear();
            txtFrmAddDiscountProduct.Clear();
            txtFrmAddQuantityProduct.Clear();
            txtFrmAddPriceProduct.Clear();
            txtFrmAddNameProduct.Clear();
            txtFrmAddIdProduct.Clear();
        }

        private void UpdateProduct()
        {

            if (!ValidateProduct())
            {
                return;
            }

            if (!checkBussine())
            {
                return ;
            }

            string updateQuery = @"UPDATE [Product] SET 
            
            [SupplierID] = (SELECT SupplierID FROM [dbo].[Supplies] WHERE SupplierNAME = @Origin), 
            [ProductNAME] = @ProductName, 
            [PriceOfProducts] = @Price, 
            [QuantityOfProducts] = @Quantity, 
            [CategoryID] = (SELECT CategoryID FROM [Category] WHERE CategoryNAME = @Category), 
            [DiscountOfProducts] = @Discount
            WHERE ProductID = @ProductID";

            using (SqlCommand cmd = new SqlCommand(updateQuery, Connection))
            {
                cmd.Parameters.AddWithValue("@ProductID", txtFrmAddIdProduct.Text);
                cmd.Parameters.AddWithValue("@ProductName", txtFrmAddNameProduct.Text);
                cmd.Parameters.AddWithValue("@Price", txtFrmAddPriceProduct.Text);
                cmd.Parameters.AddWithValue("@Quantity", txtFrmAddQuantityProduct.Text);
                cmd.Parameters.AddWithValue("@Discount", txtFrmAddDiscountProduct.Text);
                cmd.Parameters.AddWithValue("@Category", cboFrmAddCategoryProduct.Text);
                cmd.Parameters.AddWithValue("@Origin", cboFrmAddOriginProduct.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Employee updated successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            cboFrmAddCategoryProduct.Items.Clear();
            cboFrmAddOriginProduct.Items.Clear();
            txtFrmAddDiscountProduct.Clear();
            txtFrmAddQuantityProduct.Clear();
            txtFrmAddPriceProduct.Clear();
            txtFrmAddNameProduct.Clear();
            txtFrmAddIdProduct.Clear();
        }
        
    }
}
