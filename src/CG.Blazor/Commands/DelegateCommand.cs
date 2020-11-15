using System;
using System.Windows.Input;

namespace CG.Blazor.Commands
{
    public delegate void SimpleEventHandler();

    public sealed class DelegateCommand : ICommand
    {
        private SimpleEventHandler _handler;
        private bool _isEnabled = true;

        public event EventHandler CanExecuteChanged;

        public bool IsEnabled
        {
            get { return this._isEnabled; }
        }

        public DelegateCommand(SimpleEventHandler handler)
        {
            this._handler = handler;
        }
        
        void ICommand.Execute(object arg)
        {
            this._handler();
        }

        bool ICommand.CanExecute(object arg)
        {
            return this.IsEnabled;
        }

        private void OnCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }        
    }
}
