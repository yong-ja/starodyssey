using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odysseus
{
    public partial class OdysseusForm
    {
        private void snapToGridMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Options.SnapToGrip = snapToGridMenuItem.Checked;
        }
    }
}
