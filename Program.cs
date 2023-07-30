﻿using ExternalRPM.Buff;
using ExternalRPM.Data;
using ExternalRPM.Model;
using ExternalRPM.Model.Kindred;
using ExternalRPM.Modules;
using ExternalRPM.Presentation;
using System.Threading.Tasks;

namespace ExternalRPM
{
    public class Program
    {
        public static Presentation.Overlay overlay = new Presentation.Overlay();
        static void Main()
        {
            Task.Run(async () =>
            {
                await Task.Run(() => overlay.Show());
            }).GetAwaiter().GetResult();

            //ConsoleTest.TestBuffManager();
            //MainLoop();
        }

        public static void MainLoop()
        {

            Dictionary<string, JungleCamp> jungleCamps = JungleCamp.InitializeJungleCamps();
            LocalPlayer localPlayer = new LocalPlayer();

            KindredTracker kindredTracker = new KindredTracker(new List<JungleCamp>(jungleCamps.Values), localPlayer);
            CommandLineUI commandLineUI = new CommandLineUI(new List<JungleCamp>(jungleCamps.Values).ToArray(), kindredTracker);


            commandLineUI.StartCountdownThreads();
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

        
    }
}