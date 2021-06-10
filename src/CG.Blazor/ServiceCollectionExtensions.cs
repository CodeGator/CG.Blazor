using CG.Blazor.Services;
using CG.Validations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// This class contains extension methods related to the <see cref="IServiceCollection"/>
    /// type.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method registers a <see cref="TokenProvider"/> service.
        /// </summary>
        /// <param name="serviceCollection">The service collection to use 
        /// for the operation.</param>
        /// <param name="configuration">The configuration to use for the 
        /// operation.</param>
        /// <param name="serviceLifetime">The service lifetime to use for
        /// the operation.</param>
        /// <returns>The value of the <paramref name="serviceCollection"/>
        /// parameter, for chaining calls together.</returns>
        public static IServiceCollection AddTokenProvider(
            this IServiceCollection serviceCollection,
            IConfiguration configuration,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
                .ThrowIfNull(configuration, nameof(configuration));

            // Add a token provider service.
            serviceCollection.Add<TokenProvider>(serviceLifetime);

            // Return the service collection.
            return serviceCollection;
        }

        // *******************************************************************

        /// <summary>
        /// This method registers a <see cref="IStateService"/> service.
        /// </summary>
        /// <param name="serviceCollection">The service collection to use 
        /// for the operation.</param>
        /// <param name="configuration">The configuration to use for the 
        /// operation.</param>
        /// <param name="serviceLifetime">The service lifetime to use for
        /// the operation.</param>
        /// <returns>The value of the <paramref name="serviceCollection"/>
        /// parameter, for chaining calls together.</returns>
        public static IServiceCollection AddStateService(
            this IServiceCollection serviceCollection,
            IConfiguration configuration,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
                .ThrowIfNull(configuration, nameof(configuration));

            // Add a state service.
            serviceCollection.Add<IStateService, StateService>(serviceLifetime);

            // Return the service collection.
            return serviceCollection;
        }

        #endregion
    }
}
