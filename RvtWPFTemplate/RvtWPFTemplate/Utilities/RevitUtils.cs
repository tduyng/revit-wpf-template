using Autodesk.Windows;
using System;
using System.Runtime.InteropServices;

namespace RvtWPFTemplate.Utilities
{
    public class RevitUtils
    {
        #region Triggering External Event Execute by Setting Focus
        //Thanks for solution:
        //https://thebuildingcoder.typepad.com/blog/2013/12/triggering-immediate-external-event-execute.html
        //https://github.com/jeremytammik/RoomEditorApp/tree/master/RoomEditorApp
        //https://thebuildingcoder.typepad.com/blog/2016/03/implementing-the-trackchangescloud-external-event.html#5

        /// <summary>
        /// The GetForegroundWindow function returns a 
        /// handle to the foreground window.
        /// </summary>
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Move the window associated with the passed 
        /// handle to the front.
        /// </summary>
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(
          IntPtr hWnd);

        public static void SetFocusToRevit()
        {
            IntPtr hRevit = ComponentManager.ApplicationWindow;
            IntPtr hBefore = GetForegroundWindow();

            if (hBefore != hRevit)
            {
                SetForegroundWindow(hRevit);
                SetForegroundWindow(hBefore);
            }
        }

        #endregion
    }
}
