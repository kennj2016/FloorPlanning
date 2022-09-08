using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using FloorPlanning;

namespace FloorPlanning
{
    public partial class FormChangeValFinish : Form
    {
        public Color selectedColor = Color.Green;
        DrawingForm oForm = null;
        public FormChangeValFinish(DrawingForm l_Form)
        {
            //// Make sure text is rendered correctly
            //Graphics gfx = this.CreateGraphics();
            //gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            //gfx.Save();

            InitializeComponent();
            oForm = l_Form;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        { 

        }

        public void SetVals(string l_Name, double l_dArea, double l_dPerim, double l_dWaste)
        {
            txtName.Text = l_Name;
            nArea.Value = l_dArea;
            nPerimeter.Value = l_dPerim;
            nWaste.Value = l_dWaste;
        }

        public double GetArea()
        {
            return (double)nArea.Value;
        }

        public double GetPerim()
        {
            return (double)nPerimeter.Value;
        }

        public double GetWaste()
        {
            return (double)nWaste.Value;
        }
    }
}
