

using AvengersUtd.Odyssey.Graphics.Effects;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;

namespace AvengersUtd.Odyssey.Graphics.Materials
{
    public interface IMaterial
    {
        MaterialNode OwningNode { get; }
        EffectDescriptor EffectDescriptor { get; }
        void ApplyDynamicParameters();
        
    }
}