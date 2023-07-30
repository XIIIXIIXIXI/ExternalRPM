using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Data;
using ExternalRPM.Modules;
using SharpDX;
using Color = SharpDX.Color;

namespace ExternalRPM
{
    class Minimap
    {
        private static readonly long minimapObjectAddress;
        private static readonly long minimapHudAddress;
        private static float scaleFactor;

        static Minimap()
        {
            minimapObjectAddress = Offsets.Instances.GetMinimapObject;
            minimapHudAddress = Memory.Read<long>(minimapObjectAddress + Offsets.Object.MinimapObjectHud);
            scaleFactor = getScaleFactor();
        }

        public static void DrawMinimapOverlay()
        {
            Vector2 vector = getMinimapPosition();
            float minimapSize = getMinimapSize();
            Presentation.DrawFactory.DrawFilledBox(vector.X, vector.Y, minimapSize, minimapSize, Color.Aqua);
        }
        private static float getWorldSize()
        {
            return Memory.Read<float>(minimapHudAddress + Offsets.Object.WorldSize);
        }

        private static float getMinimapSize()
        {
            return Memory.Read<float>(minimapHudAddress + Offsets.Object.MinimapHudSize);
        }

        private static float getScaleFactor()
        {
            return getMinimapSize() - getWorldSize();
        }

        private static Vector2 getMinimapPosition()
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
