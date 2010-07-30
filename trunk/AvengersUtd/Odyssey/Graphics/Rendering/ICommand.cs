using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public interface ICommand
    {
        CommandType CommandType { get; }
        void Execute();
    }

}