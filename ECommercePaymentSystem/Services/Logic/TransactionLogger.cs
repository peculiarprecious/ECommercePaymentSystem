
using ECOMMERCEPAYMENTSYSTEM.Services.Interfaces;

namespace ECOMMERCEPAYMENTSYSTEM.Services.Logic
{
    public class TransactionLogger : ITransactionLogger
    {
        // Internal list to store the strings of history
        private List<string> _history = new List<string>();

        public void LogTransaction(string transactionId, decimal amount, bool success, string message)
        {
            string status = success ? "SUCCESS" : "FAILED";
            string logEntry = $"[{DateTime.Now}] ID: {transactionId} | Amount: {amount:C} | Status: {status} | Info: {message}";
            
            _history.Add(logEntry);
        }

        public void DisplayTransactionHistory()
        {
            Console.WriteLine("\n=== GLOBAL TRANSACTION HISTORY ===");
            if (_history.Count == 0) Console.WriteLine("No transactions logged yet.");
            
            foreach (var entry in _history) 
            {
                Console.WriteLine(entry);
            }
            Console.WriteLine("===================================\n");
        }
    }
}
