﻿using System;
using System.Drawing;
using System.Windows.Forms;
using AvengersUtd.Odysseus.UIControls;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.UserInterface;

namespace AvengersUtd.Odysseus
{
    public partial class Main
    {

        private void exportAsCFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.UIRenderer.Hud.Controls.IsEmpty)
                return;

            MessageBox.Show(CSExporter.Export(UIRenderer.Hud));

        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options options = new Options();
            Size prevSize = options.Resolution;
            DialogResult result = options.ShowDialog(this);
            Size currentSize = options.Resolution;
            if (result == DialogResult.OK && currentSize != prevSize)
            {
                Game.Context.ResizeDevice(currentSize, false);
                int wDiff = currentSize.Width - prevSize.Width;
                int hDiff = currentSize.Height - prevSize.Height;
                renderPanel.Width += wDiff;
                renderPanel.Height += hDiff;
                Size = new Size(Width+wDiff, Height+hDiff);
            }
        }

        private void toolboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolbox.Show(); 

            if (!toolbox.Focused)
                toolbox.Focus();
        }


        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            properties.Show(); 

            if (!properties.Focused)
                properties.Focus();
        
        }

        private void snapToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UIRenderer.ControlSelector.OwnerGrid.SnapToGrid = snapToGridToolStripMenuItem.Checked;
        }

        private void styleEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StyleEditor styleEditor = new StyleEditor();
            styleEditor.ShowDialog();
        }

        private void gradientEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GradientEditor gradientEditor = new GradientEditor();
            gradientEditor.ShowDialog();
        }
    }
}
