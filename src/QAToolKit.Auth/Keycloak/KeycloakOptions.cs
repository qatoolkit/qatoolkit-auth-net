using System;

namespace QAToolKit.Auth
{
    /// <summary>
    /// Keycloak client credential flow paramters
    /// </summary>
    public class KeycloakOptions
    {
        /// <summary>
        /// Keycloak token endpoint you want to call
        /// </summary>
        public Uri TokenEndpoint { get; set; }
        /// <summary>
        /// Keycloak client ID
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Keycloak client secret
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// Username / email of the user for which you want to retrieve the access token
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// If username is set use impersonation
        /// </summary>
        public bool UseImpersonation { get; private set; } = false;

        /// <summary>
        /// Add client credential flow parameters
        /// </summary>
        /// <param name="tokenEndpoint">Keycloak token endpoint</param>
        /// <param name="clientId">Keycloak client ID</param>
        /// <param name="clientSecret">Keycloak client secret</param>
        /// <returns></returns>
        public KeycloakOptions AddClientCredentialFlowParameters(Uri tokenEndpoint, string clientId, string clientSecret)
        {
            if (tokenEndpoint == null)
                throw new ArgumentNullException($"{nameof(tokenEndpoint)} is null.");
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentNullException($"{nameof(clientId)} is null.");
            if (string.IsNullOrEmpty(clientSecret))
                throw new ArgumentNullException($"{nameof(clientSecret)} is null.");

            TokenEndpoint = tokenEndpoint;
            ClientId = clientId;
            Secret = clientSecret;
            return this;
        }

        /// <summary>
        /// Add username for impersonation
        /// </summary>
        /// <param name="userName">Username / email of the user for which you want to retrieve the access token</param>
        /// <returns></returns>
        public KeycloakOptions AddUserNameForImpersonation(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException($"{nameof(userName)} is null.");

            UserName = userName;
            UseImpersonation = true;
            return this;
        }
    }
}
