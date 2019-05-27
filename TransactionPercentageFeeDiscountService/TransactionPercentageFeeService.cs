using System.Collections.Concurrent;

namespace TransactionPercentageFeeService
{
    public interface ITransactionPercentageFeeService
    {
        void SetMerchantDiscount(string merchantName, decimal discountPercentage);
        decimal Calculate(string merchantName, decimal amount);
    }

    public class TransactionPercentageFeeService : ITransactionPercentageFeeService
    {
        // Could be no-sql database
        private readonly ConcurrentDictionary<string, decimal> _merchantDiscountPercentage =
            new ConcurrentDictionary<string, decimal>();

        public void SetMerchantDiscount(string merchantName, decimal discountPercentage)
        {
            _merchantDiscountPercentage.AddOrUpdate(merchantName, discountPercentage,
                (key, oldValue) => discountPercentage);
        }

        public decimal Calculate(string merchantName, decimal amount)
        {
            var calculatedFee = amount * 0.01m;

            var discountApplies = _merchantDiscountPercentage.TryGetValue(merchantName, out var discountPercentage);

            if (discountApplies)
            {
                calculatedFee -= calculatedFee * discountPercentage / 100;
            }

            return calculatedFee;
        }
    }
}
