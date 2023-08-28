using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Data;
using ExternalRPM.Modules;
using SharpDX;

namespace ExternalRPM.Model
{
    public class Champion
    {
        public long MemoryID { get; private set; }
        public string Name { get; private set; }
        public float Health { get; private set; }
        public float HealthMaximum { get; private set; }
        public Vector3 Position { get; private set; }
        public int TeamID { get; private set; }
        public int NetworkID { get; private set; }
        public int Level { get; private set; }
        public bool IsVisible { get; private set; }
        public bool IsAlive { get; private set; }

        public Champion(long memoryID, string name, float health, float healthMaximum, Vector3 position, int teamID, int networkID, int level, bool isVisible, bool isAlive)
        {
            MemoryID = memoryID;
            Name = name;
            Health = health;
            HealthMaximum = healthMaximum;
            Position = position;
            TeamID = teamID;
            NetworkID = networkID;
            Level = level;
            IsVisible = isVisible;
            IsAlive = isAlive;
        }

        public static Champion[] CreateChampionsFromGameObjects(Offsets.GameObject[] gameObjects)
        {
            Champion[] champions = new Champion[gameObjects.Length];

            for (int i = 0; i < gameObjects.Length; i++)
            {
                var gameObject = gameObjects[i];
                string name = Memory.ReadString(gameObject.memoryID + Offsets.Object.ObjectName, 20);

                Champion champion = new Champion(
                    memoryID: gameObject.memoryID,
                    name: name,
                    health: gameObject.Health,
                    healthMaximum: gameObject.HealthMaximum,
                    position: gameObject.Position,
                    teamID: gameObject.TeamID,
                    networkID: gameObject.NetworkID,
                    level: gameObject.Level,
                    isVisible: gameObject.IsVisible, 
                    isAlive: gameObject.IsAlive     
                );

                champions[i] = champion;
            }

            return champions;
        }
    }

}
