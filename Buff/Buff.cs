using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Data;
using ExternalRPM.Modules;

namespace ExternalRPM.Buff
{
    public class Buff : CachedClass
    {
        public readonly long address;

        public Buff(long address)
        {
            this.address = address;
        }

        private long Entry => Use("entry", () => Memory.Read<long>(address + Offsets.Object.BuffEntityInfo));

        public string Name => Use("name", () => Memory.ReadString(Entry + Offsets.Object.BuffName, 20));

        public float StartTime => Use("startTime", () => Memory.Read<float>(address + Offsets.Object.BuffStartTime));

        public float EndTime => Use("endtime", () => Memory.Read<float>(address + Offsets.Object.BuffEndTime));

        public int Count => Use("count", () => Memory.Read<int>(address + Offsets.Object.BuffCount));
        public int Count2 => Use("count2", () => Memory.Read<int>(address + Offsets.Object.BuffCountAlt));
    }
}
