﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Modules;

namespace ExternalRPM.Model.Kindred
{
    /*
    KindredTracker Class

    Description:
    This class is responsible for tracking the status of jungle camps relevant to Kindred's marks in the game. 
    It utilizes a list of JungleCamp objects and a LocalPlayer instance to monitor the status of the camps.
    The class also works in conjunction with the MarkTracker class to keep track of Kindred's marks.

    Methods:
    - UpdateMarkStatus(): Updates the status of the jungle camps based on Kindred's marks.
    - ResetCampColors(): Resets the color of all jungle camps to their default value.
    - FindNextPotentialCamps(bool isBlueTeam, int markCounter): Identifies the next potential jungle camps for Kindred's marks based on the player's team and current mark count.
    - GetLowestRespawnTimeIndex(List<JungleCamp> camps): Returns the index of the jungle camp with the shortest remaining respawn time from the given list of jungle camps.
*/
    public class KindredTracker
    {
        private readonly List<JungleCamp> jungleCamps;
        private readonly LocalPlayer localPlayer;
        private List<int> nextPotentialCampIndices;
        private readonly TimeSpan MarkRespawnTime = TimeSpan.FromMinutes(0.45);
        public MarkTracker markTracker;

        public KindredTracker(List<JungleCamp> jungleCamps, LocalPlayer localPlayer)
        {
            this.localPlayer = localPlayer;
            this.jungleCamps = jungleCamps;
            nextPotentialCampIndices = new List<int>();
            markTracker = new MarkTracker();
            markTracker.StartMarkTracking(this);
        }

        public void UpdateMarkStatus()
        {
            if (!markTracker.IsMarkActive)
            {
                FindNextPotentialCamps(localPlayer.IsBlueTeam, markTracker.MarkCount);
                ResetCampColors();
                foreach (var index in nextPotentialCampIndices)
                {

                    jungleCamps[index].Color = SharpDX.Color.Green;
                }
            }
            else
            {
                ResetCampColors();
            }
        }

        private void ResetCampColors()
        {
            foreach (var camp in jungleCamps)
            {
                camp.Color = SharpDX.Color.White;
            }
        }
        private void FindNextPotentialCamps(bool isBlueTeam, int markCounter)
        {
            nextPotentialCampIndices.Clear();
            List<string> availableMonsters = new List<string>();

            if (!isBlueTeam)
            {
                if (markCounter == 0)
                {
                    if (jungleCamps[12].IsAlive || jungleCamps[12].RemainingRespawnTime < MarkRespawnTime)  // left Sru_Crab16.1.1
                    {
                        nextPotentialCampIndices.Add(12);
                    }
                    if (jungleCamps[13].IsAlive || jungleCamps[13].RemainingRespawnTime < MarkRespawnTime)  // right Sru_Crab15.1.1
                    {
                        nextPotentialCampIndices.Add(13);
                    }

                    // Find the camp(s) with the shortest remaining respawn time
                    if (nextPotentialCampIndices.Count == 0)
                    {
                        List<JungleCamp> zeroMark = new List<JungleCamp>()
                        {
                            jungleCamps[12],
                            jungleCamps[13]
                        };
                        nextPotentialCampIndices.Add(GetLowestRespawnTimeIndex(zeroMark));
                    }
                }
                else if (markCounter >= 1 && markCounter <= 3)
                {
                    if (jungleCamps[12].IsAlive || jungleCamps[12].RemainingRespawnTime < MarkRespawnTime) // left Sru_Crab16.1.1
                    {
                        nextPotentialCampIndices.Add(12);
                    }
                    if (jungleCamps[13].IsAlive || jungleCamps[13].RemainingRespawnTime < MarkRespawnTime) // right Sru_Crab15.1.1
                    {
                        nextPotentialCampIndices.Add(13);
                    }
                    if (jungleCamps[6].IsAlive || jungleCamps[6].RemainingRespawnTime < MarkRespawnTime) // SRU_Gromp13.1.1
                    {
                        nextPotentialCampIndices.Add(6);
                    }
                    if (jungleCamps[8].IsAlive || jungleCamps[8].RemainingRespawnTime < MarkRespawnTime) // SRU_Razorbeak3.1.1
                    {
                        nextPotentialCampIndices.Add(8);
                    }

                    // Find the camp(s) with the shortest remaining respawn time
                    if (nextPotentialCampIndices.Count == 0)
                    {
                        List<JungleCamp> oneToThreeMarks = new List<JungleCamp>()
                        {
                            jungleCamps[12],
                            jungleCamps[13],
                            jungleCamps[6],
                            jungleCamps[8]
                        };
                        nextPotentialCampIndices.Add(GetLowestRespawnTimeIndex(oneToThreeMarks));
                    }
                }
                else if (markCounter >= 4 && markCounter <= 7)
                {
                    if (jungleCamps[0].IsAlive || jungleCamps[0].RemainingRespawnTime < MarkRespawnTime) // SRU_Blue1.1.1
                    {
                        nextPotentialCampIndices.Add(0);
                    }
                    if (jungleCamps[2].IsAlive || jungleCamps[2].RemainingRespawnTime < MarkRespawnTime) // SRU_Red4.1.1
                    {
                        nextPotentialCampIndices.Add(2);
                    }
                    if (jungleCamps[10].IsAlive || jungleCamps[10].RemainingRespawnTime < MarkRespawnTime) // SRU_Krug5.1.1
                    {
                        nextPotentialCampIndices.Add(10);
                    }
                    if (jungleCamps[4].IsAlive || jungleCamps[4].RemainingRespawnTime < MarkRespawnTime) // SRU_Murkwolf2.1.1
                    {
                        nextPotentialCampIndices.Add(4);
                    }

                    // Find the camp(s) with the shortest remaining respawn time
                    if (nextPotentialCampIndices.Count == 0)
                    {
                        List<JungleCamp> fourToSevenMarks = new List<JungleCamp>()
                        {
                            jungleCamps[0],
                            jungleCamps[2],
                            jungleCamps[9],
                            jungleCamps[4]
                        };
                        nextPotentialCampIndices.Add(GetLowestRespawnTimeIndex(fourToSevenMarks));
                    }
                }
            }
            else
            {
                if (markCounter == 0)
                {
                    if (jungleCamps[12].IsAlive || jungleCamps[12].RemainingRespawnTime < MarkRespawnTime)  // left Sru_Crab16.1.1
                    {
                        nextPotentialCampIndices.Add(12);
                    }
                    if (jungleCamps[13].IsAlive || jungleCamps[13].RemainingRespawnTime < MarkRespawnTime)  // right Sru_Crab15.1.1
                    {
                        nextPotentialCampIndices.Add(13);
                    }

                    // Find the camp(s) with the shortest remaining respawn time
                    if (nextPotentialCampIndices.Count == 0)
                    {
                        List<JungleCamp> zeroMark = new List<JungleCamp>()
                        {
                            jungleCamps[12],
                            jungleCamps[13]
                        };
                        nextPotentialCampIndices.Add(GetLowestRespawnTimeIndex(zeroMark));
                    }
                }
                else if (markCounter >= 1 && markCounter <= 3)
                {
                    if (jungleCamps[12].IsAlive || jungleCamps[12].RemainingRespawnTime < MarkRespawnTime) // left Sru_Crab16.1.1
                    {
                        nextPotentialCampIndices.Add(12);
                    }
                    if (jungleCamps[13].IsAlive || jungleCamps[13].RemainingRespawnTime < MarkRespawnTime) // right Sru_Crab15.1.1
                    {
                        nextPotentialCampIndices.Add(13);
                    }
                    if (jungleCamps[7].IsAlive || jungleCamps[7].RemainingRespawnTime < MarkRespawnTime) // SRU_Gromp14.1.1
                    {
                        nextPotentialCampIndices.Add(7);
                    }
                    if (jungleCamps[9].IsAlive || jungleCamps[9].RemainingRespawnTime < MarkRespawnTime) // SRU_Razorbeak9.1.1
                    {
                        nextPotentialCampIndices.Add(9);
                    }

                    // Find the camp(s) with the shortest remaining respawn time
                    if (nextPotentialCampIndices.Count == 0)
                    {
                        List<JungleCamp> oneToThreeMarks = new List<JungleCamp>()
                        {
                            jungleCamps[12],
                            jungleCamps[13],
                            jungleCamps[7],
                            jungleCamps[9]
                        };
                        nextPotentialCampIndices.Add(GetLowestRespawnTimeIndex(oneToThreeMarks));
                    }
                }
                else if (markCounter >= 4 && markCounter <= 7)
                {
                    if (jungleCamps[1].IsAlive || jungleCamps[1].RemainingRespawnTime < MarkRespawnTime) // SRU_Blue7.1.1
                    {
                        nextPotentialCampIndices.Add(1);
                    }
                    if (jungleCamps[3].IsAlive || jungleCamps[3].RemainingRespawnTime < MarkRespawnTime) // SRU_Red10.1.1
                    {
                        nextPotentialCampIndices.Add(3);
                    }
                    if (jungleCamps[11].IsAlive || jungleCamps[11].RemainingRespawnTime < MarkRespawnTime) // SRU_Krug11.1.1
                    {
                        nextPotentialCampIndices.Add(11);
                    }
                    if (jungleCamps[5].IsAlive || jungleCamps[5].RemainingRespawnTime < MarkRespawnTime) // SRU_Murkwolf8.1.1
                    {
                        nextPotentialCampIndices.Add(5);
                    }

                    // Find the camp(s) with the shortest remaining respawn time
                    if (nextPotentialCampIndices.Count == 0)
                    {
                        List<JungleCamp> fourToSevenMarks = new List<JungleCamp>()
                        {
                            jungleCamps[1],
                            jungleCamps[3],
                            jungleCamps[11],
                            jungleCamps[5]
                        };
                        nextPotentialCampIndices.Add(GetLowestRespawnTimeIndex(fourToSevenMarks));
                    }
                }
            }
        }

        private int GetLowestRespawnTimeIndex(List<JungleCamp> camps)
        {
            int lowestIndex = jungleCamps.IndexOf(camps[0]);
            TimeSpan lowestRespawnTime = camps[0].RemainingRespawnTime;
            for (int i = 1; i < camps.Count; i++)
            {
                if (camps[i].RemainingRespawnTime < lowestRespawnTime)
                {
                    lowestIndex = jungleCamps.IndexOf(camps[i]);
                    lowestRespawnTime = camps[i].RemainingRespawnTime;
                }
            }
            return lowestIndex;
        }
    }
}
