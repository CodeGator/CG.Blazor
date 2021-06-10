using CG.Validations;
using Microsoft.Extensions.Logging;
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

        /// <summary>
        /// This property contains a reference to a logger.
        /// </summary>
        protected ILogger<StateService> Logger { get; }

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="StateService"/>
        /// class.
        /// </summary>
        /// <param name="logger">The logger to use with the service.</param>
        public StateService(
            ILogger<StateService> logger
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(logger, nameof(logger));

            // Ssave the references.
            Logger = logger;
            Data = new ConcurrentDictionary<string, object>();
        }

        #endregion
    }
}
