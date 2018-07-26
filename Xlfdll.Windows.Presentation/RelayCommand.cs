using System;
using System.Windows.Input;

namespace Xlfdll.Windows.Presentation
{
    public class RelayCommand<T> : ICommand
    {
        public RelayCommand(Action<T> execute) : this(execute, null) { }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("The main execution delegate is null.");
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        private Action<T> _execute;
        private Predicate<T> _canExecute;

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Boolean CanExecute(Object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        public void Execute(Object parameter)
        {
            _execute((T)parameter);
        }

        #endregion
    }
}