using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace AvengersUtd.BrickLab.Controls
{
    public class GroupBox : System.Windows.Controls.GroupBox
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (VisualChildrenCount == 0) return;

            var grid = GetVisualChild(0) as Grid;
            if (grid != null && grid.Children.Count > 3)
            {
                var bd = grid.Children[3] as Border;
                if (bd != null)
                {
                    bd.IsHitTestVisible = false;
                }
            }
        }
    }
}
