using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalRPM.Modules
{
    class Utils
    {
        public static Process GetLeagueProcess()
        {
            try
            {
                return Process.GetProcessesByName("League of legends").FirstOrDefault();
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Could not Find League of Legends's Process\n{Ex.ToString()}");
                return null;
            }
        }
    }
}
