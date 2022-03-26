using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Helpers
{
    public static class WindowShield
    {
        
        // Imposes no restrictions on where the window can be displayed (Default)
        public static readonly uint WDA_NONE = 0;
        // The window content is displayed only on a monitor. Everywhere else, the window appears with no content
        public static readonly uint WDA_MONITOR = 1;
        /* The window is displayed only on a monitor. Everywhere else, the window does not appear at all.
        One use for this affinity is for windows that show video recording controls, so that the controls are not included in the capture */
        public static readonly uint WDA_EXCLUDEFROMCAPTURE = 3;
  
         
        [DllImport("user32.dll")]
        public static extern bool SetWindowDisplayAffinity(IntPtr hwnd, uint dwAffinity);
        
    }
}
