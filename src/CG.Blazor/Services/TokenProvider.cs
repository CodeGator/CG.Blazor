using System;

namespace CG.Blazor.Services
{
    /// <summary>
    /// This class caches HTTP tokens to make them easily accessable, from Blazor.
    /// </summary>
    public class TokenProvider
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains a cross-reference forgery validation token.
        /// </summary>
        public string XsrfToken { get; set; }

        /// <summary>
        /// This property contains a bearer access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// This property contains a refresh token.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// This property indicates how long until the access token expires.
        /// </summary>
        public DateTimeOffset ExpiresAt { get; set; }

        #endregion
    }
}
