using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public class RadialShader : LinearShader, IRadialShader
    {
        public Vector2 Center { get; set; }
        public float RadiusX { get; set; }
        public float RadiusY { get; set; }

        internal int CenterXIndex { get; private set; }
        internal int Slices { get; private set; }

        public RadialShader()
        {
            Center = new Vector2(0.5f, 0.5f);
            RadiusX = RadiusY = 0.5f;
            Slices =32;
        }

        public static Color4[] Radial(IGradientShader shader, int numVertex, Shape shape)
        {
            RadialShader rs = (RadialShader)shader;
            Color4[] colors = new Color4[numVertex];
            switch (shape)
            {
                default:
                //case Shape.RectangleMesh:
                    colors[0] = shader.Gradient[0].Color;
                    int k = 1;
                    for (int i = 1; i < colors.Length; i++ )
                    {
                        
                        if (i > 1 && (i - 1) % rs.Slices == 0)
                            k++;
                        colors[i] = shader.Gradient[k].Color;
                    }
                    break;

            }
            return colors;
        }

        public static Color4[] RadialManual(GradientStop[] gradient, int numVertex, int offsetIndex)
        {
            Color4[] colors = new Color4[numVertex];
            for (int i = 0; i < numVertex; i++)
                colors[i] = gradient[offsetIndex].Color;

            return colors;
        }

        public static Color4[] RadialManual2(GradientStop[] gradient, int numVertex, IEnumerable<Vector2D> points, Ellipse ellipse)
        {
            Color4[] colors = new Color4[numVertex];
            int i = 0;
            foreach (Vector2D v in points)
            {
                
                Vector2D v1 = Vector2D.Modulate(v - ellipse.Center, new Vector2D(1/ellipse.RadiusX, 1/ellipse.RadiusY));
                //Vector2D v1 = v - ellipse.Center;
                //v1.Normalize();
                //Vector2D v1 = new Vector2D(v.X, v.Y);
                //v1.Normalize();
                double d = v1.LengthSquared();
                //double d = v1.Length();
                int index = FindGradientStops(gradient, (float)d);
                float offset = (float)MathHelper.ConvertRange
                    (gradient[index].Offset, gradient[index+1].Offset, 0, 1, Math.Sqrt(d));
                colors[i] = Color4.Lerp(gradient[index].Color, gradient[index+1].Color, offset);
                i++;
            }

            colors[0] = gradient[0].Color;

            return colors;
        }

        //public static Color4[] RadialManual3(GradientStop[] gradient, Ellipse[] ellipses, IEnumerable<Vector2D>points, int numVertex)
        //{
        //     Color4[] colors = new Color4[numVertex];
        //    int i = 0;
        //    foreach (Vector2D v in points)
        //    {
        //    }
        //}

        //static int FindEllipse(Vector2D v, Ellipse ellipse)
        //{
        //    Vector2D v1 = v - ellipse.Center;
        //    double dSqr = v1.LengthSquared();
        //}

        static int FindGradientStops(GradientStop[] gradient, float offsetSquared)
        {
            if (Math.Abs(offsetSquared - 0f) < 0.05)
                return 0;
            if (Math.Abs(offsetSquared - 1f) < 0.05)
                return gradient.Length - 2;
            for (int i = 1; i < gradient.Length-1; i++)
            {
                if (offsetSquared <= gradient[i].Offset * gradient[i].Offset)
                    return i;
            }
            return gradient.Length - 2;
        }

        public Ellipse CreateEllipse(OrthoRectangle rectangle)
        {
            Vector2D radialCenter = new Vector2D(rectangle.TopLeft.X + Center.X * rectangle.Width,
                                                 rectangle.TopLeft.Y - Center.Y * rectangle.Height);
            return new Ellipse(radialCenter, RadiusX * rectangle.Width, RadiusY * rectangle.Height);
        }


        
    }
}
