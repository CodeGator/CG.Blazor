using Microsoft.Extensions.DependencyInjection;
using System;

namespace CG.Blazor
{
    /// <summary>
    /// This interface represents a Blazor plugin entry point.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// This method is called by the framework when the plugin is 
        /// first loaded, to initialize the plugin. 
        /// </summary>
        /// <param name="serviceCollection">The service collection to 
        /// use for the operation.</param>
        void OnInitialize(
            IServiceCollection serviceCollection
            );
    }
}
