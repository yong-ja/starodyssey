using System;
using System.Collections.Generic;
using System.Threading;
using AvengersUtd.Odyssey.UserInterface.Controls;
using System.Diagnostics.Contracts;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{

    public class UserInterfaceUpdateCommand : BaseCommand, IUpdateCommand
    {
        private readonly Thread uiUpdateThread;
        private readonly Hud hud;
        private readonly IRenderCommand uiRCommand;

        private Queue<UpdateTask> taskQueue;

        public bool IsThreaded { get { return true; } }
        public ManualResetEventSlim EventHandle { get; private set; }

        public UserInterfaceUpdateCommand(Hud hud, IRenderCommand uiRCommand) : base(CommandType.Update)
        {
            this.hud = hud;
            uiUpdateThread = new Thread(Activate) {Name = "UI Update Thread", Priority = ThreadPriority.Lowest};
            EventHandle = new ManualResetEventSlim(false);
            this.uiRCommand = uiRCommand;
            taskQueue = new Queue<UpdateTask>();
        }

        public void StartThread()
        {
            uiUpdateThread.Start();
        }

        public void TerminateThread()
        {
            try
            {
                uiUpdateThread.Interrupt();
            }
            catch (ThreadInterruptedException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Activate()
        {
            while (hud.IsEnabled)
            {
                EventHandle.Wait();
                hud.Update();
                EventHandle.Reset();
            }
        }

        public void EnqueueTask(UpdateTask task)
        {
            Contract.Requires(task != null);
            taskQueue.Enqueue(task);
        }

        public bool ContainsTask(UpdateTask task)
        {
            Contract.Requires(task != null);
            return taskQueue.Contains(task);
        }

        public override void Execute()
        {
            if (taskQueue.Count == 0)
                return;
           
            while (taskQueue.Count > 0)
            {
                UpdateTask task = taskQueue.Dequeue();
                task.Invoke();
            }
        }

        public void Resume()
        {
            EventHandle.Set();
        }

        protected override void OnDispose()
        {
            uiUpdateThread.Abort();
            EventHandle.Dispose();
        }
    }
}
