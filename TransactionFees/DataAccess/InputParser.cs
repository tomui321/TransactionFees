using System;
using System.Globalization;
using DataContracts;

namespace TransactionFees.DataAccess
{
    public interface IInputParser
    {
        MerchantTransaction Parse(string transactionRecord);
    }

    public class InputParser : IInputParser
    {
        public MerchantTransaction Parse(string transactionRecord)
        {
            var formattedRecord = System.Text.RegularExpressions.Regex.Replace(transactionRecord, @"\s+", " ").Trim();

            if (formattedRecord == string.Empty)
            {
                return null;
            }

            var separatedRecord = formattedRecord.Split(" ");

            if (separatedRecord.Length != 3)
            {
                throw new ArgumentException("The supplied string is not a valid transaction record!");
            }

            return new MerchantTransaction
            {
                Date = DateTimeOffset.ParseExact(separatedRecord[0], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                MerchantName = separatedRecord[1],
                Amount = decimal.Parse(separatedRecord[2])
            };
        }
    }
}
