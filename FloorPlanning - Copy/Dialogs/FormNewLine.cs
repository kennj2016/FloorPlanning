using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using FloorPlanning;
using FloorPlanning.Models;

namespace FloorPlanning
{
    public partial class FormNewLine : Form
    {
        public Color selectedColor = Color.Green;
        public short Alpha = 255;
        DrawingForm oForm = null;
        private bool bEdit = false;
        private string prevName = ""; 
        private Base b; 

        public FormNewLine(DrawingForm l_Form)
        {
            //// Make sure text is rendered correctly
            //Graphics gfx = this.CreateGraphics();
            //gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            //gfx.Save();

            InitializeComponent();
            b = new Base(0, "", 0, 0, 0, 0, 0, 0, 0, 0);
            selectedColor = ultraColorPicker1.Color;
            cmbDashType.SelectedIndex = 0;
            cmdDashCap.SelectedIndex = 0;
            cmbDashStyle.SelectedIndex = 0;
            b.CapType = b.DashStyle = b.DashType = 0;
            b.Width = 1;
            b.R = selectedColor.R;
            b.G = selectedColor.G;
            b.B = selectedColor.B;
            b.A = Alpha;
            oForm = l_Form;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }

        public void SetEdit(Base l_b)
        {
            b = l_b;
            prevName = b.sName;
            this.Text = "Line Dialog - Edit Line";
            ultraColorPicker1.Color = Color.FromArgb(255, b.R, b.G, b.B);
            barTransp.Value = b.A;
            selectedColor = ultraColorPicker1.Color;
            Alpha = (short)b.A;
            txtName.Text = b.sName;

            cmbDashType.SelectedIndex = b.DashType;
            cmdDashCap.SelectedIndex = b.CapType;
            cmbDashStyle.SelectedIndex = b.DashStyle;
            nWidth.Value = b.Width;

            bEdit = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {            
            selectedColor = ultraColorPicker1.Color;
            b.CapType = cmdDashCap.SelectedIndex;
            b.DashStyle = cmbDashStyle.SelectedIndex;
            b.DashType = cmbDashType.SelectedIndex;
            b.Width = (int)nWidth.Value;
            b.R = selectedColor.R;
            b.G = selectedColor.G;
            b.B = selectedColor.B;
            b.sName = txtName.Text;
            Alpha = (short)barTransp.Value;
            b.A = Alpha;
            if (
                ((oForm.bExistsLineName(txtName.Text)) && (bEdit == false)) ||
                ((oForm.bExistsLineName(txtName.Text)) && (bEdit == true) && (txtName.Text != prevName))
                )
            {
                MessageBox.Show("There is already a Line named " + txtName.Text + " in the system. Please enter a new name and try again.", "Line", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else if (txtName.Text == "")
            {
                MessageBox.Show("The Line name can't be empty. Please enter a name and try again.", "Line", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        public Base GetBase()
        {
            return b;
        }

        private void barTransp_ValueChanged(object sender, EventArgs e)
        {
            //pnlColor.Appearance.AlphaLevel = (short)barTransp.Value;
            Alpha = (short)barTransp.Value;
            Redo();
        }

        private void ultraColorPicker1_ColorChanged(object sender, EventArgs e)
        {
            Redo();
        }

        private void ShowPensAndSmoothingMode(PaintEventArgs e)
        {
            try
            {
                // Set the SmoothingMode property to smooth the line.
                e.Graphics.SmoothingMode =
                    System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Create a new Pen object.
                Pen pen = new Pen(ultraColorPicker1.Color);

                // Set the width to 3.
                pen.Width = (float)nWidth.Value;

                // Set the DashCap
                if (cmdDashCap.SelectedIndex == 0)
                {
                    pen.DashCap = System.Drawing.Drawing2D.DashCap.Flat;
                }
                else
                {
                    pen.DashCap = System.Drawing.Drawing2D.DashCap.Round;
                }

                // Create a custom dash pattern.
                if (cmbDashStyle.SelectedIndex == 0)
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                }
                else if (cmbDashStyle.SelectedIndex == 1)
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                }
                else if (cmbDashStyle.SelectedIndex == 2)
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                }
                else
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                }

                // Draw a line
                if (cmbDashType.SelectedIndex == 0)
                {
                    e.Graphics.DrawLine(pen, 40.0F, 25.0F, 240.0F, 25.0F);
                }
                else
                {
                    e.Graphics.DrawLine(pen, 40.0F, 20.0F, 240.0F, 20.0F);
                    e.Graphics.DrawLine(pen, 40.0F, 30.0F, 240.0F, 30.0F);
                }
                // Dispose of the custom pen.
                pen.Dispose();
            }
            catch { }
        }

        private void FormNewLine_Paint(object sender, PaintEventArgs e)
        {
            //ShowPensAndSmoothingMode(e);
        }

        private void nWidth_ValueChanged(object sender, EventArgs e)
        {
            Redo();
        }

        private void cmbDashType_ValueChanged(object sender, EventArgs e)
        {
            Redo();
        }

        private void cmdDashCap_ValueChanged(object sender, EventArgs e)
        {
            Redo();
        }

        private void cmbDashStyle_ValueChanged(object sender, EventArgs e)
        {
            Redo();
        }

        private void Redo()
        {
            this.pnlDrawLine.ClientArea.Invalidate();
        }

        private void pnlBack_Paint(object sender, PaintEventArgs e)
        {
            ShowPensAndSmoothingMode(e);
        }
    }
}
