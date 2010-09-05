using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AvengersUtd.Odyssey.UserInterface;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class GenericUpdateCommand:BaseCommand,IUpdateCommand
    {
        private readonly Thread genericUpdateThread;
        private Queue<UpdateTask> updateTasks;
        private SortedDictionary<TaskType, RecurrentTask> recurrentTasks;


        internal GenericUpdateCommand() : base(CommandType.Update)
        {
            updateTasks = new Queue<UpdateTask>();
            recurrentTasks = new SortedDictionary<TaskType, RecurrentTask>();
            genericUpdateThread = new Thread(Activate) {Name = "GenericUpdater", Priority = ThreadPriority.Lowest};
            EventHandle = new ManualResetEventSlim();
        }

        #region IUpdateCommand
        public bool IsThreaded
        {
            get { return true; }
        }

        public ManualResetEventSlim EventHandle { get; private set; }
        
        public void StartThread()
        {
            genericUpdateThread.Start();
        }

        public void Activate()
        {
            while (Game.IsRunning)
            {
                if (updateTasks.Count > 0)
                    EventHandle.Reset();
                else continue;

                while (updateTasks.Count > 0)
                {
                    UpdateTask task = updateTasks.Dequeue();
                    task.Invoke();
                }
                EventHandle.Set();
            }
        }

        public void TerminateThread()
        {
            try
            {
                genericUpdateThread.Interrupt();
            }
            catch (ThreadInterruptedException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public void Resume()
        {
            EventHandle.Set();
        }
        
        public override void Execute()
        {
            foreach (KeyValuePair<TaskType, RecurrentTask> keyValuePair in recurrentTasks)
            {
                keyValuePair.Value.Task();
            }
        }

        protected override void OnDispose()
        {
            genericUpdateThread.Abort();
            EventHandle.Dispose();
        } 
        #endregion

        public void EnqueueRunOnceTask(UpdateTask task)
        {
            if (!updateTasks.Contains(task))
                updateTasks.Enqueue(task);
        }

        public void AddRecurrentTask(RecurrentTask task)
        {
            if (!recurrentTasks.ContainsKey(task.TaskType))
                recurrentTasks.Add(task.TaskType, task);
        }
    }
}
