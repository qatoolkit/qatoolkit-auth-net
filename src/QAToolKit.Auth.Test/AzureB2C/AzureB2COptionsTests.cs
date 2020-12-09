using Microsoft.Extensions.Logging;
using QAToolKit.Auth.AzureB2C;
using System;
using Xunit;
using Xunit.Abstractions;

namespace QAToolKit.Auth.Test.AzureB2C
{
    public class AzureB2COptionsTests
    {
        private readonly ILogger<AzureB2COptionsTests> _logger;

        public AzureB2COptionsTests(ITestOutputHelper testOutputHelper)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
            _logger = loggerFactory.CreateLogger<AzureB2COptionsTests>();
        }

        [Fact]
        public void KeycloakOptionsTest_Successful()
        {
            var options = new AzureB2COptions();
            options.AddClientCredentialFlowParameters(new Uri("https://api.com/token"), "12345", "12345");

            Assert.Equal("12345", options.ClientId);
            Assert.Equal("12345", options.Secret);
            Assert.Equal(new Uri("https://api.com/token"), options.TokenEndpoint);
        }

        [Fact]
        public void KeycloakOptionsNoImpersonationTest_Successful()
        {
            var options = new AzureB2COptions();
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
            var options = new AzureB2COptions();
            Assert.Throws<ArgumentNullException>(() => options.AddClientCredentialFlowParameters(null, clientId, clientSecret));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData(null, "test")]
        [InlineData("test", null)]
        public void KeycloakOptionsWrongUriTest_Fails(string clientId, string clientSecret)
        {
            var options = new AzureB2COptions();
            Assert.Throws<UriFormatException>(() => options.AddClientCredentialFlowParameters(new Uri("https"), clientId, clientSecret));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData(null, "test")]
        [InlineData("test", null)]
        public void KeycloakOptionsCorrectUriTest_Fails(string clientId, string clientSecret)
        {
            var options = new AzureB2COptions();
            Assert.Throws<ArgumentNullException>(() => options.AddClientCredentialFlowParameters(new Uri("https://localhost/token"), clientId, clientSecret));
        }
    }
}
