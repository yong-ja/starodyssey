using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class RenderManager
    {
        private LinkedList<BaseCommand> commandList;

        public BaseCommand[] Commands
        {
            get { return commandList.ToArray(); }
        }

        public RenderManager()
        {
            commandList = new LinkedList<BaseCommand>();
        }

        public bool IsStateActive(BaseCommand stateChangeCommmand)
        {
            try
            {
                BaseCommand activeState =
                    commandList.Last.FindFirstBackward(stateChangeCommmand).Value;
                return activeState == stateChangeCommmand;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public void AddCommand(BaseCommand command)
        {
            commandList.AddLast(command);
        }

    }
}
