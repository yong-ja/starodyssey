using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Materials;
using System.Drawing;
using AvengersUtd.Odyssey.Geometry;
using System.Diagnostics.Contracts;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public class BoundingBox : MeshGroup
    {
        public const float DefaultThickness = 0.05f;

        public float Width { get; private set; }
        public float Height { get; private set; }
        public float Depth { get; private set; }

        public Vector3 Min { get; private set; }
        public Vector3 Max { get; private set; }

        public static BoundingBox FromSphere(ISphere sphere)
        {
            return new BoundingBox(2 * sphere.Radius);
        }

        public BoundingBox(float side)
            : this(side, side, side)
        { }

        public BoundingBox(float width, float height, float depth)
            : base(12)
        {
            Vector3 center = Vector3.Zero;
            Width = width;
            Height = height;
            Depth = depth;

            //ushort[] indices;
            Vector3 topLeftFront = center + new Vector3(-width / 2, height / 2, -depth / 2);
            Vector3 bottomLeftFront = center + new Vector3(-width / 2, -height / 2, -depth / 2);
            Vector3 topRightFront = center + new Vector3(width / 2, height / 2, -depth / 2);
            Vector3 bottomRightFront = center + new Vector3(width / 2, -height / 2, -depth / 2);
            Vector3 topLeftBack = center + new Vector3(-width / 2, height / 2, depth / 2);
            Vector3 topRightBack = center + new Vector3(width / 2, height / 2, depth / 2);
            Vector3 bottomLeftBack = center + new Vector3(-width / 2, -height / 2, depth / 2);
            Vector3 bottomRightBack = center + new Vector3(width / 2, -height / 2, depth / 2);

            Vector3[] vertices = new Vector3[] { topLeftFront, bottomLeftFront, topRightFront, bottomRightFront,
                topLeftBack, topRightBack, bottomLeftBack, bottomRightBack };

            Objects[0] = new Box(topRightFront.X - topLeftFront.X, DefaultThickness, DefaultThickness) { PositionV3 = Midpoint(topLeftFront, topRightFront) };
            Objects[1] = new Box(DefaultThickness, DefaultThickness, topRightBack.Z - topRightFront.Z + DefaultThickness) { PositionV3 = Midpoint(topRightFront, topRightBack) };
            Objects[2] = new Box(topRightBack.X - topLeftBack.X, DefaultThickness, DefaultThickness) { PositionV3 = Midpoint(topRightBack, topLeftBack) };
            Objects[3] = new Box(DefaultThickness, DefaultThickness, topLeftBack.Z - topLeftFront.Z + DefaultThickness) { PositionV3 = Midpoint(topLeftFront, topLeftBack) };
            Objects[4] = new Box(DefaultThickness, topRightBack.Y - bottomRightBack.Y, DefaultThickness) { PositionV3 = Midpoint(topRightBack, bottomRightBack) };
            Objects[5] = new Box(DefaultThickness, topLeftBack.Y - bottomLeftBack.Y, DefaultThickness) { PositionV3 = Midpoint(topLeftBack, bottomLeftBack) };
            Objects[6] = new Box(DefaultThickness, topRightFront.Y - bottomRightFront.Y, DefaultThickness) { PositionV3 = Midpoint(topRightFront, bottomRightFront) };
            Objects[7] = new Box(DefaultThickness, topLeftFront.Y - bottomLeftFront.Y, DefaultThickness) { PositionV3 = Midpoint(topLeftFront, bottomLeftFront) };
            Objects[8] = new Box(bottomRightFront.X - bottomLeftFront.X, DefaultThickness, DefaultThickness) { PositionV3 = Midpoint(bottomLeftFront, bottomRightFront) };
            Objects[9] = new Box(DefaultThickness, DefaultThickness, bottomRightBack.Z - bottomRightFront.Z + DefaultThickness) { PositionV3 = Midpoint(bottomRightFront, bottomRightBack) };
            Objects[10] = new Box(bottomRightBack.X - bottomLeftBack.X, DefaultThickness, DefaultThickness) { PositionV3 = Midpoint(bottomRightBack, bottomLeftBack) };
            Objects[11] = new Box(DefaultThickness, DefaultThickness, bottomLeftBack.Z - bottomLeftFront.Z + DefaultThickness) { PositionV3 = Midpoint(bottomLeftFront, bottomLeftBack) };

            Material = new PhongMaterial() { DiffuseColor = Color.DarkGreen, AmbientCoefficient=1f};
            foreach (IRenderable rObject in Objects)
                rObject.Material = Material;
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
