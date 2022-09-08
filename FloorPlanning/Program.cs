using System;
//using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Drawing;

// PDFSharp
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Collections.Generic;
using FloorPlanning.Display.enums;

namespace FloorPlanning
{
    /// <summary>
    /// FloorPlanning Main Program
    /// </summary>
    class Program
    {
        public static DrawingForm mainForm;
        public static FloorPlanningSplash splash;
        public static string sProgramName = "FloorPlanning";
        public static Color MainTheme = Color.LightSteelBlue;
        public static Color SecondTheme = Color.LightSteelBlue;
        public static Color DesktopColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(68)))), ((int)(((byte)(106)))));

        public static DateTime LastModifiedStamp = DateTime.Now;
        public static DateTime LastModifiedServerProduct = DateTime.Now;
        public static bool bNewVersion = false;
        public static ManualResetEvent[] manualEvents = new ManualResetEvent[3];

        private static bool m_bSuccess;
        private static Mutex m_oMut;
         
        // App Data paths
        public static string appDataPath;


        // User variables
        public static string userName;
         
        public static object _lockerKey = new object();

        public static bool bLineMode = false;
        public static EditMode PanMode = EditMode.Page;
        public static UCFinish SelectedFinish = null;
        public static UCLine SelectedLine = null;
        public static int jump_b1_b2_dup_Mode = 1;
        public static bool bAreaZeroLine = false;
        public static bool bLineZeroLine = false;
        public static bool HideLineDoorTO = false;
        public static int n2xSeparation = 3;
        public static string LastOpenedJobFolder = "";
        public static string LastLoadedDrawingFolder = "";
        public static bool BackgroundVisible = true;
        public static System.Drawing.Font UIFont = new Font("Microsoft Sans Serif", (float)8.25);
        public static System.Drawing.Font ItemFont = new Font("Trebuchet MS", (float)18, FontStyle.Bold);

        public static string Empty_Project = "Empty Project";
        

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                m_oMut = new Mutex(true, "SingletonFloorPlanning", out m_bSuccess);
                MainTheme = Color.Tan;
                SecondTheme = Color.WhiteSmoke;
                DesktopColor = Color.WhiteSmoke;

                if (!m_bSuccess)
                {
                    GC.KeepAlive(m_oMut);
                    Application.Exit();
                    return;
                }

                Application.SetCompatibleTextRenderingDefault(false);

                Application.EnableVisualStyles();

                splash = new FloorPlanningSplash();

                splash.Show();
                SplashStatusUpdate("Initializing program.");
                DrawingDoc dDoc = new DrawingDoc();
                mainForm = new DrawingForm(dDoc);

                appDataPath = Application.StartupPath + "\\Configurations";
                AssureFolderExists(appDataPath);

                userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;                

                if (args.Length > 1)
                {
                    ;
                }
                else
                {
                    StartApplication();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private static bool AssureFolderExists(String folderPath)
        {
            // Does folder already exist?                    
            if (System.IO.Directory.Exists(folderPath))
            {
                // Folder exists
                return true;
            }
            else
            {
                // Attempt to create folder
                try
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                    return true;
                }
                catch
                {
                    Environment.Exit(-1);
                    return false;
                }
            }
        }

        private static void StartApplication()
        {
            SplashStatusUpdate("Floor Planning is loading.");
            mainForm.Show();
            mainForm.Update();
            splash.DoClose();
            Application.Run(mainForm);
        }

        private static void SplashStatusUpdate(String status)
        {
            splash.SetStatusLabel(status);
            //splash.StatusLabel.Text = status;
            splash.Update();
        }

    }
}
