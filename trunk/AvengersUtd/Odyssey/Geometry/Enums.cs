using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    [Flags]
    public enum VertexFormat
    {
        Unknown = 0,
        Position = 1,
        Color = 2,
        TextureUV = 4,
        PositionTextureUV = Position & TextureUV,
    }
}
