using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Engine;
using SlimDX.Direct3D9;
using SlimDX;

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class GridRenderer: Renderer
    {
        Mesh sphere;
        Matrix world;

        public override void Init()
        {
            sphere = Mesh.FromFile(Game.Device, "Resources\\sphere.x", MeshFlags.Managed);
            world = Matrix.Translation(new Vector3(0, 0, 10));
            qCam.SetCamera(new SlimDX.Vector3(0,-10, -10));
        }

        public override void Render()
        {
            Game.Device.SetTransform(TransformState.World, world);
            sphere.DrawSubset(0);
            return;
        }

        public override void ProcessInput()
        {
            return;
        }

        public override void Dispose()
        {
            if (!sphere.Disposed)
                sphere.Dispose();
        }
    }
}
