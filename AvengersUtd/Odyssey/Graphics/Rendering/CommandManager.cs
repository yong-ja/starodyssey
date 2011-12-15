﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class CommandManager
    {
        private readonly List<IUpdateCommand> updateList;
        private readonly LinkedList<ICommand> commandList;

        public GenericUpdateCommand Updater { get; private set; }

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
            Updater = new GenericUpdateCommand();
            updateList = new List<IUpdateCommand>();
            commandList = new LinkedList<ICommand>();
            commandList.AddFirst(DepthStencilStateChangeCommand.Default);
            commandList.AddFirst(RasterizerStateChangeCommand.Default);

            AddUpdateCommand(Updater);
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
            Contract.Requires<ArgumentException>(command.CommandType != CommandType.Render);
            commandList.AddLast(command);
        }

        public void AddBaseCommands(ICommand[] commands)
        {
            foreach (BaseCommand command in commands)
                AddBaseCommand(command);
        }

        public void AddUpdateCommand(IUpdateCommand command)
        {
            Contract.Requires<ArgumentException>(command.CommandType != CommandType.Render);

            if (command.IsThreaded)
                command.StartThread();
            updateList.Add(command);
        }

        public void AddRenderCommand(MaterialNode mNode, RenderableCollection rNodeCollection)
        {
           
            Type renderCommandType = mNode.RenderableCollection.Description.PreferredRenderCommandType;
            RenderCommand rCommand = (RenderCommand)Activator.CreateInstance(renderCommandType, new object[] {mNode, rNodeCollection});
            rCommand.Init();
            commandList.AddLast(rCommand);
        }

        public void AddRenderCommand(IRenderCommand rCommand)
        {
            Type renderCommandType = rCommand.GetType();


            rCommand.Init();
            commandList.AddLast(rCommand);
        }

        public void RunCommand(BaseCommand command, bool waitRender=false)
        {
            if (waitRender)
                Game.RenderEvent.Wait();
            command.Execute();
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