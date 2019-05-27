using DataContracts;
using InvoiceFixedFeeService;
using TransactionPercentageFeeService;

namespace MerchantFeesCalculator
{
    public interface IMerchantFeesCalculator
    {
        decimal Calculate(MerchantTransaction transaction);
    }

    public class MerchantFeesCalculator : IMerchantFeesCalculator
    {
        private readonly ITransactionPercentageFeeService _transactionPercentageFeeService;
        private readonly IInvoiceFixedFeeService _invoiceFixedFeeService;
        private readonly MerchantFeesCalculatorConfig _config;

        public MerchantFeesCalculator(ITransactionPercentageFeeService transactionPercentageFeeService,
            IInvoiceFixedFeeService invoiceFixedFeeService, MerchantFeesCalculatorConfig config)
        {
            _transactionPercentageFeeService = transactionPercentageFeeService;
            _invoiceFixedFeeService = invoiceFixedFeeService;
            _config = config;
        }

        public decimal Calculate(MerchantTransaction transaction)
        {
            var transactionPercentageFee = 0m;

            if (_config.TransactionPercentageFeeApplied)
            {
                transactionPercentageFee = _transactionPercentageFeeService.Calculate(transaction.MerchantName, transaction.Amount);
            }
            
            var invoiceFixedFee = 0m;
            if (_config.InvoiceFixedFeeApplied && transactionPercentageFee != 0m)
            {
                invoiceFixedFee = _invoiceFixedFeeService.Calculate(transaction);
            }

            return transactionPercentageFee + invoiceFixedFee;
        }
    }
}
