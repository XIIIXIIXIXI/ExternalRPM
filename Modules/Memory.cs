using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExternalRPM.Modules
{
    class Memory
    {
        private static IntPtr GetLeagueProcessHandle()
        {
            var leagueProcess = Process.GetProcessesByName("League of Legends").FirstOrDefault();
            return leagueProcess?.Handle ?? IntPtr.Zero;
        }

        public static T Read<T>(long address) where T : struct
        {
            var size = Marshal.SizeOf<T>();
            var buffer = new byte[size];

            IntPtr processHandle = GetLeagueProcessHandle();
            if (processHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException("League of Legends process not found.");
            }

            bool result = NativeImport.ReadProcessMemory(processHandle, (IntPtr)address, buffer, size, out var bytesRead);
            if (!result)
            {
                throw new InvalidOperationException("Failed to read memory.");
            }

            var ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(buffer, 0, ptr, size);
                return Marshal.PtrToStructure<T>(ptr);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static string ReadString(long address, int size)
        {
            byte[] dataBuffer = new byte[size];
            IntPtr bytesRead = IntPtr.Zero;

            NativeImport.ReadProcessMemory(System.Diagnostics.Process.GetProcessesByName("League of Legends").FirstOrDefault().Handle, (IntPtr)address, dataBuffer, dataBuffer.Length, out bytesRead);

            if (bytesRead == IntPtr.Zero)
            {
                return string.Empty;
            }
            dataBuffer = dataBuffer.TakeWhile(b => b != 0).ToArray();
            if (dataBuffer.Length > 3)
            {
                //dataBuffer = dataBuffer.Where(b => char.IsControl((char)b) == false).ToArray();
                string result = Encoding.UTF8.GetString(dataBuffer);
                return result;
            }
            else
            {
                return "0";
            }
        }
    }
}
