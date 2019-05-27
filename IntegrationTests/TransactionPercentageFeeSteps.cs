using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using DataContracts;
using MerchantFeesCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using TransactionFees;

namespace IntegrationTests
{
    [Binding]
    public class TransactionPercentageFeeSteps
    {
        private MerchantFeesCalculatorConfig _calculatorConfig = new MerchantFeesCalculatorConfig
        {
            InvoiceFixedFeeApplied = false,
            TransactionPercentageFeeApplied = false
        };

        private TransactionPercentageFeeService.TransactionPercentageFeeService _transactionPercentageFeeService =
            new TransactionPercentageFeeService.TransactionPercentageFeeService();

        private InvoiceFixedFeeService.InvoiceFixedFeeService _invoiceFixedFeeService =
            new InvoiceFixedFeeService.InvoiceFixedFeeService();

        private List<MerchantTransaction> _transactions = new List<MerchantTransaction>();
        private TestOutputWriter outputWriter = new TestOutputWriter();

        [Given(@"'(.*)' DKK transaction is made to '(.*)' on '(.*)'")]
        public void GivenTransactionIsMadeToCIRCLE_K(string amount, string merchant, string date)
        {
            var transaction = new MerchantTransaction()
            {
                Amount = Decimal.Parse(amount),
                Date = DateTimeOffset.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                MerchantName = merchant
            };

            _transactions.Add(transaction);
        }

        [Given(@"TransactionPercentageFee is configured to be '(.*)'")]
        public void SwitchTransactionPercentageFeeConfiguration(string transactionPercentageFee)
        {
            if (transactionPercentageFee == "enabled")
            {
                _calculatorConfig.TransactionPercentageFeeApplied = true;
            }
        }

        [Given(@"InvoiceFixedFee is configured to be '(.*)'")]
        public void SwitchInvoiceFixedFeeConfiguration(string invoiceFixedFee)
        {
            if (invoiceFixedFee == "enabled")
            {
                _calculatorConfig.InvoiceFixedFeeApplied = true;
            }
        }

        [Given(@"'(.*)' has TransactionPercentageFee discount of '(.*)' percent")]
        public void ApplyTransactionPercentageFeeDiscount(string merchant, string discountPercent)
        {
            _transactionPercentageFeeService.SetMerchantDiscount(merchant, decimal.Parse(discountPercent));
        }

        [When(@"fees calculation app is executed")]
        public void WhenFeesCalculationAppIsExecuted()
        {
            var merchantFeesCalculator = new MerchantFeesCalculator.MerchantFeesCalculator(
                _transactionPercentageFeeService, _invoiceFixedFeeService, _calculatorConfig);

            var transactionProcessor = new TransactionRecordProcessor(merchantFeesCalculator, outputWriter);

            _transactions
                .OrderBy(x => x.Date)
                .ToList()
                .ForEach(x =>
                    transactionProcessor.Process(x));
        }
        
        [Then(@"the output is equal to '(.*)' file content")]
        public void ThenTheOutputIsEqualToFileContent(string fileName)
        {
            var currentPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            var expectedResponse = System.IO.File.ReadAllText($"{currentPath}/ExpectedOutcome/{fileName}");

            Assert.AreEqual(expectedResponse, outputWriter.Output);
        }
    }
}
