using Microsoft.Extensions.Logging;
using System;
using Xunit;
using Xunit.Abstractions;

namespace QAToolKit.Auth.Test.Keycloak
{
    public class KeycloakOptionsTests
    {
        private readonly ILogger<KeycloakOptionsTests> _logger;

        public KeycloakOptionsTests(ITestOutputHelper testOutputHelper)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
            _logger = loggerFactory.CreateLogger<KeycloakOptionsTests>();
        }

        [Fact]
        public void KeycloakOptionsTest_Successful()
        {
            var options = new KeycloakOptions();
            options.AddClientCredentialFlowParameters(new Uri("https://api.com/token"), "12345", "12345");
            options.AddUserNameForImpersonation("myemail@email.com");

            Assert.Equal("myemail@email.com", options.UserName);
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
            var options = new KeycloakOptions();
            Assert.Throws<ArgumentNullException>(() => options.AddClientCredentialFlowParameters(null, clientId, clientSecret));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData(null, "test")]
        [InlineData("test", null)]
        public void KeycloakOptionsWrongUriTest_Fails(string clientId, string clientSecret)
        {
            var options = new KeycloakOptions();
            Assert.Throws<UriFormatException>(() => options.AddClientCredentialFlowParameters(new Uri("https"), clientId, clientSecret));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData(null, "test")]
        [InlineData("test", null)]
        public void KeycloakOptionsCorrectUriTest_Fails(string clientId, string clientSecret)
        {
            var options = new KeycloakOptions();
            Assert.Throws<ArgumentNullException>(() => options.AddClientCredentialFlowParameters(new Uri("https://localhost/token"), clientId, clientSecret));
        }
    }
}
