using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX.Direct3D9;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public static class GeometryHelper
    {
        public static Mesh CreateParallelepiped(IAxisAlignedBox box)
        {
            float width = Math.Abs(box.Maximum.X - box.Minimum.X);
            float height = Math.Abs(box.Maximum.Y - box.Minimum.Y);
            float depth = Math.Abs(box.Maximum.Z - box.Minimum.Z);

            return Mesh.CreateBox(Game.Device, width, height, depth);

        }

        public static BoundingBox TransformBoundingBox(BoundingBox originalBox, Matrix worldMatrix)
        {
            Vector3[] corners = originalBox.GetCorners();
            corners = Vector3.TransformCoordinate(corners, ref worldMatrix);
            return BoundingBox.FromPoints(corners);
        }

        public static BoundingSphere TransformBoundingSphere(BoundingSphere originalSphere, Vector3 newCenter)
        {
            return new BoundingSphere(newCenter, originalSphere.Radius);
        }

        public static Mesh CreateSphere(ISphere sphere)
        {
            return Mesh.CreateSphere(Game.Device, sphere.Radius, 16, 16);
        }
    }
}
