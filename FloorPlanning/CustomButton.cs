using Infragistics.Win;
using Infragistics.Win.UltraWinDock;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorPlanning
{
    class CustomButton : IUIElementCreationFilter
    {
        public delegate void CustomButtonClickHandler(object sender, CustomButtonArg e);
        public event CustomButtonClickHandler CustomButtonClick;
        Dictionary<DockablePaneBase, ImageAndTextButtonUIElement> panebutton = new Dictionary<DockablePaneBase, ImageAndTextButtonUIElement>();
        Image buttonImage = null;
        string buttonText = string.Empty;

        public string ButtonText
        {
            get { return buttonText; }
            set { buttonText = value; }
        }

        public Image ButtonImage
        {
            get { return buttonImage; }
            set { buttonImage = value; }
        }

        #region IUIElementCreationFilter Members

        public void AfterCreateChildElements(UIElement parent)
        {
            PaneCaptionTextAreaUIElement header = parent as PaneCaptionTextAreaUIElement;
            if (header != null && header.GetDescendant(typeof(ImageAndTextButtonUIElement)) == null)
            {
                ImageAndTextButtonUIElement button = new ImageAndTextButtonUIElement(header);
                Size s = new Size(16, 16);
                Point l = header.Rect.Location;
                if (buttonImage != null)
                    button.Image = buttonImage;
                if (!string.IsNullOrEmpty(buttonText))
                    button.Text = buttonText;
                PaneUIElement p = GetPane(header);
                if (string.IsNullOrEmpty(buttonText))
                {
                    if (p.Pane.Key == "LeftPane")
                    {
                        button.Text = "<";
                        l.Offset(1, 0);
                        button.Rect = new Rectangle(l, s);
                    }
                    else if (p.Pane.Key == "RightPane")
                    {
                        button.Text = ">";
                        l.Offset(header.Rect.Width - 17, 0);
                        button.Rect = new Rectangle(l, s);
                    }
                }
                if (p != null && !panebutton.ContainsKey(p.Pane))
                    panebutton.Add(p.Pane, button);
                else
                    panebutton[p.Pane] = button;
                button.ElementClick += new UIElementEventHandler(button_ElementClick);

                header.ChildElements.Add(button);
            }
        }

        private PaneUIElement GetPane(UIElement el)
        {
            if (el == null)
                return (PaneUIElement)null;
            if (el is PaneUIElement)
                return (PaneUIElement)el;
            else
                return GetPane(el.Parent);
        }

        void button_ElementClick(object sender, UIElementEventArgs e)
        {

            ImageAndTextButtonUIElement button = e.Element as ImageAndTextButtonUIElement;
            DockablePaneBase pane = null;
            foreach (var key in panebutton.Keys)
            {
                if (panebutton[key].Equals(button))
                    pane = key;
            }

            if (CustomButtonClick != null)
                CustomButtonClick.Invoke(button, new CustomButtonArg(pane));
        }

        public bool BeforeCreateChildElements(UIElement parent)
        {
            return false;
        }

        #endregion
    }

    class CustomButtonArg : EventArgs
    {
        DockablePaneBase pane;

        public DockablePaneBase Pane
        {
            get { return pane; }
            set { pane = value; }
        }

        public CustomButtonArg(DockablePaneBase pane)
        {
            this.pane = pane;
        }
    }
}
