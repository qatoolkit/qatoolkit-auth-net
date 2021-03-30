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
        protected readonly string _userName;
        protected readonly string _password;
        protected readonly string _assemblyName;
        protected readonly string _assemblyVersion;

        protected string _clientCredentialsToken = null;
        private DateTimeOffset? _clientCredentialsTokenValidity = null;

        private DefaultOptions _defaultOptions;

        protected string[] _scopes = null;

        internal DefaultTokenService(DefaultOptions defaultOptions)
        {
            _defaultOptions = defaultOptions;
            _client = new HttpClient();

            _tokenEndpoint = defaultOptions.TokenEndpoint;
            _clientId = defaultOptions.ClientId;
            _secret = defaultOptions.Secret;
            _scopes = defaultOptions.Scopes;
            _userName = defaultOptions.UserName;
            _password = defaultOptions.Password;

            _assemblyName = typeof(DefaultTokenService).Assembly.GetName().Name;
            _assemblyVersion = typeof(DefaultTokenService).Assembly.GetName().Version.ToString();
        }

        public virtual async Task<string> GetAccessTokenAsync()
        {
            if (IsAccessTokenValid())
            {
                return _clientCredentialsToken;
            }

            await GetToken();

            return _clientCredentialsToken;
        }

        private async Task GetToken()
        {
            var request = CreateBasicTokenEndpointRequest();

            if (request == null)
                return;

            request.Content = new FormUrlEncodedContent(PrepareRequestBody());

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

        private List<KeyValuePair<string, string>> PrepareRequestBody(bool useClientSecret = true)
        {
            var pairs = new List<KeyValuePair<string, string>>()
            {
                new("client_id", _clientId)
            };
            
            switch (_defaultOptions.FlowType)
            {
                case FlowType.ResourceOwnerPasswordCredentialFlow:
                    pairs.Add(new KeyValuePair<string, string>("grant_type", "password"));
                    pairs.Add(new KeyValuePair<string, string>("username", _userName));
                    pairs.Add(new KeyValuePair<string, string>("password", _password));
                    break;
                case FlowType.ClientCredentialFlow:
                    pairs.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                    break;
            }

            if (useClientSecret)
            {
                pairs.Add(new KeyValuePair<string, string>("client_secret", _secret));
            }

            if (_scopes != null)
            {
                pairs.Add(new KeyValuePair<string, string>("scope", string.Join(",", _scopes)));
            }

            return pairs;
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