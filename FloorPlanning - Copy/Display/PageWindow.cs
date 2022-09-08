using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections;
using System.Runtime.InteropServices;
using System.Data;

// GDI
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

// PDFSharp
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

// FloorPlanning
using FloorPlanning.Display.enums;
using System.Collections.Generic;

namespace FloorPlanning.Display
{
    /// <summary>
    /// Represents a window control for an XGraphics page.
    /// </summary> 
    public class PageWindow : UserControl
    {
        /// <summary>
        /// A delegate for invoking the render function.
        /// </summary>
        public delegate void RenderEvent(XGraphics gfx);

        private Container components = null;

        // Border around virtual page in pixel.
        const int leftBorder = 4;
        const int rightBorder = 6;  // because of shadow
        const int topBorder = 3;
        const int bottomBorder = 5;  // because of shadow
        const int horzBorders = leftBorder + rightBorder;
        const int vertBorders = topBorder + bottomBorder;
        const float ptPerMm = 0.352778f;

        private bool isDragging;

        int xdpiScreen;
        int ydpiScreen;

        System.Windows.Forms.RichTextBox textEditor;

        // Points in page mm coordinates
        PointF firstPointPicked = PointF.Empty;
        PointF secondPointPicked = PointF.Empty;
        PointF[] gripPoints;
        int selectedGrip = -1;
        static Pen gripPen = new Pen(Color.SteelBlue, 2f);
        static Pen gripPenFirst = new Pen(Color.Turquoise, 2f);
        static Pen gripPenOther = new Pen(Color.Red, 2f);
        static SolidBrush gripBrush = new SolidBrush(Color.FromArgb(128, Color.SteelBlue));

        // Points in screen pixel coordinates
        Point firstPoint = Point.Empty;
        Point secondPoint = Point.Empty;
        Point firstClickPoint = Point.Empty;
        Size gripSize;

        Bitmap canvasCache;
        bool canvasIsCached;
        bool cacheRenew = false;

        DrawingForm parentForm;

        long renderEventCounter = 0;

        public double minDist = Double.MaxValue;
        public PointF nearestP = PointF.Empty;
        public int nCorner = 0;
        public int nCorner2 = 0;

        Stopwatch zoomStopwatch;
        //private const int EM_GETLINECOUNT = 0xba;
        [DllImport("user32", EntryPoint = "SendMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);

        //Kiet Nguyen 2: [200] sheet
        public bool IsLoadPDF = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageWindow"/> class.
        /// </summary>
        public PageWindow()
        {
            this.canvas = new PageWindowCanvas(this);
            Controls.Add(this.canvas);

            textEditor = new System.Windows.Forms.RichTextBox();
            //textEditor.AcceptsReturn = true;
            textEditor.AcceptsTab = false;
            textEditor.BackColor = System.Drawing.Color.White;
            textEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textEditor.Enabled = false;
            textEditor.ForeColor = System.Drawing.Color.Black;
            textEditor.Location = new System.Drawing.Point(0, 0);
            textEditor.MaxLength = 1000;
            textEditor.Multiline = true;
            textEditor.ScrollBars = RichTextBoxScrollBars.None;

            textEditor.Name = "textBoxCommentEditor";
            textEditor.Size = new System.Drawing.Size(100, 26);
            textEditor.TabIndex = 12;
            textEditor.Visible = false;

            Controls.Add(textEditor);

            InitializeComponent();
            // textEditor events
            //this.textEditor.KeyPress += new KeyPressEventHandler(TextEditorKeyPress);
            this.textEditor.KeyDown += new KeyEventHandler(TextEditorKeyDown);

            // Mouse events
            this.canvas.MouseWheel += new MouseEventHandler(PageWindowMouseWheel);
            this.canvas.MouseMove += new MouseEventHandler(PageWindowMouseMove);
            this.canvas.MouseDown += new MouseEventHandler(PageWindowMouseDown);
            this.canvas.MouseClick += new MouseEventHandler(PageWindowMouseClick);
            this.canvas.MouseDoubleClick += new MouseEventHandler(PageWindowMouseDoubleClick);
            this.canvas.MouseUp += new MouseEventHandler(PageWindowMouseUp);
            this.canvas.MouseLeave += new EventHandler(PageWindowMouseLeave);

            // Keyboard events
            this.canvas.KeyDown += new KeyEventHandler(PageWindowKeyDownEvent);

            // Settings events
            Properties.Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Default_PropertyChanged);

            this.printableArea = new RectangleF();

            this.printableArea.GetType();

            // Prevent bogus compiler warnings
            this.posOffset = new Point();
            this.virtualPage = new Rectangle();

            zoomStopwatch = new Stopwatch();

            // Initialize grip size and ortho mode
            gripSize = new Size(9 * 5/*ADDED MAG*/, 9 * 5);

            // Initialize mouse cursor
            CurrentCursor = Cursors.Default;


        }

        bool bIsInserted = false;

        void PageWindow_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        void PageWindow_DragDrop(object sender, DragEventArgs e)
        {
            ;
        }

        public void ActivateCanvas()
        {
            this.canvas.Focus();
        }

        public Bitmap GetCanvasCache()
        {
            return canvasCache;
        }

        void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "GripSize")
                gripSize = new Size(9, 9);
        }

        private void TextEditorKeyDown(object sender, KeyEventArgs e)
        {
            // Pressing the escape key text editing
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Cancel();
                    break;
            }
        }

        SizeF oldSize = SizeF.Empty;

        public void PageWindowMouseWheel(object sender, MouseEventArgs e)
        {
            panningMode = false;

            if (e.Delta != 0)
            {
                if (IsInVirtualPage(e))
                {
                    if (DisableMouseMove)
                    {
                        ProportionalZoom(e.Delta > 0, lastPointSelected2Move.X, lastPointSelected2Move.Y);
                    }
                    else
                    {
                        ProportionalZoom(e.Delta > 0, e.X, e.Y);
                    }
                }
            }
        }


        private void PageWindowKeysMove(object sender, MouseEventArgs e)
        {
            return;//ADDED MAG

        }

        private void PageWindowMouseMove(object sender, MouseEventArgs e)
        {
            if (DisableMouseMove)
                return;

            if (zoomStopwatch.ElapsedMilliseconds > 250)
                zoomStopwatch.Reset();

            if (IsInVirtualPage(e))
            {
                if (Program.PanMode == EditMode.Pan)
                {
                    this.Cursor = Cursors.Hand;//MAG currentCommandCursor;
                }
                else if (Program.PanMode == EditMode.Draw)
                {
                    this.Cursor = Cursors.Cross;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }

                if ((Program.PanMode == EditMode.Pan) && (panningMode))//MAG(panningMode)
                {
                    //MAG this.Cursor = Cursors.NoMove2D;

                    Rectangle rcCanvas = canvas.ClientRectangle;
                    int newY = e.Y - panningBaseY;
                    int newX;

                    if (this.zoom != enums.Zoom.FitWidth)
                        newX = e.X - panningBaseX;
                    else
                        newX = virtualPage.X;

                    if (virtualPage.Height > canvas.ClientRectangle.Height)
                        virtualPage.Y = Math.Min(Math.Max(rcCanvas.Height - virtualPage.Height - vertBorders, newY), topBorder);
                    else
                        virtualPage.Y = Math.Max(Math.Min(rcCanvas.Height - virtualPage.Height - vertBorders, newY), topBorder);

                    if (virtualPage.Width > canvas.ClientRectangle.Width)
                        virtualPage.X = Math.Min(Math.Max(rcCanvas.Width - virtualPage.Width - horzBorders, newX), leftBorder);
                    else
                        virtualPage.X = Math.Max(Math.Min(rcCanvas.Width - virtualPage.Width - horzBorders, newX), leftBorder);

                    canvasIsCached = false;
                    this.canvas.Invalidate();

                    if (getPointMode)
                    {
                        firstPoint = e.Location;

                        SetTrackingValues(IsOrthoOn);
                    }

                    if (getSecondPointMode)
                    {
                        secondPoint = e.Location;

                        SetTrackingValues(IsOrthoOn);
                    }
                }
                else
                {
                    bool bInGrip = false;
                    if (getPointMode)
                    {
                        firstPoint = e.Location;
                        firstPointPicked = PageLocation(firstPoint);

                        if (cacheRenew)
                        {
                            SetCanvasCache();
                        }

                        SetTrackingValues(IsOrthoOn);
                    }
                    else if (getSecondPointMode) // && !zoomStopwatch.IsRunning)
                    {
                        secondPoint = e.Location;
                        secondPointPicked = PageLocation(secondPoint);

                        if (cacheRenew)
                        {
                            SetCanvasCache();
                        }

                        SetTrackingValues(IsOrthoOn);
                    }
                    else if (gripPoints != null)
                    {
                        // Check if cursor is in a grip

                        int i = 0;
                        foreach (PointF point in gripPoints)
                        {
                            if (PointIsInGrip(point, e.Location))
                            {
                                //MAG this.Cursor = Cursors.Cross;
                                bInGrip = true;
                                break;
                            }

                            i++;
                        }
                    }
                    if ((e.Button == MouseButtons.Left) && (bInGrip == false))//MouseMove when hold left button
                    {
                        if (parentForm.SelectedEntities.Count == 0)
                        {
                            if (parentForm.TempEntityDoesExist == false)
                                return;
                            if (this.parentForm.SelectedEntity == null)
                                return;
                        }
                        if (gripPoints != null)
                        {
                            if (firstClickPoint == Point.Empty)
                            {
                                firstClickPoint = e.Location;
                            }
                            minDist = Double.MaxValue;
                            nearestP = PointF.Empty;
                            //MULTIPLE - orig with count == 0
                            if (parentForm.SelectedEntities.Count == 0)
                            {

                                //lA[0].GetRectangleF();
                                IsDragging = true;
                                getPointMode = true;
                                selectedGrip = 0;
                                Point pointGrip0 = CanvasLocation(gripPoints[0]);
                                Point tempOffsetPointMove = new Point(e.Location.X - pointGrip0.X, e.Location.Y - pointGrip0.Y);
                                float zoomFactor = this.pageSize.Width / virtualPage.Width;
                                parentForm.GripCommand();

                            }
                            else
                            {
                                IsDragging = true;
                                getPointMode = true;
                                selectedGrip = 0;
                                float zoomFactor = this.pageSize.Width / virtualPage.Width;
                                ClearGrips();
                                DrawingEntity ent = parentForm.SelectedEntities[0];
                                {
                                    Point tempOffsetPointMove = new Point(e.Location.X - firstClickPoint.X, e.Location.Y - firstClickPoint.Y);
                                    if (nearestP != PointF.Empty)
                                    {
                                        gripPoints = new PointF[parentForm.SelectedEntities.Count + 1];
                                    }
                                    else
                                    {
                                        gripPoints = new PointF[parentForm.SelectedEntities.Count];
                                    }
                                    int iS = 0;
                                    for (iS = 0; iS < parentForm.SelectedEntities.Count; iS++)
                                    {
                                        Point pointGrip0 = CanvasLocation(new PointF(parentForm.SelectedInitialGrips[iS].X, parentForm.SelectedInitialGrips[iS].Y));
                                        Point pFinal = new Point(tempOffsetPointMove.X + pointGrip0.X, tempOffsetPointMove.Y + pointGrip0.Y);
                                    }
                                    if (nearestP != PointF.Empty)
                                    {
                                        gripPoints[iS] = new PointF(nearestP.X * DrawingDoc.mmPerPoint, nearestP.Y * DrawingDoc.mmPerPoint);
                                    }

                                    Regen();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                panningMode = false;
                this.Cursor = Cursors.Default;
            }
        }

        private void PageWindowMouseDown(object sender, MouseEventArgs e)
        {
            if (IsInVirtualPage(e))
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (!getTextMode)
                    {
                        ReCenter(true);

                        if (this.zoom != enums.Zoom.FullPage)
                        {
                            panningBaseY = e.Y - virtualPage.Y;
                            panningBaseX = e.X - virtualPage.X;
                            panningMode = true;
                        }
                    }
                }
            }
            else
                panningMode = false;
        }

        /// <summary>
        /// Kiet Nguyen: [1500] Shortcust move layourt
        /// </summary>
        public void moveLayourtToLeft()
        {
            virtualPage.X = virtualPage.X + 100;
            secondPointPicked.X = secondPointPicked.X + 100;

            canvasIsCached = false;
            this.canvas.Invalidate();
        }

        /// <summary>
        /// Kiet Nguyen: [1500] Shortcust move layourt
        /// </summary>
        public void moveLayourtToRight()
        {
            virtualPage.X = virtualPage.X - 100;
            secondPointPicked.X = secondPointPicked.X - 100;
            canvasIsCached = false;
            this.canvas.Invalidate();
        }

        /// <summary>
        /// Kiet Nguyen: [1500] Shortcust move layourt
        /// </summary>
        public void moveLayourtToUp()
        {
            virtualPage.Y = virtualPage.Y  - 100;
            secondPointPicked.Y = secondPointPicked.Y - 100;

            canvasIsCached = false;
            this.canvas.Invalidate();

        }
        /// <summary>
        /// Kiet Nguyen: [1500] Shortcust move layourt
        /// </summary>
        public void moveLayourtDown()
        {
            virtualPage.Y = virtualPage.Y + 100;
            secondPointPicked.Y = secondPointPicked.Y + 100;
            canvasIsCached = false;
            this.canvas.Invalidate();
        }

        private void PageWindowMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (IsInVirtualPage(e))
            {
                if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.None)
                {
                    if (parentForm.TempEntityDoesExist)
                    {
                        parentForm.PointDoubleClick(PageLocation(e.Location));
                    }
                }
            }
        }

        private Point lastPointSelected2Move = new Point(0, 0);
        public bool DisableMouseMove = false;

        public void StopMoveKeys()
        {
            MouseEventArgs e = new MouseEventArgs(MouseButtons.Left, 1, lastPointSelected2Move.X, lastPointSelected2Move.Y, 0);
            PageWindowMouseClick(null, e);
        }

        public void StartMoveKeys()
        {
            if (gripPoints != null)
            {
                lastPointSelected2Move = CanvasLocation(gripPoints[0]);
            }
            MouseEventArgs e = new MouseEventArgs(MouseButtons.Left, 1, lastPointSelected2Move.X, lastPointSelected2Move.Y, 0);
            PageWindowMouseClick(null, e);
        }

        public void DoKeys(Keys k)
        {
            return;//ADDED MAG

        }

        private void PageWindowMouseClick(object sender, MouseEventArgs e)
        {
            //Kiet Nguyen 2: [200] sheet
            if (!IsLoadPDF)
            {
                MessageBox.Show("You need to load a PDF before drawing components.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (IsInVirtualPage(e))
            {
                if (e.Button == MouseButtons.Left)
                {
                    firstClickPoint = Point.Empty;
                    if (getPointMode)
                    {
                        getPointMode = false;
                        IsDragging = false;
                        CurrentCursor = Cursors.Default;

                        firstPoint = e.Location;
                        firstPointPicked = PageLocation(firstPoint);

                        parentForm.PointInput(firstPointPicked);

                        if (selectedGrip > -1)
                        {
                            selectedGrip = -1;
                            Update();
                        }

                        //BUG several selected components need to be unselected
                        if ((parentForm.SelectedEntities.Count > 0) && (Control.ModifierKeys != Keys.Control))
                        {
                            Regen();
                            ClearGrips();
                            parentForm.ClearSelectedEntities();

                            minDist = Double.MaxValue;
                            nearestP = PointF.Empty;
                            nCorner = 0;
                            nCorner2 = 0;
                        }
                        //END BUG

                        //Kiet Nguyen:Sheet 600 scale
                        parentForm.processClickForScale(true,false);
                    }
                    else if (getSecondPointMode)
                    {
                        getSecondPointMode = false;
                        IsDragging = false;
                        CurrentCursor = Cursors.Default;

                        secondPoint = e.Location;
                        if (IsOrthoOn)
                        {
                            secondPointPicked = PageLocation(secondPoint);
                            float dx = Math.Abs(firstPointPicked.X - secondPointPicked.X);
                            float dy = Math.Abs(firstPointPicked.Y - secondPointPicked.Y);

                            if (dx > dy)
                                secondPointPicked.Y = firstPointPicked.Y;
                            else
                                secondPointPicked.X = firstPointPicked.X;
                        }

                        secondPoint = Point.Empty;

                        parentForm.PointInput(secondPointPicked);

                        //Kiet Nguyen:Sheet 600 scale
                        parentForm.processClickForScale(false,true);
                    }
                    else if (getTextMode)
                    {
                        getTextMode = false;
                        textEditor.Enabled = false;
                        textEditor.Visible = false;
                        //parentForm.TextInput(textEditor.Text);
                        parentForm.AcceptCommand();
                    }
                    else if (gripPoints != null)
                    {
                        IsDragging = false;

                        int i = 0;
                        foreach (PointF point in gripPoints)
                        {
                            if (PointIsInGrip(point, e.Location))
                            {
                                lastPointSelected2Move = e.Location;
                                IsDragging = true;
                                getPointMode = true;
                                //MAG this.Cursor = Cursors.Cross;
                                selectedGrip = i;
                                parentForm.GripCommand();
                                break;
                            }

                            i++;
                        }



                        if (!IsDragging)
                        {
                            parentForm.AcceptCommand();
                            parentForm.SelectEntity(PageLocation(e.Location));
                        }
                    }
                    else
                    {
                        if (parentForm.TempEntityDoesExist)
                        {
                            if (parentForm.SelectedEntity == null)
                                parentForm.AcceptCommand();
                            else
                                if (parentForm.FindEntity(PageLocation(e.Location)) != parentForm.SelectedEntity)
                                parentForm.AcceptCommand();
                        }
                        else
                        {
                            parentForm.SelectEntity(PageLocation(e.Location));
                        }
                    }
                }
            }
        }

        private void PageWindowMouseUp(object sender, MouseEventArgs e)
        {
            if (panningMode)
            {
                panningMode = false;
                cacheRenew = true;
                //MAG this.Cursor = currentCommandCursor;
            }

            try
            {
                return;//ADDED MAG
                if (Control.ModifierKeys == Keys.Control)
                {
                    if (parentForm.SelectedEntities.Count == 0)
                    {
                        if (parentForm.TempEntityDoesExist == false)
                            return;
                        if (this.parentForm.SelectedEntity == null)
                            return;
                    }
                    if (parentForm.SelectedEntities.Count > 1)
                        return;
                }
            }
            catch { }
        }
        private bool panningMode;
        private int panningBaseX, panningBaseY;

        private void PageWindowMouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;//MAG
            if (getPointMode)
            {
                firstPoint = Point.Empty;
                ClearTracking();
            }

            if (getSecondPointMode)
            {
                secondPoint = Point.Empty;
                //secondPointPicked = Point.Empty;
                ClearTracking();
            }
        }

        public void PageWindowKeyDownEvent(object sender, KeyEventArgs e)
        {
            // Kiet Nguyen: remove last point for [1500] Shortcusts
            //Because shortcust will be define in Shortcuts UI => don't need default space
            //switch (e.KeyCode)
            //{
            //    case Keys.Space:
            //        parentForm.RemoveLastPoint();
            //        break;
            //}           
        }

        /// <summary>
        /// Kiet Nguyen: remove last point for [1500] Shortcusts
        /// </summary>
        public void RemoveLastPoint()
        {
            parentForm.RemoveLastPoint();
        }

        private bool IsOrthoOn
        {
            get
            {
                return false;
            }
        }

        public PointF PageLocation(Point canvasLocation)
        {
            if (canvasLocation.IsEmpty)
            {
                return PointF.Empty;
            }
            else
            {
                PointF pageLocation = new PointF();

                float zoomFactor = this.pageSize.Width / virtualPage.Width;

                float x, y;
                x = (canvasLocation.X - virtualPage.Left) * zoomFactor * ptPerMm;
                y = (canvasLocation.Y - virtualPage.Top) * zoomFactor * ptPerMm;

                if (!parentForm.IsRotated)
                {
                    pageLocation.X = x;
                    pageLocation.Y = y;
                }
                else
                {
                    pageLocation.X = y;
                    pageLocation.Y = -x + pageSize.Width * ptPerMm;
                }

                return pageLocation;
            }
        }

        private Size CanvasSize(SizeF size)
        {
            float zoomFactor = this.pageSize.Width / virtualPage.Width;

            return new Size((int)(size.Width / (zoomFactor * ptPerMm) + 0.5),
                            (int)(size.Height / (zoomFactor * ptPerMm) + 0.5));
        }

        private int CanvasLength(float length)
        {
            float zoomFactor = this.pageSize.Width / virtualPage.Width;

            return (int)(length / (zoomFactor * ptPerMm) + 0.5);
        }

        public Point CanvasLocation(PointF pageLocation)
        {
            Point canvasLocation = new Point();

            float zoomFactor = this.pageSize.Width / virtualPage.Width;

            int x, y;
            x = (int)(pageLocation.X / (zoomFactor * ptPerMm) + 0.5);
            y = (int)(pageLocation.Y / (zoomFactor * ptPerMm) + 0.5);

            if (!parentForm.IsRotated)
            {
                canvasLocation.X = x + virtualPage.Left;
                canvasLocation.Y = y + virtualPage.Top;
            }
            else
            {
                canvasLocation.X = -y + virtualPage.Left + virtualPage.Width;
                canvasLocation.Y = x + virtualPage.Top;
            }

            return canvasLocation;
        }


        public void PlaceGrips(PointF[] points)
        {
            gripPoints = points;

            Graphics gfx = this.canvas.CreateGraphics();

            DrawGrips(gfx);
        }

        public void ClearGrips()
        {
            gripPoints = null;
            selectedGrip = -1;
            parentForm.SelectedEntity = null;
        }

        public void DrawGrips(Graphics gfx)
        {
            int i = 0;
            foreach (PointF point in gripPoints)
            {
                DrawGripAt(point, i == selectedGrip, gfx, i, gripPoints.Length - 1);
                i++;
            }
        }

        private void DrawGripAt(PointF point, bool isSelected, Graphics gfx, int i, int LastGrip)
        {
            if ((i == LastGrip) && (i == 1))//If only 2, don't draw last one
            {
                return;
            }
            Point canvasPoint = CanvasLocation(point);
            canvasPoint.X -= gripSize.Width / 2;
            canvasPoint.Y -= gripSize.Height / 2;

            Rectangle rect = new Rectangle(canvasPoint, gripSize);

            gfx.DrawEllipse((i == 0) ? gripPenFirst : gripPenOther, rect);
        }

        private bool PointIsInGrip(PointF gripPoint, Point location)
        {
            Point point = CanvasLocation(gripPoint);

            point.X += gripSize.Width / 2;
            point.Y += gripSize.Height / 2;

            if (point.X > location.X && point.Y > location.Y)
                if (point.X < location.X + gripSize.Width && point.Y < location.Y + gripSize.Height)
                    return true;

            return false;
        }

        private void ClearTracking()
        {
            canvas.Refresh();
        }

        public void SetTrackingValues(bool orthoMode)
        {
            firstPoint = CanvasLocation(firstPointPicked);

            if (!secondPoint.IsEmpty)
            {
                if (orthoMode)
                {
                    int dx = Math.Abs(firstPoint.X - secondPoint.X);
                    int dy = Math.Abs(firstPoint.Y - secondPoint.Y);

                    if (dx > dy)
                        secondPoint.Y = firstPoint.Y;
                    else
                        secondPoint.X = firstPoint.X;
                }
            }

            if (IsDragging)
                ClearTracking();
        }

        public PointF GetMagnification()
        {
            float magnificationX = virtualPage.Width / this.pageSize.Width;
            float magnificationY = virtualPage.Height / this.pageSize.Height;
            return new PointF(magnificationX, magnificationY);
        }

        public void DisplayTracking(Graphics gfx)
        {
            float magnificationX = virtualPage.Width / this.pageSize.Width;
            float magnificationY = virtualPage.Height / this.pageSize.Height;

            Matrix matrix = new Matrix();
            matrix.Translate(virtualPage.X, virtualPage.Y);
            matrix.Scale(magnificationX, magnificationY);
            gfx.Transform = matrix;

            XGraphics xgfx = XGraphics.FromGraphics(gfx, new XSize(this.pageSize.Width, this.pageSize.Height));

            if (getPointMode)
            {
                parentForm.RenderTempEntity(xgfx, PageLocation(firstPoint));
            }
            else if (getSecondPointMode)
            {
                secondPointPicked = PageLocation(secondPoint);

                if (IsOrthoOn && !secondPoint.IsEmpty)
                {
                    //firstPoint = CanvasLocation(firstPointPicked);
                    float dx = Math.Abs(firstPointPicked.X - secondPointPicked.X);
                    float dy = Math.Abs(firstPointPicked.Y - secondPointPicked.Y);

                    if (dx > dy)
                        secondPointPicked.Y = firstPointPicked.Y;
                    else
                        secondPointPicked.X = firstPointPicked.X;
                }

                parentForm.RenderTempEntity(xgfx, secondPointPicked);
                parentForm.PointUpdate();
            }
            else if (getTextMode)
            {
                ;// parentForm.RenderTempTextEntity(xgfx, textEditor.Text);
            }
            else
                parentForm.RenderTempEntity(xgfx);

            if (gripPoints != null)
            {
                gfx.SmoothingMode = SmoothingMode.None;
                gfx.ResetTransform();
                DrawGrips(gfx);
                gfx.SmoothingMode = SmoothingMode.HighQuality;
            }
        }

        /// <summary>
        /// Caches the entire canvas as a bitmap, to speed repainting
        /// while dragging new or edited temporary entity
        /// </summary>
        private void SetCanvasCache()
        {
            cacheRenew = false;

            if (!canvasIsCached)
            {
                ClearCache();
                SetCache();
            }
        }

        public void SetCache()
        {
            try
            {
                canvasCache = new Bitmap(canvas.Width, canvas.Height);

                canvas.DrawToBitmap(canvasCache, canvas.Bounds);
                canvas.BackgroundImage = canvasCache;

                canvasIsCached = true;
            }
            catch { }
        }

        // Used to create a bitmap directly from the Render method, without painting to window
        public void SetCacheFromRender()
        {
            canvasCache = new Bitmap(canvas.Width, canvas.Height);

            Graphics gfx = Graphics.FromImage(canvasCache);

            PaintBackground(gfx);
            RenderPage(gfx);
            canvas.BackgroundImage = canvasCache;

            canvasIsCached = true;
        }

        public void ClearCache()
        {
            if (canvasCache != null)
            {
                canvasCache.Dispose();
                GC.Collect();
                //System.GC.WaitForPendingFinalizers();
            }
            canvasCache = null;
            canvas.BackgroundImage = null;
            canvasIsCached = false;
        }

        /// <summary>
        /// Tells the PageWindow to request the input of a point from the user.
        /// </summary>
        /// <param name="form"></param>
        public void GetPointFromUser(Cursor cursor)
        {
            CurrentCursor = cursor;

            secondPoint = Point.Empty;
            secondPointPicked = PointF.Empty;

            SetCanvasCache();

            getPointMode = true;
            isDragging = true;
        }
        bool getPointMode;
        Cursor currentCommandCursor;

        public void GetPointFromUser()
        {
            GetPointFromUser(Cursors.Cross);
        }

        //Point selectedPoint;

        /// <summary>
        /// Tells the PageWindow to request the input of a second point from the user.
        /// This will show a line from firstPointPicked to the mouse cursor.
        /// </summary>
        /// <param name="form"></param>
        public void GetSecondPointFromUser()
        {
            GetSecondPointFromUser(firstPointPicked);
        }

        public void GetSecondPointFromUser(PointF fromPoint)
        {
            CurrentCursor = Cursors.Cross;

            firstPointPicked = fromPoint;

            SetCanvasCache();

            getSecondPointMode = true;
            isDragging = true;
        }
        bool getSecondPointMode;

        public void TempEntityHold()
        {
            SetCanvasCache();
            isDragging = true;
        }

        public PointF FirstPointPicked
        {
            get { return firstPointPicked; }
            set
            {
                firstPointPicked = value;

                // This might be called before parentForm is set
                if (parentForm != null)
                    firstPoint = CanvasLocation(firstPointPicked);
            }
        }

        public void UpateTextEditorSize(SizeF size)
        {
            SizeF newSize = new SizeF(size.Width, size.Height);
            textEditor.Size = CanvasSize(newSize);
        }

        public void GetTextFromUser(RectangleF textRectMM, Font font, Color color, String text)
        {
            SetCanvasCache();

            getTextMode = true;
            isDragging = true;

            textEditor.ForeColor = color;
            textEditor.Tag = font.Size;
            textEditor.BringToFront();
            textEditor.Text = text;

            Point location = CanvasLocation(textRectMM.Location);
            location.X = location.X - 1;
            location.Y = location.Y - 1;
            textEditor.Location = location;

            Size size = CanvasSize(textRectMM.Size);
            size.Width = size.Width + 1;
            size.Height = size.Height + 2;
            textEditor.Size = size;

            float zoomFactor = ZoomPercent / 100f;

            Font commentFont = new Font(font.FontFamily, font.Size * zoomFactor, font.Style, GraphicsUnit.Point);

            //commentFont = new Font(font.FontFamily, font.Size, font.Style, GraphicsUnit.Point);

            textEditor.Font = commentFont;
            textEditor.Enabled = true;
            textEditor.Visible = true;
            textEditor.Focus();
            //textEditor.SelectedText = "";
        }
        bool getTextMode;

        /// <summary>
        /// Centers the virtual page on the canvas, at the current zoom percent.
        /// </summary>
        /// <param name="animate"></param>
        /// <returns></returns>
        private bool ReCenter(bool animate)
        {
            int newX, newY;
            Rectangle rcCanvas = canvas.ClientRectangle;

            if (virtualPage.Height > canvas.ClientRectangle.Height)
                newY = Math.Min(Math.Max(rcCanvas.Height - virtualPage.Height - vertBorders, virtualPage.Y), topBorder);
            else
                newY = Math.Max(Math.Min(rcCanvas.Height - virtualPage.Height - vertBorders, virtualPage.Y), topBorder);

            if (virtualPage.Width > canvas.ClientRectangle.Width)
                newX = Math.Min(Math.Max(rcCanvas.Width - virtualPage.Width - horzBorders, virtualPage.X), leftBorder);
            else
                newX = Math.Max(Math.Min(rcCanvas.Width - virtualPage.Width - horzBorders, virtualPage.X), leftBorder);

            if (newY != virtualPage.Y || newX != virtualPage.X)
            {
                if (animate)
                    AnimateToXY(newX, newY);
                else
                    SetToXY(newX, newY);

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Moves the virtual page to the newX, newY location, without animating.
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        private void SetToXY(int newX, int newY)
        {
            virtualPage.X = newX;
            virtualPage.Y = newY;
            canvasIsCached = false;
            this.canvas.Invalidate();
            //this.canvas.Update();
        }

        /// <summary>
        /// Moves the virtual page to newX, newY, while zooming to newZoom, with animation.
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="newZoom"></param>
        private void AnimateToXYZoom(int newX, int newY, float newZoom)
        {
            float oldX = (float)virtualPage.X;
            float oldY = (float)virtualPage.Y;
            float oldZoom = zoomPercent;

            float deltaX = oldX - newX;
            float deltaY = oldY - newY;
            float deltaZoom = oldZoom - newZoom;

            long milliseconds = 125, i = 0, elapsed = 0;
            float halfPi = (float)(Math.PI / 2.0);
            double sinPhi;
            bool doLoop = true;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            while (doLoop)
            {
                elapsed = stopWatch.ElapsedMilliseconds;
                if (elapsed < milliseconds)
                {
                    sinPhi = Math.Sin(elapsed * halfPi / milliseconds);
                }
                else
                {
                    sinPhi = 1;
                    doLoop = false;
                }

                virtualPage.X = (int)(oldX - deltaX * sinPhi);
                virtualPage.Y = (int)(oldY - deltaY * sinPhi);

                if (deltaZoom > 0)
                    this.ZoomPercent = (oldZoom - deltaZoom * (float)sinPhi);

                canvasIsCached = false;
                this.canvas.Invalidate();
                this.canvas.Update();
                i++;
            }
            stopWatch.Stop();

        }

        /// <summary>
        /// Moves the virtual page to the newX, newY location, with animation.
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        private void AnimateToXY(int newX, int newY)
        {
            AnimateToXYZoom(newX, newY, zoomPercent);


        }

        /// <summary>
        /// Returns true if the mouse cursor is within the virtual page, false if not.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool IsInVirtualPage(MouseEventArgs e)
        {
            return (virtualPage.Left < e.X && virtualPage.Right > e.X)
                && (virtualPage.Top < e.Y && virtualPage.Bottom > e.Y);
        }

        public float ProportionalZoom(bool larger)
        {
            return ProportionalZoom(larger, canvas.Width / 2, canvas.Height / 2);
        }

        public void MoveToZero(int X, int Y, float zoomStep)
        {
            try
            {
                float finalZoom = this.ZoomPercent * zoomStep;
                this.ZoomPercent = this.ZoomPercent * zoomStep;
                virtualPage.X -= (int)((float)(X - virtualPage.X) * (zoomStep - 1.0f) - 0.5f);
                virtualPage.Y -= (int)((float)(Y - virtualPage.Y) * (zoomStep - 1.0f) - 0.5f);


                CalculateWidthHeight();
                canvasIsCached = false;
                this.canvas.Invalidate();
            }
            catch { }
        }

        public void ProportionalZoomWithFactor(int X, int Y, float zoomStep)
        {
            float finalZoom = Math.Min((float)Zoom.Maximum, this.ZoomPercent * zoomStep);

            float firstnewZoom = Math.Min((this.ZoomPercent * 1.1f), (float)Zoom.Maximum);

            if (firstnewZoom >= finalZoom)
                return;


        }

        public float ProportionalZoom(bool larger, int X, int Y)
        {
            float zoomStep = 1.1f;
            float round;
            float currentZoom = this.ZoomPercent;
            float newZoom;

            float maxZoom = (float)Zoom.Maximum;

            if (larger)
            {
                newZoom = Math.Min((currentZoom * zoomStep), maxZoom);

                round = 0.5f;
            }
            else
            {
                newZoom = Math.Max((currentZoom / zoomStep), minZoom);

                round = -0.5f;
            }

            if (newZoom / minZoom < 1.01)
                newZoom = minZoom;

            if (newZoom / maxZoom > 0.99)
                newZoom = maxZoom;

            float zoomFactor = newZoom / currentZoom;

            if (this.zoomPercent != newZoom)
            {
                zoomStopwatch.Restart();

                if (newZoom == minZoom)
                {
                    Rectangle rcCanvas = canvas.ClientRectangle;
                    int newX = leftBorder + (rcCanvas.Width - horzBorders - (int)((float)virtualPage.Width * zoomFactor)) / 2;
                    int newY = topBorder + (rcCanvas.Height - vertBorders - (int)((float)virtualPage.Height * zoomFactor)) / 2;

                    if (virtualPage.X != newX || virtualPage.Y != newY)
                        AnimateToXYZoom(newX, newY, newZoom);

                    this.Zoom = enums.Zoom.FullPage;
                }
                else
                {
                    this.ZoomPercent = newZoom;
                    virtualPage.X -= (int)((float)(X - virtualPage.X) * (zoomFactor - 1.0f) + round);
                    virtualPage.Y -= (int)((float)(Y - virtualPage.Y) * (zoomFactor - 1.0f) + round);
                    this.canvas.Invalidate();
                }

                OnZoomChanged(new EventArgs());
            }

            return newZoom;
        }
        readonly PageWindowCanvas canvas;

        public PageWindowCanvas GetCanvas()
        {
            return canvas;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets or sets the border style of the control.
        /// </summary>
        /// <value></value>
        [DefaultValue((int)BorderStyle.Fixed3D), Description("Determines the style of the border."), Category("Preview Properties")]
        public new BorderStyle BorderStyle
        {
            get { return this.borderStyle; }
            set
            {
                if (!Enum.IsDefined(typeof(BorderStyle), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));

                if (value != this.borderStyle)
                {
                    this.borderStyle = value;
                    LayoutChildren();
                }
            }
        }
        BorderStyle borderStyle = BorderStyle.Fixed3D;


        /// <summary>
        /// Gets or sets a predefined zoom factor.
        /// </summary>
        [DefaultValue((int)Zoom.FullPage), Description("Determines the zoom of the page."), Category("Preview Properties")]
        public Zoom Zoom
        {
            get { return this.zoom; }
            set
            {
                if ((int)value < (int)Zoom.Mininum || (int)value > (int)Zoom.Maximum)
                {
                    if (!Enum.IsDefined(typeof(Zoom), value))
                        throw new InvalidEnumArgumentException("value", (int)value, typeof(Zoom));
                }
                if (value != this.zoom)
                {
                    this.zoom = value;
                }
            }
        }
        Zoom zoom;

        public void Cancel()
        {
            parentForm.CancelCommand();
        }

        public void StopCommand()
        {
            CurrentCursor = Cursors.Default;

            firstPoint = Point.Empty;
            secondPoint = Point.Empty;
            selectedGrip = -1;

            if (getPointMode)
            {
                getPointMode = false;
            }

            if (getSecondPointMode)
            {
                getSecondPointMode = false;
            }

            if (getTextMode)
            {
                getTextMode = false;
            }

            IsDragging = false;

            ClearGrips();
            ClearTracking();

            textEditor.Visible = false;
            textEditor.Enabled = false;
            //textEditor.Text = "";
        }

        public void SelectionFinishedCommand()
        {
            CurrentCursor = Cursors.Default;

            firstPoint = Point.Empty;
            secondPoint = Point.Empty;
            selectedGrip = -1;

            if (getPointMode)
            {
                getPointMode = false;
            }

            if (getSecondPointMode)
            {
                getSecondPointMode = false;
            }

            if (getTextMode)
            {
                getTextMode = false;
            }

            IsDragging = false;

            //ClearGrips();
            //ClearTracking();

            textEditor.Visible = false;
            textEditor.Enabled = false;
            //textEditor.Text = "";
        }

        public void Regen()
        {
            Regen(false);
        }

        public void Regen(bool animate)
        {
            CalculateMinZoom();
            CalculateWindowDimension();

            if (!ReCenter(animate))
                this.canvas.Invalidate();
        }

        public void InvalidateCanvas()
        {
            this.Invalidate();
        }

        /// <summary>
        /// Gets or sets an arbitrary zoom factor. The range is from 10 to 800.
        /// </summary>
        //[DefaultValue((int)Zoom.FullPage), Description("Determines the zoom of the page."), Category("Window Properties")]
        public float ZoomPercent
        {
            get { return this.zoomPercent; }
            set
            {
                if (value < (float)Zoom.Mininum || value > (float)Zoom.Maximum)
                    throw new ArgumentOutOfRangeException("value", value,
                    String.Format("Value must between {0} and {1}.", (int)Zoom.Mininum, (int)Zoom.Maximum));

                if (value != this.zoomPercent)
                {
                    zoom = (Zoom)value;
                    zoomPercent = value;
                    ClearCache();
                    CalculateWindowDimension();
                }
            }
        }
        float zoomPercent;

        /// <summary>
        /// Gets or sets the color of the page.
        /// </summary>
        [Description("The background color of the page."), Category("Window Properties")]
        public Color PageColor
        {
            get { return this.pageColor; }
            set
            {
                if (value != this.pageColor)
                {
                    this.pageColor = value;
                    Invalidate();
                }
            }
        }
        Color pageColor = Color.GhostWhite;

        public bool CanvasIsCached
        {
            get { return canvasIsCached; }
        }

        public Cursor CurrentCursor
        {
            get { return this.Cursor; }//MAG currentCommandCursor; }
            set
            {
                ;//MAG this.Cursor = value;
                //MAG currentCommandCursor = value;
            }
        }

        public PointF[] GripPoints
        {
            get { return gripPoints; }
        }

        /// <summary>
        /// Gets or sets the color of the desktop.
        /// </summary>
        [Description("The color of the desktop."), Category("Window Properties")]
        public Color DesktopColor
        {
            get { return this.desktopColor; }
            set
            {
                if (value != this.desktopColor)
                {
                    this.desktopColor = value;
                    Invalidate();
                }
            }
        }
        internal Color desktopColor = SystemColors.ControlDark;

        /// <summary>
        /// Gets or sets a value indicating whether the page is visible.
        /// </summary>
        [DefaultValue(true), Description("Determines whether the page visible."), Category("Window Properties")]
        public bool ShowPage
        {
            get { return this.showPage; }
            set
            {
                if (value != this.showPage)
                {
                    this.showPage = value;
                    this.canvas.Invalidate();
                }
            }
        }
        internal bool showPage = true;

        /// <summary>
        /// Gets or sets the page size in point.
        /// </summary>
        [Description("Determines the size (in points) of the page."), Category("Window Properties")]
        public XSize PageSize
        {
            get { return new XSize((int)this.pageSize.Width, (int)this.pageSize.Height); }
            set
            {
                this.pageSize = new SizeF((float)value.Width, (float)value.Height);
            }
        }

        public bool IsDragging
        {
            get { return isDragging; }
            set
            {
                isDragging = value;
            }
        }

        public int SelectedGrip
        {
            get { return selectedGrip; }
        }

        /// <summary>
        /// Gets or sets the page orientation. Can be Landscape or Portrait
        /// </summary>
        [Description("Determines or sets the orientation of the page."), Category("Window Properties")]
        public PageOrientation Orientation
        {
            get
            {
                if (this.pageSize.Width > this.pageSize.Height)
                    return PageOrientation.Landscape;
                else
                    return PageOrientation.Portrait;
            }
            set
            {
                if (this.pageSize.Width > this.pageSize.Height)
                {
                    // Now in Landscape
                    if (value == PageOrientation.Portrait)
                        // But want to set to Portrait, so we exchange width and height
                        this.pageSize = new SizeF((float)this.pageSize.Height, (float)this.pageSize.Width);
                }
                else
                {
                    // Now in Portrait
                    if (value == PageOrientation.Landscape)
                        // But want to set to Landscape, so we exchange width and height
                        this.pageSize = new SizeF((float)this.pageSize.Height, (float)this.pageSize.Width);
                }
            }
        }

        /// <summary>
        /// Sets a delagate that is invoked when the window wants to be painted.
        /// </summary>
        public void SetRenderEvent(RenderEvent renderEvent)
        {
            this.renderEvent = renderEvent;
            Invalidate();
        }
        RenderEvent renderEvent;

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PageWindow
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Name = "PageWindow";
            this.Size = new System.Drawing.Size(500, 400);
            this.Load += new System.EventHandler(this.PageWindow_Load);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Raises the ZoomChanged event when the zoom factor changed.
        /// </summary>
        protected virtual void OnZoomChanged(EventArgs e)
        {
            if (ZoomChanged != null)
                ZoomChanged(this, e);
        }

        /// <summary>
        /// Occurs when the zoom factor changed.
        /// </summary>
        public event EventHandler ZoomChanged;

        /// <summary>
        /// Paints the background with the sheet of paper.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;

            RenderBackground(gfx);
        }

        private void RenderBackground(Graphics gfx)
        {
            Rectangle clientRect = ClientRectangle;

            switch (this.borderStyle)
            {
                case BorderStyle.FixedSingle:
                    gfx.DrawRectangle(SystemPens.WindowFrame, clientRect.X, clientRect.Y, clientRect.Width - 1, clientRect.Height - 1);
                    //d = 1;
                    break;

                case BorderStyle.Fixed3D:
                    ControlPaint.DrawBorder3D(gfx, clientRect, Border3DStyle.Sunken);
                    //d = 2;
                    break;
            }
        }

        /// <summary>
        /// Recalculates the minimum zoom if the size of the window has changed.
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            CalculateMinZoom();
            CalculateWindowDimension();

            if (ZoomPercent <= minZoom)
            {
                this.Zoom = enums.Zoom.FullPage;
                CalculateWindowDimension();
                this.canvas.Invalidate();
            }
            else
                ReCenter(false);
        }

        /// <summary>
        /// Invalidates the canvas.
        /// </summary>
        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            ClearCache();
            base.OnInvalidated(e);

            //this.canvas.Invalidate();
        }

        /// <summary>
        /// Layouts the child controls.
        /// </summary>
        protected override void OnLayout(LayoutEventArgs levent)
        {
            LayoutChildren();
        }

        void LayoutChildren()
        {
            Invalidate();
            Rectangle clientRect = ClientRectangle;
            switch (this.borderStyle)
            {
                case BorderStyle.FixedSingle:
                    clientRect.Inflate(-1, -1);
                    break;

                case BorderStyle.Fixed3D:
                    clientRect.Inflate(-2, -2);
                    break;
            }

            int x = clientRect.X;
            int y = clientRect.Y;
            int cx = clientRect.Width;
            int cy = clientRect.Height;

            if (this.canvas != null)
            {
                this.canvas.Location = new Point(x, y);
                this.canvas.Size = new Size(cx, cy);
            }
        }

        /// <summary>
        /// Calculates the minimum zoom, which is the largest zoom where the page fits entirely in the window,
        /// since no smaller zoom has any purpose.
        /// </summary>
        internal void CalculateMinZoom()
        {
            float currentMinZoom = minZoom;
            Rectangle rcCanvas = this.canvas.ClientRectangle;

            float zoomX = (7200f * (rcCanvas.Width - horzBorders) / (this.pageSize.Width * xdpiScreen));
            float zoomY = (7200f * (rcCanvas.Height - vertBorders) / (this.pageSize.Height * ydpiScreen));

            minZoom = Math.Min(zoomX, zoomY);
        }
        float minZoom;

        /// <summary>
        /// Calculates all values for drawing the page preview.
        /// </summary>
        internal void CalculateWindowDimension(out bool zoomChanged)
        {
            // UserInfo may change display resolution while running
            Graphics gfx = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr hdc = gfx.GetHdc();
            DeviceInfos devInfo = DeviceInfos.GetInfos(hdc);
            gfx.ReleaseHdc(hdc);
            gfx.Dispose();
            xdpiScreen = devInfo.LogicalDpiX;
            ydpiScreen = devInfo.LogicalDpiY;

            Rectangle rcCanvas = this.canvas.ClientRectangle;

            Zoom zoomOld = this.zoom;
            float zoomPercentOld = this.zoomPercent;

            if (!panningMode)
            {
                // Calculate new zoom factor.
                switch (this.zoom)
                {
                    case Zoom.FitWidth:
                        //this.zoomPercent = Convert.ToInt32(25400.0 * (rcCanvas.Width - (leftBorder + rightBorder)) / (this.size.Width * xdpiScreen));
                        this.zoomPercent = (7200f * (rcCanvas.Width - horzBorders) / (this.pageSize.Width * xdpiScreen));
                        CalculateWidthHeight();
                        this.virtualPage.X = leftBorder + (rcCanvas.Width - horzBorders - virtualPage.Width) / 2;
                        this.virtualPage.Y = topBorder;
                        break;

                    case Zoom.FullPage:
                        {
                            this.zoomPercent = minZoom;
                            CalculateWidthHeight();
                            this.virtualPage.X = leftBorder + (rcCanvas.Width - horzBorders - virtualPage.Width) / 2;
                            this.virtualPage.Y = topBorder + (rcCanvas.Height - vertBorders - virtualPage.Height) / 2;
                        }
                        break;

                    case Zoom.OriginalSize:
                        this.zoomPercent = (int)(0.5 + 200f / (devInfo.ScaleX + devInfo.ScaleY));
                        this.zoomPercent = (int)(0.5 + 100f / devInfo.ScaleX);
                        break;

                    default:
                        break;
                }
            }

            // Bound to zoom limits
            this.zoomPercent = Math.Max(Math.Min(this.zoomPercent, (float)Zoom.Maximum), (float)Zoom.Mininum);
            if ((int)this.zoom > 0)
                this.zoom = (Zoom)this.zoomPercent;

            CalculateWidthHeight();


            virtualCanvas.Width = rcCanvas.Width;
            virtualCanvas.Height = rcCanvas.Height;

            zoomChanged = zoomOld != this.zoom || zoomPercentOld != this.zoomPercent;
            if (zoomChanged)
            {
                OnZoomChanged(new EventArgs());
            }
        }

        internal void CalculateWidthHeight()
        {
            this.virtualPage.Width = (int)(this.pageSize.Width * xdpiScreen * this.zoomPercent / 7200f);
            this.virtualPage.Height = (int)(this.pageSize.Height * ydpiScreen * this.zoomPercent / 7200f);

            cacheRenew = true;
            ClearCache();
        }

        internal void CalculateWindowDimension()
        {
            bool zoomChanged;
            CalculateWindowDimension(out zoomChanged);

            if (parentForm != null)
                parentForm.UpdateStatusBar();
        }

        internal bool RenderPage(Graphics gfx)
        {
            gfx.SetClip(new Rectangle(this.virtualPage.X + 1, this.virtualPage.Y + 1, this.virtualPage.Width - 1, this.virtualPage.Height - 1));

            float magnificationX = virtualPage.Width / this.pageSize.Width;
            float magnificationY = virtualPage.Height / this.pageSize.Height;


#if DRAW_BMP
      Matrix matrix = new Matrix();
      matrix.Translate(virtualPage.X, virtualPage.Y);
      matrix.Translate(-this.posOffset.X, -this.posOffset.Y);
      //matrix.Scale(scaleX, scaleY);
      gfx.Transform = matrix;

#if DRAW_X
      gfx.DrawLine(Pens.Red, 0, 0, pageSize.Width, pageSize.Height);
      gfx.DrawLine(Pens.Red, 0, pageSize.Height, pageSize.Width, 0);
#endif
      if (this.renderEvent != null)
      {
        Bitmap bmp = new Bitmap(this.virtualPage.Width, this.virtualPage.Height, gfx);
        Graphics gfx2 = Graphics.FromImage(bmp);
        gfx2.Clear(this.pageColor);
        gfx2.ScaleTransform(scaleX, scaleY);
        gfx2.SmoothingMode = SmoothingMode.HighQuality;
        XGraphics xgfx = XGraphics.FromGraphics(gfx2, new XSize(this.pageSize.Width, this.pageSize.Height));
        try
        {
          this.renderEvent(xgfx);
          gfx.DrawImage(bmp, 0, 0);
        }
        finally
        {
          bmp.Dispose();
        }
      }
#else
            Matrix matrix = new Matrix();
            matrix.Translate(virtualPage.X, virtualPage.Y);
            //            matrix.Translate(-this.posOffset.X, -this.posOffset.Y);
            matrix.Scale(magnificationX, magnificationY);
            gfx.Transform = matrix;

#if DRAW_X
      gfx.DrawLine(Pens.Red, 0, 0, pageSize.Width, pageSize.Height);
      gfx.DrawLine(Pens.Red, 0, pageSize.Height, pageSize.Width, 0);
#endif

            if (this.renderEvent != null)
            {
                gfx.SmoothingMode = SmoothingMode.HighQuality;
                //gfx.InterpolationMode = InterpolationMode.Bilinear;
                gfx.TextRenderingHint = TextRenderingHint.AntiAlias;
                gfx.CompositingQuality = CompositingQuality.HighQuality;
                gfx.CompositingMode = CompositingMode.SourceOver;

                XGraphics xgfx = XGraphics.FromGraphics(gfx, new XSize(this.pageSize.Width, this.pageSize.Height));
                this.renderEvent(xgfx);
                gfx.Save();
            }
#endif

            return true;
        }

        /// <summary>
        /// Paints the background and the empty page.
        /// </summary>
        internal void PaintBackground(Graphics gfx)
        {
            // Draw sharp paper borders and shadow.
            gfx.SmoothingMode = SmoothingMode.None;
            gfx.CompositingMode = CompositingMode.SourceOver;

            gfx.TranslateTransform(-this.posOffset.X, -this.posOffset.Y);

            // Draw outer area. Use clipping to prevent flickering of page interior.
            gfx.SetClip(new Rectangle(virtualPage.X, virtualPage.Y, virtualPage.Width + 3, virtualPage.Height + 3), CombineMode.Exclude);
            gfx.SetClip(new Rectangle(virtualPage.X + virtualPage.Width + 1, virtualPage.Y, 2, 2), CombineMode.Union);
            gfx.SetClip(new Rectangle(virtualPage.X, virtualPage.Y + virtualPage.Height + 1, 2, 2), CombineMode.Union);
            gfx.Clear(this.desktopColor);

#if DRAW_X
      gfx.DrawLine(Pens.Blue, 0, 0, virtualCanvas.Width, virtualCanvas.Height);
      gfx.DrawLine(Pens.Blue, virtualCanvas.Width, 0, 0, virtualCanvas.Height);
#endif
            gfx.ResetClip();

#if !DRAW_BMP
            // Fill page interior.
            SolidBrush brushPaper = new SolidBrush(this.pageColor);
            gfx.FillRectangle(brushPaper, virtualPage.X + 1, virtualPage.Y + 1, virtualPage.Width - 1, virtualPage.Height - 1);
#endif


            // Draw page border and shadow.
            Pen penPaperBorder = SystemPens.WindowText;
            //Brush brushShadow = SystemBrushes.ControlDarkDark;
            //Kiet Nguyen: Sheet100 remove shadow
            Brush brushShadow = new System.Drawing.SolidBrush(System.Drawing.Color.White);
            //  Brush brushShadow = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

            gfx.DrawRectangle(penPaperBorder, virtualPage);
            gfx.FillRectangle(brushShadow, virtualPage.X + virtualPage.Width + 1, virtualPage.Y + 2, 2, virtualPage.Height + 1);
            gfx.FillRectangle(brushShadow, virtualPage.X + 2, virtualPage.Y + virtualPage.Height + 1, virtualPage.Width + 1, 2);

            gfx.SmoothingMode = SmoothingMode.HighQuality;
            //gfx.InterpolationMode = InterpolationMode.Bilinear;
            gfx.CompositingQuality = CompositingQuality.HighQuality;
            gfx.CompositingMode = CompositingMode.SourceOver;
            //gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            gfx.Save();
        }

        /// <summary>
        /// Check clipping rectangle calculations.
        /// </summary>
        [Conditional("DEBUG")]
        void DrawDash(Graphics gfx, Rectangle rect)
        {
            Pen pen = new Pen(Color.GreenYellow, 1);
            pen.DashStyle = DashStyle.Dash;
            gfx.DrawRectangle(pen, rect);
        }

        /// <summary>
        /// Upper left corner of scroll area.
        /// </summary>
        Point posOffset;

        /// <summary>
        /// Real page size in point.
        /// </summary>
        SizeF pageSize = PageSizeConverter.ToSize(PdfSharp.PageSize.A4).ToSizeF();

        /// <summary>
        /// Page in pixel relative to virtual canvas.
        /// </summary>
        Rectangle virtualPage;

        public Rectangle GetVirtualPage()
        {
            return virtualPage;
        }

        /// <summary>
        /// The size in pixel of an area that completely contains the virtual page and at leat a small 
        /// border around it. If this area is larger than the canvas window, it is scrolled.
        /// </summary>
        Size virtualCanvas;

        /// <summary>
        /// Printable area in point.
        /// </summary>
        readonly RectangleF printableArea;

        private void PageWindow_Load(object sender, EventArgs e)
        {
            // We keep the instance of the DrawingForm we are in, for later use.
            parentForm = (DrawingForm)this.TopLevelControl;
        }
    }
}