using QAToolKit.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace QAToolKit.Auth.AzureB2C
{
    /// <summary>
    /// AzureB2C authenticator to retrieve the AccessToken for a username.
    /// </summary>
    public sealed class AzureB2CAuthenticator : IAuthenticationService
    {
        private readonly AzureB2CTokenService _azureB2CTokenService;

        /// <summary>
        /// Create AzureB2C Authenticator instance.
        /// </summary>
        /// <param name="options">AzureB2C Client credential flow parameters</param>
        public AzureB2CAuthenticator(Action<AzureB2COptions> options)
        {
            var azureB2COptions = new AzureB2COptions();
            options?.Invoke(azureB2COptions);

            _azureB2CTokenService = new AzureB2CTokenService(azureB2COptions);
        }

        /// <summary>
        /// Get Azure B2C access token.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessToken()
        {
            return await _azureB2CTokenService.GetAccessTokenAsync();
        }
    }
}
