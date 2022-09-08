using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FloorPlanning.Models
{
    [Serializable]
    public class Finish
    {
        public int nIndex { get; set; }
        public string sName { get; set; }
        public int A { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
    }

    [Serializable]
    public class Base
    {
        public int nIndex { get; set; } 
        public string sName { get; set; }
        public int A { get; set; }
        public int R { get; set; }
        public int G { get; set; } 
        public int B { get; set; }
        public int Width { get; set; }
        public int DashType { get; set; }
        public int CapType { get; set; }
        public int DashStyle { get; set; }

        public Base()
        {
        }

        public Base(int l_nIndex, string l_sName, int l_A, int l_R, int l_G, int l_B, int l_Width, int l_DashType, int l_CapType, int l_DashStyle)
        {
            nIndex = l_nIndex;
            sName = l_sName;
            A = l_A;
            R = l_R;
            G = l_G;
            B = l_B;
            Width = l_Width;
            DashType = l_DashType;
            CapType = l_CapType;
            DashStyle = l_DashStyle;
        }

        public Base(int l_nIndex, string l_sName, int l_A, Color c, int l_Width, int l_DashType, int l_CapType, int l_DashStyle)
        {
            nIndex = l_nIndex;
            sName = l_sName;
            A = l_A;
            R = c.R;
            G = c.G;
            B = c.B;
            Width = l_Width;
            DashType = l_DashType;
            CapType = l_CapType;
            DashStyle = l_DashStyle;
        }

        public Color GetColor()
        {
            return Color.FromArgb(255, this.R, this.G, this.B);
        }
    }

    [Serializable]
    public class ListFinishLine
    {
        public List<Finish> lFinish { get; set; }
        public List<Base> lLine { get; set; }
        //Kiet Nguyen:[1500] list shortcuts
        public List<Shortcuts> lShortcuts { get; set; }
        //Kiet Nguyen:[Sheet 400] list scales
        public List<Scale> lScales { get; set; }

        public ListFinishLine()
        {
            lFinish = new List<Finish>();
            lLine = new List<Base>();
            lShortcuts = new List<Shortcuts>();
            lScales = new List<Scale>();
        }
    }
}
