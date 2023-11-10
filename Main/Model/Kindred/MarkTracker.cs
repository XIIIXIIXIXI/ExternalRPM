using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Buff;

namespace ExternalRPM.Model.Kindred
{
    /*
    Description:
    This class is responsible for tracking and managing Kindred's marks in the game. 
    It keeps track of the number of marks acquired by Kindred and the respawn time of the next mark.

    Methods:
    - StartMarkTracking(KindredTracker kindredTracker): Starts the thread for tracking Kindred's marks.
    - TrackMark(): The main method responsible for continuously monitoring Kindred's mark status and count.
    - StartMarkTimer(): Starts the timer to track the remaining respawn time of the next mark.
    - ExtractMarkBuffFromBuffs(List<Buff.Buff> buffs): Extracts the latest Kindred mark buff from the list of buffs.
    - CheckMarkStatus(): Checks if a Kindred mark is currently active or not.
    - GetMarkCount(): Retrieves the number of Kindred marks acquired by the player.
*/
    public class MarkTracker
    {
        public bool IsMarkActive { get; set; } = false;
        public int MarkCount { get; set; } = 0;
        public TimeSpan MarkRespawnTime = TimeSpan.FromSeconds(45);
        public bool MarkTimerRun = false;
        BuffManager buffManager = new BuffManager();
        private readonly object lockObject = new object();

        public void StartMarkTracking(KindredTracker kindredTracker)
        {
            Thread markTrackingThread = new Thread(TrackMark);
            markTrackingThread.Start();
        }

        private void TrackMark()
        {
            bool previousMarkStatus = false;

            while (true)
            {
                bool isMarkActive = CheckMarkStatus();
                if (isMarkActive != previousMarkStatus)
                {
                    previousMarkStatus = isMarkActive;
                    if (isMarkActive == false)
                    {
                        MarkTimerRun = true;
                        StartMarkTimer();
                    }
                    else
                    {
                        MarkTimerRun = false;
                    }
                    MarkCount = GetMarkCount();
                }
                Thread.Sleep(1000);
            }
        }

        private void StartMarkTimer()
        {
            Task.Run(() =>
            {
                while (MarkTimerRun)
                {
                    MarkRespawnTime -= TimeSpan.FromSeconds(1);
                    Thread.Sleep(1000);
                }
            });
            MarkRespawnTime = TimeSpan.FromSeconds(45);
        }
        private Buff.Buff ExtractMarkBuffFromBuffs(List<Buff.Buff> buffs)
        {
            Buff.Buff latestBuff = null;
            float latestStartTime = float.MinValue;

            foreach (Buff.Buff buff in buffs)
            {
                if (buff.Name == "kindredhitlistmonste" && buff.StartTime > latestStartTime)
                {
                    latestBuff = buff;
                    latestStartTime = buff.StartTime;
                }
            }

            return latestBuff;
        }


        private bool CheckMarkStatus()
        {

            // Logic to check if the mark is active or not
            List<Buff.Buff> buffs;
            lock (lockObject)
            {
                buffs = buffManager.Buffs;
            }
            Buff.Buff latestKindredBuff = ExtractMarkBuffFromBuffs(buffs);
            if (latestKindredBuff != null)
            {
                return latestKindredBuff.Count2 == 1;
            }
            return false;
        }

        private int GetMarkCount()
        {
            List<Buff.Buff> buffs;
            lock (lockObject)
            {
                buffs = buffManager.Buffs;
            }

            foreach (Buff.Buff buff in buffs)
            {
                if (buff.Name == "kindredmarkofthekind")
                {
                    return buff.Count;
                }
            }

            return 0;
        }
    }
}
