using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey;
using SlimDX.Direct3D9;
using SlimDX;
using AvengersUtd.Odyssey.Engine.Meshes;
using AvengersUtd.Odyssey.Objects.Materials;

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class GridRenderer:Renderer
    {
        Mesh sphere;
        SimpleMesh<DiffuseMaterial> simpleSphere;


        public override void Init()
        {
            sphere = Mesh.FromFile(Game.Device, "Meshes\\Planets\\Large.x", MeshFlags.Managed);
            simpleSphere = new SimpleMesh<DiffuseMaterial>(sphere);
        }

        public override void Render()
        {
            device.SetTransform(TransformState.World, Matrix.Translation(0, 0, 25));
            simpleSphere.DrawWithEffect();    
        }

        public override void ProcessInput()
        {
            
        }

        public override void Dispose()
        {
            sphere.Dispose();
        }
    }
}
