using System;
using System.Collections.Concurrent;
using DataContracts;

namespace InvoiceFixedFeeService
{
    public interface IInvoiceFixedFeeService
    {
        void SetLastTransactionDate(string merchantName, DateTimeOffset lastTransactionDate);
        decimal Calculate(MerchantTransaction transaction);
    }

    public class InvoiceFixedFeeService : IInvoiceFixedFeeService
    {
        // Could be no-sql database
        private readonly ConcurrentDictionary<string, DateTimeOffset> _lastTransactionDates =
            new ConcurrentDictionary<string, DateTimeOffset>();

        public void SetLastTransactionDate(string merchantName, DateTimeOffset lastTransactionDate)
        {
            _lastTransactionDates.AddOrUpdate(merchantName, lastTransactionDate,
                (key, oldValue) => lastTransactionDate);
        }

        public decimal Calculate(MerchantTransaction transaction)
        {
            var lastTransactionRecorded = _lastTransactionDates.TryGetValue(transaction.MerchantName, out var lastMerchantTransaction);
            var invoiceFixedFee = 29m;

            SetLastTransactionDate(transaction.MerchantName, transaction.Date);

            if (!lastTransactionRecorded)
            {
                return invoiceFixedFee;
            }

            if (lastMerchantTransaction.Year == transaction.Date.Year &&
                lastMerchantTransaction.Month == transaction.Date.Month)
            {
                return 0m;
            }

            return invoiceFixedFee;
        }
    }
}