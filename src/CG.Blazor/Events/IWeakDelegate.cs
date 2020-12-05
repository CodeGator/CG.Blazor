using System;

namespace CG.Blazor.Events
{
    /// <summary>
    /// This interface represents a delegate that maintains a wek reference
    /// to the associated target object.
    /// </summary>
    public interface IWeakDelegate
    {
        /// <summary>
        /// This property contains a reference to the target delegate.
        /// </summary>
        Delegate Target { get; }
    }
}
