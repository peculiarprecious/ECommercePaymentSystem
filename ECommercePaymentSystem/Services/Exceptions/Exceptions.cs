
namespace ECOMMERCEPAYMENTSYSTEM.Services.Exceptions
{
      // Base class for all our custom payment errors
    public class InvalidPaymentException : Exception
    {
        public string PaymentType { get; }
        public InvalidPaymentException(string message, string paymentType) : base(message)
        {
            PaymentType = paymentType;
        }
    }

    public class InsufficientFundsException : Exception
    {
        public decimal Amount { get; }
        public InsufficientFundsException(string message, decimal amount) : base(message)
        {
            Amount = amount;
        }
    }

    public class PaymentDeclinedException : Exception
    {
        public string Provider { get; }
        public PaymentDeclinedException(string message, string provider) : base(message)
        {
            Provider = provider;
        }
    }

    public class TransactionTimeoutException : Exception
    {
        public int Seconds { get; }
        public TransactionTimeoutException(string message, int seconds) : base(message)
        {
            Seconds = seconds;
        }
    }
}
