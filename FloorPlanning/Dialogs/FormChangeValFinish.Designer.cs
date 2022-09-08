namespace FloorPlanning
{
    partial class FormChangeValFinish
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.nWaste = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.nPerimeter = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.nArea = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblArea = new Infragistics.Win.Misc.UltraLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnOK = new Infragistics.Win.Misc.UltraButton();
            this.txtName = new Infragistics.Win.Misc.UltraLabel();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nWaste)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPerimeter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nArea)).BeginInit();
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
            this.ultraPanel1.Size = new System.Drawing.Size(575, 286);
            this.ultraPanel1.TabIndex = 7;
            // 
            // ultraPanel2
            // 
            this.ultraPanel2.BorderStyle = Infragistics.Win.UIElementBorderStyle.Inset;
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.txtName);
            this.ultraPanel2.ClientArea.Controls.Add(this.nWaste);
            this.ultraPanel2.ClientArea.Controls.Add(this.nPerimeter);
            this.ultraPanel2.ClientArea.Controls.Add(this.nArea);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraLabel2);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraLabel1);
            this.ultraPanel2.ClientArea.Controls.Add(this.lblArea);
            this.ultraPanel2.Location = new System.Drawing.Point(147, 36);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(383, 164);
            this.ultraPanel2.TabIndex = 9;
            // 
            // nWaste
            // 
            this.nWaste.Location = new System.Drawing.Point(98, 123);
            this.nWaste.MaskInput = "{LOC}n,nnn,nnn.n";
            this.nWaste.MaxValue = 999999.9D;
            this.nWaste.MinValue = 0D;
            this.nWaste.Name = "nWaste";
            this.nWaste.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.nWaste.Size = new System.Drawing.Size(197, 26);
            this.nWaste.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.nWaste.SpinIncrement = 1D;
            this.nWaste.TabIndex = 16;
            // 
            // nPerimeter
            // 
            this.nPerimeter.Location = new System.Drawing.Point(98, 78);
            this.nPerimeter.MaskInput = "{LOC}n,nnn,nnn.n";
            this.nPerimeter.MaxValue = 999999.9D;
            this.nPerimeter.MinValue = 0D;
            this.nPerimeter.Name = "nPerimeter";
            this.nPerimeter.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.nPerimeter.Size = new System.Drawing.Size(197, 26);
            this.nPerimeter.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.nPerimeter.SpinIncrement = 1D;
            this.nPerimeter.TabIndex = 15;
            // 
            // nArea
            // 
            this.nArea.Location = new System.Drawing.Point(98, 33);
            this.nArea.MaskInput = "{LOC}n,nnn,nnn.n";
            this.nArea.MaxValue = 999999.9D;
            this.nArea.MinValue = 0D;
            this.nArea.Name = "nArea";
            this.nArea.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.nArea.Size = new System.Drawing.Size(197, 26);
            this.nArea.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.nArea.SpinIncrement = 1D;
            this.nArea.TabIndex = 14;
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(12, 128);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(119, 22);
            this.ultraLabel2.TabIndex = 12;
            this.ultraLabel2.Text = "Waste(%):";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(12, 83);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(119, 22);
            this.ultraLabel1.TabIndex = 10;
            this.ultraLabel1.Text = "Perimeter:";
            // 
            // lblArea
            // 
            this.lblArea.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArea.Location = new System.Drawing.Point(12, 38);
            this.lblArea.Name = "lblArea";
            this.lblArea.Size = new System.Drawing.Size(119, 22);
            this.lblArea.TabIndex = 8;
            this.lblArea.Text = "Area:";
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
            appearance3.Image = global::FloorPlanning.Properties.Resources.cancelicon48;
            this.btnCancel.Appearance = appearance3;
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
            appearance4.Image = global::FloorPlanning.Properties.Resources.okicon48;
            this.btnOK.Appearance = appearance4;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.ImageSize = new System.Drawing.Size(32, 32);
            this.btnOK.Location = new System.Drawing.Point(176, 224);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(104, 50);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(2, 2);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(119, 22);
            this.txtName.TabIndex = 17;
            this.txtName.Text = "Finish";
            // 
            // FormChangeValFinish
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(575, 286);
            this.Controls.Add(this.ultraPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(581, 310);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(581, 310);
            this.Name = "FormChangeValFinish";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Finish Edit Dialog - New Values";
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ClientArea.PerformLayout();
            this.ultraPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nWaste)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPerimeter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraButton btnOK;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel lblArea;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor nArea;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor nWaste;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor nPerimeter;
        private Infragistics.Win.Misc.UltraLabel txtName;
    }
}