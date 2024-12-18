using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace database.FRM
{
    public partial class FrmControlQuantityProduct : UserControl
    {
        
        public string Quantity { get; private set; }
        
        public event EventHandler QuantityAdded;
        public event EventHandler QuantityRemoved;
        public event EventHandler QuantityCancel;
        public FrmControlQuantityProduct()
        {
            InitializeComponent();

        }
        


        private void txtAddQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void txtAddQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtAddQuantity.Text) )
                {

                        Quantity = txtAddQuantity.Text;
                        QuantityAdded?.Invoke(this, EventArgs.Empty);
                        ParentForm?.Close();
                    
                }
            }
            if (e.KeyCode == Keys.Escape) {
                QuantityAdded?.Invoke(this, EventArgs.Empty);

                ParentForm?.Close();
            }
        }
    }
}
