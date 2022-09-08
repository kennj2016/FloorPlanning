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
    public partial class FormSaveAsProject : Form
    {
        public string sLastPath = "";
        public string sNewName = "";
        public FormSaveAsProject()
        {
            //// Make sure text is rendered correctly
            //Graphics gfx = this.CreateGraphics();
            //gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            //gfx.Save();

            InitializeComponent();

            if (System.IO.Directory.Exists(Program.LastOpenedJobFolder))
            {
                sLastPath = Program.LastOpenedJobFolder;
            }
            else
            {
                sLastPath = Program.appDataPath;
            }
        }

        public FormSaveAsProject(bool convertFromScrapbook)
        {
            InitializeComponent();

            if (System.IO.Directory.Exists(Program.LastOpenedJobFolder))
                sLastPath = Program.LastOpenedJobFolder;
        }
        
        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }


        private void SaveAs()
        {
            SaveFileDialog SaveFileDialog1 = new SaveFileDialog(); 
            SaveFileDialog1.OverwritePrompt = true;
            SaveFileDialog1.FileName = txtName.Text;
            // DefaultExt is only used when "All files" is selected from 
            // the filter box and no extension is specified by the user.
            SaveFileDialog1.DefaultExt = "fpp";
            SaveFileDialog1.Filter = "Floor Planning files (*.fpp)|*.fpp";
            SaveFileDialog1.InitialDirectory = sLastPath;

            if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                sLastPath = Path.GetDirectoryName(SaveFileDialog1.FileName);
                Program.LastOpenedJobFolder = sLastPath;
                sNewName = txtName.Text;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveAs();
        }
    }
}
