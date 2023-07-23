using ExternalRPM.Buff;
using ExternalRPM.Model;
using ExternalRPM.Model.Kindred;
using ExternalRPM.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Modules;

namespace ExternalRPM
{
    public class ConsoleTest
    {
        public static void TestBuffManager()
        {
            while (true)
            {

                BuffManager buffManager = new BuffManager();
                List<Buff.Buff> buffList = buffManager.Buffs;
                for (int i = 0; i < buffList.Count; i++)
                {
                    if (buffList[i].Name == "kindredhitlistmonste")
                    {

                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine(
                            $"{buffList[i].Name}, Loc: {i}, count:{buffList[i].Count}, count2:{buffList[i].Count2}, start:{buffList[i].StartTime}, end:{buffList[i].EndTime}");

                    }
                    else if (buffList[i].Name == "kindredlegendpassive")
                    {
                        Console.SetCursorPosition(0, 1);
                        Console.WriteLine(
                            $"{buffList[i].Name}, Loc: {i}, count:{buffList[i].Count}, count2:{buffList[i].Count2}, start:{buffList[i].StartTime}, end:{buffList[i].EndTime}");
                    }
                }

                Thread.Sleep(1000);
            }
        }

        public static void ConsoleTestSingular()
        {
            // Initialize jungle camps
            Dictionary<string, JungleCamp> jungleCamps = JungleCamp.InitializeJungleCamps();
            LocalPlayer loaLocalPlayer = new LocalPlayer();
            // Create KindredTracker and CommandLineUI instances
            KindredTracker kindredTracker = new KindredTracker(new List<JungleCamp>(jungleCamps.Values), loaLocalPlayer);
            CommandLineUI commandLineUI = new CommandLineUI(new List<JungleCamp>(jungleCamps.Values).ToArray(), kindredTracker);

            // Start the countdown threads
            //commandLineUI.StartCountdownThreads();
            commandLineUI.StartSingleCountdownThread();

            // Simulate updates to KindredTracker (You need to replace these with actual updates from your game)
            bool isMarkActive = false; // Replace with actual status
            bool isBlueTeam = true; // Replace with actual team
            int markCounter = 0; // Replace with actual mark counter

            kindredTracker.UpdateMarkStatus();
            HashSet<string> entityList = new HashSet<string>()
            {
                "Sru_Crab16.1.1",
                "Sru_Crab15.1.1",
                "SRU_Murkwolf2.1.1"
            };
            foreach (var camp in jungleCamps.Values)
            {
                camp.HandleEntityListChange(entityList);
            }
            kindredTracker.UpdateMarkStatus();
            Thread.Sleep(5000);
            HashSet<string> entityList2 = new HashSet<string>
            {
                "SRU_Blue7.1.1",
                "SRU_Red10.1.1",
                "SRU_Krug11.1.1",
                "SRU_Murkwolf8.1.1"
            };
            foreach (var camp in jungleCamps.Values)
            {
                camp.HandleEntityListChange(entityList2);
            }
            kindredTracker.UpdateMarkStatus();
            Thread.Sleep(5000);
            entityList2.Clear();
            for (int i = 0; i < 2; i++)
            {
                foreach (var camp in jungleCamps.Values)
                {
                    camp.HandleEntityListChange(entityList2);
                }
            }
            kindredTracker.UpdateMarkStatus();
            Thread.Sleep(5000);
            entityList2.Add("SRU_Blue7.1.1");
            markCounter = 5;
            foreach (var camp in jungleCamps.Values)
            {
                camp.HandleEntityListChange(entityList2);
            }
            kindredTracker.UpdateMarkStatus();
            Thread.Sleep(5000);
        }

        public static void ConsoleTestFinal()
        {
            Dictionary<string, JungleCamp> jungleCamps = JungleCamp.InitializeJungleCamps();
            LocalPlayer localPlayer = new LocalPlayer();
            // Create KindredTracker and CommandLineUI instances
            KindredTracker kindredTracker = new KindredTracker(new List<JungleCamp>(jungleCamps.Values), localPlayer);
            CommandLineUI commandLineUI = new CommandLineUI(new List<JungleCamp>(jungleCamps.Values).ToArray(), kindredTracker);

            // Start the countdown threads
            //commandLineUI.StartCountdownThreads();
            commandLineUI.StartCountdownThreads();

            bool isMarkActive = false; // Replace with actual status
            bool isBlueTeam = true; // Replace with actual team
            int markCounter = 0; // Replace with actual mark counter

            kindredTracker.UpdateMarkStatus();
            HashSet<string> entityList = new HashSet<string>
            {
            };
            foreach (var camp in jungleCamps.Values)
            {
                camp.HandleEntityListChange(entityList);
            }
            kindredTracker.UpdateMarkStatus();
            Thread.Sleep(5000);
            HashSet<string> entityList2 = new HashSet<string>
            {
                "Sru_Crab16.1.1",
                "Sru_Crab15.1.1",
                "SRU_Murkwolf2.1.1",
                "SRU_Blue1.1.1",
                "SRU_Blue7.1.1",
                "SRU_Red4.1.1",
                "SRU_Red10.1.1",
                "SRU_Murkwolf8.1.1",
                "SRU_Gromp13.1.1",
                "SRU_Gromp14.1.1",
                "SRU_Razorbeak3.1.1",
                "SRU_Razorbeak9.1.1",
                "SRU_Krug5.1.1",
                "SRU_Krug11.1.1"
            };
            for (int i = 0; i < 2; i++)
            {
                foreach (var camp in jungleCamps.Values)
                {
                    camp.HandleEntityListChange(entityList2);
                }
            }
            kindredTracker.UpdateMarkStatus();
            Thread.Sleep(5000);
            markCounter = 5;
            kindredTracker.UpdateMarkStatus();
            Thread.Sleep(5000);
            HashSet<string> entityList3 = new HashSet<string>
            {
                "Sru_Crab16.1.1",
                "Sru_Crab15.1.1",
                "SRU_Murkwolf2.1.1",
                "SRU_Blue1.1.1",
                "SRU_Blue7.1.1",
                "SRU_Red4.1.1",
                "SRU_Razorbeak3.1.1",
                "SRU_Razorbeak9.1.1",
                "SRU_Krug5.1.1",
                "SRU_Krug11.1.1"
            };
            for (int i = 0; i < 2; i++)
            {
                foreach (var camp in jungleCamps.Values)
                {
                    camp.HandleEntityListChange(entityList3);
                }
            }
            kindredTracker.UpdateMarkStatus();
            Thread.Sleep(5000);
        }
    }
}
