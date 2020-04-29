using System;

namespace Data.Repositories.Privat24
{
    public class CurrencyRateInsertEntity
    {
        public DateTime Date { get; set; }
        public string BaseCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal SaleRateNBU { get; set; }
        public decimal PurchaseRateNBU { get; set; }
        public decimal SaleRatePB { get; set; }
        public decimal PurchaseRatePB { get; set; }
    }
}