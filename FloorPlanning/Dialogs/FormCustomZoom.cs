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
    public partial class FormCustomZoom : Form
    {
        public FormCustomZoom()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }

        public float GetZoom() 
        {
            float f = -1; 
            try
            {
                f = Convert.ToSingle(cmbZoom.Text);
                if ((f < 0) || (f > 800))
                {
                    f = -1f;
                }
                return f;
            }
            catch
            {
                f = -1f;
            }
            return f;
        }
    }
}
