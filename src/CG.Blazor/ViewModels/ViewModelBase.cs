using CG.Validations;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CG.Blazor.ViewModels
{
    /// <summary>
    /// This class is a base implementation of the <see cref="IViewModel"/>
    /// interface.
    /// </summary>
    public abstract class ViewModelBase : DisposableBase, IViewModel
    {
        // *******************************************************************
        // Events.
        // *******************************************************************

        #region Events

        /// <summary>
        /// This event is raised whenever a property value changes on the view-model.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains a reference to an inner view-model.
        /// </summary>
        protected ViewModelBase InnerViewModel { get; }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="ViewModelBase"/>
        /// class.
        /// </summary>
        public ViewModelBase()
        {

        }

        // *******************************************************************

        /// <summary>
        /// This constructor creates a new instance of the <see cref="ViewModelBase"/>
        /// class.
        /// </summary>
        /// <param name="innerViewModel">The inner view-model to use with this
        /// view-model.</param>
        public ViewModelBase(
            ViewModelBase innerViewModel
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(innerViewModel, nameof(innerViewModel));

            // Save the references.
            InnerViewModel = innerViewModel;

            // Wire up a handler for any inner view-model.
            InnerViewModel.PropertyChanged += (sender, e) =>
            {
                // Propagate the notification.
                OnPropertyChanged(e.PropertyName);
            };
        }

        #endregion

        // *******************************************************************
        // Protected methods.
        // *******************************************************************

        #region Protected methods

        /// <summary>
        /// This method raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = ""
            )
        {
            // Raise the event.
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName)
                );
        }

        // *******************************************************************

        /// <summary>
        /// This method sets the value of the specified property's backing 
        /// field, then calls <see cref="OnPropertyChanged(string)"/> on behalf
        /// of the property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="backingField">The backing field associated with the property.</param>
        /// <param name="value">The value to set in the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        protected void SetValue<T>(
            ref T backingField, 
            T value, 
            [CallerMemberName] string propertyName = null
            )
        {
            // Is the new value same as the old value?
            if (EqualityComparer<T>.Default.Equals(backingField, value))
            {
                return; // Nothing to do!
            }

            // Set the value of the backing field.
            backingField = value;

            // Tell the world what we did.
            OnPropertyChanged(
                propertyName
                );
        }

        // *******************************************************************

        /// <summary>
        /// This method is called when the object should free managed resources.
        /// </summary>
        /// <param name="disposing">True to cleanup managed resources.</param>
        protected override void Dispose(
            bool disposing
            )
        {
            // Should we cleanup managed resources.
            if (disposing)
            {
#pragma warning disable CS1998
                if (null != InnerViewModel)
                {
                    InnerViewModel.PropertyChanged -= async (sender, e) => { };
                }
#pragma warning restore CS1998 
            }

            // Give the base class a chance.
            base.Dispose(disposing);
        }

        #endregion
    }
}
