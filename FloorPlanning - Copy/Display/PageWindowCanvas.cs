using System;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;

// GDI
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using FloorPlanning.Display.enums;

namespace FloorPlanning.Display
{
    /// <summary>
    /// Implements the control that shows the page.
    /// </summary>
    public class PageWindowCanvas : System.Windows.Forms.Control
    {
        public PageWindowCanvas(PageWindow window)
        {
            this.window = window;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
        }
        PageWindow window;

        protected override void OnPaint(PaintEventArgs e) 
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            
            if (!this.window.showPage)
                return;

            if (window.CanvasIsCached)
            {
                window.DisplayTracking(e.Graphics);
                return;
            }
            else
            {
                if (this.window.IsDragging)
                {
                    window.SetCacheFromRender();
                    base.OnPaintBackground(e);
                    window.DisplayTracking(e.Graphics);
                }
                else
                {
                    window.RenderPage(e.Graphics);
                    if(window.GripPoints != null)
                        window.DisplayTracking(e.Graphics);
                }
            }

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                if (this.window.CanvasIsCached)
                {
                    base.OnPaintBackground(e);
                    return;
                }

                if (!this.window.showPage)
                {
                    e.Graphics.Clear(this.window.desktopColor);
                    return;
                }

                this.window.PaintBackground(e.Graphics);
            }
            catch (Exception ex)
            {
                ;// MessageBox.Show(ex.ToString());
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            Invalidate();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            return;//ADDED MAG
            
        }
    }
}