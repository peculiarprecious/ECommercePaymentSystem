
using ECOMMERCEPAYMENTSYSTEM.Services.Interfaces;

namespace ECOMMERCEPAYMENTSYSTEM.Services.Logic
{
    public class TransactionLogger : ITransactionLogger
    {
        private List<TransactionRecord> _history = new List<TransactionRecord>();

        public void LogTransaction(string transactionId, decimal amount, bool success, string message)
        {
            _history.Add(new TransactionRecord
            {
                Id = transactionId,
                Amount = amount,
                Status = success ? "SUCCESS" : "FAILED",
                Details = message,
                Timestamp = DateTime.Now
            });
        }

        public void DisplayTransactionHistory()
        {
            Console.WriteLine("\n" + new string('=', 110));
             Console.WriteLine("\n=== GLOBAL TRANSACTION HISTORY ===");
            Console.WriteLine(new string('=', 110));

            Console.WriteLine($"{"DATE",-20} | {"TRANSACTION ID",-15} | {"AMOUNT",-12} | {"STATUS",-10} | {"DETAILS"}");
            Console.WriteLine(new string('-', 110));

            if (_history.Count == 0)
            {
                Console.WriteLine("No transactions logged yet.");
            }
            else
            {
                foreach (var t in _history)
                {
                    // Aligning each property to match the header
                    Console.WriteLine($"{t.Timestamp,-20:dd/MM/yyyy HH:mm} | {t.Id,-15} | {t.Amount,-12:C} | {t.Status,-10} | {t.Details}");
                }
            }
            Console.WriteLine(new string('=', 110) + "\n");
        }

        public bool IsTransactionValidForRefund(string transactionId, string transactionDate)
        {
            return _history.Exists(t => t.Id == transactionId && t.Timestamp.ToString("dd/MM/yyyy") == transactionDate && t.Status == "SUCCESS");
        }


    }

    internal class TransactionRecord
    {
        public string Id { get; set; } = "";
        public decimal Amount { get; set; }
        public string Status { get; set; } = "";
        public string Details { get; set; } = "";
        public DateTime Timestamp { get; set; }
    }
}
