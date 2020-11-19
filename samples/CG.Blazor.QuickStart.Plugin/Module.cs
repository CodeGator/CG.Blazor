using CG.Blazor.QuickStart.Plugin.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CG.Blazor.QuickStart.Plugin
{
    public class Module : IModule
    {
        public void Initialize(
            IServiceCollection serviceCollection
            )
        {
            serviceCollection.AddSingleton<SampleService>();
        }
    }
}
