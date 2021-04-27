using CG.Validations;
using System.Threading;
using System.Threading.Tasks;

namespace CG.Blazor.Services
{
    /// <summary>
    /// This class is a base implementation of the <see cref="ITokenService"/>
    /// interface.
    /// </summary>
    public abstract class TokenService : ITokenService
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains a reference to a token provider.
        /// </summary>
        protected TokenProvider _tokenProvider;

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="TokenService"/>
        /// class.
        /// </summary>
        /// <param name="tokenProvider">The token provider to use with the service.</param>
        public TokenService(
            TokenProvider tokenProvider
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(tokenProvider, nameof(tokenProvider));

            // Save the references.
            _tokenProvider = tokenProvider;
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <inheritdoc />
        public abstract Task<string> EnsureAccessTokenAsync(
            CancellationToken cancellationToken = default
            );

        #endregion
    }
}
