using System;
using System.Collections.Generic;
using System.Linq;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using AvengersUtd.Odyssey.Utils.Xml;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public class GradientStop : IEquatable<GradientStop>
    {
        public Color4 Color { get; set; }
        public float Offset { get; set; }

        public GradientStop(Color4 color, float offset) : this()
        {
            Color = color;
            Offset = offset;
        }

        public GradientStop()
        {
            Offset = -1;
        }

        #region Equality

        public bool Equals(GradientStop other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Color.Equals(Color) && other.Offset.Equals(Offset);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (GradientStop)) return false;
            return Equals((GradientStop) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Color.GetHashCode()*397) ^ Offset.GetHashCode();
            }
        }

        public static bool operator ==(GradientStop left, GradientStop right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GradientStop left, GradientStop right)
        {
            return !Equals(left, right);
        }

        #endregion
    }

    public delegate Color4[] Shader(IGradientShader shader, int numVertex, Shape shape);
    
    public class LinearShader : IGradientShader
    {
        public string Name { get; set; }
        public GradientType GradientType { get; set; }
        public Shader Method { get; set; }
        public GradientStop[] Gradient { get; set; }

        public LinearShader()
        {
            Method = Uniform;
        }

        public static LinearShader CreateUniform(Color4 color)
        {
            return new LinearShader
                   {
                       Name = "DefaultUniform",
                       Gradient = new[]
                                  {
                                      new GradientStop(color, 0), new GradientStop(color, 1.0f)
                                  },
                   };
        }

        public static Color4[] LinearHorizontalGradient(IGradientShader shader, int numVertex, Shape shape)
        {
            const int heightVertices = 2;
            Color4[] colors = new Color4[numVertex];
            switch (shape)
            {
                default:
                case Shape.RectangleMesh:
                    int step = 0;
                    for (int i = 0; i < numVertex; i++)
                    {
                        colors[i] = shader.Gradient[step].Color;
                        step++;
                        if (i > 0 && i % shader.Gradient.Length-1 == 0)
                            step=0;
                    }

                    return colors;
            }
        }

        #region deprecated
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
                        throw Error.ArgumentInvalid("numVertex", typeof(LinearShader), "BorderSunken",
                            Properties.Resources.ERR_InvalidNumVertices, numVertex.ToString());

                    colors = new[] { lightColor, lightColor, darkColor, darkColor };
                    break;

            }
            return colors;
        }

        public static Color4[] BorderRaised(Color4 color, int numVertex, float startValue = 1.0f, float endValue = 0.5f, Shape shape = Shape.Rectangle)
        {
            Color4 lightColor = Color4.Scale(color, startValue);
            Color4 darkColor = Color4.Scale(color, endValue);

            Color4[] colors;
            switch (shape)
            {
                default:
                case Shape.Rectangle:
                    if (numVertex != 4)
                        throw Error.ArgumentInvalid("numVertex", typeof(LinearShader), "BorderRaised",
                            Properties.Resources.ERR_InvalidNumVertices, numVertex.ToString());

                    colors = new[] { lightColor, lightColor, darkColor, darkColor };
                    break;

            }
            return colors;
        } 
        #endregion

        public static Color4[] LinearVerticalGradient(IGradientShader shader, int numVertex, Shape shape)
        {
            const int widthVertices = 2;
            Color4[] colors = new Color4[numVertex];
            switch (shape)
            {
                default:
                case Shape.RectangleMesh:
                    int step = 0;
                    for (int i = 0; i < numVertex; i++)
                    {
                        if (i > 0 && i % widthVertices == 0)
                            step++;
                        colors[i] = shader.Gradient[step].Color;
                    }

                    //colors[0] = new Color4(1, 0,0);
                    //colors[numVertex - 1] = new Color4(0, 1, 0);

                    return colors;
            }
        }


        public static Color4[] Uniform(IGradientShader shader, int numVertex, Shape shape = Shape.None)
        {
            return FillColorArray(shader.Gradient[0].Color,numVertex);
        }

        public static Color4[] FillColorArray(Color4 color, int numVertex)
        {
            Color4[] colors = new Color4[numVertex];
            for (int i = 0; i < numVertex; i++)
                colors[i] = color;
            return colors;
        }

        internal static GradientStop[] SplitGradient(IEnumerable<GradientStop> gradient, float lowerBound, float upperBound)
        {
            IEnumerable<GradientStop> gradientOffsets = from g in gradient
                                                        where
                                                            g.Offset > lowerBound &&
                                                            g.Offset < upperBound
                                                        select g;
            List<GradientStop> gradientList = new List<GradientStop>();
            foreach (GradientStop gradientStop in gradientOffsets)
            {
                GradientStop currentStop = gradientStop;
                GradientStop prevStop = gradient.First(g => g.Offset < currentStop.Offset);
                GradientStop nextStop = gradient.First(g => g.Offset > currentStop.Offset);
                float scaledValue = MathHelper.Scale
                    (currentStop.Offset,
                     prevStop.Offset,
                     nextStop.Offset);
                GradientStop scaledStop = new GradientStop
                                          {
                                              Color = Color4.Lerp
                                                  (prevStop.Color,
                                                   nextStop.Color,
                                                   scaledValue),
                                              Offset = scaledValue
                                          };
                gradientList.Add(scaledStop);
            }

            GradientStop firstStop;
            GradientStop lastStop;
            GradientStop upperBoundStop;
            GradientStop lowerBoundStop;

            lowerBoundStop = gradient.FirstOrDefault(g => g.Offset == lowerBound);
            if (lowerBoundStop != null)
            {

                firstStop = lowerBoundStop;
                firstStop.Offset = 0;
            }
            else
            {
                lowerBoundStop = gradient.First(g => g.Offset < lowerBound);
                upperBoundStop = gradient.First(g => g.Offset > lowerBound);
                firstStop = new GradientStop
                    (
                    Color4.Lerp
                        (lowerBoundStop.Color,
                         upperBoundStop.Color,
                         MathHelper.Scale
                             (lowerBound,
                              lowerBoundStop.Offset,
                              upperBoundStop.Offset)),
                    0);
            }
            
            upperBoundStop = gradient.FirstOrDefault(g => g.Offset == upperBound);
            if (upperBoundStop != null)
            {
                lastStop = upperBoundStop;
                lastStop.Offset = 1;
            }
            else
            {
                lowerBoundStop = gradient.First(g => g.Offset < upperBound);
                upperBoundStop = gradient.First(g => g.Offset > upperBound);
                lastStop = new GradientStop
                    (
                    Color4.Lerp
                        (lowerBoundStop.Color,
                         upperBoundStop.Color,
                         MathHelper.Scale
                             (upperBound,
                              lowerBoundStop.Offset,
                              upperBoundStop.Offset)),
                    1);
            }

            gradientList.Insert(0, firstStop);
            gradientList.Add(lastStop);

            return gradientList.ToArray();
        }
    }
}
