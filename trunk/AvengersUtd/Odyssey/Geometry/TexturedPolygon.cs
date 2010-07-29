using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using SlimDX.Direct3D11;
using Buffer = SlimDX.Direct3D11.Buffer;

namespace AvengersUtd.Odyssey.Geometry
{
    public class TexturedPolygon : Polygon, IDiffuseMap
    {
        public TexturedPolygon(Buffer vertices, Buffer indices, int vertexCount) : base(vertices, indices, vertexCount, TexturedVertex.Description)
        {
        }

        public Texture2D DiffuseMap { get; set; }
    }
}
