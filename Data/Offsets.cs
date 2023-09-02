using ExternalRPM.Modules;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExternalRPM.Data
{
    /*
    Description:
    This class contains memory offsets for various game data, such as player information, object details, buffs, and minimap data. 
    The offsets are used to access specific memory locations where the game stores relevant information.

    Important: Memory offsets are specific to the game version and needs updating when the game receives patches or updates.
*/
    //13.14.522.7601
    public class Offsets
    {
        public static long BaseAddress = Utils.GetLeagueProcess().MainModule.BaseAddress.ToInt64();

        
        public class Instances
        {
            //Patch 13.14
            private static long _localPlayer = BaseAddress + 0x21AD080; //
            private static long _entityBase = BaseAddress + 0x31c4638; 
            private static long _objectManager = BaseAddress + 0x2192E08; //
            private static long _minimapObject = BaseAddress + 0x21A1FB0; //
            private static long _gameTime = BaseAddress + 0x21A1F48; //
            private static long _heroList = BaseAddress + 0x2192f28; //
            private static long _renderer = BaseAddress + 0x21fea90; //
            private static long _viewPort = BaseAddress + 0x2195CC8; //
            private static long _viewMatrix = BaseAddress + 0x21F6110;

            public static long GetLocalPlayer { get; } = Memory.Read<long>(Instances._localPlayer);
            public static long GetEntityBase { get; } = Memory.Read<long>(_entityBase);
            public static long GetobjectManager { get; } = Memory.Read<long>(_objectManager);
            public static long GetMinimapObject { get;} = Memory.Read<long>(_minimapObject);
            public static float GetGameTime { get; } = Memory.Read<float>(_gameTime);
            public static long GetHeroList { get; } = Memory.Read<long>(_heroList);
            public static long GetRenderer { get; } = Memory.Read<long>(_renderer);
            public static long GetViewPort { get; } = Memory.Read<long>(_viewPort);
            public static long GetViewMatrix { get; } = _viewMatrix;
        }

        public class Object
        {
            //LocalPlayer
            public static long Health = 0x1058; //
            public static long MaxHealth = 0x1070; //
            public static long PlayerTeam = 0x3C;
            public static long AttackRange = 0x16B4; //

            //minionlist
            public static int EntityCount = 0x10; 
            public static int EntityList = 0x8; 

            //buff
            public static long BuffManager = 0x27B0; 
            public static long BuffArray = 0x18; 
            public static long BuffEntityInfo = 0x10; 
            public static long BuffArrayLength = 0x22; 
            public static long BuffSize = 0x8;
            public static int BuffName = 0x8; 
            public static int BuffStartTime = 0x18; 
            public static int BuffEndTime = 0x1C; 
            public static int BuffDuration = 0x20; 
            public static int BuffCount = 0x8C; 
            public static int BuffCountAlt = 0x38; 

            //objectManager
            public static long ObjectMapCount = 0x48; 
            public static long ObjectMapRoot = 0x40; 
            public static long ObjectName = 0x3838;

            //minimap
            public static long MinimapObjectHud = 0x320; 
            public static long MinimapHudPosX = 0x60; 
            public static long MinimapHudPosY = 0x64; 
            public static long MinimapHudSize = 0x68; 
            public static long WorldSize = 0x28;

            //Champ Data
            public static long Position = 0x220;//
            public static long TeamID = 0x3C;//
            public static int NetworkID = 0x10;

            public static long IsVisible= 0x310; //%2 for true false
            public static long IsAlive = 0x328; //%2 for true false
        }

        public class Renderer
        {
            public static long RendererWidth = 0x0C;
            public static long RendererHeight = 0x10;
        }

        public class ViewPort
        {
            public static long MiddleScreenPosX = 0x08; // + 120 for center of champion when pressing space
            public static long MiddleScreenPosY = 0x28;
        }
        //[[baseAdress + Herolist] + 0x8] + i * 0x8
        // 
        [StructLayout(LayoutKind.Explicit)]
        public unsafe struct ObjectNode
        {
            [FieldOffset(0x0)]
            public fixed long nodes[3];
            [FieldOffset(0x20)]
            public long netId;
            [FieldOffset(0x28)]
            public long nodeObject;

        }
        /*
        [StructLayout(LayoutKind.Explicit)]
        public unsafe struct GameObject
        {

            [FieldOffset(0x1058)] //
            public float Health; 
            [FieldOffset(0x220)] 
            public Vector3 Position;
            [FieldOffset(0x1070)] //
            public float HealthMaximum;
            [FieldOffset(0x3C)] //
            public int TeamID;
            [FieldOffset(0x10)]
            public int NetworkID;
            [FieldOffset(0x3FF0)]
            public int Level;

            [FieldOffset(0x310)] public int IsVisible; //%2 for true false
            [FieldOffset(0x328)] public int IsAlive; //%2 for true false
            [FieldOffset(0xEB0)] public int IsTargetAble;


        }*/
        public unsafe struct GameObject
        {
            public long memoryID;

            // Other fields relative to memoryID
            public string Name;
            public float Health;
            public Vector3 Position;
            public float HealthMaximum;
            public int TeamID;
            public int NetworkID;
            public int Level;
            public bool IsVisible;
            public bool IsAlive;

            public GameObject(long initialMemoryID)
            {
                memoryID = initialMemoryID;

                // Calculate the offsets relative to memoryID
                Name = Memory.ReadString(memoryID + Offsets.Object.ObjectName, 20);
                Health = Memory.Read<float>(memoryID + Offsets.Object.Health);
                Position = Memory.Read<Vector3>(memoryID + Offsets.Object.Position);
                HealthMaximum = Memory.Read<float>(memoryID + Offsets.Object.MaxHealth);
                TeamID = Memory.Read<int>(memoryID + Offsets.Object.TeamID);
                NetworkID = Memory.Read<int>(memoryID + Offsets.Object.NetworkID);
                Level = Memory.Read<int>(memoryID + 0x3FF0);
                IsVisible = Memory.Read<int>(memoryID + Offsets.Object.IsVisible) % 2 == 1;
                IsAlive = Memory.Read<int>(memoryID + Offsets.Object.IsAlive) % 2 == 0;
            }
        }
    }
}
