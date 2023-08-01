using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Windows;
using SharpDX.Direct3D;
using SharpDX.Mathematics.Interop;
using Color = SharpDX.Color;
using Rectangle = SharpDX.Rectangle;


namespace ExternalRPM.Presentation
{
    class DrawFactory 
    {
        public static SharpDX.Direct3D9.Device device = null;
        public static Direct3D D3D = new Direct3D();

        /// <summary>
        /// Disposes DrawFactory resources
        /// </summary>
        public static void DisposeGraphicsFactory()
        {
            font.Dispose();
            drawBoxLine.Dispose();
            drawLine.Dispose();
            drawCircleLine.Dispose();
            drawFilledBoxLine.Dispose();
            drawTriLine.Dispose();

            device.Dispose();
        }

        public struct Margins
        {
            public int Left, Right, Top, Bottom;
        }

        public static Margins Marg;
        public static SharpDX.Direct3D9.Font font;
        public static SharpDX.Direct3D9.Line drawBoxLine;
        public static SharpDX.Direct3D9.Line drawLine;
        public static SharpDX.Direct3D9.Line drawCircleLine;
        public static SharpDX.Direct3D9.Line drawFilledBoxLine;
        public static SharpDX.Direct3D9.Line drawTriLine;

        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_LAYERED = 0x80000;
        public const int WS_EX_TRANSPARENT = 0x20;
        public const int LWA_ALPHA = 0x2;
        public const int LWA_COLORKEY = 0x1;

        public static void DrawFont(string text, int FontSize, Vector2 position, Color color)
        {
            var FontDescription = font.Description;
            FontDescription.Height = FontSize;

            var fontDimension = font.MeasureText(null, text, new Rectangle((int)position.X, (int)position.Y, 0, 0), FontDrawFlags.Center | FontDrawFlags.VerticalCenter);
            font.DrawText(null, text, fontDimension, FontDrawFlags.Center | FontDrawFlags.VerticalCenter, color);
        }
        public static void DrawFilledBox(float x, float y, float w, float h, Color Color)
        {
            Vector2[] vLine = new Vector2[2];

            drawFilledBoxLine.Width = w;
            drawFilledBoxLine.GLLines = true;
            drawFilledBoxLine.Antialias = false;

            vLine[0].X = x + w / 2;
            vLine[0].Y = y;
            vLine[1].X = x + w / 2;
            vLine[1].Y = y + h;

            drawFilledBoxLine.Begin();
            drawFilledBoxLine.Draw(vLine, new RawColorBGRA(Color.B, Color.G, Color.R, Color.A));
            drawFilledBoxLine.End();

            /*              Example             */
            /* DrawFilledBox(x , y, w, h, color); */
        }
        public static void DrawPoint(float x, float y, Color color)
        {
            DrawFilledBox(x, y, 1, 1, color);

            /*           Example            */
            /* DrawPoint(x Axis, y Axis, Color */
        }
    }
}
