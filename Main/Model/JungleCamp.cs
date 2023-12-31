﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Color = SharpDX.Color;

namespace ExternalRPM.Model
{
    /*
     Description:
    This class represents a jungle camp in the game. It holds information about the camp's name, memory address, respawn time, position, and status. 
    The class provides methods to handle changes in the entity list, updating the camp's status when it is cleared or becomes alive again. 
    Additionally, a countdown thread is utilized to track the remaining respawn time after the camp is cleared.

    Public Methods:
    - HandleEntityListChange(HashSet<string> entityList): Handles changes in the entity list and updates the camp's status accordingly.
    - _CountdownThreadLogic(): A thread method that tracks the remaining respawn time after the camp is cleared.
    - InitializeJungleCamps(): A static method that initializes a dictionary of jungle camps with their respective properties.
     */
    public class JungleCamp
    {
        public string MemoryId { get; set; } //address of name in memory
        public string Name { get; set; } //readable name

        public TimeSpan RespawnTime { get; set; } //Default start respawnTime

        public TimeSpan RemainingRespawnTime { get; set; } //time untill respawn
        public bool IsAlive { get; set; }

        public Color Color { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public int AbsentIterations { get; set; } // number of iterations the camp has been absent

        private Thread _countdownThread;
        private volatile bool _stopCountdown;

        public void HandleEntityListChange(HashSet<string> entityList)
        {
            if (entityList.Contains(MemoryId))
            {
                // The Jungle camp was previously marked as cleared
                if (!IsAlive)
                {
                    IsAlive = true;
                    RemainingRespawnTime = TimeSpan.Zero;
                    AbsentIterations = 0;
                    _stopCountdown = true;

                }
            }
            else
            {
                // The Jungle camp was previously marked as alive
                if (IsAlive)
                {
                    AbsentIterations++;
                    //Check if the camp has been absent for a certain number of iteration to consider it cleared
                    if (AbsentIterations > 1)
                    {
                        IsAlive = false;
                        Color = Color.White;
                        RemainingRespawnTime = RespawnTime;
                        _stopCountdown = false;
                        var countdownThread = new Thread(_CountdownThreadLogic);
                        countdownThread.Start();
                    }
                }
            }
        }
        private void _CountdownThreadLogic()
        {
            while ((RemainingRespawnTime > TimeSpan.Zero) && !_stopCountdown)
            {
                Thread.Sleep(1000);

                if (IsAlive)
                {
                    RemainingRespawnTime = TimeSpan.Zero;
                    break;
                }

                RemainingRespawnTime -= TimeSpan.FromSeconds(1);
            }
        }
        public static Dictionary<string, JungleCamp> InitializeJungleCamps()
        {
            Dictionary<string, JungleCamp> jungleCamps = new Dictionary<string, JungleCamp>(14); // Initialize with expected capacity


            // Helper method to add jungle camps
            void AddJungleCamp(string id, string name, TimeSpan respawnTime, float positionX, float positionY)
            {
                jungleCamps.Add(id, new JungleCamp
                {
                    MemoryId = id,
                    Name = name,
                    RespawnTime = respawnTime,
                    PositionX = positionX,
                    PositionY = positionY,
                    Color = Color.White,
                    IsAlive = false,
                    RemainingRespawnTime = respawnTime
                });
            }

            // Add jungle camps with their respawn timers
            AddJungleCamp("SRU_Blue1.1.1", "BlueBuffB", TimeSpan.FromMinutes(5), 3626, 7705); // Blue
            AddJungleCamp("SRU_Blue7.1.1", "BlueBuffR", TimeSpan.FromMinutes(5), 10834, 6796); // Red

            AddJungleCamp("SRU_Red4.1.1", "RedBuffB", TimeSpan.FromMinutes(5), 7591, 3845);
            AddJungleCamp("SRU_Red10.1.1", "RedBuffR", TimeSpan.FromMinutes(5), 6927, 10726);

            AddJungleCamp("SRU_Murkwolf2.1.1", "WolfsB", TimeSpan.FromMinutes(2.25), 3679, 6341);
            AddJungleCamp("SRU_Murkwolf8.1.1", "WolfsR", TimeSpan.FromMinutes(2.25), 10905, 8285);

            AddJungleCamp("SRU_Gromp13.1.1", "GrompB", TimeSpan.FromMinutes(2.25), 1945, 8283);
            AddJungleCamp("SRU_Gromp14.1.1", "GrompR", TimeSpan.FromMinutes(2.25), 12536, 6277);

            AddJungleCamp("SRU_Razorbeak3.1.1", "RaportsB", TimeSpan.FromMinutes(2.25), 7040, 5168);
            AddJungleCamp("SRU_Razorbeak9.1.1", "RaportsR", TimeSpan.FromMinutes(2.25), 1943, 8283);

            AddJungleCamp("SRU_Krug5.1.1", "KrugsB", TimeSpan.FromMinutes(2.25), 8396, 2619);
            AddJungleCamp("SRU_Krug11.1.1", "KrugsR", TimeSpan.FromMinutes(2.25), 6206, 12098);

            AddJungleCamp("Sru_Crab16.1.1", "ScuttleU", TimeSpan.FromMinutes(2.30), 4052, 9448); // Up
            AddJungleCamp("Sru_Crab15.1.1", "ScuttleD", TimeSpan.FromMinutes(2.30), 10358, 5405); // Down

            return jungleCamps;
        }

    }
}
