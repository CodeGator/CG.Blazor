using System;

namespace CG.Blazor.Events
{
    /// <summary>
    /// This interface represents an object that sends and receives events.
    /// </summary>
    public interface IEventAggregator 
    {
        /// <summary>
        /// This method returns an event of the specified type.
        /// </summary>
        /// <typeparam name="TEvent">The type of event</typeparam>
        /// <returns>An event of type <typeparamref name="TEvent"/></returns>
        TEvent GetEvent<TEvent>() 
            where TEvent : EventBase, new();
    }
}
