using System; 
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Data;

namespace ExternalRPM.Modules
{
    /*
    LocalPlayer Class

    Description:
    This class represents the local player in the game E.g yourself. It provides functionality to retrieve and update the local player's health, max health, and team information.
    Additionally, it allows fetching the addresses of active buffs and their corresponding names for the local player.

    - GetBuffAddresses(): Retrieves an array of addresses for active buffs of the local player from memory.
    - GetBuffName(long[] buffAddresses): Given an array of buff addresses, fetches the names of the corresponding buffs from memory and returns them as a string array.
    */
    public class LocalPlayer
    {
        public long BaseAddress { get; }
        private long BuffArray { get; }
        public float Health { get; private set; }
        public float MaxHealth { get; private set; }
        public bool IsBlueTeam { get; private set; }

        public LocalPlayer()
        {
            BaseAddress = Offsets.Instances.GetLocalPlayer;
            this.BuffArray = Memory.Read<long>(BaseAddress + Offsets.Object.BuffManager + Offsets.Object.BuffArray);
            IsBlueTeam = IsLocalPlayerBlueTeam();
        }

        private bool IsLocalPlayerBlueTeam()
        {
            return Memory.Read<int>(BaseAddress + Offsets.Object.PlayerTeam) == 100;
        }

        public void UpdateHealth()
        {
            Health = Memory.Read<float>(BaseAddress + Offsets.Object.Health);
        }

        public void UpdateMaxHealth()
        {
            MaxHealth = Memory.Read<float>(BaseAddress + Offsets.Object.MaxHealth);
        }

        public long[] GetBuffAddresses()
        {
            const int numEntries = 50;
            const int entrySize = 0x10;
            long[] buffValues = new long[numEntries];

            for (int i = 0; i < numEntries; i++)
            {
                long address = this.BuffArray + (i * entrySize);
                buffValues[i] = Memory.Read<long>(address);
            }
            return buffValues;
        }

        public string[] GetBuffName(long[] buffAddresses)
        {
            string[] buffNames = new string[buffAddresses.Length];
            for (int i = 0; i < buffAddresses.Length; i++)
            {
                long buffInfo = Memory.Read<long>(buffAddresses[i] + Offsets.Object.BuffEntityInfo);
                string name = Memory.ReadString(buffInfo + Offsets.Object.BuffName, 20);
                buffNames[i] = name;
            }
            return buffNames;
        }
    }
}
