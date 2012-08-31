using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using AvengersUtd.BrickLab.ViewModel;

namespace AvengersUtd.BrickLab.Controls
{
    public class MenuItem 
    {
        public string Header { get; set; }
        public List<MenuItem> Children { get; private set; }
        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }

        public MenuItem(string item)
        {
            Header = item;
            Children = new List<MenuItem>();
        }
    }
}
