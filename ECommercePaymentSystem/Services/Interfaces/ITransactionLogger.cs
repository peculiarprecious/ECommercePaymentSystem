namespace ECOMMERCEPAYMENTSYSTEM.Services.Interfaces
{
    public interface ITransactionLogger
    {
        void LogTransaction(string transactionId, decimal amount, bool success, string message);
        void DisplayTransactionHistory();
        bool IsTransactionValidForRefund(string transactionId, string transactionDate);
    }
}
