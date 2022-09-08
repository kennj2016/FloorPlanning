using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloorPlanning
{
    /// <summary>
    /// Specifies the currently running interactive command.
    /// Zooming and panning are not considered commands, and can be done during a command
    /// without affecting the command mode.
    /// </summary>
    public enum CommandMode
    {
        /// <summary>
        /// Stop existing command, but don't show cancel message
        /// </summary>
        SilentCancel = -2,
        
        /// <summary>
        /// Stop existing command, and show message
        /// </summary>
        Cancel = -1,

        /// <summary>
        /// No command is active
        /// </summary>
        None = 0,

        /// <summary>
        /// DeleteSelected command
        /// </summary>
        Delete = 1,

        

        /// <summary>
        /// Polyline command
        /// </summary>
        Pline = 30,

        /// <summary>
        /// Polyline command, requested first point
        /// </summary>
        PlineP1 = 31,

        /// <summary>
        /// Polyline command, requested next point
        /// </summary>
        PlineNext = 32,

        /// <summary>
        /// Polyline command, done
        /// </summary>
        PlineDone = 33,

        /// <summary>
        /// Accept polyline
        /// </summary>
        PlineAccept = 34,

        /// <summary>
        /// Close polyline
        /// </summary>
        PlineClose = 35,

        /// <summary>
        /// Close polyline
        /// </summary>
        PlineOpen = 36, 
        
        /// <summary>
        /// Polygon command
        /// </summary>
        Polygon = 40,

        /// <summary>
        /// Polygon command, requested first point
        /// </summary>
        PolygonP1 = 41,

        /// <summary>
        /// Polygon command, requested next point
        /// </summary>
        PolygonNext = 42,

        /// <summary>
        /// Polygon command, done
        /// </summary>
        PolygonDone = 43,

        /// <summary>
        /// Accept polygon
        /// </summary>
        PolygonAccept = 44,

        

        /// <summary>
        /// Select PDF Page
        /// </summary>
        PDFSelectPage = 81,

        /// <summary>
        /// Select PDF Page, done
        /// </summary>
        PDFSelectPageAccept = 82,

        /// <summary>
        /// Select PDF Page, done
        /// </summary>
        PDFSelectPageDone = 83,


        /// <summary>
        /// Autocad Tool Rotating Done
        /// </summary>
        PolygonEdit = 108,

        

        /// <summary>
        /// Autocad Tool Rotating Done
        /// </summary>
        PlineEdit = 110,


        /// <summary>
        /// DoorLine command
        /// </summary>
        PLineDoor = 200,

        /// <summary>
        /// DoorLine command, requested first point
        /// </summary>
        PLineDoorP1 = 201,

        /// <summary>
        /// DoorLine command, requested next point
        /// </summary>
        PLineDoorNext = 202,

        /// <summary>
        /// DoorLine command, done
        /// </summary>
        PLineDoorDone = 203,

        /// <summary>
        /// Accept DoorLine
        /// </summary>
        PLineDoorAccept = 204,
    }
}
