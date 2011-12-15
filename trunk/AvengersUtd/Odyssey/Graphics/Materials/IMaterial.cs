

using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using System;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public interface IMaterial
    {
        EffectDescription EffectDescription { get; }
        void ApplyDynamicParameters(Renderer rendererContext);
        MaterialNode ParentNode { get; }
        void SetParentNode(MaterialNode mNode);

        bool RequirePreRenderStateChange { get; }
        bool RequirePostRenderStateChange { get; }
        string TechniqueName { get; }
        ICommand[] PreRenderStates {get;}
        ICommand[] PostRenderStates {get;}
        RenderableCollectionDescription RenderableCollectionDescription { get; }

        void InitParameters(Renderer renderer);
        void ApplyInstanceParameters(IRenderable renderableObject);
    }
}