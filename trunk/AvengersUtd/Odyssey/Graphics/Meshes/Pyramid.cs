using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Materials;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public class Pyramid:BaseMesh<MeshVertex>
    {
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float Depth { get; private set; }

        public Pyramid(float width, float height, float depth)
            : base(MeshVertex.Description)
        {
            Width = width;
            Height = height;
            Depth = depth;

            ushort[] indices;
            Vertices = PolyMesh.CreatePyramid(PositionV4, Width, Height, Depth, out indices);
            
            Indices = indices;

            Material = new PhongMaterial();
        }
    }
}
