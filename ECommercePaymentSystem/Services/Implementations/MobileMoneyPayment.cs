using System;
using ECOMMERCEPAYMENTSYSTEM.Services.Interfaces;

namespace ECOMMERCEPAYMENTSYSTEM.Services.Implementation
{
    public class MobileMoneyPayment : IPaymentMethod
    {
        // Properties
        public required string PhoneNumber { get; set; }
        public required string Provider { get; set; } // MTN/Airtel/Glo
        public required string PIN { get; set; }

        public string PaymentType => "Mobile Money";

        /// Validation: 
        /// 1. Must be 11 digits
        /// 2. Must start with '0'
        /// 3. Must be numeric only
        /// 4. Provider must not be empty
        public bool ValidatePaymentInfo()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber)) return false;

            return PhoneNumber.Length == 11 && 
                   PhoneNumber.StartsWith("0") && 
                   long.TryParse(PhoneNumber, out _) && // Ensures no letters
                   !string.IsNullOrWhiteSpace(Provider);
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

            // 2. Security Check (Basic PIN validation simulation)
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
            Console.WriteLine("✅ Status: Instant Payment Successful!");
            
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
