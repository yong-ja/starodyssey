#region Using directives

using SlimDX.Direct3D11;
using SlimDX.DXGI;

#endregion

namespace AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph
{
    public struct RenderableCollectionDescription
    {
        public PrimitiveTopology PrimitiveTopology { get; set; }
        public bool CommonTexture { get; set; }
        public TranslucencyType TranslucencyType { get; set; }
        public InputElement[] InputElements { get; set; }
        public Format IndexFormat { get; set; }
    }

    public class RenderableCollection : SceneNodeCollection<RenderableNode>
    {
        public RenderableCollection(RenderableCollectionDescription rDescription)
        {
            Description = rDescription;
        }

        public RenderableCollectionDescription Description { get; set; }
    }
}