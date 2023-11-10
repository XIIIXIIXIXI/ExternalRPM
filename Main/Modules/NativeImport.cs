using ExternalRPM.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExternalRPM.Modules
{
    /*
    Description:
    This class contains various methods that provide access to native Windows functions through DLL imports.
    It offers functionality related to process memory reading, window handling, window attributes, and more.

    Methods:
    - ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead): Reads the memory of a specified process.
    - BringWindowToTop(IntPtr hWnd): Brings the specified window to the top of the Z order.
    - SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong): Changes attributes of the specified window.
    - GetWindowLong(IntPtr hWnd, int nIndex): Retrieves information about the specified window.
    - SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags): Sets the opacity and transparency of a layered window.
    - GetActiveWindowTitle(): Retrieves the title of the currently active window.
    - DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Presentation.DrawFactory.Margins pMargins): Extends the window frame into the client area.
*/
    class NativeImport
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
        [DllImport("dwmapi.dll")]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Presentation.DrawFactory.Margins pMargins);
    }
}
