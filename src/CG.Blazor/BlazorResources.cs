using System.Collections.Generic;
using System.Reflection;
using System.Text;

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

        #endregion
    }
}
