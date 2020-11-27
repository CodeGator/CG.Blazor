using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CG.Blazor.Plugins
{
    /// <summary>
    /// This class is a base implementation of the <see cref="IModule"/> interface.
    /// </summary>
    public abstract class ModuleBase : IModule
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc />
        public abstract void ConfigureServices(
            IServiceCollection serviceCollection,
            IConfiguration configuration
            );

        /// <inheritdoc />
        public abstract void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env
            );

        #endregion
    }
}
