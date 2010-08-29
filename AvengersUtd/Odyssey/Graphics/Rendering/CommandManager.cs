using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class CommandManager
    {
        private readonly List<IUpdateCommand> updateList;
        private readonly LinkedList<ICommand> commandList;

        public IEnumerable<ICommand> Commands
        {
            get { return commandList; }
        }

        public IEnumerable<IUpdateCommand> UpdateCommands
        {
            get { return updateList; }
        }

        public CommandManager()
        {
            updateList = new List<IUpdateCommand>();
            commandList = new LinkedList<ICommand>();
            commandList.AddFirst(DepthStencilStateChangeCommand.Default);
            commandList.AddFirst(RasterizerStateChangeCommand.Default);
        }

        //public bool IsStateActive(ICommand stateChangeCommmand)
        //{
        //    try
        //    {
        //        ICommand activeState =
        //            commandList.Last.FindFirstBackward(stateChangeCommmand).Value;
        //        return activeState == stateChangeCommmand;
        //    }
        //    catch (InvalidOperationException)
        //    {
        //        return false;
        //    }
        //}

        public void AddBaseCommand(ICommand command)
        {
            if (command.CommandType == CommandType.Render)
                throw Error.ArgumentInvalid("command", GetType(), "AddBaseCommand", Properties.Resources.ERR_AddBaseCommand);
            commandList.AddLast(command);
        }

        public void AddBaseCommands(ICommand[] commands)
        {
            foreach (BaseCommand command in commands)
                AddBaseCommand(command);
        }

        public void AddUpdateCommand(IUpdateCommand command)
        {
            if (command.CommandType != CommandType.Update)
                throw Error.ArgumentInvalid("command", GetType(), "AddUpdateCommand", Properties.Resources.ERR_AddUpdateCommand);
            if (command.IsThreaded)
                command.StartThread();
            updateList.Add(command);
        }

        public void AddRenderCommand(MaterialNode mNode, RenderableCollection rNodeCollection)
        {
            Type renderCommandType = mNode.RenderableCollection.Description.PreferredRenderCommandType;

            if (renderCommandType != typeof(RenderCommand) && !renderCommandType.IsSubclassOf(typeof(RenderCommand)))
                throw Error.ArgumentInvalid("mNode", GetType(), "AddRenderCommand", Properties.Resources.ERR_AddRenderCommand);

            RenderCommand rCommand = (RenderCommand)Activator.CreateInstance(renderCommandType, new object[] {mNode, rNodeCollection});
            commandList.AddLast(rCommand);
        }

        public void AddRenderCommand(ICommand rCommand)
        {
            Type renderCommandType = rCommand.GetType();
            if (renderCommandType != typeof(RenderCommand) && !renderCommandType.IsSubclassOf(typeof(RenderCommand)))
                throw Error.ArgumentInvalid("mNode", GetType(), "AddRenderCommand", Properties.Resources.ERR_AddRenderCommand);
            commandList.AddLast(rCommand);
        }

        public void Dispose()
        {
            foreach (IUpdateCommand updateCommand in updateList)
            {
                updateCommand.Dispose();
            }

            foreach (ICommand baseCommand in commandList)
            {
                baseCommand.Dispose();
            }
        }

    }
}
