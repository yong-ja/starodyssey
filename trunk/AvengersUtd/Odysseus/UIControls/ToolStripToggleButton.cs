using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AvengersUtd.Odysseus.UIControls
{
    public class ToolStripToggleButton : ToolStripButton
    {
        static readonly Color DefaultBackColor = Color.FromArgb(248, 248, 248);
        static readonly Color DefaultHighlightBackColor = Color.FromArgb(206, 237, 250);
        static readonly Color DefaultBorderColor = Color.FromArgb(51, 153, 255);

        static readonly Pen BorderPen = new Pen(DefaultBorderColor);
        static readonly SolidBrush DefaultBrush = new SolidBrush(DefaultBackColor);
        static readonly SolidBrush HighlightBrush = new SolidBrush(DefaultHighlightBackColor);

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            ToolStrip toolStrip = Owner;
            foreach (ToolStripButton button in
                toolStrip.Items.OfType<ToolStripButton>().Where(button => button.Tag != Tag && button.Checked))
            {
                button.CheckState = CheckState.Unchecked;
            }

            toolStrip.Tag = this;
            Checked = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle innerArea = new Rectangle
            {
                Location = new Point(ContentRectangle.X - 1, ContentRectangle.Y - 1),
                Size = new Size(ContentRectangle.Width + 2, ContentRectangle.Height + 2)
            };

            base.OnPaint(e);
            if (Checked || Selected)
            {
                e.Graphics.FillRectangle(HighlightBrush, innerArea);
                e.Graphics.DrawRectangle(BorderPen, e.ClipRectangle);

                e.Graphics.DrawString(Text, Font, Brushes.Black, ContentRectangle);
            }
            else
            {
                e.Graphics.FillRectangle(DefaultBrush, innerArea);
                e.Graphics.DrawString(Text, Font, Brushes.Black, ContentRectangle);
            }
        }
    }
}
