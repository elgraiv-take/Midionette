using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MidionetteSampleGui.Utility
{
    class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return _isEnabled;
        }

        private Action<object> _targetAction;

        public DelegateCommand(Action action)
        {
            _targetAction = (o) => action();
        }

        public DelegateCommand(Action<object> action)
        {
            _targetAction = action;
        }

        public void Execute(object parameter)
        {
            _targetAction?.Invoke(parameter);
        }
    }
}
