using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

// PDFSharp
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

// FloorPlanning
using FloorPlanning.Display;
using System.Drawing.Imaging;
using FloorPlanning.Models;

namespace FloorPlanning
{
    [Serializable]
    public class DrawingDoc : IDisposable
    {
        public const float mmPerPoint = 0.352778f;
        public const float mm2PerPoint = 0.1243973f;

        // This is the name of the drawing
        String name;

        DrawingType drawingType;
        ImageBox baseImage;
        private bool mustChoosePage = false;

        XSize pageSize;
        float scale;
        float referenceLength = 1;
        PageOrientation docOrientation;
        PageOrientation formOrientation;
        FloorPlanningProject ownerJob;
        bool isRotated;
        public Point IconDisplayLocation;


        [NonSerialized]
        public DrawingEntity lastEntity = null;

        //[NonSerialized]
        //Bitmap drawingCache;


        [NonSerialized]
        ImageBox tempBaseImage;

        [NonSerialized]
        Line tempLine;

        [NonSerialized]
        Polyline tempPolyline;

        [NonSerialized]
        DoorTakOut tempDoorTakOut;

        [NonSerialized]
        Polygon tempPolygon;

        // Legacy, for compatibility. To be removed eventually.
        //ArrayList redoList = null;
        //PageSize paperSize;
        //private int pageNumber;
        ArrayList entityList = null;
        SizeF pageSizeMM;
        // End legacy

        List<DrawingEntity> entities = null;
        List<DrawingEntity> selectedEs = null;
        List<DrawingOp> drawingOps = null;
        int nextDrawingOp = 0;

        LayerCollection layers;

        [NonSerialized]
        DrawingForm drawingForm;

        /// <summary>
        /// Kiet Nguyen: Sheet 600  Setting the custom scale
        /// Default is 1
        /// </summary>
        float customScale = 1;

   

        public enum DrawingType
        {
            /// <summary>
            /// Plan
            /// </summary>
            Plan = 0,
        }

        // Constructors section Start
        //
        public DrawingDoc()
        {
            name = Program.Empty_Project;
            drawingType = DrawingType.Plan;
            scale = 100f;
            pageSize = PageSizeConverter.ToSize(PageSize.A4);
            docOrientation = PageOrientation.Landscape;

            //entityList = new ArrayList();
            entities = new List<DrawingEntity>();
            drawingOps = new List<DrawingOp>();
        }

        public DrawingDoc(String drawingName, DrawingType drawingType, FloorPlanningProject job)
        {
            name = drawingName;
            this.drawingType = drawingType;
            ownerJob = job;

            // Default settings
            scale = 100f;
            pageSize.Width = PageSizeConverter.ToSize(PageSize.A4).Height;
            pageSize.Height = PageSizeConverter.ToSize(PageSize.A4).Width;
            docOrientation = PageOrientation.Landscape;

            // Drawing entities get stored here
            entities = new List<DrawingEntity>();
            drawingOps = new List<DrawingOp>();
            //entityList = new ArrayList();
            //redoList = new ArrayList();

            // Layers get stored here
            layers = new LayerCollection();
        }
        //
        // Constructors section End


        // Methods section Start
        //
        public void OpenEditor(string sBaseImagePath = null, float lastWidthCrop = 0, double l_opacity = 1, bool bAutoTitle = false, bool bAutoTitleMargins = false)
        {
            if (drawingForm != null)
            {
                if (drawingForm.WindowState == System.Windows.Forms.FormWindowState.Minimized)
                    drawingForm.WindowState = System.Windows.Forms.FormWindowState.Normal;

                drawingForm.Activate();
            }
            else
            {
                drawingForm = new DrawingForm(this);

                Update();

                drawingForm.PageWidth = pageSize.Width;
                drawingForm.PageHeight = pageSize.Height;

                drawingForm.RenderEvent = new Display.PageWindow.RenderEvent(Render);

                drawingForm.UndoEnable(nextDrawingOp > 0);
                drawingForm.RedoEnable(nextDrawingOp < drawingOps.Count);


                drawingForm.FormClosed += DrawingFormClosed;

                //drawingForm.Show(Program.mainForm);
                drawingForm.Show();
                if (sBaseImagePath != null)
                {
                    drawingForm.LoadBasePlan(sBaseImagePath, lastWidthCrop, l_opacity, bAutoTitle, bAutoTitleMargins);
                }
            }
        }


        public void SetDrawingForm(DrawingForm l_drawingForm, string sBaseImagePath = null)
        {
            drawingForm = l_drawingForm;

            Update();

            drawingForm.PageWidth = pageSize.Width;
            drawingForm.PageHeight = pageSize.Height;

            drawingForm.RenderEvent = new Display.PageWindow.RenderEvent(Render);

            drawingForm.UndoEnable(nextDrawingOp > 0);
            drawingForm.RedoEnable(nextDrawingOp < drawingOps.Count);
            drawingForm.FormClosed += DrawingFormClosed;
        }



        public void CloseEditor()
        {
            if (drawingForm != null)
            {
                drawingForm.Close();
            }
        }

        public void Dispose()
        {

        }

        public void Update()
        {
            // May need to intialize entities list if we have deserialized a legacy file
            if (entities == null)
            {
                if (entityList != null)
                    entities = new List<DrawingEntity>((DrawingEntity[])entityList.ToArray(typeof(DrawingEntity)));
                else
                    entities = new List<DrawingEntity>();

                entityList = null;
            }

            // Check if entities are valid, and if not, remove
            // We can't use foreach here because it does not allow the
            // entities list to be changed while looping
            for (int i = 0; i < entities.Count; i++)
            {
                if (!entities[i].IsValid())
                {
                    entities.RemoveAt(i);
                    i--;
                }
            }

            // May need to synthesize a drawingOp list if we have deserialized a legacy file
            if (drawingOps == null)
            {
                drawingOps = new List<DrawingOp>();
                nextDrawingOp = 0;

                // And we need to synthesize the drawing operations to match the existing entities
                foreach (DrawingEntity entity in entities)
                {
                    drawingOps.Add(new DrawingOp(DrawingOp.OpType.Add, entity));
                    nextDrawingOp++;
                }
            }

            // We also need to make sure all entities have OwnerDrawing set
            foreach (DrawingEntity entity in entities)
            {
                entity.OwnerDrawing = this;
            }
        }

        public DrawingType GetDrawingType()
        {
            return drawingType;
        }

        private void DrawingFormClosed(object sender, EventArgs e)
        {
            drawingForm.Dispose();
            drawingForm = null;

            if (baseImage != null)
                baseImage.DisposeImages();

            if (tempBaseImage != null)
                tempBaseImage.DisposeImages();

            ClearTempEntities();

            foreach (DrawingEntity entity in entities)
            {
                entity.DisposeImages();
            }
        }

        public void ClearTempEntities()
        {
            tempPolygon = null;
            tempLine = null;
            tempPolyline = null;
            tempBaseImage = null;
            tempDoorTakOut = null;
        }

        public DrawingEntity FindEntityAt(PointF pickPoint)
        {
            PointF point = new PointF();

            // We need to convert mm to pt
            point.X = pickPoint.X / mmPerPoint;
            point.Y = pickPoint.Y / mmPerPoint;
            DrawingEntity selectedE = null;
            //foreach (DrawingEntity entity in entities)
            for (int i = 0; i < entities.Count; i++)
            {
                DrawingEntity entity = entities[i];
                if (entity != baseImage)
                {
                    if (entity.IsSelectedByPoint(point))
                    {
                        selectedE = entity;
                        return selectedE;
                    }
                }
            }

            return selectedE;// null;
        }

        public List<DrawingEntity> FindEntitiesAt(RectangleF r)
        {
            // We need to convert mm to pt
            PointF initP = new PointF(r.Location.X / mmPerPoint, r.Location.Y / mmPerPoint);
            SizeF sf = new SizeF(r.Size.Width / mmPerPoint, r.Size.Height / mmPerPoint);
            RectangleF rpt = r;// new RectangleF(initP, sf);

            selectedEs = new List<DrawingEntity>();
            //foreach (DrawingEntity entity in entities)
            for (int i = 0; i < entities.Count; i++)
            {
                DrawingEntity entity = entities[i];
                try
                {
                    if (entity != baseImage)
                    {
                        if (entity.IsSelectedInRectangle(rpt))
                        {
                            selectedEs.Add(entity);
                        }
                    }
                }
                catch { }
            }

            return selectedEs;// null;
        }


        public Line AddLine(PointF pointInMM)
        {
            PointF point = new PointF();

            // We need to convert mm to pt
            point.X = pointInMM.X / mmPerPoint;
            point.Y = pointInMM.Y / mmPerPoint;

            tempLine = new Line(point);

            return tempLine;
        }

        public Polyline AddPolyline(PointF pointInMM, ComponentDef component)
        {
            PointF point = new PointF();

            // We need to convert mm to pt
            point.X = pointInMM.X / mmPerPoint;
            point.Y = pointInMM.Y / mmPerPoint;

            Layer componentLayer = layers.Add(component.DisplayName);

            tempPolyline = new Polyline(point, component, componentLayer, this);

            return tempPolyline;
        }

        public DoorTakOut AddDoorTakOut(PointF pointInMM, ComponentDef component, float l_measure)
        {
            PointF point = new PointF();

            // We need to convert mm to pt
            point.X = pointInMM.X / mmPerPoint;
            point.Y = pointInMM.Y / mmPerPoint;

            Layer componentLayer = layers.Add(component.DisplayName);

            tempDoorTakOut = new DoorTakOut(point, component, componentLayer, this, l_measure);

            return tempDoorTakOut;
        }

        public Polygon AddPolygon(PointF pointInMM, ComponentDef component)
        {
            PointF point = new PointF();

            // We need to convert mm to pt
            point.X = pointInMM.X / mmPerPoint;
            point.Y = pointInMM.Y / mmPerPoint;

            Layer componentLayer = layers.Add(component.DisplayName);

            tempPolygon = new Polygon(point, component, componentLayer, this);

            return tempPolygon;
        }

        // AddBaseImage for PDF
        public ImageBox AddBaseImage(string filePath, CompatiblePdfDocument pdfDoc)
        {
            return AddBaseImage(filePath, pdfDoc, 1);
        }

        public ImageBox AddBaseImage(string filePath, CompatiblePdfDocument pdfDoc, int pageNumber)
        {
            // Make sure we have the base drawing layer
            Layer baseDrawingLayer = layers.Add("Base Drawing");

            //SQUISH
            if (drawingForm != null)
            {
                drawingForm.PageWidth = pdfDoc.Pages[pageNumber - 1].Width.Point;
                drawingForm.PageHeight = pdfDoc.Pages[pageNumber - 1].Height.Point;
            }
            pageSize.Width = pdfDoc.Pages[pageNumber - 1].Width.Point;
            pageSize.Height = pdfDoc.Pages[pageNumber - 1].Height.Point;
            //END ADDED

            tempBaseImage = new ImageBox(0, 0, pageSize.Width, pageSize.Height, filePath, pageNumber, baseDrawingLayer);

            tempBaseImage.PDFPageCount = pdfDoc.PageCount;
            bool bSquish = false;
            // We get some properties from and temporarily delete the current base image
            if (baseImage != null)
            {
                while (tempBaseImage.RotationAngle != baseImage.RotationAngle)
                {
                    bSquish = !bSquish;
                    tempBaseImage.Rotate90();
                }

                baseImage.Height = (bSquish) ? tempBaseImage.Width : tempBaseImage.Height;
                baseImage.Width = (bSquish) ? tempBaseImage.Height : tempBaseImage.Width;

                //tempBaseImage.Width = baseImage.Width;
                //tempBaseImage.Height = baseImage.Height;                

                // We need to match the drawing after any rotation 
                MatchDrawingToTempBaseImage();
                DeleteEntity(baseImage, false);
            }

            tempBaseImage.PreviewMode = true;
            tempBaseImage.IsBeingEdited = true;

            return tempBaseImage;
        }

        // AddBaseImage for raster files
        public ImageBox AddBaseImage(string filePath)
        {
            float dpiRatio;
            double imageWidth, imageHeight;

            // Get JPEG or PNG info
            using (Image rasterImage = Image.FromFile(filePath))
            {
                dpiRatio = 72.0f / rasterImage.HorizontalResolution;
                imageWidth = rasterImage.PhysicalDimension.Width * dpiRatio;
                imageHeight = rasterImage.PhysicalDimension.Height * dpiRatio;
            }

            Layer baseDrawingLayer = layers.Add("Base Drawing");

            tempBaseImage = new ImageBox(0, 0, imageWidth, imageHeight, filePath, 0, baseDrawingLayer);

            // We get some properties from and temporarily delete the current base image
            if (baseImage != null)
            {
                while (tempBaseImage.RotationAngle != baseImage.RotationAngle)
                {
                    tempBaseImage.Rotate90();
                }

                DeleteEntity(baseImage, true);
            }

            MatchDrawingToTempBaseImage();

            return tempBaseImage;
        }

        public void MatchDrawingToTempBaseImage()
        {
            drawingForm.PageWidth = tempBaseImage.Width;
            drawingForm.PageHeight = tempBaseImage.Height;

            pageSize.Width = drawingForm.PageWidth;
            pageSize.Height = drawingForm.PageHeight;

            //pageSizeMM.Width = (float)pageSize.Width * mmPerPoint;
            //pageSizeMM.Height = (float)pageSize.Height * mmPerPoint;

            if (pageSize.Width > pageSize.Height)
            {
                docOrientation = PageOrientation.Landscape;
            }
            else
            {
                docOrientation = PageOrientation.Portrait;
            }
        }

        public void PrepareDrawingForPDFPage(CompatiblePdfDocument pdfDoc, int pageNumber)
        {
            PrepareDrawingForPDFPage(pdfDoc, pageNumber, 0);
        }

        public void PrepareDrawingForPDFPage(CompatiblePdfDocument pdfDoc, int pageNumber, int rotationAngle)
        {
            // Create the temporary image box for page number
            CompatiblePdfPage page = pdfDoc.Pages[pageNumber - 1];

            if (drawingForm == null)
            {
                //BUG
                if (page.Width > page.Height)
                {
                    docOrientation = PageOrientation.Landscape;
                }
                else
                {
                    docOrientation = PageOrientation.Portrait;
                }
                //END
                // No drawing form, get out of here.
                return;
            }
            drawingForm.PageWidth = page.Width.Point;
            drawingForm.PageHeight = page.Height.Point;
            pageSize.Width = page.Width.Point;
            pageSize.Height = page.Height.Point;

            //pageSizeMM.Width = (float)page.Width.Millimeter;
            //pageSizeMM.Height = (float)page.Height.Millimeter;


            if (tempBaseImage != null)
            {
                tempBaseImage.Width = pageSize.Width;
                tempBaseImage.Height = pageSize.Height;
            }

            if (drawingForm.PageWidth > drawingForm.PageHeight)
            {
                docOrientation = PageOrientation.Landscape;
            }
            else
            {
                docOrientation = PageOrientation.Portrait;
            }
        }

        public static Image loadImage(string filePath)
        {
            return loadImage(filePath, 0, 0, false);
        }

        public static Image loadImage(string filePath, int pageNumber, int rotationAngle, bool isPreview, PointF offset = new PointF())
        {
            if (Path.GetExtension(filePath).ToLowerInvariant() == ".pdf")
            {
                // Get PDF image by executing mudraw.exe
                string tempPath = System.IO.Path.GetTempPath();
                string dpi;

                if (isPreview)
                    dpi = "75";
                else
                    dpi = "150";

                //Kiet Nguyen: Convert Pdf to Image using  a third party library lib: https://github.com/chen0040/cs-pdf-to-image
                if (File.Exists(filePath))
                {
                    Process muProcess = new Process();

                    muProcess.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdf2png.exe");
                    muProcess.StartInfo.Arguments = "-g -o \"" + tempPath + Program.sProgramName + "Temp.png\" -r " + dpi + " -R " + rotationAngle.ToString() + " \"" + filePath + "\" " + pageNumber.ToString();
                    muProcess.StartInfo.CreateNoWindow = true;
                    muProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    muProcess.Start();
                    muProcess.WaitForExit();
                    muProcess.Close();
                    muProcess.Dispose();

                    string tempImagePath = Path.Combine(tempPath, Program.sProgramName + "Temp.png");

                    Image returnImage;

                    using (var fs = new FileStream(tempImagePath, FileMode.Open, FileAccess.Read))
                    {
                        using (Bitmap tempImage = new Bitmap(tempImagePath))
                        {
                            returnImage = new Bitmap(tempImage);
                        }
                    }

                    File.Delete(tempImagePath);

                    return returnImage;
                }
                else
                {
                    Image image = null;
                    return image;
                }
            }
            else if (Path.GetExtension(filePath).ToLowerInvariant() == ".jpg" ||
                     Path.GetExtension(filePath).ToLowerInvariant() == ".png")
            {
                if (!File.Exists(filePath))
                    if (File.Exists(Path.GetFileName(filePath)))
                        filePath = Path.GetFileName(filePath);

                if (File.Exists(filePath))
                {
                    Image img;
                    img = Image.FromStream(new MemoryStream(File.ReadAllBytes(filePath), false));
                    bool bEnums = true;
                    RotateFlipType rotate;

                    switch (rotationAngle)
                    {
                        case 0:
                            rotate = RotateFlipType.RotateNoneFlipNone;
                            break;

                        case 90:
                            rotate = RotateFlipType.Rotate90FlipNone;
                            break;

                        case 180:
                            rotate = RotateFlipType.Rotate180FlipNone;
                            break;

                        case 270:
                            rotate = RotateFlipType.Rotate270FlipNone;
                            break;

                        default:
                            rotate = RotateFlipType.RotateNoneFlipNone;
                            break;
                    }

                    if (bEnums)
                    {
                        img.RotateFlip(rotate);
                    }

                    return img;
                }
                else
                {
                    Image image = null;
                    return image;
                }
            }
            else
            {
                Image image = null;

                return image;
            }
        }

        public static Image loadThumbImage(string filePath)
        {
            if (Path.GetExtension(filePath).ToLowerInvariant() == ".pdf")
            {
                // Get PDF image by executing mudraw.exe
                string tempPath = System.IO.Path.GetTempPath();
                string dpi;

                dpi = "75";

                if (File.Exists(filePath))
                {
                    Process muProcess = new Process();

                    muProcess.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdf2png.exe");
                    muProcess.StartInfo.Arguments = "-g -o \"" + tempPath + Program.sProgramName + "TempThumb.png\" -r " + dpi + " -R 0 \"" + filePath + "\" 0";
                    muProcess.StartInfo.CreateNoWindow = true;
                    muProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    muProcess.Start();
                    muProcess.WaitForExit();
                    muProcess.Close();
                    muProcess.Dispose();

                    string tempImagePath = Path.Combine(tempPath, Program.sProgramName + "TempThumb.png");

                    Image returnImage;

                    using (var fs = new FileStream(tempImagePath, FileMode.Open, FileAccess.Read))
                    {
                        using (Bitmap tempImage = new Bitmap(tempImagePath))
                        {
                            returnImage = new Bitmap(tempImage);
                        }
                    }

                    File.Delete(tempImagePath);

                    return returnImage;
                }
                else
                {
                    Image image = null;
                    return image;
                }
            }
            else
            {
                Image image = null;

                return image;
            }
        }

        public static Image loadImage(Image in_img, int pageNumber, int rotationAngle, bool isPreview, PointF offset = new PointF(), bool bNewFormat = false)
        {
            using (MemoryStream m = new MemoryStream())
            {
                Image img = null;
                try
                {
                    if (bNewFormat)
                    {
                        in_img.Save(m, ImageFormat.Png);// in_img.RawFormat);                    
                    }
                    else
                    {
                        in_img.Save(m, in_img.RawFormat);
                    }
                    img = Image.FromStream(new MemoryStream(m.ToArray(), false));
                    bool bEnums = true;
                    RotateFlipType rotate;

                    switch (rotationAngle)
                    {
                        case 0:
                            rotate = RotateFlipType.RotateNoneFlipNone;
                            break;

                        case 90:
                            rotate = RotateFlipType.Rotate90FlipNone;
                            break;

                        case 180:
                            rotate = RotateFlipType.Rotate180FlipNone;
                            break;

                        case 270:
                            rotate = RotateFlipType.Rotate270FlipNone;
                            break;

                        default:
                            rotate = RotateFlipType.RotateNoneFlipNone;
                            break;
                    }

                    if (bEnums)
                    {
                        img.RotateFlip(rotate);
                    }
                }
                catch { }

                return img;
            }
        }

        public DrawingEntity AcceptEntity()
        {
            lastEntity = AcceptInternalEntity();
            return lastEntity;
        }

        public DrawingEntity LastEntity
        {
            get
            {
                return lastEntity;
            }
        }

        public DrawingEntity LastOpEntity
        {
            get
            {
                try
                {
                    if (nextDrawingOp > 0)
                    {
                        DrawingOp lastOp = drawingOps[nextDrawingOp - 1];

                        switch (lastOp.Type)
                        {
                            case DrawingOp.OpType.Add:
                                return lastOp.Entity;
                                break;
                        }
                    }
                }
                catch { }
                return null;
            }
        }

        private DrawingEntity AcceptInternalEntity()
        {
            DrawingEntity newEntity;

            if (tempLine != null)
            {
                newEntity = AddEntity(tempLine);
                tempLine = null;
                return newEntity;
            }

            if (tempPolyline != null)
            {
                newEntity = AddEntity(tempPolyline);
                tempPolyline = null;
                return newEntity;
            }

            if (tempDoorTakOut != null)
            {
                newEntity = AddEntity(tempDoorTakOut);
                tempDoorTakOut = null;
                return newEntity;
            }

            if (tempPolygon != null)
            {
                newEntity = AddEntity(tempPolygon);
                tempPolygon = null;
                return newEntity;
            }

            if (tempBaseImage != null)
            {
                baseImage = tempBaseImage;
                baseImage.PreviewMode = false;
                newEntity = AddEntity(baseImage, true);
                tempBaseImage = null;
                return newEntity;
            }
            return null;
        }

        public float GetEntityQuantity(DrawingEntity entity)
        {
            switch (entity.Type)
            {
                case DrawingEntity.EntityType.Polyline:
                    return entity.Perimeter * mmPerPoint * scale * 0.001f;

                case DrawingEntity.EntityType.Polygon:
                    return entity.Area * mm2PerPoint * scale * scale * 0.000001f;


                default:
                    return 0;
            }
        }

        public float GetEntityArea(DrawingEntity entity)
        {
            //Kiet Nguyen: custom scale by Sheet 600
            //  return entity.Area * mm2PerPoint * scale * scale * 0.000001f;
            //Kiet Nguyen: need to multiply with customScale to get feet value
            return entity.Area * customScale;
        }
        public float GetEntityPerimeter(DrawingEntity entity)
        { //Kiet Nguyen: custom scale by Sheet 600
          //   return entity.Perimeter * mmPerPoint * scale * 0.001f;
          //Kiet Nguyen: need to multiply with customScale to get feet value
            return entity.Perimeter *  customScale;
        }

        public float GetComponentQuantity(ComponentDef component)
        {
            float quantitySum = 0;

            // Look through entityList
            foreach (DrawingEntity entity in entities)
            {
                // We will only sum entities of the specified component
                if (entity.Component != null)
                    if (entity.Component.Key == component.Key)
                    {
                        switch (entity.Type)
                        {
                            case DrawingEntity.EntityType.Polyline:
                                quantitySum += entity.Perimeter * mmPerPoint * scale * 0.001f;
                                break;

                            case DrawingEntity.EntityType.Polygon:
                                quantitySum += entity.Area * mm2PerPoint * scale * scale * 0.000001f;
                                break;
                        }
                    }
            }

            return quantitySum;
        }

        public int GetComponentCornerCount(ComponentDef component)
        {
            int cornerCount = 0;

            // Look through entityList
            foreach (DrawingEntity entity in entities)
            {
                // We will only sum entities of the specified component
                if (entity.Component != null)
                    if (entity.Component.Key == component.Key)
                    {
                        switch (entity.Type)
                        {
                            case DrawingEntity.EntityType.Polyline:
                                cornerCount += entity.CornerCount;
                                break;
                        }
                    }
            }

            return cornerCount;
        }

        public int GetEntityCornerCount(DrawingEntity entity)
        {
            switch (entity.Type)
            {
                case DrawingEntity.EntityType.Polyline:
                    return entity.CornerCount;

                default:
                    return 0;
            }
        }

        // Used for deletion
        public void DeleteEntity(DrawingEntity selectedEntity)
        {
            DeleteEntity(selectedEntity, true);
        }

        // If doSave is false, this is a temporary delete for editing
        public void DeleteEntity(DrawingEntity selectedEntity, bool doSave)
        {
            entities.Remove(selectedEntity);

            if (doSave)
            {
                if (nextDrawingOp < drawingOps.Count)
                    drawingOps.RemoveRange(nextDrawingOp, drawingOps.Count - nextDrawingOp);

                drawingOps.Add(new DrawingOp(DrawingOp.OpType.Delete, selectedEntity));
            }
            else
            {
                if (selectedEntity != BaseImage)
                    // This is just a regular entity
                    drawingOps.Insert(nextDrawingOp, new DrawingOp(DrawingOp.OpType.TempDelete, selectedEntity));
                else
                    // This is the base plan
                    drawingOps.Insert(nextDrawingOp, new DrawingOp(DrawingOp.OpType.TempDeleteBasePlan, selectedEntity));
            }

            nextDrawingOp++;

            drawingForm.UndoEnable(true);
            drawingForm.RedoEnable(false);

            if (doSave)
                ownerJob.Save();
        }

        public DrawingEntity AddEntity(DrawingEntity entity)
        {
            return AddEntity(entity, false);
        }

        public DrawingEntity AddEntity(DrawingEntity entity, bool atStart)
        {
            entity.HasChanged = false;
            entity.IsBeingEdited = false;

            if (atStart)
                entities.Insert(0, entity);
            else
                entities.Add(entity);

            int lastDrawingOp = nextDrawingOp - 1;


            if (nextDrawingOp > 0 &&
                   (drawingOps[lastDrawingOp].Type == DrawingOp.OpType.TempDelete ||
                    drawingOps[lastDrawingOp].Type == DrawingOp.OpType.TempDeleteBasePlan))
            {
                // Save the deleted entity
                DrawingEntity oldEntity = drawingOps[lastDrawingOp].Entity;

                // Remove TempDelete op and any other ops that are after it
                foreach (DrawingOp op in drawingOps.GetRange(lastDrawingOp, drawingOps.Count - lastDrawingOp))
                    op.Dereference();

                drawingOps.RemoveRange(lastDrawingOp, drawingOps.Count - lastDrawingOp);

                // Is it a base plan image?
                if (atStart && entity.Type == DrawingEntity.EntityType.ImageBox)
                    // Yes - We add a ReplaceBasePlan op with isBaseImage as true
                    drawingOps.Add(new DrawingOp(DrawingOp.OpType.ReplaceBasePlan, entity, oldEntity, true));
                else
                    // No - We add a Replace op
                    drawingOps.Add(new DrawingOp(DrawingOp.OpType.Replace, entity, oldEntity));
            }
            else
            {
                if (nextDrawingOp < drawingOps.Count)
                {
                    foreach (DrawingOp op in drawingOps.GetRange(nextDrawingOp, drawingOps.Count - nextDrawingOp))
                        op.Dereference();

                    drawingOps.RemoveRange(nextDrawingOp, drawingOps.Count - nextDrawingOp);
                }


                // Is it a base plan image?
                if (atStart && entity.Type == DrawingEntity.EntityType.ImageBox)
                    // Yes - We add a AddBasePlan op with isBaseImage as true
                    drawingOps.Add(new DrawingOp(DrawingOp.OpType.AddBasePlan, entity, true));
                else
                    // No - We add an Add op
                    drawingOps.Add(new DrawingOp(DrawingOp.OpType.Add, entity));

                nextDrawingOp++;
            }

            if (drawingForm != null)
            {
                drawingForm.UndoEnable(true);
                drawingForm.RedoEnable(false);

                ownerJob.Save();
            }

            return entity;
        }

        public void Undo()
        {
            Undo(true);
        }

        public void Undo(bool enableRedo)
        {
            if (nextDrawingOp > 0)
            {
                DrawingOp lastOp = drawingOps[nextDrawingOp - 1];

                switch (lastOp.Type)
                {
                    case DrawingOp.OpType.Add:
                        entities.Remove(lastOp.Entity);
                        lastOp.Entity.DisposeImages();
                        break;

                    case DrawingOp.OpType.AddBasePlan:
                        entities.Remove(lastOp.Entity);
                        lastOp.Entity.DisposeImages();
                        baseImage = null;
                        break;

                    case DrawingOp.OpType.TempDelete:
                        entities.Add(lastOp.Entity);
                        drawingOps.RemoveAt(nextDrawingOp - 1);
                        break;

                    case DrawingOp.OpType.TempDeleteBasePlan:
                        tempBaseImage = (ImageBox)lastOp.Entity;
                        entities.Insert(0, tempBaseImage);
                        baseImage = tempBaseImage;
                        MatchDrawingToTempBaseImage();
                        tempBaseImage = null;

                        drawingOps.RemoveAt(nextDrawingOp - 1);
                        break;

                    case DrawingOp.OpType.Delete:
                        entities.Add(lastOp.Entity);
                        break;

                    case DrawingOp.OpType.Replace:
                        entities.Remove(lastOp.Entity);
                        lastOp.Entity.DisposeImages();

                        entities.Add(lastOp.OldEntity);
                        break;

                    case DrawingOp.OpType.ReplaceBasePlan:
                        entities.Remove(lastOp.Entity);
                        lastOp.Entity.DisposeImages();

                        tempBaseImage = (ImageBox)lastOp.OldEntity;
                        entities.Insert(0, tempBaseImage);
                        baseImage = tempBaseImage;

                        MatchDrawingToTempBaseImage();
                        tempBaseImage = null;
                        break;
                }

                nextDrawingOp--;

                drawingForm.UndoEnable(nextDrawingOp > 0);

                if (enableRedo)
                    // This was a user Undo, so we save
                    ownerJob.Save();
                else
                    // This was a cancelled edit, so we don't need to save
                    // And we simply remove the Op.
                    drawingOps.Remove(lastOp);

                drawingForm.RedoEnable(nextDrawingOp < drawingOps.Count);
            }
        }

        public void Redo()
        {
            if (nextDrawingOp < drawingOps.Count)
            {
                DrawingOp lastOp = drawingOps[nextDrawingOp];

                switch (lastOp.Type)
                {
                    case DrawingOp.OpType.Add:
                        entities.Add(lastOp.Entity);
                        break;

                    case DrawingOp.OpType.AddBasePlan:
                        entities.Insert(0, lastOp.Entity);
                        tempBaseImage = (ImageBox)lastOp.Entity;
                        MatchDrawingToTempBaseImage();
                        baseImage = tempBaseImage;
                        tempBaseImage = null;
                        break;

                    case DrawingOp.OpType.Delete:
                        entities.Remove(lastOp.Entity);
                        lastOp.Entity.DisposeImages();
                        break;

                    case DrawingOp.OpType.TempDelete:
                    case DrawingOp.OpType.TempDeleteBasePlan:
                        break;

                    case DrawingOp.OpType.Replace:
                        entities.Remove(lastOp.OldEntity);
                        lastOp.OldEntity.DisposeImages();

                        entities.Add(lastOp.Entity);
                        break;

                    case DrawingOp.OpType.ReplaceBasePlan:
                        entities.Remove(lastOp.OldEntity);
                        lastOp.OldEntity.DisposeImages();

                        tempBaseImage = (ImageBox)lastOp.Entity;
                        entities.Insert(0, tempBaseImage);

                        MatchDrawingToTempBaseImage();
                        baseImage = tempBaseImage;
                        tempBaseImage = null;
                        break;
                }

                nextDrawingOp++;
                ownerJob.Save();

                drawingForm.UndoEnable(true);
                drawingForm.RedoEnable(nextDrawingOp < drawingOps.Count);
            }
        }

        //public void ChangeOrientation()
        //{
        //    if (Orientation == PageOrientation.Landscape)
        //        docOrientation = PageOrientation.Portrait;
        //    else
        //        docOrientation = PageOrientation.Landscape;
        //}
        //
        // Methods section End


        // Properties section Start
        //
        /// <summary>
        /// Gets or sets name of the drawing
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                ownerJob.Save();
                if (drawingForm != null)
                    drawingForm.SetTitleBar();
            }
        }

        public XSize Size
        {
            get { return pageSize; }
        }

        public bool MustChoosePage
        {
            get { return mustChoosePage; }
        }

        public ImageBox BaseImage
        {
            get { return baseImage; }
        }

        public DrawingForm DrawingForm
        {
            get { return drawingForm; }
        }

        // Temporary drawing entities
        //
        public Polyline TempPolyline
        {
            get { return tempPolyline; }
            set
            {
                tempPolyline = value;
            }
        }

        public DoorTakOut TempDoorTakOut
        {
            get { return tempDoorTakOut; }
            set
            {
                tempDoorTakOut = value;
            }
        }

        public Line TempLine
        {
            get { return tempLine; }
        }

        public Polygon TempPolygon
        {
            get { return tempPolygon; }
            set
            {
                tempPolygon = value;
            }
        }

        public ImageBox TempBaseImage
        {
            get { return tempBaseImage; }
        }

        public void SetTempBaseImage(ImageBox i)
        {
            tempBaseImage = i;
        }

        // This property is true if any temporary entity exists
        public bool TempEntityDoesExist
        {
            get
            {
                return (tempPolygon != null ||
                        tempLine != null ||
                        tempPolyline != null ||
                        tempDoorTakOut != null ||
                        tempBaseImage != null);
            }
        }

        public bool TempEntitiesDoesExist
        {
            get
            {
                return (tempPolygon != null ||
                        tempLine != null ||
                        tempPolyline != null ||
                        tempDoorTakOut != null ||
                        tempBaseImage != null);
            }
        }

        /// <summary>
        /// Gets or sets the scale of the drawing. The value is equal to the denominator.
        /// For example, for scale 1:100, set scale to 100. Values less than 1 will be ignored.
        /// </summary>
        public float Scale
        {
            get { return scale; }
            set
            {
                if (value >= 1f)
                {
                    scale = value;

                    drawingForm.UpdateStatusBar();

                    ownerJob.Save();
                }
            }
        }

        /// <summary>
        /// Kiet Nguyen: Sheet 600  Setting the custom scale
        /// </summary>
        public float CustomScale
        {
            get
            {
                return customScale;
            }

            set
            {
                customScale = value;
            }
        }

        public float ReferenceLength
        {
            get { return referenceLength; }
            set
            {
                if (value > 0)
                    referenceLength = value;
            }
        }

        public PageOrientation Orientation
        {
            get { return docOrientation; }
            //set
            //{
            //    docOrientation = value;
            //    drawingForm.Orientation = docOrientation;
            //}
        }

        public PageOrientation FormOrientation
        {
            get { return formOrientation; }
            set
            {
                formOrientation = value;
            }
        }

        public LayerCollection Layers
        {
            get { return layers; }
        }

        public List<DrawingEntity> Entities
        {
            get { return entities; }
        }

        public FloorPlanningProject OwnerJob
        {
            get { return ownerJob; }
            set { ownerJob = value; }
        }

        public DrawingType DocType
        {
            get { return drawingType; }
            set
            {
                if (DocTypeCanBeChanged)
                    drawingType = value;
            }
        }

        bool DocTypeCanBeChanged
        {
            get
            {
                return entities.Count == 0 && drawingOps.Count == 0 || (entities.Count == 1 && drawingOps.Count == 1 && entities[0].Type == DrawingEntity.EntityType.ImageBox);
            }
        }

        public bool IsRotated
        {
            get { return isRotated; }
        }

      

        //
        // Properties section End


        /// <summary>
        /// Callback Renders the content of the Drawing
        /// </summary>
        public void Render(XGraphics gfx)
        {
            //drawingCache = null;

            SetTransforms(gfx);

            if (tempBaseImage != null)
                // If it exists, draw temporary base image
                tempBaseImage.Render(gfx);
            else if (baseImage != null)
                // Else, if it exists, draw base image
                baseImage.Render(gfx);


            // Draw entities except base image, if any
            if (entities.Count > 0)
            {
                int start = (baseImage != null ? 1 : 0);

                foreach (DrawingEntity entity in entities.GetRange(start, entities.Count - start))
                {
                    if (entity.Layer.Visible)
                        entity.Render(gfx);
                }
            }
        }

        /// <summary>
        /// Renders a single entity
        /// </summary>
        public void Render(XGraphics gfx, DrawingEntity entity)
        {
            SetTransforms(gfx);

            // Draw entity
            entity.Render(gfx);
        }

        void SetTransforms(XGraphics gfx)
        {
            // Figure out gfx orientation
            PageOrientation gfxOrientation;

            if (gfx.PageSize.Width > gfx.PageSize.Height)
                gfxOrientation = PageOrientation.Landscape;
            else
                gfxOrientation = PageOrientation.Portrait;

            // If orientations don't match, we need to rotate
            if (gfxOrientation != Orientation)
            {
                isRotated = true;

                XPoint centerPoint = new XPoint();
                centerPoint.X = Math.Min(pageSize.Width, pageSize.Height) / 2.0;
                centerPoint.Y = centerPoint.X;

                // Rotate
                if (gfxOrientation == PageOrientation.Landscape)
                {
                    gfx.RotateAtTransform(-90d, centerPoint);
                }
                else
                {
                    gfx.RotateAtTransform(90d, centerPoint);
                }

                // Then scale
                gfx.ScaleTransform(Math.Min(gfx.PageSize.Width / pageSize.Height, gfx.PageSize.Height / pageSize.Width), XMatrixOrder.Append);
            }
            else
            {
                isRotated = false;

                // Scale
                gfx.ScaleTransform(Math.Min(gfx.PageSize.Width / pageSize.Width, gfx.PageSize.Height / pageSize.Height), XMatrixOrder.Append);
            }
        }

        // Geometry functions, may move to separate class if too many
        //
        /// <summary>
        /// Returns the distance on an X,Y plane between points p1 and p2.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float Distance(PointF p1, PointF p2)
        {
            double deltaX = p2.X - p1.X;
            double deltaY = p2.Y - p1.Y;

            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public static float DistanceSquared(PointF p1, PointF p2)
        {
            float deltaX = p2.X - p1.X;
            float deltaY = p2.Y - p1.Y;

            return deltaX * deltaX + deltaY * deltaY;
        }


        public static double Angle(PointF p1, PointF p2)
        {
            return Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        }

        public static double Area(ArrayList pointList)
        {
            return Area(new List<PointF>((PointF[])pointList.ToArray(typeof(PointF))));
        }

        public static double Area(List<PointF> pointListF)
        {
            double area;

            if (pointListF.Count > 2)
            {
                int i = 1;
                area = 0;

                PointF p1 = pointListF[0];
                PointF p2 = pointListF[1];

                foreach (PointF point in pointListF)
                {
                    if (i > 2)
                    {
                        area += (point.X - p1.X) * (point.Y - p2.Y) + (p2.X - point.X) * (point.Y - p1.Y);

                        // set p2 for next iteration
                        p2 = point;
                    }
                    i++;
                }

                area *= 0.5f;

            }
            else
            {
                area = 0;
            }

            return area;
        }

        public static float Direction(PointF p1, PointF p2, PointF p3)
        {
            PointF vectorA = PointF.Subtract(p1, new SizeF(p2));
            PointF vectorB = PointF.Subtract(p2, new SizeF(p3));

            return vectorA.X * vectorB.Y - vectorA.Y * vectorB.X;
        }

        public static bool IsPointOnSegment(PointF point, PointF segmentP1, PointF segmentP2, float tolerance)
        {
            // Check if point is within segment bounding rectangle:

            // Horizontal
            if ((point.X > segmentP1.X + tolerance && point.X > segmentP2.X + tolerance) ||
               (point.X < segmentP1.X - tolerance && point.X < segmentP2.X - tolerance))
                return false;

            // Vertical
            if ((point.Y > segmentP1.Y + tolerance && point.Y > segmentP2.Y + tolerance) ||
               (point.Y < segmentP1.Y - tolerance && point.Y < segmentP2.Y - tolerance))
                return false;

            // Now check distance from point to line
            float t = ((point.X - segmentP1.X) * (segmentP2.X - segmentP1.X) + (point.Y - segmentP1.Y) * (segmentP2.Y - segmentP1.Y)) / DistanceSquared(segmentP1, segmentP2);
            if (t < 0 || t > 1) return false;
            if (DistanceSquared(point, new PointF(segmentP1.X + t * (segmentP2.X - segmentP1.X), segmentP1.Y + t * (segmentP2.Y - segmentP1.Y))) < tolerance * tolerance)
                return true;

            return false;
        }
    }
}
