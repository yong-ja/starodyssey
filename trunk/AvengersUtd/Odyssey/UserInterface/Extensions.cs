using System.Drawing;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface
{
    public static class Extensions
    {
        public static Color4 ToColor4(this Color color)
        {
            return new Color4(color.ToArgb());
        }

        public static Color ToColor(this Color4 color)
        {
            return Color.FromArgb(color.ToArgb());
        }

        public static bool IsEmpty(this Color color)
        {
            return color.ToArgb() == 0;
        }

        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }
        public static Vector3 ToVector3(this Point point)
        {
            return new Vector3(point.X, point.Y, 0);
        }
        public static Vector3 ToVector3(this Vector2 vector2)
        {
            return new Vector3(vector2.X, vector2.Y, 0);
        }
        public static Vector3 ToVector3(this Vector2 vector2, float z)
        {
            return new Vector3(vector2.X, vector2.Y, z);
        }
    }
}
