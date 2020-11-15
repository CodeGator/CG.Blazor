﻿using CG;
using CG.Blazor;
using CG.Blazor.Options;
using CG.Blazor.ViewModels;
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

        // *******************************************************************

        /// <summary>
        /// This method adds any configured plugins to the specified service
        /// collection. It also registers any resources from the plugins, with 
        /// Blazor. It also makes the Blazor router aware of any components or
        /// pages that require runtime routing support.
        /// </summary>
        /// <param name="serviceCollection">The service collection to use for 
        /// the operation.</param>
        /// <param name="configuration">The configuration to use for the operation.</param>
        /// <returns>The value of the <paramref name="serviceCollection"/>
        /// parameter, for chaining calls together.</returns>
        /// <remarks>
        /// <para>
        /// The idea, with this method, is to borrow a concept from Microsoft's
        /// PRISM library and dynamically load assemblies that contain pages, 
        /// components, classes, etc. The end result, hopefully, is a more modular 
        /// experience than is possible with vanilla Blazor.
        /// </para>
        /// </remarks>
        public static IServiceCollection AddPlugins(
            this IServiceCollection serviceCollection,
            IConfiguration configuration
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(serviceCollection, nameof(serviceCollection))
                .ThrowIfNull(configuration, nameof(configuration));

            // Clear any old blazor resources.
            BlazorResources.RoutedAssemblies.Clear();
            BlazorResources.Scripts.Clear();
            BlazorResources.StyleSheets.Clear();

            // Configure the plugin options.
            serviceCollection.ConfigureOptions<PluginOptions>(
                configuration.GetSection(PluginOptions.SectionKey),
                out var pluginOptions
                );

            var asmNameSet = new HashSet<string>();

            // Loop through the modules (ignoring any duplicates).
            foreach (var module in pluginOptions.Modules)
            {
                Assembly asm = null;

                // Is the assembly property a file path?
                if (module.Assembly.EndsWith(".dll"))
                {
                    // Load the assembly by path.
                    asm = Assembly.LoadFrom(module.Assembly);
                }
                else
                {
                    // Load the assembly by name.
                    asm = Assembly.Load(module.Assembly);
                }                

                // Have we already processed this plugin assembly?
                if (asmNameSet.Contains(asm.GetName().Name))
                {
                    continue; // Nothing to do.
                }

                // Does the module require Blazor routing support?
                if (module.Routed)
                {
                    // Remember the assembly on behalf of Blazor.
                    BlazorResources.RoutedAssemblies.Add(asm);
                }

                // Get the static resources from the assembly.
                var staticResourceNames = asm.GetManifestResourceNames();

                // Add links for any embedded style sheets.
                BuildStyleSheetLinks(
                    asm,
                    staticResourceNames,
                    module
                    );

                // Add tags for any embedded scripts.
                BuildScriptTags(
                    asm,
                    staticResourceNames,
                    module
                    );
                
                // Is there an entry point?
                if (false == string.IsNullOrEmpty(module.EntryPoint))
                {
                    // Try to load the type.
                    var type = asm.GetType(
                        module.EntryPoint,
                        true // <-- throw exception on fail.
                        );

                    // Try to create an instance.
                    var plugin = Activator.CreateInstance(
                        type
                        ) as IPlugin;

                    // Did we succeed?
                    if (null != plugin)
                    {
                        // Initialize the plugin.
                        plugin.OnInitialize(
                            serviceCollection
                            );
                    }
                }
            }

            // Return the service collection.
            return serviceCollection;
        }

        #endregion

        // *******************************************************************
        // Private methods.
        // *******************************************************************

        #region Private methods

        /// <summary>
        /// This method builds tags for embedded scripts.
        /// </summary>
        /// <param name="asm">The assembly to use for the operation.</param>
        /// <param name="staticResourceNames">The static resources avaiable in the assmbly.</param>
        /// <param name="module">The options for the module.</param>
        private static void BuildScriptTags(
            Assembly asm,
            string[] staticResourceNames,
            ModuleOptions module
            )
        {
            // Does the module contain scripts?
            if (null != module.Scripts)
            {
                // Loop through all the scripts.
                foreach (var resource in module.Scripts)
                {
                    // Check for embedded html in the path.
                    if (resource.IsHTML())
                    {
                        // Panic!
                        throw new InvalidOperationException(
                            message: $"It appears the script path '{resource}' contains HTML. HTML is not allowed in the path."
                            );
                    }

                    // Format a script tag and save it.
                    if (resource.StartsWith('/'))
                    {
                        // Check for the resource in the assembly.
                        if (staticResourceNames.Contains($"{asm.GetName().Name}.wwwroot.{resource.Substring(1)}"))
                        {
                            // Panic!
                            throw new InvalidOperationException(
                                message: $"It appears the script '{resource}' is not an embedded resource in assembly '{asm.GetName().Name}'!"
                                );
                        }

                        // Add the link.
                        BlazorResources.Scripts.Add(
                            $"<script src=\"_content/{asm.GetName().Name}{resource}\"></script>"
                            );
                    }
                    else
                    {
                        // Check for the resource in the assembly.
                        if (staticResourceNames.Contains($"{asm.GetName().Name}.wwwroot.{resource}"))
                        {
                            // Panic!
                            throw new InvalidOperationException(
                                message: $"It appears the script '{resource}' is not an embedded resource in assembly '{asm.GetName().Name}'!"
                                );
                        }

                        // Add the link.
                        BlazorResources.Scripts.Add(
                            $"<script src=\"_content/{asm.GetName().Name}/{resource}\"></script>"
                            );
                    }
                }
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method builds links for embedded style sheets.
        /// </summary>
        /// <param name="asm">The assembly to use for the operation.</param>
        /// <param name="staticResourceNames">The static resources avaiable in the assmbly.</param>
        /// <param name="module">The options for the module.</param>
        private static void BuildStyleSheetLinks(
            Assembly asm,
            string[] staticResourceNames,
            ModuleOptions module
            )
        {
            // Does the module contain stylesheets?
            if (null != module.StyleSheets)
            {
                // Loop through all the style sheets.
                foreach (var resource in module.StyleSheets)
                {
                    // Check for embedded html in the path.
                    if (resource.IsHTML())
                    {
                        // Panic!
                        throw new InvalidOperationException(
                            message: $"It appears the style sheet path '{resource}' contains HTML. HTML is not allowed in the path."
                            );
                    }

                    // Format a link and save it.
                    if (resource.StartsWith('/'))
                    {
                        // Check for the resource in the assembly.
                        if (staticResourceNames.Contains($"{asm.GetName().Name}.wwwroot.{resource.Substring(1)}"))
                        {
                            // Panic!
                            throw new InvalidOperationException(
                                message: $"It appears the style sheet '{resource}' is not an embedded resource in assembly '{asm.GetName().Name}'!"
                                );
                        }

                        // Add the link.
                        BlazorResources.StyleSheets.Add(
                            $"<link rel=\"stylesheet\" href=\"_content/{asm.GetName().Name}{resource}\" />"
                            );
                    }
                    else
                    {
                        // Check for the resource in the assembly.
                        if (staticResourceNames.Contains($"{asm.GetName().Name}.wwwroot.{resource}"))
                        {
                            // Panic!
                            throw new InvalidOperationException(
                                message: $"It appears the style sheet '{resource}' is not an embedded resource in assembly '{asm.GetName().Name}'!"
                                );
                        }

                        // Add the link.
                        BlazorResources.StyleSheets.Add(
                            $"<link rel=\"stylesheet\" href=\"_content/{asm.GetName().Name}/{resource}\" />"
                            );
                    }
                }
            }
        }

        #endregion
    }
}
