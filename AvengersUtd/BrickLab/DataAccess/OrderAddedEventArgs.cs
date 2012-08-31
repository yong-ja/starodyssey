using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.BrickLab.Model;

namespace AvengersUtd.BrickLab.DataAccess
{
    public class OrderAddedEventArgs : EventArgs
    {
        public Order NewOrder { get; private set; }

        public OrderAddedEventArgs(Order newOrder)
        {
            NewOrder = newOrder;
        }
    }
}
