using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalRPM.Modules
{
    class UnitRadiusService
    {
        public static void ParseUnitRadiusData()
        {
            try
            {
                string jsonFilePath = "C:\\Users\\marti\\source\\repos\\ExternalRPM\\Data\\UnitRadius.json";
                string UnitRadiusDataString = File.ReadAllText(jsonFilePath);
                LocalPlayer.UnitRadiusData = JObject.Parse(UnitRadiusDataString);

            }
            catch (Exception Ex)
            {
                throw new Exception("UnitRadiusParseExecption");
            }
        }
    }
}
