namespace Domain.Entities
{
    /// <summary>
    /// Valores possíveis para <see cref="User.SubscriptionStatus"/>.
    /// </summary>
    public static class SubscriptionStatus
    {
        public const string Trialing = "trialing";
        public const string Active = "active";
        public const string PastDue = "past_due";
        public const string Canceled = "canceled";
    }
}
