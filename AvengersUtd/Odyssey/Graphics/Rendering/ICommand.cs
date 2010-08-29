using System.Collections.Generic;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public delegate void UpdateTask();

    public interface ICommand
    {
        CommandType CommandType { get; }
        void Execute();
        void Dispose();
    }

    public interface IUpdateCommand: ICommand
    {
        bool IsThreaded { get; }

        void StartThread();
        void Activate();
        void TerminateThread();
        void Resume();
    }

    public interface IRenderCommand : ICommand
    {
        void PerformRender();
        void UpdateItems();
    }

}