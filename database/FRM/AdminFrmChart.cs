using iTextSharp.text.pdf;
using iTextSharp.text;
using OfficeOpenXml;
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
    public partial class AdminFrmChart : UserControl
    {
        SqlConnection Connection;
        public AdminFrmChart()
        {
            InitializeComponent();
            Connection = new SqlConnection("Server=DELL-5430\\SQLEXPRESS;Database  = StoreX; Integrated Security = true;");
        }

        private void AdminFrmChart_Load(object sender, EventArgs e)
        {
            Connection.Open();
            Filldata();
        }

        

        private void btnReaschday_CheckedChanged(object sender, EventArgs e)
        {

            DateTimePicker1.Visible = btnApplyUpdateChartDay.Checked;
            DateTimePicker2.Visible = btnApplyUpdateChartDay.Checked;

        }

        private void btnApplyUpdateChart_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = DateTimePicker1.Value;
                DateTime endDate = DateTimePicker2.Value;

                string formattedStartDate = startDate.ToString("yyyy-MM-dd");
                string formattedEndDate = endDate.ToString("yyyy-MM-dd");

                String charProduct = "SELECT " +
                    "\r\n    Product.ProductID," +
                    "\r\n    Product.ProductNAME," +
                    "\r\n    SUM(OrderDetails.QuantityOfOrder) AS TotalQuantity," +
                    "\r\n    SUM(OrderDetails.QuantityOfOrder * OrderDetails.Price) AS TotalPrice," +
                    "\r\n    MAX(Orders.OrderDATE) AS OrderDATE" +
                    "\r\nFROM " +
                    "\r\n    Product" +
                    "\r\nJOIN " +
                    "\r\n    OrderDetails" +
                    "\r\nON " +
                    "\r\n    Product.ProductID = OrderDetails.ProductID" +
                    "\r\nJOIN " +
                    "\r\n    Orders" +
                    "\r\nON " +
                    "\r\n    OrderDetails.OrderID = Orders.OrderID";

                String charEmployess = "SELECT " +
                    "\r\n    Employees.EmployeeID," +
                    "\r\n    Employees.EmployeeNAME," +
                    "\r\n    COUNT(DISTINCT Orders.OrderID) AS OrderCount," +
                    "\r\n    SUM(OrderDetails.QuantityOfOrder * OrderDetails.Price) AS TotalOrderValue," +
                    "\r\n    MAX(Orders.OrderDATE) AS EarliestOrderTime" +
                    "\r\nFROM " +
                    "\r\n    Employees" +
                    "\r\nLEFT JOIN " +
                    "\r\n    Orders" +
                    "\r\nON " +
                    "\r\n    Employees.EmployeeID = Orders.EmployeeID" +
                    "\r\nLEFT JOIN " +
                    "\r\n    OrderDetails" +
                    "\r\nON " +
                    "\r\n    Orders.OrderID = OrderDetails.OrderID";

                String charCustomer = "SELECT " +
                    "\r\n    Customers.CustomerID," +
                    "\r\n    Customers.CustumerNAME," +
                    "\r\n    COUNT(DISTINCT Orders.OrderID) AS OrderCount," +
                    "\r\n    SUM(OrderDetails.QuantityOfOrder * OrderDetails.Price) AS TotalOrderValue," +
                    "\r\n    MAX(Orders.OrderDATE) AS EarliestOrderTime" +
                    "\r\nFROM " +
                    "\r\n    Customers" +
                    "\r\nLEFT JOIN " +
                    "\r\n    Orders" +
                    "\r\nON " +
                    "\r\n    Customers.CustomerID = Orders.CustomerID" +
                    "\r\nLEFT JOIN " +
                    "\r\n    OrderDetails" +
                    "\r\nON " +
                    "\r\n    Orders.OrderID = OrderDetails.OrderID";


                string whereClause = "";
                string esleTimeQuery = "";


                string lastQueryCharProduct = " GROUP BY " +
                    "\r\n    Product.ProductID, Product.ProductNAME" +
                    "\r\nORDER BY " +
                    "\r\n    Product.ProductID;";

                string lastQueryCharCustomer = " GROUP BY " +
                    "\r\n    Customers.CustomerID, Customers.CustumerNAME" +
                    "\r\nORDER BY " +
                    "\r\n    Customers.CustomerID;";

                string lastQueryCharEmployess = " GROUP BY " +
                    "\r\n    Employees.EmployeeID, Employees.EmployeeNAME" +
                    "\r\nORDER BY " +
                    "\r\n    Employees.EmployeeID;";

                if (btnApplyUpdateChartDay.Checked)
                {
                    esleTimeQuery = " WHERE Orders.OrderDATE BETWEEN @formattedStartDate AND @formattedEndDate";

                }



                if (btnApplyUpdateChartProduct.Checked)
                {
                    whereClause = charProduct + esleTimeQuery + lastQueryCharProduct;
                }
                else if (btnApplyUpdateCharEmployess.Checked)
                {
                    whereClause = charEmployess + esleTimeQuery + lastQueryCharEmployess;
                }
                else if (btnApplyUpdateChartCustomer.Checked)
                {
                    whereClause = charCustomer + esleTimeQuery + lastQueryCharCustomer;
                }

                else
                {
                    whereClause = charProduct + esleTimeQuery + lastQueryCharProduct;

                }




                string finalQuery = whereClause;
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                using (SqlCommand cmd = new SqlCommand(finalQuery, Connection))
                {

                    cmd.Parameters.AddWithValue("@formattedEndDate", formattedEndDate);
                    cmd.Parameters.AddWithValue("@formattedStartDate", formattedStartDate);




                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    dgvwChartUpdate.DataSource = dt;

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


        private void Filldata()
        {
            String query = "SELECT " +
                    "\r\n    Product.ProductID," +
                    "\r\n    Product.ProductNAME," +
                    "\r\n    SUM(OrderDetails.QuantityOfOrder) AS TotalQuantity," +
                    "\r\n    SUM(OrderDetails.QuantityOfOrder * OrderDetails.Price) AS TotalPrice," +
                    "\r\n    Max(Orders.OrderDATE) AS OrderDATE" +
                    "\r\nFROM " +
                    "\r\n    Product" +
                    "\r\nJOIN " +
                    "\r\n    OrderDetails" +
                    "\r\nON " +
                    "\r\n    Product.ProductID = OrderDetails.ProductID" +
                    "\r\nJOIN " +
                    "\r\n    Orders" +
                    "\r\nON " +
                    "\r\n    OrderDetails.OrderID = Orders.OrderID";

            string queryProduct = " GROUP BY " +
                    "\r\n    Product.ProductID, Product.ProductNAME" +
                    "\r\nORDER BY " +
                    "\r\n    Product.ProductID;";

            string finalquery = query + queryProduct;

            DataTable tb = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(finalquery, Connection);
            ad.Fill(tb);
            dgvwChartUpdate.DataSource = tb;
            Connection.Close();
        }

        private void btnResetChart_Click(object sender, EventArgs e)
        {
            btnApplyUpdateCharEmployess.Checked = false;
            btnApplyUpdateChartCustomer.Checked = false;
            btnApplyUpdateChartProduct.Checked = false;
            btnApplyUpdateChartDay.Checked = false;
            Filldata();
        }

        private void btnExportExcelChart_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvwChartUpdate.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel Files|*.xlsx";
                    saveFileDialog.Title = "Save file Excel";
                    saveFileDialog.FileName = "ExportData.xlsx";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;
                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                        using (var package = new OfficeOpenXml.ExcelPackage())
                        {
                            var worksheet = package.Workbook.Worksheets.Add("DataExport");

                            for (int col = 0; col < dgvwChartUpdate.Columns.Count; col++)
                            {
                                worksheet.Cells[1, col + 1].Value = dgvwChartUpdate.Columns[col].HeaderText;
                                worksheet.Cells[1, col + 1].Style.Font.Bold = true;
                            }

                            for (int row = 0; row < dgvwChartUpdate.Rows.Count; row++)
                            {
                                for (int col = 0; col < dgvwChartUpdate.Columns.Count; col++)
                                {
                                    worksheet.Cells[row + 2, col + 1].Value = dgvwChartUpdate.Rows[row].Cells[col].Value?.ToString() ?? "";
                                }
                            }

                            package.SaveAs(new FileInfo(filePath));
                        }

                        MessageBox.Show("Data export successful!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while exporting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportpdfChart_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvwChartUpdate.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF Files|*.pdf";
                    saveFileDialog.Title = "Save file PDF";
                    saveFileDialog.FileName = "ExportData.pdf";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;

                        using (var document = new iTextSharp.text.Document())
                        {
                            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                            document.Open();

                            var titleFont = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD);
                            document.Add(new iTextSharp.text.Paragraph("Data Report", titleFont) { Alignment = iTextSharp.text.Element.ALIGN_CENTER });
                            document.Add(new iTextSharp.text.Paragraph(" ")); 


                            PdfPTable table = new PdfPTable(dgvwChartUpdate.Columns.Count);


                            for (int i = 0; i < dgvwChartUpdate.Columns.Count; i++)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(dgvwChartUpdate.Columns[i].HeaderText))
                                {
                                    BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY,
                                    HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
                                };
                                table.AddCell(cell);
                            }


                            for (int i = 0; i < dgvwChartUpdate.Rows.Count; i++)
                            {
                                for (int j = 0; j < dgvwChartUpdate.Columns.Count; j++)
                                {
                                    table.AddCell(dgvwChartUpdate.Rows[i].Cells[j].Value?.ToString() ?? "");
                                }
                            }


                            document.Add(table);
                        }

                        MessageBox.Show("Data export successful!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while exporting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
