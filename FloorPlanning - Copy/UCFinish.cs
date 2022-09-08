using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloorPlanning
{
    public partial class UCFinish : UserControl
    {
        public bool bSelected = false;
        public double dArea = 0;
        public double dPerim = 0;
        public double dWaste = 0;
        public Color pnlBackColor = Color.Green;
        public short Alpha = 255;
        public string FinishName = "Finish";
        public int nIndex = 0;
        DrawingForm oForm = null;
        public string lastSelectedText = "";

        public UCFinish(DrawingForm l_Form)
        {
            InitializeComponent();
            oForm = l_Form;
        }

        public void Select(bool l_bSelected)
        {
            bSelected = l_bSelected;
            SetBold();
        }

        public void SetFinishName(string l_Name)
        {
            FinishName = l_Name;
            lblName.Text = FinishName;
        }

        private void SetBold()
        {
            FontStyle fs = (bSelected) ? FontStyle.Bold : FontStyle.Regular;
            lblAreaDecimal.Font = new Font(lblAreaDecimal.Font, fs);
            lblAreaPoint.Font = new Font(lblAreaPoint.Font, fs);
            lblAreaText.Font = new Font(lblAreaText.Font, fs);
            lblAreaWhole.Font = new Font(lblAreaWhole.Font, fs);
            lblName.Font = new Font(lblName.Font, fs);
            lblPerimDecimal.Font = new Font(lblPerimDecimal.Font, fs);
            lblPerimPoint.Font = new Font(lblPerimPoint.Font, fs);
            lblPerimText.Font = new Font(lblPerimText.Font, fs);
            lblPerimWhole.Font = new Font(lblPerimWhole.Font, fs);
            lblWasteDecimal.Font = new Font(lblWasteDecimal.Font, fs);
            lblWastePoint.Font = new Font(lblWastePoint.Font, fs);
            lblWasteWhole.Font = new Font(lblWasteWhole.Font, fs);
        }

        public void SetColor(Color l_color, short l_Alpha)
        {
            pnlBackColor = l_color;
            //Kiet Nguyen: [1400] set transparency percent to label
            //Alpha = l_Alpha;
            SetAlPha(l_Alpha);
            this.pnlColor.Appearance.BackColor = pnlBackColor;
            //Kiet nguyen: don't need set alpha to background
            //keep current color
            ///     this.pnlColor.Appearance.AlphaLevel = Alpha; 
        }

        /// <summary>
        /// Kiet Nguyen: [1450] set alpha to background color
        /// </summary>
        /// <param name="l_Alpha"></param>
        /// <param name="percent"></param>
        public void SetAlPha(short l_Alpha)
        {
            Alpha = l_Alpha;
            //keep current color
            //    this.pnlColor.Appearance.AlphaLevel = Alpha;
            lbTransparencyPercent.Text = oForm.calcPercentOfTransparency(l_Alpha).ToString() + "%";
        }

        public void SetArea(double l_area)
        {
            dArea = l_area;
            lblAreaWhole.Text = ((int)dArea).ToString("N0").Replace(".", ",");
            lblAreaDecimal.Text = ((int)(((decimal)dArea % 1) * 10)).ToString();//((int)((dArea - (int)dArea) * 10)).ToString();
        }

        public void SetPerim(double l_perim)
        {
            dPerim = l_perim;
            lblPerimWhole.Text = ((int)dPerim).ToString("N0").Replace(".", ",");
            lblPerimDecimal.Text = ((int)(((decimal)dPerim % 1) * 10)).ToString();//((int)((dPerim - (int)dPerim) * 10)).ToString();
        }

        public void SetWaste(double l_waste)
        {
            dWaste = l_waste;
            lblWasteWhole.Text = ((int)dWaste).ToString("N0").Replace(".", ",");
            lblWasteDecimal.Text = ((int)(((decimal)dWaste % 1) * 10)).ToString() + "%";//((int)((dWaste - (int)dWaste) * 10)).ToString() + "%";
        }

        private void UCFinish_Click(object sender, EventArgs e)
        {
            oForm.SelectFinish(this.FinishName);
        }

        private void pnlColor_MouseDownClient(object sender, MouseEventArgs e)
        {
            oForm.SelectFinish(this.FinishName);
        }

        private void lblAreaWhole_Click(object sender, EventArgs e)
        {
            oForm.SelectFinish(this.FinishName);
        }

        private void lblAreaDecimal_Click(object sender, EventArgs e)
        {
            oForm.SelectFinish(this.FinishName);
        }

        private void lblAreaPoint_Click(object sender, EventArgs e)
        {
            oForm.SelectFinish(this.FinishName);
        }

        private void lblPerimWhole_Click(object sender, EventArgs e)
        {
            oForm.SelectFinish(this.FinishName);
        }

        private void lblPerimDecimal_Click(object sender, EventArgs e)
        {
            oForm.SelectFinish(this.FinishName);
        }

        private void lblPerimPoint_Click(object sender, EventArgs e)
        {
            oForm.SelectFinish(this.FinishName);
        }

        private void lblWasteWhole_Click(object sender, EventArgs e)
        {
            oForm.SelectFinish(this.FinishName);
        }

        private void lblWasteDecimal_Click(object sender, EventArgs e)
        {
            oForm.SelectFinish(this.FinishName);
        }

        private void lblWastePoint_Click(object sender, EventArgs e)
        {
            oForm.SelectFinish(this.FinishName);
        }

        private void SetLastSelectedText(string l_lastSelectedText)
        {
            oForm.SelectFinish(this.FinishName);
        }
    }
}
