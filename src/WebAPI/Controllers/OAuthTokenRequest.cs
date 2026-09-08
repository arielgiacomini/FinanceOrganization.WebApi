using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class OAuthTokenRequest
    {
        [FromForm(Name = "grant_type")]
        public string GrantType { get; set; } = string.Empty;

        [FromForm(Name = "client_id")]
        public string? ClientId { get; set; }

        [FromForm(Name = "client_secret")]
        public string? ClientSecret { get; set; }

        /// <summary>
        /// E-mail do usuário, usado no grant_type=password (RFC 6749 §4.3 chama este campo de "username").
        /// </summary>
        [FromForm(Name = "username")]
        public string? Username { get; set; }

        [FromForm(Name = "password")]
        public string? Password { get; set; }
    }
}
