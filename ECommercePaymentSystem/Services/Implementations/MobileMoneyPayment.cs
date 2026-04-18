using ECOMMERCEPAYMENTSYSTEM.Services.Interfaces;
using ECOMMERCEPAYMENTSYSTEM.Services.Exceptions;
namespace ECOMMERCEPAYMENTSYSTEM.Services.Implementations
{
    public class MobileMoneyPayment : IPaymentMethod
    {
        // Properties
        public required string PhoneNumber { get; set; }
        public required string Provider { get; set; } // MTN/Airtel/Glo
        public required string PIN { get; set; }

        public string PaymentType => "Mobile Money";
        public bool ValidatePaymentInfo()
        {
            // 1. Validate Phone Number (11 digits, starts with 0, numeric only)
            if (string.IsNullOrWhiteSpace(PhoneNumber) || PhoneNumber.Length != 11 ||
                !PhoneNumber.StartsWith("0") || !long.TryParse(PhoneNumber, out _))
            {
                throw new InvalidPaymentException("Phone number must be 11 numeric digits starting with '0'.", "Mobile Money");
            }

            // 2. Validate Provider (MTN/Airtel/Glo - cannot be empty)
            if (string.IsNullOrWhiteSpace(Provider))
            {
                throw new InvalidPaymentException("Network provider (MTN/Airtel/Glo) is required.", "Mobile Money");
            }

            // 3. Validate PIN (Example: must be at least 4 digits)
            if (string.IsNullOrWhiteSpace(PIN) || PIN.Length < 4 || !int.TryParse(PIN, out _))
            {
                throw new InvalidPaymentException("Security PIN must be at least 4 numeric digits.", "Mobile Money");
            }

            return true;
        }


        // Transaction fee: 1.5% of amount
        public decimal CalculateTransactionFee(decimal amount)
        {
            return amount * 0.015m;
        }

        //Processing Logic with Step-by-Step Validation

        public bool ProcessPayment(decimal amount)
        {
            // 1. Pre-Payment Validation
            if (!ValidatePaymentInfo())
            {
                Console.WriteLine("Error: Invalid Phone Number or Provider information.");
                return false;
            }

            // 2. PIN validation 
            if (string.IsNullOrWhiteSpace(PIN) || PIN.Length < 4)
            {
                Console.WriteLine("Error: Security PIN is required for Mobile Money.");
                return false;
            }

            // 3. Fee and Total Calculation
            decimal fee = CalculateTransactionFee(amount);
            decimal total = amount + fee;

            // 4. Execution
            Console.WriteLine($"\n--- {PaymentType} Transaction ---");
            Console.WriteLine($"Provider: {Provider}");
            Console.WriteLine($"Processing {total:C} (Amount: {amount:C} + 1.5% Fee: {fee:C})...");
            Console.WriteLine("Status: Instant Payment Successful!");

            return true;
        }

        public string GetTransactionDetails()
        {
            // Mask the phone number for security (e.g., 0803****123)
            string maskedPhone = PhoneNumber.Substring(0, 4) + "****" + PhoneNumber.Substring(7);
            return $"Method: {PaymentType} | Provider: {Provider} | Phone: {maskedPhone}";
        }
    }
}
