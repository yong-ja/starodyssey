using System;
using System.Collections.Generic;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public class BoundingFrustum : IEquatable<BoundingFrustum>
    {
        Plane[] planes;
        Vector3[] cornerArray;
        Matrix matrix;

        #region Properties

        public Vector3[] CornerArray
        {
            get { return cornerArray; }
        }

        public Plane[] Planes
        {
            get { return planes; }
        }

        #endregion

        public BoundingFrustum()
        {
            planes = new Plane[6];
            cornerArray = new Vector3[8];
        }

        public BoundingFrustum(Matrix value) : this()
        {
            SetMatrix(value);
        }

        public void SetMatrix(Matrix value)
        {
            matrix = value;
            planes[2].Normal.X = -value.M14 - value.M11;
            planes[2].Normal.Y = -value.M24 - value.M21;
            planes[2].Normal.Z = -value.M34 - value.M31;
            planes[2].D = -value.M44 - value.M41;
            planes[3].Normal.X = -value.M14 + value.M11;
            planes[3].Normal.Y = -value.M24 + value.M21;
            planes[3].Normal.Z = -value.M34 + value.M31;
            planes[3].D = -value.M44 + value.M41;
            planes[4].Normal.X = -value.M14 + value.M12;
            planes[4].Normal.Y = -value.M24 + value.M22;
            planes[4].Normal.Z = -value.M34 + value.M32;
            planes[4].D = -value.M44 + value.M42;
            planes[5].Normal.X = -value.M14 - value.M12;
            planes[5].Normal.Y = -value.M24 - value.M22;
            planes[5].Normal.Z = -value.M34 - value.M32;
            planes[5].D = -value.M44 - value.M42;
            planes[0].Normal.X = -value.M13;
            planes[0].Normal.Y = -value.M23;
            planes[0].Normal.Z = -value.M33;
            planes[0].D = -value.M43;
            planes[1].Normal.X = -value.M14 + value.M13;
            planes[1].Normal.Y = -value.M24 + value.M23;
            planes[1].Normal.Z = -value.M34 + value.M33;
            planes[1].D = -value.M44 + value.M43;
            for (int i = 0; i < 6; i++)
            {
                float num2 = planes[i].Normal.Length();
                planes[i].Normal = (planes[i].Normal/num2);
                planes[i].D /= num2;
            }
            Ray ray = ComputeIntersectionLine(planes[0], planes[2]);
            cornerArray[0] = ComputeIntersection(planes[4], ray);
            cornerArray[3] = ComputeIntersection(planes[5], ray);
            ray = ComputeIntersectionLine(planes[3], planes[0]);
            cornerArray[1] = ComputeIntersection(planes[4], ray);
            cornerArray[2] = ComputeIntersection(planes[5], ray);
            ray = ComputeIntersectionLine(planes[2], planes[1]);
            cornerArray[4] = ComputeIntersection(planes[4], ray);
            cornerArray[7] = ComputeIntersection(planes[5], ray);
            ray = ComputeIntersectionLine(planes[1], planes[3]);
            cornerArray[5] = ComputeIntersection(planes[4], ray);
            cornerArray[6] = ComputeIntersection(planes[5], ray);
        }

        #region Containment methods

        public static ContainmentType Contains(BoundingFrustum frustum, BoundingBox box)
        {
            bool flag = false;
            foreach (Plane plane in frustum.Planes)
            {
                switch (BoundingBox.Intersects(box, plane))
                {
                    case PlaneIntersectionType.Front:
                        return ContainmentType.Disjoint;

                    case PlaneIntersectionType.Intersecting:
                        flag = true;
                        break;
                }
            }
            if (!flag)
            {
                return ContainmentType.Contains;
            }
            return ContainmentType.Intersects;
        }

        /*
        public static ContainmentType Contains(BoundingFrustum frustum1, BoundingFrustum frustum2)
        {
            if (frustum1 == null || frustum2 == null)
            {
                throw new ArgumentNullException("frustum", "Null not allowed");
            }
            if (!BoundingFrustum.Intersects(frustum1, frustum2))
            {
                return ContainmentType.Disjoint;
            }
            foreach (Vector3 vector in frustum2.CornerArray)
            {
                if (BoundingFrustum.Contains(frustum1,vector)== ContainmentType.Disjoint)
                {
                    return ContainmentType.Intersects;
                }
            }
            return ContainmentType.Contains;
        }*/

        public static ContainmentType Contains(BoundingFrustum frustum, Vector3 point)
        {
            foreach (Plane plane in frustum.Planes)
            {
                float dot = (((plane.Normal.X*point.X) + (plane.Normal.Y*point.Y)) + (plane.Normal.Z*point.Z)) + plane.D;
                if (dot > 1E-05f)
                {
                    return ContainmentType.Disjoint;
                }
            }
            return ContainmentType.Contains;
        }

        public static ContainmentType Contains(BoundingFrustum frustum, BoundingSphere sphere)
        {
            Vector3 center = sphere.Center;
            float radius = sphere.Radius;
            int intersections = 0;
            foreach (Plane plane in frustum.Planes)
            {
                float dot = ((plane.Normal.X*center.X) + (plane.Normal.Y*center.Y)) + (plane.Normal.Z*center.Z);
                float dotD = dot + plane.D;
                if (dotD > radius)
                {
                    return ContainmentType.Disjoint;
                }
                if (dotD < -radius)
                {
                    intersections++;
                }
            }
            if (intersections != 6)
            {
                return ContainmentType.Intersects;
            }
            return ContainmentType.Contains;
        }

        #endregion

        #region Intersection methods

        public static bool Intersects(BoundingFrustum frustum, BoundingBox box)
        {
            return Contains(frustum, box) == ContainmentType.Intersects;
        }

        #endregion

        static Ray ComputeIntersectionLine(Plane p1, Plane p2)
        {
            Ray ray = new Ray();
            ray.Direction = Vector3.Cross(p1.Normal, p2.Normal);
            float num = ray.Direction.LengthSquared();
            ray.Position = Vector3.Cross((-p1.D*p2.Normal) + (p2.D*p1.Normal), ray.Direction)/num;
            return ray;
        }

        static Vector3 ComputeIntersection(Plane plane, Ray ray)
        {
            float num = (-plane.D - Vector3.Dot(plane.Normal, ray.Position))/Vector3.Dot(plane.Normal, ray.Direction);
            return (ray.Position + ray.Direction*num);
        }

        #region IEquatable<BoundingFrustum> Members

        public bool Equals(BoundingFrustum other)
        {
            if (other == null)
            {
                return false;
            }
            return (this.matrix == other.matrix);
        }

        #endregion

        public override bool Equals(object obj)
        {
            bool flag = false;
            BoundingFrustum frustum = obj as BoundingFrustum;
            if (frustum != null)
            {
                flag = this.matrix == frustum.matrix;
            }
            return flag;
        }

        public override int GetHashCode()
        {
            return this.matrix.GetHashCode();
        }
    }
}