using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public partial class Polygon
    {
        private readonly List<Vector2> points;

        public List<Vector2> Points
        {
            get { return points; }
        }

        public Polygon(IEnumerable<Vector2> points):this()
        {
            this.points.AddRange(points);
        }

        private Polygon()
        {
            this.points = new List<Vector2>();
        }

        public Vector4[] ComputeVector4Array(float zIndex)
        {
            Vector4[] pointsArray = new Vector4[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                Vector2 point = points[i];
                pointsArray[i] = new Vector4(point.X, point.Y, zIndex, 1.0f);
            }
            return pointsArray;
        }

        public static Polygon CreateEllipse(Vector2 center, float radiusX, float radiusY, int slices)
        {
            float x = center.X;
            float y = center.Y;
            float delta = MathHelper.TwoPi / slices;
            Vector2[] points = new Vector2[slices];
            for (int i = 0; i < slices; i++)
            {
                float theta = i * delta;
                points[i] = new Vector2
                    (x + (float) Math.Cos(theta)*radiusX,
                    y - (float) Math.Sin(theta)*radiusY);
            }
            
            return new Polygon(points);
        }
    }
}
