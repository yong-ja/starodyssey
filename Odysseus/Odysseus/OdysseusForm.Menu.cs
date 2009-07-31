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

        private void editControlStylesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OdysseyUI.CurrentHud.BeginDesign();
            CStyleForm styleForm = new CStyleForm();
            styleForm.ShowDialog();
            OdysseyUI.CurrentHud.EndDesign();

        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
            UIExporter.ToCS(uiRenderer.Hud);
        }
    }
}
