using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AvengersUtd.BrickLab.ViewModel
{
    public class ObservableCollection<T> : System.Collections.ObjectModel.ObservableCollection<T>
        where T : INotifyPropertyChanged
    {
        public ObservableCollection()
            : base()
        {
            CollectionChanged += new NotifyCollectionChangedEventHandler(ObservableCollection_CollectionChanged);
        }

        public ObservableCollection(IEnumerable<T> items)
            : base(items)
        {}

        void ObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                {
                    item.PropertyChanged += item_PropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= item_PropertyChanged;
                }
            }
        }

        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyCollectionChangedEventArgs a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(a);
        }
    }
}
