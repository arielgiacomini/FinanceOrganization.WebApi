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
    }
}
