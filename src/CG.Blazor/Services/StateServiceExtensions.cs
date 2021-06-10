using CG.Validations;
using System;
using System.Collections.Concurrent;

namespace CG.Blazor.Services
{
    /// <summary>
    /// This class contains extension methods related to the <see cref="IStateService"/>
    /// type.
    /// </summary>
    public static class StateServiceExtensions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method sets a named value in the state.
        /// </summary>
        /// <typeparam name="T">The type of associated value.</typeparam>
        /// <param name="state">The state service to use for the operation.</param>
        /// <param name="name">The name to use for the operation.</param>
        /// <param name="value">The value to use for the operation.</param>
        public static void SetValueByName<T>(
            this IStateService state,
            string name,
            T value
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(state, nameof(state))
                .ThrowIfNullOrEmpty(name, nameof(name));

            // Set the data.
            (state.Data as ConcurrentDictionary<string, object>).AddOrUpdate(
                name,
                value,
                (n, v) => value
                );
        }

        // *******************************************************************

        /// <summary>
        /// This method sets a named string value in the state.
        /// </summary>
        /// <typeparam name="T">The type of associated value.</typeparam>
        /// <param name="state">The state service to use for the operation.</param>
        /// <param name="name">The name to use for the operation.</param>
        /// <param name="defaultValue">The default value to use if the named
        /// value doesn't yet exist.</param>
        /// <returns>The value associated with the name, or the default value,
        /// if the value doesn't yet exist.</returns>
        public static T GetValueByName<T>(
            this IStateService state,
            string name,
            T defaultValue = default
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(state, nameof(state));

            // Get the data.
            return (T)(state.Data as ConcurrentDictionary<string, object>).GetOrAdd(
                name,
                defaultValue
                );
        }

        #endregion
    }
}
