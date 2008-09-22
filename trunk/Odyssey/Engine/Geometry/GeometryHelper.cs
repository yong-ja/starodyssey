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
            Vector4 transformedMinimum = Vector3.Transform(originalBox.Minimum, worldMatrix);
            Vector4 transformedMaximum = Vector3.Transform(originalBox.Maximum, worldMatrix);


            Vector3 newMinimum = new Vector3(transformedMinimum.X, transformedMinimum.Y, transformedMinimum.Z);
            Vector3 newMaximum = new Vector3(transformedMaximum.X, transformedMaximum.Y, transformedMaximum.Z);
            return new BoundingBox(newMinimum, newMaximum);
        }

        public static BoundingSphere TransformBoundingSphere(BoundingSphere originalSphere, Matrix worldMatrix)
        {
            Vector4 transformedCenter = Vector3.Transform(originalSphere.Center, worldMatrix);
            Vector3 newCenter = new Vector3(transformedCenter.X, transformedCenter.Y, transformedCenter.Z);
            return new BoundingSphere(newCenter, originalSphere.Radius);
        }

        public static Mesh CreateSphere(ISphere sphere)
        {
            return Mesh.CreateSphere(Game.Device, sphere.Radius, 16, 16);
        }
    }
}
