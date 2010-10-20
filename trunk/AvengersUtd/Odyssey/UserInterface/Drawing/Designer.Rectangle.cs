using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public partial class Designer
    {
        public void DrawRectangle()
        {
            CheckParameters(Options.Size|Options.BorderSize | Options.BorderShader);
            Color4[] colors = BorderShader.Method(BorderShader, 4, Shape.Rectangle);
            short[] indices;
            ColoredVertex[] vertices = Polygon.CreateQuad
                    (Position.ToVector4(),
                     Width,
                     Height,
                     colors,
                     out indices);

            ShapeDescription rectangleShape = new ShapeDescription
            {
                Vertices = vertices,
                Indices = indices,
                Primitives = indices.Length / 3,
                Shape = Shape.Rectangle
            };

            shapes.Add(rectangleShape);
        }

        public void FillRectangle()
        {
            CheckParameters(Options.Size | Options.FillShader);
            int widthSegments;
            int heightSegments;
            float[] offsets = FillShader.Gradient.Select(g => g.Offset).ToArray();
            float[] widthOffsets = null;
            float[] heightOffsets = null;

            switch (FillShader.GradientType)
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
                default:
                    throw Error.WrongCase("colorshader.GradientType", "DrawSubdividedRectangleWithOutline",
                        FillShader.GradientType);
            }
            Color4[] colors = FillShader.Method(FillShader, (1 + widthSegments) * (1 + heightSegments), Shape.Rectangle);

            short[] indices;
            ColoredVertex[] vertices = Polygon.CreateRectangleMesh
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
                Shape = Shape.SubdividedRectangle
            };

            shapes.Add(rectangleShape);
        }
    }
}
