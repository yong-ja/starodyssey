using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using AvengersUtd.BrickLab.Model;

namespace AvengersUtd.BrickLab.ViewModel
{
    public class ItemViewModel:ViewModelBase
    {
        private readonly Item item;

        public ItemViewModel(Item p)
        {
            this.item = p;
        }

        public string ItemNr
        {
            get { return item.ItemNr; }
            set
            {
                if (item.ItemNr == value)
                    return;
                item.ItemNr = value;
                RaisePropertyChanged("ItemNr");
            }
        }

        
        public Uri ImageUri
        {
            get { return item.ImageUri; }
            set
            {
                if (item.ImageUri == value)
                    return;
                item.ImageUri = value;
                RaisePropertyChanged("ImageUri");
            }
        }

        
        public int Quantity
        {
            get { return item.Quantity; }
            set
            {
                if (item.Quantity == value)
                    return;
                item.Quantity = value;
                RaisePropertyChanged("Item");
            }
        }

        
        public float Price
        {
            get { return item.Price; }
            set
            {
                if (Math.Abs(item.Price - value) < Global.Epsilon)
                    return;
                item.Price = value;
                RaisePropertyChanged("Price");
            }
        }

        
        public Condition Condition
        {
            get { return item.Condition; }
            set
            {
                if (item.Condition == value)
                    return;
                item.Condition = value;
                RaisePropertyChanged("Condition");
            }
        }

        
        //public Color Color
        //{
        //    get { return Item.Color; }
        //    set { if (Item.Color == value)
        //        return;
        //        Item.Color = value;
        //        RaisePropertyChanged("Color");
        //    }
        //}


        public string Description
        {
            get { return item.Description; }
            set
            {
                if (item.Description == value)
                    return;
                item.Description = value;
                RaisePropertyChanged("Description");
            }
        }
    }
}
