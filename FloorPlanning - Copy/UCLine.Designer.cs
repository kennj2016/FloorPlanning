namespace FloorPlanning
{
    partial class UCLine
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
            this.lblName = new System.Windows.Forms.Label();
            this.lblPerimWhole = new System.Windows.Forms.Label();
            this.lblPerimDecimal = new System.Windows.Forms.Label();
            this.lblPerimPoint = new System.Windows.Forms.Label();
            this.lblSelected = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(8, 17);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 21);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Line";
            this.lblName.Click += new System.EventHandler(this.UCLine_Click);
            // 
            // lblPerimWhole
            // 
            this.lblPerimWhole.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblPerimWhole.Location = new System.Drawing.Point(118, 23);
            this.lblPerimWhole.Name = "lblPerimWhole";
            this.lblPerimWhole.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblPerimWhole.Size = new System.Drawing.Size(58, 13);
            this.lblPerimWhole.TabIndex = 5;
            this.lblPerimWhole.Text = "0";
            this.lblPerimWhole.Click += new System.EventHandler(this.lblPerimWhole_Click);
            // 
            // lblPerimDecimal
            // 
            this.lblPerimDecimal.AutoSize = true;
            this.lblPerimDecimal.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblPerimDecimal.Location = new System.Drawing.Point(173, 23);
            this.lblPerimDecimal.Name = "lblPerimDecimal";
            this.lblPerimDecimal.Size = new System.Drawing.Size(13, 13);
            this.lblPerimDecimal.TabIndex = 9;
            this.lblPerimDecimal.Text = "0";
            this.lblPerimDecimal.Click += new System.EventHandler(this.lblPerimDecimal_Click);
            // 
            // lblPerimPoint
            // 
            this.lblPerimPoint.BackColor = System.Drawing.Color.Black;
            this.lblPerimPoint.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblPerimPoint.Location = new System.Drawing.Point(173, 34);
            this.lblPerimPoint.Name = "lblPerimPoint";
            this.lblPerimPoint.Size = new System.Drawing.Size(1, 1);
            this.lblPerimPoint.TabIndex = 11;
            this.lblPerimPoint.Text = ".";
            this.lblPerimPoint.Click += new System.EventHandler(this.lblPerimPoint_Click);
            // 
            // lblSelected
            // 
            this.lblSelected.AutoSize = true;
            this.lblSelected.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelected.Location = new System.Drawing.Point(188, 44);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(21, 21);
            this.lblSelected.TabIndex = 12;
            this.lblSelected.Text = "<";
            // 
            // UCLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblSelected);
            this.Controls.Add(this.lblPerimPoint);
            this.Controls.Add(this.lblPerimWhole);
            this.Controls.Add(this.lblPerimDecimal);
            this.Controls.Add(this.lblName);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UCLine";
            this.Size = new System.Drawing.Size(212, 79);
            this.Click += new System.EventHandler(this.UCLine_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UCLine_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblPerimWhole;
        private System.Windows.Forms.Label lblPerimDecimal;
        private System.Windows.Forms.Label lblPerimPoint;
        private System.Windows.Forms.Label lblSelected;
    }
}
