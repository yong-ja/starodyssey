using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Materials;
using System.Drawing;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public class Box : BaseMesh<MeshVertex>, IBox
    {
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float Depth { get; private set; }

        public static Box FromSphere(ISphere sphere)
        {
            return new Box(sphere.PositionV3, sphere.Radius);
        }

        public Box(Vector3 position, float width, float height, float depth)
            : base(MeshVertex.Description)
        {
            PositionV3 = position;
            Width = width;
            Height = height;
            Depth = depth;

            ushort[] indices;
            Vertices = PolyMesh.CreateBox(PositionV4, Width, Height, Depth, out indices);
            Indices = indices;

            Material = new PhongMaterial();
        }

        /// <summary>
        /// Creates a cube.
        /// </summary>
        /// <param name="position">A vector representing its center.</param>
        /// <param name="side">The length of the side.</param>
        public Box(Vector3 position, float side) : this (position, side, side, side)
        {
        }


        
    }
}
