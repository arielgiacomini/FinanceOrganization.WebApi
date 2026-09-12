namespace Application.Feature.Auth.QuickCaptureKey
{
    public interface IGenerateQuickCaptureKeyHandler
    {
        Task<GenerateQuickCaptureKeyOutput> Handle();
    }
}
