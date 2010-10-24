﻿using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
	public static partial class ShapeCreator
	{
        public static ShapeDescription DrawEquilateralTriangle(Vector3 leftVertex, float sideLength, Color4[] Color4s,
                                                               bool isTriangleUpside)
        {
            short[] indices;
            ColoredVertex[] vertices = Polygon.CreateEquilateralTriangle(leftVertex.ToVector4(), sideLength, Color4s,
                isTriangleUpside, out indices);
            return new ShapeDescription
                       {
                           Vertices = vertices,
                           Indices = indices,
                           Primitives = 1,
                           Shape = Shape.Triangle
                       };

        }
	}
}