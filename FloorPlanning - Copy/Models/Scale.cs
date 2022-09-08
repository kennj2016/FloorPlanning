using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorPlanning.Models
{
    //Kiet Nguyen:[Sheet 400] list scale
    [Serializable]
    public class Scale
    {
        public string Value { set; get; }

        public string DisplayText { set; get; }
        public string Feet { set; get; }
        public string Inches { set; get; }
    }
}
