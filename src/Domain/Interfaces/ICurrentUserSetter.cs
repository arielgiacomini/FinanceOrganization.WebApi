namespace Domain.Interfaces
{
    /// <summary>
    /// Só para uso fora de uma requisição HTTP (ex.: rotina em background) — onde não existe token
    /// nem HttpContext pra derivar o "usuário atual" automaticamente. Quem monta o trabalho precisa
    /// atribuir isso explicitamente, um usuário de cada vez, dentro de um escopo de DI criado só pra
    /// aquele usuário. Interface separada de ICurrentUserService de propósito: nenhum código que
    /// atende requisição HTTP deveria conseguir trocar o usuário no meio do caminho.
    /// </summary>
    public interface ICurrentUserSetter
    {
        void SetUserId(Guid userId);
    }
}
