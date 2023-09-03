using ExternalRPM.Model.Kindred;
using ExternalRPM.Modules;
using ExternalRPM.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalRPM.Model
{
    public class Mediator
    {
        public Minimap Minimap { get; private set; }
        public LocalPlayer LocalPlayer { get; private set; }

        public Mediator() 
        {
            this.LocalPlayer = LocalPlayer.GetInstance();
        }

        public void ToggleJungleTracker()
        {
            Dictionary<string, JungleCamp> jungleCamps = JungleCamp.InitializeJungleCamps();
            JungleCamp[] jungleCampsArray = jungleCamps.Values.ToArray();
            KindredTracker kindredTracker = new KindredTracker(new List<JungleCamp>(jungleCamps.Values), LocalPlayer);
            Minimap minimap = new Minimap(jungleCampsArray, kindredTracker);
            Task.Run(async () =>
            {
                Task.Run(() => campManager(jungleCamps, kindredTracker));

            }).GetAwaiter().GetResult();
        }

        public static void campManager(Dictionary<string, JungleCamp> jungleCamps, KindredTracker kindredTracker)
        {
            HashSet<string> camps = new HashSet<string>();
            EntityReader memoryReader = new EntityReader();
            while (true)
            {
                camps = memoryReader.GetJungleCampsEntityList();
                foreach (JungleCamp jungleCamp in jungleCamps.Values)
                {
                    jungleCamp.HandleEntityListChange(camps);
                }
                kindredTracker.UpdateMarkStatus();
                Thread.Sleep(1000);
            }
        }

        public void ToggleAttackRange()
        {
            LocalPlayer.DrawAttackRange(SharpDX.Color.Azure, 0.2f);

        }

    }
}
