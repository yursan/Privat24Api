using System;

namespace ApplicationServices.Privat24.Models
{
    public class CurrencyRateModel
    {
        public DateTime Date { get; set; }
        public string BaseCurrency { get; set; }
        public string Currency { get; set; }
        public decimal SaleRateNBU { get; set; }
        public decimal PurchaseRateNBU { get; set; }
        public decimal SaleRatePB { get; set; }
        public decimal PurchaseRatePB { get; set; }
    }
}