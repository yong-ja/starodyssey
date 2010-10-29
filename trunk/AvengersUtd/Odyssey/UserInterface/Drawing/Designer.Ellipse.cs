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
            RadialShader rs = (RadialShader) Shader;
            
            float[] offsets = Shader.Gradient.Select(g => g.Offset).ToArray();
            int segments = offsets.Length;

            Color4[] colors = Shader.Method(Shader, (segments-1) * (rs.Slices) +1, Shape.Rectangle);

            short[] indices;
            ColoredVertex[] vertices = PolyMesh.CreateEllipseMesh
                    (Position.ToVector4(),
                     Width,
                     Height,
                     rs.Slices,
                     segments,
                     colors,
                     out indices,
                     offsets);

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
