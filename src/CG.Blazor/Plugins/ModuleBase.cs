using Microsoft.Extensions.DependencyInjection;
using System;

namespace CG.Blazor.Plugins
{
    /// <summary>
    /// This class is a base implementation of the <see cref="IModule"/>
    /// interface.
    /// </summary>
    public abstract class ModuleBase : IModule
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc />
        public abstract void Initialize(
            IServiceCollection serviceCollection
            );

        #endregion
    }
}
