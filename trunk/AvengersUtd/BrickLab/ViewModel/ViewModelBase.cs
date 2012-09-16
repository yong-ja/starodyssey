using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace AvengersUtd.BrickLab.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Dispatcher Dispatcher { get; private set; }

        public ViewModelBase()
        {
            Dispatcher = Application.Current != null ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
