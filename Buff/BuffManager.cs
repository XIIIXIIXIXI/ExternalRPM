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
    /*
    Description:
    This class manages buffs for the local player in the game. It extends the CachedClass to leverage caching functionality for 
    efficiency. The class provides a property "Buffs" to retrieve a list of buffs active on the local player.

    Properties:
    - Buffs: A property that retrieves a list of buffs active on the local player. The list is cached for efficient access.

    Note: Buffs are game-specific status effects that can modify a character's attributes, skills, or behaviors during the game.
*/
    public class BuffManager : CachedClass
    {
        public List<Buff> Buffs
        {
            get
            {
                long baseAddress = Offsets.Instances.GetLocalPlayer;
                long buffsArray =
                    Memory.Read<long>(baseAddress + Offsets.Object.BuffManager + Offsets.Object.BuffArray);
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
            }
        }
        
    }
}
