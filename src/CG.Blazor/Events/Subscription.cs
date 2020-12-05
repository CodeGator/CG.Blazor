using CG.Validations;
using System;
using System.Threading;

namespace CG.Blazor.Events
{
    /// <summary>
    /// This class represents an event subscription.
    /// </summary>
    internal class Subscription : ISubscription
    {
        // *******************************************************************
        // Fields.
        // *******************************************************************

        #region Fields

        /// <summary>
        ///  This field contains a delegate.
        /// </summary>
        protected readonly IWeakDelegate _weakDelegate;
        
        #endregion

        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the event subscriber.
        /// </summary>
        public ISubscriber Subscriber { get; set; }

        /// <summary>
        /// This property contains the action for the event.
        /// </summary>
        public Action Action
        {
            get { return _weakDelegate.Target as Action; }
        }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="Subscription"/>
        /// class.
        /// </summary>
        /// <param name="weakDelegate">The delegate to use with the subscription.</param>
        public Subscription(
            IWeakDelegate weakDelegate
            )
        {
            // Validate the parameter before attempting to use them.
            Guard.Instance().ThrowIfNull(weakDelegate, nameof(weakDelegate));

            // Save the refernces.
            _weakDelegate = weakDelegate;
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method retrieves the action for the subscription.
        /// </summary>
        /// <returns>An action delegate.</returns>
        public virtual Action<object[]> GetAction()
        {
            // Look for the action.
            var action = Action;

            // Did we find one?
            if (null != action)
            {
                // Return the action.
                return arguments =>
                {
                    // Validate the action.
                    Guard.Instance().ThrowIfNull(action, nameof(action));

                    // Invoke the action.
                    action();
                };
            }

            // We didn't find the action.
            return null;
        }

        #endregion
    }



    /// <summary>
    /// This class represents an event subscription.
    /// </summary>
    internal class Subscription<T> : Subscription
    {
        // *******************************************************************
        // Fields
        // *******************************************************************

        #region Fields

        /// <summary>
        /// This field contains a filter delegate.
        /// </summary>
        private readonly IWeakDelegate _filterReference;

        #endregion

        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the delegate action for the subscription.
        /// </summary>
        public new Action<T> Action
        {
            get { return _weakDelegate.Target as Action<T>; }
        }

        /// <summary>
        /// This property contains the predicate action for the subscription.
        /// </summary
        public Predicate<T> Filter
        {
            get { return (Predicate<T>)_filterReference.Target; }
        }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="Subscription"/>
        /// class.
        /// </summary>
        /// <param name="weakDelegate">The delegate to use with the subscription.</param>
        /// <param name="filterReference">A filter delete to use with the subscription.</param>
        public Subscription(
            IWeakDelegate weakDelegate,
            IWeakDelegate filterReference
            ) : base(weakDelegate)
        {
            // Validate the parameter before attempting to use them.
            Guard.Instance().ThrowIfNull(filterReference, nameof(filterReference));

            // Save the refernces.
            _filterReference = filterReference;
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method retrieves the action for the subscription.
        /// </summary>
        /// <returns>An action delegate.</returns>
        public override Action<object[]> GetAction()
        {
            // Look for the action.
            var action = Action;

            // Look for the predicate.
            var filter = this.Filter;

            // Did we find both?
            if (action != null && filter != null)
            {
                // Return the action.
                return arguments =>
                {
                    // Get the default for the argument.
                    T argument = default(T);

                    // Is there a value to copy?
                    if (null != arguments && 
                        0 < arguments.Length && 
                        null != arguments[0])
                    {
                        // Copy the value.
                        argument = (T)arguments[0];
                    }

                    // Do we meet the filter condition?
                    if (filter(argument))
                    {
                        // Validate the action.
                        Guard.Instance().ThrowIfNull(action, nameof(action));

                        // Invoke the action.
                        action(argument);
                    }
                };
            }

            // We didn't find the action.
            return null;
        }

        #endregion
    }
}
