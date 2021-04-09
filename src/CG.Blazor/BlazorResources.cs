using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;

namespace CG.Blazor
{
    /// <summary>
    /// This class utility contains resources that were dynamically gathered 
    /// from various plugins at startup. 
    /// </summary>
    public static class BlazorResources
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains a list of Razor Class Library assemblies that 
        /// require routing support, from Blazor, at runtime.
        /// </summary>
        public static IList<Assembly> RoutedAssemblies { get; } = new List<Assembly>();

        /// <summary>
        /// This property contains a list of stylesheets that are static resources
        /// in a Razor Class Library and must be linked at runtime.
        /// </summary>
        public static IList<string> StyleSheets { get; } = new List<string>();

        /// <summary>
        /// This property contains a list of scripts that are static resources
        /// in a Razor Class Library and must be linked at runtime.
        /// </summary>
        public static IList<string> Scripts { get; } = new List<string>();

        /// <summary>
        /// This property contains a list of external, or 3rd party resources,
        /// consumed by a Razor Class Library, that also need to be linked at 
        /// runtime.
        /// </summary>
        public static IList<string> ExternalResources { get; } = new List<string>();

        /// <summary>
        /// This property contains a temporary list of modules that have been 
        /// loaded by the runtime. This list is populated by the <see cref="ServiceCollectionExtensions.AddPlugins(IServiceCollection, IDataProtector, Microsoft.Extensions.Configuration.IConfiguration)"/>
        /// method and is cleared by the <see cref="ApplicationBuilderExtensions.UsePlugins(Microsoft.AspNetCore.Builder.IApplicationBuilder, Microsoft.AspNetCore.Hosting.IWebHostEnvironment)"/>
        /// method.
        /// </summary>
        internal static IList<IModule> Modules { get; } = new List<IModule>();

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method renders any style sheets in the <see cref="BlazorResources.StyleSheets"/>
        /// collection as a collection of HTML link tags.
        /// </summary>
        /// <returns>An unencoded HTML snippet.</returns>
        public static string RenderStyleSheetLinks()
        {
            var sb = new StringBuilder();
            foreach (var link in StyleSheets)
            {
                sb.Append(link);
                sb.Append(" ");
            }
            var rawHTml = sb.ToString();
            return rawHTml;
        }

        // *******************************************************************

        /// <summary>
        /// This method renders any scripts in the <see cref="BlazorResources.Scripts"/>
        /// collection as a collection of HTML script tags.
        /// </summary>
        /// <returns>An unencoded HTML snippet.</returns>
        public static string RenderScriptTags()
        {
            var sb = new StringBuilder();
            foreach (var tag in Scripts)
            {
                sb.Append(tag);
                sb.Append(" ");
            }
            var rawHTml = sb.ToString();
            return rawHTml;
        }

        // *******************************************************************

        /// <summary>
        /// This method renders any external resource links in the <see cref="BlazorResources.ExternalResources"/>
        /// collection as a collection of HTML link tags.
        /// </summary>
        /// <returns>An unencoded HTML snippet.</returns>
        public static string RenderExternalResources()
        {
            var sb = new StringBuilder();
            foreach (var link in ExternalResources)
            {
                sb.Append(link);
                sb.Append(" ");
            }
            var rawHTml = sb.ToString();
            return rawHTml;
        }

        // *******************************************************************

        /// <summary>
        ///  This method clears any resources contained by this class utility.
        /// </summary>
        public static void Clear()
        {
            BlazorResources.RoutedAssemblies.Clear();
            BlazorResources.Scripts.Clear();
            BlazorResources.StyleSheets.Clear();
            BlazorResources.Modules.Clear();
            BlazorResources.ExternalResources.Clear();
        }

        #endregion
    }
}
