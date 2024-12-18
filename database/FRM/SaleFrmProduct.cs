using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace database.FRM
{
    public partial class SaleProduct : UserControl
    {


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        SqlConnection Connection;

        private string selectedProductID;
        private decimal selectedProductPrice;
        public string currentOrderID;
        public String currentCustomerID;
        private Label lbl_totalBill;

        string currentEmployeeID = GlobalVariables.CurrentEmployeeID;
        private string nameCustomer;
        private int idCustomer;

        public SaleProduct()
        {
            InitializeComponent();
            dgvwOrderDetailTable.MouseDoubleClick += dgvwOrderDetailTable_MouseDoubleClick;
            Connection = new SqlConnection("Server=DELL-5430\\SQLEXPRESS;Database  = StoreX; Integrated Security = true;");
        }

        




        public void FillData()
        {
            String query = "Select [Product].[ProductID] as '     #'," +
                "\r\n  [Product].[ProductNAME] as '   Name'," +
                "\r\n  [Product].[PriceOfProducts] as '    Price'," +
                "\r\n  [Product].[QuantityOfProducts] as '    Quantity'," +
                "\r\n  [Product].[DiscountOfProducts] as '    Discount'," +
                "\r\n  [Supplies].SupplierNAME as '    Country'," +
                "\r\n  [Category].CategoryNAME as '    Category' " +
                "\r\n  from [Product]" +
                "\r\n  JOIN [dbo].[Category]" +
                "\r\n  on [Product].[CategoryID] = [Category].CategoryID" +
                "\r\n  JOIN  [dbo].[Supplies]" +
                "\r\n  on [Product].[SupplierID] = [Supplies].SupplierID" +
                "\r\n  where Product.[ClearCheck] = 1;" ;
            DataTable tb = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(query, Connection);
            ad.Fill(tb);
            dgvwSaleFrmProduct.DataSource = tb;
            Connection.Close();
        }

        private void Product_frm_Load(object sender, EventArgs e)
        {
            Connection.Open();
            FillData();
            disableDataNull();
            login_frm loginApp = new login_frm();
            loginApp.UserPassAdded += (s, ev) =>
            {
                String User = loginApp.usernameLog;
                String Passr = loginApp.passwordLog;

                

            };

        }

        private void dgvwSaleFrmProduct_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hitTest = dgvwSaleFrmProduct.HitTest(e.X, e.Y);

            if (hitTest.Type == DataGridViewHitTestType.Cell)
            {
                if (hitTest.RowIndex >= 0 && hitTest.RowIndex < dgvwSaleFrmProduct.Rows.Count)
                {
                    dgvwSaleFrmProduct.Rows[hitTest.RowIndex].Selected = true;

                    selectedProductID = dgvwSaleFrmProduct.Rows[hitTest.RowIndex].Cells["     #"].Value.ToString();
                    selectedProductPrice = Convert.ToDecimal(dgvwSaleFrmProduct.Rows[hitTest.RowIndex].Cells["    Price"].Value);
                    

                    ShowQuantityPopup();

                }
            }
        }



        private void dgvwOrderDetailTable_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hitTest = dgvwOrderDetailTable.HitTest(e.X, e.Y);
            if (hitTest.Type == DataGridViewHitTestType.Cell)
            {
                if (hitTest.RowIndex >= 0 && hitTest.RowIndex < dgvwOrderDetailTable.Rows.Count)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm delete",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            if (Connection.State != ConnectionState.Open)
                                Connection.Open();

                            string orderID = dgvwOrderDetailTable.Rows[hitTest.RowIndex].Cells["#"].Value.ToString();
                            string productName = dgvwOrderDetailTable.Rows[hitTest.RowIndex].Cells["Name"].Value.ToString();
                            string quantity = dgvwOrderDetailTable.Rows[hitTest.RowIndex].Cells["Quantity"].Value.ToString();
                            string price = dgvwOrderDetailTable.Rows[hitTest.RowIndex].Cells["Price"].Value.ToString();

                            string deleteQuery = @"
                                DELETE FROM [OrderDetails]
                                WHERE [OrderID] = @OrderID
                                AND [ProductID] = (
                                    SELECT [ProductID]
                                    FROM [Product]
                                    WHERE [ProductNAME] = @ProductName
                                )
                                AND [QuantityOfOrder] = @Quantity
                                AND [Price] = @Price;";

                            using (SqlCommand cmd = new SqlCommand(deleteQuery, Connection))
                            {
                                cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = int.Parse(orderID);
                                cmd.Parameters.Add("@ProductName", SqlDbType.VarChar).Value = productName;
                                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = int.Parse(quantity);
                                cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = decimal.Parse(price);

                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Product removed from order successfully.");

                                    UpdateOrderDetailsGridView();
                                }
                                else
                                {
                                    MessageBox.Show("Cannot delete product.");

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error deleting order details: " + ex.Message);
                        }
                        finally
                        {
                            if (Connection.State == ConnectionState.Open)
                                Connection.Close();
                        }
                    }
                }
            }
        }




        private void btnEnabledDgvwOrder_Click(object sender, EventArgs e)
        {
            dgvwOrderDetailTable.Enabled = true;
            btnEnabledDgvwOrder.Visible = false;

            ShowCustomerPopup();
            


            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                
                nameCustomer = currentCustomerID.Substring(0, currentCustomerID.IndexOf("ID:")).Trim();
                idCustomer = int.Parse(currentCustomerID.Substring(currentCustomerID.IndexOf(":") + 1).Trim());


                


                string checkIDorder = @"select OrderID from Orders where OrderID = @currentOrderID";

                int cout = 0;
                                                
                using (SqlCommand cmd = new SqlCommand(checkIDorder, Connection))
                {
                    cmd.Parameters.Add("@currentOrderID", SqlDbType.Int).Value = int.Parse(currentOrderID);
                                                      
                    var result = cmd.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        cout = (int)result;
                    }

                    if (cout > 0)
                    {
                        disableDataNull();
                        MessageBox.Show("This ID already exists, please try again.-----");
                        return;
                    }
                    else
                    {
                        
                        insertOrderID();
                    };


                }
                              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking orderID: " + ex.Message);
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

        }

        public void insertOrderID()
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                string createOrder = @"INSERT INTO [dbo].[Orders] (OrderID, CustomerID, OrderDATE, EmployeeID, ShippingADDRESS)
                                    VALUES (
                                    @currentOrderID,
                                    (SELECT top 1 CustomerID
                                    FROM [Customers]
                                    WHERE (CustumerNAME = @nameCustomer) and (CustomerID = @idCustomer)),
                                    (SELECT GETDATE()),
                                    @currentEmployeeID,
                                    (SELECT top 1 [AddressOfCustomers] FROM [Customers] where [CustomerID] = @idCustomer)
                                    );";

                using (SqlCommand cmd = new SqlCommand(createOrder, Connection))
                {
                    cmd.Parameters.Add("@currentOrderID", SqlDbType.Int).Value = int.Parse(currentOrderID);
                    cmd.Parameters.Add("@currentEmployeeID", SqlDbType.Int).Value = int.Parse(currentEmployeeID);
                    cmd.Parameters.Add("@nameCustomer", SqlDbType.VarChar).Value = (nameCustomer);
                    cmd.Parameters.Add("@idCustomer", SqlDbType.Int).Value = int.Parse(idCustomer.ToString());
                    cmd.ExecuteNonQuery();

                }

                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error insert orderID  : " + ex.Message);
                disableDataNull();

            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                
            }

        }
        public void ShowQuantityPopup()
        {

            Form popupForm = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Size = new Size(228, 50),
                BackColor = Color.FromArgb(49, 49, 60)
            };

            FrmControlQuantityProduct controlQuantityProduct = new FrmControlQuantityProduct();
            controlQuantityProduct.Dock = DockStyle.Fill;
            popupForm.Controls.Add(controlQuantityProduct);

            
            controlQuantityProduct.QuantityAdded += (s, ev) =>
            {
                    string quantity = controlQuantityProduct.Quantity;
                int quantitycheckValue = int.Parse(quantity);

                if (!Checkquantity(quantitycheckValue))
                {
                    if (Connection.State == ConnectionState.Open)
                        Connection.Close();
                    return;                
                }
                
                    try
                    {

                        if (Connection.State != ConnectionState.Open)
                            Connection.Open();

                        string insertQuery = @"
                            INSERT INTO [OrderDetails] 
                            (OrderID, ProductID, QuantityOfOrder, Price) 
                            VALUES (@OrderID, @ProductID, @Quantity, @Price)";
                    
                    
                        using (SqlCommand cmd = new SqlCommand(insertQuery, Connection))
                        {
                            cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = int.Parse(currentOrderID);
                            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = int.Parse(selectedProductID);
                            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = int.Parse(quantity);
                            cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = selectedProductPrice;
                            


                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"Added {quantity} product to order {currentOrderID}");
                                UpdateOrderDetailsGridView();
                                popupForm.Close();
                            }
                            else
                            {
                                MessageBox.Show("Cannot add product to order");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating order details: " + ex.Message);
                    }
                    finally
                    {
                        if (Connection.State == ConnectionState.Open)
                            Connection.Close();
                    }
                
            };

            this.Enabled = false;
            popupForm.FormClosed += (s, ev) => this.Enabled = true;

            popupForm.ShowDialog();
        }



        private bool Checkquantity(int quantity)
        {

            if (quantity <= 0) {
                MessageBox.Show("Product quantity must be positive");
                return false;
            };

            if (Connection.State != ConnectionState.Open)
                Connection.Open();
            string checkQuan = "SELECT [QuantityOfProducts] FROM Product WHERE ProductID = @ProductID";
            SqlCommand checkCmd = new SqlCommand(checkQuan, Connection);
            checkCmd.Parameters.AddWithValue("@ProductID", selectedProductID);
            using (SqlDataReader reader = checkCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    int QuantityValue = Convert.ToInt32(reader["QuantityOfProducts"]);

                    if (QuantityValue > quantity)
                    {

                        return true;
                    }
                    else 
                    {
                        MessageBox.Show("Insufficient quantity of products in stock");
                        return false;
                    }
                }

            }
            return true;
        }


        public void ShowCustomerPopup()
        {


            Form popupForm1 = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                Size = new Size(228, 100),
                BackColor = Color.FromArgb(49, 49, 60)
            };

            FrmControlCustomerID controlCustomerID = new FrmControlCustomerID();
            popupForm1.Controls.Add(controlCustomerID);

            controlCustomerID.CustomerIDAdd += (s, ev) =>
            {

                try
                {
                    currentCustomerID = controlCustomerID.CustomerId;
                    currentOrderID = controlCustomerID.OrderId;

                    popupForm1.Close();
                    


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating Customer id: " + ex.Message);
                }
                finally
                {
                    if (Connection.State == ConnectionState.Open)
                        Connection.Close();
                }
                
            };

            this.Enabled = false;
            popupForm1.FormClosed += (s, ev) => this.Enabled = true;

            popupForm1.ShowDialog();
        }


        private void UpdateOrderDetailsGridView()
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                string query = "SELECT [OrderID] as '#', [Product].ProductID as '#P', [Product].ProductNAME as 'Name', [QuantityOfOrder] as 'Quantity', [Price] as 'Price' " +
                               "FROM [StoreX].[dbo].[OrderDetails] " +
                               "INNER JOIN Product on [OrderDetails].[ProductID]=[dbo].[Product].[ProductID]" +
                               (currentOrderID != null ? "WHERE OrderID = @OrderID" : "WHERE OrderID IS NULL");

                using (SqlCommand cmd = new SqlCommand(query, Connection))
                {
                    if (currentOrderID != null)
                    {
                        cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = int.Parse(currentOrderID);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvwOrderDetailTable.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating order details: " + ex.Message);
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
        }


        private void disableDataNull()
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                string query = "SELECT [OrderID] as '#',[Product].ProductID as '#P', [Product].ProductNAME as 'Name', [QuantityOfOrder] as 'Quantity', [Price] as 'Price' " +
                               "FROM [StoreX].[dbo].[OrderDetails] " +
                               "INNER JOIN Product on [OrderDetails].[ProductID]=[dbo].[Product].[ProductID]" +
                               "WHERE OrderID IS NULL";

                using (SqlCommand cmd = new SqlCommand(query, Connection))
                {


                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvwOrderDetailTable.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating order details: " + ex.Message);
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            dgvwOrderDetailTable.Enabled = false;
            btnEnabledDgvwOrder.Visible = true;
        }

        private void btnCancleOrderSaleProduct_Click(object sender, EventArgs e)
        {
            

            try
            {
                

                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                string insertQuery = @"
                            delete from Orders
                            where OrderID = @OrderID";


                using (SqlCommand cmd = new SqlCommand(insertQuery, Connection))
                {
                    cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = int.Parse(currentOrderID);
                    
                    int rowsAffected = cmd.ExecuteNonQuery();

                    
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating order details: " + ex.Message);
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            disableDataNull();

        }

        private void btnCreateaOrderSaleProduct_Click(object sender, EventArgs e)
        {
            Form billForm = new Form
            {
                Text = "Bill",
                Size = new System.Drawing.Size(520, 460),
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true,
                Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20)),
                FormBorderStyle = FormBorderStyle.None
            };

            FrmBill frmBill = new FrmBill
            {
                Dock = DockStyle.Fill
            };
            frmBill.FillOrderDetailsToGridView(currentOrderID);

            frmBill.CalculateTotalBill(currentOrderID);

            frmBill.Tag = billForm;
            billForm.Controls.Add(frmBill);
            billForm.Show();

            int i = 0;
            foreach (DataGridViewRow row in dgvwOrderDetailTable.Rows)
            {
                if (row.Cells["Quantity"]?.Value != null)
                {
                    int productId = Convert.ToInt32(row.Cells["#P"].Value);
                    int quantityOrdered = Convert.ToInt32(row.Cells["Quantity"].Value);
                    

                    try
                    {
                        if (Connection.State != ConnectionState.Open)
                            Connection.Open();

                        
                        string checkStockQuery = "SELECT QuantityOfProducts FROM Product WHERE ProductID = @ProductID";
                        using (SqlCommand cmd = new SqlCommand(checkStockQuery, Connection))
                        {
                            cmd.Parameters.AddWithValue("@ProductID", productId);
                            object result = cmd.ExecuteScalar();

                            if (result != null && int.TryParse(result.ToString(), out int currentStock))
                            {
                                if (currentStock >= quantityOrdered)
                                {
                                    
                                    string updateStockQuery = "UPDATE Product SET QuantityOfProducts = QuantityOfProducts - @Quantity WHERE ProductID = @ProductID";
                                    using (SqlCommand updateCmd = new SqlCommand(updateStockQuery, Connection))
                                    {
                                        updateCmd.Parameters.AddWithValue("@Quantity", quantityOrdered);
                                        updateCmd.Parameters.AddWithValue("@ProductID", productId);
                                        updateCmd.ExecuteNonQuery();
                                    }

                                }
                                else
                                {
                                    MessageBox.Show($"Product ID {productId} is not in stock in sufficient quantity. Current stock: {currentStock}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show($"No product found with ID {productId}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return; 
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error while checking and updating inventory: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    finally
                    {
                        if (Connection.State == ConnectionState.Open)
                            Connection.Close();
                    }
                }
            }

            disableDataNull();



        }

        private void btnResetBtnSalePro_Click(object sender, EventArgs e)
        {
            txtReaschProductSale.Clear();
            btnReaschCategoryProduct.Checked = false;
            btnReaschCountryProduct.Checked = false;
            btnReaschIDProduct.Checked = false;
            btnReaschPriceProduct.Checked = false;
            btnReaschNameProduct.Checked = false;


            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }

            FillData();
        }

        private void btnSeaschBtnSalePro_Click(object sender, EventArgs e)
        {
            try
            {
                string searchValue = txtReaschProductSale.Text.Trim();
                String baseQuery = "SELECT " +
                    "[ProductID] as \"     #\"," +
                    "[ProductNAME] as \"     Name\"," +
                    "[PriceOfProducts] as \"    Price\"," +
                    "[QuantityOfProducts] as \"     Quantity\"," +
                    "[Category].CategoryNAME as \"     Category\"," +
                    "[Supplies].SupplierNAME as \"     Origin\"," +
                    "[DiscountOfProducts] as \"     Discount\" " +
                    "FROM [StoreX].[dbo].[Product]" +

                    "INNER JOIN [Supplies] ON [Product].[SupplierID] = [Supplies].[SupplierID]" +
                    "INNER JOIN [Category] ON [Product].[CategoryID] = [Category].[CategoryID] " +
                    "where Product.[ClearCheck] = 1 and "; 
                string whereClause = "";

                if (btnReaschIDProduct.Checked)
                {
                    whereClause = "[ProductID] = @searchValue";
                }
                else if (btnReaschNameProduct.Checked)
                {
                    whereClause = "[ProductNAME] LIKE @searchValue + '%'";
                }
                else if (btnReaschPriceProduct.Checked)
                {
                    whereClause = "[PriceOfProducts] LIKE @searchValue + '%'";
                }
                else if (btnReaschCategoryProduct.Checked)
                {
                    whereClause = "[Category].CategoryID = (SELECT TOP (1) [CategoryID]  FROM [StoreX].[dbo].[Category]  where [CategoryNAME] like @searchValue + '%')";
                }
                else if (btnReaschCountryProduct.Checked)
                {
                    whereClause = "[Supplies].SupplierID = (SELECT TOP (1) [SupplierID]  FROM [StoreX].[dbo].[Supplies]  where [SupplierNAME] like @searchValue + '%')";
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
                    dgvwSaleFrmProduct.DataSource = dt;

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
