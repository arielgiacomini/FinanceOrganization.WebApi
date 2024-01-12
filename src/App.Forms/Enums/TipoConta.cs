namespace App.Forms.Enums
{
    public static class TipoConta
    {
        public static Dictionary<int, string> GetTipoContaEnable()
        {
            Dictionary<int, string> tipoConta = new()
            {
                { 1, "Cartão de Crédito" },
                { 2, "Cartão de Débito" },
                { 3, "Cartão VA" },
                { 4, "Cartão VR" },
                { 5, "Itaú" }
            };

            return tipoConta;
        }
    }
}