using System.ComponentModel;
using System.Reflection;

namespace Domain.Entities.Enums
{
    public enum AccountType
    {
        /// <summary>
        /// Tipo de conta a pagar.
        /// </summary>
        [Description("Conta a Pagar")]
        ContaAPagar = 1,
        /// <summary>
        /// Tipo de conta a receber.
        /// </summary>
        [Description("Conta a Receber")]
        ContaAReceber = 2
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
    }
}