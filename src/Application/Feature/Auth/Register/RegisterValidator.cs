using Domain.Interfaces;

namespace Application.Feature.Auth.Register
{
    public static class RegisterValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(RegisterInput input, IUserRepository userRepository)
        {
            Dictionary<string, string> validations = new();

            if (string.IsNullOrWhiteSpace(input.Email) || !input.Email.Contains('@'))
            {
                validations.Add(nameof(input.Email), "Informe um e-mail válido.");
            }

            var getByUser = await userRepository.GetByEmail(input.Email);

            if (getByUser is not null)
            {
                validations.Add(nameof(input.Email), "Já existe uma conta cadastrada com este e-mail.");
            }

            if (string.IsNullOrWhiteSpace(input.Name))
            {
                validations.Add(nameof(input.Name), "Informe o nome.");
            }

            if (string.IsNullOrWhiteSpace(input.Password) || input.Password.Length < 8)
            {
                validations.Add(nameof(input.Password), "A senha deve ter no mínimo 8 caracteres.");
            }

            return validations;
        }
    }
}
