using System;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public interface IPositionVertex
    {
        /// <summary>
        /// Gets or sets the position of the vertex.
        /// </summary>
        Vector4 Position { get; set; }
    }

    public interface IMeshVertex : IPositionVertex
    {
        Vector3 Normal { get; set; }
        Vector2 TextureUV { get; set; }
    }
}