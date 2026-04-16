
using ECOMMERCEPAYMENTSYSTEM.Services.Interfaces;
using ECOMMERCEPAYMENTSYSTEM.Services.Implementations;
using ECOMMERCEPAYMENTSYSTEM.Services.Exceptions;

namespace ECOMMERCEPAYMENTSYSTEM.Services.Logic
{
    public class PaymentProcessor
    {
        public bool ProcessTransaction(IPaymentMethod payment, decimal amount)
        {
            try
            {
                Console.WriteLine($"\n--- Initiating {payment.PaymentType} ---");
               

                // 1. Validation Step
                if (!payment.ValidatePaymentInfo())
                {
                    throw new InvalidPaymentException("The provided payment details are incorrectly formatted.", payment.PaymentType);
                }

                // 2. Simulation of external errors (For testing purposes)
                if (amount > 1000000) // Assumes million-naira transactions are blocked
                {
                    throw new PaymentDeclinedException("Transaction exceeds limit.", "Bank Security");
                }

                // 3. Process the actual payment
                bool success = payment.ProcessPayment(amount);

                if (success)
                {
                    Console.WriteLine($"[LOG] SUCCESS: {payment.GetTransactionDetails()}");
                    return true;
                }
                
                return false;
            }
            catch (InvalidPaymentException ex)
            {
                Console.WriteLine($"VALIDATION ERROR: {ex.Message} (Method: {ex.PaymentType})");
                return false;
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"FUNDS ERROR: {ex.Message}. Shortfall on {ex.Amount:C}");
                return false;
            }
            catch (PaymentDeclinedException ex)
            {
                Console.WriteLine($"DECLINED: {ex.Message} by {ex.Provider}");
                return false;
            }
            catch (Exception ex)
            {
                // Catch-all for any other unexpected errors
                Console.WriteLine($"SYSTEM ERROR: {ex.Message}");
                return false;
            }
            finally
            {
                // This ALWAYS runs, regardless of success or failure
                Console.WriteLine($"[LOG] Session ended at {DateTime.Now}");
                Console.WriteLine("------------------------------------------");
            }
        }
    }
}
