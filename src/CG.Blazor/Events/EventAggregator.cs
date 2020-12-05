using System;
using System.Collections.Concurrent;

namespace CG.Blazor.Events
{
    /// <summary>
    /// This class is a default implementation of the <see cref="IEventAggregator"/>
    /// interface.
    /// </summary>
    internal class EventAggregator  : IEventAggregator
    {
        // *******************************************************************
        // Fields.
        // *******************************************************************

        #region Fields

        /// <summary>
        /// This field contains a table of events.
        /// </summary>
        private readonly ConcurrentDictionary<Type, EventBase> _events 
            = new ConcurrentDictionary<Type, EventBase>();

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc />
        public virtual TEvent GetEvent<TEvent>() 
            where TEvent : EventBase, new()
        {
            // Look for the event (or create one).
            var theEvent = _events.GetOrAdd(
                typeof(TEvent),
                x => new TEvent()
                ) as TEvent;

            // Return the results.
            return theEvent;
        }

        #endregion
    }
}
