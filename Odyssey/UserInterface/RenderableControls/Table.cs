using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class Table : ContainerControl
    {
        public const string ControlTag = "Table";

        int cellSpacing;
        Padding cellPadding;

        TableRowCollection rows;

        public int CellSpacing
        {
            get { return cellSpacing; }
            set { cellSpacing = value; }
        }

        public TableRowCollection Rows
        {
            get { return rows; }
        }

        public override bool IntersectTest(System.Drawing.Point cursorLocation)
        {
            return Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
        }

        public Table()
        {
            ApplyControlStyle(StyleManager.GetControlStyle(ControlTag));
            rows = new TableRowCollection(this);
        }

        protected override void UpdatePositionDependantParameters()
        {
            for (int i = 0; i < rows.Count; i++)
            {
                TableRow row = rows[i];
                row.Position = new Vector2(0, row.Height * i );
            }
        }

        internal void ComputeSize()
        {
            int width = BorderSize * 2 + Padding.Horizontal + rows[0].Width;
            int height = BorderSize * 2 + Padding.Vertical;

            foreach (TableRow row in rows)
            {
                height += row.Height + cellSpacing;
            }

            Size = new Size(width, height);
        }
    }
}
