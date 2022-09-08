using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorPlanning.Models
{
    //Kiet Nguyen:[1500] list shortcuts
    [Serializable]
    public class Shortcuts
    {
        public string Area { set; get; }
        public char Keystroke { set; get; }
        public string Action { set; get; }

        public string DisplayText { set; get; }
    }
}
