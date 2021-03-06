﻿using QAToolKit.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace QAToolKit.Auth.Keycloak
{
    /// <summary>
    /// Keycloak authenticator to retrieve the AccessToken for a username.
    /// </summary>
    public sealed class KeycloakAuthenticator : IAuthenticationService
    {
        private readonly KeycloakTokenService _keycloakTokenService;

        /// <summary>
        /// Create Keycloak Authenticator instance
        /// </summary>
        /// <param name="options">Keycloak Client credential flow parameters</param>
        public KeycloakAuthenticator(Action<KeycloakOptions> options)
        {
            var keycloakOptions = new KeycloakOptions();
            options?.Invoke(keycloakOptions);

            _keycloakTokenService = new KeycloakTokenService(keycloakOptions);
        }

        /// <summary>
        /// Get access token
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessToken()
        {
            return await _keycloakTokenService.GetAccessTokenAsync();
        }

        /// <summary>
        /// Exchange client credentials token for user token
        /// </summary>
        /// <param name="userName">User name you want the token for</param>
        /// <returns></returns>
        public async Task<string> ExchangeForUserToken(string userName)
        {
            return await _keycloakTokenService.ExchangeTokenForUserToken(userName);
        }
    }
}
