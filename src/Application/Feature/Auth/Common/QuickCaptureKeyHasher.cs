using System.Security.Cryptography;
using System.Text;

namespace Application.Feature.Auth.Common
{
    /// <summary>
    /// Geração/hash da chave opaca de Lançamento Rápido. É um segredo de alta entropia (256 bits),
    /// não uma senha — por isso um hash simples (SHA-256, sem sal) já é seguro contra força bruta:
    /// o que protege aqui é o espaço de busca do token, não a lentidão do algoritmo.
    /// </summary>
    public static class QuickCaptureKeyHasher
    {
        private const string Prefix = "qck_";

        public static string GenerateKey()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(32);

            return Prefix + Convert.ToHexString(randomBytes).ToLowerInvariant();
        }

        public static string Hash(string key)
        {
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(key));

            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }
    }
}
