using System.Linq;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public partial class Designer
    {
        public void DrawRectangle()
        {
            CheckParameters(Options.BorderSize | Options.Shader);
           
            float actualWidth = Width;
            float actualHeight = Height;
            Vector3 actualPosition = Position;
            float leftSegmentOffset = BorderSize.Left / Width;
            float rightSegmentOffset = (Width - BorderSize.Right) / Width;
            float topSegmentOffset = BorderSize.Top / Height;
            float bottomSegmentOffset = (Height - BorderSize.Bottom) / Height;

            SaveState();
            if (BorderSize.Top > 0)
            {
                GradientStop[] gradient = null;
                
                switch (Shader.GradientType)
                {
                    case GradientType.LinearVerticalGradient:
                        gradient = LinearShader.SplitGradient(Shader.Gradient, 0, topSegmentOffset);
                        break;
                    case GradientType.Uniform:
                    case GradientType.LinearHorizontalGradient:
                        gradient = Shader.Gradient;
                        break;
                    default:
                        throw Error.WrongCase("Shader.GradientType", "DrawRectangle",
                        Shader.GradientType);
                }
                LinearShader topShader = new LinearShader
                {
                    Gradient = gradient,
                    GradientType = Shader.GradientType,
                    Method = Shader.Method
                };

                Width = actualWidth;
                Height = BorderSize.Top;
                Shader = topShader;
                FillRectangle();
            }
            if (BorderSize.Left > 0)
            {
                GradientStop[] gradient = null;
                
                switch (Shader.GradientType)
                {
                    case GradientType.Uniform:
                        gradient = Shader.Gradient;
                        break;  
                    case GradientType.LinearVerticalGradient:
                        gradient = LinearShader.SplitGradient(Shader.Gradient, topSegmentOffset, bottomSegmentOffset);
                        break;
                    case GradientType.LinearHorizontalGradient:
                        gradient = LinearShader.SplitGradient(Shader.Gradient, 0, leftSegmentOffset);
                        break;
                    default:
                        throw Error.WrongCase("Shader.GradientType", "DrawSubdividedRectangleWithOutline",
                        Shader.GradientType);
                }
                LinearShader leftShader = new LinearShader
                {
                    Gradient = gradient,
                    GradientType = Shader.GradientType,
                    Method = Shader.Method
                };

                Width = BorderSize.Left;
                Height = actualHeight - BorderSize.Vertical;
                Position = new Vector3(actualPosition.X, actualPosition.Y - BorderSize.Top, actualPosition.Z);
                Shader = leftShader;
                FillRectangle();
            }
            if (BorderSize.Bottom > 0)
            {
                GradientStop[] gradient = null;
                switch (Shader.GradientType)
                {
 
                    case GradientType.LinearVerticalGradient:
                        gradient = LinearShader.SplitGradient(Shader.Gradient, bottomSegmentOffset, 1);
                        break;
                    case GradientType.Uniform:
                    case GradientType.LinearHorizontalGradient:
                        gradient = Shader.Gradient;
                        break;
                    default:
                        throw Error.WrongCase
                                ("Shader.GradientType",
                                 "DrawSubdividedRectangleWithOutline",
                                 Shader.GradientType);
                }
                LinearShader bottomShader = new LinearShader
                {
                    Gradient = gradient,
                    GradientType = Shader.GradientType,
                    Method = Shader.Method
                };

                Width = actualWidth;
                Height = BorderSize.Bottom;
                Position = new Vector3(actualPosition.X, actualPosition.Y - actualHeight + BorderSize.Bottom, actualPosition.Z);
                Shader = bottomShader;
                FillRectangle();
            }
            if (BorderSize.Right > 0)
            {
                GradientStop[] gradient = null;
               switch (Shader.GradientType)
                {
                    case GradientType.Uniform:
                        gradient = Shader.Gradient;
                        break;

                    case GradientType.LinearVerticalGradient:
                        gradient = LinearShader.SplitGradient(Shader.Gradient, topSegmentOffset, bottomSegmentOffset);
                        break;
                    case GradientType.LinearHorizontalGradient:
                        gradient = LinearShader.SplitGradient(Shader.Gradient, rightSegmentOffset, 1);
                        break;
                    default:
                        throw Error.WrongCase("Shader.GradientType", "DrawSubdividedRectangleWithOutline",
                        Shader.GradientType);
                }
                LinearShader rightShader = new LinearShader
                {
                    Gradient = gradient,
                    GradientType = Shader.GradientType,
                    Method = Shader.Method
                };

                Width = BorderSize.Right;
                Height = actualHeight - BorderSize.Vertical;
                Position = new Vector3(actualPosition.X + actualWidth - BorderSize.Right, actualPosition.Y - BorderSize.Top, actualPosition.Z);
                Shader = rightShader;
                FillRectangle();
            }
            RestoreState();
        }

        public void FillRectangle()
        {
            CheckParameters(Options.Size | Options.Shader);
            int widthSegments;
            int heightSegments;
            float[] offsets = Shader.GradientType != GradientType.Uniform
                                      ? Shader.Gradient.Select(g => g.Offset).ToArray()
                                      : null;
            float[] widthOffsets = null;
            float[] heightOffsets = null;

            switch (Shader.GradientType)
            {
                case GradientType.Uniform:
                    widthSegments = heightSegments = 1;
                    break;
                case GradientType.LinearVerticalGradient:
                    widthSegments = 1;
                    heightSegments = offsets.Length - 1;
                    heightOffsets = offsets;
                    break;
                case GradientType.LinearHorizontalGradient:
                    widthSegments = offsets.Length - 1;
                    heightSegments = 1;
                    widthOffsets = offsets;
                    break;
                case GradientType.Radial:
                    widthSegments = offsets.Length;
                    heightSegments = widthSegments;
                    break;

                default:
                    throw Error.WrongCase("colorshader.GradientType", "DrawSubdividedRectangleWithOutline",
                        Shader.GradientType);
            }
            Color4[] colors = Shader.Method(Shader, (1 + widthSegments) * (1 + heightSegments), Shape.Rectangle);

            short[] indices;
            ColoredVertex[] vertices = PolyMesh.CreateRectangleMesh
                    (Position.ToVector4(),
                     Width,
                     Height,
                     widthSegments,
                     heightSegments,
                     colors,
                     out indices,
                     widthOffsets,
                     heightOffsets);

            ShapeDescription rectangleShape = new ShapeDescription
            {
                Vertices = vertices,
                Indices = indices,
                Primitives = indices.Length/3,
                Shape = Shape.RectangleMesh
            };

            shapes.Add(rectangleShape);
        }
    }
}
