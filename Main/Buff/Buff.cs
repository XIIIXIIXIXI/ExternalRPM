using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Data;
using ExternalRPM.Modules;

namespace ExternalRPM.Buff
{
    /*
    Description:
    This class represents all the attributes a buff instance has in the game. It extends the CachedClass to leverage caching functionality for 
    efficiency. The class provides properties to retrieve information about the buff, such as its name, start time, end time, and count.
*/
    public class Buff : CachedClass
    {
        public readonly long address;

        public Buff(long address)
        {
            this.address = address;
        }

        private long Entry => Memory.Read<long>(address + Offsets.Object.BuffEntityInfo);

        public string Name => Memory.ReadString(Entry + Offsets.Object.BuffName, 20);

        public float StartTime => Memory.Read<float>(address + Offsets.Object.BuffStartTime);

        public float EndTime => Memory.Read<float>(address + Offsets.Object.BuffEndTime);

        public int Count => Memory.Read<int>(address + Offsets.Object.BuffCount);
        public int Count2 => Memory.Read<int>(address + Offsets.Object.BuffCountAlt);
    }
}
