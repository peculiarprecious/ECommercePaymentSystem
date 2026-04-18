using ECOMMERCEPAYMENTSYSTEM.Services.Interfaces;
using ECOMMERCEPAYMENTSYSTEM.Services.Exceptions;
namespace ECOMMERCEPAYMENTSYSTEM.Services.Implementations
{
    public class BankTransferPayment : IPaymentMethod, IRefundable
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
            // 1. Validate Account Number (10 Digits and Numeric)
            if (string.IsNullOrWhiteSpace(AccountNumber) || AccountNumber.Length != 10 || !long.TryParse(AccountNumber, out _))
            {
                throw new InvalidPaymentException("Account number must be exactly 10 numeric digits.", "Bank Transfer");
            }

            // 2. Validate Bank Name (Required)
            if (string.IsNullOrWhiteSpace(BankName))
            {
                throw new InvalidPaymentException("Bank name is required for transfer.", "Bank Transfer");
            }

            // 3. Validate Account Holder Name (Required)
            if (string.IsNullOrWhiteSpace(AccountHolderName))
            {
                throw new InvalidPaymentException("Account holder name must be provided.", "Bank Transfer");
            }

            return true;
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

            string maskedAcc = (AccountNumber.Length == 10)
                ? AccountNumber.Substring(0, 3) + "****" + AccountNumber.Substring(7)
                : AccountNumber;

            return $"Method: {PaymentType,-15} | Bank: {BankName,-12} | Acc: {maskedAcc,-12} | Status: Pending (1-2 Days)";
        }

        // To Process Refund
        public bool ProcessRefund(string transactionId, decimal amount)
        {
            Console.WriteLine($"Bank Refund requested for {transactionId}. Manual verification required.");
            return true;
        }

        public int GetRefundProcessingDays() => 7; // Bank transfers take longer
    }
}
