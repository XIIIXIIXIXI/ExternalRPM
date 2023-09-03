using System; 
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ExternalRPM.Data;
using ExternalRPM.Presentation;
using SharpDX;
using Newtonsoft.Json.Linq;

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
        private static LocalPlayer instance;

        public static JObject UnitRadiusData;
        public long BaseAddress { get; }
        private long BuffArray { get; }

        public string Name { get; private set; }
        public float Health { get; private set; }
        public float MaxHealth { get; private set; }
        public bool IsBlueTeam { get; private set; }
        public Vector3 position { get; private set; }
        public int boundingRadius {  get; private set; }
        public bool isAlive { get; private set; }
        public bool isVisible {  get; private set; }

        private LocalPlayer()
        {
            BaseAddress = Offsets.Instances.GetLocalPlayer;
            this.BuffArray = Memory.Read<long>(BaseAddress + Offsets.Object.BuffManager + Offsets.Object.BuffArray);
            IsBlueTeam = IsLocalPlayerBlueTeam();
            Name = Memory.ReadString(BaseAddress + Offsets.Object.ObjectName, 20);
            boundingRadius = GetBoundingRadius();
        }
        // Public method to get the singleton instance
        public static LocalPlayer GetInstance()
        {
            if (instance == null)
            {
                instance = new LocalPlayer();
            }
            return instance;
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

        public Vector3 GetPosition()
        {
            return Memory.Read<Vector3>(BaseAddress + Offsets.Object.Position);
            
        }
        public void DrawAttackRange(SharpDX.Color color, float Thickness)
        {
           
           if (IsVisible() && IsAlive())
            {
                DrawFactory.DrawCircleRange(GetPosition(), boundingRadius + GetAttackRange(), color, Thickness);
            }
        }

        public float GetAttackRange()
        {
            return Memory.Read<float>(BaseAddress + Offsets.Object.AttackRange);
        }
        public int GetBoundingRadius()
        {
            return int.Parse(UnitRadiusData[Name]["Gameplay radius"].ToString());
        }
        public bool IsAlive()
        {
            isAlive = Memory.Read<int>(BaseAddress + Offsets.Object.IsAlive) % 2 == 0;
            return isAlive;
        }
        public bool IsVisible()
        {
            isVisible = Memory.Read<int>(BaseAddress + Offsets.Object.IsVisible) % 2 == 1;
            return isVisible;
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
