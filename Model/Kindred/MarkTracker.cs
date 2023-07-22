using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Buff;

namespace ExternalRPM.Model.Kindred
{
    public class MarkTracker
    {
        private bool IsMarkActive { get; set; } = false;
        private int markCount { get; set; } = 0;
        BuffManager buffManager = new BuffManager();
        private KindredTracker kindredTracker;
        private readonly object lockObject = new object();

        public void StartMarkTracking(KindredTracker kindredTracker)
        {
            this.kindredTracker = kindredTracker;
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
                    markCount = GetMarkCount();
                    kindredTracker.UpdateMarkStatus(isMarkActive, markCount);
                }

                // You can adjust the sleep duration based on how frequently you want to check for mark changes
                Thread.Sleep(1000);
            }
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
