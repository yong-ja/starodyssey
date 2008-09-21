using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class CommandList<TCommand> : Collection<TCommand>
        where TCommand : BaseCommand
    {
    }
}
