using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Effects;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public interface ICastsShadows
    {
        Texture ShadowMap { get; set; }
    }

    public interface ITexturedMaterial
    {
        void LoadTextures(MaterialDescriptor materialDescriptor);
    }
}