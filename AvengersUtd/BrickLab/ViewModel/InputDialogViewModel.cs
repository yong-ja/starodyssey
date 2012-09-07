using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using AvengersUtd.BrickLab.Controls;
using AvengersUtd.BrickLab.View;

namespace AvengersUtd.BrickLab.ViewModel
{

    public class InputDialogViewModel : ViewModelBase
    {
        internal delegate bool CanExecuteDelegate();
        private string label;
        private string description;
        private string response;
        
        

        public string Label
        {
            get { return label; }
            set
            {
                if (label == value)
                    return;
                label = value;
                RaisePropertyChanged("Label");
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (description == value)
                    return;
                description = value; 
                RaisePropertyChanged("Description");
            }
        }

        public string Response
        {
            get { return response; }
            set
            {
                if (response == value)
                    return;
                response = value;
                RaisePropertyChanged("Response");
            }
        }

        internal CanExecuteDelegate CanExecuteMethod { get; set; }
        internal Type InputDialogType { get { return typeof (InputDialogView); } }
        public ICommand DefaultCommand { get; set; }
       

        public ICommand CloseDialogCommand
        {
            get { return new DelegateCommand<Window>(param => param.Close(), param => param.IsVisible); }
        }
    }
}
