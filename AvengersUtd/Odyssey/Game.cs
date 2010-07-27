using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Rendering;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace AvengersUtd.Odyssey
{
    public static class Game
    {
        public static Renderer CurrentScene { get; private set;}

        public static void Loop()
        {
            CurrentScene.Device.ImmediateContext.ClearRenderTargetView(RenderForm11.renderTarget, Color.LightBlue);
            CurrentScene.Render();
            CurrentScene.Present();
        }

        public static void ChangeRenderer(Renderer renderer)
        {
            renderer.Init();
            CurrentScene = renderer;
        }


    }
}
