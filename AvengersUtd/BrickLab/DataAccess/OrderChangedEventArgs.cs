using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.BrickLab.Model;

namespace AvengersUtd.BrickLab.DataAccess
{
    public class OrderChangedEventArgs : EventArgs
    {
        public Order OldOrder {get; private set;}
        public Order NewOrder { get; private set; }

        public OrderChangedEventArgs(Order newOrder, Order oldOrder)
        {
            NewOrder = newOrder;
            OldOrder = oldOrder;
        }
    }
}
