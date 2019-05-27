using System;
using DataContracts;
using FluentAssertions;
using NUnit.Framework;
using TransactionFees.DataAccess;

namespace UnitTests
{
    public class InputParserTests
    {
        private InputParser _inputParser;

        [SetUp]
        public void Setup()
        {
            _inputParser = new InputParser();
        }

        [Test]
        public void Parse_EmptyString_ReturnsNull()
        {
            var testTransaction = string.Empty;

            var result = _inputParser.Parse(testTransaction);

            Assert.IsNull(result);
        }

        [Test]
        public void Parse_StringWithSpacesAndTabs_ReturnsNull()
        {
            var testTransaction = "         ";

            var result = _inputParser.Parse(testTransaction);

            Assert.IsNull(result);
        }

        [Test]
        public void Parse_CorruptedStringFormat_ThrowsException()
        {
            var testTransaction = "2010-01-01 Test 20.55 Something Something";

            var ex = Assert.Throws<ArgumentException>(() => _inputParser.Parse(testTransaction));
            Assert.AreEqual("The supplied string is not a valid transaction record!", ex.Message);
        }

        [Test]
        public void Parse_CorrectStringFormat_ReturnsParsedObject()
        {
            var testTransaction = "2010-01-01 Test 20.55";
            var expected = new MerchantTransaction()
            {
                Amount = 20.55m,
                Date = new DateTimeOffset(new DateTime(2010, 01, 01)),
                MerchantName = "Test"
            };

            var result = _inputParser.Parse(testTransaction);

            result.Should().BeEquivalentTo(expected);
        }
    }
}