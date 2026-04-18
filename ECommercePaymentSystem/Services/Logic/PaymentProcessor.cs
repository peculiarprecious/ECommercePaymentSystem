using System;
using ECOMMERCEPAYMENTSYSTEM.Services.Interfaces;
using ECOMMERCEPAYMENTSYSTEM.Services.Implementations;
using ECOMMERCEPAYMENTSYSTEM.Services.Exceptions;

namespace ECOMMERCEPAYMENTSYSTEM.Services.Logic
{
    public class PaymentProcessor
    {
        private readonly ITransactionLogger _logger = new TransactionLogger();

        public bool ProcessTransaction(IPaymentMethod payment, decimal amount)
        {

            bool success = false;
            string message = "Transaction Initiated";
            string datePart = DateTime.Now.ToString("yyyyMMdd");

            //Create the ID (using a random number for the '001' part to keep it simple)
            string transactionId = $"TXN-{datePart}-{new Random().Next(100, 999)}";

            try
            {
                Console.WriteLine($"\n--- Initiating {payment.PaymentType} ---");

                // 1. Validation Step
                if (!payment.ValidatePaymentInfo())
                {
                    message = "Validation Failed";
                    throw new InvalidPaymentException("The provided payment details are incorrectly formatted.", payment.PaymentType);
                }

                // 2. Limit Check. Assumes million-naira transactions are blocked
                if (amount > 1000000)
                {
                    message = "Limit Exceeded";
                    throw new PaymentDeclinedException("Transaction exceeds limit.", "Bank Security");
                }

                // 3. Process the actual payment
                success = payment.ProcessPayment(amount);

                if (success)
                {
                    message = payment.GetTransactionDetails();
                    Console.WriteLine($"[LOG] SUCCESS: {message}");
                }

                return success;
            }
            catch (InvalidPaymentException ex)
            {
                success = false;
                message = ex.Message;
                Console.WriteLine($"VALIDATION ERROR: {ex.Message} (Method: {ex.PaymentType})");
                return false;
            }
            catch (InsufficientFundsException ex)
            {
                success = false;
                message = ex.Message;
                Console.WriteLine($"FUNDS ERROR: {ex.Message}. Shortfall on {ex.Amount:C}");
                return false;
            }
            catch (PaymentDeclinedException ex)
            {
                success = false;
                message = ex.Message;
                Console.WriteLine($"DECLINED: {ex.Message} by {ex.Provider}");
                return false;
            }
            catch (Exception ex)
            {
                success = false;
                message = ex.Message;
                Console.WriteLine($"SYSTEM ERROR: {ex.Message}");
                return false;
            }
            finally
            {
                _logger.LogTransaction(transactionId, amount, success, message);

                Console.WriteLine($"[LOG] Session ended at {DateTime.Now}");
                Console.WriteLine("------------------------------------------");
            }
        }


        public bool VerifyForRefund(string txId, string date)
        {
            return _logger.IsTransactionValidForRefund(txId, date);
        }

        public void ShowHistory()
        {
            _logger.DisplayTransactionHistory();
        }
    }
}
