namespace Application.Feature.Auth.GenerateToken
{
    public interface IGenerateTokenHandler
    {
        GenerateTokenOutput Handle(GenerateTokenInput input);
    }
}