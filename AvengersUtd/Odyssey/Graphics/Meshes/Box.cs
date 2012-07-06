using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Materials;
using System.Diagnostics.Contracts;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public class Box : BaseMesh<MeshVertex>, IBox
    {
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float Depth { get; private set; }

        public Vector3 Min { get; private set; }
        public Vector3 Max { get; private set; }

        public static Box FromSphere(ISphere sphere)
        {
            return new Box(2 * sphere.Radius);
        }


        public Box(float width, float height, float depth)
            : base(MeshVertex.Description)
        {
            Width = width;
            Height = height;
            Depth = depth;

            ushort[] indices;
            Vertices = PolyMesh.CreateBox(PositionV4, Width, Height, Depth, out indices);
            Vector3 pMin, pMax;

            FindMinMax(Vertices.Cast<IPositionVertex>(), out pMin, out pMax);
            Min = pMin;
            Max = pMax;
            Indices = indices;

            Material = new PhongMaterial();
        }

        /// <summary>
        /// Creates a cube.
        /// </summary>
        /// <param name="position">A vector representing its center.</param>
        /// <param name="side">The length of the side.</param>
        public Box(float side) : this (side, side, side)
        {
        }

        static void FindMinMax(IEnumerable<IPositionVertex> vertices, out Vector3 pMin, out Vector3 pMax)
        {
            Contract.Requires(vertices != null);
            Contract.Requires(vertices.Count() >= 2);
            float minX = vertices.Min(v => v.Position.X);
            float minY = vertices.Min(v => v.Position.Y);
            float minZ = vertices.Min(v => v.Position.Z);
            float maxX = vertices.Max(v => v.Position.X);
            float maxY = vertices.Max(v => v.Position.Y);
            float maxZ = vertices.Max(v => v.Position.Z);

            pMin = new Vector3(minX, minY, minZ);
            pMax = new Vector3(maxX, maxY, maxZ);
        }

    }
}
