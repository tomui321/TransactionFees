using System;
using DataContracts;
using MerchantFeesCalculator;
using Moq;
using NUnit.Framework;
using TransactionFees;
using TransactionFees.DataAccess;

namespace UnitTests
{
    public class TransactionRecordProcessorTests
    {
        private TransactionRecordProcessor _transactionRecordProcessor;

        private Mock<IMerchantFeesCalculator> _merchantFeesCalculatorMock;
        private Mock<IOutputWriter> _outputWriterMock;

        [SetUp]
        public void Setup()
        {
            _merchantFeesCalculatorMock = new Mock<IMerchantFeesCalculator>();
            _outputWriterMock = new Mock<IOutputWriter>();

            _transactionRecordProcessor =
                new TransactionRecordProcessor(_merchantFeesCalculatorMock.Object, _outputWriterMock.Object);
        }

        [Test]
        public void Process_TransactionIsNull_WritesEmptyLine()
        {
            _transactionRecordProcessor.Process(null);

            _merchantFeesCalculatorMock.Verify(x => x.Calculate(It.IsAny<MerchantTransaction>()), Times.Never);
            _outputWriterMock.Verify(x => x.WriteRecord(It.IsAny<MerchantTransaction>(), It.IsAny<decimal>()), Times.Never);
            _outputWriterMock.Verify(x => x.WriteEmptyLine(), Times.Once);
        }

        [Test]
        public void Process_TransactionSupplied_CalculatesAndWritesOutput()
        {
            var transaction = GetMerchantTransaction();
            var calculatedFee = 5;
            _merchantFeesCalculatorMock
                .Setup(x => x.Calculate(It.IsAny<MerchantTransaction>()))
                .Returns(calculatedFee);

            _transactionRecordProcessor.Process(transaction);

            _merchantFeesCalculatorMock.Verify(x => x.Calculate(It.IsAny<MerchantTransaction>()), Times.Once);
            _outputWriterMock.Verify(x => x.WriteRecord(It.IsAny<MerchantTransaction>(), It.IsAny<decimal>()), Times.Once);
            _outputWriterMock.Verify(x => x.WriteEmptyLine(), Times.Never);
        }

        private MerchantTransaction GetMerchantTransaction()
        {
            return new MerchantTransaction
            {
                Amount = 1,
                Date = new DateTimeOffset(),
                MerchantName = "Test"
            };
        }
    }
}