using System;
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Controls;

namespace AvengersUtd.Odyssey
{
    
    public struct RecurrentTask:IComparable<RecurrentTask>
    {
        public int Priority { get; private set; }
        public TaskType TaskType { get; private set; }
        public UpdateTask Task { get; private set; }

        public RecurrentTask(TaskType taskType, UpdateTask task, int priority)
            : this()
        {
            Priority = priority;
            TaskType = taskType;
            Task = task;
        }

        public int CompareTo(RecurrentTask other)
        {
            return Priority.CompareTo(other.Priority);
        }
    }

}
