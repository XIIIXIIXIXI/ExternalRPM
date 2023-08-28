using ExternalRPM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Model;
using SharpDX;

namespace ExternalRPM.Modules
{
    class ChampionReader
    {
        static long heroList = Offsets.Instances.GetHeroList;
        static long objectManagerRootNode = Memory.Read<long>(heroList + 0x8);
        public static Offsets.GameObject[] ReadChampions()
        {
            int count = Memory.Read<int>(heroList + 0x10);
            Offsets.GameObject[] output = new Offsets.GameObject[count];
            for (int i = 0; i < count; i++)
            {
                /*
                Offsets.GameObject.memoryID = Memory.Read<long>(objectManagerRootNode + i * 0x8);
                var objectToScan = Memory.Read<long>(objectManagerRootNode + i * 0x8);
                var readObject = Memory.Read<Offsets.GameObject>(Offsets.GameObject.memoryID);
                readObject.
                string name = Memory.ReadString(Offsets.GameObject.memoryID + Offsets.Object.ObjectName, 20);
                output[i] = readObject;
                */
                long memoryID = Memory.Read<long>(objectManagerRootNode + i * 0x8);
                Offsets.GameObject instance = new Offsets.GameObject(memoryID);
                output[i] = instance;
            }

            return output;
        }
        public static Vector3 ReadChampionPosition(long address)
        {
            return Memory.Read<Vector3>(address + Offsets.Object.Position);
        }
    }

}
