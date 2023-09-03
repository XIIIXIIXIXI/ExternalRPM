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
        private Mediator mediator;
        private Minimap minimap;

        public Drawing(Mediator mediator)
        {
            this.mediator = mediator;
           
        }

        public void OnDeviceDrawMinimap()
        {
            if (Utils.IsGameOnDisplay())
            {
                //minimap.DrawMinimapOverlay();
                //minimap.DrawMarkerOnMinimap();
                mediator.Minimap.DrawRespawntimerOnMap();
                mediator.Minimap.DrawKindredMarkRespawnTimer();
            }
        }
        public void OnDeviceDrawAttackRange()
        {
            mediator.ToggleAttackRange();
        }
    }
}
