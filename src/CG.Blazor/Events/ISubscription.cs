using System;

namespace CG.Blazor.Events
{
    /// <summary>
    /// This interface represents an event subscription.
    /// </summary>
    public interface ISubscription
    {
        /// <summary>
        /// This property contains a reference to the subscriber.
        /// </summary>
        ISubscriber Subscriber { get; set; }

        /// <summary>
        /// This method returns the event action for the subscription.
        /// </summary>
        /// <returns>An <see cref="Action{Object[]}"/> object.</returns>
        Action<object[]> GetAction();
    }
}
