using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExternalRPM.Modules
{
    class NativeImport
    {
        public static class WindowConstants
        {
            public const int SW_HIDE = 0;
            public const int SW_SHOW = 5;
            public const int WM_NCLBUTTONDOWN = 0xA1;
            public const int HTCAPTION = 0x2;
        }

        public static class FileConstants
        {
            public const uint GENERIC_WRITE = 0x40000000;
            public const uint GENERIC_READ = 0x80000000;
            public const uint FILE_SHARE_WRITE = 0x2;
            public const uint OPEN_EXISTING = 0x3;
        }

        public const int DefaultCodePage = 437;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);
    }
}
