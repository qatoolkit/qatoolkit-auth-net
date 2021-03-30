using NSubstitute;
using QAToolKit.Auth.Keycloak;
using QAToolKit.Core.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QAToolKit.Auth.Test.Keycloak
{
    public class KeycloakAuthenticatorTests
    {
        [Fact]
        public async Task CreateAuthenticatonServiceTest_Success()
        {
            var authenticator = Substitute.For<IAuthenticationService>();
            await authenticator.GetAccessToken();
            Assert.Single(authenticator.ReceivedCalls());
        }

        [Fact]
        public async Task CreateAuthenticatonServiceWithReturnsTest_Success()
        {
            var authenticator = Substitute.For<IAuthenticationService>();
            authenticator.GetAccessToken().Returns(args => "12345");

            Assert.Equal("12345", await authenticator.GetAccessToken());
            Assert.Single(authenticator.ReceivedCalls());
        }

        [Fact]
        public void CreateKeycloakOptionsTest_Success()
        {
            var options = new KeycloakOptions();
            options.AddClientCredentialFlowParameters(new Uri("https://api.com/token"), "12345", "12345");

            var keycloakOptions = Substitute.For<Action<KeycloakOptions>>();
            keycloakOptions.Invoke(options);
            Assert.Single(keycloakOptions.ReceivedCalls());
        }
        
        [Fact]
        public void CreateKeycloakROPCOptionsTest_Success()
        {
            var options = new KeycloakOptions();
            options.AddResourceOwnerPasswordCredentialFlowParameters(new Uri("https://api.com/token"), "12345", "12345",
                "user","pass");

            var keycloakOptions = Substitute.For<Action<KeycloakOptions>>();
            keycloakOptions.Invoke(options);
            Assert.Single(keycloakOptions.ReceivedCalls());
        }
    }
}
