using CG.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CG.Blazor.Options
{
    /// <summary>
    /// This class represents configuration options for Blazor plugins.
    /// </summary>
    public class PluginOptions : OptionsBase
    {
        // *******************************************************************
        // Constants.
        // *******************************************************************

        #region Constants

        /// <summary>
        /// This constant represents the corresponding configuration key for 
        /// these options.
        /// </summary>
        public const string SectionKey = "Plugins";

        #endregion

        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains a list of plugin modules.
        /// </summary>
        [Required]
        public IList<ModuleOptions> Modules { get; set; }

        #endregion
    }
}
