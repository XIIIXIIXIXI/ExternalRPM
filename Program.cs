using System.Drawing.Text;
using ExternalRPM.Buff;
using ExternalRPM.Data;
using ExternalRPM.Model;
using ExternalRPM.Model.Kindred;
using ExternalRPM.Modules;
using ExternalRPM.Presentation;
using System.Threading.Tasks;
using SharpDX;
using System.Diagnostics;

namespace ExternalRPM
{
    public class Program
    {
        
        public static Presentation.Overlay overlay; // = new Presentation.Overlay();
        static void Main()
        {


            //Release();
            UnitRadiusService.ParseUnitRadiusData();
            Mediator mediator = new Mediator();
            overlay = new Presentation.Overlay(mediator);
            Task.Run(async () =>
            {

                await Task.Run(() => overlay.Show());

            }).GetAwaiter().GetResult();








            /*
            while (true)
            {
                (Matrix viewMatrix, Matrix projectionMatrix) = Renderer.GetMatrices();
                //Debug.WriteLine($"View X: {viewMatrix.M41}, Y: {viewMatrix.M42}, Z: {viewMatrix.M43}");
                Vector3 screenPosition;
                var w2s = Renderer.WorldToScreen(localPlayer.GetPosition(), out screenPosition); 
                //Thread.Sleep(1000);
            }
            */

            //var matrix = Renderer.GetViewMatrix();
            //var projMatrix = Renderer.GetProjectionMatrix();



        }

        public static void Release()
        {

            Mediator mediator = new Mediator();
            overlay = new Presentation.Overlay(mediator);
            Task.Run(async () =>
            {
                await Task.Run(() => overlay.Show());

            }).GetAwaiter().GetResult();
        }
        
        public static void MainLoop()
        {

            Dictionary<string, JungleCamp> jungleCamps = JungleCamp.InitializeJungleCamps();
            
            LocalPlayer localPlayer = LocalPlayer.GetInstance();

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