using NSubstitute;
using QAToolKit.Auth.AzureB2C;
using QAToolKit.Core.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace QAToolKit.Auth.Test.AzureB2C
{
    public class AzureB2CAuthenticatorTests
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
        public void CreateAzureB2COptionsTest_Success()
        {
            var options = new AzureB2COptions();
            options.AddClientCredentialFlowParameters(new Uri("https://api.com/token"), "12345", "12345");

            var azureB2COptions = Substitute.For<Action<AzureB2COptions>>();
            azureB2COptions.Invoke(options);
            Assert.Single(azureB2COptions.ReceivedCalls());
        }
    }
}
