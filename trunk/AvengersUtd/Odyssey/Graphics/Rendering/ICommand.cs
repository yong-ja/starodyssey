using System.Collections.Generic;
using System.Threading;
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
        ManualResetEventSlim EventHandle { get; }

        void StartThread();
        void Activate();
        void TerminateThread();
        void Resume();
    }

    public interface IRenderCommand : ICommand
    {
        void Init();
        void PerformRender();
        void UpdateItems();
    }

}