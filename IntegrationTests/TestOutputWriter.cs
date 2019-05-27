using System;
using DataContracts;
using TransactionFees.DataAccess;

namespace IntegrationTests
{
    public class TestOutputWriter : IOutputWriter
    {
        public string Output = string.Empty;

        public void WriteRecord(MerchantTransaction transaction, decimal fee)
        {
            Output += $"{transaction.Date:yyyy-MM-dd} {transaction.MerchantName} {fee}";
            Output += Environment.NewLine;
        }

        public void WriteEmptyLine()
        {
            Output += string.Empty;
            Output += Environment.NewLine;
        }
    }
}
