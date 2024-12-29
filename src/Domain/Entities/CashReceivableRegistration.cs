namespace Domain.Entities;

public class CashReceivableRegistration
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Account { get; set; }
    public string? Frequence { get; set; }
    public string? RegistrationType { get; set; }
    /// <summary>
    /// Data do acordo de recebimento
    /// </summary>
    public DateTime? AgreementDate { get; set; }
    public string? InitialMonthYear { get; set; }
    public string? FynallyMonthYear { get; set; }
    public string? Category { get; set; }
    public decimal Value { get; set; }
    /// <summary>
    /// Dia escolhido para recebimento
    /// </summary>
    public int BestReceivingDay { get; set; }
    public string? AdditionalMessage { get; set; }
    /// <summary>
    /// Inativação - Delete Lógico da Tabela
    /// </summary>
    public bool? Enabled { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? LastChangeDate { get; set; }
}