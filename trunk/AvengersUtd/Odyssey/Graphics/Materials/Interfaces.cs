using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public interface IColor4Material
    {
        Color4 AmbientColor4 { get;  }
        Color4 DiffuseColor { get; }
        Color4 SpecularColor4 { get; }

    }

    public interface IDiffuseMap
    {
        string DiffuseMapKey { get; set; }
        Texture2D DiffuseMapTexture2D { get; }
        ShaderResourceView DiffuseMapResource { get; }
    }
}
