namespace Domain.Entities
{
    /// <summary>
    /// Usuário dono dos dados (carteira, contas, categorias, etc). Todo registro
    /// de domínio pertence a exatamente um usuário.
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Hash da senha (PasswordHasher). Nulo para usuários que só entram via Google.
        /// </summary>
        public string? PasswordHash { get; set; }
        /// <summary>
        /// Identificador único da conta Google (claim "sub" do id_token). Nulo para usuários sem login Google.
        /// </summary>
        public string? GoogleSub { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime TrialStartsAt { get; set; }
        public DateTime TrialEndsAt { get; set; }
        /// <summary>
        /// Um dos valores de <see cref="SubscriptionStatus"/>.
        /// </summary>
        public string SubscriptionStatus { get; set; } = Entities.SubscriptionStatus.Trialing;
    }
}
