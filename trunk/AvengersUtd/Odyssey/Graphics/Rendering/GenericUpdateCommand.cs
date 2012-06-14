using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using AvengersUtd.Odyssey.Utils.Logging;
using AvengersUtd.Odyssey.UserInterface;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class GenericUpdateCommand:BaseCommand,IUpdateCommand
    {
        private readonly Thread genericUpdateThread;
        private readonly Queue<UpdateTask> updateTasks;
        private readonly SortedDictionary<TaskType, RecurrentTask> recurrentTasks;


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
            try
            {
                while (Game.IsRunning)
                {
                    if (updateTasks.Count > 0)
                        EventHandle.Reset();
                    else EventHandle.Wait();

                    while (updateTasks.Count > 0)
                    {
                        UpdateTask task = updateTasks.Dequeue();
                        task.Invoke();
                    }
                    EventHandle.Set();
                }
            }
            catch(Exception ex)
            {
                CriticalEvent.Unhandled.LogError(new TraceData(this.GetType(), MethodBase.GetCurrentMethod()), ex);
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
            WarningEvent.ThreadAborted.Log(GetType().Name);
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
