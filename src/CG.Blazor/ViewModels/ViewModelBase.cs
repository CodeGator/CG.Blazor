using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CG.Blazor.ViewModels
{
    /// <summary>
    /// This class is a base implementation of the <see cref="IViewModel"/>
    /// interface.
    /// </summary>
    public abstract class ViewModelBase : IViewModel
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

        #endregion
    }
}
