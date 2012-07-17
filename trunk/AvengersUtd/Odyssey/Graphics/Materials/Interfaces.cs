using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public interface IColorMaterial : IMaterial
    {
        Color4 AmbientColor { get;  }
        Color4 DiffuseColor { get; set; }
        Color4 SpecularColor { get; }
        float AmbientCoefficient { get; }
        float SpecularCoefficient { get; }
        float DiffuseCoefficient { get; }

    }

    public interface IDiffuseMap
    {
        string DiffuseMapKey { get; set; }
        Texture2D DiffuseMapTexture2D { get; }
        ShaderResourceView DiffuseMapResource { get; }
    }
}
