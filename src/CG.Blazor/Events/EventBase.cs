using CG.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CG.Blazor.Events
{
    /// <summary>
    /// This class is a base for all aggregated event types.
    /// </summary>
    public abstract class EventBase
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains a list of subscriptions.
        /// </summary>
        protected IList<ISubscription> Subscriptions { get; }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="EventBase"/>
        /// class.
        /// </summary>
        protected EventBase()
        {
            // Create defaults for the properties.
            Subscriptions = new List<ISubscription>();
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods
        
        /// <summary>
        /// This method subscribes an action to the event.
        /// </summary>
        /// <param name="action">The delegate to use for the subscription.</param>
        /// <returns>A <see cref="ISubscriber"/> to represent the subscription.</returns>
        public virtual ISubscriber Subscribe(
            Action action
            )
        {
            // Validate the parameter before attempting to use them.
            Guard.Instance().ThrowIfNull(action, nameof(action));

            // Create a new subscription.
            var token = Subscribe(
                action, 
                false
                );

            // Return the token.
            return token;
        }

        // *******************************************************************

        /// <summary>
        /// This method subscribes an action to the event.
        /// </summary>
        /// <param name="action">The delegate to use for the subscription.</param>
        /// <param name="keepAlive">True to keep the reference alive by maintaining
        /// a reference to the action; false otherwise.</param>
        /// <returns>A <see cref="ISubscriber"/> to represent the subscription.</returns>
        public virtual ISubscriber Subscribe(
            Action action, 
            bool keepAlive
            )
        {
            // Validate the parameter before attempting to use them.
            Guard.Instance().ThrowIfNull(action, nameof(action));

            // Create a weak reference to the action.
            var actionReference = new WeakDelegate(
                action, 
                keepAlive
                );

            // Create the model.
            var subscription = new Subscription(
                actionReference
                );

            // Create a new subscription.
            var token = InternalSubscribe(
                subscription
                );

            // Return the token.
            return token;
        }

        // *******************************************************************

        /// <summary>
        /// This method unsubscribes an action from the event.
        /// </summary>
        /// <param name="action">The action to use for the subscription.</param>
        public virtual void Unsubscribe(
            Action action
            )
        {
            // Validate the parameter before attempting to use them.
            Guard.Instance().ThrowIfNull(action, nameof(action));

            lock (Subscriptions)
            {
                // Look for the subscription.
                var subscription = Subscriptions
                    .Cast<Subscription>() // only want Subscription types.
                    .FirstOrDefault(evt => evt.Action == action);

                // Did we find it?
                if (null != subscription)
                {
                    // Remove the subscription.
                    Subscriptions.Remove(
                        subscription
                        );
                }
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method unsubscribes an action from the event.
        /// </summary>
        /// <param name="token">The token to use for the subscription.</param>
        public virtual void Unsubscribe(
            ISubscriber token
            )
        {
            // Validate the parameter before attempting to use them.
            Guard.Instance().ThrowIfNull(token, nameof(token));

            lock (Subscriptions)
            {
                // Look for the subscription.
                var subscription = Subscriptions.FirstOrDefault(
                    evt => evt.Subscriber == token
                    );

                // Did we find it?
                if (null != subscription)
                {
                    // Remove the subscription.
                    Subscriptions.Remove(
                        subscription
                        );
                }
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method indicates whether the event contains the specified action.
        /// </summary>
        /// <param name="action">The action to use for the operation.</param>
        /// <returns>True if a match was found; false otherwise.</returns>
        public virtual bool Contains(
            Action action
            )
        {
            // Validate the parameter before attempting to use them.
            Guard.Instance().ThrowIfNull(action, nameof(action));

            lock (Subscriptions)
            {
                // Look for the subscription.
                var subscription = Subscriptions
                    .Cast<Subscription>() // only want Subscription types.
                    .FirstOrDefault(evt => evt.Action == action);

                // Did we find it?
                return null != subscription;
            }            
        }

        // *******************************************************************

        /// <summary>
        /// This method indicates whether the event contains the specified token.
        /// </summary>
        /// <param name="token">The token to use for the operation.</param>
        /// <returns>True if a match was found; false otherwise.</returns>
        public virtual bool Contains(
            ISubscriber token
            )
        {
            // Validate the parameter before attempting to use them.
            Guard.Instance().ThrowIfNull(token, nameof(token));

            lock (Subscriptions)
            {
                // Look for the subscription?
                var subscription = Subscriptions.FirstOrDefault(
                    evt => evt.Subscriber == token
                    );

                // Did we find it?
                return subscription != null;
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method publishes to any subscriptions for the event.
        /// </summary>
        public virtual void Publish()
        {
            InternalPublish();
        }

        #endregion

        // *******************************************************************
        // Protected methods.
        // *******************************************************************

        #region Protected methods

        /// <summary>
        /// This method adds a subscription to the list of subscriptions.
        /// </summary>
        /// <param name="subscription">The subscription to use for the operation.</param>
        /// <returns></returns>
        protected virtual ISubscriber InternalSubscribe(
            ISubscription subscription
            )
        {
            // Validate the parameter before attempting to use them.
            Guard.Instance().ThrowIfNull(subscription, nameof(subscription));

            // Create a token for the subscription.
            subscription.Subscriber = new Subscriber(
                Unsubscribe
                );

            lock (Subscriptions)
            {
                // Add the subscription.
                Subscriptions.Add(
                    subscription
                    );
            }

            // Return the token.
            return subscription.Subscriber;
        }

        // *******************************************************************

        /// <summary>
        /// This method publishes to all active subscriptions.
        /// </summary>
        /// <param name="arguments">The arguments to use for the operation.</param>
        protected virtual void InternalPublish(
            params object[] arguments
            )
        {
            // Validate the parameter before attempting to use them.
            Guard.Instance().ThrowIfNull(arguments, nameof(arguments));

            // Loop and execute actions.
            foreach (var action in PruneAndReturnActions())
            {
                // Invoke the action.
                action(arguments);
            }
        }

        #endregion

        // *******************************************************************
        // Private methods.
        // *******************************************************************

        #region Private methods

        /// <summary>
        /// This method removes any inactive subscriptions from the internal list
        /// of subscriptions, then it returns a list of actions for any subscriptions
        /// that are left in the list.
        /// </summary>
        /// <returns>A list of actions.</returns>
        private List<Action<object[]>> PruneAndReturnActions()
        {
            var list = new List<Action<object[]>>();

            lock (Subscriptions)
            {
                // Loop through the subscriptions, oldest first.
                for (var i = Subscriptions.Count - 1; i >= 0; i--)
                {
                    // Get the action for the subscription.
                    var action = Subscriptions[i].GetAction();

                    // Is the action dead?
                    if (action == null)
                    {
                        // Remove from the subscription list.
                        Subscriptions.RemoveAt(i);
                    }
                    else
                    {
                        // Otherwise, add to the action list.
                        list.Add(action);
                    }
                }
            }

            // Return the actions.
            return list;
        }

        #endregion
    }


    /// <summary>
    /// This class is a base for all aggregated event types.
    /// </summary>
    /// <typeparam name="T">The type of data associated with the event.</typeparam>
    public abstract class EventBase<T> : EventBase
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method subscribes an action to the event.
        /// </summary>
        /// <param name="action">The action to use for the operation.</param>
        /// <returns>A <see cref="ISubscriber"/> to represent the subscription.</returns>
        public ISubscriber Subscribe(
            Action<T> action
            )
        {
            // Validate the parameter before attempting to use them.
            Guard.Instance().ThrowIfNull(action, nameof(action));

            // Create a new subscription.
            var token = Subscribe(
                action,
                false
                );

            // Return the token.
            return token;
        }

        // *******************************************************************

        /// <summary>
        /// This method subscribes an action to the event.
        /// </summary>
        /// <param name="action">The action to use for the operation.</param>
        /// <param name="filter">The predicate to associate with the subscription.</param>
        /// <returns>A <see cref="ISubscriber"/> to represent the subscription.</returns>
        public ISubscriber Subscribe(
            Action<T> action, 
            Predicate<T> filter
            )
        {
            return Subscribe(
                action, 
                false, 
                filter
                );
        }

        // *******************************************************************

        /// <summary>
        /// This method subscribes an action to the event.
        /// </summary>
        /// <param name="action">The action to use for the operation.</param>
        /// <param name="keepAlive">True to keep the reference alive by maintaining
        /// a reference to the action; false otherwise.</param>
        /// <returns>A <see cref="ISubscriber"/> to represent the subscription.</returns>
        public ISubscriber Subscribe(
            Action<T> action, 
            bool keepAlive
            )
        {
            return Subscribe(
                action, 
                keepAlive, 
                null
                );
        }

        // *******************************************************************

        /// <summary>
        /// This method subscribes an action to the event.
        /// </summary>
        /// <param name="action">The action to use for the operation.</param>
        /// <param name="keepAlive">True to keep the reference alive by maintaining
        /// a reference to the action; false otherwise.</param>
        /// <param name="filter">An optional filter, for the subscrpition.</param>
        /// <returns>A <see cref="ISubscriber"/> to represent the subscription.</returns>
        public ISubscriber Subscribe(
            Action<T> action, 
            bool keepAlive, 
            Predicate<T> filter
            )
        {
            // Validate the parameter before attempting to use them.
            Guard.Instance().ThrowIfNull(action, nameof(action));

            // Create the weak reference.
            var actionReference = new WeakDelegate(
                action, 
                keepAlive
                );

            // Create the filter delegate.
            WeakDelegate filterReference;
            if (filter != null)
            {
                filterReference = new WeakDelegate(
                    filter, 
                    keepAlive
                    );
            }
            else
            {
                filterReference = new WeakDelegate(
                    new Predicate<T>(
                        delegate { return true; }), 
                        true
                        );
            }

            // Create the subscription.
            var subscription = new Subscription<T>(
                actionReference, 
                filterReference
                );
            
            // Add the subscription.
            var token = InternalSubscribe(
                subscription
                );

            // Return the token.
            return token;
        }

        // *******************************************************************

        /// <summary>
        /// This method publishes to any subscriptions for the event.
        /// </summary>
        /// <param name="data">The data associated with the event.</param>
        public void Publish(
            T data
            )
        {
            InternalPublish(data);
        }

        // *******************************************************************

        /// <summary>
        /// This method unsubscribes an action from the event.
        /// </summary>
        /// <param name="action">The action to use for the subscription.</param>
        public void Unsubscribe(
            Action<T> action
            )
        {
            // Validate the parameter before attempting to use them.
            Guard.Instance().ThrowIfNull(action, nameof(action));

            lock (Subscriptions)
            {
                // Look for the subscription.
                var subscription = Subscriptions
                    .Cast<Subscription<T>>() // only want Subscription<T> types.
                    .FirstOrDefault(evt => evt.Action == action);

                // Did we find one?
                if (null != subscription)
                {
                    // Remove the subscription.
                    Subscriptions.Remove(
                        subscription
                        );
                }
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method indicates whether the event contains the specified action.
        /// </summary>
        /// <param name="action">The action to use for the operation.</param>
        /// <returns>True if a match was found; false otherwise.</returns>
        public bool Contains(
            Action<T> action
            )
        {
            // Validate the parameter before attempting to use them.
            Guard.Instance().ThrowIfNull(action, nameof(action));

            lock (Subscriptions)
            {
                // Look for the subscription.
                var subscription = Subscriptions
                    .Cast<Subscription<T>>() // only want Subscription<T> types.
                    .FirstOrDefault(evt => evt.Action == action);

                // Did we find one?
                return subscription != null;
            }            
        }

        #endregion
    }
}
