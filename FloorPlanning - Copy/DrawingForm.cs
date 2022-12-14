using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.IO;

// PDFSharp
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;

// FloorPlanning
using FloorPlanning.Display;
using FloorPlanning.Display.enums;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization.Formatters.Binary;
using Infragistics.Win.UltraWinToolbars;
using System.Linq;
using FloorPlanning.Models;
using System.Text;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Reflection;
using Infragistics.Win.Misc;
namespace FloorPlanning
{
    /// <summary>
    /// Drawing Editor Form
    /// </summary>
    public partial class DrawingForm : System.Windows.Forms.Form
    {
        // Flag to indicated that if an editing command is canceled,
        // we must Undo back to the original, unedited drawing entity.
        public bool undoOnCancel;

        CompatiblePdfDocument pdfDoc;
        public Dictionary<string, int> dBOM = new Dictionary<string, int>();
        public bool IsDragOp = false;
        public delegate void CommandChangedEventHandler(DrawingForm sender, EventArgs e);


        String softwareName;
        FloorPlanningProject currentProject;

        Thread tBackground = null;

        private ComponentDef currentComponent;
        private bool comeFromClick = false;

        ImageList lCurrentThumbs = new ImageList();

        List<UCFinish> lUCFinish = new List<UCFinish>();
        public ListFinishLine lConfigFinishLine = new ListFinishLine();
        string sFinishIniFile = Application.StartupPath + "\\finishes.ini";
        List<UCLine> lUCLine = new List<UCLine>();
        // Command change notification handler
        public event CommandChangedEventHandler ChangedCommand;

        PointF initialPoint = new PointF(0, 0);
        PointF endPoint = new PointF(0, 0);
        // Invoke the OnChanged event; called whenever currentCommand changes
        protected virtual void OnChanged(EventArgs e)
        {
            if (ChangedCommand != null)
                ChangedCommand(this, e);
        }

        private void LoadTheme()
        {
            this.BackColor = Program.SecondTheme;
            this.pageWindow.BackColor = Program.SecondTheme;
            this.pageWindow.DesktopColor = Program.DesktopColor;//System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(68)))), ((int)(((byte)(106)))));
        }

        private void InitDefault()
        {
            try
            {
                lConfigFinishLine = new ListFinishLine();
                int NumFinish = 7;
                Color[] lColors = new Color[] { Color.Yellow, Color.Green, Color.Pink, Color.Blue, Color.Orange, Color.Red, Color.LightYellow };
                String[] lNames = new String[] { "Finish-1", "Finish-2", "Finish-3", "Finish-4", "Finish-5", "Finish-6", "Finish-7" };
                String[] lLinesNames = new String[] { "Line-1", "Line-2", "Line-3", "Line-4", "Line-5", "Line-6", "Line-7" };

                Base line1 = new Base(1, lLinesNames[0], 255, Color.Red, 2, 0, 0, 0);
                Base line2 = new Base(2, lLinesNames[1], 255, Color.Green, 2, 0, 1, 2);
                Base line3 = new Base(3, lLinesNames[2], 255, Color.Gray, 2, 0, 1, 1);
                Base line4 = new Base(4, lLinesNames[3], 255, Color.Blue, 2, 0, 1, 3);
                Base line5 = new Base(5, lLinesNames[4], 255, Color.Orange, 2, 0, 0, 0);
                Base line6 = new Base(6, lLinesNames[5], 255, Color.Black, 2, 0, 0, 0);
                Base line7 = new Base(7, lLinesNames[6], 255, Color.Red, 2, 0, 0, 0);
                lConfigFinishLine.lLine.Add(line1);
                lConfigFinishLine.lLine.Add(line2);
                lConfigFinishLine.lLine.Add(line3);
                lConfigFinishLine.lLine.Add(line4);
                lConfigFinishLine.lLine.Add(line5);
                lConfigFinishLine.lLine.Add(line6);
                lConfigFinishLine.lLine.Add(line7);


                for (int i = 0; i < NumFinish; i++)
                {
                    Finish f = new Finish();
                    f.A = 128;
                    f.R = (int)lColors[i].R;
                    f.G = (int)lColors[i].G;
                    f.B = (int)lColors[i].B;
                    f.nIndex = i + 1;
                    f.sName = lNames[i];
                    lConfigFinishLine.lFinish.Add(f);
                }

                WriteFinish(sFinishIniFile);
            }
            catch { }
        }

        private void InitFinishes()
        {
            //Autostart
            try
            {
                if (!File.Exists(sFinishIniFile))
                {
                    InitDefault();
                }

                if (LoadFinish(sFinishIniFile) == false)
                {
                    InitDefault();
                    LoadFinish(sFinishIniFile);
                }
            }
            catch { }
        }

        public DrawingForm(DrawingDoc drawing)
        {
            InitializeComponent();
            this.Icon = global::FloorPlanning.Properties.Resources.icon1;

            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, ultraPanelRight, new object[] { true });
            //LoadTheme();
            pnlAreaModeInRight.BringToFront();
            this.Text = "Floor Planning";
            softwareName = this.Text;

            this.KeyPreview = true;//MAG
            this.KeyDown += new KeyEventHandler(DrawingForm_KeyDown);
            this.KeyPress += new KeyPressEventHandler(DrawingForm_KeyPress);
            drawingDoc = drawing;
            pdfDoc = null;

            SetTitleBar();

            this.Width = 1160;
            this.Height = 720;

            this.pageWindow.PageSize = PageSizeConverter.ToSize(PageSize.A4);
            this.pageWindow.Orientation = drawing.Orientation;
            this.pageWindow.Zoom = Zoom.FullPage;
            InitFinishes();
            currentCommand = CommandMode.None;
            HidePrompt();
            UpdateStatusBar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        //https://stackoverflow.com/questions/12160786/winforms-tabcontrol-hide-tabs-and-disable-all-keyboard-shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //if (keyData == (Keys.Control | Keys.Tab) || keyData == (Keys.Control | Keys.Shift | Keys.Tab) || keyData == Keys.Left || keyData == Keys.Right || keyData == Keys.Home || keyData == Keys.End)
            //{
            //    return true;
            //}

            if (keyData == Keys.Left || keyData == Keys.Right)
            {
                if ((drawingDoc.TempBaseImage != null) || (drawingDoc.BaseImage != null))
                {
                    //Kiet Nguyen: [1500] shortcuts
                    var actionObj = this.lConfigFinishLine.lShortcuts.Where(x => x.Keystroke == (char)keyData).FirstOrDefault();
                    if (actionObj.Action == ShortcutsAction.GeneralMode_Move_left.ToString())
                    {
                        this.pageWindow.moveLayourtToLeft();
                        return true;
                    }
                    else if (actionObj.Action == ShortcutsAction.GeneralMode_Move_right.ToString())
                    {
                        this.pageWindow.moveLayourtToRight();
                        return true;
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void DrawingForm_KeyDown(object sender, KeyEventArgs e)
        {
            //pageWindow.PageWindowKeyDownEvent(sender, e);
            var keyValue = (char)e.KeyCode;
            if ((drawingDoc.TempBaseImage != null) || (drawingDoc.BaseImage != null))
            {
                //Kiet Nguyen: [1500] shortcuts
                var actionObj = this.lConfigFinishLine.lShortcuts.Where(x => x.Keystroke == keyValue).FirstOrDefault();
                if (actionObj != null)
                {
                    if (actionObj.Area == "GENERA")
                    {
                        if (actionObj.Action == ShortcutsAction.GeneralMode_Move_left.ToString())
                        {
                            this.pageWindow.moveLayourtToLeft();
                        }
                        else if (actionObj.Action == ShortcutsAction.GeneralMode_Move_right.ToString())
                        {
                            this.pageWindow.moveLayourtToRight();
                        }
                        else if (actionObj.Action == ShortcutsAction.GeneralMode_Move_up.ToString())
                        {
                            this.pageWindow.moveLayourtToUp();
                        }
                        else if (actionObj.Action == ShortcutsAction.GeneralMode_Move_down.ToString())
                        {
                            this.pageWindow.moveLayourtDown();
                        }
                        else if (actionObj.Action == ShortcutsAction.GeneralMode_Zoom_in.ToString())
                        {
                            this.pageWindow.ProportionalZoom(true);
                        }
                        else if (actionObj.Action == ShortcutsAction.GeneralMode_Zoom_out.ToString())
                        {
                            this.pageWindow.ProportionalZoom(false);
                        }
                        else if (actionObj.Action == ShortcutsAction.GeneralMode_Toggle_area_base_modes.ToString())
                        {
                            if (tabPanes.SelectedTab.Text == "Pages")
                            {
                                //Change to Area
                                selectedAreaMode();
                            }
                            else if (tabPanes.SelectedTab.Text == "Area")
                            {
                                //Change to Lines
                                selectedLineMode();
                            }
                            else if (tabPanes.SelectedTab.Text == "Lines")
                            {
                                //Change to Area
                                selectedAreaMode();
                            }
                        }
                    }
                    else if (tabPanes.SelectedTab.Text == "Area" && ((StateButtonTool)ultraToolbarsManager1.Tools["AreaMode"]).Checked && ((StateButtonTool)ultraToolbarsManager1.Tools["DrawMode"]).Checked)
                    {
                        actionObj = this.lConfigFinishLine.lShortcuts.Where(x => x.Area == "AREA" && x.Keystroke == keyValue).FirstOrDefault();
                        if (actionObj != null)
                        {
                            if (actionObj.Action == ShortcutsAction.AreaMode_Erase_last_line.ToString())
                            {
                                pageWindow.RemoveLastPoint();
                            }
                            else if (actionObj.Action == ShortcutsAction.AreaMode_Complete_shape.ToString())
                            {
                                drawingDoc.TempPolygon.IsBeingEdited = false;
                                AcceptCommand();
                                pageWindow.ClearGrips();
                                pageWindow.Regen();
                            }
                            //Cancel shape in progress
                            else if (actionObj.Action == ShortcutsAction.AreaMode_Cancel_shape_in_progress.ToString())
                            {
                                //No need to remove point in array
                                //bRemoveEntity = drawingDoc.TempPolygon.RemoveLastPoint();
                                //pageWindow.FirstPointPicked = drawingDoc.TempPolygon.LastPoint;
                                SilentCancelCommand();
                                pageWindow.ClearGrips();
                                pageWindow.Regen();
                                DrawComponent();
                            }
                            else if (actionObj.Action == ShortcutsAction.AreaMode_Zero_line.ToString())
                            {
                                Program.bAreaZeroLine = !Program.bAreaZeroLine;
                                if (drawingDoc.TempPolygon != null)
                                {
                                    drawingDoc.TempPolygon.UpdateLastZeroLine();
                                }
                                SelectButton(btnAreaZeroLine, Program.bAreaZeroLine);

                            }
                        }
                    }
                    else if (tabPanes.SelectedTab.Text == "Lines" && ((StateButtonTool)ultraToolbarsManager1.Tools["DrawMode"]).Checked)
                    {
                        actionObj = this.lConfigFinishLine.lShortcuts.Where(x => x.Area == "LINE" && x.Keystroke == keyValue).FirstOrDefault();
                        if (actionObj != null)
                        {
                            if (actionObj.Action == ShortcutsAction.LineMode_Erase_last_line.ToString())
                            {
                                RemovePointLine();
                            }
                            else if (actionObj.Action == ShortcutsAction.LineMode_Jump.ToString())
                            {
                                completeLine_jum();
                            }
                            else if (actionObj.Action == ShortcutsAction.LineMode_Single_line.ToString())
                            {
                                Program.jump_b1_b2_dup_Mode = 1;
                                LineModeSel();
                            }
                            else if (actionObj.Action == ShortcutsAction.LineMode_Double_line.ToString())
                            {
                                Program.jump_b1_b2_dup_Mode = 2;
                                LineModeSel();
                            }
                            else if (actionObj.Action == ShortcutsAction.LineMode_Duplicate_line.ToString())
                            {
                                Program.jump_b1_b2_dup_Mode = 3;
                                LineModeSel();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Kiet.Nguyen: undo point 
        /// </summary>
        private void RemovePointLine()
        {
            pageWindow.RemoveLastPoint();
            //Kiet Nguyen: Disable [Duplicate] button when cancel action 2x
            //   btnDuplicate.Enabled = false;
        }

        /// <summary>
        /// Kiet.Nguyen: complete draw line
        /// </summary>
        private void completeLine_jum()
        {
            Program.jump_b1_b2_dup_Mode = 0;
            LineModeSel();

            if (drawingDoc.TempPolyline != null)
            {
                //CurrentCommand = CommandMode.PlineClose;
                drawingDoc.TempPolyline.IsBeingEdited = false;
                AcceptCommand();
                pageWindow.ClearGrips();
                pageWindow.Regen();
            }
        }

        /// <summary>
        /// using keyDown event install of  DrawingForm_KeyPress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DrawingForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Kiet Nguyen:[1500] disable for shortcuts
            //if (((this.ActiveControl is UltraButton) || (this.ActiveControl is Button)) && (e.KeyChar == (char)Keys.Space))
            //{
            //    var button = this.ActiveControl;
            //    button.Enabled = false;
            //    Application.DoEvents();
            //    button.Enabled = true;
            //    this.pageWindow.Focus();

            //Kiet Nguyen: Disable [Duplicate] button when cancel action 2x
            //    btnDuplicate.Enabled = false;
            //}            
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitProject()
        {
            try
            {
                String drawingKey = "";

                Program.LastOpenedJobFolder = Program.appDataPath;
                CreateProject(Program.appDataPath, Program.Empty_Project);

                drawingKey = currentProject.AddDrawing(currentProject.Name, DrawingDoc.DrawingType.Plan);
                drawingDoc = currentProject.Drawings[drawingKey];
                drawingDoc.SetDrawingForm(this);
                EnableButtons();
                this.SetTitleBar();
                this.pageWindow.Zoom = Zoom.FullPage;
                this.pageWindow.Regen(true);
                SetAllThumbnailsInListView();
                lblLastAreaWhole.Text = lblLastAreaDecimal.Text = lblLastPerimeterWhole.Text = lblLastPerimeterDecimal.Text = "0";
                lblLastTotalPerimeterDecimal.Text = lblLastTotalPerimeterWhole.Text = "0";
                lblLastAreaTotalWhole.Text = lblLastAreaTotalDecimal.Text = "0";
                lblLastWasteWhole.Text = "0";
                lblLastWasteDecimal.Text = "0%";

                CalculateAll();

                //Kiet Nguyen
                cmbScale.DisplayMember = "DisplayText";
                cmbScale.ValueMember = "Value";
            }
            catch { }
        }

        private void WriteFinish(string sFile)
        {
            try
            {
                byte[] dataOut = null;
                using (var msOut = new MemoryStream())
                {
                    var serializer = new XmlSerializer(typeof(ListFinishLine));
                    serializer.Serialize(msOut, lConfigFinishLine);
                    dataOut = msOut.ToArray();
                }

                EncryptFile(dataOut, sFile);
            }
            catch
            {
                ;
            }
        }

        private bool SaveFinish()
        {
            try
            {
                lConfigFinishLine.lFinish = new List<Finish>();
                for (int i = 0; i < lUCFinish.Count; i++)
                {
                    Finish f = new Finish();
                    f.A = lUCFinish[i].Alpha;
                    f.R = lUCFinish[i].pnlBackColor.R;
                    f.G = lUCFinish[i].pnlBackColor.G;
                    f.B = lUCFinish[i].pnlBackColor.B;
                    f.sName = lUCFinish[i].FinishName;
                    lConfigFinishLine.lFinish.Add(f);
                }


                lConfigFinishLine.lLine = new List<Base>();
                for (int i = 0; i < lUCLine.Count; i++)
                {
                    lConfigFinishLine.lLine.Add(lUCLine[i].b);
                }

                WriteFinish(sFinishIniFile);
                return true;
            }
            catch
            {
            }
            return false;
        }

        private bool SaveLine()
        {
            try
            {
                lConfigFinishLine.lFinish = new List<Finish>();
                for (int i = 0; i < lUCFinish.Count; i++)
                {
                    Finish f = new Finish();
                    f.A = lUCFinish[i].Alpha;
                    f.R = lUCFinish[i].pnlBackColor.R;
                    f.G = lUCFinish[i].pnlBackColor.G;
                    f.B = lUCFinish[i].pnlBackColor.B;
                    f.sName = lUCFinish[i].FinishName;
                    lConfigFinishLine.lFinish.Add(f);
                }
                WriteFinish(sFinishIniFile);
                return true;
            }
            catch
            {
            }
            return false;
        }

        private bool LoadFinish(string sFile)
        {
            try
            {
                byte[] outB = DecryptFile(sFile);

                using (var reader = new MemoryStream(outB))
                {
                    var serializer = new XmlSerializer(typeof(ListFinishLine));
                    lConfigFinishLine = (ListFinishLine)serializer.Deserialize(reader);
                    for (int i = 0; i < lConfigFinishLine.lFinish.Count; i++)
                    {
                        Color col = Color.FromArgb(lConfigFinishLine.lFinish[i].R, lConfigFinishLine.lFinish[i].G, lConfigFinishLine.lFinish[i].B);
                        UCFinish uc = new UCFinish(this);
                        uc.SetColor(col, (byte)lConfigFinishLine.lFinish[i].A);
                        uc.SetFinishName(lConfigFinishLine.lFinish[i].sName);
                        uc.Select(false);
                        uc.nIndex = lConfigFinishLine.lFinish[i].nIndex;
                        uc.SetWaste(0);
                        pnlFinishUCs.ClientArea.Controls.Add(uc);
                        uc.Dock = DockStyle.Top;
                        uc.nIndex = lUCFinish.Count + 1;
                        uc.BringToFront();
                        lUCFinish.Add(uc);
                    }

                    lUCFinish[0].Select(true);
                    Program.SelectedFinish = lUCFinish[0];
                    lblSelectedArea.Text = Program.SelectedFinish.FinishName;

                    for (int i = 0; i < lConfigFinishLine.lLine.Count; i++)
                    {
                        Color col = Color.FromArgb(lConfigFinishLine.lLine[i].R, lConfigFinishLine.lLine[i].G, lConfigFinishLine.lLine[i].B);
                        UCLine uc = new UCLine(this);
                        uc.SetBase(lConfigFinishLine.lLine[i]);
                        uc.Select(false);
                        uc.nIndex = lConfigFinishLine.lLine[i].nIndex;
                        pnlLineUCs.ClientArea.Controls.Add(uc);
                        uc.Dock = DockStyle.Top;
                        uc.nIndex = lUCLine.Count + 1;
                        uc.BringToFront();
                        lUCLine.Add(uc);
                    }
                    lUCLine[0].Select(true);
                    Program.SelectedLine = lUCLine[0];
                }

                //Kiet Nguyen: Sheet 400 init scales list
                if (lConfigFinishLine.lScales.Count == 0)
                {
                    initScaleList();
                }
                cmbScale.DataSource = lConfigFinishLine.lScales;
                cmbScale.SelectedIndex = 0;

                return true;
            }
            catch (Exception ex)
            {
                ;
            }
            return false;
        }

        /// <summary>
        /// Kiet Nguyen: Sheet 400
        /// </summary>
        private void initScaleList()
        {

            Scale objNone = new Scale();
            objNone.Value = "None";
            objNone.DisplayText = "None";
            lConfigFinishLine.lScales.Add(objNone);

            Scale obj1_32 = new Scale();
            obj1_32.Value = "1/32";
            obj1_32.DisplayText = "1/32";
            lConfigFinishLine.lScales.Add(obj1_32);

            Scale obj3_64 = new Scale();
            obj3_64.Value = "3/64";
            obj3_64.DisplayText = "3/64";
            lConfigFinishLine.lScales.Add(obj3_64);

            Scale obj1_16 = new Scale();
            obj1_16.Value = "1/16";
            obj1_16.DisplayText = "1/16";
            lConfigFinishLine.lScales.Add(obj1_16);

            Scale obj3_32 = new Scale();
            obj3_32.Value = "3/32";
            obj3_32.DisplayText = "3/32";
            lConfigFinishLine.lScales.Add(obj3_32);

            Scale obj1_8 = new Scale();
            obj1_8.Value = "1/8";
            obj1_8.DisplayText = "1/8";
            lConfigFinishLine.lScales.Add(obj1_8);

            Scale obj3_16 = new Scale();
            obj3_16.Value = "3/16";
            obj3_16.DisplayText = "3/16";
            lConfigFinishLine.lScales.Add(obj3_16);

            Scale obj1_4 = new Scale();
            obj1_4.Value = "1/4";
            obj1_4.DisplayText = "1/4";
            lConfigFinishLine.lScales.Add(obj1_4);

            Scale obj3_8 = new Scale();
            obj3_8.Value = "3/8";
            obj3_8.DisplayText = "3/8";
            lConfigFinishLine.lScales.Add(obj3_8);

            Scale obj1_2 = new Scale();
            obj1_2.Value = "1/2";
            obj1_2.DisplayText = "1/2";
            lConfigFinishLine.lScales.Add(obj1_2);

            Scale obj3_4 = new Scale();
            obj3_4.Value = "3/4";
            obj3_4.DisplayText = "3/4";
            lConfigFinishLine.lScales.Add(obj3_4);

            Scale objCustom = new Scale();
            objCustom.Value = "Custom";
            objCustom.DisplayText = "Custom";
            lConfigFinishLine.lScales.Add(objCustom);
        }


        ///<summary>
        /// Steve Lydford - 12/05/2008.
        ///
        /// Encrypts a file using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        public static void EncryptFile(byte[] inputBytes/*string inputFile*/, string outputFile)
        {

            try
            {
                string password = @"34FEdsg4";
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                //FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                //int data;
                //while ((data = fsIn.ReadByte()) != -1)
                //    cs.WriteByte((byte)data);
                for (long i = 0; i < inputBytes.LongLength; i++)
                {
                    cs.WriteByte(inputBytes[i]);
                }

                //fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch
            {
                MessageBox.Show("Encryption failed!", "Error");
            }
        }
        ///<summary>
        /// Steve Lydford - 12/05/2008.
        ///
        /// Decrypts a file using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        public static byte[] DecryptFile(string inputFile)
        {
            try
            {
                string password = @"34FEdsg4";

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                //FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                //while ((data = cs.ReadByte()) != -1)
                //    fsOut.WriteByte((byte)data);

                //fsOut.Close();
                byte[] outBytes = null;
                List<byte> lOut = new List<byte>();
                long i = 0;
                while ((data = cs.ReadByte()) != -1)
                {
                    lOut.Add((byte)data);
                }
                outBytes = lOut.ToArray();
                cs.Close();
                fsCrypt.Close();
                return outBytes;
            }
            catch { }
            return null;
        }

        public double GetTrim()
        {
            double Trim = 0;
            try
            {
                Trim = Double.Parse(cmbTrimFactor.Text);
            }
            catch { }
            return Trim;
        }

        public void CalculateAll()
        {
            try
            {
                Dictionary<int, double> dAreas = new Dictionary<int, double>();
                Dictionary<int, double> dPerim = new Dictionary<int, double>();
                double Trim = GetTrim();
                for (int i = 1; i < lConfigFinishLine.lFinish.Count; i++)
                {
                    dAreas.Add(i, 0);
                    dPerim.Add(i, 0);
                }

                foreach (DrawingEntity e in drawingDoc.Entities)
                {
                    try
                    {
                        if (e.Type == DrawingEntity.EntityType.Polygon)
                        {
                            int nIndex = Int32.Parse(((Polygon)e).Component.UniqueCode);
                            if (((Polygon)e).Component.bRemove)
                            {
                                dAreas[nIndex] -= drawingDoc.GetEntityArea((Polygon)e);
                                dPerim[nIndex] += drawingDoc.GetEntityPerimeter((Polygon)e);
                            }
                            else
                            {
                                dAreas[nIndex] += drawingDoc.GetEntityArea((Polygon)e);
                                dPerim[nIndex] += drawingDoc.GetEntityPerimeter((Polygon)e);
                            }
                        }
                    }
                    catch { }
                }

                foreach (UCFinish uc in lUCFinish)
                {
                    try
                    {
                        uc.SetArea(dAreas[uc.nIndex]);
                        uc.SetPerim(dPerim[uc.nIndex]);
                        uc.SetWaste(dPerim[uc.nIndex] * Trim);
                    }
                    catch { }
                }

                double TotalArea = 0;
                double TotalPerimeter = 0;
                foreach (double a in dAreas.Values)
                {
                    TotalArea += a;
                }
                foreach (double a in dPerim.Values)
                {
                    TotalPerimeter += a;
                }

                lblLastAreaTotalWhole.Text = ((int)TotalArea).ToString("N0").Replace(".", ",");
                lblLastAreaTotalDecimal.Text = ((int)(((decimal)TotalArea % 1) * 10)).ToString();
                lblLastTotalPerimeterWhole.Text = ((int)TotalPerimeter).ToString("N0").Replace(".", ",");
                lblLastTotalPerimeterDecimal.Text = ((int)(((decimal)TotalPerimeter % 1) * 10)).ToString();
            }
            catch { }


            //Lines
            try
            {
                Dictionary<int, double> dPerim = new Dictionary<int, double>();
                for (int i = 1; i < lConfigFinishLine.lLine.Count; i++)
                {
                    dPerim.Add(i, 0);
                }

                foreach (DrawingEntity e in drawingDoc.Entities)
                {
                    int nIndex = 0;
                    try
                    {
                        if (e.Type == DrawingEntity.EntityType.Polygon)
                        {
                            Polygon p = (Polygon)e;
                            nIndex = p.Component.B.nIndex;
                            if (nIndex >= 1000)
                            {
                                nIndex -= 1000;
                            }
                            if (p.Component.bAreaZeroLine) //Remove
                            {
                                ;// dPerim[nIndex] -= drawingDoc.GetEntityPerimeter(p);
                            }
                            else
                            {
                                dPerim[nIndex] += drawingDoc.GetEntityPerimeter(p);
                            }
                        }
                        if (e.Type == DrawingEntity.EntityType.DoorTakeOut)
                        {
                            DoorTakOut d = (DoorTakOut)e;
                            nIndex = d.Component.B.nIndex;
                            if (nIndex >= 1000)
                            {
                                nIndex -= 1000;
                            }
                            dPerim[nIndex] -= d.Perimeter;
                        }
                        if (e.Type == DrawingEntity.EntityType.Polyline)
                        {
                            Polyline l = (Polyline)e;
                            nIndex = l.Component.B.nIndex;
                            if (nIndex >= 1000)
                            {
                                nIndex -= 1000;
                            }
                            float perim = drawingDoc.GetEntityPerimeter(l);
                            if (l.dashType == 1)//doubleline
                            {
                                perim *= 2;
                            }
                            if (l.Component.bLineZeroLine) //Remove
                            {
                                dPerim[nIndex] -= perim;
                            }
                            else
                            {
                                dPerim[nIndex] += perim;
                            }
                        }
                    }
                    catch { }
                }

                foreach (UCLine uc in lUCLine)
                {
                    try
                    {
                        uc.SetPerim(dPerim[uc.nIndex]);
                    }
                    catch { }
                }

                double TotalPerimeter = 0;
                foreach (double a in dPerim.Values)
                {
                    TotalPerimeter += a;
                }

                lblTotalLineWhole.Text = ((int)TotalPerimeter).ToString("N0").Replace(".", ",");
                lblTotalLineDecimal.Text = ((int)(((decimal)TotalPerimeter % 1) * 10)).ToString();

            }
            catch { }
        }

        public void ApplyChangesToFinish()
        {
            try
            {
                foreach (DrawingEntity e in drawingDoc.Entities)
                {
                    try
                    {
                        if (e.Type == DrawingEntity.EntityType.Polygon)
                        {
                            Polygon po = (Polygon)e;
                            if (po.Component.bRemove == false)
                            {
                                int nIndex = Int32.Parse(((Polygon)e).Component.UniqueCode);
                                if (nIndex == Program.SelectedFinish.nIndex)
                                {
                                    po.SetComponent(currentComponent);
                                }
                            }
                        }
                    }
                    catch { }
                }
                pageWindow.Regen();
            }
            catch { }
        }

        public PageWindow GetPageWindow()
        {
            return pageWindow;
        }

        public void SetSelectedEntities(List<DrawingEntity> l)
        {
            selectedEntities = l;
        }

        public void ClearSelectedEntities()
        {
            selectedEntities.Clear();
            selectedInitialGrips.Clear();
        }



        public void SetTitleBar()
        {
            this.Text = drawingDoc.Name + " - " + softwareName;
        }

        private void ShowCommandBar(ToolStrip toolStrip)
        {

        }

        private void HideCommandBar()
        {

        }

        private void ShowPrompt()
        {
            ShowPrompt(CommandPrompt(currentCommand));
        }

        private void ShowPrompt(String prompt)
        {
            statusStripPrompt.Text = prompt;
        }

        private void HidePrompt()
        {
            statusStripPrompt.Image = null;
            statusStripPrompt.Text = "";
        }


        public CommandMode CurrentCommand
        {
            get { return this.currentCommand; }
            set
            {

                if (this.currentCommand != value)
                {
                    this.currentCommand = value;
                }

                DoCommand();
            }
        }

        public List<DrawingEntity> GetEntities()
        {
            return drawingDoc.Entities;
        }

        public PageOrientation Orientation
        {
            get { return this.pageWindow.Orientation; }
            set
            {
                if (pageWindow.Orientation != value)
                {
                    pageWindow.Orientation = value;
                    drawingDoc.FormOrientation = value;

                    UpdateOrientationIcon();

                    pageWindow.Regen();
                }
            }
        }

        public PageWindow DrawingFormPageWindow
        {
            get { return pageWindow; }
        }

        public bool IsRotated
        {
            get { return drawingDoc.IsRotated; }
        }

        public double PageWidth
        {
            get { return pageWindow.PageSize.Width; }
            set
            {
                XSize size = new XSize();

                size.Height = this.pageWindow.PageSize.Height;
                size.Width = value;

                pageWindow.PageSize = size;
                pageWindow.Regen();

                UpdateOrientationIcon();
            }
        }

        public double PageHeight
        {
            get { return pageWindow.PageSize.Height; }
            set
            {
                XSize size = new XSize();

                size.Width = this.pageWindow.PageSize.Width;
                size.Height = value;
                pageWindow.PageSize = size;
                pageWindow.Regen();

                UpdateOrientationIcon();
            }
        }

        public void UpdateOrientationIcon()
        {
            switch (Orientation)
            {
                case PageOrientation.Landscape:
                    ;
                    break;

                case PageOrientation.Portrait:
                    break;
            }
        }

        public float DrawingScale
        {
            get { return drawingDoc.Scale; }
            set
            {
                drawingDoc.Scale = value;
            }
        }

        /// <summary>
        /// Gets the reference length obtained by the Scale command.
        /// </summary>
        /// <returns></returns>
        public float ReferenceLength
        {
            get { return drawingDoc.ReferenceLength; }
        }

        public PageWindow.RenderEvent RenderEvent
        {
            get
            {
                return this.renderEvent;
            }
            set
            {
                this.pageWindow.SetRenderEvent(value);
                this.renderEvent = value;
            }
        }
        PageWindow.RenderEvent renderEvent;

        public void UpdateDrawing()
        {
            this.pageWindow.Invalidate();
        }


        public void UpdateStatusBar()
        {
            string status, zoom;

            if (drawingDoc != null)
                status = String.Format("Scale: 1:{0:0.0}", drawingDoc.Scale);
            else
                status = "";

            if (pageWindow != null)
                zoom = String.Format("Zoom: {0:0}%", pageWindow.ZoomPercent);
            else
                zoom = "";

            statusStripScale.Text = status;

            statusStripZoom.Text = zoom;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateStatusBar();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateStatusBar();
        }

        public void LoadBasePlan(string sBaseImagePath = null, float lastWidthCrop = 0, double l_opacity = 1, bool bAutoTitle = false, bool bAutoTitleMargins = false)
        {
            if (sBaseImagePath == null)
            {
                if (System.IO.Directory.Exists(Program.LastLoadedDrawingFolder))
                    openFileDialog.InitialDirectory = Program.LastLoadedDrawingFolder;
                else
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|JPG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.DefaultExt = "pdf";
                openFileDialog.FileName = "";
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    sBaseImagePath = openFileDialog.FileName;
                }
            }
            if (sBaseImagePath != null)
            {
                Program.LastLoadedDrawingFolder = Path.GetDirectoryName(sBaseImagePath);

                if (sBaseImagePath.ToLowerInvariant().EndsWith(".pdf"))
                {
                    // Get PDF Info
                    try
                    {
                        this.Cursor = Cursors.WaitCursor;

                        pdfDoc = CompatiblePdfReader.CompatibleOpen(sBaseImagePath, PdfDocumentOpenMode.InformationOnly);

                        drawingDoc.PrepareDrawingForPDFPage(pdfDoc, 1);
                        drawingDoc.AddBaseImage(sBaseImagePath, pdfDoc);
                        if (drawingDoc.TempBaseImage != null)
                        {
                            drawingDoc.TempBaseImage.Opacity = l_opacity;
                        }
                        if (drawingDoc.BaseImage != null)
                        {
                            drawingDoc.BaseImage.Opacity = l_opacity;
                        }
                        pageWindow.Regen();

                        this.Cursor = Cursors.Default;

                        SelectPage();
                        //Kiet Nguyen 2: [200] sheet
                        this.pageWindow.IsLoadPDF = true;
                    }
                    catch
                    {
                        MessageBox.Show(this, "Unable to load PDF file " + sBaseImagePath + ".", "File Not Loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Cursor = Cursors.Default;
                    }
                }
                else if (sBaseImagePath.ToLowerInvariant().EndsWith(".jpg") ||
                         sBaseImagePath.ToLowerInvariant().EndsWith(".png"))
                {
                    try
                    {
                        this.Cursor = Cursors.WaitCursor;

                        drawingDoc.AddBaseImage(sBaseImagePath);
                        if (drawingDoc.TempBaseImage != null)
                        {
                            drawingDoc.TempBaseImage.Opacity = l_opacity;
                        }
                        if (drawingDoc.BaseImage != null)
                        {
                            drawingDoc.BaseImage.Opacity = l_opacity;
                        }

                        undoOnCancel = true;
                        //DisableTools();

                        pageWindow.Regen();

                        this.Cursor = Cursors.Default;

                        try
                        {
                            if (lastWidthCrop != 0)
                            {
                                PointF p0 = pageWindow.PageLocation(new Point(pageWindow.GetVirtualPage().Left + 1, pageWindow.GetVirtualPage().Top + 1));
                                PointF p1 = pageWindow.PageLocation(new Point(pageWindow.GetVirtualPage().Right + 1, pageWindow.GetVirtualPage().Top + 1));
                                float referenceLength = DrawingDoc.Distance(p0, p1);
                                float calculatedScale = lastWidthCrop / referenceLength;
                                this.DrawingScale = calculatedScale;
                            }
                        }
                        catch { }

                        SelectPage();
                        if (bAutoTitle)
                        {
                            AcceptCommand();
                            pageWindow.Regen();
                        }
                        //Kiet Nguyen 2: [200] sheet
                        this.pageWindow.IsLoadPDF = true;
                    }
                    catch
                    {
                        MessageBox.Show(this, "Unable to load image file " + sBaseImagePath + ".", "File Not Loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                    MessageBox.Show("Only PDF or JPG files are supported by this version of " + Program.sProgramName + ".");
            }
        }

        public void SelectPage()
        {
            CurrentCommand = CommandMode.PDFSelectPage;
        }

        void PrepSelectPageToolstrip()
        {
            ;
        }


        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private void InternalZoomIterations(List<PointF> lPWithoutMag, bool bFirst, float finalZoom, float origVWidth, float origVHeight,
            float totalX, float totalY)
        {
            //return;
            PointF magF = pageWindow.GetMagnification();
            List<PointF> lP = new List<PointF>();
            foreach (PointF p in lPWithoutMag)
            {
                lP.Add(new PointF(p.X * magF.X, p.Y * magF.Y));
            }

            Rectangle virtPageInit = pageWindow.GetVirtualPage();

            float factorX = 1 + ((float)origVWidth - (lP[1].X - lP[0].X)) / (lP[1].X - lP[0].X);
            float factorY = 1 + ((float)origVHeight - (lP[2].Y - lP[1].Y)) / (lP[2].Y - lP[1].Y);

            float h1 = (float)(Math.Pow(lP[0].X + virtPageInit.X, 2f) + Math.Pow(lP[0].Y + virtPageInit.Y, 2f));
            float h2 = (float)(Math.Pow(totalX - lP[1].X - virtPageInit.X, 2f) + Math.Pow(lP[1].Y + virtPageInit.Y, 2f));
            float h3 = (float)(Math.Pow(totalX - lP[2].X - virtPageInit.X, 2f) + Math.Pow(totalY - lP[2].Y - virtPageInit.Y, 2f));
            float h4 = (float)(Math.Pow(lP[3].X + virtPageInit.X, 2f) + Math.Pow(totalY - lP[3].Y - virtPageInit.Y, 2f));

            float XZoom = 0;
            float YZoom = 0;
            if ((h1 < h2) && (h1 < h3) && (h1 < h4))
            {
                XZoom = lP[0].X;
                YZoom = lP[0].Y;
            }
            else if ((h2 < h3) && (h2 < h4))
            {
                XZoom = lP[1].X;
                YZoom = lP[1].Y;
            }
            else if (h3 < h4)
            {
                XZoom = lP[2].X;
                YZoom = lP[2].Y;
            }
            else
            {
                XZoom = lP[3].X;
                YZoom = lP[3].Y;
            }


            float zoomFinal = Math.Min(factorX, factorY);
            if (bFirst)
            {
                finalZoom = Math.Min((float)Zoom.Maximum, pageWindow.ZoomPercent * zoomFinal);
            }
            float firstnewZoom = Math.Min((pageWindow.ZoomPercent * 1.1f), (float)Zoom.Maximum);

            //If needed correction, we could setup a factor or 0.8
            if (firstnewZoom >= finalZoom /** .8*/)
                return;
            if (pageWindow.ProportionalZoom(true, (int)(virtPageInit.X + (int)XZoom), (int)(virtPageInit.Y + (int)YZoom)) < finalZoom)
            {
                InternalZoomIterations(lPWithoutMag, false, finalZoom, origVWidth, origVHeight, totalX, totalY);
            }
        }

        public void DoCommand()
        {
            switch (currentCommand)
            {
                case CommandMode.SilentCancel:
                    if (undoOnCancel)
                        drawingDoc.Undo(false);

                    pageWindow.StopCommand();
                    drawingDoc.ClearTempEntities();
                    CurrentCommand = CommandMode.None;
                    pageWindow.Update();
                    break;

                case CommandMode.Cancel:
                    if (undoOnCancel)
                        drawingDoc.Undo(false);

                    pageWindow.StopCommand();
                    drawingDoc.ClearTempEntities();
                    CurrentCommand = CommandMode.None;
                    pageWindow.Refresh();
                    ShowCancelPrompt();
                    break;

                case CommandMode.Delete:
                    drawingDoc.DeleteEntity(selectedEntity);
                    drawingDoc.lastEntity = null;
                    drawingDoc.ClearTempEntities();
                    pageWindow.StopCommand();
                    CurrentCommand = CommandMode.None;
                    break;

                case CommandMode.None:
                    undoOnCancel = false;
                    HideCommandBar();
                    //EnableTools();
                    break;

                case CommandMode.PDFSelectPage:
                    //DisableTools();
                    PrepSelectPageToolstrip();
                    //ShowCommandBar(toolStripSelectPage);
                    try
                    {
                        drawingDoc.TempBaseImage.PreviewMode = false;
                        drawingDoc.AcceptEntity();
                    }
                    catch { }
                    CurrentCommand = CommandMode.PDFSelectPageDone;
                    break;
                case CommandMode.PDFSelectPageAccept:
                    try
                    {
                        drawingDoc.TempBaseImage.PreviewMode = false;
                        drawingDoc.AcceptEntity();
                    }
                    catch { }
                    CurrentCommand = CommandMode.PDFSelectPageDone;
                    break;
                case CommandMode.PDFSelectPageDone:
                    CurrentCommand = CommandMode.None;
                    break;

                case CommandMode.Pline:
                    CurrentCommand = CommandMode.PlineP1;
                    break;
                case CommandMode.PlineP1:
                    pageWindow.GetPointFromUser();
                    break;
                case CommandMode.PlineNext:
                    pageWindow.GetSecondPointFromUser(point1);
                    break;
                case CommandMode.PlineDone:
                    CurrentCommand = CommandMode.None;
                    double dPerim = drawingDoc.GetEntityPerimeter(drawingDoc.LastEntity);
                    lblLastLineWhole.Text = ((int)dPerim).ToString("N0").Replace(".", ",");
                    lblLastLineDecimal.Text = ((int)(((decimal)dPerim % 1) * 10)).ToString();
                    if (drawingDoc.LastEntity.Component.bLineZeroLine == true)
                    {
                        lblLastLineWhole.Text = lblLastLineWhole.Text + "-";
                    }
                    CalculateAll();
                    pageWindow.Regen();//ADDED MAG
                    DrawComponent();//Lets start again
                    break;

                case CommandMode.PlineAccept:
                    if (drawingDoc.TempPolyline.Count > 1)
                    {
                        drawingDoc.AcceptEntity();
                        CurrentCommand = CommandMode.PlineDone;
                    }
                    else
                    {
                        CurrentCommand = CommandMode.Cancel;
                    }
                    break;
                case CommandMode.PlineClose:
                    if (drawingDoc.TempPolyline.Count > 2)
                    {
                        drawingDoc.TempPolyline.IsClosed = true;
                        AcceptCommand();
                    }
                    break;

                case CommandMode.PLineDoor:
                    CurrentCommand = CommandMode.PLineDoorP1;
                    break;

                case CommandMode.PLineDoorP1:
                    pageWindow.GetPointFromUser();
                    break;

                case CommandMode.PLineDoorAccept:
                    drawingDoc.AcceptEntity();
                    CurrentCommand = CommandMode.PLineDoorDone;
                    break;

                case CommandMode.PLineDoorDone:
                    CurrentCommand = CommandMode.None;
                    CalculateAll();
                    pageWindow.Regen();//ADDED MAG
                    CurrentCommand = CommandMode.PLineDoorP1;
                    //DrawComponent();//Lets start again
                    break;

                case CommandMode.Polygon:
                    CurrentCommand = CommandMode.PolygonP1;
                    break;
                case CommandMode.PolygonP1:
                    pageWindow.GetPointFromUser();
                    break;
                case CommandMode.PolygonNext:
                    pageWindow.GetSecondPointFromUser(point1);
                    break;
                case CommandMode.PolygonDone:
                    CurrentCommand = CommandMode.None;
                    pageWindow.Regen();//ADDED MAG
                    try
                    {
                        double dArea = drawingDoc.GetEntityArea(drawingDoc.LastEntity);
                        lblLastAreaWhole.Text = ((int)dArea).ToString("N0").Replace(".", ",");
                        lblLastAreaDecimal.Text = ((int)(((decimal)dArea % 1) * 10)).ToString();//((int)((dPerim - (int)dPerim) * 10)).ToString();

                        dPerim = drawingDoc.GetEntityPerimeter(drawingDoc.LastEntity);
                        lblLastPerimeterWhole.Text = ((int)dPerim).ToString("N0").Replace(".", ",");
                        lblLastPerimeterDecimal.Text = ((int)(((decimal)dPerim % 1) * 10)).ToString();

                        if (drawingDoc.LastEntity.Component.bAreaZeroLine == false)
                        {
                            lblLastLineWhole.Text = ((int)dPerim).ToString("N0").Replace(".", ",");
                            lblLastLineDecimal.Text = ((int)(((decimal)dPerim % 1) * 10)).ToString();
                        }

                        double Trim = GetTrim();
                        double dWaste = dPerim * Trim;
                        lblLastWasteWhole.Text = ((int)dWaste).ToString("N0").Replace(".", ",");
                        lblLastWasteDecimal.Text = ((int)(((decimal)dWaste % 1) * 10)).ToString() + "%";
                        CalculateAll();
                    }
                    catch { }
                    DrawComponent();//Lets start again
                    break;

                case CommandMode.PolygonAccept:
                    if (true)
                    {
                        if (drawingDoc.TempPolygon.PointList.Count > 2)
                        {
                            drawingDoc.TempPolygon.IsBeingEdited = false;
                            drawingDoc.TempPolygon.AddLastZeroPoint();
                            drawingDoc.AcceptEntity();
                            CurrentCommand = CommandMode.PolygonDone;
                        }
                        else
                        {
                            CurrentCommand = CommandMode.Cancel;
                        }
                    }
                    break;
                case CommandMode.PolygonEdit:
                    if (selectedEntity.Type == DrawingEntity.EntityType.Polygon)
                    {
                        CurrentCommand = CommandMode.PolygonNext;
                        drawingDoc.TempPolygon = new Polygon((Polygon)selectedEntity);
                        currentComponent = drawingDoc.TempPolygon.Component;
                        drawingDoc.TempPolygon.IsBeingEdited = true;
                        drawingDoc.DeleteEntity(selectedEntity, false);
                        undoOnCancel = true;
                        pageWindow.TempEntityHold();
                        pageWindow.Regen();
                    }
                    else
                        CurrentCommand = CommandMode.Cancel;
                    break;
            }
        }


        public void PointUpdate()
        {
            ;
        }

        public void PointDoubleClick(PointF point)
        {
            if (point.IsEmpty)
            {
                CurrentCommand = CommandMode.Cancel;
                return;
            }
        }

        public void PointInput(PointF point)
        {
            if (point.IsEmpty)
            {
                CurrentCommand = CommandMode.Cancel;
                return;
            }



            switch (currentCommand)
            {


                case CommandMode.PlineP1:
                    currentComponent.bLineZeroLine = Program.bLineZeroLine;
                    if (Program.jump_b1_b2_dup_Mode == 0)
                    {
                        Program.jump_b1_b2_dup_Mode = 1;
                    }

                    if (Program.jump_b1_b2_dup_Mode == 3)//duplicate
                    {
                        LineModeSel();
                        PointF firstP = new PointF(point.X + (initialPoint.X - endPoint.X), point.Y + (initialPoint.Y - endPoint.Y));
                        drawingDoc.AddPolyline(point, currentComponent);
                        drawingDoc.TempPolyline.AddPoint(firstP);
                        CurrentCommand = CommandMode.PlineNext;
                        AcceptCommand();
                    }
                    else
                    {
                        initialPoint = point;
                        LineModeSel();

                        drawingDoc.AddPolyline(point, currentComponent);
                        point1 = point;

                        CurrentCommand = CommandMode.PlineNext;
                    }
                    break;
                case CommandMode.PlineNext:
                    if (drawingDoc.TempPolyline.DashType == 1)//double, only 2 points
                    {
                        endPoint = point;
                        drawingDoc.TempPolyline.AddPoint(point);
                        AcceptCommand();
                    }
                    else
                    {
                        drawingDoc.TempPolyline.AddPoint(point);
                        point1 = point;

                        CurrentCommand = CommandMode.PlineNext;
                    }
                    break;

                case CommandMode.PolygonP1:
                    currentComponent.bAreaZeroLine = Program.bAreaZeroLine;
                    drawingDoc.AddPolygon(point, currentComponent);
                    drawingDoc.TempPolygon.IsBeingEdited = true;//ADDED MAG
                    //drawingDoc.AddPoint(point);
                    point1 = point;
                    CurrentCommand = CommandMode.PolygonNext;
                    break;
                case CommandMode.PolygonNext:
                    drawingDoc.TempPolygon.AddPoint(point);
                    point1 = point;

                    CurrentCommand = CommandMode.PolygonNext;
                    break;

                case CommandMode.PolygonEdit:
                    pageWindow.TempEntityHold();
                    break;

                case CommandMode.PLineDoorP1:
                    float fMeasure = 3.0f;
                    if (rd6f.Checked)
                    {
                        fMeasure = 6.0f;
                    }
                    else if (rdOtherft.Checked)
                    {
                        try
                        {
                            fMeasure = float.Parse(txtOtherFt.Text);
                        }
                        catch { }
                    }
                    drawingDoc.AddDoorTakOut(point, currentComponent, fMeasure);
                    drawingDoc.TempDoorTakOut.AddPoint(new PointF(point.X, point.Y));
                    AcceptCommand();
                    /*drawingDoc.AcceptEntity();
                    CurrentCommand = CommandMode.PLineDoorDone;
                    AcceptCommand();*/
                    break;
            }
        }
        PointF point1;
        PointF point2;


        private void DisableButtons()
        {
            foreach (RibbonGroup g in ultraToolbarsManager1.Ribbon.Tabs[0].Groups)
            {
                foreach (ToolBase tool in g.Tools)
                {
                    tool.SharedProps.Enabled = false;
                }
            }
            ultraToolbarsManager1.Tools["New"].SharedProps.Enabled = true;
            ultraToolbarsManager1.Tools["Open"].SharedProps.Enabled = true;
        }

        private void EnableButtons()
        {
            foreach (RibbonGroup g in ultraToolbarsManager1.Ribbon.Tabs[0].Groups)
            {
                foreach (ToolBase tool in g.Tools)
                {
                    tool.SharedProps.Enabled = true;
                }
            }
        }


        public void RemoveLastPoint()
        {
            bool bRemoveEntity = false;
            if (drawingDoc.TempPolygon != null)
            {
                bRemoveEntity = drawingDoc.TempPolygon.RemoveLastPoint();
                pageWindow.FirstPointPicked = drawingDoc.TempPolygon.LastPoint;
                if (bRemoveEntity)
                {
                    SilentCancelCommand();
                    pageWindow.ClearGrips();
                    pageWindow.Regen();
                    DrawComponent();
                }
                else
                {
                    Refresh();
                }
            }
            if (drawingDoc.TempPolyline != null)
            {
                bRemoveEntity = drawingDoc.TempPolyline.RemoveLastPoint();
                pageWindow.FirstPointPicked = drawingDoc.TempPolyline.LastPoint;
                if (bRemoveEntity)
                {
                    SilentCancelCommand();
                    pageWindow.ClearGrips();
                    pageWindow.Regen();
                    DrawComponent();
                }
                else
                {
                    Refresh();
                }
            }
            if (CurrentCommand == CommandMode.PLineDoorP1)
            {
                try
                {
                    if (drawingDoc.LastOpEntity.Type == DrawingEntity.EntityType.DoorTakeOut)
                    {
                        drawingDoc.Undo(false);
                        pageWindow.Regen();
                        CalculateAll();
                    }
                }
                catch { }
            }
        }

        public void RenderTempEntity(XGraphics xgfx)
        {
            RenderTempEntity(xgfx, PointF.Empty);
        }

        public void RenderTempEntity(XGraphics xgfx, PointF point)
        {
            if (drawingDoc.TempLine != null)
            {
                if (!point.IsEmpty)
                    drawingDoc.TempLine.Point2(point);

                drawingDoc.Render(xgfx, drawingDoc.TempLine);
            }

            if (drawingDoc.TempPolyline != null)
            {
                if (!point.IsEmpty)
                    drawingDoc.TempPolyline.AddPoint(point);

                drawingDoc.Render(xgfx, drawingDoc.TempPolyline);

                // We remove the point since it was just for rendering while dragging
                if (!point.IsEmpty)
                    drawingDoc.TempPolyline.RemoveLastPoint();
            }

            if (drawingDoc.TempDoorTakOut != null)
            {
                if (!point.IsEmpty)
                    drawingDoc.TempDoorTakOut.AddPoint(point);

                drawingDoc.Render(xgfx, drawingDoc.TempDoorTakOut);

                // We remove the point since it was just for rendering while dragging
                if (!point.IsEmpty)
                    drawingDoc.TempDoorTakOut.RemoveLastPoint();
            }

            if (drawingDoc.TempPolygon != null)
            {
                if (!point.IsEmpty)
                    drawingDoc.TempPolygon.AddPoint(point);

                drawingDoc.Render(xgfx, drawingDoc.TempPolygon);

                // We remove the point since it was just for rendering while dragging
                if (!point.IsEmpty)
                    drawingDoc.TempPolygon.RemoveLastPoint();
            }
        }

        public void GripCommand()
        {
            if (drawingDoc.TempEntityDoesExist)
            {
                ;
            }
        }

        public void CancelCommand()
        {
            pageWindow.DisableMouseMove = false;
            IsDragOp = false;
            CurrentCommand = CommandMode.Cancel;
        }

        public void SilentCancelCommand()
        {
            pageWindow.DisableMouseMove = false;
            IsDragOp = false;
            CurrentCommand = CommandMode.SilentCancel;
        }

        public void AcceptCommand()
        {
            if (CurrentCommand != CommandMode.None)
            {
                pageWindow.DisableMouseMove = false;
                IsDragOp = false;
            }
            pageWindow.StopCommand();

            GC.Collect();//MEM

            switch (CurrentCommand)
            {
                case CommandMode.PlineNext:
                case CommandMode.PlineClose:
                    CurrentCommand = CommandMode.PlineAccept;
                    break;

                case CommandMode.PLineDoorP1:
                    CurrentCommand = CommandMode.PLineDoorAccept;
                    break;

                case CommandMode.PolygonNext:
                    CurrentCommand = CommandMode.PolygonAccept;
                    break;

                case CommandMode.PDFSelectPage:
                    CurrentCommand = CommandMode.PDFSelectPageAccept;
                    pdfDoc = null;
                    break;
            }
        }

        private string CommandPrompt(CommandMode command)
        {
            switch (command)
            {
                case CommandMode.None:
                    return "";
                default:
                    return "";
            }
        }

        public void ShowCancelPrompt()
        {
            //picturePromptIcon.Image = Properties.Resources.CancelIcon;
            //ShowPrompt("Cancelled");
            HidePrompt();
            //statusStripPrompt.Image = Properties.Resources.CancelIcon;
            statusStripPrompt.Text = "Cancelled";
            statusStrip.Update();

            System.Threading.Thread.Sleep(1000);
            statusStripPrompt.Image = null;
            statusStripPrompt.Text = "";
            statusStrip.Update();
            //picturePromptIcon.Image = Properties.Resources.BalloonIcon;
        }

        public void ShowSelectedPrompt()
        {
            //picturePromptIcon.Image = Properties.Resources.CancelIcon;
            //ShowPrompt("Cancelled");
            HidePrompt();
            //statusStripPrompt.Image = Properties.Resources.InfoIcon;
            statusStripPrompt.Text = "Selection done!";
            statusStrip.Update();

            System.Threading.Thread.Sleep(1000);
            statusStripPrompt.Image = null;
            statusStripPrompt.Text = "";
            statusStrip.Update();
            //picturePromptIcon.Image = Properties.Resources.BalloonIcon;
        }

        public DrawingEntity FindEntity(PointF pickPoint)
        {
            return drawingDoc.FindEntityAt(pickPoint);
        }

        public bool SelectEntity(PointF pickPoint)
        {
            //No selection yet
            pageWindow.ClearGrips();
            //EnableTools();
            return false;

            SelectedEntity = drawingDoc.FindEntityAt(pickPoint);

            //selectedEntities = new List<DrawingEntity>();

            if (selectedEntity != null)
            {
                drawingDoc.lastEntity = selectedEntity;
                Debug.Print("Found entity!");

                SetGrips(selectedEntity);


                return true;
            }
            else
            {
                pageWindow.ClearGrips();
                //EnableTools();
                return false;
            }
        }

        public bool SelectEntity(DrawingEntity e, bool bGrips = true)
        {
            SelectedEntity = e;

            //selectedEntities = new List<DrawingEntity>();

            if (selectedEntity != null)
            {
                ClearSelectedEntities();
                drawingDoc.lastEntity = selectedEntity;
                if (bGrips)
                {
                    SetGrips(selectedEntity);
                }

                switch (selectedEntity.Type)
                {
                    case DrawingEntity.EntityType.Polygon:
                        CurrentCommand = CommandMode.PolygonEdit;
                        break;
                }

                /*switch (selectedEntity.Type)
                {
                    ;
                }*/

                return true;
            }
            else
            {
                pageWindow.ClearGrips();
                //EnableTools();
                return false;
            }
        }

        public void SetGrips(DrawingEntity selectedEntity)
        {
            if (selectedEntity == null)
                return;

            float ptPerMm = 0.352778f;

            PointF[] gripPoints = new PointF[selectedEntity.GripPoints().Length + ((pageWindow.nearestP != PointF.Empty) ? 1 : 0)];
            int i = 0;

            foreach (PointF point in selectedEntity.GripPoints())
            {
                gripPoints[i] = new PointF();
                gripPoints[i].X = point.X * ptPerMm;
                gripPoints[i].Y = point.Y * ptPerMm;
                i++;
            }

            pageWindow.PlaceGrips(gripPoints);
        }

        DrawingEntity selectedEntity;

        public DrawingEntity SelectedEntity
        {
            get { return selectedEntity; }
            set
            {
                selectedEntity = value;
            }
        }

        public bool TempEntityDoesExist
        {
            get { return drawingDoc.TempEntityDoesExist; }
        }


        List<DrawingEntity> selectedEntities = new List<DrawingEntity>();
        Dictionary<int, PointF> selectedInitialGrips = new Dictionary<int, PointF>();

        public List<DrawingEntity> SelectedEntities
        {
            get { return selectedEntities; }
            set
            {
                selectedEntities = value;
            }
        }

        public Dictionary<int, PointF> SelectedInitialGrips
        {
            get
            {
                return selectedInitialGrips;
            }
        }

        public bool TempEntitiesDoesExist
        {
            get { return drawingDoc.TempEntitiesDoesExist; }
        }

        public DrawingDoc DrawingDoc
        {
            get { return drawingDoc; }
        }


        //
        // End Properties

        public void UndoEnable(bool setting)
        {
            ;
        }

        public void RedoEnable(bool setting)
        {
            ;
        }

        private void pageWindow_Load(object sender, EventArgs e)
        {
            this.pageWindow.Zoom = Zoom.FullPage;
            this.pageWindow.Regen();
        }

        private void DrawingForm_Load(object sender, EventArgs e)
        {
            //Kiet Nguyen 2: [310] sheet
            Infragistics.Shared.ResourceCustomizer rc = Infragistics.Win.UltraWinToolbars.Resources.Customizer;
            rc.SetCustomizedString("MinimizeRibbon", "Hide the Menu Bar");


            //Kiet Nguyen: [1100] sheet            
            pnlAreaModeInRight.Dock = DockStyle.Fill;
            pnlLineModeRightPanel.Dock = DockStyle.Fill;

            ultraToolbarsManager1.CreationFilter = new CRightGroup();
            DisableButtons();

            InitProject();

            Program.mainForm.UpdateMainUIState();
            this.MouseWheel += new MouseEventHandler(DrawingForm_MouseWheel);

            btnRight.Location = new Point(this.Size.Width - 40, 161);
        }

        void DrawingForm_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                MouseEventArgs newE = new MouseEventArgs(e.Button, e.Clicks, e.X - this.pageWindow.Location.X, e.Y - this.pageWindow.Location.Y, e.Delta);
                this.pageWindow.PageWindowMouseWheel(sender, newE);
            }
            catch { }
            ;// throw new NotImplementedException();
        }

        private void DoCustomZoom(float fZ)
        {
            pageWindow.ZoomPercent = fZ;
            pageWindow.Regen();
        }


        private void DrawComponent(bool l_bRemove = false)
        {
            //Kiet Nguyen 2: [200] sheet
            //if ((drawingDoc.TempBaseImage == null) && (drawingDoc.BaseImage == null))
            //{
            //    MessageBox.Show("You need to load a PDF before drawing components.", "Warning", MessageBoxButtons.OK,
            //                           MessageBoxIcon.Warning);
            //    return;
            //}

            if (Program.SelectedFinish == null)
            {
                MessageBox.Show("You need to select a Finish item first.", "Finish", MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                return;
            }

            if (Program.bLineMode)
            {
                currentComponent = new ComponentDef(1000 + Program.SelectedLine.nIndex.ToString(), Program.SelectedLine.Name,
                    "l.f.", ComponentType.Perimeter, Program.SelectedLine.pnlBackColor, Program.SelectedLine.Alpha, 0, CalculationMode.Sum, Program.SelectedLine.b, l_bRemove,
                    Program.bAreaZeroLine);
            }
            else
            {
                currentComponent = new ComponentDef(Program.SelectedFinish.nIndex.ToString(), Program.SelectedFinish.Name,
                    "s.f.", ComponentType.Area, Program.SelectedFinish.pnlBackColor, Program.SelectedFinish.Alpha, 0, CalculationMode.Sum, Program.SelectedLine.b, l_bRemove,
                    Program.bAreaZeroLine);
            }

            switch (currentComponent.Type)
            {
                case ComponentType.Area:
                    CurrentCommand = CommandMode.Polygon;
                    break;
                case ComponentType.Perimeter:
                    CurrentCommand = CommandMode.Pline;
                    break;
            }
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static void ResizeImage(Image image, int width, int height, string sImageRes)
        {
            try
            {
                if (File.Exists(sImageRes))
                {
                    File.Delete(sImageRes);
                }
            }
            catch { }
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
                destImage.SetResolution(96, 96);
                destImage.Save(sImageRes, ImageFormat.Png);
            }

            //return destImage;
        }



        private void ShowPage(object sender, EventArgs e)
        {
            SetPage((int)((ToolStripMenuItem)sender).Tag);
        }

        private void SetPage(int newPageNumber)
        {
            int previousPageNumber = drawingDoc.TempBaseImage.PDFPageNumber;
            if (previousPageNumber != newPageNumber)
            {
                drawingDoc.TempBaseImage.RotateToZero();
                drawingDoc.TempBaseImage.PDFPageNumber = newPageNumber;
                drawingDoc.PrepareDrawingForPDFPage(pdfDoc, newPageNumber);
                pageWindow.Regen();
            }
        }

        public void DeleteSelected()
        {
            CurrentCommand = CommandMode.Delete;
        }

        public void InsertLastEntity()
        {
            if (drawingDoc.lastEntity == null)
                return;
            SelectEntity(drawingDoc.lastEntity);
        }

        private void DrawingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CurrentCommand != CommandMode.None)
            {
                this.Cursor = Cursors.WaitCursor;
                CancelCommand();
            }

            try
            {
                if (currentProject.Drawings.Count > 0)
                {
                    DialogResult res = MessageBox.Show("Do you want to save current project before exiting?", "Exit FloorPlanning", MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);
                    if (res == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (currentProject.Name == Program.Empty_Project)
                        {
                            FormSaveAsProject fSaveAs = new FormSaveAsProject();
                            if (fSaveAs.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                if (currentProject.SaveAs(fSaveAs.sLastPath, fSaveAs.sNewName))
                                {
                                    CurrentJob.Name = currentProject.Name = drawingDoc.Name = fSaveAs.sNewName;
                                    CurrentJob.Save();
                                    CurrentJob.SaveSame();
                                    this.SetTitleBar();
                                }
                            }
                        }
                        else
                        {
                            currentProject.SaveSame();
                        }
                    }
                    else if (res == System.Windows.Forms.DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            catch { }
        }

        private void DrawingForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            SaveFinish();
            this.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        private void pageWindow_ZoomChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Creates a new Job
        /// </summary>
        /// <param name="jobName">Name of the job, a string with letters, numbers or spaces.</param>
        public void CreateProject(string savePath, string jobName)
        {
            CloseCurrentProject();

            currentProject = new FloorPlanningProject(jobName);
            currentProject.SavePath = savePath;

            this.Text = currentProject.Name + " - " + softwareName;
        }


        public void OpenProject(string filePath)
        {
            CloseCurrentProject();

            //Open the filePath above and read objects from it.
            Stream stream;
            try
            {
                stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not open file!\n\nError: " + e.Message, "Open Job", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            BinaryFormatter bformatter = new BinaryFormatter();

            try
            {
                currentProject = (FloorPlanningProject)bformatter.Deserialize(stream);
                currentProject.SavePath = Path.GetDirectoryName(filePath);
                currentProject.CreatedDate = File.GetCreationTime(filePath);
            }
            catch
            {
                MessageBox.Show("File is not valid or was created by an incompatible version.", "Open Job", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }
            finally
            {
                stream.Close();
            }

            UpdateMainUIState();
        }

        public void CloseCurrentProject()
        {
            if (currentProject != null)
            {
                // Save recentJobs to settings
                Properties.Settings.Default.Save();

                // Proceed to close the job
                //dont close! currentJob.CloseDrawings();
                this.Text = softwareName;
                currentProject = null;
            }
        }

        public void SetAllThumbnailsInListView()
        {
            this.pctPage.Visible = this.lblPageNumber.Visible = false;
            bool bImage = false;
            try
            {
                lCurrentThumbs = new ImageList();
                lCurrentThumbs.ImageSize = new Size(128, 128);
                lCurrentThumbs.ColorDepth = ColorDepth.Depth32Bit;
                Dictionary<string, int> dImagesIndex = new Dictionary<string, int>();
                int nIndex = 0;
                foreach (string sK in currentProject.Drawings.Keys)
                {
                    DrawingDoc drawing = currentProject.Drawings[sK];
                    //Image img = Properties.Resources.LandscapeIcon64;
                    Image img = null;
                    if (drawing.BaseImage != null)
                    {
                        bImage = true;
                        img = drawing.BaseImage.GetThumb();
                        if (img == null)
                        {
                            ;// img = Properties.Resources.LandscapeIcon64;
                        }
                    }

                    lCurrentThumbs.Images.Add(img);
                    dImagesIndex[sK] = nIndex++;
                }
            }
            catch { }
            try
            {
                this.pctPage.Visible = this.lblPageNumber.Visible = bImage;
                if (bImage)
                {
                    this.pctPage.BackgroundImage = lCurrentThumbs.Images[0];
                }
                else
                {
                    this.pctPage.BackgroundImage = null;
                }
            }
            catch { }
        }

        public DrawingDoc AddDrawing(String name, DrawingDoc.DrawingType type)
        {
            String drawingKey = currentProject.AddDrawing(name, type);
            DrawingDoc drawingDoc = currentProject.Drawings[drawingKey];
            currentProject.ListDrawings();

            //AddListItem(drawingKey, drawingDoc.Name);
            UpdateMainUIState();
            Application.DoEvents();

            return drawingDoc;
        }

        public void DeleteSelectedListItems()
        {
            ;
        }

        public void UpdateMainUIState()
        {
            ;
        }
        //
        // Methods section End


        // Properties section Start
        //
        /// <summary>
        /// Gets the job of the currently selected tab
        /// </summary>
        public FloorPlanningProject CurrentJob
        {
            get { return currentProject; }
        }
        //
        // Properties section End



        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseCurrentProject();
            try
            {
                tBackground.Abort();
            }
            catch { }
            //Environment.Exit(0);
            Process.GetCurrentProcess().Kill();
        }


        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            String drawingKey = "";
            switch (e.Tool.Key)
            {
                case "New":
                    var FormNewProject = new FormNewProject();
                    if (FormNewProject.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (FormNewProject.bSaved)
                        {
                            drawingKey = currentProject.AddDrawing(currentProject.Name, DrawingDoc.DrawingType.Plan);
                            drawingDoc = currentProject.Drawings[drawingKey];
                            drawingDoc.SetDrawingForm(this);
                            EnableButtons();
                            this.SetTitleBar();
                            this.pageWindow.Zoom = Zoom.FullPage;
                            this.pageWindow.Regen(true);
                            SetAllThumbnailsInListView();
                            lblLastAreaWhole.Text = lblLastAreaDecimal.Text = lblLastPerimeterWhole.Text = lblLastPerimeterDecimal.Text = "0";
                            lblLastTotalPerimeterDecimal.Text = lblLastTotalPerimeterWhole.Text = "0";
                            lblLastAreaTotalWhole.Text = lblLastAreaTotalDecimal.Text = "0";
                            lblLastWasteWhole.Text = "0";
                            lblLastWasteDecimal.Text = "0%";
                        }
                    }
                    CalculateAll();
                    break;
                case "Open":
                    OpenFileDialog openDlg = new OpenFileDialog();
                    openDlg.InitialDirectory = Program.appDataPath;
                    if (System.IO.Directory.Exists(Program.LastOpenedJobFolder))
                        openDlg.InitialDirectory = Program.LastOpenedJobFolder;
                    openDlg.Filter = "Floor Planning files (*.fpp)|*.fpp";
                    if (openDlg.ShowDialog() == DialogResult.OK)
                    {
                        if (openDlg.FileName.Contains(Program.Empty_Project))
                        {
                            MessageBox.Show("Please select a valid file.", "Open File", MessageBoxButtons.OK,
                                                                    MessageBoxIcon.Warning);
                            break;
                        }
                        Program.LastOpenedJobFolder = Path.GetDirectoryName(openDlg.FileName);
                        OpenProject(openDlg.FileName);
                        if (currentProject.Drawings.Count == 0)
                        {
                            drawingKey = currentProject.AddDrawing(currentProject.Name, DrawingDoc.DrawingType.Plan);
                        }
                        else
                        {
                            drawingKey = currentProject.Drawings.Keys.First();
                        }
                        drawingDoc = currentProject.Drawings[drawingKey];
                        drawingDoc.SetDrawingForm(this);
                        EnableButtons();
                        lblLastAreaWhole.Text = lblLastAreaDecimal.Text = lblLastPerimeterWhole.Text = lblLastPerimeterDecimal.Text = "0";
                        lblLastTotalPerimeterDecimal.Text = lblLastTotalPerimeterWhole.Text = "0";
                        lblLastAreaTotalWhole.Text = lblLastAreaTotalDecimal.Text = "0";
                        lblLastWasteWhole.Text = "0";
                        lblLastWasteDecimal.Text = "0%";
                        CalculateAll();
                    }
                    this.SetTitleBar();
                    this.pageWindow.Zoom = Zoom.FullPage;
                    this.pageWindow.Regen(true);
                    SetAllThumbnailsInListView();
                    break;
                case "Save":
                    currentProject.SaveSame();
                    this.SetTitleBar();
                    break;
                case "SaveAs":
                    FormSaveAsProject fSaveAs = new FormSaveAsProject();
                    if (fSaveAs.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (currentProject.SaveAs(fSaveAs.sLastPath, fSaveAs.sNewName))
                        {
                            CurrentJob.Name = currentProject.Name = drawingDoc.Name = fSaveAs.sNewName;
                            CurrentJob.Save();
                            CurrentJob.SaveSame();
                            this.SetTitleBar();
                        }
                    }
                    //this.SetTitleBar();
                    break;
                case "LoadPDF":
                    SilentCancelCommand();
                    LoadBasePlan();
                    SetAllThumbnailsInListView();
                    SetPageMode();
                    tabPanes.Tabs["Pages"].Selected = true;
                    break;
                case "ReloadPDF":
                    string sPrevImage = null;
                    string sPrevSavePath = Application.StartupPath + "\\Configurations";
                    string sPrevName = "NewProject";
                    try
                    {
                        if (currentProject != null)
                        {
                            if ((currentProject.SavePath != null) && (currentProject.SavePath != ""))
                            {
                                sPrevSavePath = currentProject.SavePath;
                            }
                        }
                    }
                    catch
                    {
                        sPrevSavePath = Application.StartupPath + "\\Configurations";
                    }
                    sPrevName = currentProject.Name;
                    try
                    {
                        if (DrawingDoc.BaseImage != null)
                        {
                            if ((DrawingDoc.BaseImage.FilePath != null) && (DrawingDoc.BaseImage.FilePath != ""))
                            {
                                sPrevImage = DrawingDoc.BaseImage.FilePath;
                            }
                        }
                    }
                    catch { }
                    SilentCancelCommand();
                    InitProject();
                    LoadBasePlan(sPrevImage);
                    SetAllThumbnailsInListView();
                    SetPageMode();
                    tabPanes.Tabs["Pages"].Selected = true;

                    if (currentProject.SaveAs(sPrevSavePath, sPrevName))
                    {
                        CurrentJob.Name = currentProject.Name = drawingDoc.Name = sPrevName;
                        CurrentJob.Save();
                        CurrentJob.SaveSame();
                        this.SetTitleBar();
                    }

                    break;
                case "FitWidth":
                    this.pageWindow.Zoom = Zoom.FitWidth;
                    this.pageWindow.Regen(true);
                    break;
                case "FitHeight"://Full, not height
                    this.pageWindow.Zoom = Zoom.FullPage;
                    this.pageWindow.Regen(true);
                    break;
                case "ZoomIn":
                    this.pageWindow.ProportionalZoom(true);
                    break;
                case "ZoomOut":
                    this.pageWindow.ProportionalZoom(false);
                    break;
                case "ZoomPercent":
                    FormCustomZoom fZ = new FormCustomZoom();
                    if (fZ.ShowDialog() == DialogResult.OK)
                    {
                        DoCustomZoom(fZ.GetZoom());
                    }
                    break;
                case "PanMode":
                    Program.PanMode = EditMode.Pan;
                    SilentCancelCommand();
                    break;
                case "DrawMode":
                    if ((tabPanes.Tabs["Area"].Selected == false) &&
                        (tabPanes.Tabs["Lines"].Selected == false))
                    {
                        Program.bLineMode = false;
                        btnDuplicate.Enabled = false;
                        comeFromClick = true;
                        pnlLineModeRightPanel.Visible = false;
                        pnlAreaModeInRight.Visible = true;
                        Program.PanMode = EditMode.Draw;
                        tabPanes.Tabs["Area"].Selected = true;
                        if (!((StateButtonTool)ultraToolbarsManager1.Tools["AreaMode"]).Checked)
                        {
                            ((StateButtonTool)ultraToolbarsManager1.Tools["AreaMode"]).Checked = true;
                        }
                        SilentCancelCommand();
                        this.pageWindow.InvalidateCanvas();
                        DrawComponent();
                    }
                    else if (tabPanes.Tabs["Area"].Selected)
                    {
                        Program.bLineMode = false;
                        btnDuplicate.Enabled = false;
                        Program.PanMode = EditMode.Draw;
                        SilentCancelCommand();
                        this.pageWindow.InvalidateCanvas();
                        DrawComponent();
                    }
                    else if (tabPanes.Tabs["Lines"].Selected)
                    {
                        Program.bLineMode = true;
                        Program.PanMode = EditMode.Draw;
                        SilentCancelCommand();
                        this.pageWindow.InvalidateCanvas();
                        DrawComponent();
                    }
                    break;
                case "PageMode":
                    SetPageMode();
                    break;
                case "AreaMode":
                    Program.bLineMode = false;
                    btnDuplicate.Enabled = false;
                    pnlLineModeRightPanel.Visible = false;
                    pnlAreaModeInRight.Visible = true;
                    tabPanes.Tabs["Area"].Selected = true;
                    SilentCancelCommand();
                    this.pageWindow.InvalidateCanvas();
                    if (Program.PanMode == EditMode.Draw)
                    {
                        DrawComponent();
                    }
                    break;
                case "LineMode":
                    Program.bLineMode = true;
                    pnlAreaModeInRight.Visible = false;
                    pnlLineModeRightPanel.Visible = true;
                    tabPanes.Tabs["Lines"].Selected = true;
                    SilentCancelCommand();
                    this.pageWindow.InvalidateCanvas();
                    if (Program.PanMode == EditMode.Draw)
                    {
                        DrawComponent();
                    }
                    break;
                case "ShowImage":
                    Program.BackgroundVisible = true;
                    pageWindow.Regen();
                    break;
                case "HideImage":
                    Program.BackgroundVisible = false;
                    pageWindow.Regen();
                    break;

                case "Shortcuts_mnu":
                    try
                    {
                        FormShortcuts fN = new FormShortcuts(this);
                        fN.ShowDialog();
                    }
                    catch { }
                    break;
            }
        }

        private void SetPageMode()
        {
            try
            {
                Program.bLineMode = false;
                btnDuplicate.Enabled = false;
                Program.PanMode = EditMode.Page;
                SilentCancelCommand();
                this.pageWindow.InvalidateCanvas();
            }
            catch { }
        }

        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                if (tabPanes.SelectedTab.Text == "Area")
                {
                    selectedAreaMode();
                }
                else if (tabPanes.SelectedTab.Text == "Lines")
                {
                    selectedLineMode();
                }
                else if (tabPanes.SelectedTab.Text == "Pages")
                {
                    Program.bLineMode = false;
                    btnDuplicate.Enabled = false;
                    if (!((StateButtonTool)ultraToolbarsManager1.Tools["PageMode"]).Checked)
                    {
                        ((StateButtonTool)ultraToolbarsManager1.Tools["PageMode"]).Checked = true;
                    }
                    SilentCancelCommand();

                    //Kiet Nguyen: không hiểu sao nó không work, mặc dù setting bằng false mà cứ ra true
                    ((StateButtonTool)ultraToolbarsManager1.Tools["AreaMode"]).Checked = false;

                }
                else
                {
                    Program.bLineMode = false;
                    btnDuplicate.Enabled = false;
                    if (!((StateButtonTool)ultraToolbarsManager1.Tools["PageMode"]).Checked)
                    {
                        ((StateButtonTool)ultraToolbarsManager1.Tools["PageMode"]).Checked = true;
                    }
                    SilentCancelCommand();

                    //Kiet Nguyen: [1300] auto selected PanMode 
                    if (!((StateButtonTool)ultraToolbarsManager1.Tools["PanMode"]).Checked)
                    {
                        ((StateButtonTool)ultraToolbarsManager1.Tools["PanMode"]).Checked = true;
                    }
                }
                comeFromClick = false;

                grpAreaInRight.Enabled = (tabPanes.SelectedTab.Text == "Area");
                grpLineInRight.Enabled = (tabPanes.SelectedTab.Text == "Lines");
                pnlAreaModeInRight.Visible = (tabPanes.SelectedTab.Text == "Area");
                pnlLineModeRightPanel.Visible = (tabPanes.SelectedTab.Text == "Lines");
            }
            catch { }
        }

        private void btnNewShape_Click(object sender, EventArgs e)
        {
            FormNewShape fN = new FormNewShape(this);
            if (fN.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Color col = fN.GetColor();
                string sName = fN.GetName();
                UCFinish uc = new UCFinish(this);
                uc.SetColor(col, fN.GetAlpha());
                uc.SetFinishName(sName);
                uc.Select(true);
                uc.SetWaste(0);
                pnlFinishUCs.ClientArea.Controls.Add(uc);
                uc.Dock = DockStyle.Top;
                uc.nIndex = lUCFinish.Count + 1;
                uc.BringToFront();
                lUCFinish.Add(uc);
                SelectFinish(sName);
            }
        }

        public void SelectFinish(string l_FinishName)
        {
            try
            {
                foreach (UCFinish f in lUCFinish)
                {
                    if (f.FinishName == l_FinishName)
                    {
                        Program.SelectedFinish = f;
                        lblSelectedArea.Text = f.FinishName;
                        f.Select(true);

                        setTransparencyProgressBar(f.Alpha);
                    }
                    else
                    {
                        f.Select(false);
                    }
                }
            }
            catch { }

            try
            {
                if (Program.PanMode == EditMode.Draw)
                {
                    DrawComponent();
                }
            }
            catch { }
        }

        public bool bExistsFinishName(string l_FinishName)
        {
            try
            {
                foreach (UCFinish f in lUCFinish)
                {
                    if (f.FinishName == l_FinishName)
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public bool bExistsLineName(string l_LineName)
        {
            try
            {
                foreach (UCLine f in lUCLine)
                {
                    if (f.LineName == l_LineName)
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        private void btnEditShape_Click(object sender, EventArgs e)
        {
            try
            {
                FormNewShape fN = new FormNewShape(this);
                if (Program.SelectedFinish == null)
                {
                    MessageBox.Show("You need to select a Finish item first.", "Finish", MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                    return;
                }
                fN.SetEdit(Program.SelectedFinish.FinishName, Program.SelectedFinish.pnlBackColor, Program.SelectedFinish.Alpha);
                if (fN.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Color col = fN.GetColor();
                    string sName = fN.GetName();
                    UCFinish uc = Program.SelectedFinish;
                    uc.SetColor(col, fN.GetAlpha());
                    uc.SetFinishName(sName);
                    currentComponent = new ComponentDef(Program.SelectedFinish.nIndex.ToString(), Program.SelectedFinish.Name,
                        "s.f.", ComponentType.Area, Program.SelectedFinish.pnlBackColor, Program.SelectedFinish.Alpha, 0, CalculationMode.Sum, Program.SelectedLine.b, false,
                        Program.bAreaZeroLine);
                    ApplyChangesToFinish();
                }
            }
            catch { }
        }

        private void btnChangeSelectedValue_Click(object sender, EventArgs e)
        {
            try
            {
                FormChangeValFinish fN = new FormChangeValFinish(this);
                if (Program.SelectedFinish == null)
                {
                    MessageBox.Show("You need to select a Finish item first.", "Finish", MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                    return;
                }
                UCFinish uc = Program.SelectedFinish;
                fN.SetVals(uc.FinishName, uc.dArea, uc.dPerim, uc.dWaste);
                if (fN.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    uc.SetArea(fN.GetArea());
                    uc.SetPerim(fN.GetPerim());
                    uc.SetWaste(fN.GetWaste());
                }
            }
            catch { }
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            if (drawingDoc.TempPolygon != null)
            {
                drawingDoc.TempPolygon.IsBeingEdited = false;
                AcceptCommand();
                pageWindow.ClearGrips();
                pageWindow.Regen();
            }
        }



        public void SelectLine(string l_LineName)
        {
            try
            {
                foreach (UCLine f in lUCLine)
                {
                    if (f.LineName == l_LineName)
                    {
                        Program.SelectedLine = f;
                        f.Select(true);
                    }
                    else
                    {
                        f.Select(false);
                    }
                }
            }
            catch { }

            try
            {
                if (Program.PanMode == EditMode.Draw)
                {
                    DrawComponent();
                }
            }
            catch { }
        }

        private void btnJump_Click(object sender, EventArgs e)
        {
            //Kiet Nguyen: move to common function
            completeLine_jum();

            //Program.jump_b1_b2_dup_Mode = 0;
            //LineModeSel();

            //if (drawingDoc.TempPolyline != null)
            //{
            //    //CurrentCommand = CommandMode.PlineClose;
            //    drawingDoc.TempPolyline.IsBeingEdited = false;
            //    AcceptCommand();
            //    pageWindow.ClearGrips();
            //    pageWindow.Regen();
            //}
        }

        private void btn1x_Click(object sender, EventArgs e)
        {
            Program.jump_b1_b2_dup_Mode = 1;
            LineModeSel();
        }

        private void btn2x_Click(object sender, EventArgs e)
        {
            Program.jump_b1_b2_dup_Mode = 2;
            LineModeSel();
        }

        private void LineModeSel()
        {
            switch (Program.jump_b1_b2_dup_Mode)
            {
                case 0:
                    SelectButton(btnJump, true);
                    SelectButton(btn1x, true);
                    SelectButton(btn2x, false);
                    SelectButton(btnDuplicate, false);
                    btnDuplicate.Enabled = false;
                    break;
                case 1:
                    SelectButton(btnJump, false);
                    SelectButton(btn1x, true);
                    SelectButton(btn2x, false);
                    SelectButton(btnDuplicate, false);
                    btnDuplicate.Enabled = false;
                    break;
                case 2:
                    SelectButton(btnJump, false);
                    SelectButton(btn1x, false);
                    SelectButton(btn2x, true);
                    SelectButton(btnDuplicate, false);
                    btnDuplicate.Enabled = true;
                    break;
                case 3:
                    SelectButton(btnDuplicate, true);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="bSelected"></param>
        private void SelectButton(UltraButton b, bool bSelected)
        {
            if (bSelected)
            {
                b.Appearance.BackColor = Color.Bisque;
                b.Appearance.BackColor2 = Color.Orange;
                b.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            }
            else
            {

                b.Appearance.BackColor = b.Appearance.BackColor2 = System.Drawing.SystemColors.Control;
            }
        }

        private void btnEditLine_Click(object sender, EventArgs e)
        {
            try
            {
                FormNewLine fN = new FormNewLine(this);
                if (Program.SelectedLine == null)
                {
                    MessageBox.Show("You need to select a Line item first.", "Finish", MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                    return;
                }
                fN.SetEdit(Program.SelectedLine.b);
                if (fN.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    UCLine uc = Program.SelectedLine;
                    uc.SetBase(fN.GetBase());
                }
            }
            catch { }
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            Program.jump_b1_b2_dup_Mode = 3;
            LineModeSel();
        }

        private void btnActivateLineDoorTO_Click(object sender, EventArgs e)
        {
            //Kiet.Nguyen: implement by [1100]
            //if (btnActivateLineDoorTO.Text == "Deactivate")
            //{
            //    SelectButton(btnActivateLineDoorTO, false);
            //    btnActivateLineDoorTO.Text = "Activate";
            //    DrawComponent();//Lets start again
            //}
            //else
            //{
            //    SelectButton(btnActivateLineDoorTO, true);
            //    btnActivateLineDoorTO.Text = "Deactivate";
            //    SilentCancelCommand();
            //    CurrentCommand = CommandMode.PLineDoorP1;
            //}

            //Kiet.Nguyen: implement by [1100]
            //Need to active button
            SelectButton(btnActivateLineDoorTO, true);
            SilentCancelCommand();
            CurrentCommand = CommandMode.PLineDoorP1;

            //Kiet Nguyen: [2100]
            btnDeactivateLineDoorTO.Visible = true;
        }

        /// <summary>
        /// Kiet.Nguyen
        /// event when click on Deactive button of line area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeactivateLineDoorTO_Click(object sender, EventArgs e)
        {
            //Need to deactive [active] button
            SelectButton(btnActivateLineDoorTO, false);
            DrawComponent();//Lets start again

            //Kiet Nguyen: [2100]
            btnDeactivateLineDoorTO.Visible = false;
        }


        private void btnAreaTakeOut_Click(object sender, EventArgs e)
        {
            DrawComponent(true);
        }

        private void btnAreaZeroLine_Click(object sender, EventArgs e)
        {
            Program.bAreaZeroLine = !Program.bAreaZeroLine;
            if (drawingDoc.TempPolygon != null)
            {
                drawingDoc.TempPolygon.UpdateLastZeroLine();
            }
            SelectButton(btnAreaZeroLine, Program.bAreaZeroLine);
        }

        private void btnHideLineDoorTO_Click(object sender, EventArgs e)
        {
            ////Kiet.Nguyen: implement by [1100]
            //Program.HideLineDoorTO = !Program.HideLineDoorTO;
            //SelectButton(btnHideLineDoorTO, Program.HideLineDoorTO);
            //btnHideLineDoorTO.Text = Program.HideLineDoorTO ? "Show" : "Hide";
            //pageWindow.Regen();

            //Kiet.Nguyen: implement by [1100]
            Program.HideLineDoorTO = true;
            SelectButton(btnHideLineDoorTO, true);
            pageWindow.Regen();

            //Kiet Nguyen: [2100]
            btnShowLineDoorTO.Visible = true;
        }

        /// <summary>
        /// Kiet.Nguyen
        /// event when click on Show button of line area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowLineDoorTO_Click(object sender, EventArgs e)
        {
            //Kiet.Nguyen: implement by [1100]
            Program.HideLineDoorTO = false;
            SelectButton(btnHideLineDoorTO, false);
            pageWindow.Regen();


            //Kiet Nguyen: [2100]
            btnShowLineDoorTO.Visible = false;
        }


        private void btnLineZeroLine_Click(object sender, EventArgs e)
        {
            Program.bLineZeroLine = !Program.bLineZeroLine;
            SelectButton(btnLineZeroLine, Program.bLineZeroLine);
        }


        private void btnApplySeparation_Click(object sender, EventArgs e)
        {
            //Kiet.Nguyen: remove item by [1100] sheet on Project Part 1.xlsx file
            //try
            //{
            //    Program.n2xSeparation = Int32.Parse(txt2xSeparation.Text);
            //}
            //catch
            //{
            //    txt2xSeparation.Text = "4";
            //    Program.n2xSeparation = 3;
            //}
            Program.n2xSeparation = 3;

            try
            {
                foreach (DrawingEntity enti in drawingDoc.Entities)
                {
                    try
                    {
                        if (enti.Type == DrawingEntity.EntityType.Polyline)
                        {
                            Polyline pl = (Polyline)enti;
                            if (pl.DashType == 1)
                            {
                                pl.HasChanged = true;
                            }
                        }
                    }
                    catch { }
                }
            }
            catch { }
            CommandMode prev = CurrentCommand;
            SilentCancelCommand();
            this.pageWindow.InvalidateCanvas();
            CurrentCommand = prev;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barTransparency_ValueChanged(object sender, EventArgs e)
        {
            if (Program.SelectedFinish == null)
            {
                MessageBox.Show("You need to select a Finish item first.", "Finish", MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                return;
            }

            //get currently select finished
            short l_Alpha = (short)barTransparency.Value;
            setTransparencyProgressBar(l_Alpha);

            try
            {
                if (Program.SelectedFinish == null)
                {
                    MessageBox.Show("You need to select a Finish item first.", "Finish", MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                    return;
                }

                UCFinish uc = Program.SelectedFinish;
                uc.SetAlPha(l_Alpha);

                currentComponent = new ComponentDef(Program.SelectedFinish.nIndex.ToString(), Program.SelectedFinish.Name,
                    "s.f.", ComponentType.Area, Program.SelectedFinish.pnlBackColor, Program.SelectedFinish.Alpha, 0, CalculationMode.Sum, Program.SelectedLine.b, false,
                    Program.bAreaZeroLine);
                ApplyChangesToFinish();
            }
            catch { }
        }


        /// <summary>
        /// Kiet Nguyen: [1450],[1400]
        /// set transparency progress bar
        /// </summary>
        /// <param name="value"></param>
        private void setTransparencyProgressBar(short value)
        {
            barTransparency.Value = value;
            txtTransparencyPercent.Text = calcPercentOfTransparency(value).ToString() + "%";
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double calcPercentOfTransparency(short value)
        {
            //convert to percent
            return Math.Round((((double)value) * 100 / 255), 0);
        }

        /// <summary>
        /// Kiet Nguyen: setting for select area mode
        /// </summary>
        private void selectedAreaMode()
        {
            Program.bLineMode = false;
            btnDuplicate.Enabled = false;
            if (!comeFromClick)
            {
                if (!((StateButtonTool)ultraToolbarsManager1.Tools["AreaMode"]).Checked)
                {
                    ((StateButtonTool)ultraToolbarsManager1.Tools["AreaMode"]).Checked = true;
                }

                //Kiet Nguyen: [1300]
                if (!((StateButtonTool)ultraToolbarsManager1.Tools["DrawMode"]).Checked)
                {
                    ((StateButtonTool)ultraToolbarsManager1.Tools["DrawMode"]).Checked = true;
                }
            }
        }

        /// <summary>
        /// Kiet Nguyen: setting for selected line mode
        /// </summary>
        private void selectedLineMode()
        {
            Program.bLineMode = true;
            if (!((StateButtonTool)ultraToolbarsManager1.Tools["LineMode"]).Checked)
            {
                ((StateButtonTool)ultraToolbarsManager1.Tools["LineMode"]).Checked = true;
            }
            SilentCancelCommand();

            if (Program.PanMode == EditMode.Draw)
            {
                DrawComponent();
            }
            //Kiet Nguyen: [1300]
            if (!((StateButtonTool)ultraToolbarsManager1.Tools["DrawMode"]).Checked)
            {
                ((StateButtonTool)ultraToolbarsManager1.Tools["DrawMode"]).Checked = true;
            }

            //Kiet nguyen: [1200] set default 1x when  lines mode
            SelectButton(btn1x, true);
        }

        //Kiet Nguyen 2: [310] sheet
        private void ultraToolbarsManager1_BeforeToolbarListDropdown(object sender, BeforeToolbarListDropdownEventArgs e)
        {
            e.ShowQuickAccessToolbarPositionMenuItem = false;
            e.ShowQuickAccessToolbarAddRemoveMenuItem = false;
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (btnLeft.Text == "<")
            {
                btnLeft.Text = ">";
                ultraDockManager1.PaneFromControl(tabPanes).Close();
            }
            else
            {
                btnLeft.Text = "<";
                ultraDockManager1.PaneFromControl(tabPanes).Show();
                ultraDockManager1.PaneFromControl(tabPanes).Pin();
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (btnRight.Text == ">")
            {
                btnRight.Text = "<";
                ultraDockManager1.PaneFromControl(ultraPanelRight).Close();
            }
            else
            {
                btnRight.Text = ">";
                ultraDockManager1.PaneFromControl(ultraPanelRight).Show();
                ultraDockManager1.PaneFromControl(ultraPanelRight).Pin();
            }
        }

        private void DrawingForm_SizeChanged(object sender, EventArgs e)
        {
            btnRight.Location = new Point(this.Size.Width - 40, 161);
        }

        private void ultraToolbarsManager1_PropertyChanged(object sender, Infragistics.Win.PropertyChangedEventArgs e)
        {
            if ((Infragistics.Win.UltraWinToolbars.PropertyIds)e.ChangeInfo.PropId == Infragistics.Win.UltraWinToolbars.PropertyIds.Ribbon && e.ChangeInfo.Trigger.ToString() == "Ribbon.IsMinimized")
            {
                if (ultraToolbarsManager1.Ribbon.DisplayMode == RibbonDisplayMode.TabsOnly)
                {
                    btnLeft.Location = new Point(2, 58);
                    btnRight.Location = new Point(this.Size.Width - 40, 58);
                }
                else if (ultraToolbarsManager1.Ribbon.DisplayMode == RibbonDisplayMode.Full)
                {
                    btnLeft.Location = new Point(2, 161);
                    btnRight.Location = new Point(this.Size.Width - 40, 161);
                }
            }
        }

        public void processClickForScale(Boolean firstClick, Boolean secondClick)
        {
            if (firstClick)
            {
                enableScaleClick1stPoint();
            }
            else if (secondClick)
            {
                enableScaleClick2NdPoint();
            }
        }

        /// <summary>
        /// Kiet Nguyen: Sheet 600 (Setting the scale - continued)
        /// </summary>
        public void enableScaleClick1stPoint()
        {
            lbClick1StPoint.BackColor = Color.Yellow;
            btnScaleCancel.Enabled = true;
            btnScaleReset.Enabled = true;
        }

        /// <summary>
        /// Kiet Nguyen: Sheet 600 (Setting the scale - continued)
        /// </summary>
        public void disableScaleClick1stPoint()
        {
            lbClick1StPoint.BackColor = Color.LightSteelBlue;
        }

        /// <summary>
        /// Kiet Nguyen: Sheet 600 (Setting the scale - continued)
        /// </summary>
        public void enableScaleClick2NdPoint()
        {
            lbClick2NdPoint.BackColor = Color.Yellow;
            txtScaleFeet.Enabled = true;
            txtScaleInches.Enabled = true;
            btnScaleOk.Enabled = true;
            btnScaleNew.Enabled = true;
        }

        /// <summary>
        /// Kiet Nguyen: Sheet 600 (Setting the scale - continued)
        /// </summary>
        public void disableScaleClick2NdPoint()
        {
            lbClick2NdPoint.BackColor = Color.LightSteelBlue;
            txtScaleFeet.Enabled = false;
            txtScaleInches.Enabled = false;
            btnScaleOk.Enabled = false;
            btnScaleNew.Enabled = false;
        }

        /// <summary>
        /// Kiet Nguyen: Sheet 600 (Setting the scale - continued)
        /// </summary>
        private void btnScaleNew_Click(object sender, EventArgs e)
        {
            disableScaleClick1stPoint();
            disableScaleClick2NdPoint();
            txtScaleFeet.Enabled = false;
            txtScaleInches.Enabled = false;
            btnScaleOk.Enabled = false;
            btnScaleNew.Enabled = false;
        }

        /// <summary>
        /// Kiet Nguyen: Sheet 600 (Setting the scale - continued)
        /// </summary>
        private void btnScaleCancel_Click(object sender, EventArgs e)
        {
            undoScaleAction();
        }

        /// <summary>
        /// Kiet Nguyen: Sheet 600 (Setting the scale - continued)
        /// </summary>
        private void btnScaleOk_Click(object sender, EventArgs e)
        {
            completeLine_jum();
        }

        private void btnScaleReset_Click(object sender, EventArgs e)
        {
            undoScaleAction();
        }

        /// <summary>
        /// Kiet Nguyen: Sheet 600 (Setting the scale - continued)
        /// </summary>
        private void undoScaleAction()
        {
            disableScaleClick1stPoint();
            disableScaleClick2NdPoint();
            txtScaleFeet.Enabled = false;
            txtScaleInches.Enabled = false;
            btnScaleOk.Enabled = false;
            btnScaleNew.Enabled = false;
        }

        /// <summary>
        /// Kiet Nguyen2: Sheet 600 (Show/hide custom group box)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbScale_ValueChanged(object sender, EventArgs e)
        {
            if (cmbScale.SelectedItem.DataValue.ToString().ToLower() == "custom")
                grpCustom.Visible = true;
            else
                grpCustom.Visible = false;
        }

        private void pageWindow_MouseClick(object sender, MouseEventArgs e)
        {
            if ((drawingDoc.TempBaseImage == null) && (drawingDoc.BaseImage == null))
            {
                MessageBox.Show("You need to load a PDF before drawing components.", "Warning", MessageBoxButtons.OK,
                                       MessageBoxIcon.Warning);
                return;
            }
        }

        private void pageWindow_Click(object sender, EventArgs e)
        {

        }
    }
}