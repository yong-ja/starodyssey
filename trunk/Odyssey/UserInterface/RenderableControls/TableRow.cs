using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using System.Drawing;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class TableRow : ContainerControl 
    {
        public const string ControlTag = "TableRow";
        int rowIndex;

        Table ownerTable;

        TableCellCollection cells;

        public TableCellCollection Cells
        {
            get { return cells; }
        }

        public int Width
        {
            get { return Size.Width; }
            set
            {
                if (Size.Width != value)
                {
                    Size = new Size(value, Size.Height);
                }
            }
        }

        public int Height
        {
            get { return Size.Height; }
            set
            {
                if (Size.Height != value)
                {
                    Size = new Size(Size.Width, value);
                }
            }
        }

        internal Table OwnerTable
        {
            get { return ownerTable; }
            set
            {
                if (ownerTable != value)
                {
                    ownerTable = value;
                    UpdateSizeDependantParameters();
                }

            }
        }

        internal int RowIndex
        {
            get { return rowIndex; }
            set { rowIndex = value; }
        }

        public TableRow()
        {
            ApplyControlStyle(StyleManager.GetControlStyle(ControlTag));
            cells = new TableCellCollection(this);
        }

        protected override void UpdateSizeDependantParameters()
        {
            base.UpdateSizeDependantParameters();
            int cellSpacing = ownerTable.CellSpacing;

            for (int i = 0; i < cells.Count; i++)
            {
                TableCell cell = cells[i];

                int newWidth = ClientSize.Width / cells.Count - cellSpacing * (cells.Count - 1);
                cell.Size = new Size(newWidth, ClientSize.Height);
                cell.Position = new Vector2(i * newWidth + cellSpacing, 0);

            }
        }


    }
}
