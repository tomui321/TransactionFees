using System;
using System.IO;
using InvoiceFixedFeeService;
using MerchantFeesCalculator;
using Microsoft.Extensions.DependencyInjection;
using TransactionFees.DataAccess;
using TransactionPercentageFeeService;

namespace TransactionFees
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += ExceptionHandler;
            
            var calculatorConfig = new MerchantFeesCalculatorConfig
            {
                InvoiceFixedFeeApplied = true,
                TransactionPercentageFeeApplied = true
            };

            var serviceProvider = new ServiceCollection()
                .AddSingleton<ITransactionRecordProcessor, TransactionRecordProcessor>()
                .AddSingleton<IInputParser, InputParser>()
                .AddSingleton<IOutputWriter, OutputWriter>()
                .AddSingleton(calculatorConfig)
                .AddSingleton<IMerchantFeesCalculator, MerchantFeesCalculator.MerchantFeesCalculator>()
                .AddSingleton<ITransactionPercentageFeeService, TransactionPercentageFeeService.TransactionPercentageFeeService>()
                .AddSingleton<IInvoiceFixedFeeService, InvoiceFixedFeeService.InvoiceFixedFeeService>()
                .BuildServiceProvider();

            PrepareDataForTransactionPercentageFeeService(serviceProvider);

            ProcessFile(serviceProvider);
            Console.ReadLine();
        }

        private static void PrepareDataForTransactionPercentageFeeService(ServiceProvider serviceProvider)
        {
            // Shouldn't be needed when the database is used
            var transactionPercentageFeeService = serviceProvider.GetService<ITransactionPercentageFeeService>();

            transactionPercentageFeeService.SetMerchantDiscount("TELIA", 10m);
            transactionPercentageFeeService.SetMerchantDiscount("CIRCLE_K", 20m);
        }

        private static void ProcessFile(IServiceProvider serviceProvider)
        {
            var transactionsProcessor = serviceProvider.GetService<ITransactionRecordProcessor>();
            var inputParser = serviceProvider.GetService<IInputParser>();

            var fileStream = new FileStream("transactions.txt", FileMode.Open);
            using (var reader = new StreamReader(fileStream))
            {
                string record;
                while ((record = reader.ReadLine()) != null)
                {
                    var transaction = inputParser.Parse(record);
                    transactionsProcessor.Process(transaction);
                }
            }
        }

        static void ExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
