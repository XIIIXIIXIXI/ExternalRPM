using ExternalRPM.Buff;
using ExternalRPM.Data;
using ExternalRPM.Model;
using ExternalRPM.Model.Kindred;
using ExternalRPM.Modules;
using ExternalRPM.Presentation;

namespace ExternalRPM
{
    public class Program
    {
        public static void Main()
        {
            //ConsoleTest.TestBuffManager();
            MainLoop();
        }

        public static void MainLoop()
        {
            //ConsoleTest.TestBuffManager();
            Dictionary<string, JungleCamp> jungleCamps = JungleCamp.InitializeJungleCamps();
            LocalPlayer localPlayer = new LocalPlayer();
            // Create KindredTracker and CommandLineUI instances
            KindredTracker kindredTracker = new KindredTracker(new List<JungleCamp>(jungleCamps.Values), localPlayer);
            CommandLineUI commandLineUI = new CommandLineUI(new List<JungleCamp>(jungleCamps.Values).ToArray());

            // Start the countdown threads
            //commandLineUI.StartCountdownThreads();
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
        }

        
    }
}