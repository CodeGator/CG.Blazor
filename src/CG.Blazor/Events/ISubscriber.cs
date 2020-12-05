using System;

namespace CG.Blazor.Events
{
    /// <summary>
    /// This interface represents an event subscriber.
    /// </summary>
    public interface ISubscriber
    {
        /// <summary>
        /// This property contains the identifier for the subscriber.
        /// </summary>
        Guid Id { get; }
    }
}
