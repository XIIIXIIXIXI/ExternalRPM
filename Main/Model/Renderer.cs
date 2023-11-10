using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalRPM.Data;
using ExternalRPM.Modules;
using SharpDX;

namespace ExternalRPM.Model
{
    class Renderer
    {
        private static long ViewMatrix = Offsets.Instances.GetViewMatrix;
        private static long projectionMatrix = ViewMatrix + 0x60;
        private static long Instance { get; } = Offsets.Instances.GetRenderer;

        
        public static (Matrix viewMatrix, Matrix projectionMatrix) GetMatrices()
        {
            return Memory.ReadMatrices(ViewMatrix);

        }
        public static Matrix GetViewProjectionMatrix()
        {
            (Matrix viewMatrix, Matrix projectionMatrix) = GetMatrices();
            return Matrix.Multiply(viewMatrix, projectionMatrix);
        }

        public static Vector2 GetScreenResolution()
        {
            return new Vector2() { X = Memory.Read<int>(Instance + Offsets.Renderer.RendererWidth), Y = Memory.Read<int>(Instance + Offsets.Renderer.RendererHeight) };
        }
        public static bool WorldToScreen(Vector3 origin, out Vector3 screen)
        {
            Vector2 resolution = GetScreenResolution();
            Matrix mWorld = new Matrix(1.0f, 0.0f, 0.0f, 0.0f,
                                        0.0f, 1.0f, 0.0f, 0.0f,
                                        0.0f, 0.0f, 1.0f, 0.0f,
                                        0.0f, 0.0f, 0.0f, 1.0f);
            (Matrix ViewMatrix, Matrix ProjectionMatrix) = GetMatrices();
            Matrix viewProjection = Matrix.Multiply(ViewMatrix, ProjectionMatrix);
            Matrix worldViewProjection = Matrix.Multiply(mWorld, viewProjection);

            //TopLeftCorner of Window
            int x = 0;
            int y = 0;

            screen = Vector3.Project(origin, x, y, resolution.X, resolution.Y, 0.0f, 1.0f, worldViewProjection);

            if (ViewMatrix.M21 != 0.0f)
                return false;

            screen.X = (screen.X) / resolution.X * resolution.X;
            screen.Y = (screen.Y) / resolution.Y * resolution.Y;

            if (screen.X < 0.0f || screen.X > resolution.X)
                return false;

            if (screen.Y < 0.0f || screen.Y > resolution.Y)
                return false;

            return true;
        }
    }
}
