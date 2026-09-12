namespace Application.Feature.Auth.QuickCaptureKey
{
    public class GenerateQuickCaptureKeyOutput
    {
        public bool Success { get; set; }
        /// <summary>
        /// Valor em texto plano da chave. Só existe nesta resposta — o backend guarda apenas o hash.
        /// </summary>
        public string? QuickCaptureKey { get; set; }
    }
}
