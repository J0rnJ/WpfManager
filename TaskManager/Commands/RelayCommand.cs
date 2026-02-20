using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TaskManager.Commands
{
    public class RelayCommand : ICommand
    {
        #region Fields
        private readonly Action? _execute;
        private readonly Func<bool>? _canExecute;
        #endregion

        #region Events
        public event EventHandler? CanExecuteChanged;
        #endregion

        #region Constructors
        public RelayCommand(Action? execute)
            : this(execute, null)
        { }

        public RelayCommand(Action? execute, Func<bool>? canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion

        #region Methods
        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return this._canExecute();
        }

        public void Execute(object? parameter)
        {
            if (this._execute != null)
            {
                this._execute();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                EventHandler handler = this.CanExecuteChanged;
                handler(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}
