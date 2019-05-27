using System;
using DataContracts;

namespace TransactionFees.DataAccess
{
    public interface IOutputWriter
    {
        void WriteRecord(MerchantTransaction transaction, decimal fee);
        void WriteEmptyLine();
    }

    public class OutputWriter : IOutputWriter
    {
        public void WriteRecord(MerchantTransaction transaction, decimal fee)
        {
            Console.WriteLine($"{transaction.Date:yyyy-MM-dd} {transaction.MerchantName} {decimal.Round(fee,2)}");
        }

        public void WriteEmptyLine()
        {
            Console.WriteLine(string.Empty);
        }
    }
}
