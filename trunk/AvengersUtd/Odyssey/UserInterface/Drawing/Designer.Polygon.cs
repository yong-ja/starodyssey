using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public partial class Designer
    {
        public void DrawPolygon(ushort[] indices)
        {
            Color4[] colors = Shader.Method(Shader, Points.Length, Shape.Rectangle);
            ColoredVertex[] vertices = new ColoredVertex[Points.Length];

            for (int i = 0; i < Points.Length; i++)
            {
                Vector4 vertex = Points[i];
                vertices[i] = new ColoredVertex(vertex, colors[i]);
            }

            ShapeDescription polygonShape = new ShapeDescription
            {
                Vertices = vertices,
                Indices = indices,
                Primitives = indices.Length / 3,
                Shape = Shape.RectangleMesh
            };

            shapes.Add(polygonShape);
        }
    }
}
