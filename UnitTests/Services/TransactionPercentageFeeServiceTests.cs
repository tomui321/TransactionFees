using NUnit.Framework;

namespace UnitTests.Services
{
    public class TransactionPercentageFeeServiceTests
    {
        private TransactionPercentageFeeService.TransactionPercentageFeeService _transactionPercentageFeeService;

        [SetUp]
        public void Setup()
        {
            _transactionPercentageFeeService = new TransactionPercentageFeeService.TransactionPercentageFeeService();
        }

        [Test]
        public void Calculate_NoDiscount_StandardFeeApplied()
        {
            var amount = 1000;
            var standardFee = 10;

            var result = _transactionPercentageFeeService.Calculate("TEST", amount);
            
            Assert.AreEqual(standardFee, result);
        }

        [Test]
        public void Calculate_DiscountExists_ReducedFeeApplied()
        {
            var amount = 1000;
            var discountPercentage = 10;
            var discountedFee = 9;
            _transactionPercentageFeeService.SetMerchantDiscount("TEST", discountPercentage);

            var result = _transactionPercentageFeeService.Calculate("TEST", amount);

            Assert.AreEqual(discountedFee, result);
        }
    }
}
