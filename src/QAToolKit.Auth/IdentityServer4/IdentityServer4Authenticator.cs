using QAToolKit.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace QAToolKit.Auth.IdentityServer4
{
    /// <summary>
    /// IdentityServer4 authenticator to retrieve the AccessToken for a username.
    /// </summary>
    public sealed class IdentityServer4Authenticator : IAuthenticationService
    {
        private readonly IdentityServer4TokenService _id4TokenService;

        /// <summary>
        /// Create IdentityServer4 Authenticator instance
        /// </summary>
        /// <param name="options">IdentityServer4 Client credential flow parameters</param>
        public IdentityServer4Authenticator(Action<IdentityServer4Options> options)
        {
            var id4Options = new IdentityServer4Options();
            options?.Invoke(id4Options);

            _id4TokenService = new IdentityServer4TokenService(id4Options);
        }

        /// <summary>
        /// Get access token
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessToken()
        {
            return await _id4TokenService.GetAccessTokenAsync();
        }
    }
}
