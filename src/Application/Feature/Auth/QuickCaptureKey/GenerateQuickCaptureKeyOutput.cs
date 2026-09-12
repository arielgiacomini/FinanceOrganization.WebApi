namespace Application.Feature.Auth.QuickCaptureKey
{
    public class GenerateQuickCaptureKeyOutput
    {
        public bool Success { get; set; }
        /// <summary>
        /// Valor em texto plano da chave. Só existe nesta resposta — o backend guarda apenas o hash.
        /// </summary>
        public string? QuickCaptureKey { get; set; }
        /// <summary>
        /// A quem a chave ficou amarrada. Devolvido na resposta de propósito: é a forma mais direta
        /// de confirmar, sem precisar olhar log de servidor, que a chave foi gerada na conta certa —
        /// basta comparar com o e-mail/UserId da sua conta principal.
        /// </summary>
        public Guid? UserId { get; set; }
        public string? Email { get; set; }
    }
}
