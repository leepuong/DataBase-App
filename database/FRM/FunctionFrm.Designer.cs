namespace database.FRM
{
    partial class FunctionFrm
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
            btnEditItem = new Guna.UI2.WinForms.Guna2Button();
            btnDeleteItem = new Guna.UI2.WinForms.Guna2Button();
            SuspendLayout();
            // 
            // btnEditItem
            // 
            btnEditItem.CustomizableEdges = customizableEdges1;
            btnEditItem.DisabledState.BorderColor = Color.DarkGray;
            btnEditItem.DisabledState.CustomBorderColor = Color.DarkGray;
            btnEditItem.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnEditItem.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnEditItem.Dock = DockStyle.Top;
            btnEditItem.FillColor = Color.FromArgb(230, 185, 166);
            btnEditItem.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEditItem.ForeColor = Color.White;
            btnEditItem.HoverState.BorderColor = Color.FromArgb(47, 54, 69);
            btnEditItem.HoverState.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold);
            btnEditItem.HoverState.ForeColor = Color.FromArgb(47, 54, 69);
            btnEditItem.Location = new Point(0, 0);
            btnEditItem.Name = "btnEditItem";
            btnEditItem.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnEditItem.Size = new Size(100, 40);
            btnEditItem.TabIndex = 2;
            btnEditItem.Text = "Edit";
            btnEditItem.Click += btnEditItem_Click;
            // 
            // btnDeleteItem
            // 
            btnDeleteItem.CustomizableEdges = customizableEdges3;
            btnDeleteItem.DisabledState.BorderColor = Color.DarkGray;
            btnDeleteItem.DisabledState.CustomBorderColor = Color.DarkGray;
            btnDeleteItem.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnDeleteItem.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnDeleteItem.Dock = DockStyle.Top;
            btnDeleteItem.FillColor = Color.FromArgb(230, 185, 166);
            btnDeleteItem.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDeleteItem.ForeColor = Color.White;
            btnDeleteItem.HoverState.BorderColor = Color.FromArgb(47, 54, 69);
            btnDeleteItem.HoverState.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold);
            btnDeleteItem.HoverState.ForeColor = Color.FromArgb(47, 54, 69);
            btnDeleteItem.Location = new Point(0, 40);
            btnDeleteItem.Name = "btnDeleteItem";
            btnDeleteItem.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnDeleteItem.Size = new Size(100, 40);
            btnDeleteItem.TabIndex = 4;
            btnDeleteItem.Text = "Delete";
            btnDeleteItem.Click += btnDeleteItem_Click;
            // 
            // FunctionFrm
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(btnDeleteItem);
            Controls.Add(btnEditItem);
            Margin = new Padding(0);
            Name = "FunctionFrm";
            Size = new Size(100, 80);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button btnEditItem;
        private Guna.UI2.WinForms.Guna2Button btnDeleteItem;
    }
}
