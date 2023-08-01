using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExternalRPM.Modules
{
    /*
    Description:
    This class provides methods to read data from the memory of the League of Legends process.
    It utilizes native Windows functions via the NativeImport class for reading process memory.

    Methods:
    - Read<T>(long Address): Reads data of type T from the specified memory address in the League of Legends process.
    - ReadString(long address, int size): Reads a string of the specified size from the memory address in the League of Legends process.

*/
    class Memory
    {
        private static IntPtr GetLeagueProcessHandle()
        {
            var leagueProcess = Process.GetProcessesByName("League of Legends").FirstOrDefault();
            return leagueProcess?.Handle ?? IntPtr.Zero;
        }

        public static T Read<T>(long Address)
        {
            var Size = Marshal.SizeOf<T>();
            var Buffer = new byte[Size];
            bool Result = NativeImport.ReadProcessMemory(Process.GetProcessesByName("League of Legends").FirstOrDefault().Handle, (IntPtr)Address, Buffer, Size, out var lpRead);
            var Ptr = Marshal.AllocHGlobal(Size);
            Marshal.Copy(Buffer, 0, Ptr, Size);
            var Struct = Marshal.PtrToStructure<T>(Ptr);
            Marshal.FreeHGlobal(Ptr);
            return Struct;
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
