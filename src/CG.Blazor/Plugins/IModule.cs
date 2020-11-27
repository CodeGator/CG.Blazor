using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
        /// first loaded, to configure the services within the plugin. 
        /// </summary>
        /// <param name="serviceCollection">The service collection to 
        /// use for the operation.</param>
        /// <param name="configuration">The configuration to use for the
        /// operation.</param>
        void ConfigureServices(
            IServiceCollection serviceCollection,
            IConfiguration configuration
            );

        /// <summary>
        /// This method is called by the framework when the module is first
        /// loaded, to configure the logic within the plugin
        /// </summary>
        /// <param name="app">The application builder to use for the operation.</param>
        /// <param name="env">The environment to use for the operation.</param>
        void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env
            );
    }
}
