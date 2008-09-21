using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class TableCellCollection : Collection<TableCell>
    {
        TableRow ownerRow;

        public TableCellCollection(TableRow row)
        {
            ownerRow = row;
        }

        protected override void InsertItem(int index, TableCell item)
        {
            base.InsertItem(index, item);
            item.Id = string.Format("{0}_Cell:R{1:D2}C{2:D2}", ownerRow.OwnerTable, ownerRow.RowIndex, index);
            item.ColumnIndex = index;
            item.OwnerRow = ownerRow;
            ownerRow.Controls.Add(item);
        }

        
    }
}
