using CG;
using CG.Blazor;
using CG.Blazor.Options;
using CG.Blazor.Properties;
using CG.Blazor.ViewModels;
using CG.Runtime;
using CG.Validations;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// This class contains extension methods related to the <see cref="IServiceCollection"/>
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method adds any custom view-model types to the specified service 
        /// collection object as scoped services.
        /// </summary>
        /// <param name="serviceCollection">The service collection to use for
        /// the operation.</param>
        /// <param name="assemblyWhiteList">An optional white list, for filtering
        /// the assemblies used in the operation.
        /// <param name="assemblyBlackList">An optional black list, for filtering
        /// the assemblies used in the operation.
        /// <returns>The value of the <paramref name="serviceCollection"/>
        /// parameter, for chaining calls together.</returns>
        /// <remarks>
        /// This idea, with this method, is to dynamically locate and register
        /// any concrete view-model types along with their corresponding service
        /// types. This way, we avoid having view-model registration turn into a 
        /// maintenance issue, and (hopefully) the whole process becomes a little 
        /// more testable since we can swap out VM instance(s) in our unit test 
        /// fixtures.
        /// </remarks>
        public static IServiceCollection AddViewModels(
            this IServiceCollection serviceCollection,
            string assemblyWhiteList = "",
            string assemblyBlackList = "Microsoft.*,System.*,netstandard"
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection));

            // Find all concrete types that derive from ViewModeBase.
            var impTypes = typeof(ViewModelBase).DerivedTypes(
                assemblyWhiteList,
                assemblyBlackList
                );

            // Loop through any types we found.
            foreach (var impType in impTypes)
            {
                // Look for a corresponding service interface type, which, if
                //   there is one, should be implemented by the type and derive 
                //   from the IViewModel interface.

                var serviceType = impType.FindInterfaces((x, y) =>
                {
                    // Look through all the types.
                    foreach (var z in y as Type[])
                    {
                        // Ignore same type.
                        if (z == x)
                        {
                            continue;
                        }

                        // Watch for assignable types (think inheritence).
                        if (z.IsAssignableFrom(x))
                        {
                            return true;
                        }
                    }

                    // Nothing found.
                    return false;
                },
                new[] { typeof(IViewModel) }
                ).FirstOrDefault();

                // Register the view-model type, with or without an associated 
                //   service type, depending on the previous search operation.

                // Did we find a service type?
                if (null != serviceType)
                {
                    // Register the view-model type with the service type.
                    serviceCollection.AddScoped(
                        serviceType,
                        impType
                        );
                }
                else
                {
                    // Otherwise, as a fall-back, register the view-model alone, 
                    //   without a corresponding service type.
                    serviceCollection.AddScoped(
                        impType
                        );
                }
            }

            // Return the service collection.
            return serviceCollection;
        }

        #endregion
    }
}
