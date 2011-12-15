using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class CommandList<TCommand> : Collection<TCommand>
        where TCommand : ICommand
    {
        protected override void ClearItems()
        {
            foreach (TCommand tCommand in Items)
                tCommand.Dispose();
            base.ClearItems();
            
        }
    }
}
