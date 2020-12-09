using Microsoft.Extensions.Logging;
using QAToolKit.Auth.IdentityServer4;
using System;
using Xunit;
using Xunit.Abstractions;

namespace QAToolKit.Auth.Test.IdentityServer4
{
    public class IdentityServer4OptionsTests
    {
        private readonly ILogger<IdentityServer4OptionsTests> _logger;

        public IdentityServer4OptionsTests(ITestOutputHelper testOutputHelper)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
            _logger = loggerFactory.CreateLogger<IdentityServer4OptionsTests>();
        }

        [Fact]
        public void KeycloakOptionsTest_Successful()
        {
            var options = new IdentityServer4Options();
            options.AddClientCredentialFlowParameters(new Uri("https://api.com/token"), "12345", "12345");

            Assert.Equal("12345", options.ClientId);
            Assert.Equal("12345", options.Secret);
            Assert.Equal(new Uri("https://api.com/token"), options.TokenEndpoint);
        }

        [Fact]
        public void KeycloakOptionsNoImpersonationTest_Successful()
        {
            var options = new IdentityServer4Options();
            options.AddClientCredentialFlowParameters(new Uri("https://api.com/token"), "12345", "12345");

            Assert.Equal("12345", options.ClientId);
            Assert.Equal("12345", options.Secret);
            Assert.Equal(new Uri("https://api.com/token"), options.TokenEndpoint);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData(null, "test")]
        [InlineData("test", null)]
        public void KeycloakOptionsUriNullTest_Fails(string clientId, string clientSecret)
        {
            var options = new IdentityServer4Options();
            Assert.Throws<ArgumentNullException>(() => options.AddClientCredentialFlowParameters(null, clientId, clientSecret));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData(null, "test")]
        [InlineData("test", null)]
        public void KeycloakOptionsWrongUriTest_Fails(string clientId, string clientSecret)
        {
            var options = new IdentityServer4Options();
            Assert.Throws<UriFormatException>(() => options.AddClientCredentialFlowParameters(new Uri("https"), clientId, clientSecret));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData(null, "test")]
        [InlineData("test", null)]
        public void KeycloakOptionsCorrectUriTest_Fails(string clientId, string clientSecret)
        {
            var options = new IdentityServer4Options();
            Assert.Throws<ArgumentNullException>(() => options.AddClientCredentialFlowParameters(new Uri("https://localhost/token"), clientId, clientSecret));
        }
    }
}
