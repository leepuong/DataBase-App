using database.FRM;
using Guna.UI2.WinForms;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;


namespace database
{
    public partial class Manager : Form
    {
        SqlConnection Connection;
        private string userRole = string.Empty;
        

        public Manager()
        {
            InitializeComponent();

            Connection = new SqlConnection("Server=DELL-5430\\SQLEXPRESS;Database  = StoreX; Integrated Security = true;");
            userRole = "default";
        }

        public Manager(string role)
        {
            InitializeComponent();
            userRole = role;
            

            Connection = new SqlConnection("Server=DELL-5430\\SQLEXPRESS;Database = StoreX; Integrated Security = true;");
            SetControlsByRole();
        }
        private void Manager_Load(object sender, EventArgs e)
        {
        }


        private void SetControlsByRole()
        {
  
            switch (userRole.ToLower())
            {
                case "admin":

                    btnSales.Enabled = true;
                    btnWarehouse.Enabled = true;
                    btn_Admin.Enabled = true;
                    btn_Admin.Checked = true;

                    AdminFrmEmployees adminFEmployees = new AdminFrmEmployees();
                    addFromControl(adminFEmployees);
                    btnCtrlEmployees.Checked = true;

                    break;

                case "sale":
 
                    btnSales.Enabled = true;
                    btnSales.Checked = true;

                    btnWarehouse.Enabled = false;
                    btnWarehouse.Visible = false;

                    btn_Admin.Enabled = false;
                    btn_Admin.Visible = false;


                    btnCtrlEmployees.Enabled = false;
                    btnCtrlWareHouse.Enabled = false;

                    SaleProduct saleFProduct = new SaleProduct();
                    addFromControl(saleFProduct);
                    btnCtrlProduct.Checked = true;


                    btnCtrlProduct.Location = new Point(0, 236);
                    btnCtrlCustomer.Location = new Point(0, 300);
                    btnCtrlOrder.Location = new Point(0, 364);
                    btnCtrlProduct.Visible = true;
                    btnCtrlCustomer.Visible = true;
                    btnCtrlOrder.Visible = true;
                    btnCtrlEmployees.Visible = false;
                    btnCtrlWareHouse.Visible = false;


                    break;

                case "warehouse":

                    btnSales.Enabled = false;
                    btnSales.Visible = false;

                    btnWarehouse.Enabled = true;
                    btnWarehouse.Location = new Point(46, 156);
                    btnWarehouse.Checked = true;

                    btn_Admin.Enabled = false;
                    btn_Admin.Visible = false;


                    btnCtrlEmployees.Enabled = false;
                    btnCtrlProduct.Enabled = false;
                    btnCtrlCustomer.Enabled = false;

                    btnCtrlEmployees.Visible = false;
                    btnCtrlProduct.Visible = false;
                    btnCtrlCustomer.Visible = false;
                    btnCtrlProduct.Visible = false;
                    btnCtrlOrder.Visible = false;

                    btnCtrlWareHouse.Location = new Point(0, 236);



                    WareHouseWareHouse wareHouseFWareHouse = new WareHouseWareHouse();
                    addFromControl(wareHouseFWareHouse);
                    btnCtrlWareHouse.Checked = true;


                    break;

                default:
   
                    btnSales.Enabled = false;
                    btnWarehouse.Enabled = false;
                    btn_Admin.Enabled = false;
                    break;
            }
        }

        private void btnSales_CheckedChanged(object sender, EventArgs e)
        {

            if (btnSales.Checked && userRole.ToLower().Equals("admin"))
            {
                btn_Admin.Checked = false;
                btnWarehouse.Checked = false;

                btnCtrlEmployees.Enabled = false;
                btnCtrlWareHouse.Enabled = false;
                btnCtrlProduct.Enabled = true;
                btnCtrlCustomer.Enabled = true;

                btnCtrlOrder.Text = "Order";
                btnCtrlWareHouse.Visible = false;
                btnCtrlEmployees.Visible = false;

                btnCtrlProduct.Location = new Point(0,236);
                btnCtrlProduct.Visible = true;
                btnCtrlCustomer.Location = new Point(0, 300);
                btnCtrlCustomer.Visible = true;
                btnCtrlOrder.Location = new Point(0, 364);
                btnCtrlOrder.Visible = true;

                SaleProduct saleFProduct = new SaleProduct();
                addFromControl(saleFProduct);
                btnCtrlProduct.Checked = true;

            }
        }

        private void btnWarehouse_CheckedChanged(object sender, EventArgs e)
        {
            if (btnWarehouse.Checked && userRole.ToLower().Equals("admin"))
            {
                btnSales.Checked = false;
                btn_Admin.Checked = false;

                btnCtrlEmployees.Enabled = false;
                btnCtrlProduct.Enabled = false;
                btnCtrlCustomer.Enabled = false;

                btnCtrlWareHouse.Enabled = true;

                btnCtrlEmployees.Visible = false;
                btnCtrlProduct.Visible = false;
                btnCtrlCustomer.Visible = false;
                btnCtrlProduct.Visible = false;
                btnCtrlOrder.Visible = false;

                btnCtrlWareHouse.Location = new Point(0, 236);

                btnCtrlOrder.Text = "Order";
                btnCtrlWareHouse.Visible = true;
                WareHouseWareHouse wareHouseFWareHouse = new WareHouseWareHouse();
                addFromControl(wareHouseFWareHouse);
                btnCtrlWareHouse.Checked = true;
            }


        }



        private void btn_Admin_CheckedChanged(object sender, EventArgs e)
        {
            if (btn_Admin.Checked && userRole.ToLower().Equals("admin"))
            {
                btnSales.Checked = false;
                btnWarehouse.Checked = false;

                btnCtrlEmployees.Enabled = true;
                btnCtrlProduct.Enabled = true;
                btnCtrlCustomer.Enabled = true;

                btnCtrlEmployees.Location = new Point(0,236);
                btnCtrlProduct.Location = new Point(0,300);
                btnCtrlCustomer.Location = new Point(0,364);
                btnCtrlOrder.Location = new Point(0,428);

                btnCtrlEmployees.Visible = true;
                btnCtrlProduct.Visible = true;
                btnCtrlCustomer.Visible = true;
                btnCtrlOrder.Visible = true;

                btnCtrlOrder.Text = "Statistical";
                btnCtrlWareHouse.Visible = false;
                AdminFrmEmployees adminFEmployees = new AdminFrmEmployees();
                addFromControl(adminFEmployees);
                btnCtrlEmployees.Checked = true;

            }
        }

        private void btn_Admin_Click(object sender, EventArgs e)
        {
            if (userRole.ToLower().Equals("admin"))
            {
                btn_Admin.Checked = true;
            }

        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            if (userRole.ToLower().Equals("sale"))
            {
                btnSales.Checked = true;
            }
            if (userRole.ToLower().Equals("admin"))
            {
                btnSales.Checked = true;
            }
        }


        private void btnWarehouse_Click(object sender, EventArgs e)
        {
            if (userRole.ToLower().Equals("warehouse"))
            {
                btnWarehouse.Checked = true;
            }
            if (userRole.ToLower().Equals("admin"))
            {
                btnWarehouse.Checked = true;
            }
        }



        private void addFromControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelContent.Controls.Clear();
            panelContent.Controls.Add(userControl);
            userControl.BringToFront();
        }


        private void btnCtrlEmployees_Click(object sender, EventArgs e)
        {
            AdminFrmEmployees adminFEmployees = new AdminFrmEmployees();
            if (userRole.ToLower().Equals("admin") && btn_Admin.Checked == true)
            {
                addFromControl(adminFEmployees);

            }
        }



        private void btnCtrlProduct_Click(object sender, EventArgs e)
        {
            SaleProduct saleFProduct = new SaleProduct();
            AdminFrmProduct adminFProduct = new AdminFrmProduct();

            if (userRole.ToLower().Equals("sale") | btnSales.Checked == true)
            {
                addFromControl(saleFProduct);
            }
            if (userRole.ToLower().Equals("admin") && btn_Admin.Checked == true)
            {
                addFromControl(adminFProduct);
            }
        }

        private void btnCtrlOrder_Click(object sender, EventArgs e)
        {
            SaleOrder saleFOrder = new SaleOrder();
            AdminFrmChart adminFChart = new AdminFrmChart();
            if (userRole.ToLower().Equals("sale") | btnSales.Checked == true)
            {
                addFromControl(saleFOrder);
            }
            
            if (userRole.ToLower().Equals("admin") && btn_Admin.Checked == true)
            {
                addFromControl(adminFChart);
            }

        }


        private void btnCtrlCustomer_Click(object sender, EventArgs e)
        {
            

            AdminFrmCustomer adminFCustomer = new AdminFrmCustomer();
                addFromControl((AdminFrmCustomer)adminFCustomer);
            
        }

        private void btnCtrlWareHouse_Click(object sender, EventArgs e)
        {
            WareHouseWareHouse wareHouseFWareHouse = new WareHouseWareHouse();
            if (userRole.ToLower().Equals("warehouse") | btnWarehouse.Checked == true)
            {
                addFromControl(wareHouseFWareHouse);
            }
        }

        


        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void btnCtrlLogout_Click(object sender, EventArgs e)
        {
            login_frm loginFrm = new login_frm();
            loginFrm.Show();
            this.Hide();
        }
    }
}
