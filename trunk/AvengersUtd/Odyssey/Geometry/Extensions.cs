using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public static class Extensions
    {
        #region Geometry

        #region Vector2 extensions
        public static Vector4 ToVector4(this Vector2D vector2)
        {
            return new Vector4(vector2, 0f, 1.0f);
        }

        #endregion

        #region Vector3 extensions
        public static Vector4 ToVector4(this Vector3 vector3)
        {
            return new Vector4(vector3, 1.0f);
        }

       

        #endregion

        #region Vector4 extensions
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

        #region Plane extensions
        #endregion

        #endregion

    }
}
