using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Rendering;
using AvengersUtd.Odyssey.Text;
using AvengersUtd.Odyssey.Utils;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace AvengersUtd.Odyssey
{
    public static class Game
    {
        private static Timer timer;
        public static Renderer CurrentRenderer { get; private set;}
        public static DebugLogger Logger { get; private set; }
        public static double FrameTime { get; private set; }

        static Game()
        {
            timer = new Timer();
            timer.Reset();
            Logger = new DebugLogger();
        }
        public static void Loop()
        {
            CurrentRenderer.Device.ImmediateContext.ClearRenderTargetView(RenderForm11.renderTarget, Color.LightBlue);
            CurrentRenderer.Render();
            CurrentRenderer.Present();
            FrameTime = timer.GetElapsedTime();
        }

        public static void ChangeRenderer(Renderer renderer)
        {
            CurrentRenderer = renderer;
            renderer.Init();
        }


    }
}
