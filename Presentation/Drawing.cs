using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Modules;

namespace ExternalRPM.Presentation
{
    class Drawing
    {
        public static void OnDeviceDraw()
        {
            if (Utils.IsGameOnDisplay())
            {
                Minimap.DrawMinimapOverlay();
            }
        }
    }
}
