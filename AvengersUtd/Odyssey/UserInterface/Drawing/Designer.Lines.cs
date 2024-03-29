﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using PolyMesh = AvengersUtd.Odyssey.Graphics.Meshes.PolyMesh;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public partial class Designer
    {

        public void DrawClosedPath()
        {
            CheckParameters(Options.Shader);

            Color4[] colors = Shader.Method(Shader, Points.Length, Shape.Rectangle);
            ushort[] indices;
            ColoredVertex[] vertices = PolyMesh.DrawPolyLine((int)Width, colors, true, Points, out indices);

            ShapeDescription pathShape = new ShapeDescription
            {
                Vertices = vertices,
                Indices = indices,
                Primitives = indices.Length / 3,
                Shape = Shape.RectangleMesh
            };

            shapes.Add(pathShape);
        }

        
    }
}
