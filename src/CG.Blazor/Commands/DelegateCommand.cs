using CG.Validations;
using System;
using System.Windows.Input;

namespace CG.Blazor.Commands
{
    /// <summary>
    /// This delegate type is used to process command events.
    /// </summary>
    public delegate void CommandEventHandler();

    /// <summary>
    /// This class is a default implementation of the <see cref="ICommand"/>
    /// interface.
    /// </summary>
    public sealed class DelegateCommand : ICommand
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties
        
        private CommandEventHandler _handler;
        private bool _isEnabled = true;

        #endregion

        // *******************************************************************
        // Events.
        // *******************************************************************

        #region Events

        /// <summary>
        /// This event is raised whenever the <see cref="IsEnabled"/> property 
        /// of the command has changed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        #endregion

        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property indicates whether the command can be executed, or not.
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
        }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="DelegateCommand"/>
        /// class.
        /// </summary>
        /// <param name="handler">The delegate to use with the command.</param>
        public DelegateCommand(
            CommandEventHandler handler
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(handler, nameof(handler));

            // Save the refernces.
            _handler = handler;
        }

        #endregion

        // *******************************************************************
        // ICommand implementation.
        // *******************************************************************

        #region ICommand implementation

        /// <inheritdoc/>
        void ICommand.Execute(
            object arg
            )
        {
            _handler();
        }

        // *******************************************************************

        /// <inheritdoc/>
        bool ICommand.CanExecute(
            object arg
            )
        {
            return IsEnabled;
        }

        #endregion

        // *******************************************************************
        // Private methods.
        // *******************************************************************

        #region Private methods

        /// <summary>
        /// This method raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        private void OnCanExecuteChanged()
        {
            if (null != CanExecuteChanged)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
