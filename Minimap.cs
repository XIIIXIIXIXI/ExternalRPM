using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Data;
using ExternalRPM.Model;
using ExternalRPM.Model.Kindred;
using ExternalRPM.Modules;
using SharpDX;
using SharpDX.Direct3D9;
using Color = SharpDX.Color;

namespace ExternalRPM
{
    class Minimap
    {
        private readonly long minimapObjectAddress;
        private readonly long minimapHudAddress;
        private float scaleFactor;

        private JungleCamp[] jungleCamps;
        private KindredTracker kindredTracker;

        public Minimap(JungleCamp[] jungleCamps, KindredTracker kindredTracker)
        {
            this.jungleCamps = jungleCamps;
            this.kindredTracker = kindredTracker;
            minimapObjectAddress = Offsets.Instances.GetMinimapObject;
            minimapHudAddress = Memory.Read<long>(minimapObjectAddress + Offsets.Object.MinimapObjectHud);
            scaleFactor = getScaleFactor();
        }

        public void DrawMinimapOverlay()
        {
            Vector2 vector = getMinimapPosition();
            float minimapSize = getMinimapSize();
            Presentation.DrawFactory.DrawFilledBox(vector.X, vector.Y, minimapSize, minimapSize, Color.Aqua);
        }

        public void DrawMarkerOnMinimap()
        {
            foreach (var camp in jungleCamps)
            {
                Vector2 vector = WorldToMap(camp.PositionX, camp.PositionY);
                Presentation.DrawFactory.DrawPoint(vector.X, vector.Y, Color.Red);
            }

        }

        public void DrawRespawntimerOnMap()
        {
            String textToDraw;
            Vector2 position;
            foreach (var camp in jungleCamps)
            {
                if (camp.IsAlive || camp.RemainingRespawnTime <= TimeSpan.Zero)
                {
                }
                else
                {
                    Color colorCode = camp.Color == Color.Green ? Color.Green : Color.White;
                    position = WorldToMap(camp.PositionX, camp.PositionY);
                    textToDraw = $"{camp.RemainingRespawnTime.Minutes}:{camp.RemainingRespawnTime.Seconds:D2}";
                    Presentation.DrawFactory.DrawFont(textToDraw, FontSize: 20, position, Color.White);
                }
            }
        }
        private float getWorldSize()
        {
            return Memory.Read<float>(minimapHudAddress + Offsets.Object.WorldSize);
        }

        private float getMinimapSize()
        {
            return Memory.Read<float>(minimapHudAddress + Offsets.Object.MinimapHudSize);
        }

        private float getScaleFactor()
        {
            return getMinimapSize() / getWorldSize();
        }

        private Vector2 getMinimapPosition()
        {
            float minimapPosX = Memory.Read<float>(minimapHudAddress + Offsets.Object.MinimapHudPosX);
            float minimapPosY = Memory.Read<float>(minimapHudAddress + Offsets.Object.MinimapHudPosY);

            return new Vector2(minimapPosX, minimapPosY);
        }

        public Vector2 WorldToMap(float posX, float posY)
        {
            Vector2 vector = getMinimapPosition();
            float x = (posX * scaleFactor) + vector.X;
            float y = vector.Y + getMinimapSize() - (posY * scaleFactor);
            return new Vector2(x, y);

        }
        // This function should be moved out of this class, but we keep it here for simplicity for now
        public void DrawKindredMarkRespawnTimer()
        {
            String textToDraw;
            Vector2 position;
            position.X = 900;
            position.Y = 10;
            if (kindredTracker.markTracker.MarkTimerRun)
            {
                textToDraw =
                    $"Mark: {kindredTracker.markTracker.MarkRespawnTime.Seconds:D2}";
                Presentation.DrawFactory.DrawFont(textToDraw, FontSize: 20, position, Color.White);
            }
            
        }

    }
    /*
    class Minimap
    {
        private float scaleFactor;
        private float minimapPosX;
        private float minimapPosY;
        private float minimapSize;
        private float worldSize;

        public Minimap()
        {
            updateScaleFactor();
        }
        public void updateScaleFactor()
        {
            long minimapObjectAddress = Offsets.Instances.GetMinimapObject;
            long minimapHudAddress = Memory.Read<long>(minimapObjectAddress + Offsets.Object.MinimapObjectHud);
            this.worldSize = Memory.Read<float>(minimapHudAddress + Offsets.Object.WorldSize);
            this.minimapSize = Memory.Read<float>(minimapHudAddress + Offsets.Object.MinimapHudSize);
            this.scaleFactor = minimapSize / this.worldSize;
            this.minimapPosX = Memory.Read<float>(minimapHudAddress + Offsets.Object.MinimapHudPosX);
            this.minimapPosY = Memory.Read<float>(minimapHudAddress + Offsets.Object.MinimapHudPosY);
        }

        public Vector2 WorldToMap(float posX, float posY)
        {
            Vector2 vector;
            vector.X = (posX * this.scaleFactor) + this.minimapPosX;
            vector.Y = this.minimapPosY + this.minimapSize - (posY * this.scaleFactor);
            return vector;
        }

        public void DrawMinimapOverlay()
        {
            Presentation.DrawFactory.DrawFilledBox(this.minimapPosX, this.minimapPosY, this.minimapSize, this.minimapSize, Color.Aqua);
        }
    }
    */
}
