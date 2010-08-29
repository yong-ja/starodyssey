using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public delegate Color4[] Shader(Color4 color, int numVertex, float startValue, float endValue, Shape shape);
    public struct ColorShader :IEquatable<ColorShader>
    {
        public Shader Method { get; set; }
        public float StartValue { get; set; }
        public float EndValue { get; set; }

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

        public static Color4[] LinearVerticalGradient(Color4 color, int numVertex, float startValue, float endValue, Shape shape)
        {
            Color4 lightColor = startValue == 1.0 ? color : Color4.Scale(color, startValue);
            Color4 darkColor = Color4.Scale(color, endValue);

            switch (shape)
            {
                default:
                case Shape.Rectangle:
                   if (numVertex != 4)
                       throw Error.ArgumentInvalid("numVertex", typeof(ColorShader), "LinearVerticalGradient",
                            Properties.Resources.ERR_InvalidNumVertices, numVertex.ToString());
                    return new[] { lightColor, lightColor, darkColor, darkColor};

            }

        }

        public static Color4[] Uniform(Color4 color, int numVertex, float startValue=0.0f, float endValue=0.0f, Shape shape=Shape.None)
        {
            Color4[] colors = new Color4[numVertex];
            for (int i = 0; i < numVertex; i++)
                colors[i] = color;
            return colors;
        }

        #region Equality
        public static bool operator ==(ColorShader left, ColorShader right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ColorShader left, ColorShader right)
        {
            return !(left == right);
        }

        public bool Equals(ColorShader other)
        {
            return Equals(other.Method, Method) && other.StartValue.Equals(StartValue) && other.EndValue.Equals(EndValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(ColorShader)) return false;
            return Equals((ColorShader)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Method != null ? Method.GetHashCode() : 0);
                result = (result * 397) ^ StartValue.GetHashCode();
                result = (result * 397) ^ EndValue.GetHashCode();
                return result;
            }
        } 
        #endregion
    }
}
