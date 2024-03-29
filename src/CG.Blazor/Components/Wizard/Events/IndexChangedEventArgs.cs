﻿using System;

namespace CG.Blazor.Components
{
    /// <summary>
    /// This class contains arguments for the OnIndexChanged event.
    /// </summary>
    public class IndexChangedEventArgs
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property indicates the new index.
        /// </summary>
        public int? NewIndex { get; set; }

        /// <summary>
        /// This property indicates the current index.
        /// </summary>
        public int? CurrentIndex { get; internal set; }

        #endregion
    }
}
