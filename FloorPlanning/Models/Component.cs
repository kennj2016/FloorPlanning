using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;


namespace FloorPlanning.Models
{
    [Serializable]
    public class ComponentDef
    {
        string uniqueCode;
        string displayName;
        string displayUnit;
        string installationNotes;
        ComponentType type;
        CalculationMode mode;
        Color drawColor;
        Base b;
        float cornerAdditional;
        string imageFileName;
        public DateTime lastUpdate;
        public short Alpha = 255;
        public bool bRemove = false;
        public bool bAreaZeroLine = false;
        public bool bLineZeroLine = false;

        [NonSerialized]
        bool hasBeenEdited = false;

        [NonSerialized]
        bool imageHasBeenEdited = false;

        public ComponentDef(string code, string name, string unit, ComponentType type, Color color, short l_Alpha,
            float cornerAdditional, CalculationMode mode, Base l_base, bool l_bRemove, bool l_bAreaZeroLine)
        {
            displayName = name;
            displayUnit = unit;
            Alpha = l_Alpha;
            uniqueCode = code;
            drawColor = Color.FromArgb(Alpha, color);//color;
            //perimeterColor = Color.FromArgb(Alpha, l_perimeterColor);
            this.type = type;
            this.cornerAdditional = cornerAdditional;
            this.mode = mode;
            this.B = l_base;
            bRemove = l_bRemove;
            bAreaZeroLine = l_bAreaZeroLine;
            //this.internalOnly = internalOnly;
        }

        /*public ComponentDef(string uniqueCode, string displayName, string imageFileName)
        {
            this.uniqueCode = uniqueCode;
            this.displayName = displayName;
            this.imageFileName = imageFileName;
        }

        public ComponentDef(string uniqueCode)
        {
            this.uniqueCode = uniqueCode;
        }*/

        public ComponentDef()
        {
        }

        public string Key
        {
            get { return uniqueCode; }
        }

        public bool HasBeenEdited
        {
            get { return hasBeenEdited; }
            set
            {
                if (value)
                {
                    hasBeenEdited = true;
                    lastUpdate = DateTime.UtcNow;
                }
            }
        }

        public bool ImageHasBeenEdited
        {
            get { return imageHasBeenEdited; }
            set
            {
                if (value)
                { 
                    imageHasBeenEdited = true;
                    HasBeenEdited = true;
                }
            }
        }

        public string UniqueCode
        {
            get { return uniqueCode; }
            set
            {
                uniqueCode = value;
                HasBeenEdited = true;
            }
        }

        public string DisplayName
        {
            get { return displayName; }
            set
            {
                if (value.Length > 0)
                {
                    displayName = value;
                    HasBeenEdited = true;
                }
            }
        }

        public string ImageFileName
        {
            get { return imageFileName; }
            set
            {
                imageFileName = value;
                ImageHasBeenEdited = true;
            }
        }

        public string InstallationNotes
        {
            get { return installationNotes; }
            set
            {
                installationNotes = value;
                HasBeenEdited = true;
            }
        }

        public string DisplayUnit
        {
            get { return displayUnit; }
            set
            {
                if (value.Length > 0)
                {
                    displayUnit = value;
                    HasBeenEdited = true;
                }
            }
        }

        public ComponentType Type
        {
            get { return type; }
            set
            {
                type = value;
                HasBeenEdited = true;
            }
        }

        public CalculationMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
                HasBeenEdited = true;
            }
        }

        public Color DrawColor
        {
            get { return drawColor; }
            set
            {
                drawColor = value;
                HasBeenEdited = true;
            }
        }

        public Base B
        {
            get { return b; }
            set
            {
                b = value;
                HasBeenEdited = true;
            }
        }

        /*public Color PerimeterColor
        {
            get { return perimeterColor; }
            set
            {
                perimeterColor = value;
                HasBeenEdited = true;
            }
        }*/

        public float CornerAdditional
        {
            get { return cornerAdditional; }
            set
            {
                cornerAdditional = value;
                HasBeenEdited = true;
            }
        }

        public bool IsValid()
        {
            return true;
        }
    }

    public enum ComponentType
    {
        /// <summary>
        /// Perimeter
        /// </summary>
        Perimeter = 0,

        /// <summary>
        /// Area
        /// </summary>
        Area = 1,

    }

    public enum CalculationMode
    {
        /// <summary>
        /// Sum
        /// </summary>
        Sum = 0,

        /// <summary>
        /// Itemize
        /// </summary>
        Itemize = 1,
    }
}

