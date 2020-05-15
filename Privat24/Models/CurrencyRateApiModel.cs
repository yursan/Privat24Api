using System;

namespace Privat24WebApp.Models
{
    public class CurrencyRateApiModel
    {
        public DateTime Date { get; set; }
        public decimal USDSaleRateNBU { get; set; }
        public decimal USDSaleRatePB { get; set; }
        public decimal EURSaleRateNBU { get; set; }
        public decimal EURSaleRatePB { get; set; }
        public decimal USDPurchaseRateNBU { get; set; }
        public decimal USDPurchaseRatePB { get; set; }
        public decimal EURPurchaseRateNBU { get; set; }
        public decimal EURPurchaseRatePB { get; set; }
    }
}