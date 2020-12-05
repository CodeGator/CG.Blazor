using CG.Validations;
using System;

namespace CG.Blazor.Events
{
    /// <summary>
    /// This class is a default implementation of the <see cref="IEquatable{ISubscriber}"/> 
    /// and <see cref="ISubscriber"/> interfaces.
    /// </summary>
    internal sealed class Subscriber : 
        DisposableBase, 
        IEquatable<ISubscriber>, 
        ISubscriber
    {
        // *******************************************************************
        // Fields.
        // *******************************************************************

        #region Fields

        /// <summary>
        /// This field contains an identifier for the subscriber.
        /// </summary>
        private readonly Guid _id;

        /// <summary>
        /// This field contains an action for unsubscribe operations.
        /// </summary>
        private readonly Action<ISubscriber> _unsubscribeAction;

        #endregion

        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <inheritdoc />
        public Guid Id { get { return _id; } }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="Subscriber"/>
        /// class.
        /// </summary>
        /// <param name="unsubscribeAction">The action to perform during an unsubscribe
        /// operation.</param>
        public Subscriber(
            Action<ISubscriber> unsubscribeAction
            )
        {
            // Validate the parameter before attempting to use them.
            Guard.Instance().ThrowIfNull(unsubscribeAction, nameof(unsubscribeAction));

            // Save the references.
            _id = Guid.NewGuid();
            _unsubscribeAction = unsubscribeAction;
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method determines whether the specified subscriber instances 
        /// are considered equal.
        /// </summary>
        /// <param name="other">The subscriber to use for the comparison.</param>
        /// <returns>True if the two objects are considered equal; false otherwise.</returns>
        public bool Equals(
            ISubscriber other
            )
        {
            // Is the reference missing?
            if (null == other)
            {
                return false;
            }

            // Defer to the base class.
            return Equals(_id, other.Id);
        }

        // *******************************************************************

        /// <summary>
        /// This method determines whether the specified object instances are 
        /// considered equal.
        /// </summary>
        /// <param name="obj">The object to use for the comparison.</param>
        /// <returns>True if the two objects are considered equal; false otherwise.</returns>
        public override bool Equals(
            object obj
            )
        {
            // Is the reference the same?
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            // Defer to the base class.
            return Equals(obj as ISubscriber);
        }

        // *******************************************************************

        /// <summary>
        /// This method returns a hashcode to uniquely identify the object.
        /// </summary>
        /// <returns>An integer hash code value.</returns>
        public override int GetHashCode()
        {
            // Our identity is our identifier.
            return _id.GetHashCode();
        }

        #endregion

        // *******************************************************************
        // Protected methods.
        // *******************************************************************

        #region Protected methods

        /// <summary>
        /// This method is called to cleanup any managed resources owned by the object.
        /// </summary>
        /// <param name="disposing">True to cleanup managed resources.</param>
        protected override void Dispose(
            bool disposing
            )
        {
            // Should we cleanup managed resources?
            if (disposing)
            {
                // Do we own an unsubscribe action?
                if (_unsubscribeAction != null)
                {
                    // Invoke the delegate.
                    _unsubscribeAction(this);
                }
            }

            // Give the base class a chance.
            base.Dispose(disposing);
        }

        #endregion
    }
}
