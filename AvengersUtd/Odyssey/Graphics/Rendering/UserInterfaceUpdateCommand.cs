using System;
using System.Collections.Generic;
using System.Threading;
using AvengersUtd.Odyssey.UserInterface.Controls;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{

    public class UserInterfaceUpdateCommand : BaseCommand, IUpdateCommand
    {
        private readonly Thread uiUpdateThread;
        private readonly ManualResetEventSlim uiUpdateEvent;
        private readonly Hud hud;
        private readonly IRenderCommand uiRCommand;

        public Queue<UpdateTask> TaskQueue { get; private set; }

        public bool IsThreaded { get { return true; } }

        public UserInterfaceUpdateCommand(Hud hud, IRenderCommand uiRCommand) : base(CommandType.Update)
        {
            this.hud = hud;
            uiUpdateThread = new Thread(Activate) {Name = "UI Update Thread", Priority = ThreadPriority.Lowest};
            uiUpdateEvent = new ManualResetEventSlim(false);
            this.uiRCommand = uiRCommand;
            TaskQueue = new Queue<UpdateTask>();
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
                if (hud.ShouldUpdateShapes)
                    uiUpdateEvent.Reset();
                else continue;

                hud.Update();
                uiUpdateEvent.Set();
            }
        }

        public override void Execute()
        {
            if (TaskQueue.Count == 0)
                return;

            uiUpdateEvent.Wait();
            
            while (TaskQueue.Count != 0)
            {
                UpdateTask task = TaskQueue.Dequeue();
                task.Invoke();
            }
        }

        public void Resume()
        {
            uiUpdateEvent.Set();
        }

        public void Synch()
        {
            uiRCommand.UpdateItems();
        }
    
        protected override void OnDispose()
        {
            uiUpdateThread.Abort();
            uiUpdateEvent.Dispose();
        }
    }
}
