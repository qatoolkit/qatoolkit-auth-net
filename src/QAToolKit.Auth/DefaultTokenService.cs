using Newtonsoft.Json.Linq;
using QAToolKit.Auth.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace QAToolKit.Auth
{
    internal abstract class DefaultTokenService
    {
        private const int TokenValidityOffsetSeconds = 15;
        protected readonly HttpClient _client;
        protected readonly Uri _tokenEndpoint;
        protected readonly string _clientId;
        protected readonly string _secret;
        protected readonly string _assemblyName;
        protected readonly string _assemblyVersion;
        
        protected string _clientCredentialsToken = null;
        private DateTimeOffset? _clientCredentialsTokenValidity = null;

        protected string[] _scopes = null;

        internal DefaultTokenService(DefaultOptions defaultOptions)
        {
            _client = new HttpClient();

            _tokenEndpoint = defaultOptions.TokenEndpoint;
            _clientId = defaultOptions.ClientId;
            _secret = defaultOptions.Secret;
            _scopes = defaultOptions.Scopes;

            _assemblyName = typeof(DefaultTokenService).Assembly.GetName().Name;
            _assemblyVersion = typeof(DefaultTokenService).Assembly.GetName().Version.ToString();
        }

        public virtual async Task<string> GetAccessTokenAsync()
        {
            if (IsAccessTokenValid())
            {
                return _clientCredentialsToken;
            }

            await GetClientCredentialsToken();

            return _clientCredentialsToken;
        }

        private async Task GetClientCredentialsToken()
        {
            var request = CreateBasicTokenEndpointRequest();

            if (request == null)
                return;

            var pairs = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _secret)
            };

            if (_scopes != null)
            {
                pairs.Add(new KeyValuePair<string, string>("scope", string.Join(",", _scopes)));
            }

            request.Content = new FormUrlEncodedContent(pairs);

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                dynamic body = JObject.Parse(await response.Content.ReadAsStringAsync());

                _clientCredentialsToken = body.access_token;
                int expiresIn = body.expires_in;

                _clientCredentialsTokenValidity = DateTimeOffset.Now.AddSeconds(expiresIn);

                return;
            }

            throw new UnauthorizedClientException(await response.Content.ReadAsStringAsync());
        }

        private bool IsAccessTokenValid()
        {
            if (_clientCredentialsToken == null || !_clientCredentialsTokenValidity.HasValue)
                return false;

            return _clientCredentialsTokenValidity.Value.AddSeconds(-TokenValidityOffsetSeconds) >= DateTimeOffset.Now;
        }

        protected HttpRequestMessage CreateBasicTokenEndpointRequest()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _tokenEndpoint);

            SetRequestAcceptHeader(request);
            SetRequestUserAgentHeader(request);

            return request;
        }

        private static void SetRequestAcceptHeader(HttpRequestMessage req)
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
