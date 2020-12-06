using Newtonsoft.Json.Linq;
using QAToolKit.Auth.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace QAToolKit.Auth
{
    internal class KeycloakTokenService : IDisposable
    {
        private const int TokenValidityOffsetSeconds = 15;
        private readonly HttpClient _client;
        private readonly Uri _tokenEndpoint;
        private readonly string _clientId;
        private readonly string _secret;
        private string _accessToken = null;
        private DateTimeOffset? _accessTokenValidity = null;
        private readonly string _assemblyName;
        private readonly string _assemblyVersion;
        private readonly string _impersonatedUsername;

        public KeycloakTokenService(KeycloakOptions keycloakOptions)
        {
            _client = new HttpClient();

            _tokenEndpoint = keycloakOptions.TokenEndpoint;
            _clientId = keycloakOptions.ClientId;
            _secret = keycloakOptions.Secret;

            _assemblyName = typeof(KeycloakTokenService).Assembly.GetName().Name;
            _assemblyVersion = typeof(KeycloakTokenService).Assembly.GetName().Version.ToString();
            _impersonatedUsername = keycloakOptions.UserName;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (IsAccessTokenValid())
            {
                return _accessToken;
            }

            await PostTokenClientCredentials();

            return _accessToken;
        }

        private async Task PostTokenClientCredentials()
        {
            var request = CreateBasicTokenEndpointRequest();

            if (request == null)
                return;

            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _secret)
            });

            var now = DateTimeOffset.Now;

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                dynamic body = JObject.Parse(await response.Content.ReadAsStringAsync());

                if (string.IsNullOrEmpty(_impersonatedUsername))
                {
                    string at = body.access_token;
                    int ei = body.expires_in;

                    _accessToken = at;
                    _accessTokenValidity = now.AddSeconds(ei);
                }
                else
                {
                    var impersonatedTokenRequest = CreateBasicTokenEndpointRequest();

                    if (impersonatedTokenRequest == null)
                        return;

                    impersonatedTokenRequest.Content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("client_id", _clientId),
                        new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:token-exchange"),
                        new KeyValuePair<string, string>("subject_token", body.access_token.ToString()),
                        new KeyValuePair<string, string>("requested_subject", _impersonatedUsername),
                        new KeyValuePair<string, string>("client_secret", _secret)
                    });

                    var impersonatedTokenResponse = await _client.SendAsync(impersonatedTokenRequest);

                    var contentStr = await impersonatedTokenResponse.Content.ReadAsStringAsync();

                    if (impersonatedTokenResponse.IsSuccessStatusCode)
                    {
                        if (!string.IsNullOrEmpty(contentStr))
                        {
                            dynamic content = JObject.Parse(contentStr);

                            string at = content.access_token;
                            int ei = content.expires_in;

                            _accessToken = at;
                            _accessTokenValidity = now.AddSeconds(ei);
                        }
                        else
                        {
                            throw new KeycloakException(contentStr);
                        }
                    }
                    else
                    {
                        throw new KeycloakAccessDeniedException(contentStr);
                    }
                }
            }
            else
            {
                throw new KeycloakUnauthorizedClientException(await response.Content.ReadAsStringAsync());
            }
        }

        private bool IsAccessTokenValid(DateTimeOffset? now = null)
        {
            if (_accessToken == null || !_accessTokenValidity.HasValue)
                return false;

            if (!now.HasValue)
                now = DateTimeOffset.Now;

            return _accessTokenValidity.Value.AddSeconds(-TokenValidityOffsetSeconds) >= now;
        }

        private HttpRequestMessage CreateBasicTokenEndpointRequest()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _tokenEndpoint);

            SetRequestAcceptHeader(request);
            SetRequestUserAgentHeader(request);

            return request;
        }

        private void SetRequestAcceptHeader(HttpRequestMessage req)
        {
            req.Headers.Add("Accept", "application/json");
        }

        private void SetRequestUserAgentHeader(HttpRequestMessage req)
        {
            req.Headers.Add("User-Agent", $"{_assemblyName}/{_assemblyVersion}");
        }

        /// <summary>
        /// Dispose the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose object
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            _client?.Dispose();
        }
    }
}
