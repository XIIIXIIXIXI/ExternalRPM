using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Data;
using ExternalRPM.Modules;

namespace ExternalRPM
{
    public class JungleCampMemoryReader
    {
        //static fields that never changes:
        private readonly long entityListPtr;

        public JungleCampMemoryReader()
        {
            long entityBaseAddress = Offsets.Instances.GetEntityBase;
            this.entityListPtr = Memory.Read<long>(entityBaseAddress + Offsets.Object.EntityList);
        }
        public HashSet<string> GetJungleCampsEntityList()
        {
            int entityCount = Memory.Read<int>(Offsets.Instances.GetEntityBase + Offsets.Object.EntityCount);

            HashSet<string> campNames = new();
            for (int i = 0; i < entityCount; i++)
            {
                long entityAddress = Memory.Read<long>(entityListPtr + (Offsets.Object.EntityList * i)) + 0x60;
                string entityString = Memory.ReadString(entityAddress, 20);
                if (entityString.Contains("SRU") || entityString.Contains("Sru"))
                {
                    campNames.Add(entityString);
                }

                long entityAddress2 = Memory.Read<long>(entityAddress);
                string entityString2 = Memory.ReadString(entityAddress2, 20);
                if ((entityString2.Contains("SRU") && !entityString2.Contains("Mini")) || entityString2.Contains("Sru"))
                {
                    campNames.Add(entityString2);
                }
            }
            return campNames;
        }
    }
}
