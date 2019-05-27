using MerchantFeesCalculator;
using TransactionFees.DataAccess;
using DataContracts;

namespace TransactionFees
{
    public interface ITransactionRecordProcessor
    {
        void Process(MerchantTransaction transaction);
    }

    public class TransactionRecordProcessor : ITransactionRecordProcessor
    {
        private readonly IMerchantFeesCalculator _merchantFeesCalculator;
        private readonly IOutputWriter _outputWriter;

        public TransactionRecordProcessor(IMerchantFeesCalculator merchantFeesCalculator, IOutputWriter outputWriter)
        {
            _merchantFeesCalculator = merchantFeesCalculator;
            _outputWriter = outputWriter;
        }
        
        public void Process(MerchantTransaction transaction)
        {
            if (transaction != null)
            {
                var calculatedFee = _merchantFeesCalculator.Calculate(transaction);
                _outputWriter.WriteRecord(transaction, calculatedFee);
            }
            else
            {
                _outputWriter.WriteEmptyLine();
            }
        }
    }
}
