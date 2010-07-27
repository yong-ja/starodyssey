using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public interface IRenderCommand
    {
        SceneNodeCollection Items { get; }
        void Execute();
        void PerformRender();
    }
}