using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public static class Extensions
    {
        #region Vector2 extensions
        //public static Point ToPoint(this Vector2 vector2)
        //{
        //    return new Point((int)vector2.X, (int)vector2.Y);
        //}
        #endregion

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

    }
}
