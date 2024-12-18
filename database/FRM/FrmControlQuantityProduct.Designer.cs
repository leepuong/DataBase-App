namespace database.FRM
{
    partial class FrmControlQuantityProduct
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
            txtAddQuantity = new Guna.UI2.WinForms.Guna2TextBox();
            SuspendLayout();
            // 
            // txtAddQuantity
            // 
            txtAddQuantity.BackColor = Color.Transparent;
            txtAddQuantity.BorderColor = Color.Black;
            txtAddQuantity.BorderRadius = 12;
            txtAddQuantity.CustomizableEdges = customizableEdges1;
            txtAddQuantity.DefaultText = "";
            txtAddQuantity.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtAddQuantity.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtAddQuantity.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtAddQuantity.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtAddQuantity.FillColor = Color.FromArgb(49, 49, 60);
            txtAddQuantity.FocusedState.BorderColor = Color.FromArgb(230, 185, 166);
            txtAddQuantity.Font = new Font("Segoe UI", 9F);
            txtAddQuantity.ForeColor = Color.White;
            txtAddQuantity.HoverState.BorderColor = Color.FromArgb(230, 185, 166);
            txtAddQuantity.Location = new Point(0, 0);
            txtAddQuantity.Margin = new Padding(3, 5, 3, 5);
            txtAddQuantity.Name = "txtAddQuantity";
            txtAddQuantity.PasswordChar = '\0';
            txtAddQuantity.PlaceholderForeColor = Color.FromArgb(158, 158, 177);
            txtAddQuantity.PlaceholderText = "Quantity";
            txtAddQuantity.SelectedText = "";
            txtAddQuantity.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtAddQuantity.Size = new Size(228, 50);
            txtAddQuantity.TabIndex = 37;
            txtAddQuantity.KeyDown += txtAddQuantity_KeyDown;
            // 
            // FrmControlQuantityProduct
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(txtAddQuantity);
            Name = "FrmControlQuantityProduct";
            Size = new Size(228, 50);
            ResumeLayout(false);


        }

        #endregion
        private Guna.UI2.WinForms.Guna2TextBox txtAddQuantity;
    }
}
