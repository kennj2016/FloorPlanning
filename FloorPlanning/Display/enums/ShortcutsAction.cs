namespace FloorPlanning.Display.enums
{
    /// <summary>
    /// Defines a zoom factor used in the preview control.
    /// </summary>
    public enum ShortcutsAction
    {
        /// <summary>
        /// Area mode: Erase last line
        /// </summary>
        AreaMode_Erase_last_line = 1,
        /// <summary>
        /// Area mode: Complete shape
        /// </summary>
        AreaMode_Complete_shape = 2,
        /// <summary>
        /// Area mode: Cancel shape in progress
        /// </summary>
        AreaMode_Cancel_shape_in_progress = 3,
        /// <summary>
        /// Area mode: Zero line
        /// </summary>
        AreaMode_Zero_line = 4,

        /// <summary>
        /// Line mode: Erase last line
        /// </summary>
        LineMode_Erase_last_line = 21,
        /// <summary>
        /// Line mode: Jump
        /// </summary>
        LineMode_Jump = 22,
        /// <summary>
        /// Line mode: Single line
        /// </summary>
        LineMode_Single_line = 23,
        /// <summary>
        /// Line mode: Double line
        /// </summary>
        LineMode_Double_line = 24,
        /// <summary>
        /// Line mode: Duplicate line
        /// </summary>
        LineMode_Duplicate_line = 25,


        /// <summary>
        /// General mode: Move left
        /// </summary>
        GeneralMode_Move_left = 41,
        /// <summary>
        /// General mode: Move right
        /// </summary>
        GeneralMode_Move_right = 42,
        /// <summary>
        /// General mode: Move up
        /// </summary>
        GeneralMode_Move_up = 43,
        /// <summary>
        /// General mode: Move down
        /// </summary>
        GeneralMode_Move_down = 44,
        /// <summary>
        /// General mode: Zoom in
        /// </summary>
        GeneralMode_Zoom_in = 45,
        /// <summary>
        /// General mode: Zoom out
        /// </summary>
        GeneralMode_Zoom_out = 46,
        /// <summary>
        /// General mode: Toggle area-base modes
        /// </summary>
        GeneralMode_Toggle_area_base_modes = 47,
    }
}
