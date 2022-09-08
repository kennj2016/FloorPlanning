using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

// PDFSharp
using PdfSharp;
using PdfSharp.Drawing;

namespace PdfSharp.Pdf.IO
{
    /// <summary>
    /// uses itextsharp 4.1.6 to convert any pdf to 1.4 compatible pdf, called instead of PdfReader.open
    /// </summary>

    static public class CompatiblePdfReader
    {
        ///// <summary>
        ///// uses itextsharp 4.1.6 to convert any pdf to 1.4 compatible pdf, called instead of PdfReader.open
        ///// </summary>
        //static public PdfDocument Open(string PdfPath, PdfDocumentOpenMode openmode)
        //{
        //    using (FileStream fileStream = new FileStream(PdfPath, FileMode.Open, FileAccess.Read))
        //    {
        //        int len = (int)fileStream.Length;
        //        Byte[] fileArray = new Byte[len];
        //        fileStream.Read(fileArray, 0, len);
        //        fileStream.Close();

        //        return Open(fileArray, openmode);
        //    }
        //}

        ///// <summary>
        ///// uses itextsharp 4.1.6 to convert any pdf to 1.4 compatible pdf, called instead of PdfReader.open
        ///// </summary>
        //static public PdfDocument Open(byte[] fileArray, PdfDocumentOpenMode openmode)
        //{
        //    return Open(new MemoryStream(fileArray), openmode);
        //}

        ///// <summary>
        ///// uses itextsharp 4.1.6 to convert any pdf to 1.4 compatible pdf, called instead of PdfReader.open
        ///// </summary>
        //static public PdfDocument Open(MemoryStream sourceStream, PdfDocumentOpenMode openmode)
        //{
        //    PdfDocument outDoc = null;
        //    sourceStream.Position = 0;

        //    try
        //    {
        //        outDoc = PdfReader.Open(sourceStream, openmode);
        //    }
        //    catch (PdfSharp.Pdf.IO.PdfReaderException)
        //    {
        //        //workaround if pdfsharp doesn't support this pdf
        //        sourceStream.Position = 0;
        //        MemoryStream outputStream = new MemoryStream();
        //        iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(sourceStream);
        //        iTextSharp.text.pdf.PdfStamper pdfStamper = new iTextSharp.text.pdf.PdfStamper(reader, outputStream, '\0', true);
        //        pdfStamper.FormFlattening = true;
        //        pdfStamper.Writer.SetPdfVersion(iTextSharp.text.pdf.PdfWriter.PDF_VERSION_1_4);
        //        pdfStamper.Writer.CloseStream = false;
        //        pdfStamper.Close();

        //        outDoc = PdfReader.Open(outputStream, openmode);
        //    }

        //    return outDoc;
        //}

        static public CompatiblePdfDocument CompatibleOpen(string PdfPath, PdfDocumentOpenMode openmode)
        {
            if(openmode == PdfDocumentOpenMode.InformationOnly)
            {
                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(PdfPath);
                CompatiblePdfDocument outDoc = new CompatiblePdfDocument(reader);
                reader.Close();
                return outDoc;
            }
            else
                return null;
        }
    }

    /// <summary>
    /// CompatiblePdfDocument is really a wrapper around the iTextSharp reader,
    /// that supplies the PdfSharp PdfDocument methods we need.
    /// </summary>
    
    public class CompatiblePdfDocument
    {
        iTextSharp.text.pdf.PdfReader compatibleReader;
        List<CompatiblePdfPage> pages = new List<CompatiblePdfPage>();
        int pageCount;

        public CompatiblePdfDocument(iTextSharp.text.pdf.PdfReader compatibleReader)
        {
            this.compatibleReader = compatibleReader;
            pageCount = compatibleReader.NumberOfPages;

            for (int i = 0; i < pageCount; i++)
                pages.Add(new CompatiblePdfPage(compatibleReader, i + 1));

            return;
        }

        // Properties
        public int PageCount
        {
            get { return pageCount; }
        }

        public List<CompatiblePdfPage> Pages
        {
            get { return pages; }
        }
    }

    public class CompatiblePdfPage
    {
        XUnit width, height;
        int pageNumber;

        public CompatiblePdfPage(iTextSharp.text.pdf.PdfReader compatibleReader, int pageNumber)
        {
            this.pageNumber = pageNumber;

            iTextSharp.text.Rectangle pageInfo = compatibleReader.GetPageSizeWithRotation(pageNumber);

            width = new XUnit(pageInfo.Width);
            height = new XUnit(pageInfo.Height);
        }

        // Properties
        public XUnit Width
        {
            get { return width; }
        }

        public XUnit Height
        {
            get { return height; }
        }
    }
}