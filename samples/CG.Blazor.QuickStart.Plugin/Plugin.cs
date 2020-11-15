using CG.Blazor.QuickStart.Plugin.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CG.Blazor.QuickStart.Plugin
{
    public class Plugin : IPlugin
    {
        public void OnInitialize(
            IServiceCollection serviceCollection
            )
        {
            serviceCollection.AddSingleton<SampleService>();
        }
    }
}
