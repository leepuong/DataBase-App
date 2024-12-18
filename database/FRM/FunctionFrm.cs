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
    public partial class FunctionFrm : UserControl
    {
        public event EventHandler EditButtonClicked;
        public event EventHandler DeleteButtonClicked;

        public FunctionFrm()
        {
            InitializeComponent();
        }





        private void btnEditItem_Click(object sender, EventArgs e)
        {
            EditButtonClicked?.Invoke(this, EventArgs.Empty);
            if (this.Parent is Form parentForm)
            {
                parentForm.Close();
            }
                

        }



        private void btnDeleteItem_Click(object sender, EventArgs e)
        {

            DeleteButtonClicked?.Invoke(this, EventArgs.Empty);
            if (this.Parent is Form parentForm)
            {
                parentForm.Close();
            }


        }



        private void FunctionFrm_Load(object sender, EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.None;

        }

    }
}
