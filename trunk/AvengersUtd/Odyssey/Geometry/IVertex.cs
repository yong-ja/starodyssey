using System;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public interface IVertex
    {
        /// <summary>
        /// Gets or sets the position of the vertex.
        /// </summary>
        Vector4 Position { get; set; }
    }
}