using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Model;
using ExternalRPM.Model.Kindred;
using ExternalRPM.Modules;

namespace ExternalRPM.Presentation
{
    class Drawing
    {
        private JungleCamp[] jungleCamps;
        private Minimap minimap;
        private KindredTracker kindredTracker;
        public Drawing(JungleCamp[] jungleCamps, KindredTracker kindredTracker)
        {
            this.jungleCamps = jungleCamps;
            this.kindredTracker = kindredTracker;
            this.minimap = new Minimap(this.jungleCamps, this.kindredTracker);
        }

        public void OnDeviceDraw()
        {
            if (Utils.IsGameOnDisplay())
            {
                //minimap.DrawMinimapOverlay();
                //minimap.DrawMarkerOnMinimap();
                minimap.DrawRespawntimerOnMap();
                
            }
        }
    }
}
