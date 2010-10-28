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
        public void DrawEllipse()
        {
            CheckParameters(Options.Size | Options.Shader);
            int slices=32;
            int segments=1;
            float[] offsets = Shader.Gradient.Select(g => g.Offset).ToArray();

            Color4[] colors = Shader.Method(Shader, slices +1, Shape.Rectangle);

            short[] indices;
            ColoredVertex[] vertices = Polygon.CreateEllipseMesh
                    (Position.ToVector4(),
                     Width,
                     Height,
                     slices,
                     segments,
                     colors,
                     out indices);

            ShapeDescription rectangleShape = new ShapeDescription
            {
                Vertices = vertices,
                Indices = indices,
                Primitives = indices.Length / 3,
                Shape = Shape.RectangleMesh
            };

            shapes.Add(rectangleShape);
        }
    }
}
