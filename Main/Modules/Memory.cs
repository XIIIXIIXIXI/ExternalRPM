using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

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

        public static Matrix ReadMatrix(long address)
        {
            Matrix tmp = Matrix.Zero;

            byte[] Buffer = new byte[64];
            IntPtr ByteRead;

            NativeImport.ReadProcessMemory(Process.GetProcessesByName("League of Legends").FirstOrDefault().Handle, (IntPtr)address, Buffer, 64, out ByteRead);

            if (ByteRead == IntPtr.Zero)
            {
                //Console.WriteLine($"[ReadMatrix] No bytes has been read at 0x{address.ToString("X")}");
                return new Matrix();
            }

            tmp.M11 = BitConverter.ToSingle(Buffer, (0 * 4));
            tmp.M12 = BitConverter.ToSingle(Buffer, (1 * 4));
            tmp.M13 = BitConverter.ToSingle(Buffer, (2 * 4));
            tmp.M14 = BitConverter.ToSingle(Buffer, (3 * 4));

            tmp.M21 = BitConverter.ToSingle(Buffer, (4 * 4));
            tmp.M22 = BitConverter.ToSingle(Buffer, (5 * 4));
            tmp.M23 = BitConverter.ToSingle(Buffer, (6 * 4));
            tmp.M24 = BitConverter.ToSingle(Buffer, (7 * 4));

            tmp.M31 = BitConverter.ToSingle(Buffer, (8 * 4));
            tmp.M32 = BitConverter.ToSingle(Buffer, (9 * 4));
            tmp.M33 = BitConverter.ToSingle(Buffer, (10 * 4));
            tmp.M34 = BitConverter.ToSingle(Buffer, (11 * 4));

            tmp.M41 = BitConverter.ToSingle(Buffer, (12 * 4));
            tmp.M42 = BitConverter.ToSingle(Buffer, (13 * 4));
            tmp.M43 = BitConverter.ToSingle(Buffer, (14 * 4));
            tmp.M44 = BitConverter.ToSingle(Buffer, (15 * 4));

            return tmp;
        }
        public static (Matrix ViewMatrix, Matrix ProjectionMatrix) ReadMatrices(long address)
        {
            Matrix viewMatrix = Matrix.Zero;
            Matrix projectionMatrix = Matrix.Zero;

            byte[] buffer = new byte[128];
            IntPtr bytesRead;

            IntPtr processHandle = Process.GetProcessesByName("League of Legends").FirstOrDefault().Handle;

            NativeImport.ReadProcessMemory(processHandle, (IntPtr)address, buffer, 128, out bytesRead);

            if (bytesRead == IntPtr.Zero)
            {
                // Handle error, return default matrices, or throw an exception
                return (Matrix.Zero, Matrix.Zero);
            }

            // Read the ViewMatrix
            for (int i = 0; i < 16; i++)
            {
                viewMatrix[i / 4, i % 4] = BitConverter.ToSingle(buffer, i * 4);
            }

            // Read the ProjectionMatrix
            for (int i = 16; i < 32; i++)
            {
                projectionMatrix[(i - 16) / 4, (i - 16) % 4] = BitConverter.ToSingle(buffer, i * 4);
            }

            return (viewMatrix, projectionMatrix);
        }
    }

    
}
