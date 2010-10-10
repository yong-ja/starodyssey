using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public struct GradientStop
    {
        public Color4 Color { get; set; }
        public float Offset { get; set; }

        public GradientStop(Color4 color, float offset) : this()
        {
            Color = color;
            Offset = offset;
        }
    }

    public delegate Color4[] Shader(ColorShader shader, int numVertex, Shape shape);
    
    public class ColorShader
    {
        public Shader Method { get; set; }
        public Color4 Color { get; set; }
        public int WidthSegments { get; set; }
        public int HeightSegments{ get; set; }
        public GradientStop[] Gradient { get; set; }

        public ColorShader()
        {
            WidthSegments = 1;
            HeightSegments = 1;
            Method = Uniform;
        }

        public static Color4[] LinearHorizontalGradient(Color4 color, int numVertex, float startValue, float endValue, Shape shape)
        {
            Color4 lightColor = startValue == 1.0 ? color : Color4.Scale(color, startValue);
            Color4 darkColor = Color4.Scale(color, endValue);
            Color4[] colors;
            switch (shape)
            {
                default:
                case Shape.Rectangle:
                    if (numVertex != 4)
                        throw Error.ArgumentInvalid("numVertex", typeof (ColorShader), "LinearHorizontalGradient",
                            Properties.Resources.ERR_InvalidNumVertices, numVertex.ToString());

                    colors= new[] {lightColor, darkColor, darkColor, lightColor};
                    break;

            }
            return colors;
        }

        public static Color4[] BorderSunken(Color4 color, int numVertex, float startValue = 0.5f, float endValue = 1.0f, Shape shape = Shape.Rectangle)
        {
            Color4 lightColor = Color4.Scale(color, startValue);
            Color4 darkColor = Color4.Scale(color, endValue);

            Color4[] colors;
            switch (shape)
            {
                default:
                case Shape.Rectangle:
                    if (numVertex != 4)
                        throw Error.ArgumentInvalid("numVertex", typeof(ColorShader), "BorderSunken",
                            Properties.Resources.ERR_InvalidNumVertices, numVertex.ToString());

                    colors = new[] {lightColor, lightColor, darkColor, darkColor};
                    break;

            }
            return colors;

            
        }

        public static Color4[] BorderRaised(Color4 color, int numVertex, float startValue=1.0f, float endValue=0.5f, Shape shape = Shape.Rectangle)
        {
            Color4 lightColor = Color4.Scale(color, startValue);
            Color4 darkColor = Color4.Scale(color, endValue);

            Color4[] colors;
            switch (shape)
            {
                default:
                case Shape.Rectangle:
                    if (numVertex != 4)
                        throw Error.ArgumentInvalid("numVertex", typeof(ColorShader), "BorderRaised",
                            Properties.Resources.ERR_InvalidNumVertices, numVertex.ToString());

                    colors = new[] { lightColor, lightColor, darkColor, darkColor };
                    break;

            }
            return colors;
        }

        public static Color4[] LinearVerticalGradient(ColorShader shader, int numVertex, Shape shape)
        {

            //int numVertex = (1 + widthSegments)*(1 + heightSegments);
            Color4[] colors = new Color4[numVertex];
            switch (shape)
            {
                default:
                case Shape.SubdividedRectangle:
                    if (numVertex != (1+ shader.WidthSegments) * (1+shader.HeightSegments))
                        throw Error.ArgumentInvalid("numVertex", typeof(ColorShader), "LinearVerticalStepGradient",
                             Properties.Resources.ERR_InvalidNumVertices, numVertex.ToString());

                    int step = 0;
                    for (int i = 0; i < numVertex; i++)
                    {

                        if (i > 0 && i % (shader.WidthSegments + 1) == 0)
                            step++;
                        colors[i] = shader.Gradient[step].Color;
                    }

                    //colors[0] = new Color4(1, 0,0);
                    //colors[numVertex - 1] = new Color4(0, 1, 0);

                    return colors;
            }
        }

        public static Color4[] Uniform(ColorShader shader, int numVertex, Shape shape=Shape.None)
        {
            return FillColorArray(shader.Color,4);
        }

        public static Color4[] FillColorArray(Color4 color, int numVertex)
        {
            Color4[] colors = new Color4[numVertex];
            for (int i = 0; i < numVertex; i++)
                colors[i] = color;
            return colors;
        }

    }
}
