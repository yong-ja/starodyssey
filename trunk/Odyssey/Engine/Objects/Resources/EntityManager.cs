using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Engine;
using AvengersUtd.Odyssey.Objects;
using SlimDX.Direct3D9;
using System.IO;
using AvengersUtd.Odyssey.Meshes;
using SlimDX;


namespace AvengersUtd.Odyssey.Objects
{
    /// <summary>
    /// Offers utility method to be used in the engine
    /// </summary>
    public static class EntityManager
    {
        static VertexBuffer vbQuad = null;

        static SortedDictionary<string, Mesh> meshCache = new SortedDictionary<string, Mesh>();

        static SortedDictionary<string, ExtendedMaterial[]> materialsCache =
            new SortedDictionary<string, ExtendedMaterial[]>();


        public static ExtendedMaterial[] LoadMaterials(string filename)
        {
            return materialsCache[filename];
        }

        public static Mesh LoadMesh(string filename)
        {
            if (meshCache.ContainsKey(filename))
            {
                return meshCache[filename];
            }
            else
            {
                Mesh mesh;
                try
                {
                    mesh = Mesh.FromFile(Game.Device, filename, MeshFlags.Managed);

                    meshCache.Add(filename, mesh);
                    materialsCache.Add(filename, mesh.GetMaterials());
                    return mesh;
                }
                catch (InvalidDataException ex)
                {
                    MessageBox.Show("You are missing this file: " +
                                    filename);

                    return null;
                }
            }
        }

        public static void Dispose()
        {
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

        
        public static Texture RenderToTexture(int width, int height, params IEntity[] entities)
        {
            Device device = Game.Device;
            //TODO: Aggiustare rendertotexture
            Surface oldRenderTarget = device.GetRenderTarget(0);
            Texture output = new Texture(device, width, height, 1, Usage.RenderTarget ,//| Usage.AutoGenerateMipMap,
                                         Format.X8R8G8B8, Pool.Default);
            Surface renderSurface = output.GetSurfaceLevel(0);
            RenderToSurface renderTarget =
                new RenderToSurface(device, width, height, Format.X8R8G8B8, Format.D16);

            Viewport v = new Viewport();
            v.Width = width;
            v.Height = height;
            v.MaxZ = 1.0f;


          //  device.SetRenderState(RenderState.ZEnable, ZBufferType.UseZBuffer);

            renderTarget.BeginScene(renderSurface, v);
            renderTarget.Device.Clear(ClearFlags.Target|ClearFlags.ZBuffer , Color.Black, 1.0f, 0);
            //Game.CurrentScene.Camera.Update();
            //renderTarget.Device.SetTransform(TransformState.View,
            //    Game.CurrentScene.Camera.View);

            //renderTarget.Device.SetTransform(TransformState.World, Game.CurrentScene.Camera.World);
            foreach (IEntity e in entities)
            {
                //renderTarget.Device.SetTransform(TransformState.World, Matrix.Translation(e.Position));
                e.Render();
            }

            renderTarget.EndScene(Filter.None);
            renderTarget.Dispose();


            //device.SetRenderTarget(0, output.GetSurfaceLevel(0));
            ////renderTarget.BeginScene(output.GetSurfaceLevel(0), v);
            //device.BeginScene();
            //device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.LightGray, 1.0f, 0);
            ////device.SetTransform(TransformState.World, Game.CurrentScene.Camera.World);
            ////device.SetTransform(TransformState.Projection, Game.CurrentScene.Camera.Projection);
            ////device.SetTransform(TransformState.View, Game.CurrentScene.Camera.View);
            //device.SetTransform(TransformState.View, Matrix.LookAtLH(new Vector3(0f,0f, -20f),
            //    e.Position, new Vector3(0f,1f,0)));
            //e.Render();
            //device.EndScene();
            ////renderTarget.EndScene(Filter.None);

            //renderTarget.Dispose();

            //device.SetRenderTarget(0, oldRenderTarget);
            //oldRenderTarget.Dispose();
            return output;
        }
         
    }
}