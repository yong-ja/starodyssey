using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AvengersUtd.BrickLab.Controls
{
    public class DelegateCommand<T> : ICommand
        where T : class
    {
        private readonly Action<T> execute;
        private readonly Predicate<T> canExecute;
        private Type type;

        public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            type = typeof (T);
        }

        public void Execute(T parameter)
        {
            execute(parameter);
        }

        public bool CanExecute(T parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            T arg = parameter as T;

            return arg != null && CanExecute(arg);
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        void ICommand.Execute(object parameter)
        {
            T arg = parameter as T;
            if (arg != null)
                Execute(arg);
        }
    }
}
    