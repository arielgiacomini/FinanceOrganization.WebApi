﻿namespace Application.Feature.BillToPay.CreateBillToPay
{
    public class CreateBillToPayInput
    {
        public string? Name { get; set; }
        public string? Account { get; set; }
        public string? Frequence { get; set; }
        public string? InitialMonthYear { get; set; }
        public string? FynallyMonthYear { get; set; }
        public string? Category { get; set; }
        public decimal Value { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public int? BestPayDay { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastChangeDate { get; set; }
    }
}