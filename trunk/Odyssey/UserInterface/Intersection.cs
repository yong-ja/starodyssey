#region Disclaimer

/* 
 * Intersection
 *
 * Created on 21 August 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey User Interface Library
 *
 * This source code is Intellectual property of the Author
 * and is released under the Creative Commons Attribution 
 * NonCommercial License, available at:
 * http://creativecommons.org/licenses/by-nc/3.0/ 
 * You can alter and use this source code as you wish, 
 * provided that you do not use the results in commercial
 * projects, without the express and written consent of
 * the Author.
 *
 */

#endregion

using System.Drawing;
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface
{
    public static class Intersection
    {
        public static bool RectangleTest(Vector2 position, Size size, Point cursorLocation)
        {
            int xEvent = cursorLocation.X;
            int yEvent = cursorLocation.Y;
            int xPos = (int) position.X;
            int yPos = (int) position.Y;

            if ((xEvent >= xPos && xEvent <= xPos + size.Width) &&
                (yEvent >= yPos && yEvent <= yPos + size.Height))
                return true;
            else
                return false;
        }

        public static bool CircleTest(Vector2 center, float radius, Point cursorLocation)
        {
            float d = (cursorLocation.X - center.X)*(cursorLocation.X - center.X) +
                      (cursorLocation.Y - center.Y)*(cursorLocation.Y - center.Y);
            if (d <= radius*radius)
                return true;
            else
                return false;
        }




        public static bool RayPlaneTest(Ray ray, Plane plane, out Vector3 intersectionPoint)
        {
            bool result;
            intersectionPoint = new Vector3();
            Vector3 rD = ray.Direction;
            Vector3 r0 = ray.Position;

            ray.Direction.Normalize();

            float vD = Vector3.Dot(plane.Normal, rD);
            if (vD == 0)
            {

                return false;
            }
            float v0 = -(Vector3.Dot(plane.Normal, r0) + plane.D);
            float t = v0 / vD;

            if (t > 0)
            {
                result = true;
                //intersectionPoint = new Vector3(
                //    r0.X + rD.X * t,
                //    r0.Y + rD.X * t,
                //    r0.Z + rD.Z * t);
                intersectionPoint = r0 + t * rD;
            }
            else
            {
                result = false;
            }

            return result;
            

        }
    }
}