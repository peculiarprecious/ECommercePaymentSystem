using ECOMMERCEPAYMENTSYSTEM.Services.Interfaces;

namespace ECOMMERCEPAYMENTSYSTEM.Services.Implementations
{
    public class CreditCardPayment : IPaymentMethod, IRefundable
    {
        public required string CardNumber { get; set; }
        public required string CardHolderName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public required string CVV { get; set; }

        public string PaymentType => "Credit Card";

        public bool ValidatePaymentInfo()
        {
            // Added a null/empty check to prevent crashes
            if (string.IsNullOrWhiteSpace(CardNumber) || CardNumber.Length != 16)
                return false;

            // Check if expiry date is in the future
            if (ExpiryDate < DateTime.Now)
                return false;

            return true;
        }

        public decimal CalculateTransactionFee(decimal amount) => amount * 0.025m;

        public bool ProcessPayment(decimal amount)
        {
            if (!ValidatePaymentInfo()) return false;

            decimal total = amount + CalculateTransactionFee(amount);
            Console.WriteLine($"Charging {total:C} to Credit Card (includes 2.5% fee)...");
            return true;
        }

        // To mask the card number
        public string GetTransactionDetails() =>
            $"Method: {PaymentType} | Holder: {CardHolderName} | Card: {CardNumber.Substring(12).PadLeft(16, '*')}";

        // // To Process refund
        public bool ProcessRefund(string transactionId, decimal amount)
        {
            Console.WriteLine($"Refund of {amount:C} initiated for Transaction: {transactionId}");
            return true;
        }

        public int GetRefundProcessingDays() => 3; // Cards usually take 3 days
    }
}
