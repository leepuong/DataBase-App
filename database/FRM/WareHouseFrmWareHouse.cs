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
using iTextSharp.text.pdf;
using iTextSharp.text;
using OfficeOpenXml;
using System.IO;

namespace database.FRM
{
    public partial class WareHouseWareHouse : UserControl
    {
        SqlConnection Connection;

        public WareHouseWareHouse()
        {
            InitializeComponent();
            Connection = new SqlConnection("Server=DELL-5430\\SQLEXPRESS;Database  = StoreX; Integrated Security = true;");

        }

        public void FillData()
        {
            String query = " Select [Product].[ProductID] as '    #'," +
                "\r\n  [Product].[ProductNAME] as '   Name'," +
                "\r\n  [Product].[PriceOfProducts] as '    Price'," +
                "\r\n  [Product].[QuantityOfProducts] as '    Quantity'," +
                "\r\n  [Product].[DiscountOfProducts] as '    Discount'," +
                "\r\n  [Supplies].SupplierNAME as '    Country'," +
                "\r\n  [Category].CategoryNAME as '    Category'," +
                "\r\n  CASE WHEN Product.[ClearCheck] = 0 THEN 'Out of Business'" +
                "\r\n  WHEN Product.[ClearCheck] = 1 THEN 'business'" +
                "\r\n  ELSE NULL" +
                "\r\n  END AS 'Status'" +
                "\r\n  from [Product]" +
                "\r\n  JOIN [dbo].[Category] on [Product].[CategoryID] = [Category].CategoryID" +
                "\r\n  join  [dbo].[Supplies]  on [Product].[SupplierID] = [Supplies].SupplierID;";
            DataTable tb = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(query, Connection);
            ad.Fill(tb);
            dgvwWareFrmWareHouse.DataSource = tb;
            Connection.Close();

        }

        private void WareHouseWareHouse_Load(object sender, EventArgs e)
        {
            Connection.Open();
            FillData();
        }

        private void btnSeaschBtnSalePro_Click(object sender, EventArgs e)
        {
            try
            {
                string searchValue = txtReaschProductWareHouse.Text.Trim();
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
                    "INNER JOIN [Category] ON [Product].[CategoryID] = [Category].[CategoryID] where ";
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
                    dgvwWareFrmWareHouse.DataSource = dt;

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

        private void btnResetBtnSalePro_Click(object sender, EventArgs e)
        {
            txtReaschProductWareHouse.Clear();
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



        private void btnExportpdf_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF Files|*.pdf";
                saveFileDialog.Title = "Export Products to PDF";
                saveFileDialog.FileName = "ProductList_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (dgvwWareFrmWareHouse.Rows.Count == 0)
                    {
                        MessageBox.Show("No data to export!", "Information",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        Document document = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
                        PdfWriter writer = PdfWriter.GetInstance(document, fs);

                        document.Open();


                        iTextSharp.text.Font titleFont = new iTextSharp.text.Font(
                            iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD);
                        Paragraph title = new Paragraph("Product List", titleFont);
                        title.Alignment = Element.ALIGN_CENTER;
                        document.Add(title);


                        iTextSharp.text.Font dateFont = new iTextSharp.text.Font(
                            iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.ITALIC);
                        Paragraph date = new Paragraph("Exported on: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), dateFont);
                        date.Alignment = Element.ALIGN_CENTER;
                        document.Add(date);
                        document.Add(new Paragraph(" "));


                        PdfPTable pdfTable = new PdfPTable(dgvwWareFrmWareHouse.Columns.Count);
                        pdfTable.WidthPercentage = 100;


                        foreach (DataGridViewColumn column in dgvwWareFrmWareHouse.Columns)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText,
                                new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD)));
                            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfTable.AddCell(cell);
                        }


                        foreach (DataGridViewRow row in dgvwWareFrmWareHouse.Rows)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {

                                string cellValue = cell.Value?.ToString() ?? "";
                                pdfTable.AddCell(new Phrase(cellValue,
                                    new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10)));
                            }
                        }


                        document.Add(pdfTable);


                        document.Close();

                        MessageBox.Show("Export to PDF successful!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to PDF: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvwWareFrmWareHouse.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel Files|*.xlsx";
                    saveFileDialog.Title = "Save Excel File";
                    saveFileDialog.FileName = "ProductList_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;
                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                        using (var package = new OfficeOpenXml.ExcelPackage())
                        {
                            var worksheet = package.Workbook.Worksheets.Add("ProductList");

                            for (int col = 0; col < dgvwWareFrmWareHouse.Columns.Count; col++)
                            {
                                worksheet.Cells[1, col + 1].Value = dgvwWareFrmWareHouse.Columns[col].HeaderText;
                                worksheet.Cells[1, col + 1].Style.Font.Bold = true;
                            }

                            for (int row = 0; row < dgvwWareFrmWareHouse.Rows.Count; row++)
                            {
                                for (int col = 0; col < dgvwWareFrmWareHouse.Columns.Count; col++)
                                {
                                    worksheet.Cells[row + 2, col + 1].Value =
                                        dgvwWareFrmWareHouse.Rows[row].Cells[col].Value?.ToString() ?? "";
                                }
                            }

                            worksheet.Cells.AutoFitColumns();

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
    }
}
