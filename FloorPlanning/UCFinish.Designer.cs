namespace FloorPlanning
{
    partial class UCFinish
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.lblName = new System.Windows.Forms.Label();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.pnlColor = new Infragistics.Win.Misc.UltraPanel();
            this.lblAreaText = new System.Windows.Forms.Label();
            this.lblPerimText = new System.Windows.Forms.Label();
            this.lblAreaWhole = new System.Windows.Forms.Label();
            this.lblPerimWhole = new System.Windows.Forms.Label();
            this.lblWasteWhole = new System.Windows.Forms.Label();
            this.lblAreaDecimal = new System.Windows.Forms.Label();
            this.lblWasteDecimal = new System.Windows.Forms.Label();
            this.lblPerimDecimal = new System.Windows.Forms.Label();
            this.lblAreaPoint = new System.Windows.Forms.Label();
            this.lblPerimPoint = new System.Windows.Forms.Label();
            this.lblWastePoint = new System.Windows.Forms.Label();
            this.lbTransparencyPercent = new System.Windows.Forms.Label();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.pnlColor.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(51, 21);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Finish";
            this.lblName.Click += new System.EventHandler(this.UCFinish_Click);
            // 
            // ultraPanel1
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance1.BorderColor = System.Drawing.Color.Gray;
            this.ultraPanel1.Appearance = appearance1;
            this.ultraPanel1.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.pnlColor);
            this.ultraPanel1.Location = new System.Drawing.Point(9, 27);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(60, 38);
            this.ultraPanel1.TabIndex = 1;
            // 
            // pnlColor
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.pnlColor.Appearance = appearance2;
            this.pnlColor.Location = new System.Drawing.Point(2, 2);
            this.pnlColor.Name = "pnlColor";
            this.pnlColor.Size = new System.Drawing.Size(54, 32);
            this.pnlColor.TabIndex = 2;
            this.pnlColor.MouseDownClient += new System.Windows.Forms.MouseEventHandler(this.pnlColor_MouseDownClient);
            // 
            // lblAreaText
            // 
            this.lblAreaText.AutoSize = true;
            this.lblAreaText.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblAreaText.Location = new System.Drawing.Point(81, 19);
            this.lblAreaText.Name = "lblAreaText";
            this.lblAreaText.Size = new System.Drawing.Size(33, 13);
            this.lblAreaText.TabIndex = 2;
            this.lblAreaText.Text = "Area:";
            this.lblAreaText.Click += new System.EventHandler(this.UCFinish_Click);
            // 
            // lblPerimText
            // 
            this.lblPerimText.AutoSize = true;
            this.lblPerimText.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblPerimText.Location = new System.Drawing.Point(81, 37);
            this.lblPerimText.Name = "lblPerimText";
            this.lblPerimText.Size = new System.Drawing.Size(38, 13);
            this.lblPerimText.TabIndex = 3;
            this.lblPerimText.Text = "Perim:";
            this.lblPerimText.Click += new System.EventHandler(this.UCFinish_Click);
            // 
            // lblAreaWhole
            // 
            this.lblAreaWhole.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblAreaWhole.Location = new System.Drawing.Point(120, 19);
            this.lblAreaWhole.Name = "lblAreaWhole";
            this.lblAreaWhole.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblAreaWhole.Size = new System.Drawing.Size(58, 13);
            this.lblAreaWhole.TabIndex = 4;
            this.lblAreaWhole.Text = "0";
            this.lblAreaWhole.Click += new System.EventHandler(this.lblAreaWhole_Click);
            // 
            // lblPerimWhole
            // 
            this.lblPerimWhole.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblPerimWhole.Location = new System.Drawing.Point(120, 37);
            this.lblPerimWhole.Name = "lblPerimWhole";
            this.lblPerimWhole.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblPerimWhole.Size = new System.Drawing.Size(58, 13);
            this.lblPerimWhole.TabIndex = 5;
            this.lblPerimWhole.Text = "0";
            this.lblPerimWhole.Click += new System.EventHandler(this.lblPerimWhole_Click);
            // 
            // lblWasteWhole
            // 
            this.lblWasteWhole.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblWasteWhole.Location = new System.Drawing.Point(74, 60);
            this.lblWasteWhole.Name = "lblWasteWhole";
            this.lblWasteWhole.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblWasteWhole.Size = new System.Drawing.Size(58, 13);
            this.lblWasteWhole.TabIndex = 6;
            this.lblWasteWhole.Text = "0";
            this.lblWasteWhole.Visible = false;
            this.lblWasteWhole.Click += new System.EventHandler(this.lblWasteWhole_Click);
            // 
            // lblAreaDecimal
            // 
            this.lblAreaDecimal.AutoSize = true;
            this.lblAreaDecimal.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblAreaDecimal.Location = new System.Drawing.Point(175, 19);
            this.lblAreaDecimal.Name = "lblAreaDecimal";
            this.lblAreaDecimal.Size = new System.Drawing.Size(13, 13);
            this.lblAreaDecimal.TabIndex = 7;
            this.lblAreaDecimal.Text = "0";
            this.lblAreaDecimal.Click += new System.EventHandler(this.lblAreaDecimal_Click);
            // 
            // lblWasteDecimal
            // 
            this.lblWasteDecimal.AutoSize = true;
            this.lblWasteDecimal.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblWasteDecimal.Location = new System.Drawing.Point(129, 60);
            this.lblWasteDecimal.Name = "lblWasteDecimal";
            this.lblWasteDecimal.Size = new System.Drawing.Size(22, 13);
            this.lblWasteDecimal.TabIndex = 8;
            this.lblWasteDecimal.Text = "0%";
            this.lblWasteDecimal.Visible = false;
            this.lblWasteDecimal.Click += new System.EventHandler(this.lblWasteDecimal_Click);
            // 
            // lblPerimDecimal
            // 
            this.lblPerimDecimal.AutoSize = true;
            this.lblPerimDecimal.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblPerimDecimal.Location = new System.Drawing.Point(175, 37);
            this.lblPerimDecimal.Name = "lblPerimDecimal";
            this.lblPerimDecimal.Size = new System.Drawing.Size(13, 13);
            this.lblPerimDecimal.TabIndex = 9;
            this.lblPerimDecimal.Text = "0";
            this.lblPerimDecimal.Click += new System.EventHandler(this.lblPerimDecimal_Click);
            // 
            // lblAreaPoint
            // 
            this.lblAreaPoint.BackColor = System.Drawing.Color.Black;
            this.lblAreaPoint.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblAreaPoint.Location = new System.Drawing.Point(175, 30);
            this.lblAreaPoint.Name = "lblAreaPoint";
            this.lblAreaPoint.Size = new System.Drawing.Size(1, 1);
            this.lblAreaPoint.TabIndex = 10;
            this.lblAreaPoint.Text = ".";
            this.lblAreaPoint.Click += new System.EventHandler(this.lblAreaPoint_Click);
            // 
            // lblPerimPoint
            // 
            this.lblPerimPoint.BackColor = System.Drawing.Color.Black;
            this.lblPerimPoint.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblPerimPoint.Location = new System.Drawing.Point(175, 48);
            this.lblPerimPoint.Name = "lblPerimPoint";
            this.lblPerimPoint.Size = new System.Drawing.Size(1, 1);
            this.lblPerimPoint.TabIndex = 11;
            this.lblPerimPoint.Text = ".";
            this.lblPerimPoint.Click += new System.EventHandler(this.lblPerimPoint_Click);
            // 
            // lblWastePoint
            // 
            this.lblWastePoint.BackColor = System.Drawing.Color.Black;
            this.lblWastePoint.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblWastePoint.Location = new System.Drawing.Point(175, 66);
            this.lblWastePoint.Name = "lblWastePoint";
            this.lblWastePoint.Size = new System.Drawing.Size(1, 1);
            this.lblWastePoint.TabIndex = 12;
            this.lblWastePoint.Text = ".";
            this.lblWastePoint.Click += new System.EventHandler(this.lblWastePoint_Click);
            // 
            // lbTransparencyPercent
            // 
            this.lbTransparencyPercent.AutoSize = true;
            this.lbTransparencyPercent.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lbTransparencyPercent.Location = new System.Drawing.Point(166, 60);
            this.lbTransparencyPercent.Name = "lbTransparencyPercent";
            this.lbTransparencyPercent.Size = new System.Drawing.Size(22, 13);
            this.lbTransparencyPercent.TabIndex = 8;
            this.lbTransparencyPercent.Text = "0%";
            this.lbTransparencyPercent.Click += new System.EventHandler(this.lblWasteDecimal_Click);
            // 
            // UCFinish
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblAreaPoint);
            this.Controls.Add(this.lblPerimPoint);
            this.Controls.Add(this.lblWastePoint);
            this.Controls.Add(this.lblWasteWhole);
            this.Controls.Add(this.lblPerimWhole);
            this.Controls.Add(this.lblAreaWhole);
            this.Controls.Add(this.lblPerimDecimal);
            this.Controls.Add(this.lbTransparencyPercent);
            this.Controls.Add(this.lblWasteDecimal);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblAreaDecimal);
            this.Controls.Add(this.lblAreaText);
            this.Controls.Add(this.lblPerimText);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UCFinish";
            this.Size = new System.Drawing.Size(212, 79);
            this.Click += new System.EventHandler(this.UCFinish_Click);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.pnlColor.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel pnlColor;
        private System.Windows.Forms.Label lblAreaText;
        private System.Windows.Forms.Label lblPerimText;
        private System.Windows.Forms.Label lblAreaWhole;
        private System.Windows.Forms.Label lblPerimWhole;
        private System.Windows.Forms.Label lblWasteWhole;
        private System.Windows.Forms.Label lblAreaDecimal;
        private System.Windows.Forms.Label lblWasteDecimal;
        private System.Windows.Forms.Label lblPerimDecimal;
        private System.Windows.Forms.Label lblAreaPoint;
        private System.Windows.Forms.Label lblPerimPoint;
        private System.Windows.Forms.Label lblWastePoint;
        private System.Windows.Forms.Label lbTransparencyPercent;
    }
}
