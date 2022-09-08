using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using FloorPlanning;
using FloorPlanning.Models;
using FloorPlanning.Display.enums;

namespace FloorPlanning
{
    public partial class FormShortcuts : Form
    {
        private char noneCharCode = '-';
        private string noneCharName = "NONE";
        DrawingForm oForm = null;
        public FormShortcuts(DrawingForm l_Form)
        {
            InitializeComponent();
            oForm = l_Form;

            cboAreaEraseLastLine.DisplayMember = "DisplayText";
            cboAreaEraseLastLine.ValueMember = "Keystroke";

            cboAreaCompleteShape.DisplayMember = "DisplayText";
            cboAreaCompleteShape.ValueMember = "Keystroke";

            cboAreaCancelShape.DisplayMember = "DisplayText";
            cboAreaCancelShape.ValueMember = "Keystroke";

            cboAreaZeroLine.DisplayMember = "DisplayText";
            cboAreaZeroLine.ValueMember = "Keystroke";

            cboLineAraseLastLine.DisplayMember = "DisplayText";
            cboLineAraseLastLine.ValueMember = "Keystroke";

            cboLineJump.DisplayMember = "DisplayText";
            cboLineJump.ValueMember = "Keystroke";

            cboLineSingleLine.DisplayMember = "DisplayText";
            cboLineSingleLine.ValueMember = "Keystroke";

            cboLineDoubleLine.DisplayMember = "DisplayText";
            cboLineDoubleLine.ValueMember = "Keystroke";

            cboLineDuplicateLine.DisplayMember = "DisplayText";
            cboLineDuplicateLine.ValueMember = "Keystroke";

            cboGeneralMoveLeft.DisplayMember = "DisplayText";
            cboGeneralMoveLeft.ValueMember = "Keystroke";

            cboGeneralMoveRight.DisplayMember = "DisplayText";
            cboGeneralMoveRight.ValueMember = "Keystroke";


            cboGeneralMoveUp.DisplayMember = "DisplayText";
            cboGeneralMoveUp.ValueMember = "Keystroke";

            cboGeneralMoveDown.DisplayMember = "DisplayText";
            cboGeneralMoveDown.ValueMember = "Keystroke";

            cboGeneralZoomIn.DisplayMember = "DisplayText";
            cboGeneralZoomIn.ValueMember = "Keystroke";

            cboGeneralZoomOut.DisplayMember = "DisplayText";
            cboGeneralZoomOut.ValueMember = "Keystroke";

            cboGeneralToggleAreaBase.DisplayMember = "DisplayText";
            cboGeneralToggleAreaBase.ValueMember = "Keystroke";

            this.ControlBox = false;

        }

        private void FormShortcuts_Load(object sender, EventArgs e)
        {
            cboAreaEraseLastLine.DataSource = buildSourceListForCombobox();
            cboAreaCompleteShape.DataSource = buildSourceListForCombobox();
            cboAreaCancelShape.DataSource = buildSourceListForCombobox();
            cboAreaZeroLine.DataSource = buildSourceListForCombobox();

            cboLineAraseLastLine.DataSource = buildSourceListForCombobox();
            cboLineJump.DataSource = buildSourceListForCombobox();
            cboLineSingleLine.DataSource = buildSourceListForCombobox();
            cboLineDoubleLine.DataSource = buildSourceListForCombobox();
            cboLineDuplicateLine.DataSource = buildSourceListForCombobox();

            cboGeneralMoveLeft.DataSource = buildSourceListForCombobox();
            cboGeneralMoveRight.DataSource = buildSourceListForCombobox();
            cboGeneralMoveUp.DataSource = buildSourceListForCombobox();
            cboGeneralMoveDown.DataSource = buildSourceListForCombobox();
            cboGeneralZoomIn.DataSource = buildSourceListForCombobox();
            cboGeneralZoomOut.DataSource = buildSourceListForCombobox();
            cboGeneralToggleAreaBase.DataSource = buildSourceListForCombobox();

            if (oForm.lConfigFinishLine.lShortcuts.Count == 0)
            {
                cboAreaEraseLastLine.SelectedIndex = 0;
                cboAreaCompleteShape.SelectedIndex = 0;
                cboAreaCancelShape.SelectedIndex = 0;
                cboAreaZeroLine.SelectedIndex = 0;

                cboLineAraseLastLine.SelectedIndex = 0;
                cboLineJump.SelectedIndex = 0;
                cboLineSingleLine.SelectedIndex = 0;
                cboLineDoubleLine.SelectedIndex = 0;
                cboLineDuplicateLine.SelectedIndex = 0;

                cboGeneralMoveLeft.SelectedIndex = 0;
                cboGeneralMoveRight.SelectedIndex = 0;
                cboGeneralMoveUp.SelectedIndex = 0;
                cboGeneralMoveDown.SelectedIndex = 0;
                cboGeneralZoomIn.SelectedIndex = 0;
                cboGeneralZoomOut.SelectedIndex = 0;
                cboGeneralToggleAreaBase.SelectedIndex = 0;
            }
            else
            {
                var cboAreaEraseLastLineObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "AREA" && x.Action == ShortcutsAction.AreaMode_Erase_last_line.ToString()).FirstOrDefault();
                if (cboAreaEraseLastLineObj != null)
                {
                    cboAreaEraseLastLine.SelectedIndex = getIndexValueOfKeys(cboAreaEraseLastLineObj.Keystroke);
                }


                var cboAreaCompleteShapeObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "AREA" && x.Action == ShortcutsAction.AreaMode_Complete_shape.ToString()).FirstOrDefault();
                if (cboAreaCompleteShapeObj != null)
                {
                    cboAreaCompleteShape.SelectedIndex = getIndexValueOfKeys(cboAreaCompleteShapeObj.Keystroke);
                }

                var cboAreaCancelShapeObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "AREA" && x.Action == ShortcutsAction.AreaMode_Cancel_shape_in_progress.ToString()).FirstOrDefault();
                if (cboAreaCancelShapeObj != null)
                {
                    cboAreaCancelShape.SelectedIndex = getIndexValueOfKeys(cboAreaCancelShapeObj.Keystroke);
                }

                var cboAreaZeroLineObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "AREA" && x.Action == ShortcutsAction.AreaMode_Zero_line.ToString()).FirstOrDefault();
                if (cboAreaZeroLineObj != null)
                {
                    cboAreaZeroLine.SelectedIndex = getIndexValueOfKeys(cboAreaZeroLineObj.Keystroke);
                }

                var cboLineAraseLastLineObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "LINE" && x.Action == ShortcutsAction.LineMode_Erase_last_line.ToString()).FirstOrDefault();
                if (cboLineAraseLastLineObj != null)
                {
                    cboLineAraseLastLine.SelectedIndex = getIndexValueOfKeys(cboLineAraseLastLineObj.Keystroke);
                }

                var cboLineJumpObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "LINE" && x.Action == ShortcutsAction.LineMode_Jump.ToString()).FirstOrDefault();
                if (cboLineJumpObj != null)
                {
                    cboLineJump.SelectedIndex = getIndexValueOfKeys(cboLineJumpObj.Keystroke);
                }

                var cboLineSingleLineObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "LINE" && x.Action == ShortcutsAction.LineMode_Single_line.ToString()).FirstOrDefault();
                if (cboLineSingleLineObj != null)
                {
                    cboLineSingleLine.SelectedIndex = getIndexValueOfKeys(cboLineSingleLineObj.Keystroke);
                }

                var cboLineDoubleLineObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "LINE" && x.Action == ShortcutsAction.LineMode_Double_line.ToString()).FirstOrDefault();
                if (cboLineDoubleLineObj != null)
                {
                    cboLineDoubleLine.SelectedIndex = getIndexValueOfKeys(cboLineDoubleLineObj.Keystroke);
                }

                var cboLineDuplicateLineObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "LINE" && x.Action == ShortcutsAction.LineMode_Duplicate_line.ToString()).FirstOrDefault();
                if (cboLineDuplicateLineObj != null)
                {
                    cboLineDuplicateLine.SelectedIndex = getIndexValueOfKeys(cboLineDuplicateLineObj.Keystroke);
                }

                var cboGeneralMoveLeftObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "GENERA" && x.Action == ShortcutsAction.GeneralMode_Move_left.ToString()).FirstOrDefault();
                if (cboGeneralMoveLeftObj != null)
                {
                    cboGeneralMoveLeft.SelectedIndex = getIndexValueOfKeys(cboGeneralMoveLeftObj.Keystroke);
                }

                var cboGeneralMoveRightObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "GENERA" && x.Action == ShortcutsAction.GeneralMode_Move_right.ToString()).FirstOrDefault();
                if (cboGeneralMoveRightObj != null)
                {
                    cboGeneralMoveRight.SelectedIndex = getIndexValueOfKeys(cboGeneralMoveRightObj.Keystroke);
                }

                var cboGeneralMoveUpObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "GENERA" && x.Action == ShortcutsAction.GeneralMode_Move_up.ToString()).FirstOrDefault();
                if (cboGeneralMoveUpObj != null)
                {
                    cboGeneralMoveUp.SelectedIndex = getIndexValueOfKeys(cboGeneralMoveUpObj.Keystroke);
                }

                var cboGeneralMoveDownObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "GENERA" && x.Action == ShortcutsAction.GeneralMode_Move_down.ToString()).FirstOrDefault();
                if (cboGeneralMoveDownObj != null)
                {
                    cboGeneralMoveDown.SelectedIndex = getIndexValueOfKeys(cboGeneralMoveDownObj.Keystroke);
                }

                var cboGeneralZoomInObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "GENERA" && x.Action == ShortcutsAction.GeneralMode_Zoom_in.ToString()).FirstOrDefault();
                if (cboGeneralZoomInObj != null)
                {
                    cboGeneralZoomIn.SelectedIndex = getIndexValueOfKeys(cboGeneralZoomInObj.Keystroke);
                }


                var cboGeneralZoomOutObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "GENERA" && x.Action == ShortcutsAction.GeneralMode_Zoom_out.ToString()).FirstOrDefault();
                if (cboGeneralZoomOutObj != null)
                {
                    cboGeneralZoomOut.SelectedIndex = getIndexValueOfKeys(cboGeneralZoomOutObj.Keystroke);
                }

                var cboGeneralToggleAreaBaseObj = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "GENERA" && x.Action == ShortcutsAction.GeneralMode_Toggle_area_base_modes.ToString()).FirstOrDefault();
                if (cboGeneralToggleAreaBaseObj != null)
                {
                    cboGeneralToggleAreaBase.SelectedIndex = getIndexValueOfKeys(cboGeneralToggleAreaBaseObj.Keystroke);
                }
            }
        }

        private List<Shortcuts> buildSourceListForCombobox()
        {
            List<Shortcuts> lst = new List<Shortcuts>();

            Shortcuts objNone = new Shortcuts();
            objNone.DisplayText = noneCharName;
            objNone.Keystroke = noneCharCode;
            lst.Add(objNone);

            Shortcuts objF1 = new Shortcuts();
            objF1.DisplayText = "F1";
            objF1.Keystroke = (char)Keys.F1;
            lst.Add(objF1);

            Shortcuts objF2 = new Shortcuts();
            objF2.DisplayText = "F2";
            objF2.Keystroke = (char)Keys.F2;
            lst.Add(objF2);

            Shortcuts objF3 = new Shortcuts();
            objF3.DisplayText = "F3";
            objF3.Keystroke = (char)Keys.F3;
            lst.Add(objF3);

            Shortcuts objF4 = new Shortcuts();
            objF4.DisplayText = "F4";
            objF4.Keystroke = (char)Keys.F4;
            lst.Add(objF4);

            Shortcuts objF5 = new Shortcuts();
            objF5.DisplayText = "F5";
            objF5.Keystroke = (char)Keys.F5;
            lst.Add(objF5);

            Shortcuts objF6 = new Shortcuts();
            objF6.DisplayText = "F6";
            objF6.Keystroke = (char)Keys.F6;
            lst.Add(objF6);

            Shortcuts objF7 = new Shortcuts();
            objF7.DisplayText = "F7";
            objF7.Keystroke = (char)Keys.F7;
            lst.Add(objF7);

            Shortcuts objF8 = new Shortcuts();
            objF8.DisplayText = "F8";
            objF8.Keystroke = (char)Keys.F8;
            lst.Add(objF8);

            Shortcuts objF9 = new Shortcuts();
            objF9.DisplayText = "F9";
            objF9.Keystroke = (char)Keys.F9;
            lst.Add(objF9);

            Shortcuts objF10 = new Shortcuts();
            objF10.DisplayText = "F10";
            objF10.Keystroke = (char)Keys.F10;
            lst.Add(objF10);

            Shortcuts objF11 = new Shortcuts();
            objF11.DisplayText = "F11";
            objF11.Keystroke = (char)Keys.F11;
            lst.Add(objF11);

            Shortcuts objF12 = new Shortcuts();
            objF12.DisplayText = "F12";
            objF12.Keystroke = (char)Keys.F12;
            lst.Add(objF12);

            Shortcuts objA = new Shortcuts();
            objA.DisplayText = "A";
            objA.Keystroke = (char)Keys.A;
            lst.Add(objA);

            Shortcuts objB = new Shortcuts();
            objB.DisplayText = "B";
            objB.Keystroke = (char)Keys.B;
            lst.Add(objB);

            Shortcuts objC = new Shortcuts();
            objC.DisplayText = "C";
            objC.Keystroke = (char)Keys.C;
            lst.Add(objC);

            Shortcuts objD = new Shortcuts();
            objD.DisplayText = "D";
            objD.Keystroke = (char)Keys.D;
            lst.Add(objD);

            Shortcuts objE = new Shortcuts();
            objE.DisplayText = "E";
            objE.Keystroke = (char)Keys.E;
            lst.Add(objE);

            Shortcuts objF = new Shortcuts();
            objF.DisplayText = "F";
            objF.Keystroke = (char)Keys.F;
            lst.Add(objF);

            Shortcuts objG = new Shortcuts();
            objG.DisplayText = "G";
            objG.Keystroke = (char)Keys.G;
            lst.Add(objG);

            Shortcuts objH = new Shortcuts();
            objH.DisplayText = "H";
            objH.Keystroke = (char)Keys.H;
            lst.Add(objH);

            Shortcuts objI = new Shortcuts();
            objI.DisplayText = "I";
            objI.Keystroke = (char)Keys.I;
            lst.Add(objI);

            Shortcuts objJ = new Shortcuts();
            objJ.DisplayText = "J";
            objJ.Keystroke = (char)Keys.J;
            lst.Add(objJ);

            Shortcuts objK = new Shortcuts();
            objK.DisplayText = "K";
            objK.Keystroke = (char)Keys.K;
            lst.Add(objK);

            Shortcuts objL = new Shortcuts();
            objL.DisplayText = "L";
            objL.Keystroke = (char)Keys.L;
            lst.Add(objL);

            Shortcuts objM = new Shortcuts();
            objM.DisplayText = "M";
            objM.Keystroke = (char)Keys.M;
            lst.Add(objM);

            Shortcuts objN = new Shortcuts();
            objN.DisplayText = "N";
            objN.Keystroke = (char)Keys.N;
            lst.Add(objN);

            Shortcuts objO = new Shortcuts();
            objO.DisplayText = "O";
            objO.Keystroke = (char)Keys.O;
            lst.Add(objO);

            Shortcuts objP = new Shortcuts();
            objP.DisplayText = "P";
            objP.Keystroke = (char)Keys.P;
            lst.Add(objP);

            Shortcuts objQ = new Shortcuts();
            objQ.DisplayText = "Q";
            objQ.Keystroke = (char)Keys.Q;
            lst.Add(objQ);

            Shortcuts objR = new Shortcuts();
            objR.DisplayText = "R";
            objR.Keystroke = (char)Keys.R;
            lst.Add(objR);

            Shortcuts objS = new Shortcuts();
            objS.DisplayText = "S";
            objS.Keystroke = (char)Keys.S;
            lst.Add(objS);

            Shortcuts objT = new Shortcuts();
            objT.DisplayText = "T";
            objT.Keystroke = (char)Keys.T;
            lst.Add(objT);


            Shortcuts objY = new Shortcuts();
            objY.DisplayText = "Y";
            objY.Keystroke = (char)Keys.Y;
            lst.Add(objY);

            Shortcuts objZ = new Shortcuts();
            objZ.DisplayText = "Z";
            objZ.Keystroke = (char)Keys.Z;
            lst.Add(objZ);

            Shortcuts objX = new Shortcuts();
            objX.DisplayText = "X";
            objX.Keystroke = (char)Keys.X;
            lst.Add(objX);


            Shortcuts objD1 = new Shortcuts();
            objD1.DisplayText = "1";
            objD1.Keystroke = (char)Keys.D1;
            lst.Add(objD1);

            Shortcuts objD2 = new Shortcuts();
            objD2.DisplayText = "2";
            objD2.Keystroke = (char)Keys.D2;
            lst.Add(objD2);


            Shortcuts objD3 = new Shortcuts();
            objD3.DisplayText = "3";
            objD3.Keystroke = (char)Keys.D3;
            lst.Add(objD3);

            Shortcuts objD4 = new Shortcuts();
            objD4.DisplayText = "4";
            objD4.Keystroke = (char)Keys.D4;
            lst.Add(objD4);

            Shortcuts objD5 = new Shortcuts();
            objD5.DisplayText = "5";
            objD5.Keystroke = (char)Keys.D5;
            lst.Add(objD5);

            Shortcuts objD6 = new Shortcuts();
            objD6.DisplayText = "6";
            objD6.Keystroke = (char)Keys.D6;
            lst.Add(objD6);

            Shortcuts objD7 = new Shortcuts();
            objD7.DisplayText = "7";
            objD7.Keystroke = (char)Keys.D7;
            lst.Add(objD7);

            Shortcuts objD8 = new Shortcuts();
            objD8.DisplayText = "8";
            objD8.Keystroke = (char)Keys.D8;
            lst.Add(objD8);

            Shortcuts objD9 = new Shortcuts();
            objD9.DisplayText = "9";
            objD9.Keystroke = (char)Keys.D9;
            lst.Add(objD9);

            Shortcuts objD0 = new Shortcuts();
            objD0.DisplayText = "0";
            objD0.Keystroke = (char)Keys.D0;
            lst.Add(objD0);

            Shortcuts objNumPad0 = new Shortcuts();
            objNumPad0.DisplayText = "NumPad0";
            objNumPad0.Keystroke = (char)Keys.NumPad0;
            lst.Add(objNumPad0);

            Shortcuts objNumPad1 = new Shortcuts();
            objNumPad1.DisplayText = "NumPad1";
            objNumPad1.Keystroke = (char)Keys.NumPad1;
            lst.Add(objNumPad1);

            Shortcuts objNumPad2 = new Shortcuts();
            objNumPad2.DisplayText = "NumPad2";
            objNumPad2.Keystroke = (char)Keys.NumPad2;
            lst.Add(objNumPad2);

            Shortcuts objNumPad3 = new Shortcuts();
            objNumPad3.DisplayText = "NumPad3";
            objNumPad3.Keystroke = (char)Keys.NumPad3;
            lst.Add(objNumPad3);

            Shortcuts objNumPad4 = new Shortcuts();
            objNumPad4.DisplayText = "NumPad4";
            objNumPad4.Keystroke = (char)Keys.NumPad4;
            lst.Add(objNumPad4);

            Shortcuts objNumPad5 = new Shortcuts();
            objNumPad5.DisplayText = "NumPad5";
            objNumPad5.Keystroke = (char)Keys.NumPad5;
            lst.Add(objNumPad5);

            Shortcuts objNumPad6 = new Shortcuts();
            objNumPad6.DisplayText = "NumPad6";
            objNumPad6.Keystroke = (char)Keys.NumPad6;
            lst.Add(objNumPad6);

            Shortcuts objNumPad7 = new Shortcuts();
            objNumPad7.DisplayText = "NumPad7";
            objNumPad7.Keystroke = (char)Keys.NumPad7;
            lst.Add(objNumPad7);

            Shortcuts objNumPad8 = new Shortcuts();
            objNumPad8.DisplayText = "NumPad8";
            objNumPad8.Keystroke = (char)Keys.NumPad8;
            lst.Add(objNumPad8);

            Shortcuts objNumPad9 = new Shortcuts();
            objNumPad9.DisplayText = "NumPad9";
            objNumPad9.Keystroke = (char)Keys.NumPad9;
            lst.Add(objNumPad9);

            Shortcuts objNumPadPlus = new Shortcuts();
            objNumPadPlus.DisplayText = "NumPad +";
            objNumPadPlus.Keystroke = (char)Keys.Add;
            lst.Add(objNumPadPlus);

            Shortcuts objLeft = new Shortcuts();
            objLeft.DisplayText = "Left arrow";
            objLeft.Keystroke = (char)Keys.Left;
            lst.Add(objLeft);


            Shortcuts objRight = new Shortcuts();
            objRight.DisplayText = "Right arrow";
            objRight.Keystroke = (char)Keys.Right;
            lst.Add(objRight);

            Shortcuts objUp = new Shortcuts();
            objUp.DisplayText = "Up arrow";
            objUp.Keystroke = (char)Keys.Up;
            lst.Add(objUp);

            Shortcuts objDowns = new Shortcuts();
            objDowns.DisplayText = "Down arrow";
            objDowns.Keystroke = (char)Keys.Down;
            lst.Add(objDowns);

            Shortcuts objHome = new Shortcuts();
            objHome.DisplayText = "Home";
            objHome.Keystroke = (char)Keys.Home;
            lst.Add(objHome);

            Shortcuts objInsert = new Shortcuts();
            objInsert.DisplayText = "Insert";
            objInsert.Keystroke = (char)Keys.Insert;
            lst.Add(objInsert);

            Shortcuts objDelete = new Shortcuts();
            objDelete.DisplayText = "Delete";
            objDelete.Keystroke = (char)Keys.Delete;
            lst.Add(objDelete);

            Shortcuts objEnd = new Shortcuts();
            objEnd.DisplayText = "End";
            objEnd.Keystroke = (char)Keys.End;
            lst.Add(objEnd);

            Shortcuts objEnter = new Shortcuts();
            objEnter.DisplayText = "Enter";
            objEnter.Keystroke = (char)Keys.Enter;
            lst.Add(objEnter);

            Shortcuts objSpace = new Shortcuts();
            objSpace.DisplayText = "Space";
            objSpace.Keystroke = (char)Keys.Space;
            lst.Add(objSpace);

            Shortcuts objTab = new Shortcuts();
            objTab.DisplayText = "Tab";
            objTab.Keystroke = (char)Keys.Tab;
            lst.Add(objTab);

            return lst;
        }

        /// <summary>
        ///  return index of key list
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int getIndexValueOfKeys(char value)
        {
            for (int i = 0; i < cboAreaCancelShape.Items.Count; i++)
            {
                if (value == (char)cboAreaCancelShape.Items[i].DataValue)
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (oForm.lConfigFinishLine.lShortcuts == null)
            {
                oForm.lConfigFinishLine.lShortcuts = new List<Models.Shortcuts>();
            }
            oForm.lConfigFinishLine.lShortcuts.Clear();

            if (cboAreaEraseLastLine.SelectedItem != null && cboAreaEraseLastLine.Text != noneCharName)
            {
                Shortcuts cboAreaEraseLastLineObj = new Shortcuts();
                cboAreaEraseLastLineObj.Area = "AREA";
                cboAreaEraseLastLineObj.Action = ShortcutsAction.AreaMode_Erase_last_line.ToString();
                cboAreaEraseLastLineObj.Keystroke = (char)cboAreaEraseLastLine.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboAreaEraseLastLineObj);
            }


            if (cboAreaCompleteShape.SelectedItem != null && cboAreaCompleteShape.Text != noneCharName)
            {
                Shortcuts cboAreaCompleteShapeObj = new Shortcuts();
                cboAreaCompleteShapeObj.Area = "AREA";
                cboAreaCompleteShapeObj.Action = ShortcutsAction.AreaMode_Complete_shape.ToString();
                cboAreaCompleteShapeObj.Keystroke = (char)cboAreaCompleteShape.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboAreaCompleteShapeObj);
            }

            if (cboAreaCancelShape.SelectedItem != null && cboAreaCancelShape.Text != noneCharName)
            {
                Shortcuts cboAreaCancelShapeObj = new Shortcuts();
                cboAreaCancelShapeObj.Area = "AREA";
                cboAreaCancelShapeObj.Action = ShortcutsAction.AreaMode_Cancel_shape_in_progress.ToString();
                cboAreaCancelShapeObj.Keystroke = (char)cboAreaCancelShape.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboAreaCancelShapeObj);
            }

            if (cboAreaZeroLine.SelectedItem != null && cboAreaZeroLine.Text != noneCharName)
            {
                Shortcuts cboAreaZeroLineObj = new Shortcuts();
                cboAreaZeroLineObj.Area = "AREA";
                cboAreaZeroLineObj.Action = ShortcutsAction.AreaMode_Zero_line.ToString();
                cboAreaZeroLineObj.Keystroke = (char)cboAreaZeroLine.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboAreaZeroLineObj);
            }

            if (cboLineAraseLastLine.SelectedItem != null && cboLineAraseLastLine.Text != noneCharName)
            {
                Shortcuts cboLineAraseLastLineObj = new Shortcuts();
                cboLineAraseLastLineObj.Area = "LINE";
                cboLineAraseLastLineObj.Action = ShortcutsAction.LineMode_Erase_last_line.ToString();
                cboLineAraseLastLineObj.Keystroke = (char)cboLineAraseLastLine.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboLineAraseLastLineObj);
            }

            if (cboLineJump.SelectedItem != null && cboLineJump.Text != noneCharName)
            {
                Shortcuts cboLineJumpObj = new Shortcuts();
                cboLineJumpObj.Area = "LINE";
                cboLineJumpObj.Action = ShortcutsAction.LineMode_Jump.ToString();
                cboLineJumpObj.Keystroke = (char)cboLineJump.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboLineJumpObj);
            }

            if (cboLineSingleLine.SelectedItem != null && cboLineSingleLine.Text != noneCharName)
            {
                Shortcuts cboLineSingleLineObj = new Shortcuts();
                cboLineSingleLineObj.Area = "LINE";
                cboLineSingleLineObj.Action = ShortcutsAction.LineMode_Single_line.ToString();
                cboLineSingleLineObj.Keystroke = (char)cboLineSingleLine.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboLineSingleLineObj);
            }

            if (cboLineDoubleLine.SelectedItem != null && cboLineDoubleLine.Text != noneCharName)
            {
                Shortcuts cboLineDoubleLineObj = new Shortcuts();
                cboLineDoubleLineObj.Area = "LINE";
                cboLineDoubleLineObj.Action = ShortcutsAction.LineMode_Double_line.ToString();
                cboLineDoubleLineObj.Keystroke = (char)cboLineDoubleLine.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboLineDoubleLineObj);
            }

            if (cboLineDuplicateLine.SelectedItem != null && cboLineDuplicateLine.Text != noneCharName)
            {
                Shortcuts cboLineDoubleLineObj = new Shortcuts();
                cboLineDoubleLineObj.Area = "LINE";
                cboLineDoubleLineObj.Action = ShortcutsAction.LineMode_Duplicate_line.ToString();
                cboLineDoubleLineObj.Keystroke = (char)cboLineDuplicateLine.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboLineDoubleLineObj);
            }

            if (cboGeneralMoveLeft.SelectedItem != null && cboGeneralMoveLeft.Text != noneCharName)
            {
                Shortcuts cboGeneralMoveLeftObj = new Shortcuts();
                cboGeneralMoveLeftObj.Area = "GENERA";
                cboGeneralMoveLeftObj.Action = ShortcutsAction.GeneralMode_Move_left.ToString();
                cboGeneralMoveLeftObj.Keystroke = (char)cboGeneralMoveLeft.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboGeneralMoveLeftObj);
            }

            if (cboGeneralMoveRight.SelectedItem != null && cboGeneralMoveRight.Text != noneCharName)
            {
                Shortcuts cboGeneralMoveRightObj = new Shortcuts();
                cboGeneralMoveRightObj.Area = "GENERA";
                cboGeneralMoveRightObj.Action = ShortcutsAction.GeneralMode_Move_right.ToString();
                cboGeneralMoveRightObj.Keystroke = (char)cboGeneralMoveRight.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboGeneralMoveRightObj);
            }

            if (cboGeneralMoveUp.SelectedItem != null && cboGeneralMoveUp.Text != noneCharName)
            {
                Shortcuts cboGeneralMoveUpObj = new Shortcuts();
                cboGeneralMoveUpObj.Area = "GENERA";
                cboGeneralMoveUpObj.Action = ShortcutsAction.GeneralMode_Move_up.ToString();
                cboGeneralMoveUpObj.Keystroke = (char)cboGeneralMoveUp.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboGeneralMoveUpObj);
            }

            if (cboGeneralMoveDown.SelectedItem != null && cboGeneralMoveDown.Text != noneCharName)
            {
                Shortcuts cboGeneralMoveDownObj = new Shortcuts();
                cboGeneralMoveDownObj.Area = "GENERA";
                cboGeneralMoveDownObj.Action = ShortcutsAction.GeneralMode_Move_down.ToString();
                cboGeneralMoveDownObj.Keystroke = (char)cboGeneralMoveDown.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboGeneralMoveDownObj);
            }


            if (cboGeneralZoomIn.SelectedItem != null && cboGeneralZoomIn.Text != noneCharName)
            {
                Shortcuts cboGeneralZoomInObj = new Shortcuts();
                cboGeneralZoomInObj.Area = "GENERA";
                cboGeneralZoomInObj.Action = ShortcutsAction.GeneralMode_Zoom_in.ToString();
                cboGeneralZoomInObj.Keystroke = (char)cboGeneralZoomIn.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboGeneralZoomInObj);
            }

            if (cboGeneralZoomOut.SelectedItem != null && cboGeneralZoomOut.Text != noneCharName)
            {
                Shortcuts cboGeneralZoomOutObj = new Shortcuts();
                cboGeneralZoomOutObj.Area = "GENERA";
                cboGeneralZoomOutObj.Action = ShortcutsAction.GeneralMode_Zoom_out.ToString();
                cboGeneralZoomOutObj.Keystroke = (char)cboGeneralZoomOut.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboGeneralZoomOutObj);
            }

            if (cboGeneralToggleAreaBase.SelectedItem != null && cboGeneralToggleAreaBase.Text != noneCharName)
            {
                Shortcuts cboGeneralToggleAreaBaseObj = new Shortcuts();
                cboGeneralToggleAreaBaseObj.Area = "GENERA";
                cboGeneralToggleAreaBaseObj.Action = ShortcutsAction.GeneralMode_Toggle_area_base_modes.ToString();
                cboGeneralToggleAreaBaseObj.Keystroke = (char)cboGeneralToggleAreaBase.SelectedItem.DataValue;
                oForm.lConfigFinishLine.lShortcuts.Add(cboGeneralToggleAreaBaseObj);
            }

            //validate value in area mode
            var lstArea = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "AREA").ToList();
            for (int i = 0; i < lstArea.Count; i++)
            {
                for (int j = i + 1; j < lstArea.Count; j++)
                {
                    if (lstArea[i].Keystroke == lstArea[j].Keystroke)
                    {
                        MessageBox.Show("Area mode. Duplicate key: " + lstArea[i].DisplayText);
                        return;
                    }
                }
            }

            //validate value in Line mode
            var lstLine = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "LINE").ToList();
            for (int i = 0; i < lstLine.Count; i++)
            {
                for (int j = i + 1; j < lstLine.Count; j++)
                {
                    if (lstLine[i].Keystroke == lstLine[j].Keystroke)
                    {
                        MessageBox.Show("Line  mode. Duplicate key: " + lstLine[i].DisplayText);
                        return;
                    }
                }
            }

            //validate value in General mode
            var lstGeneral = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area == "GENERA").ToList();
            for (int i = 0; i < lstGeneral.Count; i++)
            {
                for (int j = i + 1; j < lstGeneral.Count; j++)
                {
                    if (lstGeneral[i].Keystroke == lstGeneral[j].Keystroke)
                    {
                        MessageBox.Show("General mode. Duplicate key: " + lstGeneral[i].DisplayText);
                        return;
                    }
                }
            }


            //validate value in General mode don't exist in area or line mode
            var lstOther = oForm.lConfigFinishLine.lShortcuts.Where(x => x.Area != "GENERA").ToList();
            for (int i = 0; i < lstGeneral.Count; i++)
            {
                for (int j = 0; j < lstOther.Count; j++)
                {
                    if (lstGeneral[i].Keystroke == lstOther[j].Keystroke)
                    {
                        MessageBox.Show(lstGeneral[i].DisplayText + " .Key in general mode exist in area mode or line mode");
                        return;
                    }
                }
            }

            this.Close();
        }
    }
}