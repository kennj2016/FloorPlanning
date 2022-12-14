#region PDFsharp - A .NET library for processing PDF
//
// Copyright (c) 2005-2009 empira Software GmbH, Cologne (Germany)
//
// http://www.pdfsharp.com
//
// http://sourceforge.net/projects/pdfsharp
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT OF THIRD PARTY RIGHTS.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
// USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.IO;

// PDFSharp
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;

// FloorPlanning
using FloorPlanning.Display;
using FloorPlanning.Display.enums;
using System.Collections.Generic;
using System.Drawing.Drawing2D;


namespace FloorPlanning
{
    /// <summary>
    /// Drawing Editor Form
    /// </summary>
    partial class DrawingForm
    {
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private DrawingDoc drawingDoc;

        //private PdfSharp.Forms.PagePreview pagePreview;
        private CommandMode currentCommand;


        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.OptionSet optionSet1 = new Infragistics.Win.UltraWinToolbars.OptionSet("areaoption");
            Infragistics.Win.UltraWinToolbars.OptionSet optionSet2 = new Infragistics.Win.UltraWinToolbars.OptionSet("panoption");
            Infragistics.Win.UltraWinToolbars.OptionSet optionSet3 = new Infragistics.Win.UltraWinToolbars.OptionSet("showimageoption");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("ProjectGroup");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool43 = new Infragistics.Win.UltraWinToolbars.ButtonTool("New");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool58 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Open");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool65 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool66 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaveAs");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool67 = new Infragistics.Win.UltraWinToolbars.ButtonTool("LoadPDF");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool68 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReloadPDF");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool3 = new Infragistics.Win.UltraWinToolbars.LabelTool("ReportsGroup");
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ContextualTabGroup contextualTabGroup1 = new Infragistics.Win.UltraWinToolbars.ContextualTabGroup("contextualTabGroup1");
            Infragistics.Win.UltraWinToolbars.ContextualTabGroup contextualTabGroup2 = new Infragistics.Win.UltraWinToolbars.ContextualTabGroup("contextualTabGroup2");
            Infragistics.Win.UltraWinToolbars.RibbonTab ribbonTab1 = new Infragistics.Win.UltraWinToolbars.RibbonTab("Menu");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup1 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("File");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool37 = new Infragistics.Win.UltraWinToolbars.ButtonTool("New");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Open");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaveAs");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("LoadPDF");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup2 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("Zoom");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ZoomIn");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ZoomOut");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FitWidth");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FitHeight");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ZoomPercent");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup3 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("Canvas Mode");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("PanMode", "panoption");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool3 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("DrawMode", "panoption");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool9 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Page Mode", "");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup4 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("Draw Mode");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool5 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("AreaMode", "areaoption");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool7 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("LineMode", "areaoption");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup5 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("Filters");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FilterAreas");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FilterLines");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup6 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("Show Areas");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ShowAreas");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("HideAreas");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = new Infragistics.Win.UltraWinToolbars.ButtonTool("GreyAreas");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup7 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("Show image");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool12 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ShowImage", "showimageoption");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool14 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("HideImage", "showimageoption");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup8 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("Edit finishes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool33 = new Infragistics.Win.UltraWinToolbars.ButtonTool("EditArea");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool34 = new Infragistics.Win.UltraWinToolbars.ButtonTool("EditLines");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup9 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("Scaling");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool47 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CustomScale");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool48 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MeasureLine");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup10 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("Misc");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool51 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AligntoGrid");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool52 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Counters");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool55 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SelectColors");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup11 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("Settings");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ShowSettings");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup12 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("ribbonGroup1");
            Infragistics.Win.UltraWinToolbars.RibbonTab ribbonTab2 = new Infragistics.Win.UltraWinToolbars.RibbonTab("ribbon1");
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Open");
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save");
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaveAs");
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("LoadPDF");
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ZoomIn");
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ZoomOut");
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FitWidth");
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FitHeight");
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ZoomPercent");
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("PanMode", "panoption");
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool4 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("DrawMode", "panoption");
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool6 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("AreaMode", "areaoption");
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool8 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("LineMode", "areaoption");
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FilterAreas");
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FilterLines");
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool26 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ShowAreas");
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool27 = new Infragistics.Win.UltraWinToolbars.ButtonTool("HideAreas");
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool28 = new Infragistics.Win.UltraWinToolbars.ButtonTool("GreyAreas");
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool35 = new Infragistics.Win.UltraWinToolbars.ButtonTool("EditArea");
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool36 = new Infragistics.Win.UltraWinToolbars.ButtonTool("EditLines");
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool40 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AreaSettings");
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool41 = new Infragistics.Win.UltraWinToolbars.ButtonTool("LineSettings");
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool42 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SeamSettings");
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool45 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AreaEditSettings");
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool46 = new Infragistics.Win.UltraWinToolbars.ButtonTool("LineEditSettings");
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool49 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CustomScale");
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool50 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MeasureLine");
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool53 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AligntoGrid");
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool54 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Counters");
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool56 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SelectColors");
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool38 = new Infragistics.Win.UltraWinToolbars.ButtonTool("New");
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance90 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ShowSettings");
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool3 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("PopupMenuTool1");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool5 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("EditFinishSettings");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool29 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Shortcuts_mnu");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool44 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool1");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool57 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool2");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool4 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("PopupMenuTool1");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool59 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AreaSettings");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool60 = new Infragistics.Win.UltraWinToolbars.ButtonTool("LineSettings");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool61 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SeamSettings");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool6 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("EditFinishSettings");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool62 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AreaEditSettings");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool63 = new Infragistics.Win.UltraWinToolbars.ButtonTool("LineEditSettings");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool10 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("PageMode", "panoption");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool11 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Page Mode", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool13 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ShowImage", "showimageoption");
            Infragistics.Win.Appearance appearance93 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance94 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool15 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("HideImage", "showimageoption");
            Infragistics.Win.Appearance appearance95 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance96 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("ProjectGroup");
            Infragistics.Win.Appearance appearance97 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool69 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReloadPDF");
            Infragistics.Win.Appearance appearance98 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool4 = new Infragistics.Win.UltraWinToolbars.LabelTool("ReportsGroup");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool30 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool3");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool31 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Shortcuts");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool6 = new Infragistics.Win.UltraWinToolbars.LabelTool("LabelTool1");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool32 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool4");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool17 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("StateButtonTool1", "");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool7 = new Infragistics.Win.UltraWinToolbars.LabelTool("LabelTool2");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool8 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("PopupMenuTool2");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool39 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Shortcuts_mnu");
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedBottom, new System.Guid("955c2a34-b363-49bc-879c-39c83105a59b"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("c94b10b0-b57a-449f-b6ea-8437f677c1b2"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("955c2a34-b363-49bc-879c-39c83105a59b"), -1);
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane2 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("cb996483-00b0-43d9-8409-11f2c2b1bed4"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane2 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("5e39669f-be1c-4c01-bbc5-33ab72f18753"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("cb996483-00b0-43d9-8409-11f2c2b1bed4"), -1);
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane3 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedRight, new System.Guid("350ad495-80c8-47a9-b577-26feb9195940"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane3 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("de31e619-7e85-48ea-bcbf-c48cce85ca9b"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("350ad495-80c8-47a9-b577-26feb9195940"), -1);
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawingForm));
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.lblPageNumber = new Infragistics.Win.Misc.UltraLabel();
            this.pnlTabUp = new Infragistics.Win.Misc.UltraPanel();
            this.pctPage = new System.Windows.Forms.PictureBox();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraPanelAreaTab = new Infragistics.Win.Misc.UltraPanel();
            this.txtTransparencyPercent = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.label3 = new System.Windows.Forms.Label();
            this.barTransparency = new System.Windows.Forms.TrackBar();
            this.lblAreaPoint = new System.Windows.Forms.Label();
            this.lblLastWasteWhole = new System.Windows.Forms.Label();
            this.lblAreaText = new System.Windows.Forms.Label();
            this.lblLastWasteDecimal = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblLastAreaDecimal = new System.Windows.Forms.Label();
            this.lblLastAreaWhole = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblLastPerimeterDecimal = new System.Windows.Forms.Label();
            this.lblLastPerimeterWhole = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblLastAreaTotalWhole = new System.Windows.Forms.Label();
            this.lblLastAreaTotalDecimal = new System.Windows.Forms.Label();
            this.lblLastTotalPerimeterDecimal = new System.Windows.Forms.Label();
            this.lblLastTotalPerimeterWhole = new System.Windows.Forms.Label();
            this.pnlAreaTab = new Infragistics.Win.Misc.UltraPanel();
            this.grpAreaTab = new Infragistics.Win.Misc.UltraGroupBox();
            this.pnlFinishUCs = new Infragistics.Win.Misc.UltraPanel();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraPanelLineTab = new Infragistics.Win.Misc.UltraPanel();
            this.pnlLineProps = new Infragistics.Win.Misc.UltraPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblLastLineWhole = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblLastLineDecimal = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.btnEditLine = new Infragistics.Win.Misc.UltraButton();
            this.lblTotalLineDecimal = new System.Windows.Forms.Label();
            this.lblTotalLineWhole = new System.Windows.Forms.Label();
            this.pnlLineTab = new Infragistics.Win.Misc.UltraPanel();
            this.grpLineTab = new Infragistics.Win.Misc.UltraGroupBox();
            this.pnlLineUCs = new Infragistics.Win.Misc.UltraPanel();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusStripPrompt = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripZoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPanes = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraPanelRight = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraGroupBox5 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraButton6 = new Infragistics.Win.Misc.UltraButton();
            this.rdNoX1Y1 = new System.Windows.Forms.RadioButton();
            this.rdYesX1Y1 = new System.Windows.Forms.RadioButton();
            this.ultraGroupBox4 = new Infragistics.Win.Misc.UltraGroupBox();
            this.cmbScale = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.chkScale = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.grpCustom = new Infragistics.Win.Misc.UltraGroupBox();
            this.lbClick2NdPoint = new System.Windows.Forms.Label();
            this.lbClick1StPoint = new System.Windows.Forms.Label();
            this.txtScaleInches = new System.Windows.Forms.TextBox();
            this.txtScaleFeet = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnScaleOk = new Infragistics.Win.Misc.UltraButton();
            this.btnScaleReset = new Infragistics.Win.Misc.UltraButton();
            this.btnScaleCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnScaleNew = new Infragistics.Win.Misc.UltraButton();
            this.grpTrim = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbTrimFactor = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraPanelBackRight = new Infragistics.Win.Misc.UltraPanel();
            this.pnlAreaModeInRight = new Infragistics.Win.Misc.UltraPanel();
            this.grpAreaInRight = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnAreaZeroLine = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton4 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton3 = new Infragistics.Win.Misc.UltraButton();
            this.btnAreaTakeOut = new Infragistics.Win.Misc.UltraButton();
            this.btnComplete = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.ultraGroupBox3 = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblSelectedArea = new Infragistics.Win.Misc.UltraLabel();
            this.btnEmptyShape = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton2 = new Infragistics.Win.Misc.UltraButton();
            this.pnlLineModeRightPanel = new Infragistics.Win.Misc.UltraPanel();
            this.grpLineInRight = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraGroupBox8 = new Infragistics.Win.Misc.UltraGroupBox();
            this.grpDoorTakeOut = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnDeactivateLineDoorTO = new Infragistics.Win.Misc.UltraButton();
            this.btnShowLineDoorTO = new Infragistics.Win.Misc.UltraButton();
            this.btnActivateLineDoorTO = new Infragistics.Win.Misc.UltraButton();
            this.btnHideLineDoorTO = new Infragistics.Win.Misc.UltraButton();
            this.txtOtherFt = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.rdOtherft = new System.Windows.Forms.RadioButton();
            this.rd6f = new System.Windows.Forms.RadioButton();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.rd3f = new System.Windows.Forms.RadioButton();
            this.ultraGroupBox9 = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnLineZeroLine = new Infragistics.Win.Misc.UltraButton();
            this.btnDuplicate = new Infragistics.Win.Misc.UltraButton();
            this.btnJump = new Infragistics.Win.Misc.UltraButton();
            this.btn2x = new Infragistics.Win.Misc.UltraButton();
            this.btn1x = new Infragistics.Win.Misc.UltraButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._DrawingForm_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._DrawingForm_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._DrawingForm_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._DrawingForm_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.ultraDockManager1 = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._DrawingFormUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DrawingFormUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DrawingFormUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DrawingFormUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DrawingFormAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.dockableWindow3 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.windowDockingArea1 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.dockableWindow4 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.btnLeft = new Infragistics.Win.Misc.UltraButton();
            this.btnRight = new Infragistics.Win.Misc.UltraButton();
            this.windowDockingArea2 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.windowDockingArea3 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.pageWindow = new FloorPlanning.Display.PageWindow();
            this.ultraTabPageControl1.SuspendLayout();
            this.pnlTabUp.ClientArea.SuspendLayout();
            this.pnlTabUp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctPage)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            this.ultraPanelAreaTab.ClientArea.SuspendLayout();
            this.ultraPanelAreaTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransparencyPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barTransparency)).BeginInit();
            this.pnlAreaTab.ClientArea.SuspendLayout();
            this.pnlAreaTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpAreaTab)).BeginInit();
            this.grpAreaTab.SuspendLayout();
            this.pnlFinishUCs.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            this.ultraPanelLineTab.ClientArea.SuspendLayout();
            this.ultraPanelLineTab.SuspendLayout();
            this.pnlLineProps.SuspendLayout();
            this.pnlLineTab.ClientArea.SuspendLayout();
            this.pnlLineTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpLineTab)).BeginInit();
            this.grpLineTab.SuspendLayout();
            this.pnlLineUCs.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabPanes)).BeginInit();
            this.tabPanes.SuspendLayout();
            this.ultraPanelRight.ClientArea.SuspendLayout();
            this.ultraPanelRight.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox5)).BeginInit();
            this.ultraGroupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox4)).BeginInit();
            this.ultraGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCustom)).BeginInit();
            this.grpCustom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpTrim)).BeginInit();
            this.grpTrim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrimFactor)).BeginInit();
            this.ultraPanelBackRight.ClientArea.SuspendLayout();
            this.ultraPanelBackRight.SuspendLayout();
            this.pnlAreaModeInRight.ClientArea.SuspendLayout();
            this.pnlAreaModeInRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpAreaInRight)).BeginInit();
            this.grpAreaInRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).BeginInit();
            this.ultraGroupBox3.SuspendLayout();
            this.pnlLineModeRightPanel.ClientArea.SuspendLayout();
            this.pnlLineModeRightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpLineInRight)).BeginInit();
            this.grpLineInRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox8)).BeginInit();
            this.ultraGroupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpDoorTakeOut)).BeginInit();
            this.grpDoorTakeOut.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOtherFt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox9)).BeginInit();
            this.ultraGroupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).BeginInit();
            this.dockableWindow3.SuspendLayout();
            this.dockableWindow1.SuspendLayout();
            this.windowDockingArea1.SuspendLayout();
            this.dockableWindow4.SuspendLayout();
            this.windowDockingArea2.SuspendLayout();
            this.windowDockingArea3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.lblPageNumber);
            this.ultraTabPageControl1.Controls.Add(this.pnlTabUp);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(247, 666);
            // 
            // lblPageNumber
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.lblPageNumber.Appearance = appearance1;
            this.lblPageNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPageNumber.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageNumber.Location = new System.Drawing.Point(0, 198);
            this.lblPageNumber.Name = "lblPageNumber";
            this.lblPageNumber.Size = new System.Drawing.Size(247, 23);
            this.lblPageNumber.TabIndex = 0;
            this.lblPageNumber.Text = "Page 1";
            this.lblPageNumber.Visible = false;
            // 
            // pnlTabUp
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.pnlTabUp.Appearance = appearance2;
            // 
            // pnlTabUp.ClientArea
            // 
            this.pnlTabUp.ClientArea.Controls.Add(this.pctPage);
            this.pnlTabUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTabUp.Location = new System.Drawing.Point(0, 0);
            this.pnlTabUp.Name = "pnlTabUp";
            this.pnlTabUp.Size = new System.Drawing.Size(247, 198);
            this.pnlTabUp.TabIndex = 1;
            // 
            // pctPage
            // 
            this.pctPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pctPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pctPage.Location = new System.Drawing.Point(0, 0);
            this.pctPage.Margin = new System.Windows.Forms.Padding(30);
            this.pctPage.Name = "pctPage";
            this.pctPage.Padding = new System.Windows.Forms.Padding(10);
            this.pctPage.Size = new System.Drawing.Size(247, 198);
            this.pctPage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pctPage.TabIndex = 0;
            this.pctPage.TabStop = false;
            this.pctPage.Visible = false;
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.ultraPanelAreaTab);
            this.ultraTabPageControl2.Controls.Add(this.pnlAreaTab);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(247, 666);
            // 
            // ultraPanelAreaTab
            // 
            appearance3.BackColor = System.Drawing.Color.White;
            this.ultraPanelAreaTab.Appearance = appearance3;
            // 
            // ultraPanelAreaTab.ClientArea
            // 
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.txtTransparencyPercent);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.label3);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.barTransparency);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.lblAreaPoint);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.lblLastWasteWhole);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.lblAreaText);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.lblLastWasteDecimal);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.label9);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.label8);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.label5);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.label4);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.lblLastAreaDecimal);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.lblLastAreaWhole);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.label16);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.label13);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.lblLastPerimeterDecimal);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.lblLastPerimeterWhole);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.label1);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.label12);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.lblLastAreaTotalWhole);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.lblLastAreaTotalDecimal);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.lblLastTotalPerimeterDecimal);
            this.ultraPanelAreaTab.ClientArea.Controls.Add(this.lblLastTotalPerimeterWhole);
            this.ultraPanelAreaTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanelAreaTab.Location = new System.Drawing.Point(0, 413);
            this.ultraPanelAreaTab.Name = "ultraPanelAreaTab";
            this.ultraPanelAreaTab.Size = new System.Drawing.Size(247, 253);
            this.ultraPanelAreaTab.TabIndex = 15;
            // 
            // txtTransparencyPercent
            // 
            this.txtTransparencyPercent.Enabled = false;
            this.txtTransparencyPercent.Location = new System.Drawing.Point(174, 143);
            this.txtTransparencyPercent.Name = "txtTransparencyPercent";
            this.txtTransparencyPercent.ReadOnly = true;
            this.txtTransparencyPercent.Size = new System.Drawing.Size(61, 24);
            this.txtTransparencyPercent.TabIndex = 41;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 15);
            this.label3.TabIndex = 40;
            this.label3.Text = "Transparency";
            // 
            // barTransparency
            // 
            this.barTransparency.Location = new System.Drawing.Point(4, 166);
            this.barTransparency.Maximum = 255;
            this.barTransparency.Minimum = 1;
            this.barTransparency.Name = "barTransparency";
            this.barTransparency.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.barTransparency.Size = new System.Drawing.Size(238, 45);
            this.barTransparency.TabIndex = 39;
            this.barTransparency.TickStyle = System.Windows.Forms.TickStyle.None;
            this.barTransparency.Value = 255;
            this.barTransparency.ValueChanged += new System.EventHandler(this.barTransparency_ValueChanged);
            // 
            // lblAreaPoint
            // 
            this.lblAreaPoint.BackColor = System.Drawing.Color.Black;
            this.lblAreaPoint.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblAreaPoint.Location = new System.Drawing.Point(154, 35);
            this.lblAreaPoint.Name = "lblAreaPoint";
            this.lblAreaPoint.Size = new System.Drawing.Size(1, 1);
            this.lblAreaPoint.TabIndex = 14;
            this.lblAreaPoint.Text = ".";
            // 
            // lblLastWasteWhole
            // 
            this.lblLastWasteWhole.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblLastWasteWhole.Location = new System.Drawing.Point(112, 24);
            this.lblLastWasteWhole.Name = "lblLastWasteWhole";
            this.lblLastWasteWhole.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblLastWasteWhole.Size = new System.Drawing.Size(45, 13);
            this.lblLastWasteWhole.TabIndex = 12;
            this.lblLastWasteWhole.Text = "0";
            // 
            // lblAreaText
            // 
            this.lblAreaText.AutoSize = true;
            this.lblAreaText.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblAreaText.Location = new System.Drawing.Point(12, 24);
            this.lblAreaText.Name = "lblAreaText";
            this.lblAreaText.Size = new System.Drawing.Size(100, 13);
            this.lblAreaText.TabIndex = 11;
            this.lblAreaText.Text = "Last Waste Factor:";
            // 
            // lblLastWasteDecimal
            // 
            this.lblLastWasteDecimal.AutoSize = true;
            this.lblLastWasteDecimal.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblLastWasteDecimal.Location = new System.Drawing.Point(154, 24);
            this.lblLastWasteDecimal.Name = "lblLastWasteDecimal";
            this.lblLastWasteDecimal.Size = new System.Drawing.Size(22, 13);
            this.lblLastWasteDecimal.TabIndex = 13;
            this.lblLastWasteDecimal.Text = "0%";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Black;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label9.Location = new System.Drawing.Point(154, 81);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(1, 1);
            this.label9.TabIndex = 22;
            this.label9.Text = ".";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label8.Location = new System.Drawing.Point(12, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Total Area:";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Black;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label5.Location = new System.Drawing.Point(154, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1, 1);
            this.label5.TabIndex = 26;
            this.label5.Text = ".";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label4.Location = new System.Drawing.Point(12, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Last Area:";
            // 
            // lblLastAreaDecimal
            // 
            this.lblLastAreaDecimal.AutoSize = true;
            this.lblLastAreaDecimal.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblLastAreaDecimal.Location = new System.Drawing.Point(154, 47);
            this.lblLastAreaDecimal.Name = "lblLastAreaDecimal";
            this.lblLastAreaDecimal.Size = new System.Drawing.Size(13, 13);
            this.lblLastAreaDecimal.TabIndex = 17;
            this.lblLastAreaDecimal.Text = "0";
            // 
            // lblLastAreaWhole
            // 
            this.lblLastAreaWhole.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblLastAreaWhole.Location = new System.Drawing.Point(112, 47);
            this.lblLastAreaWhole.Name = "lblLastAreaWhole";
            this.lblLastAreaWhole.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblLastAreaWhole.Size = new System.Drawing.Size(45, 13);
            this.lblLastAreaWhole.TabIndex = 16;
            this.lblLastAreaWhole.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label16.Location = new System.Drawing.Point(12, 116);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(86, 13);
            this.label16.TabIndex = 27;
            this.label16.Text = "Total Perimeter:";
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Black;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label13.Location = new System.Drawing.Point(154, 127);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(1, 1);
            this.label13.TabIndex = 30;
            this.label13.Text = ".";
            // 
            // lblLastPerimeterDecimal
            // 
            this.lblLastPerimeterDecimal.AutoSize = true;
            this.lblLastPerimeterDecimal.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblLastPerimeterDecimal.Location = new System.Drawing.Point(154, 70);
            this.lblLastPerimeterDecimal.Name = "lblLastPerimeterDecimal";
            this.lblLastPerimeterDecimal.Size = new System.Drawing.Size(13, 13);
            this.lblLastPerimeterDecimal.TabIndex = 21;
            this.lblLastPerimeterDecimal.Text = "0";
            // 
            // lblLastPerimeterWhole
            // 
            this.lblLastPerimeterWhole.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblLastPerimeterWhole.Location = new System.Drawing.Point(112, 70);
            this.lblLastPerimeterWhole.Name = "lblLastPerimeterWhole";
            this.lblLastPerimeterWhole.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblLastPerimeterWhole.Size = new System.Drawing.Size(45, 13);
            this.lblLastPerimeterWhole.TabIndex = 20;
            this.lblLastPerimeterWhole.Text = "0";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label1.Location = new System.Drawing.Point(154, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1, 1);
            this.label1.TabIndex = 18;
            this.label1.Text = ".";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label12.Location = new System.Drawing.Point(12, 70);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "Last Perimeter:";
            // 
            // lblLastAreaTotalWhole
            // 
            this.lblLastAreaTotalWhole.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblLastAreaTotalWhole.Location = new System.Drawing.Point(112, 93);
            this.lblLastAreaTotalWhole.Name = "lblLastAreaTotalWhole";
            this.lblLastAreaTotalWhole.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblLastAreaTotalWhole.Size = new System.Drawing.Size(45, 13);
            this.lblLastAreaTotalWhole.TabIndex = 24;
            this.lblLastAreaTotalWhole.Text = "0";
            // 
            // lblLastAreaTotalDecimal
            // 
            this.lblLastAreaTotalDecimal.AutoSize = true;
            this.lblLastAreaTotalDecimal.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblLastAreaTotalDecimal.Location = new System.Drawing.Point(154, 93);
            this.lblLastAreaTotalDecimal.Name = "lblLastAreaTotalDecimal";
            this.lblLastAreaTotalDecimal.Size = new System.Drawing.Size(13, 13);
            this.lblLastAreaTotalDecimal.TabIndex = 25;
            this.lblLastAreaTotalDecimal.Text = "0";
            // 
            // lblLastTotalPerimeterDecimal
            // 
            this.lblLastTotalPerimeterDecimal.AutoSize = true;
            this.lblLastTotalPerimeterDecimal.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblLastTotalPerimeterDecimal.Location = new System.Drawing.Point(154, 116);
            this.lblLastTotalPerimeterDecimal.Name = "lblLastTotalPerimeterDecimal";
            this.lblLastTotalPerimeterDecimal.Size = new System.Drawing.Size(13, 13);
            this.lblLastTotalPerimeterDecimal.TabIndex = 29;
            this.lblLastTotalPerimeterDecimal.Text = "0";
            // 
            // lblLastTotalPerimeterWhole
            // 
            this.lblLastTotalPerimeterWhole.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblLastTotalPerimeterWhole.Location = new System.Drawing.Point(112, 116);
            this.lblLastTotalPerimeterWhole.Name = "lblLastTotalPerimeterWhole";
            this.lblLastTotalPerimeterWhole.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblLastTotalPerimeterWhole.Size = new System.Drawing.Size(45, 13);
            this.lblLastTotalPerimeterWhole.TabIndex = 28;
            this.lblLastTotalPerimeterWhole.Text = "0";
            // 
            // pnlAreaTab
            // 
            this.pnlAreaTab.BorderStyle = Infragistics.Win.UIElementBorderStyle.Inset;
            // 
            // pnlAreaTab.ClientArea
            // 
            this.pnlAreaTab.ClientArea.Controls.Add(this.grpAreaTab);
            this.pnlAreaTab.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAreaTab.Location = new System.Drawing.Point(0, 0);
            this.pnlAreaTab.Margin = new System.Windows.Forms.Padding(5);
            this.pnlAreaTab.Name = "pnlAreaTab";
            this.pnlAreaTab.Size = new System.Drawing.Size(247, 413);
            this.pnlAreaTab.TabIndex = 1;
            // 
            // grpAreaTab
            // 
            this.grpAreaTab.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.None;
            this.grpAreaTab.ContentPadding.Bottom = 10;
            this.grpAreaTab.ContentPadding.Left = 10;
            this.grpAreaTab.ContentPadding.Right = 10;
            this.grpAreaTab.ContentPadding.Top = 10;
            this.grpAreaTab.Controls.Add(this.pnlFinishUCs);
            this.grpAreaTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpAreaTab.Location = new System.Drawing.Point(0, 0);
            this.grpAreaTab.Name = "grpAreaTab";
            this.grpAreaTab.Size = new System.Drawing.Size(243, 409);
            this.grpAreaTab.TabIndex = 2;
            this.grpAreaTab.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // pnlFinishUCs
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            this.pnlFinishUCs.Appearance = appearance4;
            this.pnlFinishUCs.AutoScroll = true;
            this.pnlFinishUCs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFinishUCs.Location = new System.Drawing.Point(10, 10);
            this.pnlFinishUCs.Name = "pnlFinishUCs";
            this.pnlFinishUCs.Size = new System.Drawing.Size(223, 389);
            this.pnlFinishUCs.TabIndex = 0;
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.ultraPanelLineTab);
            this.ultraTabPageControl3.Controls.Add(this.pnlLineTab);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(247, 666);
            // 
            // ultraPanelLineTab
            // 
            appearance5.BackColor = System.Drawing.Color.White;
            this.ultraPanelLineTab.Appearance = appearance5;
            // 
            // ultraPanelLineTab.ClientArea
            // 
            this.ultraPanelLineTab.ClientArea.Controls.Add(this.pnlLineProps);
            this.ultraPanelLineTab.ClientArea.Controls.Add(this.label2);
            this.ultraPanelLineTab.ClientArea.Controls.Add(this.lblLastLineWhole);
            this.ultraPanelLineTab.ClientArea.Controls.Add(this.label6);
            this.ultraPanelLineTab.ClientArea.Controls.Add(this.lblLastLineDecimal);
            this.ultraPanelLineTab.ClientArea.Controls.Add(this.label15);
            this.ultraPanelLineTab.ClientArea.Controls.Add(this.label23);
            this.ultraPanelLineTab.ClientArea.Controls.Add(this.btnEditLine);
            this.ultraPanelLineTab.ClientArea.Controls.Add(this.lblTotalLineDecimal);
            this.ultraPanelLineTab.ClientArea.Controls.Add(this.lblTotalLineWhole);
            this.ultraPanelLineTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanelLineTab.Location = new System.Drawing.Point(0, 413);
            this.ultraPanelLineTab.Name = "ultraPanelLineTab";
            this.ultraPanelLineTab.Size = new System.Drawing.Size(247, 253);
            this.ultraPanelLineTab.TabIndex = 17;
            // 
            // pnlLineProps
            // 
            appearance6.BackColor = System.Drawing.Color.White;
            this.pnlLineProps.Appearance = appearance6;
            this.pnlLineProps.AutoScroll = true;
            this.pnlLineProps.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.pnlLineProps.Location = new System.Drawing.Point(25, 70);
            this.pnlLineProps.Name = "pnlLineProps";
            this.pnlLineProps.Size = new System.Drawing.Size(190, 74);
            this.pnlLineProps.TabIndex = 32;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label2.Location = new System.Drawing.Point(154, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1, 1);
            this.label2.TabIndex = 14;
            this.label2.Text = ".";
            // 
            // lblLastLineWhole
            // 
            this.lblLastLineWhole.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblLastLineWhole.Location = new System.Drawing.Point(112, 24);
            this.lblLastLineWhole.Name = "lblLastLineWhole";
            this.lblLastLineWhole.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblLastLineWhole.Size = new System.Drawing.Size(45, 13);
            this.lblLastLineWhole.TabIndex = 12;
            this.lblLastLineWhole.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label6.Location = new System.Drawing.Point(12, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Last Line:";
            // 
            // lblLastLineDecimal
            // 
            this.lblLastLineDecimal.AutoSize = true;
            this.lblLastLineDecimal.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblLastLineDecimal.Location = new System.Drawing.Point(154, 24);
            this.lblLastLineDecimal.Name = "lblLastLineDecimal";
            this.lblLastLineDecimal.Size = new System.Drawing.Size(13, 13);
            this.lblLastLineDecimal.TabIndex = 13;
            this.lblLastLineDecimal.Text = "0";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label15.Location = new System.Drawing.Point(12, 47);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 13);
            this.label15.TabIndex = 15;
            this.label15.Text = "Total Lines:";
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.Color.Black;
            this.label23.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label23.Location = new System.Drawing.Point(154, 58);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(1, 1);
            this.label23.TabIndex = 18;
            this.label23.Text = ".";
            // 
            // btnEditLine
            // 
            this.btnEditLine.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditLine.Location = new System.Drawing.Point(12, 150);
            this.btnEditLine.Name = "btnEditLine";
            this.btnEditLine.Size = new System.Drawing.Size(96, 23);
            this.btnEditLine.TabIndex = 31;
            this.btnEditLine.Text = "Edit";
            this.btnEditLine.Click += new System.EventHandler(this.btnEditLine_Click);
            // 
            // lblTotalLineDecimal
            // 
            this.lblTotalLineDecimal.AutoSize = true;
            this.lblTotalLineDecimal.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblTotalLineDecimal.Location = new System.Drawing.Point(154, 47);
            this.lblTotalLineDecimal.Name = "lblTotalLineDecimal";
            this.lblTotalLineDecimal.Size = new System.Drawing.Size(13, 13);
            this.lblTotalLineDecimal.TabIndex = 17;
            this.lblTotalLineDecimal.Text = "0";
            // 
            // lblTotalLineWhole
            // 
            this.lblTotalLineWhole.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblTotalLineWhole.Location = new System.Drawing.Point(112, 47);
            this.lblTotalLineWhole.Name = "lblTotalLineWhole";
            this.lblTotalLineWhole.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTotalLineWhole.Size = new System.Drawing.Size(45, 13);
            this.lblTotalLineWhole.TabIndex = 16;
            this.lblTotalLineWhole.Text = "0";
            // 
            // pnlLineTab
            // 
            this.pnlLineTab.BorderStyle = Infragistics.Win.UIElementBorderStyle.Inset;
            // 
            // pnlLineTab.ClientArea
            // 
            this.pnlLineTab.ClientArea.Controls.Add(this.grpLineTab);
            this.pnlLineTab.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLineTab.Location = new System.Drawing.Point(0, 0);
            this.pnlLineTab.Margin = new System.Windows.Forms.Padding(5);
            this.pnlLineTab.Name = "pnlLineTab";
            this.pnlLineTab.Size = new System.Drawing.Size(247, 413);
            this.pnlLineTab.TabIndex = 16;
            // 
            // grpLineTab
            // 
            this.grpLineTab.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.None;
            this.grpLineTab.ContentPadding.Bottom = 10;
            this.grpLineTab.ContentPadding.Left = 10;
            this.grpLineTab.ContentPadding.Right = 10;
            this.grpLineTab.ContentPadding.Top = 10;
            this.grpLineTab.Controls.Add(this.pnlLineUCs);
            this.grpLineTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpLineTab.Location = new System.Drawing.Point(0, 0);
            this.grpLineTab.Name = "grpLineTab";
            this.grpLineTab.Size = new System.Drawing.Size(243, 409);
            this.grpLineTab.TabIndex = 2;
            this.grpLineTab.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // pnlLineUCs
            // 
            appearance7.BackColor = System.Drawing.Color.White;
            this.pnlLineUCs.Appearance = appearance7;
            this.pnlLineUCs.AutoScroll = true;
            this.pnlLineUCs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLineUCs.Location = new System.Drawing.Point(10, 10);
            this.pnlLineUCs.Name = "pnlLineUCs";
            this.pnlLineUCs.Size = new System.Drawing.Size(223, 389);
            this.pnlLineUCs.TabIndex = 0;
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(247, 666);
            // 
            // statusStrip
            // 
            this.statusStrip.AllowMerge = false;
            this.statusStrip.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusStripPrompt,
            this.statusStripScale,
            this.statusStripZoom});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1295, 29);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusStripPrompt
            // 
            this.statusStripPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.statusStripPrompt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.statusStripPrompt.Name = "statusStripPrompt";
            this.statusStripPrompt.Size = new System.Drawing.Size(1040, 24);
            this.statusStripPrompt.Spring = true;
            this.statusStripPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusStripScale
            // 
            this.statusStripScale.AutoSize = false;
            this.statusStripScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.statusStripScale.Name = "statusStripScale";
            this.statusStripScale.Size = new System.Drawing.Size(140, 24);
            // 
            // statusStripZoom
            // 
            this.statusStripZoom.AutoSize = false;
            this.statusStripZoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.statusStripZoom.Name = "statusStripZoom";
            this.statusStripZoom.Size = new System.Drawing.Size(100, 24);
            this.statusStripZoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPanes
            // 
            this.tabPanes.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabPanes.Controls.Add(this.ultraTabPageControl1);
            this.tabPanes.Controls.Add(this.ultraTabPageControl2);
            this.tabPanes.Controls.Add(this.ultraTabPageControl3);
            this.tabPanes.Controls.Add(this.ultraTabPageControl4);
            this.tabPanes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPanes.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPanes.Location = new System.Drawing.Point(0, 18);
            this.tabPanes.Name = "tabPanes";
            this.tabPanes.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabPanes.Size = new System.Drawing.Size(249, 690);
            this.tabPanes.TabIndex = 22;
            ultraTab1.Key = "Pages";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Pages";
            ultraTab2.Key = "Area";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Area";
            ultraTab3.Key = "Lines";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Lines";
            ultraTab4.Key = "Other";
            ultraTab4.TabPage = this.ultraTabPageControl4;
            ultraTab4.Text = "Other";
            this.tabPanes.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3,
            ultraTab4});
            this.tabPanes.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2007;
            this.tabPanes.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraTabControl1_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(247, 666);
            // 
            // ultraPanelRight
            // 
            // 
            // ultraPanelRight.ClientArea
            // 
            this.ultraPanelRight.ClientArea.Controls.Add(this.ultraPanel1);
            this.ultraPanelRight.ClientArea.Controls.Add(this.ultraPanelBackRight);
            this.ultraPanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanelRight.Location = new System.Drawing.Point(0, 18);
            this.ultraPanelRight.Name = "ultraPanelRight";
            this.ultraPanelRight.Size = new System.Drawing.Size(200, 690);
            this.ultraPanelRight.TabIndex = 24;
            // 
            // ultraPanel1
            // 
            this.ultraPanel1.AutoScroll = true;
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraGroupBox5);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraGroupBox4);
            this.ultraPanel1.ClientArea.Controls.Add(this.grpTrim);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 341);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(200, 349);
            this.ultraPanel1.TabIndex = 12;
            // 
            // ultraGroupBox5
            // 
            this.ultraGroupBox5.Controls.Add(this.ultraButton6);
            this.ultraGroupBox5.Controls.Add(this.rdNoX1Y1);
            this.ultraGroupBox5.Controls.Add(this.rdYesX1Y1);
            this.ultraGroupBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraGroupBox5.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.ultraGroupBox5.Location = new System.Drawing.Point(0, 370);
            this.ultraGroupBox5.Name = "ultraGroupBox5";
            this.ultraGroupBox5.Size = new System.Drawing.Size(183, 89);
            this.ultraGroupBox5.TabIndex = 8;
            this.ultraGroupBox5.Text = "X1=X2, Y1=Y2";
            // 
            // ultraButton6
            // 
            this.ultraButton6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton6.Location = new System.Drawing.Point(39, 59);
            this.ultraButton6.Name = "ultraButton6";
            this.ultraButton6.Size = new System.Drawing.Size(96, 23);
            this.ultraButton6.TabIndex = 9;
            this.ultraButton6.Text = "Edit";
            // 
            // rdNoX1Y1
            // 
            this.rdNoX1Y1.AutoSize = true;
            this.rdNoX1Y1.Checked = true;
            this.rdNoX1Y1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.rdNoX1Y1.Location = new System.Drawing.Point(109, 36);
            this.rdNoX1Y1.Name = "rdNoX1Y1";
            this.rdNoX1Y1.Size = new System.Drawing.Size(40, 17);
            this.rdNoX1Y1.TabIndex = 1;
            this.rdNoX1Y1.TabStop = true;
            this.rdNoX1Y1.Text = "No";
            this.rdNoX1Y1.UseVisualStyleBackColor = true;
            // 
            // rdYesX1Y1
            // 
            this.rdYesX1Y1.AutoSize = true;
            this.rdYesX1Y1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.rdYesX1Y1.Location = new System.Drawing.Point(24, 36);
            this.rdYesX1Y1.Name = "rdYesX1Y1";
            this.rdYesX1Y1.Size = new System.Drawing.Size(41, 17);
            this.rdYesX1Y1.TabIndex = 0;
            this.rdYesX1Y1.Text = "Yes";
            this.rdYesX1Y1.UseVisualStyleBackColor = true;
            // 
            // ultraGroupBox4
            // 
            this.ultraGroupBox4.Controls.Add(this.cmbScale);
            this.ultraGroupBox4.Controls.Add(this.chkScale);
            this.ultraGroupBox4.Controls.Add(this.grpCustom);
            this.ultraGroupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraGroupBox4.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.ultraGroupBox4.Location = new System.Drawing.Point(0, 89);
            this.ultraGroupBox4.Name = "ultraGroupBox4";
            this.ultraGroupBox4.Size = new System.Drawing.Size(183, 281);
            this.ultraGroupBox4.TabIndex = 7;
            this.ultraGroupBox4.Text = "Scale";
            // 
            // cmbScale
            // 
            appearance8.TextHAlignAsString = "Right";
            this.cmbScale.Appearance = appearance8;
            this.cmbScale.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.cmbScale.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbScale.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbScale.Location = new System.Drawing.Point(3, 240);
            this.cmbScale.Name = "cmbScale";
            this.cmbScale.Size = new System.Drawing.Size(177, 24);
            this.cmbScale.TabIndex = 5;
            this.cmbScale.ValueChanged += new System.EventHandler(this.cmbScale_ValueChanged);
            // 
            // chkScale
            // 
            appearance9.FontData.Name = "Segoe UI";
            this.chkScale.Appearance = appearance9;
            this.chkScale.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkScale.Location = new System.Drawing.Point(3, 220);
            this.chkScale.Name = "chkScale";
            this.chkScale.Size = new System.Drawing.Size(177, 20);
            this.chkScale.TabIndex = 3;
            this.chkScale.Text = "No Scale Needed";
            // 
            // grpCustom
            // 
            this.grpCustom.Controls.Add(this.lbClick2NdPoint);
            this.grpCustom.Controls.Add(this.lbClick1StPoint);
            this.grpCustom.Controls.Add(this.txtScaleInches);
            this.grpCustom.Controls.Add(this.txtScaleFeet);
            this.grpCustom.Controls.Add(this.label10);
            this.grpCustom.Controls.Add(this.label7);
            this.grpCustom.Controls.Add(this.btnScaleOk);
            this.grpCustom.Controls.Add(this.btnScaleReset);
            this.grpCustom.Controls.Add(this.btnScaleCancel);
            this.grpCustom.Controls.Add(this.btnScaleNew);
            this.grpCustom.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCustom.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.grpCustom.Location = new System.Drawing.Point(3, 21);
            this.grpCustom.Name = "grpCustom";
            this.grpCustom.Size = new System.Drawing.Size(177, 199);
            this.grpCustom.TabIndex = 41;
            this.grpCustom.Text = "Custom";
            this.grpCustom.Visible = false;
            // 
            // lbClick2NdPoint
            // 
            this.lbClick2NdPoint.AutoSize = true;
            this.lbClick2NdPoint.Location = new System.Drawing.Point(45, 49);
            this.lbClick2NdPoint.Name = "lbClick2NdPoint";
            this.lbClick2NdPoint.Size = new System.Drawing.Size(94, 17);
            this.lbClick2NdPoint.TabIndex = 6;
            this.lbClick2NdPoint.Text = "Click 2nd point";
            // 
            // lbClick1StPoint
            // 
            this.lbClick1StPoint.AutoSize = true;
            this.lbClick1StPoint.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lbClick1StPoint.Location = new System.Drawing.Point(45, 22);
            this.lbClick1StPoint.Name = "lbClick1StPoint";
            this.lbClick1StPoint.Size = new System.Drawing.Size(89, 17);
            this.lbClick1StPoint.TabIndex = 5;
            this.lbClick1StPoint.Text = "Click 1st point";
            // 
            // txtScaleInches
            // 
            this.txtScaleInches.Enabled = false;
            this.txtScaleInches.Location = new System.Drawing.Point(97, 111);
            this.txtScaleInches.Name = "txtScaleInches";
            this.txtScaleInches.Size = new System.Drawing.Size(48, 25);
            this.txtScaleInches.TabIndex = 4;
            // 
            // txtScaleFeet
            // 
            this.txtScaleFeet.Enabled = false;
            this.txtScaleFeet.Location = new System.Drawing.Point(34, 111);
            this.txtScaleFeet.Name = "txtScaleFeet";
            this.txtScaleFeet.Size = new System.Drawing.Size(48, 25);
            this.txtScaleFeet.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(101, 139);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 17);
            this.label10.TabIndex = 3;
            this.label10.Text = "Inches";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(39, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 17);
            this.label7.TabIndex = 3;
            this.label7.Text = "Feet";
            // 
            // btnScaleOk
            // 
            this.btnScaleOk.Enabled = false;
            this.btnScaleOk.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScaleOk.Location = new System.Drawing.Point(119, 166);
            this.btnScaleOk.Name = "btnScaleOk";
            this.btnScaleOk.Size = new System.Drawing.Size(48, 23);
            this.btnScaleOk.TabIndex = 2;
            this.btnScaleOk.Text = "OK";
            this.btnScaleOk.Click += new System.EventHandler(this.btnScaleOk_Click);
            // 
            // btnScaleReset
            // 
            this.btnScaleReset.Enabled = false;
            this.btnScaleReset.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScaleReset.Location = new System.Drawing.Point(72, 79);
            this.btnScaleReset.Name = "btnScaleReset";
            this.btnScaleReset.Size = new System.Drawing.Size(48, 23);
            this.btnScaleReset.TabIndex = 2;
            this.btnScaleReset.Text = "Reset";
            this.btnScaleReset.Click += new System.EventHandler(this.btnScaleReset_Click);
            // 
            // btnScaleCancel
            // 
            this.btnScaleCancel.Enabled = false;
            this.btnScaleCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScaleCancel.Location = new System.Drawing.Point(65, 166);
            this.btnScaleCancel.Name = "btnScaleCancel";
            this.btnScaleCancel.Size = new System.Drawing.Size(48, 23);
            this.btnScaleCancel.TabIndex = 2;
            this.btnScaleCancel.Text = "Cancel";
            this.btnScaleCancel.Click += new System.EventHandler(this.btnScaleCancel_Click);
            // 
            // btnScaleNew
            // 
            this.btnScaleNew.Enabled = false;
            this.btnScaleNew.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScaleNew.Location = new System.Drawing.Point(11, 166);
            this.btnScaleNew.Name = "btnScaleNew";
            this.btnScaleNew.Size = new System.Drawing.Size(48, 23);
            this.btnScaleNew.TabIndex = 2;
            this.btnScaleNew.Text = "New";
            this.btnScaleNew.Click += new System.EventHandler(this.btnScaleNew_Click);
            // 
            // grpTrim
            // 
            this.grpTrim.Controls.Add(this.ultraLabel2);
            this.grpTrim.Controls.Add(this.cmbTrimFactor);
            this.grpTrim.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTrim.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.grpTrim.Location = new System.Drawing.Point(0, 0);
            this.grpTrim.Name = "grpTrim";
            this.grpTrim.Size = new System.Drawing.Size(183, 89);
            this.grpTrim.TabIndex = 6;
            this.grpTrim.Text = "Trim";
            // 
            // ultraLabel2
            // 
            appearance10.TextHAlignAsString = "Center";
            appearance10.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance10;
            this.ultraLabel2.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.ultraLabel2.Location = new System.Drawing.Point(36, 21);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel2.TabIndex = 1;
            this.ultraLabel2.Text = "Trim Factor";
            // 
            // cmbTrimFactor
            // 
            appearance11.TextHAlignAsString = "Right";
            this.cmbTrimFactor.Appearance = appearance11;
            this.cmbTrimFactor.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.cmbTrimFactor.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            valueListItem1.DataValue = "0";
            this.cmbTrimFactor.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1});
            this.cmbTrimFactor.Location = new System.Drawing.Point(36, 50);
            this.cmbTrimFactor.Name = "cmbTrimFactor";
            this.cmbTrimFactor.Size = new System.Drawing.Size(100, 24);
            this.cmbTrimFactor.TabIndex = 2;
            this.cmbTrimFactor.Text = "0";
            // 
            // ultraPanelBackRight
            // 
            // 
            // ultraPanelBackRight.ClientArea
            // 
            this.ultraPanelBackRight.ClientArea.Controls.Add(this.pnlAreaModeInRight);
            this.ultraPanelBackRight.ClientArea.Controls.Add(this.pnlLineModeRightPanel);
            this.ultraPanelBackRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanelBackRight.Location = new System.Drawing.Point(0, 0);
            this.ultraPanelBackRight.Name = "ultraPanelBackRight";
            this.ultraPanelBackRight.Size = new System.Drawing.Size(200, 341);
            this.ultraPanelBackRight.TabIndex = 3;
            // 
            // pnlAreaModeInRight
            // 
            this.pnlAreaModeInRight.BorderStyle = Infragistics.Win.UIElementBorderStyle.Inset;
            // 
            // pnlAreaModeInRight.ClientArea
            // 
            this.pnlAreaModeInRight.ClientArea.Controls.Add(this.grpAreaInRight);
            this.pnlAreaModeInRight.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlAreaModeInRight.Location = new System.Drawing.Point(9, 134);
            this.pnlAreaModeInRight.Margin = new System.Windows.Forms.Padding(5);
            this.pnlAreaModeInRight.Name = "pnlAreaModeInRight";
            this.pnlAreaModeInRight.Size = new System.Drawing.Size(200, 341);
            this.pnlAreaModeInRight.TabIndex = 0;
            // 
            // grpAreaInRight
            // 
            this.grpAreaInRight.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.None;
            this.grpAreaInRight.ContentPadding.Bottom = 10;
            this.grpAreaInRight.ContentPadding.Left = 10;
            this.grpAreaInRight.ContentPadding.Right = 10;
            this.grpAreaInRight.ContentPadding.Top = 10;
            this.grpAreaInRight.Controls.Add(this.ultraGroupBox1);
            this.grpAreaInRight.Controls.Add(this.ultraGroupBox3);
            this.grpAreaInRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpAreaInRight.Enabled = false;
            this.grpAreaInRight.Location = new System.Drawing.Point(0, 0);
            this.grpAreaInRight.Name = "grpAreaInRight";
            this.grpAreaInRight.Size = new System.Drawing.Size(196, 337);
            this.grpAreaInRight.TabIndex = 2;
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.btnAreaZeroLine);
            this.ultraGroupBox1.Controls.Add(this.ultraButton4);
            this.ultraGroupBox1.Controls.Add(this.ultraButton3);
            this.ultraGroupBox1.Controls.Add(this.btnAreaTakeOut);
            this.ultraGroupBox1.Controls.Add(this.btnComplete);
            this.ultraGroupBox1.Controls.Add(this.ultraButton1);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraGroupBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupBox1.Location = new System.Drawing.Point(10, 32);
            this.ultraGroupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(176, 209);
            this.ultraGroupBox1.TabIndex = 9;
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // btnAreaZeroLine
            // 
            appearance12.BackColor = System.Drawing.SystemColors.Control;
            this.btnAreaZeroLine.Appearance = appearance12;
            this.btnAreaZeroLine.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAreaZeroLine.Location = new System.Drawing.Point(42, 178);
            this.btnAreaZeroLine.Name = "btnAreaZeroLine";
            this.btnAreaZeroLine.Size = new System.Drawing.Size(96, 23);
            this.btnAreaZeroLine.TabIndex = 8;
            this.btnAreaZeroLine.Text = "Zero Line";
            this.btnAreaZeroLine.Click += new System.EventHandler(this.btnAreaZeroLine_Click);
            // 
            // ultraButton4
            // 
            this.ultraButton4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton4.Location = new System.Drawing.Point(42, 149);
            this.ultraButton4.Name = "ultraButton4";
            this.ultraButton4.Size = new System.Drawing.Size(96, 23);
            this.ultraButton4.TabIndex = 3;
            this.ultraButton4.Text = "Paste";
            // 
            // ultraButton3
            // 
            this.ultraButton3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton3.Location = new System.Drawing.Point(42, 120);
            this.ultraButton3.Name = "ultraButton3";
            this.ultraButton3.Size = new System.Drawing.Size(96, 23);
            this.ultraButton3.TabIndex = 2;
            this.ultraButton3.Text = "Copy";
            // 
            // btnAreaTakeOut
            // 
            this.btnAreaTakeOut.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAreaTakeOut.Location = new System.Drawing.Point(42, 47);
            this.btnAreaTakeOut.Name = "btnAreaTakeOut";
            this.btnAreaTakeOut.Size = new System.Drawing.Size(96, 23);
            this.btnAreaTakeOut.TabIndex = 2;
            this.btnAreaTakeOut.Text = "Take Out";
            this.btnAreaTakeOut.Click += new System.EventHandler(this.btnAreaTakeOut_Click);
            // 
            // btnComplete
            // 
            this.btnComplete.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnComplete.Location = new System.Drawing.Point(42, 18);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(96, 23);
            this.btnComplete.TabIndex = 2;
            this.btnComplete.Text = "Complete";
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // ultraButton1
            // 
            this.ultraButton1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton1.Location = new System.Drawing.Point(42, 76);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(96, 23);
            this.ultraButton1.TabIndex = 4;
            this.ultraButton1.Text = "Insert";
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.Controls.Add(this.lblSelectedArea);
            this.ultraGroupBox3.Controls.Add(this.btnEmptyShape);
            this.ultraGroupBox3.Controls.Add(this.ultraButton2);
            this.ultraGroupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraGroupBox3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupBox3.Location = new System.Drawing.Point(10, 10);
            this.ultraGroupBox3.Margin = new System.Windows.Forms.Padding(5);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(176, 22);
            this.ultraGroupBox3.TabIndex = 11;
            this.ultraGroupBox3.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            this.ultraGroupBox3.Visible = false;
            // 
            // lblSelectedArea
            // 
            appearance13.BackColor = System.Drawing.Color.Transparent;
            appearance13.TextHAlignAsString = "Center";
            appearance13.TextVAlignAsString = "Middle";
            this.lblSelectedArea.Appearance = appearance13;
            this.lblSelectedArea.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSelectedArea.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblSelectedArea.Location = new System.Drawing.Point(3, 3);
            this.lblSelectedArea.Name = "lblSelectedArea";
            this.lblSelectedArea.Size = new System.Drawing.Size(170, 70);
            this.lblSelectedArea.TabIndex = 3;
            this.lblSelectedArea.Text = "Area:";
            // 
            // btnEmptyShape
            // 
            this.btnEmptyShape.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmptyShape.Location = new System.Drawing.Point(44, 50);
            this.btnEmptyShape.Name = "btnEmptyShape";
            this.btnEmptyShape.Size = new System.Drawing.Size(96, 23);
            this.btnEmptyShape.TabIndex = 3;
            this.btnEmptyShape.Text = "Empty Shape";
            // 
            // ultraButton2
            // 
            this.ultraButton2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton2.Location = new System.Drawing.Point(42, 26);
            this.ultraButton2.Name = "ultraButton2";
            this.ultraButton2.Size = new System.Drawing.Size(96, 23);
            this.ultraButton2.TabIndex = 5;
            this.ultraButton2.Text = "Overlay Shape";
            // 
            // pnlLineModeRightPanel
            // 
            this.pnlLineModeRightPanel.BorderStyle = Infragistics.Win.UIElementBorderStyle.Inset;
            // 
            // pnlLineModeRightPanel.ClientArea
            // 
            this.pnlLineModeRightPanel.ClientArea.Controls.Add(this.grpLineInRight);
            this.pnlLineModeRightPanel.Location = new System.Drawing.Point(10, 23);
            this.pnlLineModeRightPanel.Margin = new System.Windows.Forms.Padding(5);
            this.pnlLineModeRightPanel.Name = "pnlLineModeRightPanel";
            this.pnlLineModeRightPanel.Size = new System.Drawing.Size(200, 341);
            this.pnlLineModeRightPanel.TabIndex = 3;
            this.pnlLineModeRightPanel.Visible = false;
            // 
            // grpLineInRight
            // 
            this.grpLineInRight.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.None;
            this.grpLineInRight.ContentPadding.Bottom = 10;
            this.grpLineInRight.ContentPadding.Left = 10;
            this.grpLineInRight.ContentPadding.Right = 10;
            this.grpLineInRight.ContentPadding.Top = 10;
            this.grpLineInRight.Controls.Add(this.ultraGroupBox8);
            this.grpLineInRight.Controls.Add(this.ultraGroupBox9);
            this.grpLineInRight.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpLineInRight.Enabled = false;
            this.grpLineInRight.Location = new System.Drawing.Point(0, 0);
            this.grpLineInRight.Name = "grpLineInRight";
            this.grpLineInRight.Size = new System.Drawing.Size(196, 337);
            this.grpLineInRight.TabIndex = 2;
            // 
            // ultraGroupBox8
            // 
            this.ultraGroupBox8.Controls.Add(this.grpDoorTakeOut);
            this.ultraGroupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox8.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupBox8.Location = new System.Drawing.Point(10, 167);
            this.ultraGroupBox8.Margin = new System.Windows.Forms.Padding(5);
            this.ultraGroupBox8.Name = "ultraGroupBox8";
            this.ultraGroupBox8.Size = new System.Drawing.Size(176, 160);
            this.ultraGroupBox8.TabIndex = 10;
            this.ultraGroupBox8.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // grpDoorTakeOut
            // 
            this.grpDoorTakeOut.Controls.Add(this.btnDeactivateLineDoorTO);
            this.grpDoorTakeOut.Controls.Add(this.btnShowLineDoorTO);
            this.grpDoorTakeOut.Controls.Add(this.btnActivateLineDoorTO);
            this.grpDoorTakeOut.Controls.Add(this.btnHideLineDoorTO);
            this.grpDoorTakeOut.Controls.Add(this.txtOtherFt);
            this.grpDoorTakeOut.Controls.Add(this.rdOtherft);
            this.grpDoorTakeOut.Controls.Add(this.rd6f);
            this.grpDoorTakeOut.Controls.Add(this.ultraLabel3);
            this.grpDoorTakeOut.Controls.Add(this.rd3f);
            this.grpDoorTakeOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDoorTakeOut.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.grpDoorTakeOut.Location = new System.Drawing.Point(3, 3);
            this.grpDoorTakeOut.Name = "grpDoorTakeOut";
            this.grpDoorTakeOut.Size = new System.Drawing.Size(170, 154);
            this.grpDoorTakeOut.TabIndex = 7;
            this.grpDoorTakeOut.Text = "Door Take-Out";
            // 
            // btnDeactivateLineDoorTO
            // 
            appearance14.BackColor = System.Drawing.SystemColors.Control;
            this.btnDeactivateLineDoorTO.Appearance = appearance14;
            this.btnDeactivateLineDoorTO.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeactivateLineDoorTO.Location = new System.Drawing.Point(6, 102);
            this.btnDeactivateLineDoorTO.Name = "btnDeactivateLineDoorTO";
            this.btnDeactivateLineDoorTO.Size = new System.Drawing.Size(67, 23);
            this.btnDeactivateLineDoorTO.TabIndex = 12;
            this.btnDeactivateLineDoorTO.Text = "Deactivate";
            this.btnDeactivateLineDoorTO.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDeactivateLineDoorTO.Visible = false;
            this.btnDeactivateLineDoorTO.Click += new System.EventHandler(this.btnDeactivateLineDoorTO_Click);
            // 
            // btnShowLineDoorTO
            // 
            appearance15.BackColor = System.Drawing.SystemColors.Control;
            this.btnShowLineDoorTO.Appearance = appearance15;
            this.btnShowLineDoorTO.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowLineDoorTO.Location = new System.Drawing.Point(79, 102);
            this.btnShowLineDoorTO.Name = "btnShowLineDoorTO";
            this.btnShowLineDoorTO.Size = new System.Drawing.Size(75, 23);
            this.btnShowLineDoorTO.TabIndex = 11;
            this.btnShowLineDoorTO.Text = "Show";
            this.btnShowLineDoorTO.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnShowLineDoorTO.Visible = false;
            this.btnShowLineDoorTO.Click += new System.EventHandler(this.btnShowLineDoorTO_Click);
            // 
            // btnActivateLineDoorTO
            // 
            appearance16.BackColor = System.Drawing.SystemColors.Control;
            this.btnActivateLineDoorTO.Appearance = appearance16;
            this.btnActivateLineDoorTO.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActivateLineDoorTO.Location = new System.Drawing.Point(6, 73);
            this.btnActivateLineDoorTO.Name = "btnActivateLineDoorTO";
            this.btnActivateLineDoorTO.Size = new System.Drawing.Size(67, 23);
            this.btnActivateLineDoorTO.TabIndex = 12;
            this.btnActivateLineDoorTO.Text = "Activate";
            this.btnActivateLineDoorTO.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnActivateLineDoorTO.Click += new System.EventHandler(this.btnActivateLineDoorTO_Click);
            // 
            // btnHideLineDoorTO
            // 
            appearance17.BackColor = System.Drawing.SystemColors.Control;
            this.btnHideLineDoorTO.Appearance = appearance17;
            this.btnHideLineDoorTO.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHideLineDoorTO.Location = new System.Drawing.Point(79, 73);
            this.btnHideLineDoorTO.Name = "btnHideLineDoorTO";
            this.btnHideLineDoorTO.Size = new System.Drawing.Size(75, 23);
            this.btnHideLineDoorTO.TabIndex = 11;
            this.btnHideLineDoorTO.Text = "Hide";
            this.btnHideLineDoorTO.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnHideLineDoorTO.Click += new System.EventHandler(this.btnHideLineDoorTO_Click);
            // 
            // txtOtherFt
            // 
            this.txtOtherFt.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.txtOtherFt.Location = new System.Drawing.Point(89, 45);
            this.txtOtherFt.Name = "txtOtherFt";
            this.txtOtherFt.Size = new System.Drawing.Size(55, 24);
            this.txtOtherFt.TabIndex = 10;
            this.txtOtherFt.Text = "6";
            // 
            // rdOtherft
            // 
            this.rdOtherft.AutoSize = true;
            this.rdOtherft.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.rdOtherft.Location = new System.Drawing.Point(18, 45);
            this.rdOtherft.Name = "rdOtherft";
            this.rdOtherft.Size = new System.Drawing.Size(55, 17);
            this.rdOtherft.TabIndex = 4;
            this.rdOtherft.Text = "Other";
            this.rdOtherft.UseVisualStyleBackColor = true;
            // 
            // rd6f
            // 
            this.rd6f.AutoSize = true;
            this.rd6f.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.rd6f.Location = new System.Drawing.Point(89, 23);
            this.rd6f.Name = "rd6f";
            this.rd6f.Size = new System.Drawing.Size(42, 17);
            this.rd6f.TabIndex = 3;
            this.rd6f.Text = "6 ft";
            this.rd6f.UseVisualStyleBackColor = true;
            // 
            // ultraLabel3
            // 
            appearance18.BackColor = System.Drawing.Color.Transparent;
            appearance18.TextHAlignAsString = "Center";
            appearance18.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance18;
            this.ultraLabel3.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.ultraLabel3.Location = new System.Drawing.Point(146, 47);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(18, 28);
            this.ultraLabel3.TabIndex = 13;
            this.ultraLabel3.Text = "ft";
            // 
            // rd3f
            // 
            this.rd3f.AutoSize = true;
            this.rd3f.Checked = true;
            this.rd3f.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.rd3f.Location = new System.Drawing.Point(18, 23);
            this.rd3f.Name = "rd3f";
            this.rd3f.Size = new System.Drawing.Size(42, 17);
            this.rd3f.TabIndex = 2;
            this.rd3f.TabStop = true;
            this.rd3f.Text = "3 ft";
            this.rd3f.UseVisualStyleBackColor = true;
            // 
            // ultraGroupBox9
            // 
            this.ultraGroupBox9.Controls.Add(this.btnLineZeroLine);
            this.ultraGroupBox9.Controls.Add(this.btnDuplicate);
            this.ultraGroupBox9.Controls.Add(this.btnJump);
            this.ultraGroupBox9.Controls.Add(this.btn2x);
            this.ultraGroupBox9.Controls.Add(this.btn1x);
            this.ultraGroupBox9.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraGroupBox9.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraGroupBox9.Location = new System.Drawing.Point(10, 10);
            this.ultraGroupBox9.Margin = new System.Windows.Forms.Padding(5);
            this.ultraGroupBox9.Name = "ultraGroupBox9";
            this.ultraGroupBox9.Size = new System.Drawing.Size(176, 157);
            this.ultraGroupBox9.TabIndex = 9;
            this.ultraGroupBox9.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // btnLineZeroLine
            // 
            appearance19.BackColor = System.Drawing.SystemColors.Control;
            this.btnLineZeroLine.Appearance = appearance19;
            this.btnLineZeroLine.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLineZeroLine.Location = new System.Drawing.Point(21, 126);
            this.btnLineZeroLine.Name = "btnLineZeroLine";
            this.btnLineZeroLine.Size = new System.Drawing.Size(136, 23);
            this.btnLineZeroLine.TabIndex = 11;
            this.btnLineZeroLine.Text = "Negative Line";
            this.btnLineZeroLine.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnLineZeroLine.Click += new System.EventHandler(this.btnLineZeroLine_Click);
            // 
            // btnDuplicate
            // 
            appearance20.BackColor = System.Drawing.SystemColors.Control;
            this.btnDuplicate.Appearance = appearance20;
            this.btnDuplicate.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDuplicate.Location = new System.Drawing.Point(21, 97);
            this.btnDuplicate.Name = "btnDuplicate";
            this.btnDuplicate.Size = new System.Drawing.Size(137, 23);
            this.btnDuplicate.TabIndex = 9;
            this.btnDuplicate.Text = "Duplicate";
            this.btnDuplicate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDuplicate.Click += new System.EventHandler(this.btnDuplicate_Click);
            // 
            // btnJump
            // 
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            this.btnJump.Appearance = appearance21;
            this.btnJump.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJump.Location = new System.Drawing.Point(21, 10);
            this.btnJump.Name = "btnJump";
            this.btnJump.Size = new System.Drawing.Size(137, 23);
            this.btnJump.TabIndex = 8;
            this.btnJump.Text = "Jump";
            this.btnJump.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnJump.Click += new System.EventHandler(this.btnJump_Click);
            // 
            // btn2x
            // 
            appearance22.BackColor = System.Drawing.SystemColors.Control;
            this.btn2x.Appearance = appearance22;
            this.btn2x.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2x.Location = new System.Drawing.Point(22, 68);
            this.btn2x.Name = "btn2x";
            this.btn2x.Size = new System.Drawing.Size(136, 23);
            this.btn2x.TabIndex = 7;
            this.btn2x.Text = "2x";
            this.btn2x.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btn2x.Click += new System.EventHandler(this.btn2x_Click);
            // 
            // btn1x
            // 
            appearance23.BackColor = System.Drawing.Color.Bisque;
            appearance23.BackColor2 = System.Drawing.Color.Orange;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.btn1x.Appearance = appearance23;
            this.btn1x.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn1x.Location = new System.Drawing.Point(22, 39);
            this.btn1x.Name = "btn1x";
            this.btn1x.Size = new System.Drawing.Size(136, 23);
            this.btn1x.TabIndex = 6;
            this.btn1x.Text = "1x";
            this.btn1x.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btn1x.Click += new System.EventHandler(this.btn1x_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "pdf";
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Title = "Select Drawing File";
            // 
            // _DrawingForm_Toolbars_Dock_Area_Left
            // 
            this._DrawingForm_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DrawingForm_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DrawingForm_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._DrawingForm_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DrawingForm_Toolbars_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._DrawingForm_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 162);
            this._DrawingForm_Toolbars_Dock_Area_Left.Name = "_DrawingForm_Toolbars_Dock_Area_Left";
            this._DrawingForm_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(8, 742);
            this._DrawingForm_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this;
            this.ultraToolbarsManager1.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this.ultraToolbarsManager1.LockToolbars = true;
            this.ultraToolbarsManager1.Office2007UICompatibility = false;
            optionSet1.AllowAllUp = false;
            optionSet2.AllowAllUp = false;
            optionSet3.AllowAllUp = false;
            this.ultraToolbarsManager1.OptionSets.Add(optionSet1);
            this.ultraToolbarsManager1.OptionSets.Add(optionSet2);
            this.ultraToolbarsManager1.OptionSets.Add(optionSet3);
            labelTool1.InstanceProps.IsFirstInGroup = true;
            buttonTool43.InstanceProps.IsFirstInGroup = true;
            this.ultraToolbarsManager1.Ribbon.ApplicationMenu.ToolAreaLeft.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            labelTool1,
            buttonTool43,
            buttonTool58,
            buttonTool65,
            buttonTool66,
            buttonTool67,
            buttonTool68,
            labelTool3});
            this.ultraToolbarsManager1.Ribbon.ApplicationMenu.ToolAreaLeft.Settings.LabelDisplayStyle = Infragistics.Win.UltraWinToolbars.LabelMenuDisplayStyle.Header;
            this.ultraToolbarsManager1.Ribbon.ApplicationMenu.ToolAreaRight.MaxWidth = 1;
            this.ultraToolbarsManager1.Ribbon.ApplicationMenu.ToolAreaRight.MinWidth = 1;
            this.ultraToolbarsManager1.Ribbon.ApplicationMenuButtonImage = global::FloorPlanning.Properties.Resources.icon64centered;
            appearance26.FontData.Name = "Segoe UI";
            this.ultraToolbarsManager1.Ribbon.GroupSettings.Appearance = appearance26;
            contextualTabGroup1.Caption = "contextualTabGroup1";
            contextualTabGroup1.Key = "contextualTabGroup1";
            contextualTabGroup2.Caption = "contextualTabGroup2";
            contextualTabGroup2.Key = "contextualTabGroup2";
            this.ultraToolbarsManager1.Ribbon.NonInheritedContextualTabGroups.AddRange(new Infragistics.Win.UltraWinToolbars.ContextualTabGroup[] {
            contextualTabGroup1,
            contextualTabGroup2});
            ribbonTab1.Caption = "Menu";
            ribbonGroup1.Caption = "";
            buttonTool37.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            buttonTool1.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            buttonTool2.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            buttonTool3.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            buttonTool4.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            ribbonGroup1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool37,
            buttonTool1,
            buttonTool2,
            buttonTool3,
            buttonTool4});
            ribbonGroup2.Caption = "";
            buttonTool9.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            buttonTool10.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            buttonTool11.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            buttonTool12.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            buttonTool13.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            ribbonGroup2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool9,
            buttonTool10,
            buttonTool11,
            buttonTool12,
            buttonTool13});
            ribbonGroup3.Caption = "";
            stateButtonTool1.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            stateButtonTool3.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            ribbonGroup3.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool1,
            stateButtonTool3,
            stateButtonTool9});
            ribbonGroup4.Caption = "";
            stateButtonTool5.Checked = true;
            stateButtonTool5.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            stateButtonTool7.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            ribbonGroup4.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool5,
            stateButtonTool7});
            ribbonGroup5.Caption = "";
            buttonTool19.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            buttonTool20.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            ribbonGroup5.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool19,
            buttonTool20});
            ribbonGroup6.Caption = "";
            buttonTool23.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            buttonTool24.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            buttonTool25.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            ribbonGroup6.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool23,
            buttonTool24,
            buttonTool25});
            ribbonGroup7.Caption = "";
            stateButtonTool12.Checked = true;
            stateButtonTool12.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            stateButtonTool14.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            ribbonGroup7.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool12,
            stateButtonTool14});
            ribbonGroup8.Caption = "";
            buttonTool33.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            buttonTool34.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            ribbonGroup8.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool33,
            buttonTool34});
            ribbonGroup9.Caption = "";
            buttonTool47.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            buttonTool48.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            ribbonGroup9.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool47,
            buttonTool48});
            ribbonGroup10.Caption = "";
            buttonTool51.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            buttonTool52.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            buttonTool55.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            ribbonGroup10.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool51,
            buttonTool52,
            buttonTool55});
            ribbonGroup11.Caption = "";
            popupMenuTool1.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            ribbonGroup11.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool1});
            ribbonGroup12.Caption = "ribbonGroup1";
            ribbonTab1.Groups.AddRange(new Infragistics.Win.UltraWinToolbars.RibbonGroup[] {
            ribbonGroup1,
            ribbonGroup2,
            ribbonGroup3,
            ribbonGroup4,
            ribbonGroup5,
            ribbonGroup6,
            ribbonGroup7,
            ribbonGroup8,
            ribbonGroup9,
            ribbonGroup10,
            ribbonGroup11,
            ribbonGroup12});
            ribbonTab2.Caption = "ribbon1";
            this.ultraToolbarsManager1.Ribbon.NonInheritedRibbonTabs.AddRange(new Infragistics.Win.UltraWinToolbars.RibbonTab[] {
            ribbonTab1,
            ribbonTab2});
            appearance27.FontData.Name = "Segoe UI";
            this.ultraToolbarsManager1.Ribbon.RibbonAreaAppearance = appearance27;
            appearance28.FontData.Name = "Segoe UI";
            this.ultraToolbarsManager1.Ribbon.TabAreaAppearance = appearance28;
            this.ultraToolbarsManager1.Ribbon.Visible = true;
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            this.ultraToolbarsManager1.ToolbarSettings.UseLargeImages = Infragistics.Win.DefaultableBoolean.True;
            appearance29.Image = global::FloorPlanning.Properties.Resources.open32;
            buttonTool5.SharedPropsInternal.AppearancesLarge.Appearance = appearance29;
            appearance30.Image = global::FloorPlanning.Properties.Resources.open16;
            buttonTool5.SharedPropsInternal.AppearancesSmall.Appearance = appearance30;
            buttonTool5.SharedPropsInternal.Caption = "Open";
            buttonTool5.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.DefaultForToolType;
            appearance31.Image = global::FloorPlanning.Properties.Resources.save32;
            buttonTool6.SharedPropsInternal.AppearancesLarge.Appearance = appearance31;
            appearance32.Image = global::FloorPlanning.Properties.Resources.save16;
            buttonTool6.SharedPropsInternal.AppearancesSmall.Appearance = appearance32;
            buttonTool6.SharedPropsInternal.Caption = "Save";
            appearance33.Image = global::FloorPlanning.Properties.Resources.saveas32;
            buttonTool7.SharedPropsInternal.AppearancesLarge.Appearance = appearance33;
            appearance34.Image = global::FloorPlanning.Properties.Resources.saveas16;
            buttonTool7.SharedPropsInternal.AppearancesSmall.Appearance = appearance34;
            buttonTool7.SharedPropsInternal.Caption = "Save As";
            appearance35.Image = global::FloorPlanning.Properties.Resources.pdf32;
            buttonTool8.SharedPropsInternal.AppearancesLarge.Appearance = appearance35;
            appearance36.Image = global::FloorPlanning.Properties.Resources.pdf16;
            buttonTool8.SharedPropsInternal.AppearancesSmall.Appearance = appearance36;
            buttonTool8.SharedPropsInternal.Caption = "Load PDF";
            appearance37.Image = global::FloorPlanning.Properties.Resources.zoomin32;
            buttonTool14.SharedPropsInternal.AppearancesLarge.Appearance = appearance37;
            appearance38.Image = global::FloorPlanning.Properties.Resources.zoomin16;
            buttonTool14.SharedPropsInternal.AppearancesSmall.Appearance = appearance38;
            buttonTool14.SharedPropsInternal.Caption = "Zoom In";
            appearance39.Image = global::FloorPlanning.Properties.Resources.zoomout32;
            buttonTool15.SharedPropsInternal.AppearancesLarge.Appearance = appearance39;
            appearance40.Image = global::FloorPlanning.Properties.Resources.zoomout16;
            buttonTool15.SharedPropsInternal.AppearancesSmall.Appearance = appearance40;
            buttonTool15.SharedPropsInternal.Caption = "Zoom Out";
            appearance41.Image = global::FloorPlanning.Properties.Resources.fitwidth32;
            buttonTool16.SharedPropsInternal.AppearancesLarge.Appearance = appearance41;
            appearance42.Image = global::FloorPlanning.Properties.Resources.fitwidth16;
            buttonTool16.SharedPropsInternal.AppearancesSmall.Appearance = appearance42;
            buttonTool16.SharedPropsInternal.Caption = "Fit Width";
            appearance43.Image = global::FloorPlanning.Properties.Resources.fitheight32;
            buttonTool17.SharedPropsInternal.AppearancesLarge.Appearance = appearance43;
            appearance44.Image = global::FloorPlanning.Properties.Resources.fitheight16;
            buttonTool17.SharedPropsInternal.AppearancesSmall.Appearance = appearance44;
            buttonTool17.SharedPropsInternal.Caption = "Fit Height";
            appearance45.Image = global::FloorPlanning.Properties.Resources.zoompercent32;
            buttonTool18.SharedPropsInternal.AppearancesLarge.Appearance = appearance45;
            appearance46.Image = global::FloorPlanning.Properties.Resources.zoompercent16;
            buttonTool18.SharedPropsInternal.AppearancesSmall.Appearance = appearance46;
            buttonTool18.SharedPropsInternal.Caption = "Zoom Percent";
            stateButtonTool2.OptionSetKey = "panoption";
            appearance47.Image = global::FloorPlanning.Properties.Resources.panmode32;
            stateButtonTool2.SharedPropsInternal.AppearancesLarge.Appearance = appearance47;
            appearance48.Image = global::FloorPlanning.Properties.Resources.panmode16;
            stateButtonTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance48;
            stateButtonTool2.SharedPropsInternal.Caption = "Pan Mode";
            stateButtonTool4.OptionSetKey = "panoption";
            appearance49.Image = global::FloorPlanning.Properties.Resources.drawmode32;
            stateButtonTool4.SharedPropsInternal.AppearancesLarge.Appearance = appearance49;
            appearance50.Image = global::FloorPlanning.Properties.Resources.drawmode16;
            stateButtonTool4.SharedPropsInternal.AppearancesSmall.Appearance = appearance50;
            stateButtonTool4.SharedPropsInternal.Caption = "Draw Mode";
            stateButtonTool6.Checked = true;
            stateButtonTool6.OptionSetKey = "areaoption";
            appearance51.Image = global::FloorPlanning.Properties.Resources.areamode32;
            stateButtonTool6.SharedPropsInternal.AppearancesLarge.Appearance = appearance51;
            appearance52.Image = global::FloorPlanning.Properties.Resources.areamode16;
            stateButtonTool6.SharedPropsInternal.AppearancesSmall.Appearance = appearance52;
            stateButtonTool6.SharedPropsInternal.Caption = "Area Mode";
            stateButtonTool8.OptionSetKey = "areaoption";
            appearance53.Image = global::FloorPlanning.Properties.Resources.linemode32;
            stateButtonTool8.SharedPropsInternal.AppearancesLarge.Appearance = appearance53;
            appearance54.Image = global::FloorPlanning.Properties.Resources.linemode16;
            stateButtonTool8.SharedPropsInternal.AppearancesSmall.Appearance = appearance54;
            stateButtonTool8.SharedPropsInternal.Caption = "Line Mode";
            appearance55.Image = global::FloorPlanning.Properties.Resources.filterareas32;
            buttonTool21.SharedPropsInternal.AppearancesLarge.Appearance = appearance55;
            appearance56.Image = global::FloorPlanning.Properties.Resources.filterareas16;
            buttonTool21.SharedPropsInternal.AppearancesSmall.Appearance = appearance56;
            buttonTool21.SharedPropsInternal.Caption = "Filter Areas";
            appearance57.Image = global::FloorPlanning.Properties.Resources.filterlines32;
            buttonTool22.SharedPropsInternal.AppearancesLarge.Appearance = appearance57;
            appearance58.Image = global::FloorPlanning.Properties.Resources.filterlines16;
            buttonTool22.SharedPropsInternal.AppearancesSmall.Appearance = appearance58;
            buttonTool22.SharedPropsInternal.Caption = "Filter Lines";
            appearance59.Image = global::FloorPlanning.Properties.Resources.showareas32;
            buttonTool26.SharedPropsInternal.AppearancesLarge.Appearance = appearance59;
            appearance60.Image = global::FloorPlanning.Properties.Resources.showareas16;
            buttonTool26.SharedPropsInternal.AppearancesSmall.Appearance = appearance60;
            buttonTool26.SharedPropsInternal.Caption = "Show Areas";
            appearance61.Image = global::FloorPlanning.Properties.Resources.hideareas32;
            buttonTool27.SharedPropsInternal.AppearancesLarge.Appearance = appearance61;
            appearance62.Image = global::FloorPlanning.Properties.Resources.hideareas16;
            buttonTool27.SharedPropsInternal.AppearancesSmall.Appearance = appearance62;
            buttonTool27.SharedPropsInternal.Caption = "Hide Areas";
            appearance63.Image = global::FloorPlanning.Properties.Resources.greyareas32;
            buttonTool28.SharedPropsInternal.AppearancesLarge.Appearance = appearance63;
            appearance64.Image = global::FloorPlanning.Properties.Resources.greyareas16;
            buttonTool28.SharedPropsInternal.AppearancesSmall.Appearance = appearance64;
            buttonTool28.SharedPropsInternal.Caption = "Grey Areas";
            appearance65.Image = global::FloorPlanning.Properties.Resources.editarea32;
            buttonTool35.SharedPropsInternal.AppearancesLarge.Appearance = appearance65;
            appearance66.Image = global::FloorPlanning.Properties.Resources.editarea16;
            buttonTool35.SharedPropsInternal.AppearancesSmall.Appearance = appearance66;
            buttonTool35.SharedPropsInternal.Caption = "Edit Area";
            appearance67.Image = global::FloorPlanning.Properties.Resources.editlines32;
            buttonTool36.SharedPropsInternal.AppearancesLarge.Appearance = appearance67;
            appearance68.Image = global::FloorPlanning.Properties.Resources.editlines16;
            buttonTool36.SharedPropsInternal.AppearancesSmall.Appearance = appearance68;
            buttonTool36.SharedPropsInternal.Caption = "Edit Lines";
            appearance69.Image = global::FloorPlanning.Properties.Resources.areasettings32;
            buttonTool40.SharedPropsInternal.AppearancesLarge.Appearance = appearance69;
            appearance70.Image = global::FloorPlanning.Properties.Resources.areasettings16;
            buttonTool40.SharedPropsInternal.AppearancesSmall.Appearance = appearance70;
            buttonTool40.SharedPropsInternal.Caption = "Area Settings";
            buttonTool40.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance71.Image = global::FloorPlanning.Properties.Resources.linesettings32;
            buttonTool41.SharedPropsInternal.AppearancesLarge.Appearance = appearance71;
            appearance72.Image = global::FloorPlanning.Properties.Resources.linesettings16;
            buttonTool41.SharedPropsInternal.AppearancesSmall.Appearance = appearance72;
            buttonTool41.SharedPropsInternal.Caption = "Line Settings";
            buttonTool41.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance73.Image = global::FloorPlanning.Properties.Resources.seamsettings32;
            buttonTool42.SharedPropsInternal.AppearancesLarge.Appearance = appearance73;
            appearance74.Image = global::FloorPlanning.Properties.Resources.seamsettings16;
            buttonTool42.SharedPropsInternal.AppearancesSmall.Appearance = appearance74;
            buttonTool42.SharedPropsInternal.Caption = "Seam Settings";
            buttonTool42.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance75.Image = global::FloorPlanning.Properties.Resources.areaeditsettings32;
            buttonTool45.SharedPropsInternal.AppearancesLarge.Appearance = appearance75;
            appearance76.Image = global::FloorPlanning.Properties.Resources.areaeditsettings16;
            buttonTool45.SharedPropsInternal.AppearancesSmall.Appearance = appearance76;
            buttonTool45.SharedPropsInternal.Caption = "Area Edit Settings";
            buttonTool45.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance77.Image = global::FloorPlanning.Properties.Resources.lineeditsettings32;
            buttonTool46.SharedPropsInternal.AppearancesLarge.Appearance = appearance77;
            appearance78.Image = global::FloorPlanning.Properties.Resources.lineeditsettings16;
            buttonTool46.SharedPropsInternal.AppearancesSmall.Appearance = appearance78;
            buttonTool46.SharedPropsInternal.Caption = "Line Edit Settings";
            buttonTool46.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance79.Image = global::FloorPlanning.Properties.Resources.customscale32;
            buttonTool49.SharedPropsInternal.AppearancesLarge.Appearance = appearance79;
            appearance80.Image = global::FloorPlanning.Properties.Resources.customscale16;
            buttonTool49.SharedPropsInternal.AppearancesSmall.Appearance = appearance80;
            buttonTool49.SharedPropsInternal.Caption = "Custom Scale";
            appearance81.Image = global::FloorPlanning.Properties.Resources.measureline32;
            buttonTool50.SharedPropsInternal.AppearancesLarge.Appearance = appearance81;
            appearance82.Image = global::FloorPlanning.Properties.Resources.measureline16;
            buttonTool50.SharedPropsInternal.AppearancesSmall.Appearance = appearance82;
            buttonTool50.SharedPropsInternal.Caption = "Measure Line";
            appearance83.Image = global::FloorPlanning.Properties.Resources.aligntogrid32;
            buttonTool53.SharedPropsInternal.AppearancesLarge.Appearance = appearance83;
            appearance84.Image = global::FloorPlanning.Properties.Resources.aligntogrid16;
            buttonTool53.SharedPropsInternal.AppearancesSmall.Appearance = appearance84;
            buttonTool53.SharedPropsInternal.Caption = "Align to Grid";
            appearance85.Image = global::FloorPlanning.Properties.Resources.counters32;
            buttonTool54.SharedPropsInternal.AppearancesLarge.Appearance = appearance85;
            appearance86.Image = global::FloorPlanning.Properties.Resources.counters16;
            buttonTool54.SharedPropsInternal.AppearancesSmall.Appearance = appearance86;
            buttonTool54.SharedPropsInternal.Caption = "Counters";
            appearance87.Image = global::FloorPlanning.Properties.Resources.selectcolors32;
            buttonTool56.SharedPropsInternal.AppearancesLarge.Appearance = appearance87;
            appearance88.Image = global::FloorPlanning.Properties.Resources.selectcolors16;
            buttonTool56.SharedPropsInternal.AppearancesSmall.Appearance = appearance88;
            buttonTool56.SharedPropsInternal.Caption = "Select Colors";
            appearance89.Image = global::FloorPlanning.Properties.Resources.new32;
            buttonTool38.SharedPropsInternal.AppearancesLarge.Appearance = appearance89;
            appearance90.Image = global::FloorPlanning.Properties.Resources.new16;
            buttonTool38.SharedPropsInternal.AppearancesSmall.Appearance = appearance90;
            buttonTool38.SharedPropsInternal.Caption = "New";
            buttonTool38.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.DefaultForToolType;
            popupMenuTool2.Settings.PopupStyle = Infragistics.Win.UltraWinToolbars.PopupStyle.Toolbar;
            popupMenuTool2.Settings.UseLargeImages = Infragistics.Win.DefaultableBoolean.True;
            appearance91.Image = global::FloorPlanning.Properties.Resources.showsettings32;
            popupMenuTool2.SharedPropsInternal.AppearancesLarge.Appearance = appearance91;
            appearance92.Image = global::FloorPlanning.Properties.Resources.showsettings16;
            popupMenuTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance92;
            popupMenuTool2.SharedPropsInternal.Caption = "Show Settings";
            popupMenuTool2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool3,
            popupMenuTool5,
            buttonTool29});
            buttonTool44.SharedPropsInternal.Caption = "ButtonTool1";
            buttonTool57.SharedPropsInternal.Caption = "ButtonTool2";
            buttonTool57.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            popupMenuTool4.DropDownArrowStyle = Infragistics.Win.UltraWinToolbars.DropDownArrowStyle.SegmentedStateButton;
            popupMenuTool4.InstanceProps.MinimumSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            popupMenuTool4.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            popupMenuTool4.Settings.PopupStyle = Infragistics.Win.UltraWinToolbars.PopupStyle.Toolbar;
            popupMenuTool4.SharedPropsInternal.Caption = "Finish Settings";
            popupMenuTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            popupMenuTool4.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool59,
            buttonTool60,
            buttonTool61});
            popupMenuTool6.DropDownArrowStyle = Infragistics.Win.UltraWinToolbars.DropDownArrowStyle.SegmentedStateButton;
            popupMenuTool6.Settings.PopupStyle = Infragistics.Win.UltraWinToolbars.PopupStyle.Toolbar;
            popupMenuTool6.Settings.UseLargeImages = Infragistics.Win.DefaultableBoolean.True;
            popupMenuTool6.SharedPropsInternal.Caption = "Edit Finish Settings";
            popupMenuTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            popupMenuTool6.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool62,
            buttonTool63});
            stateButtonTool10.Checked = true;
            stateButtonTool10.OptionSetKey = "panoption";
            stateButtonTool10.SharedPropsInternal.Caption = "Page Mode";
            stateButtonTool10.SharedPropsInternal.Visible = false;
            stateButtonTool13.Checked = true;
            stateButtonTool13.OptionSetKey = "showimageoption";
            appearance93.Image = global::FloorPlanning.Properties.Resources.showimage32;
            stateButtonTool13.SharedPropsInternal.AppearancesLarge.Appearance = appearance93;
            appearance94.Image = global::FloorPlanning.Properties.Resources.showimage16;
            stateButtonTool13.SharedPropsInternal.AppearancesSmall.Appearance = appearance94;
            stateButtonTool13.SharedPropsInternal.Caption = "Show Image";
            stateButtonTool15.OptionSetKey = "showimageoption";
            appearance95.Image = global::FloorPlanning.Properties.Resources.hideimage32;
            stateButtonTool15.SharedPropsInternal.AppearancesLarge.Appearance = appearance95;
            appearance96.Image = global::FloorPlanning.Properties.Resources.hideimage16;
            stateButtonTool15.SharedPropsInternal.AppearancesSmall.Appearance = appearance96;
            stateButtonTool15.SharedPropsInternal.Caption = "Hide Image";
            appearance97.FontData.BoldAsString = "True";
            labelTool2.SharedPropsInternal.AppearancesLarge.Appearance = appearance97;
            labelTool2.SharedPropsInternal.Caption = "Project";
            labelTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.DefaultForToolType;
            appearance98.Image = global::FloorPlanning.Properties.Resources.pdf32;
            buttonTool69.SharedPropsInternal.AppearancesSmall.Appearance = appearance98;
            buttonTool69.SharedPropsInternal.Caption = "Reload PDF";
            labelTool4.SharedPropsInternal.Caption = "Reports";
            buttonTool30.SharedPropsInternal.Caption = "ButtonTool3";
            buttonTool31.SharedPropsInternal.Caption = "Shortcuts";
            labelTool6.SharedPropsInternal.Caption = "Shortcuts";
            buttonTool32.SharedPropsInternal.Caption = "ButtonTool4";
            stateButtonTool17.SharedPropsInternal.Caption = "StateButtonTool1";
            labelTool7.SharedPropsInternal.Caption = "LabelTool2";
            labelTool7.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            popupMenuTool8.SharedPropsInternal.Caption = "PopupMenuTool2";
            popupMenuTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool39.SharedPropsInternal.Caption = "Shortcuts";
            buttonTool39.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool5,
            buttonTool6,
            buttonTool7,
            buttonTool8,
            buttonTool14,
            buttonTool15,
            buttonTool16,
            buttonTool17,
            buttonTool18,
            stateButtonTool2,
            stateButtonTool4,
            stateButtonTool6,
            stateButtonTool8,
            buttonTool21,
            buttonTool22,
            buttonTool26,
            buttonTool27,
            buttonTool28,
            buttonTool35,
            buttonTool36,
            buttonTool40,
            buttonTool41,
            buttonTool42,
            buttonTool45,
            buttonTool46,
            buttonTool49,
            buttonTool50,
            buttonTool53,
            buttonTool54,
            buttonTool56,
            buttonTool38,
            popupMenuTool2,
            buttonTool44,
            buttonTool57,
            popupMenuTool4,
            popupMenuTool6,
            stateButtonTool10,
            stateButtonTool11,
            stateButtonTool13,
            stateButtonTool15,
            labelTool2,
            buttonTool69,
            labelTool4,
            buttonTool30,
            buttonTool31,
            labelTool6,
            buttonTool32,
            stateButtonTool17,
            labelTool7,
            popupMenuTool8,
            buttonTool39});
            this.ultraToolbarsManager1.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.ultraToolbarsManager1_BeforeToolbarListDropdown);
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            this.ultraToolbarsManager1.PropertyChanged += new Infragistics.Win.PropertyChangedEventHandler(this.ultraToolbarsManager1_PropertyChanged);
            // 
            // _DrawingForm_Toolbars_Dock_Area_Right
            // 
            this._DrawingForm_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DrawingForm_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DrawingForm_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._DrawingForm_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DrawingForm_Toolbars_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._DrawingForm_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1303, 162);
            this._DrawingForm_Toolbars_Dock_Area_Right.Name = "_DrawingForm_Toolbars_Dock_Area_Right";
            this._DrawingForm_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(8, 742);
            this._DrawingForm_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _DrawingForm_Toolbars_Dock_Area_Top
            // 
            this._DrawingForm_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DrawingForm_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DrawingForm_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._DrawingForm_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DrawingForm_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._DrawingForm_Toolbars_Dock_Area_Top.Name = "_DrawingForm_Toolbars_Dock_Area_Top";
            this._DrawingForm_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1311, 162);
            this._DrawingForm_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _DrawingForm_Toolbars_Dock_Area_Bottom
            // 
            this._DrawingForm_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DrawingForm_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DrawingForm_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._DrawingForm_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DrawingForm_Toolbars_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._DrawingForm_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 904);
            this._DrawingForm_Toolbars_Dock_Area_Bottom.Name = "_DrawingForm_Toolbars_Dock_Area_Bottom";
            this._DrawingForm_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1311, 8);
            this._DrawingForm_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomToolStripPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 478);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(1232, 22);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(1232, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightToolStripPanel.Location = new System.Drawing.Point(1232, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 478);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 478);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(1232, 478);
            // 
            // ultraDockManager1
            // 
            this.ultraDockManager1.CaptionStyle = Infragistics.Win.UltraWinDock.CaptionStyle.Office2007;
            this.ultraDockManager1.CompressUnpinnedTabs = false;
            this.ultraDockManager1.DefaultPaneSettings.AllowResize = Infragistics.Win.DefaultableBoolean.False;
            dockAreaPane1.DockedBefore = new System.Guid("cb996483-00b0-43d9-8409-11f2c2b1bed4");
            dockableControlPane1.Control = this.statusStrip;
            dockableControlPane1.OriginalControlBounds = new System.Drawing.Rectangle(50, 454, 288, 22);
            dockableControlPane1.Settings.ShowCaption = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane1.Size = new System.Drawing.Size(100, 100);
            dockableControlPane1.Text = "statusStrip";
            dockAreaPane1.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane1});
            dockAreaPane1.Size = new System.Drawing.Size(1295, 29);
            dockAreaPane2.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.VerticalSplit;
            dockAreaPane2.DockedBefore = new System.Guid("350ad495-80c8-47a9-b577-26feb9195940");
            dockableControlPane2.Control = this.tabPanes;
            dockableControlPane2.FlyoutSize = new System.Drawing.Size(249, -1);
            dockableControlPane2.OriginalControlBounds = new System.Drawing.Rectangle(83, 273, 200, 100);
            dockableControlPane2.Size = new System.Drawing.Size(100, 100);
            dockableControlPane2.Text = "       Tab Panes";
            dockAreaPane2.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane2});
            dockAreaPane2.Size = new System.Drawing.Size(249, 708);
            dockAreaPane3.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.VerticalSplit;
            dockableControlPane3.Control = this.ultraPanelRight;
            dockableControlPane3.FlyoutSize = new System.Drawing.Size(200, -1);
            dockableControlPane3.OriginalControlBounds = new System.Drawing.Rectangle(1113, 238, 200, 100);
            dockableControlPane3.Size = new System.Drawing.Size(100, 100);
            dockableControlPane3.Text = "Area mode";
            dockAreaPane3.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane3});
            dockAreaPane3.Size = new System.Drawing.Size(200, 708);
            this.ultraDockManager1.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane1,
            dockAreaPane2,
            dockAreaPane3});
            this.ultraDockManager1.HostControl = this;
            this.ultraDockManager1.ShowCloseButton = false;
            this.ultraDockManager1.ShowPinButton = false;
            this.ultraDockManager1.ShowToolTips = false;
            this.ultraDockManager1.ShowUnpinnedTabAreas = Infragistics.Win.DefaultableBoolean.True;
            this.ultraDockManager1.UnpinnedTabHoverAction = Infragistics.Win.UltraWinDock.UnpinnedTabHoverAction.None;
            this.ultraDockManager1.WindowStyle = Infragistics.Win.UltraWinDock.WindowStyle.Office2007;
            // 
            // _DrawingFormUnpinnedTabAreaLeft
            // 
            this._DrawingFormUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._DrawingFormUnpinnedTabAreaLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._DrawingFormUnpinnedTabAreaLeft.Location = new System.Drawing.Point(8, 162);
            this._DrawingFormUnpinnedTabAreaLeft.Name = "_DrawingFormUnpinnedTabAreaLeft";
            this._DrawingFormUnpinnedTabAreaLeft.Owner = this.ultraDockManager1;
            this._DrawingFormUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 742);
            this._DrawingFormUnpinnedTabAreaLeft.TabIndex = 16;
            // 
            // _DrawingFormUnpinnedTabAreaRight
            // 
            this._DrawingFormUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._DrawingFormUnpinnedTabAreaRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._DrawingFormUnpinnedTabAreaRight.Location = new System.Drawing.Point(1303, 162);
            this._DrawingFormUnpinnedTabAreaRight.Name = "_DrawingFormUnpinnedTabAreaRight";
            this._DrawingFormUnpinnedTabAreaRight.Owner = this.ultraDockManager1;
            this._DrawingFormUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 742);
            this._DrawingFormUnpinnedTabAreaRight.TabIndex = 17;
            // 
            // _DrawingFormUnpinnedTabAreaTop
            // 
            this._DrawingFormUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._DrawingFormUnpinnedTabAreaTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._DrawingFormUnpinnedTabAreaTop.Location = new System.Drawing.Point(8, 162);
            this._DrawingFormUnpinnedTabAreaTop.Name = "_DrawingFormUnpinnedTabAreaTop";
            this._DrawingFormUnpinnedTabAreaTop.Owner = this.ultraDockManager1;
            this._DrawingFormUnpinnedTabAreaTop.Size = new System.Drawing.Size(1295, 0);
            this._DrawingFormUnpinnedTabAreaTop.TabIndex = 18;
            // 
            // _DrawingFormUnpinnedTabAreaBottom
            // 
            this._DrawingFormUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._DrawingFormUnpinnedTabAreaBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._DrawingFormUnpinnedTabAreaBottom.Location = new System.Drawing.Point(8, 904);
            this._DrawingFormUnpinnedTabAreaBottom.Name = "_DrawingFormUnpinnedTabAreaBottom";
            this._DrawingFormUnpinnedTabAreaBottom.Owner = this.ultraDockManager1;
            this._DrawingFormUnpinnedTabAreaBottom.Size = new System.Drawing.Size(1295, 0);
            this._DrawingFormUnpinnedTabAreaBottom.TabIndex = 19;
            // 
            // _DrawingFormAutoHideControl
            // 
            this._DrawingFormAutoHideControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._DrawingFormAutoHideControl.Location = new System.Drawing.Point(29, 162);
            this._DrawingFormAutoHideControl.Name = "_DrawingFormAutoHideControl";
            this._DrawingFormAutoHideControl.Owner = this.ultraDockManager1;
            this._DrawingFormAutoHideControl.Size = new System.Drawing.Size(14, 704);
            this._DrawingFormAutoHideControl.TabIndex = 20;
            // 
            // dockableWindow3
            // 
            this.dockableWindow3.Controls.Add(this.statusStrip);
            this.dockableWindow3.Location = new System.Drawing.Point(0, 5);
            this.dockableWindow3.Name = "dockableWindow3";
            this.dockableWindow3.Owner = this.ultraDockManager1;
            this.dockableWindow3.Size = new System.Drawing.Size(1295, 29);
            this.dockableWindow3.TabIndex = 41;
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Controls.Add(this.tabPanes);
            this.dockableWindow1.Location = new System.Drawing.Point(0, 0);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.ultraDockManager1;
            this.dockableWindow1.Size = new System.Drawing.Size(249, 708);
            this.dockableWindow1.TabIndex = 42;
            // 
            // windowDockingArea1
            // 
            this.windowDockingArea1.Controls.Add(this.dockableWindow3);
            this.windowDockingArea1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.windowDockingArea1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windowDockingArea1.Location = new System.Drawing.Point(8, 870);
            this.windowDockingArea1.Name = "windowDockingArea1";
            this.windowDockingArea1.Owner = this.ultraDockManager1;
            this.windowDockingArea1.Size = new System.Drawing.Size(1295, 34);
            this.windowDockingArea1.TabIndex = 21;
            // 
            // dockableWindow4
            // 
            this.dockableWindow4.Controls.Add(this.ultraPanelRight);
            this.dockableWindow4.Location = new System.Drawing.Point(5, 0);
            this.dockableWindow4.Name = "dockableWindow4";
            this.dockableWindow4.Owner = this.ultraDockManager1;
            this.dockableWindow4.Size = new System.Drawing.Size(200, 708);
            this.dockableWindow4.TabIndex = 43;
            // 
            // btnLeft
            // 
            appearance24.BackColor = System.Drawing.Color.Transparent;
            appearance24.BackColor2 = System.Drawing.Color.Transparent;
            appearance24.BackColorDisabled = System.Drawing.Color.Transparent;
            appearance24.BackColorDisabled2 = System.Drawing.Color.Transparent;
            appearance24.BorderColor = System.Drawing.Color.Transparent;
            appearance24.BorderColor2 = System.Drawing.Color.Transparent;
            appearance24.BorderColor3DBase = System.Drawing.Color.Transparent;
            appearance24.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnLeft.Appearance = appearance24;
            this.btnLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLeft.Location = new System.Drawing.Point(2, 161);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(20, 18);
            this.btnLeft.TabIndex = 35;
            this.btnLeft.Text = "<";
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnRight
            // 
            appearance25.BackColor = System.Drawing.Color.Transparent;
            appearance25.BackColor2 = System.Drawing.Color.Transparent;
            appearance25.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance25.BackColorDisabled = System.Drawing.Color.Transparent;
            appearance25.BackColorDisabled2 = System.Drawing.Color.Transparent;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance25.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance25.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance25.BorderColor = System.Drawing.Color.Transparent;
            appearance25.BorderColor2 = System.Drawing.Color.Transparent;
            appearance25.BorderColor3DBase = System.Drawing.Color.Transparent;
            appearance25.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnRight.Appearance = appearance25;
            this.btnRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRight.Location = new System.Drawing.Point(1268, 161);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(20, 18);
            this.btnRight.TabIndex = 36;
            this.btnRight.Text = ">";
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // windowDockingArea2
            // 
            this.windowDockingArea2.Controls.Add(this.dockableWindow1);
            this.windowDockingArea2.Dock = System.Windows.Forms.DockStyle.Left;
            this.windowDockingArea2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windowDockingArea2.Location = new System.Drawing.Point(8, 162);
            this.windowDockingArea2.Name = "windowDockingArea2";
            this.windowDockingArea2.Owner = this.ultraDockManager1;
            this.windowDockingArea2.Size = new System.Drawing.Size(254, 708);
            this.windowDockingArea2.TabIndex = 23;
            // 
            // windowDockingArea3
            // 
            this.windowDockingArea3.Controls.Add(this.dockableWindow4);
            this.windowDockingArea3.Dock = System.Windows.Forms.DockStyle.Right;
            this.windowDockingArea3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windowDockingArea3.Location = new System.Drawing.Point(1098, 162);
            this.windowDockingArea3.Name = "windowDockingArea3";
            this.windowDockingArea3.Owner = this.ultraDockManager1;
            this.windowDockingArea3.Size = new System.Drawing.Size(205, 708);
            this.windowDockingArea3.TabIndex = 25;
            // 
            // pageWindow
            // 
            this.pageWindow.AllowDrop = true;
            this.pageWindow.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pageWindow.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pageWindow.CurrentCursor = System.Windows.Forms.Cursors.Default;
            this.pageWindow.Cursor = System.Windows.Forms.Cursors.Default;
            this.pageWindow.DesktopColor = System.Drawing.Color.WhiteSmoke;
            this.pageWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageWindow.FirstPointPicked = ((System.Drawing.PointF)(resources.GetObject("pageWindow.FirstPointPicked")));
            this.pageWindow.IsDragging = false;
            this.pageWindow.Location = new System.Drawing.Point(262, 162);
            this.pageWindow.Margin = new System.Windows.Forms.Padding(0);
            this.pageWindow.Name = "pageWindow";
            this.pageWindow.Orientation = PdfSharp.PageOrientation.Portrait;
            this.pageWindow.PageColor = System.Drawing.Color.White;
            this.pageWindow.PageSize = ((PdfSharp.Drawing.XSize)(resources.GetObject("pageWindow.PageSize")));
            this.pageWindow.Size = new System.Drawing.Size(836, 708);
            this.pageWindow.TabIndex = 30;
            this.pageWindow.ZoomPercent = 62.35154F;
            this.pageWindow.ZoomChanged += new System.EventHandler(this.pageWindow_ZoomChanged);
            this.pageWindow.Load += new System.EventHandler(this.pageWindow_Load);
            this.pageWindow.Click += new System.EventHandler(this.pageWindow_Click);
            this.pageWindow.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pageWindow_MouseClick);
            // 
            // DrawingForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1311, 912);
            this.Controls.Add(this._DrawingFormAutoHideControl);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.pageWindow);
            this.Controls.Add(this.windowDockingArea3);
            this.Controls.Add(this.windowDockingArea2);
            this.Controls.Add(this.windowDockingArea1);
            this.Controls.Add(this._DrawingFormUnpinnedTabAreaBottom);
            this.Controls.Add(this._DrawingFormUnpinnedTabAreaTop);
            this.Controls.Add(this._DrawingFormUnpinnedTabAreaRight);
            this.Controls.Add(this._DrawingFormUnpinnedTabAreaLeft);
            this.Controls.Add(this._DrawingForm_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._DrawingForm_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._DrawingForm_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._DrawingForm_Toolbars_Dock_Area_Top);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(780, 440);
            this.Name = "DrawingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FloorPlanning";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DrawingForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DrawingForm_FormClosed);
            this.Load += new System.EventHandler(this.DrawingForm_Load);
            this.SizeChanged += new System.EventHandler(this.DrawingForm_SizeChanged);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.pnlTabUp.ClientArea.ResumeLayout(false);
            this.pnlTabUp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctPage)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraPanelAreaTab.ClientArea.ResumeLayout(false);
            this.ultraPanelAreaTab.ClientArea.PerformLayout();
            this.ultraPanelAreaTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtTransparencyPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barTransparency)).EndInit();
            this.pnlAreaTab.ClientArea.ResumeLayout(false);
            this.pnlAreaTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpAreaTab)).EndInit();
            this.grpAreaTab.ResumeLayout(false);
            this.pnlFinishUCs.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            this.ultraPanelLineTab.ClientArea.ResumeLayout(false);
            this.ultraPanelLineTab.ClientArea.PerformLayout();
            this.ultraPanelLineTab.ResumeLayout(false);
            this.pnlLineProps.ResumeLayout(false);
            this.pnlLineTab.ClientArea.ResumeLayout(false);
            this.pnlLineTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpLineTab)).EndInit();
            this.grpLineTab.ResumeLayout(false);
            this.pnlLineUCs.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabPanes)).EndInit();
            this.tabPanes.ResumeLayout(false);
            this.ultraPanelRight.ClientArea.ResumeLayout(false);
            this.ultraPanelRight.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox5)).EndInit();
            this.ultraGroupBox5.ResumeLayout(false);
            this.ultraGroupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox4)).EndInit();
            this.ultraGroupBox4.ResumeLayout(false);
            this.ultraGroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCustom)).EndInit();
            this.grpCustom.ResumeLayout(false);
            this.grpCustom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpTrim)).EndInit();
            this.grpTrim.ResumeLayout(false);
            this.grpTrim.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrimFactor)).EndInit();
            this.ultraPanelBackRight.ClientArea.ResumeLayout(false);
            this.ultraPanelBackRight.ResumeLayout(false);
            this.pnlAreaModeInRight.ClientArea.ResumeLayout(false);
            this.pnlAreaModeInRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpAreaInRight)).EndInit();
            this.grpAreaInRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).EndInit();
            this.ultraGroupBox3.ResumeLayout(false);
            this.pnlLineModeRightPanel.ClientArea.ResumeLayout(false);
            this.pnlLineModeRightPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpLineInRight)).EndInit();
            this.grpLineInRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox8)).EndInit();
            this.ultraGroupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpDoorTakeOut)).EndInit();
            this.grpDoorTakeOut.ResumeLayout(false);
            this.grpDoorTakeOut.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOtherFt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox9)).EndInit();
            this.ultraGroupBox9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).EndInit();
            this.dockableWindow3.ResumeLayout(false);
            this.dockableWindow3.PerformLayout();
            this.dockableWindow1.ResumeLayout(false);
            this.windowDockingArea1.ResumeLayout(false);
            this.dockableWindow4.ResumeLayout(false);
            this.windowDockingArea2.ResumeLayout(false);
            this.windowDockingArea3.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _DrawingForm_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _DrawingForm_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _DrawingForm_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _DrawingForm_Toolbars_Dock_Area_Top;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusStripPrompt;
        private ToolStripStatusLabel statusStripScale;
        private ToolStripStatusLabel statusStripZoom;
        private ToolStripPanel BottomToolStripPanel;
        private ToolStripPanel TopToolStripPanel;
        private ToolStripPanel RightToolStripPanel;
        private ToolStripPanel LeftToolStripPanel;
        private ToolStripContentPanel ContentPanel;
        private Infragistics.Win.UltraWinDock.AutoHideControl _DrawingFormAutoHideControl;
        private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabPanes;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.Misc.UltraPanel ultraPanelRight;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea3;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow3;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea2;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea1;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DrawingFormUnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DrawingFormUnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DrawingFormUnpinnedTabAreaRight;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DrawingFormUnpinnedTabAreaLeft;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private Infragistics.Win.Misc.UltraLabel lblPageNumber;
        private Infragistics.Win.Misc.UltraPanel pnlTabUp;
        private PictureBox pctPage;
        private PageWindow pageWindow;
        private Infragistics.Win.Misc.UltraPanel pnlAreaModeInRight;
        private Infragistics.Win.Misc.UltraPanel pnlAreaTab;
        private Infragistics.Win.Misc.UltraGroupBox grpAreaTab;
        private Infragistics.Win.Misc.UltraPanel pnlFinishUCs;
        private Infragistics.Win.Misc.UltraPanel ultraPanelAreaTab;
        private Label label13;
        private Label lblLastTotalPerimeterWhole;
        private Label lblLastTotalPerimeterDecimal;
        private Label label16;
        private Label label5;
        private Label lblLastAreaTotalWhole;
        private Label lblLastAreaTotalDecimal;
        private Label label8;
        private Label label9;
        private Label lblLastPerimeterWhole;
        private Label lblLastPerimeterDecimal;
        private Label label12;
        private Label label1;
        private Label lblLastAreaWhole;
        private Label lblLastAreaDecimal;
        private Label label4;
        private Label lblAreaPoint;
        private Label lblLastWasteWhole;
        private Label lblLastWasteDecimal;
        private Label lblAreaText;
        private Infragistics.Win.Misc.UltraPanel ultraPanelLineTab;
        private Label label2;
        private Label lblLastLineWhole;
        private Label label6;
        private Label lblLastLineDecimal;
        private Label label15;
        private Label lblTotalLineDecimal;
        private Label lblTotalLineWhole;
        private Label label23;
        private Infragistics.Win.Misc.UltraButton btnEditLine;
        private Infragistics.Win.Misc.UltraPanel pnlLineTab;
        private Infragistics.Win.Misc.UltraGroupBox grpLineTab;
        private Infragistics.Win.Misc.UltraPanel pnlLineUCs;
        private Infragistics.Win.Misc.UltraPanel pnlLineProps;
        private Infragistics.Win.Misc.UltraPanel ultraPanelBackRight;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox5;
        private RadioButton rdNoX1Y1;
        private RadioButton rdYesX1Y1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox4;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbScale;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkScale;
        private Infragistics.Win.Misc.UltraGroupBox grpTrim;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbTrimFactor;
        private Infragistics.Win.Misc.UltraGroupBox grpAreaInRight;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraButton btnAreaZeroLine;
        private Infragistics.Win.Misc.UltraButton ultraButton4;
        private Infragistics.Win.Misc.UltraButton ultraButton3;
        private Infragistics.Win.Misc.UltraButton btnAreaTakeOut;
        private Infragistics.Win.Misc.UltraButton btnComplete;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox3;
        private Infragistics.Win.Misc.UltraLabel lblSelectedArea;
        private Infragistics.Win.Misc.UltraButton btnEmptyShape;
        private Infragistics.Win.Misc.UltraButton ultraButton2;
        private Infragistics.Win.Misc.UltraPanel pnlLineModeRightPanel;
        private Infragistics.Win.Misc.UltraGroupBox grpLineInRight;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox8;
        private Infragistics.Win.Misc.UltraGroupBox grpDoorTakeOut;
        private Infragistics.Win.Misc.UltraButton btnDeactivateLineDoorTO;
        private Infragistics.Win.Misc.UltraButton btnShowLineDoorTO;
        private Infragistics.Win.Misc.UltraButton btnActivateLineDoorTO;
        private Infragistics.Win.Misc.UltraButton btnHideLineDoorTO;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtOtherFt;
        private RadioButton rdOtherft;
        private RadioButton rd6f;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private RadioButton rd3f;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox9;
        private Infragistics.Win.Misc.UltraButton btnLineZeroLine;
        private Infragistics.Win.Misc.UltraButton btnDuplicate;
        private Infragistics.Win.Misc.UltraButton btnJump;
        private Infragistics.Win.Misc.UltraButton btn2x;
        private Infragistics.Win.Misc.UltraButton btn1x;
        private TrackBar barTransparency;
        private Label label3;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtTransparencyPercent;
        private Infragistics.Win.Misc.UltraButton btnLeft;
        private Infragistics.Win.Misc.UltraButton btnRight;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow4;
        private Infragistics.Win.Misc.UltraGroupBox grpCustom;
        private Label lbClick2NdPoint;
        private Label lbClick1StPoint;
        private TextBox txtScaleInches;
        private TextBox txtScaleFeet;
        private Label label10;
        private Label label7;
        private Infragistics.Win.Misc.UltraButton btnScaleOk;
        private Infragistics.Win.Misc.UltraButton btnScaleReset;
        private Infragistics.Win.Misc.UltraButton btnScaleCancel;
        private Infragistics.Win.Misc.UltraButton btnScaleNew;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraButton ultraButton6;
    }
}