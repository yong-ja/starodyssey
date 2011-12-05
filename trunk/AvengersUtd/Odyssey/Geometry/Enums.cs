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
        Color4 = 2,
        TextureUV = 4,
        TextureUVW = 8,
        Normal = 16,
        Tangent = 32,
        BiNormal = 64,
        PositionTextureUV = Position | TextureUV,
        PositionColor4 = Position | Color4,
        PositionNormal = Position | Normal,
        TexturedMesh = Position | TextureUV | Normal | Tangent | BiNormal,
        PositionTextureUVW = Position | TextureUVW,
        Mesh = Position | Normal | TextureUV
    }
}
