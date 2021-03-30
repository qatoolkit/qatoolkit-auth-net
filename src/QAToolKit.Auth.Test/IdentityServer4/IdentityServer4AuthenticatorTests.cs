using NSubstitute;
using QAToolKit.Auth.IdentityServer4;
using QAToolKit.Core.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace QAToolKit.Auth.Test.IdentityServer4
{
    public class IdentityServer4AuthenticatorTests
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
        public void CreateIdentityServer4OptionsTest_Success()
        {
            var options = new IdentityServer4Options();
            options.AddClientCredentialFlowParameters(new Uri("https://api.com/token"), "12345", "12345");

            var id4Options = Substitute.For<Action<IdentityServer4Options>>();
            id4Options.Invoke(options);
            Assert.Single(id4Options.ReceivedCalls());
        }
        
        [Fact]
        public void CreateIdentityServer4ROPCOptionsTest_Success()
        {
            var options = new IdentityServer4Options();
            options.AddResourceOwnerPasswordCredentialFlowParameters(new Uri("https://api.com/token"), "12345", "12345",
                "user", "pass");

            var id4Options = Substitute.For<Action<IdentityServer4Options>>();
            id4Options.Invoke(options);
            Assert.Single(id4Options.ReceivedCalls());
        }
    }
}
