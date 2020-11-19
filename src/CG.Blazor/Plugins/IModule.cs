using Microsoft.Extensions.DependencyInjection;
using System;

namespace CG.Blazor
{
    /// <summary>
    /// This interface represents a Blazor plugin module.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// This method is called by the framework when the module is 
        /// first loaded, to initialize the plugin. 
        /// </summary>
        /// <param name="serviceCollection">The service collection to 
        /// use for the operation.</param>
        void Initialize(
            IServiceCollection serviceCollection
            );
    }
}
