using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using Infragistics.Win;
using Infragistics.Win.UltraWinToolbars;
using System.Drawing;

namespace FloorPlanning
{
    class CRightGroup : IUIElementCreationFilter
    {
        public void AfterCreateChildElements(UIElement parent)
        {

        }

        public bool BeforeCreateChildElements(UIElement parent)
        {
            if (parent is RibbonGroupUIElement && parent.Parent.ChildElements.LastIndexOf(parent) == parent.Parent.ChildElements.Count - 1)
            {

                Point loc = new Point(parent.Parent.Parent.Rect.Right - parent.Rect.Width - 3, parent.Rect.Location.Y);
                int xOffset = loc.X - parent.Rect.Location.X;
                parent.Rect = new Rectangle(loc, parent.Rect.Size);


            }
            if (parent is RibbonGroupAreaUIElement)
            {
                Size s = new Size(parent.Parent.Rect.Width - 2, parent.Rect.Height);
                parent.Rect = new Rectangle(parent.Rect.Location, s);

            }
            return false;
        }
    }
}

