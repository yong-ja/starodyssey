using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Windows.Input;
using AvengersUtd.BrickLab.Controls;
using AvengersUtd.BrickLab.DataAccess;

namespace AvengersUtd.BrickLab.ViewModel
{
    public class InventoryViewModel : ViewModelBase
    {
        private readonly Inventory inventory;
        private ObservableCollection<ItemViewModel> partList;
        public ICommand DownloadSetCommand { get { return new DelegateCommand<string>(DownloadSet, null); } }

        public InventoryViewModel()
        {
            inventory = new Inventory();
        }

        public ObservableCollection<ItemViewModel> PartList
        {
            get { return partList; }
            private set
            {
                if (partList == value)
                    return;

                partList = value;
                RaisePropertyChanged("PartList");
            }
        }

        public void DownloadSet(string setId)
        {
            //Contract.Requires(!string.IsNullOrEmpty(setId));
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += delegate
                             {
                                 inventory.DownloadSetInventory(setId);
                                 PartList = new ObservableCollection<ItemViewModel>(from p in inventory.GetParts()
                                                                                    select new ItemViewModel(p));
                             };
            Mouse.OverrideCursor = Cursors.Wait;
            worker.RunWorkerCompleted += (sender, e) => Mouse.OverrideCursor = Cursors.Arrow;
            worker.RunWorkerAsync();
        }
    }
}
