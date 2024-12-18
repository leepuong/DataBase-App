namespace database.FRM
{
    partial class FrmControlCustomerID
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            txtAddOrderID = new Guna.UI2.WinForms.Guna2TextBox();
            cboAddCustomerID = new Guna.UI2.WinForms.Guna2ComboBox();
            SuspendLayout();
            // 
            // txtAddOrderID
            // 
            txtAddOrderID.BackColor = Color.Transparent;
            txtAddOrderID.BorderColor = Color.Black;
            txtAddOrderID.BorderRadius = 12;
            txtAddOrderID.CustomizableEdges = customizableEdges1;
            txtAddOrderID.DefaultText = "";
            txtAddOrderID.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtAddOrderID.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtAddOrderID.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtAddOrderID.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtAddOrderID.FillColor = Color.FromArgb(49, 49, 60);
            txtAddOrderID.FocusedState.BorderColor = Color.FromArgb(230, 185, 166);
            txtAddOrderID.Font = new Font("Segoe UI", 9F);
            txtAddOrderID.ForeColor = Color.White;
            txtAddOrderID.HoverState.BorderColor = Color.FromArgb(230, 185, 166);
            txtAddOrderID.Location = new Point(0, 48);
            txtAddOrderID.Margin = new Padding(3, 5, 3, 5);
            txtAddOrderID.Name = "txtAddOrderID";
            txtAddOrderID.PasswordChar = '\0';
            txtAddOrderID.PlaceholderForeColor = Color.FromArgb(158, 158, 177);
            txtAddOrderID.PlaceholderText = "Order ID";
            txtAddOrderID.SelectedText = "";
            txtAddOrderID.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtAddOrderID.Size = new Size(228, 50);
            txtAddOrderID.TabIndex = 38;
            txtAddOrderID.KeyDown += txtAddOrderID_KeyDown;
            // 
            // cboAddCustomerID
            // 
            cboAddCustomerID.BackColor = Color.Transparent;
            cboAddCustomerID.BorderColor = Color.Black;
            cboAddCustomerID.BorderRadius = 12;
            cboAddCustomerID.CustomizableEdges = customizableEdges3;
            cboAddCustomerID.DisabledState.BorderColor = Color.FromArgb(230, 185, 166);
            cboAddCustomerID.DrawMode = DrawMode.OwnerDrawFixed;
            cboAddCustomerID.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAddCustomerID.FillColor = Color.FromArgb(49, 49, 60);
            cboAddCustomerID.FocusedColor = Color.FromArgb(230, 185, 166);
            cboAddCustomerID.FocusedState.BorderColor = Color.FromArgb(230, 185, 166);
            cboAddCustomerID.FocusedState.FillColor = Color.FromArgb(230, 185, 166);
            cboAddCustomerID.FocusedState.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cboAddCustomerID.FocusedState.ForeColor = Color.FromArgb(49, 49, 60);
            cboAddCustomerID.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cboAddCustomerID.ForeColor = Color.FromArgb(158, 158, 177);
            cboAddCustomerID.HoverState.BorderColor = Color.FromArgb(230, 185, 166);
            cboAddCustomerID.ItemHeight = 34;
            cboAddCustomerID.Location = new Point(30, 0);
            cboAddCustomerID.Name = "cboAddCustomerID";
            cboAddCustomerID.ShadowDecoration.CustomizableEdges = customizableEdges4;
            cboAddCustomerID.Size = new Size(170, 40);
            cboAddCustomerID.TabIndex = 39;
            // 
            // FrmControlCustomerID
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(cboAddCustomerID);
            Controls.Add(txtAddOrderID);
            Name = "FrmControlCustomerID";
            Size = new Size(228, 100);
            Load += FrmControlCustomerID_Load;
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox txtAddOrderID;
        private Guna.UI2.WinForms.Guna2ComboBox cboAddCustomerID;
    }
}
