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
        private readonly List<System.Drawing.PointF> points;

        public List<PointF> Points
        {
            get { return points; }
        }

        public Polygon(IEnumerable<PointF> points):this()
        {
            this.points.AddRange(points);
        }

        private Polygon()
        {
            this.points = new List<PointF>();
        }

        public Vector4[] ComputeVector4Array(float zIndex)
        {
            Vector4[] pointsArray = new Vector4[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                PointF point = points[i];
                pointsArray[i] = new Vector4(point.X, point.Y, zIndex, 1.0f);
            }
            return pointsArray;
        }

        public static Polygon CreateEllipse(PointF center, float radiusX, float radiusY, int slices)
        {
            float x = center.X;
            float y = center.Y;
            float delta = MathHelper.TwoPi / slices;
            PointF[] points = new PointF[slices];
            for (int i = 0; i < slices; i++)
            {
                float theta = i * delta;
                points[i] = new PointF
                    (x + (float) Math.Cos(theta)*radiusX,
                    y - (float) Math.Sin(theta)*radiusY);
            }
            
            return new Polygon(points);
        }
    }
}
