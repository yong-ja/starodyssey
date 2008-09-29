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

        #region Vector3 extensions
        public static Vector3 ToVector3(this Vector4 vector4)
        {
            return new Vector3(vector4.X, vector4.Y, vector4.Z);
        }

        public static Vector3[] ToVector3Array(this Vector4[] vector4Array)
        {
            Vector3[] vector3Array = new Vector3[vector4Array.Length - 1];
            for (int i = 0; i < vector4Array.Length - 1; i++)
                vector3Array[i] = vector4Array[i].ToVector3();

            return vector3Array;
        }
        #endregion

        public static BoundingBox TransformBoundingBox(BoundingBox originalBox, Matrix worldMatrix)
        {
            Vector3[] corners = originalBox.GetCorners();
            Vector4[] transformedCorners = Vector3.Transform(corners, ref worldMatrix);

            return BoundingBox.FromPoints(transformedCorners.ToVector3Array());
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
