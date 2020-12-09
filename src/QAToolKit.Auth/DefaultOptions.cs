using System;

namespace QAToolKit.Auth
{
    /// <summary>
    /// Default auth options object
    /// </summary>
    public abstract class DefaultOptions
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
        /// Scopes that client has access to
        /// </summary>
        public string[] Scopes { get; set; } = null;

        /// <summary>
        /// Add client credential flow parameters
        /// </summary>
        /// <param name="tokenEndpoint">Keycloak token endpoint</param>
        /// <param name="clientId">Keycloak client ID</param>
        /// <param name="clientSecret">Keycloak client secret</param>
        /// <param name="scopes">Scopes that client has access to</param>
        /// <returns></returns>
        public virtual DefaultOptions AddClientCredentialFlowParameters(Uri tokenEndpoint, string clientId, string clientSecret, string[] scopes = null)
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
            Scopes = scopes;
            return this;
        }
    }
}
