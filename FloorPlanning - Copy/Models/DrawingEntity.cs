using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

// PDFSharp
using PdfSharp;

using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using AForge.Imaging.Filters;
using FloorPlanning.Models;

namespace FloorPlanning
{
    [Serializable]
    public abstract class DrawingEntity : IDisposable
    {
        public enum EntityType
        {
            /// <summary>
            /// No entity, for abstract class parent class only
            /// </summary>
            None = 0,

            /// <summary>
            /// Polyline
            /// </summary>
            Polyline = 1,

            /// <summary>
            /// Polygon
            /// </summary>
            Polygon = 2,


            /// <summary>
            /// Image Box
            /// </summary>
            ImageBox = 5,

            

            DoorTakeOut = 12,
        }

        DrawingDoc ownerDrawing = null;
        Layer layer = null;
        ImageEntry imageEntry = null;

        [NonSerialized] bool isBeingEdited = false;
        [NonSerialized] bool hasChanged = false;
        [NonSerialized] bool hasBeenRendered = false;

        // Methods
        //
        public abstract void Render(XGraphics gfx);

        public virtual bool MouseMove(PointF point)
        {
            return false;
        }

        public virtual bool MouseClick(PointF point, bool shift)
        {
            return false;
        }

        public virtual bool IsValid()
        {
            return true;
        }

        public virtual bool IsSelectedByPoint(PointF pickPoint)
        {
            return false;
        }

        public virtual bool IsSelectedInRectangle(RectangleF r)
        {
            return false;
        }

        public virtual PointF[] GripPoints()
        {
            return null;
        }

        public virtual void GripMoved(int gripNumber, PointF point)
        {
            return;
        }

        public virtual void DisposeImages()
        {
            return;
        }

        public virtual void Dispose()
        {
            return;
        }
        //
        // End Methods

        // Properties
        //
        public virtual float Area
        {
            get { return 0; }
        }

        public virtual float Perimeter
        {
            get { return 0; }
        }

        public virtual int ItemNumber
        {
            get { return 0; }
        }

        public virtual EntityType Type
        {
            get { return EntityType.None; }
        }

        public virtual ComponentDef Component
        {
            get { return null; }
        }

        public virtual ImageEntry ImageEntry
        {
            get { return imageEntry; }
            set { imageEntry = value; }
        }

        public virtual Layer Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        public virtual DrawingDoc OwnerDrawing
        {
            get { return ownerDrawing; }
            set { ownerDrawing = value; }
        }

        public virtual bool IsBeingEdited
        {
            get { return isBeingEdited; }
            set { isBeingEdited = value; }
        }

        public virtual bool HasChanged
        {
            get { return hasChanged; }
            set { hasChanged = value; }
        }

        public virtual bool HasBeenRendered
        {
            get { return hasBeenRendered; }
            set { hasBeenRendered = value; }
        }

        public virtual int CornerCount
        {
            get { return 0; }
        }
    }
    
    [Serializable]
    public class ImageBox : DrawingEntity
    {
        PointF location;

        double width, height;
        [NonSerialized]
        double initWidth;
        [NonSerialized]
        double initHeight;
        [NonSerialized]
        double initScale;
        [NonSerialized]
        XImage image;
        [NonSerialized]
        Image imageFiltered;
        [NonSerialized]
        Image lastImageFiltered;
        Layer layer;
        string filePath;
        int pageNumber;
        int pageCount;
        bool previewMode;
        int rotationAngle;
        double opacity;
        bool opacitySet;

        // Missing File Font
        static XPdfFontOptions fontOptions = new XPdfFontOptions(PdfFontEmbedding.None);
        static Font missingFileFont = new Font("Verdana", 16, GraphicsUnit.World);
        static XFont missingFileXFont = new XFont(missingFileFont, fontOptions);

        // Brushes
        static XSolidBrush textBrush = new XSolidBrush(Color.DimGray);
        static XSolidBrush rectBrush = new XSolidBrush(Color.WhiteSmoke);

        public ImageBox(float left, float top, double width, double height, string filePath, int pageNumber, Layer layer)
        {
            this.layer = layer;
            location.X = left;
            location.Y = top;
            this.width = width;
            this.height = height;
            this.pageNumber = pageNumber;

            rotationAngle = 0;
            opacity = 1.0;
            opacitySet = false;
            this.filePath = filePath;

            layer.AddEntity(this);
        }

        public ImageBox(string filePath, Layer layer)
        {
            location = PointF.Empty;
            
            this.layer = layer;
            this.filePath = filePath;

            layer.AddEntity(this);

            rotationAngle = 0;
            opacity = 1.0;
            opacitySet = false;
        }

        private const int bytesPerPixel = 4;

        /// <summary>
        /// Change the opacity of an image
        /// </summary>
        /// <param name="originalImage">The original image</param>
        /// <param name="opacity">Opacity, where 1.0 is no opacity, 0.0 is full transparency</param>
        /// <returns>The changed image</returns>
        public static Image ChangeImageOpacity(Image originalImage, double opacity)
        {
            if ((originalImage.PixelFormat & PixelFormat.Indexed) == PixelFormat.Indexed)
            {
                // Cannot modify an image with indexed colors
                return originalImage;
            }

            Bitmap bmp = (Bitmap)originalImage.Clone();

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            // This code is specific to a bitmap with 32 bits per pixels 
            // (32 bits = 4 bytes, 3 for RGB and 1 byte for alpha).
            int numBytes = bmp.Width * bmp.Height * bytesPerPixel;
            byte[] argbValues = new byte[numBytes];

            // Copy the ARGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, argbValues, 0, numBytes);

            // Manipulate the bitmap, such as changing the
            // RGB values for all pixels in the the bitmap.
            for (int counter = 0; counter < argbValues.Length; counter += bytesPerPixel)
            {
                // argbValues is in format BGRA (Blue, Green, Red, Alpha)

                // If 100% transparent, skip pixel
                if (argbValues[counter + bytesPerPixel - 1] == 0)
                    continue;

                int pos = 0;
                pos++; // B value
                pos++; // G value
                pos++; // R value

                argbValues[counter + pos] = (byte)(argbValues[counter + pos] * opacity);
            }

            // Copy the ARGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        public static float AlphaBlend(float sourceColor, float backgroundColor, float alpha)
        {
            return sourceColor*alpha/255f + backgroundColor*(255f - alpha)/255f;
        }

        public static Image ColorImage(Image originalImage, Image overlay, Color c)
        {
            Bitmap bmp = (Bitmap)originalImage.Clone();
            Bitmap bmpO = (Bitmap)overlay.Clone();

            // Specify a pixel format.
            PixelFormat pxf = PixelFormat.Format32bppArgb;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);
            BitmapData bmpOData = bmpO.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;
            IntPtr ptrO = bmpOData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            // This code is specific to a bitmap with 32 bits per pixels 
            // (32 bits = 4 bytes, 3 for RGB and 1 byte for alpha).
            int numBytes = bmp.Width * bmp.Height * bytesPerPixel;
            byte[] argbValues = new byte[numBytes];
            byte[] argbOValues = new byte[numBytes];

            // Copy the ARGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, argbValues, 0, numBytes);
            System.Runtime.InteropServices.Marshal.Copy(ptrO, argbOValues, 0, numBytes);

            // Manipulate the bitmap, such as changing the
            // RGB values for all pixels in the the bitmap.
            for (int counter = 0; counter < argbValues.Length; counter += bytesPerPixel)
            {
                int pos = 0;
                if (argbOValues[counter + pos] < 252)
                {
                    argbValues[counter + pos] = (byte)((int)AlphaBlend(argbOValues[counter + pos], c.B, c.A));
                    pos++; // B value
                    argbValues[counter + pos] = (byte)((int)AlphaBlend(argbOValues[counter + pos], c.G, c.A));
                    pos++; // G value
                    argbValues[counter + pos] = (byte)((int)AlphaBlend(argbOValues[counter + pos], c.R, c.A));
                    pos++; // R value
                    argbValues[counter + pos] = 255;
                }
                else
                {
                    argbValues[counter + pos] = c.B;
                    pos++; // B value
                    argbValues[counter + pos] = c.G;
                    pos++; // G value
                    argbValues[counter + pos] = c.R;
                    pos++; // R value
                    argbValues[counter + pos] = 255;// c.A;
                }
            }

            // Copy the ARGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        // Methods
        //
        public override bool IsSelectedByPoint(PointF pickPoint)
        {
            if (pickPoint.X > location.X && pickPoint.Y > location.Y)
                if (pickPoint.X < location.X + width && pickPoint.Y < location.Y + height)
                    return true;

            return false;
        }

        public override bool IsSelectedInRectangle(RectangleF r)
        {
            PointF p0 = new PointF(location.X, location.Y);
            PointF p1 = new PointF(location.X, location.Y + (float)height);
            PointF p2 = new PointF(location.X + (float)width, location.Y);
            PointF p3 = new PointF(location.X + (float)width, location.Y + (float)height);
            if (r.Contains(p0) || r.Contains(p1) || r.Contains(p2) || r.Contains(p3))
            {
                return true;
            }

            return false;
        }

        
        public override PointF[] GripPoints()
        {
            PointF[] gripPoints = new PointF[1];

            gripPoints[0] = location;

            return gripPoints;
        }

        public Image GetThumb()
        {
            try
            {
                return DrawingDoc.loadThumbImage(filePath);
            }
            catch
            {
            }
            return null;
        }

        public Image GetImage()
        {
            try
            {
                return DrawingDoc.loadImage(filePath, 0, 0, true);
            }
            catch 
            { }
            return null;
        }


        public void GetPartialImage(XGraphicsPath path, XGraphics gfx, Color c, bool bRemove)
        {
            //return XImage.FromGdiPlusImage(imageFiltered);

            RectangleF cropRect = path.Internals.GdiPath.GetBounds();           

            /*Bitmap b = new Bitmap((int)Width, (int)Height);
            Graphics bg = Graphics.FromImage(b);
            bg.DrawImage(imageFiltered, (int)location.X, (int)location.Y, (int)width, (int)height);*/


            initScale = initWidth / width;
            int upperWith = (int)Math.Ceiling((double)cropRect.Width * initScale);
            int upperHeight = (int)Math.Ceiling((double)cropRect.Height * initScale);
            Bitmap nb = new Bitmap(upperWith, upperHeight);
            Graphics g = Graphics.FromImage(nb);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.CompositingMode = CompositingMode.SourceCopy;
            g.DrawImage(lastImageFiltered, (float)(-(double)cropRect.X * initScale), (float)(-(double)cropRect.Y * initScale));
            //return nb;
            //nb.MakeTransparent(Color.Black);
            
            ///////////////////
            Bitmap background = new Bitmap(nb.Width, nb.Height);
            background = (Bitmap)ColorImage((Bitmap)background, (Bitmap)nb, c);

            /*XImage img = null;
            var imgattr = new ImageAttributes();

            Color lowerColor = Color.FromArgb(200, 200, 200);
            imgattr.SetColorKey(lowerColor, Color.White);


            var g2 = Graphics.FromImage(background);
            g2.SmoothingMode = SmoothingMode.HighQuality;
            g2.CompositingQuality = CompositingQuality.HighQuality;
            g2.CompositingMode = CompositingMode.SourceCopy;

            g2.DrawImage(
                nb,
                new Rectangle(0, 0, nb.Width, nb.Height),
                0, 0, nb.Width, nb.Height,
                GraphicsUnit.Pixel, imgattr
            );*/

            /*nb = new Bitmap((int)((double)cropRect.Width), (int)((double)cropRect.Height));
            g = Graphics.FromImage(nb);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.CompositingMode = CompositingMode.SourceCopy;
            initScale = initWidth / width;
            g.DrawImage(background, 0, 0, nb.Width, nb.Height);*/

            XImage img = null;
            if (bRemove)
            {
                img = XImage.FromGdiPlusImage(nb);
            }
            else
            {
                img = XImage.FromGdiPlusImage(background);
            }

            ///////////////////

            //XImage img = XImage.FromGdiPlusImage(nb);
            gfx.IntersectClip(path);
            gfx.DrawImage(img, cropRect.X, cropRect.Y, cropRect.Width, cropRect.Height);//img.Width, img.Height);

            /*Bitmap imgFinal = null;
            
            try
            {
                imgFinal = (Bitmap)imageFiltered.Clone();
                Bitmap b = new Bitmap(imageFiltered.Width, imageFiltered.Height);
                Graphics bg = Graphics.FromImage(b);
                bg.Clip = new Region(path.Internals.GdiPath);
                bg.DrawImage(imgFinal, 0, 0);
            }
            catch 
            {
                imgFinal = null;
            }
            return imgFinal;*/
        }

        public override void Render(XGraphics gfx)
        {
            if (Program.BackgroundVisible == false)
                return;
            if (image == null)
            {
                // Get image
                if (opacitySet == false)
                {
                    opacitySet = true;
                    opacity = 1;
                }
                try
                {
                    Image origImg = ChangeImageOpacity(DrawingDoc.loadImage(filePath, pageNumber, rotationAngle, previewMode), opacity);
                    Bitmap ini = new Bitmap(origImg);
                    initHeight = ini.Height;
                    initWidth = ini.Width;
                    //Bitmap orig = Grayscale.CommonAlgorithms.RMY.Apply(ini);
                    /*DifferenceEdgeDetector filter = new DifferenceEdgeDetector();
                    // apply the filter
                    filter.ApplyInPlace(orig);
                    Invert fI = new Invert();
                    // apply the filter
                    fI.ApplyInPlace(orig);
                    Threshold fT = new Threshold(248);
                    string s = orig.PixelFormat.ToString();
                    fT.ApplyInPlace(orig);*/
                    //imageFiltered = XImage.FromGdiPlusImage(orig);
                    imageFiltered = ini;
                    image = XImage.FromGdiPlusImage(origImg);
                    initScale = initHeight / image.PointHeight;
                    Bitmap b = new Bitmap((int)initWidth, (int)initHeight);
                    //Graphics bg = gfx.Graphics;
                    
                    Graphics bg = Graphics.FromImage(b);
                    bg.SmoothingMode = SmoothingMode.HighQuality;
                    bg.CompositingQuality = CompositingQuality.HighQuality;
                    bg.CompositingMode = CompositingMode.SourceCopy;
                    bg.DrawImage(imageFiltered, (int)location.X, (int)location.Y, (int)initWidth, (int)initHeight);
                    lastImageFiltered = (Bitmap)b.Clone();
                                            
                }
                catch (Exception ex)
                {
                    string e = ex.ToString();
                }
            }

            XGraphicsContainer container = gfx.BeginContainer();

            if (image != null)
            {
                if (width == 0)
                {
                    // If width has not been specified, use the image width
                    gfx.DrawImage(image, location);

                    width = image.PointWidth;
                    height = image.PointHeight;
                }
                else
                {
                    gfx.DrawImage(image, location.X, location.Y, width, height);
                    Bitmap b = new Bitmap((int)initWidth, (int)initHeight);
                    Graphics bg = Graphics.FromImage(b);
                    bg.SmoothingMode = SmoothingMode.HighQuality;
                    bg.CompositingQuality = CompositingQuality.HighQuality;
                    bg.CompositingMode = CompositingMode.SourceCopy;
                    bg.DrawImage(imageFiltered, (int)location.X, (int)location.Y, (int)initWidth, (int)initHeight);
                    lastImageFiltered = (Bitmap)b.Clone();
                    initScale = initHeight / image.PointHeight;
                    //imageFiltered = (Image)b.Clone();
                }
            }
            else
            {                
                gfx.DrawRectangle(rectBrush, location.X, location.Y, width, height);
                gfx.DrawString("Could not find base drawing file:" + "\n" + filePath, missingFileFont, textBrush, location.X + width / 2, location.Y + height / 2, XStringFormats.Center);
            }

            gfx.EndContainer(container);
        }

        public void Rotate90()
        {
            rotationAngle = (rotationAngle + 90) % 360;

            double tempWidth;

            tempWidth = width;
            width = height;
            height = tempWidth;
                
            image = null;
        }

        public void RotateToZero()
        {
            rotationAngle = 0;
        }

        public override void DisposeImages()
        {
            if (image != null)
            {
                image.Dispose();
                image = null;
            }
        }

        ~ImageBox()
        {
            DisposeImages();
        }
        //
        // End Methods Section

        // Properties
        //
        public override EntityType Type
        {
            get { return EntityType.ImageBox; }
        }

        public bool PreviewMode
        {
            get { return previewMode; }
            set
            {
                previewMode = value;
                image = null;
            }
        }

        public int PDFPageCount
        {
            get { return pageCount;}
            set { pageCount = value;}
        }

        public int PDFPageNumber
        {
            get { return pageNumber; }
            set
            {
                pageNumber = value;
                image = null;
            }
        }

        public PointF Location
        {
            get { return location; }
            set
            {
                location = value;
            }
        }

        public PointF LocationMM
        {
            get
            {
                return new PointF(location.X * DrawingDoc.mmPerPoint, location.Y * DrawingDoc.mmPerPoint);
            }
            set
            {
                // We need to convert mm to pt
                location.X = value.X / DrawingDoc.mmPerPoint;
                location.Y = value.Y / DrawingDoc.mmPerPoint;
            }
        }

        public override Layer Layer
        {
            get { return layer; }
        }

        public String FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        public int RotationAngle
        {
            get { return rotationAngle; }
            set { rotationAngle = value; }
        }
        public double Opacity
        {
            get { return opacity; }
            set { 
                opacity = value;
                opacitySet = true;
            }
        }
        //
        // End properties section
    }

    public class Line : DrawingEntity
    {
        PointF point1, point2;

        static XPen pen = new XPen(Color.FromArgb(160, 80, 120, 120), 3);

        public Line(PointF point1)
        {
            this.point1 = new PointF();
            this.point2 = new PointF();

            this.point1 = point1;
        }

        // Methods
        //
        public void Point2(PointF pointInMM)
        {
            if (!pointInMM.IsEmpty)
            {
                // We need to convert mm to pt
                point2.X = pointInMM.X / DrawingDoc.mmPerPoint;
                point2.Y = pointInMM.Y / DrawingDoc.mmPerPoint;
            }
        }

        public override void Render(XGraphics gfx)
        {
            gfx.DrawLine(pen, point1, point2);
        }
    }


    [Serializable]
    public class DoorTakOut : DrawingEntity
    {
        PointF point;
        ComponentDef component;
        Layer layer;
        float length;
        DrawingDoc ownerDrawing;
        public float measure;

        // Constructors
        //
        public DoorTakOut(PointF firstPoint, ComponentDef component, Layer layer, DrawingDoc ownerDrawing, float l_measure)
        {
            point = firstPoint;
            measure = l_measure;
            this.component = component;
            this.ownerDrawing = ownerDrawing;
            this.layer = layer;

            layer.AddEntity(this);
        }

        public DoorTakOut(DoorTakOut pToClone)
        {
            base.OwnerDrawing = pToClone.OwnerDrawing;
            base.Layer = pToClone.Layer;

            this.point = pToClone.point;
            this.measure = pToClone.measure;
            this.component = pToClone.Component;
            this.ownerDrawing = pToClone.OwnerDrawing;

            Layer.AddEntity(this);


            HasChanged = false;
        }
        //
        // End Constructors

        // Methods
        //
        public void AddPoint(PointF pointInMM)
        {
            if (!pointInMM.IsEmpty)
            {
                // We need to convert mm to pt
                point.X = pointInMM.X / DrawingDoc.mmPerPoint;
                point.Y = pointInMM.Y / DrawingDoc.mmPerPoint;
            }
        }

        public void RemoveLastPoint()
        {
            ;
        }

        /*public override bool IsSelectedByPoint(PointF pickPoint)
        {
            int i = 0;

            PointF previousPoint = (PointF)pointList[pointList.Count - 1];

            foreach (PointF point in pointList)
            {
                if (i > 0 || (i == 0 && isClosed))
                    if (DrawingDoc.IsPointOnSegment(pickPoint, previousPoint, point, 2f))
                        return true;

                previousPoint = point;
                i++;
            }

            return false;
        }

        public override bool IsSelectedInRectangle(RectangleF r)
        {
            foreach (PointF cornerPoint in cornerList)
            {
                if (r.Contains(cornerPoint))
                {
                    return true;
                }
            }

            return false;
        }

        public override PointF[] GripPoints()
        {
            PointF[] gripPoints = (PointF[])pointList.ToArray(typeof(PointF));

            return gripPoints;
        }*/

        public override int ItemNumber
        {
            get
            {
                if (layer != null)
                    return layer.GetEntityNumber(this);
                else
                    return 0;
            }
        }

        public override void Render(XGraphics gfx)
        {
            if (Program.bLineMode && !Program.HideLineDoorTO)
            {
                XSolidBrush brush = new XSolidBrush(Color.FromArgb(255, component.DrawColor.R,component.DrawColor.G,component.DrawColor.B));//component.DrawColor);
                Font itemFont = new Font(Program.ItemFont.FontFamily, 9f,
                    FontStyle.Bold, GraphicsUnit.World);
                XPdfFontOptions fontOptions = new XPdfFontOptions(PdfFontEmbedding.Always);                
                XFont font = new XFont(itemFont, fontOptions);

                XSize stringSize = new XSize();
                stringSize = gfx.MeasureString(this.measure.ToString(), font);

                gfx.DrawString(this.measure.ToString(), font, brush, new PointF(point.X - (float)stringSize.Width / 2, point.Y + (float)stringSize.Height / 3));

                PointF canvasPoint = new PointF(point.X, point.Y);
                canvasPoint.X -= 10;
                canvasPoint.Y -= 10;
                RectangleF rect = new RectangleF(canvasPoint, new SizeF(20, 20));
                XPen penL = new XPen(component.B.GetColor(), 2);
                gfx.DrawEllipse(penL, rect);

            }
        }
        //
        // End Methods

        // Properties section
        //
        public PointF LastPoint
        {
            get
            {
                PointF lastPointPt = (PointF)point;
                PointF lastPointMM = new PointF();

                // We need to convert pt to mm
                lastPointMM.X = lastPointPt.X * DrawingDoc.mmPerPoint;
                lastPointMM.Y = lastPointPt.Y * DrawingDoc.mmPerPoint;

                return lastPointMM;
            }
        }

        public override ComponentDef Component
        {
            get { return component; }
        }

        public override float Perimeter
        {
            get { return measure; }
        }

        public override EntityType Type
        {
            get { return EntityType.DoorTakeOut; }
        }
    }

    [Serializable]
    public class Polyline : DrawingEntity 
    {
        ArrayList pointList = new ArrayList();
        PointF[] points;
        byte[] types; 
        ComponentDef component;
        Layer layer;
        float length;
        bool isClockwise;
        bool isClosed;
        DrawingDoc ownerDrawing;
        List<PointF> cornerList = new List<PointF>();
        public int dashType = 0;

        // GraphicsPath is cached for performace, it is not saved to file.
        [NonSerialized]
        XGraphicsPath graphicsPath;
        [NonSerialized]
        XGraphicsPath graphicsParalelPath;

        // Constructors
        //
        public Polyline(PointF firstPoint, ComponentDef component, Layer layer, DrawingDoc ownerDrawing)
        {
            pointList.Add(firstPoint);
            this.component = component;
            if ((Program.jump_b1_b2_dup_Mode >= 2))
            {
                dashType = 1;//double
            }
            this.ownerDrawing = ownerDrawing;
            this.layer = layer;

            layer.AddEntity(this);
        }

        public Polyline(Polyline pToClone)
        {
            base.OwnerDrawing = pToClone.OwnerDrawing;
            base.Layer = pToClone.Layer;

            this.pointList = pToClone.pointList;
            this.component = pToClone.Component;
            this.ownerDrawing = pToClone.OwnerDrawing;

            createGraphicsPath();
            Layer.AddEntity(this);


            HasChanged = false;
        }

        public Polyline(PointF[] points, ComponentDef component, Layer layer, DrawingDoc ownerDrawing)
        {
            this.pointList = new ArrayList(points);
            this.component = component;
            this.ownerDrawing = ownerDrawing;

            createGraphicsPath();
            layer.AddEntity(this);
        }
        //
        // End Constructors

        // Methods
        //
        public void AddPoint(PointF pointInMM)
        {
            if (!pointInMM.IsEmpty)
            {
                PointF point = new PointF();

                // We need to convert mm to pt
                point.X = pointInMM.X / DrawingDoc.mmPerPoint;
                point.Y = pointInMM.Y / DrawingDoc.mmPerPoint;

                pointList.Add(point);

                createGraphicsPath();
            }
        }

        public void Close(PointF point)
        {
            isClosed = true;
            createGraphicsPath();
        }

        public bool RemoveLastPoint()
        {
            if (pointList.Count > 1)
            {
                pointList.RemoveAt(pointList.Count - 1);

                createGraphicsPath();
                return false;
            }
            else
            {
                return true;
            }
        }

        private ArrayList CreateParallel(ArrayList l_a)
        {
            PointF p0 = (PointF)l_a[0];
            PointF p1 = (PointF)l_a[1];
            float x1 = p0.X;
            float x2 = p1.X;
            float y1 = p0.Y;
            float y2 = p1.Y;

            var L = Math.Sqrt((x1-x2)*(x1-x2)+(y1-y2)*(y1-y2));

            var offsetPixels = Program.n2xSeparation;// 10.0;

            // This is the second line
            var x1p = x1 + offsetPixels * (y2-y1) / L;
            var x2p = x2 + offsetPixels * (y2-y1) / L;
            var y1p = y1 + offsetPixels * (x1-x2) / L;
            var y2p = y2 + offsetPixels * (x1 - x2) / L;
            ArrayList a = new ArrayList();
            a.Add(new PointF((float)x1p, (float)y1p));
            a.Add(new PointF((float)x2p, (float)y2p));
            return a;
        }

        void createGraphicsPath()
        {
            if (pointList == null)
            {
                pointList = new ArrayList(points);
                points = null;
            }

            if (pointList.Count > 1)
            {
                int i = 0;

                types = new byte[pointList.Count];

                foreach (PointF point in pointList)
                {
                    types[i] = (byte)PathPointType.Line;
                    i++;
                }

                types[0] = (byte)PathPointType.Start;
                if (isClosed)
                    types[i - 1] = (byte)(PathPointType.CloseSubpath | PathPointType.Line);

                graphicsPath = new XGraphicsPath(pointList.ToArray(typeof(PointF)) as PointF[], types, XFillMode.Alternate);
                if (dashType == 1)
                {
                    ArrayList pointParalelList = CreateParallel(pointList);
                    byte[] typesParalel = new byte[2];
                    typesParalel[0] = (byte)PathPointType.Start;
                    typesParalel[1] = (byte)PathPointType.Line;
                    if (pointParalelList.Count == 2)
                    {
                        graphicsParalelPath = new XGraphicsPath(pointParalelList.ToArray(typeof(PointF)) as PointF[], typesParalel, XFillMode.Alternate);
                    }
                    else
                    {
                        graphicsParalelPath = null;
                    }
                }

                FindDirection();
                if (component.Type == ComponentType.Perimeter)
                {
                    FindCorners();
                    CalculateLength();
                }
            }
            else
            {
                graphicsPath = null;
                graphicsParalelPath = null;
            }
        }

        void CalculateLength()
        {
            if (pointList.Count > 1)
            {
                int i = 0;
                length = 0;

                PointF previousPoint = (PointF)pointList[pointList.Count - 1];

                foreach (PointF point in pointList)
                {
                    if (i > 0 || (i == 0 && isClosed))
                        length += DrawingDoc.Distance(previousPoint, point);

                    previousPoint = point;
                    i++;
                }

                // Need to check because polylines in older jobs might not have the ownerDrawing variable
                if (ownerDrawing != null)
                    length += cornerList.Count * (component.CornerAdditional / DrawingDoc.mmPerPoint) / ownerDrawing.Scale;
            }
            else
                length = 0;
        }

        void FindDirection()
        {
            if (isClosed)
            {
                if (DrawingDoc.Area(pointList) > 0)
                    isClockwise = true;
                else
                    isClockwise = false;
            }
        }

        public override bool IsSelectedByPoint(PointF pickPoint)
        {
            int i = 0;

            PointF previousPoint = (PointF)pointList[pointList.Count - 1];

            foreach (PointF point in pointList)
            {
                if (i > 0 || (i == 0 && isClosed))
                    if (DrawingDoc.IsPointOnSegment(pickPoint, previousPoint, point, 2f))
                        return true;

                previousPoint = point;
                i++;
            }

            return false;
        }

        public override bool IsSelectedInRectangle(RectangleF r)
        {
            foreach (PointF cornerPoint in cornerList)
            {
                if (r.Contains(cornerPoint))
                {
                    return true;
                }
            }

            return false;
        }

        public override PointF[] GripPoints()
        {
            PointF[] gripPoints = (PointF[])pointList.ToArray(typeof(PointF));

            return gripPoints;
        }

        public override int ItemNumber
        {
            get
            {
                if (layer != null)
                    return layer.GetEntityNumber(this);
                else
                    return 0;
            }
        }

        public void FlipDirection()
        {
            if (!isClosed)
            {
                isClockwise = !isClockwise;
            }
        }
        private void FindCorners()
        {
            if (cornerList == null)
                cornerList = new List<PointF>();
            else
                cornerList.Clear();

            if (pointList.Count > 2 && component.CornerAdditional > 0)
            {
                PointF p1 = (PointF)pointList[pointList.Count - 2];
                PointF p2 = (PointF)pointList[pointList.Count - 1];

                int i = 1;
                foreach (PointF p3 in pointList)
                {
                    bool isPositive = DrawingDoc.Direction(p1, p2, p3) > 0;

                    if (isClosed || i > 2)
                        if (isPositive && isClockwise || !isPositive && !isClockwise)
                        {
                            cornerList.Add(p2);
                        }

                    p1 = p2;
                    p2 = p3;
                    i++;
                }
            }
        }

        public override void Render(XGraphics gfx)
        {
            //XPen pen = new XPen(component.DrawColor, 2);//5);
            XPen pen = new XPen(component.B.GetColor(), (double)component.B.Width);//Color.Red, 2);
            if (component.B.CapType == 0)
            {
                pen.LineCap = XLineCap.Flat;
            }
            else
            {
                pen.LineCap = XLineCap.Round;
            }

            // Create a custom dash pattern.
            if (component.B.DashStyle == 0)
            {
                pen.DashStyle = XDashStyle.Solid;
            }
            else if (component.B.DashStyle == 1)
            {
                pen.DashStyle = XDashStyle.Dot;
            }
            else if (component.B.DashStyle == 2)
            {
                pen.DashStyle = XDashStyle.Dash;
            }
            else
            {
                pen.DashStyle = XDashStyle.DashDot;
            }


            //If zero line, dashed
            if (component.bLineZeroLine)
            {
                pen.DashStyle = XDashStyle.Dash;
            }

            // Check if old version
            if (pointList == null)
            {
                pointList = new ArrayList(points);
                points = null;
            }

            if (pointList.Count > 1)
            {
                if ((graphicsPath == null) || HasChanged)
                {
                    createGraphicsPath();
                    HasChanged = true;
                }

                if (Program.bLineMode == false)
                    return;
                XGraphicsContainer container = gfx.BeginContainer();

                if (dashType == 0)
                {
                    gfx.DrawPath(pen, graphicsPath);
                }
                else
                {
                    gfx.DrawPath(pen, graphicsPath);
                    if (graphicsParalelPath != null)
                        gfx.DrawPath(pen, graphicsParalelPath);
                }

                // Render corners
                if (cornerList != null)
                    foreach (PointF cornerPoint in cornerList)
                    {
                        RectangleF rect = new RectangleF(PointF.Subtract(cornerPoint, new SizeF(5f, 5f)), new SizeF(10f, 10f));
                        gfx.DrawEllipse(pen, rect);
                    }

                gfx.EndContainer(container);
            }
        }
        //
        // End Methods

        // Properties section
        //
        public override int CornerCount
        {
            get { return cornerList.Count; }
        }

        public int DashType
        {
            get
            {
                return dashType;
            }
        }
        public PointF LastPoint
        {
            get
            {
                PointF lastPointPt = (PointF)pointList[pointList.Count - 1];
                PointF lastPointMM = new PointF();

                // We need to convert pt to mm
                lastPointMM.X = lastPointPt.X * DrawingDoc.mmPerPoint;
                lastPointMM.Y = lastPointPt.Y * DrawingDoc.mmPerPoint;

                return lastPointMM;
            }
        }

        public bool IsClosed
        {
            get { return isClosed; }
            set
            {
                if (value != isClosed)
                {
                    isClosed = value;
                    createGraphicsPath();
                }
            }
        }

        public override ComponentDef Component
        {
            get { return component; }
        }

        public override float Perimeter
        {
            get { return length; }
        }

        public PointF[] Points
        {
            get
            {
                // Check if old version
                if (pointList == null)
                {
                    pointList = new ArrayList(points);
                    points = null;
                }

                return pointList.ToArray(typeof(PointF)) as PointF[];
            }
        }

        public int Count
        {
            get { return pointList.Count; }
        }

        public override EntityType Type
        {
            get { return EntityType.Polyline; }
        }

        public bool IsClockwise
        {
            get { return isClockwise; }
        }
    }

    [Serializable]
    public class Polygon : DrawingEntity
    {
        // Legacy
        ArrayList pointList;
        PointF firstPoint = new PointF(0, 0);
        List<PointF> pointListF = null;
        List<bool> pointListFZeros = null;
        byte[] types;
        ComponentDef component;
        double area;
        double length;
        bool isClockwise;
        Layer layer;
        PointF textLocation = new PointF();
        Polyline polyLine = null;
        bool isCrop = false;
        float cropDistance = 2000f;
        float lastWidthCrop = 0f;

        // GraphicsPath is cached for performace, it is not saved to file.
        [NonSerialized]
        XGraphicsPath graphicsPath;
        [NonSerialized]
        XGraphicsPath graphicsPathLines;
        [NonSerialized]
        List<PointF> pointFMeasuresFinal;


        public Polygon(PointF firstPoint, ComponentDef component, Layer layer, DrawingDoc ownerDrawing)
        {
            pointListF = new List<PointF>();
            pointListFZeros = new List<bool>();
            pointListF.Add(firstPoint);
            pointListFZeros.Add(Program.bAreaZeroLine);
            this.component = component;
            this.layer = layer;
            layer.AddEntity(this);

            base.OwnerDrawing = ownerDrawing;
        }

        public Polygon(Polygon pToClone)
        {
            base.OwnerDrawing = pToClone.OwnerDrawing;
            base.Layer = pToClone.Layer;
            layer = pToClone.Layer;
            Layer = pToClone.Layer;
            cropDistance = pToClone.cropDistance;
            this.pointListF = pToClone.pointListF;
            this.pointListFZeros = pToClone.pointListFZeros;
            this.component = pToClone.Component;

            this.OwnerDrawing = pToClone.OwnerDrawing;

            createGraphicsPath();
            layer.AddEntity(this);

            HasChanged = false;
        }

        public Polygon(PointF[] points, ComponentDef component, Layer layer, DrawingDoc ownerDrawing)
        {
            pointListF = new List<PointF>(points);
            pointListFZeros = new List<bool>();
            this.component = component;
            createGraphicsPath();

            this.layer = layer;
            layer.AddEntity(this);

            base.OwnerDrawing = ownerDrawing;
        }

        public void SetCropDistance(float l_cropDistance)
        {
            cropDistance = l_cropDistance;
            createGraphicsPath();
        }

        public float GetCropDistance()
        {
            return cropDistance;
        }

        void createGraphicsPath()
        {
            List<PointF> lPF = new List<PointF>();
            if (pointListF == null || pointListF.Count == 0)
            {
                if (pointList != null)
                {
                    pointListF = new List<PointF>((PointF[])pointList.ToArray(typeof(PointF)));
                    pointList = null;
                }
                else
                {
                    return;
                }
            }

            foreach (PointF pF in pointListF)
            {
                lPF.Add(pF);
            }
            if (pointListF.Count == 1)
            {
                lPF.Add(firstPoint);
            }

            if (lPF != null && lPF.Count > 1)
            {
                types = new byte[lPF.Count];
                int i = 0;

                foreach (PointF point in lPF)
                {
                    types[i] = (byte)PathPointType.Line;
                    i++;
                }

                types[0] = (byte)PathPointType.Start;

                graphicsPath = new XGraphicsPath();
                

                if (lPF.Count > 2)
                {
                    graphicsPath = new XGraphicsPath();
                    graphicsPath.AddPolygon(lPF.ToArray());
                    CalculateArea();
                    CalculateLength();

                    if (component.Mode == CalculationMode.Itemize && layer != null)
                    {
                        SegmentF baseLine = FindBaseLine();

                        if (baseLine != null)
                        {
                            textLocation.X = baseLine.Point1.X + 20 * Math.Sign(baseLine.Point2.X - baseLine.Point1.X);
                            textLocation.Y = baseLine.Point1.Y - 20;
                        }
                        else
                        {
                            // If we could not find a base line, we just use the first point
                            // This might result in the number being outside the Polygon
                            textLocation.X = lPF[0].X + 20;
                            textLocation.Y = lPF[0].Y - 20;
                        }
                    }
                }
                else if (pointListF.Count > 1)
                {
                    graphicsPath = new XGraphicsPath(pointListF.ToArray(), types, XFillMode.Alternate);
                }
            }
        }

        public void AddLastZeroPoint()
        {
            pointListFZeros.Add(Program.bAreaZeroLine);
            CalculateLength();
        }

        public void AddPoint(PointF pointInMM)
        {
            if (!pointInMM.IsEmpty)
            {
                PointF point = new PointF();

                // We need to convert mm to pt
                point.X = pointInMM.X / DrawingDoc.mmPerPoint;
                point.Y = pointInMM.Y / DrawingDoc.mmPerPoint;

                pointListF.Add(point);
                pointListFZeros.Add(Program.bAreaZeroLine);
                createGraphicsPath();
            }
        }

        public bool RemoveLastPoint()
        {
            if (pointListF.Count > 1)
            {
                firstPoint = new PointF(pointListF[pointListF.Count - 1].X, pointListF[pointListF.Count - 1].Y);
                pointListF.RemoveAt(pointListF.Count - 1);
                pointListFZeros.RemoveAt(pointListFZeros.Count - 1);
                createGraphicsPath();
                return false;
            }
            else
            {
                return true;
            }
        }

        void CalculateArea()
        {
            area = DrawingDoc.Area(pointListF);

            if (area > 0)
                isClockwise = true;
            else
                isClockwise = false;

            area = Math.Abs(area);
        }

        void CalculateLength()
        {
            if (pointListF.Count > 1)
            {
                int i = 0;
                length = 0;

                PointF previousPoint = (PointF)pointListF[pointListF.Count - 1];

                foreach (PointF point in pointListF)
                {
                    if (i > 0)// || (i == 0 && isClosed))
                    {
                        try
                        {
                            if (pointListFZeros[i] == false)
                            {
                                length += DrawingDoc.Distance(previousPoint, point);
                            }
                        }
                        catch { }                        
                    }

                    previousPoint = point;
                    i++;
                }
                if (pointListFZeros.Count > i)
                {
                    if (pointListFZeros[i] == false)
                    {
                        length += DrawingDoc.Distance((PointF)pointListF[0], (PointF)pointListF[pointListF.Count - 1]);
                    }
                }
                else
                {
                    ;// length += DrawingDoc.Distance((PointF)pointListF[0], (PointF)pointListF[pointListF.Count - 1]);
                }
            }
            else
                length = 0;
        }

        //private bool IsClockwise()
        //{
        //    double angleSum = 0.0;

        //    int sides = pointList.Count - 2;

        //    // Set p2 to last point
        //    PointF p1 = (PointF)pointList[pointList.Count - 2];
        //    PointF p2 = (PointF)pointList[pointList.Count - 1];

        //    double angle21 = DrawingDoc.Angle(p2, p1);
        //    double angle23, deltaAngle;

        //    foreach (PointF p3 in pointList)
        //    {
        //        angle23 = DrawingDoc.Angle(p2, p3);
        //        deltaAngle = angle23 - angle21;
        //        angleSum += deltaAngle > 0 ? deltaAngle : Math.PI * 2.0 + deltaAngle;
        //        angle21 = DrawingDoc.Angle(p3, p2);
        //        p2 = p3;
        //    }

        //    return Math.Abs(angleSum - Math.PI * sides) > 0.01;
        //}

        // Properties section
        //

        public PointF LastPoint
        {
            get
            {
                PointF lastPointPt = (PointF)pointListF[pointListF.Count - 1];
                PointF lastPointMM = new PointF();

                // We need to convert pt to mm
                lastPointMM.X = lastPointPt.X * DrawingDoc.mmPerPoint;
                lastPointMM.Y = lastPointPt.Y * DrawingDoc.mmPerPoint;

                return lastPointMM;
            }
        }

        public override bool IsSelectedByPoint(PointF pickPoint)
        {
            return IsPointInPolygon(pickPoint);
        }

        public override bool IsSelectedInRectangle(RectangleF r)
        {
            foreach (PointF cornerPoint in pointListF)
            {
                if (r.Contains(cornerPoint))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool IsValid()
        {
            // Checks if the entity has points in either
            // pointListF or the legacy pointList
            // Updates to pointListF if necesary
            // If neither has points, the entity is from an old beta version,
            // so not it's not valid, and we can't fix it
            if (pointListF == null || pointListF.Count == 0)
            {
                if (pointList != null)
                {
                    pointListF = new List<PointF>((PointF[])pointList.ToArray(typeof(PointF)));
                    pointList = null;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return true;
        }

        private bool IsPointInPolygon(PointF pickPoint)
        {
            bool isInside = false;

            PointF previousPoint = pointListF[pointListF.Count - 1];

            foreach (PointF point in pointListF)
            {
                if (((point.Y > pickPoint.Y) != (previousPoint.Y > pickPoint.Y)) &&
                    (pickPoint.X < (previousPoint.X - point.X) * (pickPoint.Y - point.Y) / (previousPoint.Y - point.Y) + point.X))
                {
                    isInside = !isInside;
                }

                previousPoint = point;
            }

            return isInside;
        }


        // Determine which quadrant point is in with respect to origin
        int WhichQuadrant(PointF origin, PointF point)
        {
            if (point.X < origin.X)
            {
                if (point.Y > origin.Y)
                    return 2;
                else
                    return 1;
            }
            else
            {
                if (point.Y > origin.Y)
                    return 3;
                else
                    return 0;
            }
        }

        public override PointF[] GripPoints()
        {
            PointF[] gripPoints = pointListF.ToArray();

            return gripPoints;
        }

        // Properties
        //
        public override float Area
        {            
            get 
            {
                return (float)area; 
            }
        }

        public override float Perimeter
        {
            get 
            {
                return (float)length; 
            }
        }

        public override ComponentDef Component
        {
            get { return component; }
        }

        public void SetComponent(ComponentDef l_component)
        {
            component = l_component;
        }

        public override EntityType Type
        {
            get { return EntityType.Polygon; }
        }

        public List<PointF> PointList
        {
            get { return pointListF; }
        }

        public int Count
        {
            get { return pointListF.Count; }
        }

        public float GetLastWidthCrop()
        {
            return lastWidthCrop;
        }

        public override int ItemNumber
        {
            get
            {
                if (layer != null)
                    return layer.GetEntityNumber(this);
                else
                    return 0;
            }
        }

        public override Layer Layer
        {
            get { return layer; }
        }

        public override DrawingDoc OwnerDrawing
        {
            get { return base.OwnerDrawing; }
        }

        // Methods
        //
        public override void Render(XGraphics gfx)
        {
            try
            {
                XGraphicsContainer container = gfx.BeginContainer();
                pointFMeasuresFinal = new List<PointF>();
                //If white, then pen black
                XPen pen = new XPen(component.DrawColor, 1);
                XPen penL = new XPen(component.B.GetColor(), (double)component.B.Width);//Color.Red, 2);
                XPen penDashed = new XPen(component.B.GetColor(), (double)component.B.Width);//Color.Red, 2);
                penDashed.DashStyle = XDashStyle.Dash;
                if (component.B.CapType == 0)
                {
                    penL.LineCap = XLineCap.Flat;
                }
                else
                {
                    penL.LineCap = XLineCap.Round;
                }

                // Create a custom dash pattern.
                if (component.B.DashStyle == 0)
                {
                    penL.DashStyle = XDashStyle.Solid;
                }
                else if (component.B.DashStyle == 1)
                {
                    penL.DashStyle = XDashStyle.Dot;
                }
                else if (component.B.DashStyle == 2)
                {
                    penL.DashStyle = XDashStyle.Dash;
                }
                else
                {
                    penL.DashStyle = XDashStyle.DashDot;
                }

                //If zero line, dashed
                /*if (component.bAreaZeroLine)
                {
                    penL.DashStyle = XDashStyle.Dash;
                }*/


                if ((component.DrawColor.R == 255) && (component.DrawColor.G == 255) && (component.DrawColor.B == 255))
                {
                    pen = new XPen(XColor.FromArgb(Color.Black), 1);
                }

                if (graphicsPath == null)
                    createGraphicsPath();

                //Make the DrawColor Solid
                XSolidBrush brush = new XSolidBrush(Color.FromArgb(255, component.DrawColor.R,component.DrawColor.G,component.DrawColor.B));//component.DrawColor);
                //XSolidBrush brushL = new XSolidBrush(Color.Red);


                if (graphicsPath != null)
                {
                    XPdfFontOptions fontOptions = new XPdfFontOptions(PdfFontEmbedding.Always);
                    Font itemFont = new Font(Program.ItemFont.FontFamily, 7f/*Program.ItemFont.Size * 0.7f/*Scale by 0.7*/, GraphicsUnit.World);
                    XFont font = new XFont(itemFont, fontOptions);

                    //if ((component.DrawColor.R == 255) && (component.DrawColor.G == 255) && (component.DrawColor.B == 255))
                    //{
                    //    XColor x = new XColor();
                    //    x.A = 0.5;
                    //    x.R = x.G = x.B = 255;
                    //    brush = new XSolidBrush(x);//50% transparent //Color.White);
                    //    pen.DashStyle = XDashStyle.Dot;
                    //}

                    if ((!this.IsBeingEdited) && (Program.bLineMode == false))
                    {
                        if ((OwnerDrawing.BaseImage != null) && (Program.BackgroundVisible == true))
                        {
                            // Set clipping region to path.
                            OwnerDrawing.BaseImage.GetPartialImage(graphicsPath, gfx, component.DrawColor, component.bRemove);
                            //gfx.IntersectClip(graphicsPath);
                            //gfx.DrawImage(OwnerDrawing.BaseImage.GetPartialImage(graphicsPath), OwnerDrawing.BaseImage.Location.X, OwnerDrawing.BaseImage.Location.Y, OwnerDrawing.BaseImage.Width, OwnerDrawing.BaseImage.Height);
                        }
                        else
                        {
                            gfx.DrawPath(pen, brush, graphicsPath);
                        }
                        //gfx.DrawPath(pen, brush, graphicsPath);
                        brush = new XSolidBrush(XColor.FromArgb(191, 100, 100, 100));

                        if (component.Mode == CalculationMode.Itemize && layer != null)
                        {
                            //XPdfFontOptions fontOptions = new XPdfFontOptions(PdfFontEmbedding.Always);
                            //Font itemFont = new Font(Program.ItemFont.FontFamily, Program.ItemFont.Size, GraphicsUnit.World);
                            //XFont font = new XFont(itemFont, fontOptions);
                            if (isCrop == false)
                            {
                                itemFont = new Font(Program.ItemFont.FontFamily, Program.ItemFont.Size, GraphicsUnit.World);
                                gfx.DrawString(this.ItemNumber.ToString(), font, brush, textLocation);
                            }
                        }
                    }
                    else
                    {
                    //if (graphicsPathLines != null)
                    //{
                        if (Program.bLineMode == false)
                        {
                            //gfx.DrawPath(penL, graphicsPath);
                            for (int i = 0; i < pointListF.Count-1; i++)
                            {
                                XGraphicsPath xG = new XGraphicsPath();
                                xG.AddLine(pointListF[i], pointListF[i+1]);
                                if (pointListFZeros[i])
                                {
                                    gfx.DrawPath(penDashed, xG);
                                }
                                else
                                {
                                    gfx.DrawPath(penL, xG);
                                }
                            }

                            if (pointListF.Count > 0)
                            {
                                float ptPerMm = 0.352778f;
                                PointF[] auxGrips = new PointF[pointListF.Count];
                                int iGrip = 0;

                                foreach (PointF pointG in pointListF)
                                {
                                    auxGrips[iGrip] = new PointF();
                                    auxGrips[iGrip].X = pointG.X * ptPerMm;
                                    auxGrips[iGrip].Y = pointG.Y * ptPerMm;
                                    iGrip++;
                                }
                                this.OwnerDrawing.DrawingForm.GetPageWindow().PlaceGrips(auxGrips);
                            }
                        }
                        else
                        {
                            //gfx.DrawPath(penL, graphicsPath);
                            for (int i = 0; i < pointListF.Count - 1; i++)
                            {
                                XGraphicsPath xG = new XGraphicsPath();
                                xG.AddLine(pointListF[i], pointListF[i + 1]);
                                if (pointListFZeros[i])
                                {
                                    gfx.DrawPath(penDashed, xG);
                                }
                                else
                                {
                                    gfx.DrawPath(penL, xG);
                                }
                            }
                            if (pointListF.Count > 2)
                            {
                                XGraphicsPath xG = new XGraphicsPath();
                                xG.AddLine(pointListF[0], pointListF[pointListF.Count - 1]);
                                if (pointListFZeros[pointListFZeros.Count - 1])
                                {
                                    gfx.DrawPath(penDashed, xG);
                                }
                                else
                                {
                                    gfx.DrawPath(penL, xG);
                                }
                            }
                        }
                    }
                }

                gfx.EndContainer(container);
            }
            catch
            {
                int a2 = 0;
            }


        }

        public void UpdateLastZeroLine()
        {
            try
            {
                pointListFZeros[pointListFZeros.Count - 1] = Program.bAreaZeroLine;
            }
            catch { }
        }

        private SegmentF FindBaseLine()
        {
            SegmentF lowestBaseLine = null;

            if (pointListF.Count > 2)
            {
                PointF p1 = pointListF[pointListF.Count - 1];

                int i = 0;
                foreach (PointF p2 in pointListF)
                {
                    // IsBaseLine determines if this segment is a line on the bottom of the polygon
                    if (IsBaseLine(p1, p2))
                        if (lowestBaseLine == null)
                            lowestBaseLine = (new SegmentF(p1, p2));
                        else
                            if (lowestBaseLine.Point1.Y < p1.Y)
                                lowestBaseLine = (new SegmentF(p1, p2));

                    p1 = p2;
                    i++;
                }
            }

            return lowestBaseLine;
        }

        public bool IsBaseLine(PointF p1, PointF p2)
        {
            // Discard vertical and near vertical lines
            if (Math.Abs((p1.X - p2.X) / (p1.Y - p2.Y)) < 0.5)
                return false;

            if (isClockwise)
                return p1.X > p2.X;
            else
                return p1.X < p2.X;
        }
    }

    [Serializable]
    public class LayerCollection
    {
        List<Layer> layerList;

        public LayerCollection()
        {
            layerList = new List<Layer>();
        }

        public Layer Add(string name)
        {
            Layer newLayer = new Layer(name);

            if (!layerList.Contains(newLayer))
            {
                layerList.Add(newLayer);
                return newLayer;
            }
            else
            {
                return (Layer)layerList[layerList.IndexOf(newLayer)];
            }
        }

        public Layer Get(string name)
        {
            Layer layer = new Layer(name);

            if (layerList.Contains(layer))
            {
                return (Layer)layerList[layerList.IndexOf(layer)];
            }
            else
                return null;
        }

        public List<Layer> List()
        {
            return layerList;
        }

        public int Count()
        {
            return layerList.Count;
        }
    }

    [Serializable]
    public class Layer
    {
        bool visible;
        string name;
        ArrayList entityList;

        // Constructor
        public Layer(string name)
        {
            this.name = name;

            entityList = new ArrayList();

            visible = true;
        }

        // Methods

        // The layer unique id is the name
        public override bool Equals(object obj)
        {
            // If parameter cannot be cast to Layer return false.
            Layer layer = obj as Layer;

            if ((Object)layer == null)
            {
                return false;
            }

            return (name == layer.Name);
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public void AddEntity(DrawingEntity entity)
        {
            entityList.Add(entity);
            entity.Layer = this;
        }

        public int GetEntityNumber(DrawingEntity entity)
        {
            int i = 0;

            foreach (DrawingEntity listedEntity in entityList)
                if (listedEntity.OwnerDrawing != null)
                    if (listedEntity.OwnerDrawing.Entities.Contains(listedEntity))
                    {
                        i++;
                        if (listedEntity == entity)
                            return i;
                    }

            return i + 1;
        }

        public List<DrawingEntity> GetEntities()
        {
            return new List<DrawingEntity>((DrawingEntity[])entityList.ToArray(typeof(DrawingEntity)));
        }

        public void RemoveEntity(DrawingEntity entity)
        {
            entityList.Remove(entity);
        }

        public void RenderEntities(XGraphics gfx)
        {
            if (visible)
            {
                // Draw entities
                foreach (DrawingEntity entity in entityList)
                {
                    entity.Render(gfx);
                }
            }
        }

        // Properties
        public String Name
        {
            get { return name; }
        }
        
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
            }
        }
    }

    [Serializable]
    public class SegmentF
    {
        PointF point1, point2;
        float length;

        // Constructors
        //
        public SegmentF(PointF point1, PointF point2)
        {
            this.point1 = point1;
            this.point2 = point2;
            length = DrawingDoc.Distance(point1, point2);
        }
        //
        // End of Constructors

        // Methods
        public PointF PointAtX(float x)
        {
            // If x is not within the bounds of the segment, no point can be found
            if((x > point1.X && x > point2.X) || (x < point1.X && x < point2.X))
                return PointF.Empty;

            float dx = point1.X - point2.X;
            float dy = point1.Y - point2.Y;
            float slope;

            if (Math.Abs(dx) > 0)
                slope = dy / dx;
            else
                // Line is vertical, so no unique point for x
                return PointF.Empty;

            float y = point1.Y + (x - point1.X) * slope;

            PointF pointAtX = new PointF(x, y);

            return pointAtX;
        }

        // Properties
        //
        public PointF Point1
        {
            get { return point1; }
            set
            {
                point1 = value;
                length = DrawingDoc.Distance(point1, point2);
            }
        }

        public PointF Point2
        {
            get { return point2; }
            set
            {
                point2 = value;
                length = DrawingDoc.Distance(point1, point2);
            }
        }

        float Length
        {
            get { return length; }
        }
        //
        // End of Properties
    }

    [Serializable]
    public class Adjustment
    {
        float value;
        float lowerLimit;

        // Constructor
        //
        public Adjustment()
        {
            value = 0;
        }

        public Adjustment(float value)
        {
            this.value = value;
        }

        // Methods
        //
        public float Plus1()
        {
            return ++value;
        }

        public float Minus1()
        {
            return value = Math.Max(--value, lowerLimit);
        }

        public float Plus10()
        {
            return value += 10;
        }

        public float Minus10()
        {
            return value = Math.Max(value -10, lowerLimit);
        }

        public float Reset()
        {
            value = 0;
            return value;
        }

        // Properties
        //
        public float Value
        {
            get { return value; }
        }

        public float LowerLimit
        {
            get { return lowerLimit; }
            set { lowerLimit = value; }
        }
    }
}

