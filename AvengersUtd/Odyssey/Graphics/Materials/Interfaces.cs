using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public interface IColorMaterial
    {
        Color4 AmbientColor { get;  }
        Color4 DiffuseColor { get; }
        Color4 SpecularColor { get; }

    }

    public interface IDiffuseMap
    {
        Texture2D DiffuseMap { get; }
    }
}
