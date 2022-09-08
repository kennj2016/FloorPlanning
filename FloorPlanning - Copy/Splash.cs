using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Timers;
using System.Diagnostics;


namespace FloorPlanning
{
    public partial class FloorPlanningSplash : Form
    {
        bool firstPaint = true;
        Stopwatch elapsed = new Stopwatch();

        public FloorPlanningSplash()
        {
            InitializeComponent();
            labelProductName.Text = AssemblyProduct;
        }

        public void SetStatusLabel(string status)
        {
            StatusLabel.Text = status;
        }

        public void DoClose()
        {
            long elapsedMilliseconds = elapsed.ElapsedMilliseconds;

            if (elapsedMilliseconds < 5000)
                System.Threading.Thread.Sleep(5000 - (int)elapsedMilliseconds);

            Stopwatch sw = new Stopwatch();
            double op = .99;

            sw.Start();
            while (op > 0)
            {
                op = Math.Max(0, (double)(792 - sw.ElapsedMilliseconds) / 800);
                this.Opacity = op;
                this.Update();
            }

            Close();
        }

        #region Assembly Attribute Accessors

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyProductAttribute productAttribute = (AssemblyProductAttribute)attributes[0];
                    if (productAttribute.Product != "")
                    {
                        return productAttribute.Product;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        #endregion

        private void Splash_Paint(object sender, PaintEventArgs e)
        {
            if (firstPaint)
            {
                firstPaint = false;
                labelProductName.Refresh();
                elapsed.Start();

                Stopwatch sw = new Stopwatch();
                double op = this.Opacity;

                sw.Start();
                while (op < 0.99)
                {
                    op = Math.Min(0.99, Math.Max(0.01, (double)(sw.ElapsedMilliseconds) / 600));
                    this.Opacity = op;
                    this.Update();
                }
            }
        }
    }
}
