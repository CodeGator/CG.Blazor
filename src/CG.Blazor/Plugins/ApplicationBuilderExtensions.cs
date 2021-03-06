﻿using CG.Blazor;
using CG.Blazor.Options;
using CG.Blazor.Plugins;
using CG.Blazor.Properties;
using CG.Validations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// This class contains extension methods related to the <see cref="IApplicationBuilder"/>
    /// </summary>
    public static partial class ApplicationBuilderExtensions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method wires up services needed to support Blazor plugins.
        /// </summary>
        /// <param name="applicationBuilder">The application builder to use 
        /// for the operation.</param>
        /// <param name="webHostEnvironment">The hosting environment to use 
        /// for the operation.</param>
        /// <returns>the value of the <paramref name="applicationBuilder"/>
        /// parameter, for chaining method calls together.</returns>
        public static IApplicationBuilder UsePlugins(
            this IApplicationBuilder applicationBuilder,
            IWebHostEnvironment webHostEnvironment
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(applicationBuilder, nameof(applicationBuilder));

            // One of the things we need to do, in order to support static resources
            //   in late-bound, dynamically loaded assemblies, is to create an embedded
            //   file provider for each plugin assembly and wire everything together with
            //   a composite file provider. We'll do that here.

            // Ensure we're setup to use static files.
            applicationBuilder.UseStaticFiles();

            var allProviders = new List<IFileProvider>();

            // Is there already a file provider?
            if (webHostEnvironment.WebRootFileProvider != null)
            {
                // Add the existing file provider.
                allProviders.Add(
                    webHostEnvironment.WebRootFileProvider
                    );
            }

            var asmNameSet = new HashSet<string>();

            // Add providers for any embedded style sheets.
            BuildStyleSheetProviders(
                asmNameSet,
                allProviders
                );

            // Add providers for any embedded scripts.
            BuildScriptProviders(
                asmNameSet,
                allProviders
                );

            // Get the plugin options.
            var options = applicationBuilder.ApplicationServices.GetRequiredService<IOptions<PluginOptions>>();

            // Add any remaining providers.
            BuildRemainingProviders(
                options,
                asmNameSet,
                allProviders
                );

            // Replace the existing file provider with a composite provider.
            webHostEnvironment.WebRootFileProvider = new CompositeFileProvider(
                allProviders
                );

            // The final thing we need to do is walk through the list of modules
            //   and call the Configure method on each one, just in case any of 
            //   them are expecting that to happen.
            foreach (var module in BlazorResources.Modules)
            {
                // Configure any services in the module.
                module.Configure(
                    applicationBuilder,
                    webHostEnvironment
                    );
            }

            // At this point we clear the cached modules.
            BlazorResources.Modules.Clear();

            // Return the application builder.
            return applicationBuilder;
        }

        #endregion

        // *******************************************************************
        // Private methods.
        // *******************************************************************

        #region Private methods

        /// <summary>
        /// This method looks through the script tags in the <see cref="BlazorResources.Scripts"/>
        /// collection and ensures that each plugin has a file provider in the
        /// <paramref name="allProviders"/> collection, to read the static 
        /// resource at runtime.
        /// </summary>
        /// <param name="asmNameSet">The set of all previously processed plugin
        /// assemblies.</param>
        /// <param name="allProviders">The list of all previously added file
        /// providers.</param>
        private static void BuildScriptProviders(
            HashSet<string> asmNameSet,
            List<IFileProvider> allProviders
            )
        {
            // Loop through all the script tags.
            foreach (var resource in BlazorResources.Scripts)
            {
                // We won't check these tags for embedded HTML since that 
                //   was already done in the AddPlugins method.

                // Look for the leading content portion.
                var index1 = resource.IndexOf("_content/");
                if (index1 == -1)
                {
                    // Panic.
                    throw new InvalidOperationException(
                        message: string.Format(
                            Resources.ApplicationBuilderExtensions_MissingContent,
                            resource
                            )
                        );
                }

                // Adjust the index.
                index1 += "_content/".Length;

                // Look for the first '/' character.
                var index2 = resource.IndexOf("/", index1);
                if (index2 == -1)
                {
                    // Panic.
                    throw new InvalidOperationException(
                        message: string.Format(
                            Resources.ApplicationBuilderExtensions_MissingSlash,
                            resource
                            )
                        );
                }

                // Parse out the assembly name.
                var asmName = resource[index1..index2];

                // Have we already created a file provider for this assembly?
                if (asmNameSet.Contains(asmName))
                {
                    continue; // Nothing to do.
                }

                // If we get here then we need to create an embedded file provider
                //   for the plugin assembly.

                Assembly asm = null;
                try
                {
                    // Get the assembly reference.
                    asm = Assembly.Load(
                        asmName
                        );
                }
                catch (FileNotFoundException ex)
                {
                    // Provide better context for the error.
                    throw new FileNotFoundException(
                        message: string.Format(
                            Resources.ApplicationBuilderExtensions_AsmName,
                            asmName
                            ),
                        innerException: ex
                        );
                }

                try
                {
                    // Create a file provider to read embedded resources.
                    var fileProvider = new ManifestEmbeddedFileProviderEx(
                            asm,
                            $"wwwroot"
                            );

                    // Add the provider to the collection.
                    allProviders.Insert(0, fileProvider);
                }
                catch (InvalidOperationException ex)
                {
                    // Provide better context for the error.
                    throw new InvalidOperationException(
                        message: string.Format(
                            Resources.ApplicationBuilderExtensions_WwwRoot,
                            asm.GetName().Name
                            ),
                        innerException: ex
                        );
                }
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method looks through the style sheet links in the <see cref="BlazorResources.StyleSheets"/>
        /// collection and ensures that each plugin has a file provider in the
        /// <paramref name="allProviders"/> collection, to read the static 
        /// resource at runtime.
        /// </summary>
        /// <param name="asmNameSet">The set of all previously processed plugin
        /// assemblies.</param>
        /// <param name="allProviders">The list of all previously added file
        /// providers.</param>
        private static void BuildStyleSheetProviders(
            HashSet<string> asmNameSet,
            List<IFileProvider> allProviders
            )
        {
            // Loop through all the style sheets links.
            foreach (var resource in BlazorResources.StyleSheets)
            {
                // We won't check these tags for embedded HTML since that 
                //   was already done in the AddPlugins method.

                // Look for the leading content portion.
                var index1 = resource.IndexOf("_content/");
                if (index1 == -1)
                {
                    // Panic.
                    throw new InvalidOperationException(
                        message: string.Format(
                            Resources.ApplicationBuilderExtensions_MissingContent,
                            resource
                            )
                        );
                }

                // Adjust the index.
                index1 += "_content/".Length;

                // Look for the first '/' character.
                var index2 = resource.IndexOf("/", index1);
                if (index2 == -1)
                {
                    // Panic.
                    throw new InvalidOperationException(
                        message: string.Format(
                            Resources.ApplicationBuilderExtensions_MissingSlash,
                            resource
                            )
                        );
                }

                // Parse out the assembly name.
                var asmName = resource[index1..index2];

                // Have we already created a file provider for this assembly?
                if (asmNameSet.Contains(asmName))
                {
                    continue; // Nothing to do.
                }

                // If we get here then we need to create an embedded file provider
                //   for the plugin assembly.

                Assembly asm = null;
                try
                {
                    // Get the assembly reference.
                    asm = Assembly.Load(
                        asmName
                        );
                }
                catch (FileNotFoundException ex)
                {
                    // Provide better context for the error.
                    throw new FileNotFoundException(
                        message: string.Format(
                            Resources.ApplicationBuilderExtensions_AsmName,
                            asmName
                            ),
                        innerException: ex
                        );
                }

                try
                {
                    // Create a file provider to read embedded resources.
                    var fileProvider = new ManifestEmbeddedFileProviderEx(
                            asm,
                            $"wwwroot"
                            );

                    // Add the provider to the collection.
                    allProviders.Insert(0, fileProvider);
                }
                catch (InvalidOperationException ex)
                {
                    // Provide better context for the error.
                    throw new InvalidOperationException(
                        message: string.Format(
                            Resources.ApplicationBuilderExtensions_WwwRoot,
                            asm.GetName().Name
                            ),
                        innerException: ex
                        );
                }
            }
        }

        // *******************************************************************

        /// <summary>
        /// This method adds a file provider for any plugin assembly that doesn't
        /// contain links to embedded style sheets, or javascripts. 
        /// </summary>
        /// <param name="options">The options to use for the operation.</param>
        /// <param name="asmNameSet">The set of all previously processed plugin
        /// assemblies.</param>
        /// <param name="allProviders">The list of all previously added file
        /// providers.</param>
        private static void BuildRemainingProviders(
            IOptions<PluginOptions> options,
            HashSet<string> asmNameSet,
            List<IFileProvider> allProviders
            )
        {
            // Loop through all the plugin modules.
            foreach (var module in options.Value.Modules)
            {
                Assembly asm = null;

                // Is this module configured with a path?
                if (module.AssemblyNameOrPath.EndsWith(".dll"))
                {
                    // Strip out just the assembly file name.
                    var fileName = Path.GetFileNameWithoutExtension(
                        module.AssemblyNameOrPath
                        );

                    // Have we already processed this assembly?
                    if (asmNameSet.Contains(fileName))
                    {
                        continue; // Nothing left to do.
                    }

                    // Make an assembly name.
                    var asmName = new AssemblyName(
                        fileName
                        );

                    // Load the assembly by path.
                    asm = Assembly.LoadFrom(
                        module.AssemblyNameOrPath
                        );
                }
                else
                {
                    // Have we already processed this assembly?
                    if (asmNameSet.Contains(module.AssemblyNameOrPath))
                    {
                        continue; // Nothing left to do.
                    }

                    // Make an assembly name.
                    var asmName = new AssemblyName(
                        module.AssemblyNameOrPath
                        );

                    // Load the assembly by name.
                    asm = Assembly.Load(
                        module.AssemblyNameOrPath
                        );
                }

                try
                {
                    // Create a file provider to read embedded resources.
                    var fileProvider = new ManifestEmbeddedFileProviderEx(
                            asm,
                            $"wwwroot"
                            );

                    // Add the provider to the collection.
                    allProviders.Insert(0, fileProvider);
                }
                catch (InvalidOperationException ex)
                {
                    // Provide better context for the error.
                    throw new InvalidOperationException(
                        message: string.Format(
                            Resources.ApplicationBuilderExtensions_WwwRoot,
                            asm.GetName().Name
                            ),
                        innerException: ex
                        );
                }
            }
        }

        #endregion
    }
}
