using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CG.Blazor.Services
{
    /// <summary>
    /// This class is a default implementation of the <see cref="IStateService"/>
    /// interface.
    /// </summary>
    public class StateService : DisposableBase, IStateService
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <inheritdoc />
        public IDictionary<string, object> Data { get; }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="StateService"/>
        /// class.
        /// </summary>
        public StateService()
        {
            Data = new ConcurrentDictionary<string, object>();
        }

        #endregion
    }
}
