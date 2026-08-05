using Application.Feature.Auth.GenerateToken;
using Domain.Options;
using Microsoft.Extensions.Options;
using Xunit;

namespace FinanceOrganization.UnitTests.Application.Feature.GenerateToken
{
    public class GenerateTokenHandlerTests
    {
        private readonly GenerateTokenHandler _handler;

        public GenerateTokenHandlerTests()
        {
            var jwtOptions = Options.Create(new JwtOptions
            {
                Issuer = "FinanceOrganization.WebApi.Tests",
                Audience = "FinanceOrganization.Client.Tests",
                Secret = "unit-test-super-secret-key-01234567890",
                ExpirationMinutes = 60
            });

            var authClientOptions = Options.Create(new AuthClientOptions
            {
                ClientId = "client-id-valido",
                ClientSecret = "client-secret-valido"
            });

            _handler = new GenerateTokenHandler(jwtOptions, authClientOptions);
        }

        [Fact]
        public void Handle_DeveGerarToken_QuandoCredenciaisValidas()
        {
            var output = _handler.Handle(new GenerateTokenInput
            {
                ClientId = "client-id-valido",
                ClientSecret = "client-secret-valido"
            });

            Assert.True(output.Success);
            Assert.False(string.IsNullOrWhiteSpace(output.AccessToken));
            Assert.Equal(3600, output.ExpiresInSeconds);
        }

        [Fact]
        public void Handle_DeveRecusar_QuandoClientSecretInvalido()
        {
            var output = _handler.Handle(new GenerateTokenInput
            {
                ClientId = "client-id-valido",
                ClientSecret = "secret-errado"
            });

            Assert.False(output.Success);
            Assert.Null(output.AccessToken);
        }

        [Fact]
        public void Handle_DeveRecusar_QuandoClientIdInvalido()
        {
            var output = _handler.Handle(new GenerateTokenInput
            {
                ClientId = "client-id-errado",
                ClientSecret = "client-secret-valido"
            });

            Assert.False(output.Success);
            Assert.Null(output.AccessToken);
        }
    }
}
