using System;
using System.Windows.Input;
using Hospital.Desktop.Model.Util;

namespace ViewModel.Util
{
    public class DelegateCommand : ICommand
    {

        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public DelegateCommand(Action<object?> execute) : this((Func<object?, bool>?) null, execute) { }

        public DelegateCommand(State<bool> canExecute, Action<object?> execute)
        {
            _canExecute = _ => canExecute.Value;
            canExecute.ValueChanged += (_, _) => RaiseCanExecuteChanged();
            _execute = execute;
        }
        
        public DelegateCommand(Func<object?, bool>? canExecute, Action<object?> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }


        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;  //_canExecute == null || _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            if(CanExecute(parameter)) _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
