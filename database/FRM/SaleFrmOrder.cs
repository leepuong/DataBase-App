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

namespace database.FRM
{
    public partial class SaleOrder : UserControl
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
        private string currentOrderID;

        SqlConnection Connection;
        public SaleOrder()
        {
            InitializeComponent();

            Connection = new SqlConnection("Server=DELL-5430\\SQLEXPRESS;Database  = StoreX; Integrated Security = true;");
        }
        public void FillData()
        {
            String query = "SELECT TOP (1000) [OrderID] as '     #Oreder'" +
                "\r\n      ,[CustomerID] as '    #-C'" +
                "\r\n      ,[EmployeeID] as '     #-S'" +
                "\r\n      ,[OrderDATE] as '     Day'" +
                "\r\n      ,[ShippingADDRESS] as '     Ship to'" +
                "\r\n  FROM [StoreX].[dbo].[Orders]";
            DataTable tb = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(query, Connection);
            ad.Fill(tb);
            dgvwSaleFrmOrder.DataSource = tb;
            Connection.Close();
        }

        private void SaleOrder_Load(object sender, EventArgs e)
        {
            Connection.Open();
            FillData();
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
        }

        private void dgvwSaleFrmOrder_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hitTest = dgvwSaleFrmOrder.HitTest(e.X, e.Y);

            if (hitTest.Type == DataGridViewHitTestType.Cell)
            {
                if (hitTest.RowIndex >= 0 && hitTest.RowIndex < dgvwSaleFrmOrder.Rows.Count)
                {
                    dgvwSaleFrmOrder.Rows[hitTest.RowIndex].Selected = true;

                    currentOrderID = dgvwSaleFrmOrder.Rows[hitTest.RowIndex].Cells["     #Oreder"].Value.ToString();



                }
            }
        }


        private void btnReaschday_CheckedChanged(object sender, EventArgs e)
        {
            DateTimePicker1.Visible = btnReaschday.Checked;
            DateTimePicker2.Visible = btnReaschday.Checked;
        }

        private void btnSeaschBtnAdminCus_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = DateTimePicker1.Value;
                DateTime endDate = DateTimePicker2.Value;

                string formattedStartDate = startDate.ToString("yyyy-MM-dd");
                string formattedEndDate = endDate.ToString("yyyy-MM-dd");
                string searchValue = txtReaschOrder.Text.Trim();
                String baseQuery = "SELECT TOP (1000) [OrderID] as '     #Oreder'" +
                "\r\n      ,[CustomerID] as '    #-C'" +
                "\r\n      ,[EmployeeID] as '     #-S'" +
                "\r\n      ,[OrderDATE] as '     Day'" +
                "\r\n      ,[ShippingADDRESS] as '     Ship to'" +
                "\r\n  FROM [StoreX].[dbo].[Orders]" + " where ";
                string whereClause = "";

                if (btnReaschIDOrder.Checked)
                {
                    whereClause = "[OrderID] = @searchValue";
                }
                else if (btnReaschIDCustomer.Checked)
                {
                    whereClause = "[CustomerID] = @searchValue";
                }
                else if (btnReaschIDSaler.Checked)
                {
                    whereClause = "[EmployeeID] = @searchValue";
                }
                else if (btnReaschAddressOrder.Checked)
                {
                    whereClause = "[ShippingADDRESS] LIKE @searchValue + '%'";
                }
                else
                {
                    whereClause = "[OrderID] = @searchValue";

                }

                if (btnReaschday.Checked)
                {
                    whereClause += " and ([OrderDATE] BETWEEN @formattedStartDate AND @formattedEndDate)";
                    
                }


                string finalQuery = baseQuery + whereClause;
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                using (SqlCommand cmd = new SqlCommand(finalQuery, Connection))
                {
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@formattedEndDate", formattedEndDate);
                    cmd.Parameters.AddWithValue("@formattedStartDate", formattedStartDate);




                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    dgvwSaleFrmOrder.DataSource = dt;

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
            txtReaschOrder.Clear();
            btnReaschday.Checked = false;
            btnReaschIDOrder.Checked = false;
            btnReaschIDSaler.Checked = false;
            btnReaschIDCustomer.Checked = false;


            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }

            FillData();
        }
    }
}
