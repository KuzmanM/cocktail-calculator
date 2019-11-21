using System;
using System.Windows.Input;

namespace Common.WPF
{
    public class Command : ICommand
    {
        #region Initialisation

        private Action<object> _action;
        private Func<object, bool> _canExecute;

        public Command(Action<object> action, Func<object, bool> canExecute = null)
        {
            _action = action;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                return _canExecute(parameter);
            else
                return true;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}
