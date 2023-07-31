using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExternalRPM.Model;
using ExternalRPM.Model.Kindred;
using ExternalRPM.Modules;
using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Windows;
using Color = SharpDX.Color;

namespace ExternalRPM.Presentation
{
    public partial class Overlay : Form
    {
        private JungleCamp[] jungleCamps;
        private readonly KindredTracker kindredTracker;
        public Overlay(JungleCamp[] jungleCamps, KindredTracker kindredTracker)
        {
            InitializeComponent();
            this.jungleCamps = jungleCamps;
            this.kindredTracker = kindredTracker;
        }

        internal void OnDraw()
        {
            Drawing drawing = new Drawing(jungleCamps, kindredTracker);
            RenderLoop.Run(this, () =>
            {
                 NativeImport.BringWindowToTop(this.Handle);

                DrawFactory.device.Clear(ClearFlags.Target, new SharpDX.Mathematics.Interop.RawColorBGRA(0, 0, 0, 0),
                    1.0f, 0);
                DrawFactory.device.SetRenderState(RenderState.ZEnable, false);
                DrawFactory.device.SetRenderState(RenderState.Lighting, false);
                DrawFactory.device.SetRenderState(RenderState.CullMode, Cull.None);
                DrawFactory.device.SetTransform(TransformState.Projection,
                    Matrix.OrthoOffCenterLH(0, this.Width, this.Height, 0, 0, 1));

                DrawFactory.device.BeginScene();

                //Change this with a draw handler
                //DrawFactory.DrawFilledBox(200, 200, 600, 600, Color.Aqua);
                drawing.OnDeviceDraw();
                //Presentation.Drawing.OnDeviceDraw(jungleCamps);

                DrawFactory.device.EndScene();
                DrawFactory.device.Present();
            });
        }

        private void Overlay_Load(object sender, EventArgs e)
        {
            OnLoad();
        }

        private static bool IsInitialised = false;

        internal void OnLoad()
        {
            NativeImport.SetWindowLong(this.Handle, DrawFactory.GWL_EXSTYLE,
                (IntPtr)(NativeImport.GetWindowLong(this.Handle, DrawFactory.GWL_EXSTYLE) ^ DrawFactory.WS_EX_LAYERED ^
                         DrawFactory.WS_EX_TRANSPARENT));

            NativeImport.SetLayeredWindowAttributes(this.Handle, 0, 255, DrawFactory.LWA_ALPHA);

            if (IsInitialised == false)
            {
                PresentParameters presentParameters = new PresentParameters();
                presentParameters.Windowed = true;
                presentParameters.SwapEffect = SwapEffect.Discard;
                presentParameters.BackBufferFormat = Format.A8R8G8B8;

                DrawFactory.device = new Device(DrawFactory.D3D, 0, DeviceType.Hardware, this.Handle,
                    CreateFlags.HardwareVertexProcessing, presentParameters);

                DrawFactory.drawLine = new Line(DrawFactory.device);
                DrawFactory.drawBoxLine = new Line(DrawFactory.device);
                DrawFactory.drawCircleLine = new Line(DrawFactory.device);
                DrawFactory.drawFilledBoxLine = new Line(DrawFactory.device);
                DrawFactory.drawTriLine = new Line(DrawFactory.device);

                FontDescription fontDescription = new FontDescription()
                {
                    FaceName = "Fixedsys Regular",
                    CharacterSet = FontCharacterSet.Default,
                    Height = 20,
                    Weight = FontWeight.Bold,
                    MipLevels = 0,
                    OutputPrecision = FontPrecision.Default,
                    PitchAndFamily = FontPitchAndFamily.Default,
                    Quality = FontQuality.ClearType
                };

                DrawFactory.font = new SharpDX.Direct3D9.Font(DrawFactory.device, fontDescription);
                //DrawFactory.InitialiseCircleDrawing(DrawFactory.device);

                IsInitialised = true;

                OnDraw();
            }
        }

        private void Overlay_Paint(object sender, PaintEventArgs e)
        {
            OnPaint();
        }
        public void OnPaint()
        {
            DrawFactory.Marg.Left = 0;
            DrawFactory.Marg.Top = 0;
            DrawFactory.Marg.Right = this.Width;
            DrawFactory.Marg.Bottom = this.Height;

            NativeImport.DwmExtendFrameIntoClientArea(this.Handle, ref DrawFactory.Marg);
        }
    }
}
