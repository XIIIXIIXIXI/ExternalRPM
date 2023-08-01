using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Data;

namespace ExternalRPM.Modules
{
    /*
    Description:
    This class is responsible for reading entity data from the game's memory. It specifically focuses on gathering information about jungle camps.
    It utilizes memory offsets to access the entity list and extract relevant data about jungle camps.

    Public Methods:
    - GetJungleCampsEntityList(): Retrieves a HashSet of strings containing the names of jungle camps found in the entity list.

*/
    public class EntityReader
    {
        //static fields that never changes:
        private readonly long entityListPtr;

        public EntityReader()
        {
            long entityBaseAddress = Offsets.Instances.GetEntityBase;
            entityListPtr = Memory.Read<long>(entityBaseAddress + Offsets.Object.EntityList);
        }
        public HashSet<string> GetJungleCampsEntityList()
        {
            int entityCount = Memory.Read<int>(Offsets.Instances.GetEntityBase + Offsets.Object.EntityCount);

            HashSet<string> campNames = new();
            for (int i = 0; i < entityCount; i++)
            {
                long entityAddress = Memory.Read<long>(entityListPtr + Offsets.Object.EntityList * i) + 0x60;
                string entityString = Memory.ReadString(entityAddress, 20);
                if (entityString.Contains("SRU") || entityString.Contains("Sru"))
                {
                    campNames.Add(entityString);
                }

                long entityAddress2 = Memory.Read<long>(entityAddress);
                string entityString2 = Memory.ReadString(entityAddress2, 20);
                if (entityString2.Contains("SRU") && !entityString2.Contains("Mini") || entityString2.Contains("Sru"))
                {
                    campNames.Add(entityString2);
                }
            }
            return campNames;
        }
    }
}
