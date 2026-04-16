using ECOMMERCEPAYMENTSYSTEM.Services.Interfaces;

namespace ECOMMERCEPAYMENTSYSTEM.Services.Implementations
{
    public class BankTransferPayment : IPaymentMethod
    {
        // Properties
        public required string AccountNumber { get; set; }
        public required string BankName { get; set; } 
        public required string AccountHolderName { get; set; }

        // Interface Property
        public string PaymentType => "Bank Transfer";

        // Validation: Account number must be exactly 10 digits and not null.
        public bool ValidatePaymentInfo()
        {
            if (string.IsNullOrWhiteSpace(AccountNumber)) return false;
            
            // Check if it's exactly 10 digits and contains only numbers
            return AccountNumber.Length == 10 && long.TryParse(AccountNumber, out _);
        }

        /// Transaction fee: Flat ₦100 fee.
        public decimal CalculateTransactionFee(decimal amount)
        {
            return 100.00m; 
        }

        /// Core Processing Logic
        public bool ProcessPayment(decimal amount)
        {
            // 1. Validation Check
            if (!ValidatePaymentInfo())
            {
                //Throwing an exception to pass the right error message to the user
                throw new InvalidOperationException($"Invalid Bank Details: Account Number '{AccountNumber}' must be 10 digits.");
            }

            // 2. Fee Calculation
            decimal fee = CalculateTransactionFee(amount);
            decimal totalToTransfer = amount + fee;

            // 3. Execution Simulation
            Console.WriteLine($"\n--- {PaymentType} Initiation ---");
            Console.WriteLine($"Bank: {BankName}");
            Console.WriteLine($"Initiating transfer of {totalToTransfer:N2} (Amount: {amount:N2} + Fee: {fee:N2})");
            Console.WriteLine("Processing 1-2 business days...");
            
            return true;
        }

        public string GetTransactionDetails()
        {
            return $"Method: {PaymentType} | Bank: {BankName} | Acc: {AccountNumber} | Status: Pending (1-2 Days)";
        }
    }
}
