using ECOMMERCEPAYMENTSYSTEM.Services.Interfaces;
using ECOMMERCEPAYMENTSYSTEM.Services.Exceptions;

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
    // 1. Validate Card Number (Length and Numeric only)
    if (string.IsNullOrWhiteSpace(CardNumber) || CardNumber.Length != 16 || !long.TryParse(CardNumber, out _))
    {
        throw new InvalidPaymentException("Card number must be exactly 16 numeric digits.", "Credit Card");
    }

    // 2. Validate Card Holder Name (Cannot be empty)
    if (string.IsNullOrWhiteSpace(CardHolderName))
    {
        throw new InvalidPaymentException("Card holder name is required.", "Credit Card");
    }

    // 3. Validate Expiry Date (Must be in the future)
    if (ExpiryDate < DateTime.Now)
    {
        throw new InvalidPaymentException("The credit card has expired.", "Credit Card");
    }

    // 4. Validate CVV (Exactly 3 digits and numeric only)
    if (string.IsNullOrWhiteSpace(CVV) || CVV.Length != 3 || !int.TryParse(CVV, out _))
    {
        throw new InvalidPaymentException("CVV must be exactly 3 numeric digits.", "Credit Card");
    }

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
