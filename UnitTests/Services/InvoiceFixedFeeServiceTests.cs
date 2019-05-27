using System;
using DataContracts;
using NUnit.Framework;

namespace UnitTests.Services
{
    public class InvoiceFixedFeeServiceTests
    {
        private InvoiceFixedFeeService.InvoiceFixedFeeService _invoiceFixedFeeService;

        [SetUp]
        public void Setup()
        {
            _invoiceFixedFeeService = new InvoiceFixedFeeService.InvoiceFixedFeeService();
        }

        [Test]
        public void Calculate_FirstEverTransactionForMerchant_InvoiceFixedFeeApplies()
        {
            var expectedInvoiceFixedFee = 29;
            var transaction = new MerchantTransaction
            {
                Amount = 1,
                Date = new DateTime(2010, 01, 01),
                MerchantName = "TEST"
            };

            var result = _invoiceFixedFeeService.Calculate(transaction);
            
            Assert.AreEqual(expectedInvoiceFixedFee, result);
        }

        [Test]
        public void Calculate_FirstTransactionOfTheMonthForMerchant_InvoiceFixedFeeApplies()
        {
            var expectedInvoiceFixedFee = 29;
            _invoiceFixedFeeService.SetLastTransactionDate("TEST", new DateTime(2010, 01, 01));
            var transaction = new MerchantTransaction
            {
                Amount = 1,
                Date = new DateTime(2010, 02, 01),
                MerchantName = "TEST"
            };

            var result = _invoiceFixedFeeService.Calculate(transaction);

            Assert.AreEqual(expectedInvoiceFixedFee, result);
        }

        [Test]
        public void Calculate_TransactionForMerchantExistSameMonthLastYear_InvoiceFixedFeeApplies()
        {
            var expectedInvoiceFixedFee = 29;
            _invoiceFixedFeeService.SetLastTransactionDate("TEST", new DateTime(2009, 01, 01));
            var transaction = new MerchantTransaction
            {
                Amount = 1,
                Date = new DateTime(2010, 01, 01),
                MerchantName = "TEST"
            };
            
            var result = _invoiceFixedFeeService.Calculate(transaction);

            Assert.AreEqual(expectedInvoiceFixedFee, result);
        }

        [Test]
        public void Calculate_TransactionForMerchantExistSameMonth_InvoiceFixedFeeDoesNotApply()
        {
            _invoiceFixedFeeService.SetLastTransactionDate("TEST" ,new DateTime(2010, 01, 01));
            var transaction = new MerchantTransaction
            {
                Amount = 1,
                Date = new DateTime(2010, 01, 02),
                MerchantName = "TEST"
            };

            var result = _invoiceFixedFeeService.Calculate(transaction);

            Assert.AreEqual(0, result);
        }
    }
}
