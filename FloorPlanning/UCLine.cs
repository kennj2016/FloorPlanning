using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FloorPlanning.Models;

namespace FloorPlanning
{
    public partial class UCLine : UserControl
    {
        public bool bSelected = false; 
        public double dArea = 0;
        public double dPerim = 0;
        public double dWaste = 0;
        public Color pnlBackColor = Color.Green;
        public short Alpha = 255;
        public string LineName = "Line";
        public int nIndex = 0;
        DrawingForm oForm = null;
        public string lastSelectedText = "";
        public Base b;

        public UCLine(DrawingForm l_Form)
        {
            InitializeComponent();
            b = new Base(0, "", 0, 0, 0, 0, 1, 0, 0, 0);
            oForm = l_Form;
        }

        public void Select(bool l_bSelected)
        {
            bSelected = l_bSelected;
            SetBold();
        }

        public void SetBase(Base l_b)
        {
            b = l_b;
            LineName = b.sName;
            lblName.Text = LineName;
            pnlBackColor = Color.FromArgb(255, b.R, b.G, b.B); ;
            Alpha = (short)b.A;
            this.Invalidate();
        }
        
        private void SetBold()
        {
            FontStyle fs = (bSelected) ? FontStyle.Bold : FontStyle.Regular;
            lblName.Font = new Font(lblName.Font, fs);
            lblPerimDecimal.Font = new Font(lblPerimDecimal.Font, fs);
            lblPerimPoint.Font = new Font(lblPerimPoint.Font, fs);
            lblPerimWhole.Font = new Font(lblPerimWhole.Font, fs);
            lblSelected.Visible = bSelected;
        }

        public void SetPerim(double l_perim)
        {
            dPerim = l_perim;
            lblPerimWhole.Text = ((int)dPerim).ToString("N0").Replace(".", ",");
            lblPerimDecimal.Text = ((int)(((decimal)dPerim % 1) * 10)).ToString();//((int)((dPerim - (int)dPerim) * 10)).ToString();
        }

        private void UCLine_Click(object sender, EventArgs e)
        {
            oForm.SelectLine(this.LineName);
        }

        private void pnlColor_MouseDownClient(object sender, MouseEventArgs e)
        {
            oForm.SelectLine(this.LineName);
        }

        private void lblPerimWhole_Click(object sender, EventArgs e)
        {
            oForm.SelectLine(this.LineName);
        }

        private void lblPerimDecimal_Click(object sender, EventArgs e)
        {
            oForm.SelectLine(this.LineName);
        }

        private void lblPerimPoint_Click(object sender, EventArgs e)
        {
            oForm.SelectLine(this.LineName);
        }

        private void SetLastSelectedText(string l_lastSelectedText)
        {
            oForm.SelectLine(this.LineName);
        }

        private void UCLine_Paint(object sender, PaintEventArgs e)
        {
            ShowPensAndSmoothingMode(e);
        }

        private void ShowPensAndSmoothingMode(PaintEventArgs e)
        {

            // Set the SmoothingMode property to smooth the line.
            e.Graphics.SmoothingMode =
                System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Create a new Pen object.
            Pen pen = new Pen(pnlBackColor);

            // Set the width
            pen.Width = (float)b.Width;

            // Set the DashCap
            if (b.CapType == 0)
            {
                pen.DashCap = System.Drawing.Drawing2D.DashCap.Flat;
            }
            else
            {
                pen.DashCap = System.Drawing.Drawing2D.DashCap.Round;
            }

            // Create a custom dash pattern.
            if (b.DashStyle == 0)
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            }
            else if (b.DashStyle == 1)
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            }
            else if (b.DashStyle == 2)
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            }
            else
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            }

            // Draw a line
            if (b.DashType == 0)
            {
                e.Graphics.DrawLine(pen, 12.0F, 54.0F, 182.0F, 54.0F);
            }
            else
            {
                e.Graphics.DrawLine(pen, 12.0F, 44.0F, 182.0F, 44.0F);
                e.Graphics.DrawLine(pen, 12.0F, 64.0F, 182.0F, 64.0F);
            }

            // Dispose of the custom pen.
            pen.Dispose();
        }
    }
}
