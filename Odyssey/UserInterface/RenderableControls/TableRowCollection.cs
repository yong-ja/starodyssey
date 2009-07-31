using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface
{
    public class TableRowCollection : Collection<TableRow>
    {
        Table ownerTable;

        public TableRowCollection(Table owner)
        {
            ownerTable = owner;
        }

        protected override void InsertItem(int index, TableRow item)
        {
            base.InsertItem(index, item);
            item.Id = string.Format("{0}_Row{1:D2}", ownerTable.Id, index);
            item.RowIndex = index;
            item.OwnerTable = ownerTable;
            ownerTable.Controls.Add(item);
            ownerTable.ComputeSize();
        }

    }
}
