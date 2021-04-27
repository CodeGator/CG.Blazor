using System.Threading;
using System.Threading.Tasks;

namespace CG.Blazor.Services
{
    /// <summary>
    /// This interface represents an object that manages IdentityServer4 
    /// access tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// This method ensures the returned access token is always current.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The refreshed HTTP access token.</returns>
        Task<string> EnsureAccessTokenAsync(
            CancellationToken cancellationToken = default
            );
    }
}
