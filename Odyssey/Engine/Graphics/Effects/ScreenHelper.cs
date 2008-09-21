using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using AvengersUtd.Odyssey.Utils.Collections;
using SlimDX;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Rendering;

namespace AvengersUtd.Odyssey.Graphics.Effects
{
    public static class ScreenHelper
    {
        /// <summary>
        /// Helper method that renders a collection of Transform nodes (each of
        /// which contains a <see cref="IEntity"/> on a texture.
        /// </summary>
        /// <param name="width">The width, in pixels, of the resulting texture.</param>
        /// <param name="height">The height, in pixels, of the resulting texture.</param>
        /// <param name="textureFormat">The format of the resulting texture.</param>
        /// <param name="clearColor">The color to use to clear the texture.</param>
        /// <param name="renderCommand">The render command that will tell this method how to render the scene.</param>
        /// <returns></returns>
        public static void RenderToTexture(int width, int height, Format textureFormat, Texture texture, Color4 clearColor,
                                       IRenderCommand renderCommand)
        {
            Device device = Game.Device;

            RenderToSurface renderTarget =
                new RenderToSurface(device, width, height, textureFormat, Format.D24S8);


            Viewport v = new Viewport {Width = width, Height = height, MaxZ = 1.0f};
            Surface renderSurface = texture.GetSurfaceLevel(0);
            renderTarget.BeginScene(renderSurface, v);
            renderTarget.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, clearColor, 1.0f, 0);

            renderCommand.PerformRender();

            renderTarget.EndScene(Filter.None);
            renderSurface.Dispose();
            renderTarget.Dispose();
        }

        /*
       public static void RenderOverlayQuad(Texture t, int width, int height)
       {
           Device device = Game.Device;
           if (vbQuad == null)
               vbQuad = GetScreenAlignedQuad(width, height);

           //device.RenderState.AlphaSourceBlend = Blend.One;
           //device.RenderState.DestinationBlend = Blend.SourceAlpha;
           //device.RenderState.AlphaBlendOperation = BlendOperation.Add;
           //device.RenderState.AlphaBlendEnable = true;

           device.VertexFormat = CustomVertex.TransformedTextured.Format;
           device.SetStreamSource(0, vbQuad, 0);
           device.SetTexture(0, t);
           device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);

           //device.RenderState.AlphaBlendEnable = false;
       }

       public static VertexBuffer GetScreenAlignedQuad(float width, float height)
       {
           Device device = Game.Device;

           VertexBuffer vb = new VertexBuffer
               (typeof (CustomVertex.TransformedTextured), 4, device, Usage.None,
                CustomVertex.TransformedTextured.Format, Pool.Managed);
           CustomVertex.TransformedTextured[] quad =
               (CustomVertex.TransformedTextured[]) vb.Lock(0, 0);
           int z = 0;
           quad[0] = new CustomVertex.TransformedTextured(0, height, z, 1f, 0f, 1f);
           quad[1] = new CustomVertex.TransformedTextured(0, 0, z, 1f, 0f, 0f);
           quad[2] = new CustomVertex.TransformedTextured(width, height, z, 1f, 1f, 1f);
           quad[3] = new CustomVertex.TransformedTextured(width, 0f, z, 1f, 1f, 0f);

           vb.Unlock();
           return vb;
       }*/
    }
}