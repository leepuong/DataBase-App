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
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace database.FRM
{
    public partial class FrmBill : UserControl
    {
        SqlConnection Connection;
        public FrmBill()
        {
            InitializeComponent();
            Connection = new SqlConnection("Server=DELL-5430\\SQLEXPRESS;Database = StoreX; Integrated Security = true;");
        }

        private string currentOrderID;

        public void SetOrderID(string orderID)
        {
            currentOrderID = orderID;
        }


        public void FillOrderDetailsToGridView(string orderID)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                string query = @"
                SELECT [OrderID] as 'ID', [Product].ProductNAME as 'Name', [QuantityOfOrder] as 'Quantity', [Price] as 'Price' 
                FROM [StoreX].[dbo].[OrderDetails]  
                INNER JOIN Product on [OrderDetails].[ProductID]=[dbo].[Product].[ProductID]
                WHERE OrderID = @OrderID";

                using (SqlCommand cmd = new SqlCommand(query, Connection))
                {
                    cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = int.Parse(orderID);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvwbillOrder.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading order details: " + ex.Message);
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
        }


        private void btnExitFrmBill_Click(object sender, EventArgs e)
        {
            if (this.Tag is Form parentForm)
            {
                parentForm.Close();
                this.Hide();
            }
        }


        public void CalculateTotalBill(string orderID)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                string query = "SELECT SUM(Price*[QuantityOfOrder]) as Total FROM OrderDetails WHERE OrderID = @OrderID";

                using (SqlCommand cmd = new SqlCommand(query, Connection))
                {
                    cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = int.Parse(orderID);

                    object result = cmd.ExecuteScalar();
                    lbl_totalBill.Text = result != null && result != DBNull.Value
                        ? "Total: " + result.ToString()
                        : "Total: 0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating bill total: " + ex.Message);
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
        }
        private void dgvwbillOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_CreateBill_Click(object sender, EventArgs e)
        {
            if (dgvwbillOrder.Rows.Count == 0)
            {
                MessageBox.Show("No data to export to PDF.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                Title = "Export invoice to PDF",
                FileName = $"Bill_Date_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                        PdfWriter writer = PdfWriter.GetInstance(document, fs);

                        document.Open();

                        iTextSharp.text.Font titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD);
                        Paragraph title = new Paragraph("SALES INVOICE", titleFont);
                        title.Alignment = Element.ALIGN_CENTER;
                        document.Add(title);

                        iTextSharp.text.Font infoFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10);
                        document.Add(new Paragraph($"Export Date: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", infoFont));
                        document.Add(new Paragraph($"Order ID: {currentOrderID}", infoFont));
                        document.Add(Chunk.NEWLINE);

                        PdfPTable table = new PdfPTable(4);
                        table.WidthPercentage = 100;
                        table.SetWidths(new float[] { 1f, 3f, 1f, 1f });

                        string[] headers = { "ID", "Product Name", "Quantity", "Price" };
                        foreach (string header in headers)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(header, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD)));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                        }

                        foreach (DataGridViewRow row in dgvwbillOrder.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                table.AddCell(row.Cells["ID"].Value?.ToString() ?? "");
                                table.AddCell(row.Cells["Name"].Value?.ToString() ?? "");
                                table.AddCell(row.Cells["Quantity"].Value?.ToString() ?? "");
                                table.AddCell(row.Cells["Price"].Value?.ToString() ?? "");
                            }
                        }

                        document.Add(table);

                        document.Add(Chunk.NEWLINE);
                        Paragraph totalParagraph = new Paragraph($"{lbl_totalBill.Text}", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD));
                        totalParagraph.Alignment = Element.ALIGN_RIGHT;
                        document.Add(totalParagraph);

                        document.Close();

                        MessageBox.Show($"PDF export successful!\nSaved to: {saveFileDialog.FileName}", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
