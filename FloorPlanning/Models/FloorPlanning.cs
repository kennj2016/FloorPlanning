using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

// PDFSharp
using PdfSharp.Drawing;

namespace FloorPlanning
{
    [Serializable]
    public class FloorPlanningProject
    {
        String name;
        String savePath;
        Dictionary<string, DrawingDoc> drawings;
        public ImageCollection imageEntries;

        [NonSerialized]
        bool noSave;
                
        DateTime createdDate;

        int drawingID;

        public delegate void ChangedEventHandler(FloorPlanningProject sender, EventArgs e);

        // List change notification handler
        [field: NonSerialized] public event ChangedEventHandler Changed;

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }

        // Constructors start
        //
        public FloorPlanningProject()
        {
            name = "Unnamed";
            InitJob();
        }

        public FloorPlanningProject(String jobName)
        {
            name = jobName;

            InitJob();
        }

        private void InitJob()
        {
            createdDate = DateTime.Now;
            
            drawings = new Dictionary<string, DrawingDoc>();
            drawingID = 0;
            

            // Init imageEntries collection
            imageEntries = new ImageCollection();
        }
        //
        // Constructors section end


        // Properties section start
        //
        public string SavePath
        {
            get { return savePath; }
            set
            {
                savePath = value;
            }
        }

        public string FullPath
        {
            get { return Path.Combine(savePath, name + ".fpp"); }
        }

        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                // Eliminate any extra spaces
                string pattern = "\\s{2,}";
                Regex rgx = new Regex(pattern);
                
                name = rgx.Replace(value.Trim(), " ");
            }
        }        

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set
            {
                if (createdDate.Year == 1)
                    createdDate = value;
            }
        }

        public Dictionary<string, DrawingDoc> Drawings
        {
            get { return drawings; }
        }

        //
        // Properties section end
        

        string NewDrawingID()
        {
            drawingID++;
            return drawingID.ToString();
        }

        public void StartSaveGroup()
        {
            noSave = true;
        }

        public void EndSaveGroup()
        {
            noSave = false;
            Save();
        }

        public String AddDrawing(String drawingName, DrawingDoc.DrawingType type)
        {
            DrawingDoc drawing = new DrawingDoc(drawingName, type, this);
            String drawingKey;
            
            try
            {
                drawingKey = NewDrawingID();
                drawings.Add(drawingKey, drawing);
            }
            catch
            {
                // Was unable to add drawing
                return null;
            }

            Save();

            // Trigger event
            //OnChanged(EventArgs.Empty);

            return drawingKey;
        }

        public void DeleteDrawings(string[] keyList)
        {
            foreach (string key in keyList)
            {
                if (drawings[key].DrawingForm == null)
                    drawings.Remove(key);
                else
                    if (drawings[key].DrawingForm.Visible == false)
                        drawings.Remove(key);
                    else
                        MessageBox.Show("Cannot delete drawing while it is open!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Save();

            OnChanged(EventArgs.Empty);
        }

        //public void ExportToPDF(string filename, string[] keyList)
        //{
        //    foreach (string key in keyList)
        //    {
        //        DrawingDoc drawing;
        //        drawings.TryGetValue(key, out drawing);

        //        if (drawing != null)
        //        {

        //        }
        //    }
        //}

        public DrawingDoc DrawingFromKey(String key)
        {
            DrawingDoc drawing;

            drawings.TryGetValue(key, out drawing);

            return drawing;
        }
        
        public bool DrawingDoesExist(String name)
        {
            foreach(DrawingDoc drawing in drawings.Values)
                if(drawing.Name == name)
                    return true;

            return false;
        }

        public void OpenDrawing(String key, string sBaseImagePath = null, float lastWidthCrop = 0, double l_opacity = 1, bool bAutoTitle = false, bool bAutoTitleMargins = false)
        {
            DrawingDoc drawing;

            if (drawings.TryGetValue(key, out drawing))
            {
                //drawing.bAddTitleStamp = bAutoTitle;
                drawing.OpenEditor(sBaseImagePath, lastWidthCrop, l_opacity, bAutoTitle, bAutoTitleMargins);
            }
        }

        public void RenameDrawing(string key, string newName)
        {
            DrawingDoc drawing;

            if (drawings.TryGetValue(key, out drawing))
                drawing.Name = newName;
        }

        public void ListDrawings() 
        {
            foreach (DrawingDoc drawing in drawings.Values)
            {
                ;
            }
        }

        public void CloseDrawings()
        {
            foreach (DrawingDoc drawing in drawings.Values)
                drawing.CloseEditor();
        }

        public bool Save()
        {
            // Opens a file and serializes the job into binary format file with .fpp extension.

            // Don't save if in a save group
            if (noSave)
                return true;

            try
            {
                Stream stream = File.Open(Path.Combine(this.SavePath, name + "_temp.fpp2"), FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, this);
                stream.Close();
                if (!File.Exists(Path.Combine(this.SavePath, name + ".fpp")))
                {
                    SaveSame();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveSame()
        {
            try
            {
                string sFileOrg = Path.Combine(this.SavePath, name + "_temp.fpp2");
                string sFileDst = Path.Combine(this.SavePath, name + ".fpp");
                File.Copy(sFileOrg, sFileDst, true);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool SaveAs(string l_Path, string l_Name)
        {
            try
            {
                string sFileOrg = Path.Combine(this.SavePath, name + "_temp.fpp2");
                this.SavePath = l_Path;
                name = l_Name;
                string sFileDst = Path.Combine(this.SavePath, name + ".fpp");
                File.Copy(sFileOrg, sFileDst, true);
                //Now the temp
                sFileDst = Path.Combine(this.SavePath, name + "_temp.fpp2");
                File.Copy(sFileOrg, sFileDst, true);
                Save();
            }
            catch
            {
                return false;
            }
            return true;
        }
        //
        // Methods section end
    }

    [Serializable]
    public class ImageEntry
    {
        string fileName;
        int referenceCount = 0;
        
        [NonSerialized]
        XImage image = null;

        [NonSerialized]
        string filePath;

        // displayCount is for memory management.
        // If an image is not needed for any currently open window,
        // its useCount should be zero
        // It is NOT a count of object references.
        [NonSerialized]
        int displayCount = 0;

        public ImageEntry(string fileName)
        {
            this.fileName = fileName;
            IncrementReferenceCount();
        }

        public XImage Image
        {
            get
            {
                if (image == null && filePath != null)
                    image = XImage.FromGdiPlusImage(DrawingDoc.loadImage(filePath));

                if (image.Format == null)
                    return image = null;
                else
                    return image;
            }
        }

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public int UseCount
        {
            get { return displayCount; }
        }

        public void IncrementDisplayCount()
        {
            displayCount++;
        }

        public void DecrementDisplayCount()
        {
            displayCount--;

            if(displayCount == 0 && image != null)
            {
                image.Dispose();
                image = null;
            }

        }

        public int ReferenceCount
        {
            get { return referenceCount; }
        }

        public void IncrementReferenceCount()
        {
            referenceCount++;
        }

        public void DecrementReferenceCount()
        {
            referenceCount--;
        }

        ~ImageEntry()
        {
            if (image != null)
            {
                image.Dispose();
                image = null;
            }
        }
    }

    [Serializable]
    public class ImageCollection
    {
        Dictionary<string, ImageEntry> imageEntryList;

        public ImageCollection()
        {
            imageEntryList = new Dictionary<string, ImageEntry>();
        }

        public ImageEntry Add(string newFileName)
        {
            if (imageEntryList.ContainsKey(newFileName))
            {
                imageEntryList[newFileName].IncrementReferenceCount();
                return imageEntryList[newFileName];
            }
            else
            {
                ImageEntry newImageEntry = new ImageEntry(newFileName);
                imageEntryList.Add(newFileName, newImageEntry);
                return newImageEntry;
            }
        }

        public bool Remove(string fileName)
        {
            if (imageEntryList.ContainsKey(fileName))
            {
                imageEntryList[fileName].DecrementReferenceCount();

                // If no longer referenced, remove the imageEntry
                if (imageEntryList[fileName].ReferenceCount == 0)
                    imageEntryList.Remove(fileName);

                return true;
            }
            else
            {
                return false;
            }
        }

        public ImageEntry Get(string fileName)
        {
            if (imageEntryList.ContainsKey(fileName))
            {
                return imageEntryList[fileName];
            }
            else
                return null;
        }

        public List<ImageEntry> List()
        {
            return imageEntryList.Values.ToList();
        }

        public int Count()
        {
            return imageEntryList.Count;
        }
    }
}
