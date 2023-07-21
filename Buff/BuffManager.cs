using ExternalRPM.Data;
using ExternalRPM.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ExternalRPM.Buff
{
    public class BuffManager : CachedClass
    {
        public List<Buff> Buffs => Use("buffs", () =>
        {
            long baseAddress = Offsets.Instances.GetLocalPlayer;
            long buffsArray = Memory.Read<long>(baseAddress + Offsets.Object.BuffManager + Offsets.Object.BuffArray);
            int buffsSize = 0x10;
            List<Buff> result = new List<Buff>();
            for (int i = 0; i < 150; i++)
            {
                long buffAddress = buffsArray + (i * buffsSize);
                long buffValue = Memory.Read<long>(buffAddress);
                // if (buffAddress > buffsSize) break;
                Buff buff = new Buff(buffValue);
                if (buff.EndTime == 0)
                {
                    break;
                }
                if (buff.EndTime != -1)
                {
                    result.Add(buff);
                }
                //if (buff.Count > 500) break;
                
            }
            return result;
        });
    }
}
