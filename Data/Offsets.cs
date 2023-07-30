using ExternalRPM.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalRPM.Data
{
    //13.14.522.7601
    internal class Offsets
    {
        public static long BaseAddress = Utils.GetLeagueProcess().MainModule.BaseAddress.ToInt64();

        
        public class Instances
        {
            //Patch 13.13
            private static long _localPlayer = BaseAddress + 0x421F828; 
            private static long _entityBase = BaseAddress + 0x31C45B8; //MinionList
            private static long _objectManager = BaseAddress + 0x2173220; 
            private static long _minimapObject = BaseAddress + 0x42137f0; //
            private static long _gameTime = BaseAddress + 0x4213710;

            public static long GetLocalPlayer { get; } = Memory.Read<long>(Instances._localPlayer);
            public static long GetEntityBase { get; } = Memory.Read<long>(_entityBase);
            public static long GetobjectManager { get; } = Memory.Read<long>(_objectManager);
            public static long GetMinimapObject { get;} = Memory.Read<long>(_minimapObject);
            public static float GetGameTime { get; } = Memory.Read<float>(_gameTime);
        }

        public class Object
        {
            //LocalPlayer
            public static int Health = 0x1068;
            public static int MaxHealth = 0x1080;
            public static int PlayerTeam = 0x3C;

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
            public static long ObjectName = 0x3848;

            //minimap
            public static long MinimapObjectHud = 0x320; 
            public static long MinimapHudPosX = 0x60; 
            public static long MinimapHudPosY = 0x64; 
            public static long MinimapHudSize = 0x68; 
            public static long WorldSize = 0x28;

        }
    }
}
