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
        public Vector2 GradientOrigin { get; set; }
        public float RadiusX { get; set; }
        public float RadiusY { get; set; }

        internal int CenterXIndex { get; private set; }

        public RadialShader()
        {
            Center = GradientOrigin = new Vector2(0.5f, 0.5f);
            RadiusX = RadiusY = 0.5f;
        }

        float[] BuildWidthOffsets()
        {
            List<float> offsets = new List<float>();
            float center = Center.X;

            offsets.Add(center);
            foreach (GradientStop t in Gradient)
            {
                float offset = (t.Offset * RadiusX);
                float leftOffset = center - offset;
                float rightOffset = center + offset;
                
                offsets.AddRange(new[] {leftOffset, rightOffset});
            }

            offsets.Sort();
            CenterXIndex = offsets.IndexOf(center);
            return offsets.ToArray();
        }

        public static Color4[] Radial(IGradientShader shader, int numVertex, Shape shape)
        {
            RadialShader rs = (RadialShader)shader;
            Color4[] colors = new Color4[numVertex];
            switch (shape)
            {
                default:
<<<<<<< .mine
                //case Shape.RectangleMesh:
                    colors[0] = shader.Gradient[0].Color;
                    for (int i = 1; i < colors.Length; i++ )
                    {
                        colors[i] = shader.Gradient[1].Color;
                    }
                        break;
=======
                case Shape.RectangleMesh:
                    // A radial gradient needs  rectangle mesh composed by n*n segments.
                    // The total vertex count
                    for (int i = 0; i < colors.Length; i++)
                    {
                        colors[i] = rs.Gradient[rs.Gradient.Length - 1].Color;
                    }
                    colors[4] = rs.Gradient[0].Color;
                    break;
>>>>>>> .r185
            }
            return colors;
        }


        
    }
}
