using System;

namespace DataContracts
{
    public class MerchantTransaction
    {
        public DateTimeOffset Date { get; set; }
        public string MerchantName { get; set; }
        public decimal Amount { get; set; }
    }
}