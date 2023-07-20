namespace ExternalRPM
{
    public class Program
    {
        public static void Main()
        {
            ConsoleTest();
        }

        public static void ConsoleTest()
        {
            // Initialize jungle camps
            Dictionary<string, JungleCamp> jungleCamps = JungleCamp.InitializeJungleCamps();

            // Create KindredTracker and CommandLineUI instances
            KindredTracker kindredTracker = new KindredTracker(new List<JungleCamp>(jungleCamps.Values));
            CommandLineUI commandLineUI = new CommandLineUI(new List<JungleCamp>(jungleCamps.Values).ToArray());

            // Start the countdown threads
            //commandLineUI.StartCountdownThreads();
            commandLineUI.StartSingleCountdownThread();

            // Simulate updates to KindredTracker (You need to replace these with actual updates from your game)
            bool isMarkActive = false; // Replace with actual status
            bool isBlueTeam = true; // Replace with actual team
            int markCounter = 0; // Replace with actual mark counter

            kindredTracker.UpdateMarkStatus(isMarkActive, isBlueTeam, markCounter);
            List<string> entityList = new List<string>
            {
                "Sru_Crab16.1.1",
                "Sru_Crab15.1.1",
                "SRU_Murkwolf2.1.1"
            };
            foreach (var camp in jungleCamps.Values)
            { 
                camp.HandleEntityListChange(entityList);
            }
            kindredTracker.UpdateMarkStatus(isMarkActive, isBlueTeam, markCounter);
            Thread.Sleep(5000);
            List<string> entityList2 = new List<string>
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
            kindredTracker.UpdateMarkStatus(isMarkActive, isBlueTeam, markCounter);
            Thread.Sleep(5000);
            entityList2.Clear();
            for (int i = 0; i < 2; i++)
            {
                foreach (var camp in jungleCamps.Values)
                {
                    camp.HandleEntityListChange(entityList2);
                }
            }
            kindredTracker.UpdateMarkStatus(isMarkActive, isBlueTeam, markCounter);
            Thread.Sleep(5000);
            entityList2.Add("SRU_Blue7.1.1");
            markCounter = 5;
            foreach (var camp in jungleCamps.Values)
            {
                camp.HandleEntityListChange(entityList2);
            }
            kindredTracker.UpdateMarkStatus(isMarkActive, isBlueTeam, markCounter);
            Thread.Sleep(5000);

        }
    }
}