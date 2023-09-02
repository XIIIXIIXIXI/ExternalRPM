using System;
using System.Collections.Generic;
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

        public static (Matrix viewMatrix, Matrix projectionMatrix) GetViewMatrix()
        {
            return Memory.ReadMatrices(ViewMatrix);

        }

        public static Matrix GetProjectionMatrix()
        {
            return Memory.ReadMatrix(projectionMatrix);
        }
    }
}
