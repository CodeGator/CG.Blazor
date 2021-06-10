using System.Collections.Generic;

namespace CG.Blazor.Services
{
    /// <summary>
    /// This interface represents an object that holds state.
    /// </summary>
    public interface IStateService
    {
        /// <summary>
        /// This property contains a table of named data.
        /// </summary>
        IDictionary<string, object> Data { get; }
    }
}
