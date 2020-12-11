using CG.Options;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CG.Blazor.Options
{
    /// <summary>
    /// This class represents configuration options for a Blazor plugin module.
    /// </summary>
    public class ModuleOptions : OptionsBase
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the assembly name, or path, for a Blazor plugin.
        /// </summary>
        [Required]
        public string AssemblyNameOrPath { get; set; }

        /// <summary>
        /// This property indicates that the plugin requires routing support,
        /// from Blazor, at runtime.
        /// </summary>
        public bool Routed { get; set; }

        /// <summary>
        /// This property contains an optional list of resources, from the plugin, 
        /// that should be injected into the HTML head section, at runtime.
        /// </summary>
        public IList<string> StyleSheets { get; set; }

        /// <summary>
        /// This property contains an optional list of scripts, from the plugin, 
        /// that should be injected into the HTML head section, at runtime.
        /// </summary>
        public IList<string> Scripts { get; set; }

        /// <summary>
        /// This property contains the name of an entry point for the plugin.
        /// </summary>
        public string EntryPoint { get; set; }

        #endregion
    }
}
