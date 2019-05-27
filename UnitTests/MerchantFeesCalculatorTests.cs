using System;
using DataContracts;
using InvoiceFixedFeeService;
using MerchantFeesCalculator;
using Moq;
using NUnit.Framework;
using TransactionPercentageFeeService;

namespace UnitTests
{
    public class MerchantFeesCalculatorTests
    {
        private MerchantFeesCalculator.MerchantFeesCalculator _merchantFeesCalculator;
        private MerchantFeesCalculatorConfig _config;

        private Mock<ITransactionPercentageFeeService>  _transactionPercentageFeeServiceMock;
        private Mock<IInvoiceFixedFeeService> _invoiceFixedFeeServiceMock;

        [SetUp]
        public void Setup()
        {
            _transactionPercentageFeeServiceMock = new Mock<ITransactionPercentageFeeService>();
            _invoiceFixedFeeServiceMock = new Mock<IInvoiceFixedFeeService>();
            _config = new MerchantFeesCalculatorConfig
            {
                TransactionPercentageFeeApplied = true,
                InvoiceFixedFeeApplied = true
            };

            _merchantFeesCalculator =
                new MerchantFeesCalculator.MerchantFeesCalculator(_transactionPercentageFeeServiceMock.Object, _invoiceFixedFeeServiceMock.Object, _config);
        }

        [Test]
        public void Calculate_PercentageFeeIsZero_ZeroAmountReturned()
        {
            _transactionPercentageFeeServiceMock.Setup(x => x.Calculate(It.IsAny<string>(), It.IsAny<decimal>()))
                .Returns(0);
            var transaction = GetMerchantTransaction();

            var result = _merchantFeesCalculator.Calculate(transaction);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result);
            _transactionPercentageFeeServiceMock.Verify(x => x.Calculate(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once);
            _invoiceFixedFeeServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Calculate_PercentageFeeIsNotZero_CorrectAmountIsReturned()
        {
            var transaction = GetMerchantTransaction();
            var transactionPercentageFee = 100;
            var invoiceFixedFee = 200;
            _transactionPercentageFeeServiceMock.Setup(x => x.Calculate(It.IsAny<string>(), It.IsAny<decimal>()))
                .Returns(transactionPercentageFee);
            _invoiceFixedFeeServiceMock.Setup(x => x.Calculate(It.IsAny<MerchantTransaction>()))
                .Returns(invoiceFixedFee);

            var result = _merchantFeesCalculator.Calculate(transaction);

            Assert.IsNotNull(result);
            Assert.AreEqual(transactionPercentageFee + invoiceFixedFee, result);
            _transactionPercentageFeeServiceMock.Verify(x => x.Calculate(It.IsAny<string>(), It.IsAny<decimal>()), Times.Once);
            _invoiceFixedFeeServiceMock.Verify(x => x.Calculate(It.IsAny<MerchantTransaction>()), Times.Once);
        }
        
        private MerchantTransaction GetMerchantTransaction()
        {
            return new MerchantTransaction
            {
                Amount = 1000,
                Date = new DateTimeOffset(),
                MerchantName = "Test"
            };
        }
    }
}