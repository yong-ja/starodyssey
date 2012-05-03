using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Materials;
using System.Drawing;
using AvengersUtd.Odyssey.Geometry;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public class AABB3D : MeshGroup
    {
        const float DefaultThickness = 0.05f;

        public float Width { get; private set; }
        public float Height { get; private set; }
        public float Depth { get; private set; }

        public static AABB3D FromSphere(ISphere sphere)
        {
            return new AABB3D(sphere.PositionV3, 2 * sphere.Radius);
        }

        public AABB3D(Vector3 position, float side)
            : this(position, side, side, side)
        { }

        public AABB3D(Vector3 position, float width, float height, float depth)
            : base(12)
        {
            PositionV3 = position;
            Width = width;
            Height = height;
            Depth = depth;

            //ushort[] indices;
            Vector3 topLeftFront = PositionV3 + new Vector3(-width / 2, height / 2, -depth / 2);
            Vector3 bottomLeftFront = PositionV3 + new Vector3(-width / 2, -height / 2, -depth / 2);
            Vector3 topRightFront = PositionV3 + new Vector3(width / 2, height / 2, -depth / 2);
            Vector3 bottomRightFront = PositionV3 + new Vector3(width / 2, -height / 2, -depth / 2);
            Vector3 topLeftBack = PositionV3 + new Vector3(-width / 2, height / 2, depth / 2);
            Vector3 topRightBack = PositionV3 + new Vector3(width / 2, height / 2, depth / 2);
            Vector3 bottomLeftBack = PositionV3 + new Vector3(-width / 2, -height / 2, depth / 2);
            Vector3 bottomRightBack = PositionV3 + new Vector3(width / 2, -height / 2, depth / 2);
            //Indices = indices;

            Objects[0] = new Box(Midpoint(topLeftFront, topRightFront), topRightFront.X - topLeftFront.X, DefaultThickness, DefaultThickness);
            Objects[1] = new Box(Midpoint(topRightFront, topRightBack), DefaultThickness, DefaultThickness, topRightBack.Z - topRightFront.Z + DefaultThickness);
            Objects[2] = new Box(Midpoint(topRightBack, topLeftBack), topRightBack.X - topLeftBack.X, DefaultThickness, DefaultThickness);
            Objects[3] = new Box(Midpoint(topLeftFront, topLeftBack), DefaultThickness, DefaultThickness, topLeftBack.Z - topLeftFront.Z + DefaultThickness);
            Objects[4] = new Box(Midpoint(topRightBack, bottomRightBack), DefaultThickness, topRightBack.Y - bottomRightBack.Y, DefaultThickness);
            Objects[5] = new Box(Midpoint(topLeftBack, bottomLeftBack), DefaultThickness, topLeftBack.Y - bottomLeftBack.Y, DefaultThickness);
            Objects[6] = new Box(Midpoint(topRightFront, bottomRightFront), DefaultThickness, topRightFront.Y - bottomRightFront.Y, DefaultThickness);
            Objects[7] = new Box(Midpoint(topLeftFront, bottomLeftFront), DefaultThickness, topLeftFront.Y - bottomLeftFront.Y, DefaultThickness);
            Objects[8] = new Box(Midpoint(bottomLeftFront, bottomRightFront), bottomRightFront.X - bottomLeftFront.X, DefaultThickness, DefaultThickness);
            Objects[9] = new Box(Midpoint(bottomRightFront, bottomRightBack), DefaultThickness, DefaultThickness, bottomRightBack.Z - bottomRightFront.Z + DefaultThickness);
            Objects[10] = new Box(Midpoint(bottomRightBack, bottomLeftBack), bottomRightBack.X - bottomLeftBack.X, DefaultThickness, DefaultThickness);
            Objects[11] = new Box(Midpoint(bottomLeftFront, bottomLeftBack), DefaultThickness, DefaultThickness, bottomLeftBack.Z - bottomLeftFront.Z + DefaultThickness);

            Material = new PhongMaterial() { DiffuseColor = Color.Yellow, AmbientCoefficient=1f};
        }

       
        private Vector3 Midpoint(Vector3 p1, Vector3 p2)
        {
            float x = (p1.X + p2.X) / 2;
            float y = (p1.Y + p2.Y) / 2;
            float z = (p1.Z + p2.Z) / 2;

            return new Vector3(x, y, z);
        }

    }
}
