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
        internal int Slices { get; private set; }

        public RadialShader()
        {
            Center = GradientOrigin = new Vector2(0.5f, 0.5f);
            RadiusX = RadiusY = 0.5f;
            Slices = 32;
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


        
    }
}
