
namespace ECOMMERCEPAYMENTSYSTEM.Services.Interfaces
{
    public interface IPaymentMethod
    {
        // Property to identify the method (e.g., "Credit Card", "PayPal")
        string PaymentType { get; }

        // Core logic to move money
        bool ProcessPayment(decimal amount);

        // Provides a summary of what happened
        string GetTransactionDetails();

        // Checks if card numbers or emails are formatted correctly
        bool ValidatePaymentInfo();

        // Logic to calculate fees (e.g., 2% for cards, 2.5% for mobile money)
        decimal CalculateTransactionFee(decimal amount);
    }
}
