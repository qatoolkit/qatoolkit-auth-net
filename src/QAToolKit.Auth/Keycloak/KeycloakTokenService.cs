using Newtonsoft.Json.Linq;
using QAToolKit.Auth.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace QAToolKit.Auth.Keycloak
{
    internal class KeycloakTokenService : DefaultTokenService
    {
        public KeycloakTokenService(KeycloakOptions keycloakOptions) : base(keycloakOptions)
        { }

        internal async Task<string> ExchangeTokenForUserToken(string userName)
        {
            var impersonatedTokenRequest = CreateBasicTokenEndpointRequest();

            if (impersonatedTokenRequest == null)
                throw new ArgumentNullException($"{impersonatedTokenRequest} is null.");

            impersonatedTokenRequest.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:token-exchange"),
                new KeyValuePair<string, string>("subject_token", _clientCredentialsToken),
                new KeyValuePair<string, string>("requested_subject", userName),
                new KeyValuePair<string, string>("client_secret", _secret)
            });

            var impersonatedTokenResponse = await _client.SendAsync(impersonatedTokenRequest);

            var contentStr = await impersonatedTokenResponse.Content.ReadAsStringAsync();

            if (impersonatedTokenResponse.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(contentStr))
                {
                    dynamic content = JObject.Parse(contentStr);

                    return content.access_token;
                }
                else
                {
                    throw new AuthenticationException(contentStr);
                }
            }
            else
            {
                throw new AccessDeniedException(contentStr);
            }
        }
    }
}
