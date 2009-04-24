using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.UserInterface;

namespace AvengersUtd.Odysseus
{
    public partial class OdysseusForm
    {
    
        private void snapToGridMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Options.SnapToGrid = snapToGridMenuItem.Checked;
        }

       
    }
}
