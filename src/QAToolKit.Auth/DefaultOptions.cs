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
        public Uri TokenEndpoint { get; private set; }
        /// <summary>
        /// Keycloak client ID
        /// </summary>
        public string ClientId { get; private set; }
        /// <summary>
        /// Keycloak client secret
        /// </summary>
        public string Secret { get; private set; }
        /// <summary>
        /// Scopes that client has access to
        /// </summary>
        public string[] Scopes { get; private set; } = null;
        /// <summary>
        /// Username for ROPC flow
        /// </summary>
        public string UserName { get; private set; }
        /// <summary>
        /// User password for ROPC flow
        /// </summary>
        public string Password { get; private set; }
        /// <summary>
        /// Oauth2 flow type
        /// </summary>
        public FlowType FlowType { get; private set; }

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
            UserName = null;
            Password = null;
            FlowType = FlowType.ClientCredentialFlow;
            return this;
        }
        
        /// <summary>
        /// Add resource owner password credential flow parameters
        /// </summary>
        /// <param name="tokenEndpoint">Keycloak token endpoint</param>
        /// <param name="clientId">Keycloak client ID</param>
        /// <param name="clientSecret">Keycloak client secret</param>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="scopes">Scopes that client has access to</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual DefaultOptions AddResourceOwnerPasswordCredentialFlowParameters(Uri tokenEndpoint, string clientId, string clientSecret,
            string userName, string password, string[] scopes = null)
        {
            if (tokenEndpoint == null)
                throw new ArgumentNullException($"{nameof(tokenEndpoint)} is null.");
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentNullException($"{nameof(clientId)} is null.");
            if (string.IsNullOrEmpty(clientSecret))
                throw new ArgumentNullException($"{nameof(clientSecret)} is null.");
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException($"{nameof(userName)} is null.");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException($"{nameof(password)} is null.");
            
            TokenEndpoint = tokenEndpoint;
            ClientId = clientId;
            Secret = clientSecret;
            Scopes = scopes;
            UserName = userName;
            Password = password;
            FlowType = FlowType.ResourceOwnerPasswordCredentialFlow;
            return this;
        }
    }
}
