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
    public partial class FormNewShape : Form
    {
        public Color selectedColor = Color.Green;
        public short Alpha = 255;
        DrawingForm oForm = null;
        private bool bEdit = false;
        private string prevName = "";

        public FormNewShape(DrawingForm l_Form)
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

        public void SetEdit(string l_Name, Color l_Color, short l_Alpha)
        {
            prevName = l_Name;
            this.Text = "Shape Dialog - Edit Shape";
            ultraColorPicker1.Color = l_Color;
            pnlColor.Appearance.BackColor = l_Color;
            pnlColor.Appearance.AlphaLevel = l_Alpha;
            barTransp.Value = l_Alpha;
            selectedColor = l_Color;
            Alpha = l_Alpha;
            txtName.Text = l_Name;
            bEdit = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {            
            selectedColor = ultraColorPicker1.Color;
            if (
                ((oForm.bExistsFinishName(txtName.Text)) && (bEdit == false)) ||
                ((oForm.bExistsFinishName(txtName.Text)) && (bEdit == true) && (txtName.Text != prevName))
                )
            {
                MessageBox.Show("There is already a Finish named " + txtName.Text + " in the system. Please enter a new name and try again.", "Finish", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else if (txtName.Text == "")
            {
                MessageBox.Show("The Finish name can't be empty. Please enter a name and try again.", "Finish", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        public Color GetColor()
        {
            return selectedColor;
        }

        public short GetAlpha()
        {
            return Alpha;
        }

        public string GetName()
        {
            return txtName.Text;
        }

        private void barTransp_ValueChanged(object sender, EventArgs e)
        {
            pnlColor.Appearance.AlphaLevel = (short)barTransp.Value;
            Alpha = (short)barTransp.Value;
        }

        private void ultraColorPicker1_ColorChanged(object sender, EventArgs e)
        {
            pnlColor.Appearance.BackColor = ultraColorPicker1.Color;
        }
    }
}
