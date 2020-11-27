using CG.Blazor.Plugins;
using CG.Blazor.QuickStart.Plugin.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CG.Blazor.QuickStart.Plugin
{
    public class Module : ModuleBase
    {
        public override void ConfigureServices(
            IServiceCollection serviceCollection, 
            IConfiguration configuration
            )
        {
            serviceCollection.AddSingleton<SampleService>();
        }

        public override void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env
            )
        {
            
        }
    }
}
