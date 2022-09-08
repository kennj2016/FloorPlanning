namespace FloorPlanning
{
    partial class FormNewShape
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.barTransp = new System.Windows.Forms.TrackBar();
            this.lblTrans = new Infragistics.Win.Misc.UltraLabel();
            this.pnlColor = new Infragistics.Win.Misc.UltraPanel();
            this.ultraColorPicker1 = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.txtName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblTextEnterName = new Infragistics.Win.Misc.UltraLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnOK = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barTransp)).BeginInit();
            this.pnlColor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraColorPicker1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Create new FloorPlanning job in folder:";
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraPanel2);
            this.ultraPanel1.ClientArea.Controls.Add(this.pictureBox1);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnCancel);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnOK);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(565, 276);
            this.ultraPanel1.TabIndex = 7;
            // 
            // ultraPanel2
            // 
            this.ultraPanel2.BorderStyle = Infragistics.Win.UIElementBorderStyle.Inset;
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.barTransp);
            this.ultraPanel2.ClientArea.Controls.Add(this.lblTrans);
            this.ultraPanel2.ClientArea.Controls.Add(this.pnlColor);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraColorPicker1);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraLabel2);
            this.ultraPanel2.ClientArea.Controls.Add(this.txtName);
            this.ultraPanel2.ClientArea.Controls.Add(this.lblTextEnterName);
            this.ultraPanel2.Location = new System.Drawing.Point(147, 36);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(383, 164);
            this.ultraPanel2.TabIndex = 9;
            // 
            // barTransp
            // 
            this.barTransp.Location = new System.Drawing.Point(12, 122);
            this.barTransp.Maximum = 255;
            this.barTransp.Minimum = 1;
            this.barTransp.Name = "barTransp";
            this.barTransp.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.barTransp.Size = new System.Drawing.Size(138, 45);
            this.barTransp.TabIndex = 15;
            this.barTransp.TickStyle = System.Windows.Forms.TickStyle.None;
            this.barTransp.Value = 255;
            this.barTransp.ValueChanged += new System.EventHandler(this.barTransp_ValueChanged);
            // 
            // lblTrans
            // 
            this.lblTrans.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrans.Location = new System.Drawing.Point(12, 93);
            this.lblTrans.Name = "lblTrans";
            this.lblTrans.Size = new System.Drawing.Size(138, 22);
            this.lblTrans.TabIndex = 14;
            this.lblTrans.Text = "Transparency:";
            // 
            // pnlColor
            // 
            appearance1.AlphaLevel = ((short)(255));
            appearance1.BackColor = System.Drawing.Color.Green;
            this.pnlColor.Appearance = appearance1;
            this.pnlColor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.pnlColor.Location = new System.Drawing.Point(168, 93);
            this.pnlColor.Name = "pnlColor";
            this.pnlColor.Size = new System.Drawing.Size(160, 66);
            this.pnlColor.TabIndex = 13;
            // 
            // ultraColorPicker1
            // 
            this.ultraColorPicker1.DefaultColor = System.Drawing.Color.Green;
            this.ultraColorPicker1.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ultraColorPicker1.Location = new System.Drawing.Point(168, 52);
            this.ultraColorPicker1.Name = "ultraColorPicker1";
            this.ultraColorPicker1.Size = new System.Drawing.Size(160, 26);
            this.ultraColorPicker1.TabIndex = 12;
            this.ultraColorPicker1.Text = "Green";
            this.ultraColorPicker1.ColorChanged += new System.EventHandler(this.ultraColorPicker1_ColorChanged);
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(12, 56);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(138, 22);
            this.ultraLabel2.TabIndex = 10;
            this.ultraLabel2.Text = "Choose Color:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(168, 11);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(197, 26);
            this.txtName.TabIndex = 9;
            // 
            // lblTextEnterName
            // 
            this.lblTextEnterName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextEnterName.Location = new System.Drawing.Point(12, 15);
            this.lblTextEnterName.Name = "lblTextEnterName";
            this.lblTextEnterName.Size = new System.Drawing.Size(179, 22);
            this.lblTextEnterName.TabIndex = 8;
            this.lblTextEnterName.Text = "Enter new Finish name:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::FloorPlanning.Properties.Resources.icon64centered;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(21, 36);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 164);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // btnCancel
            // 
            appearance2.Image = global::FloorPlanning.Properties.Resources.cancelicon48;
            this.btnCancel.Appearance = appearance2;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImageSize = new System.Drawing.Size(32, 32);
            this.btnCancel.Location = new System.Drawing.Point(298, 224);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 50);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOK
            // 
            appearance3.Image = global::FloorPlanning.Properties.Resources.okicon48;
            this.btnOK.Appearance = appearance3;
            this.btnOK.ImageSize = new System.Drawing.Size(32, 32);
            this.btnOK.Location = new System.Drawing.Point(176, 224);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(104, 50);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FormNewShape
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(565, 276);
            this.Controls.Add(this.ultraPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(581, 310);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(581, 310);
            this.Name = "FormNewShape";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shape Dialog - New Shape";
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ClientArea.PerformLayout();
            this.ultraPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barTransp)).EndInit();
            this.pnlColor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraColorPicker1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraButton btnOK;
        private Infragistics.Win.Misc.UltraLabel lblTextEnterName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtName;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ultraColorPicker1;
        private System.Windows.Forms.TrackBar barTransp;
        private Infragistics.Win.Misc.UltraLabel lblTrans;
        private Infragistics.Win.Misc.UltraPanel pnlColor;
    }
}