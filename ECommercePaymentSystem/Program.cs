using System;
using ECOMMERCEPAYMENTSYSTEM.Services.Implementations;
using ECOMMERCEPAYMENTSYSTEM.Services.Interfaces;
using ECOMMERCEPAYMENTSYSTEM.Services.Logic;
using ECOMMERCEPAYMENTSYSTEM.Services.Exceptions;
namespace ECOMMERCEPAYMENTSYSTEM;

class Program
{
    private static PaymentProcessor _processor = new PaymentProcessor();
    //--- Start of Main ---
    static void Main(string[] args)
    {

        bool isRunning = true;
        while (isRunning)
        {
            DisplayMenu();

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 7.");
                continue;
            }

            try
            {
                switch (choice)
                {

                    case 1: HandleCreditCard(); break;
                    case 2: HandleBankTransfer(); break;
                    case 3: HandleMobileMoney(); break;
                    case 4: HandleRefund(); break;
                    case 5: _processor.ShowHistory(); break;
                    case 6: TestExceptionHandling(); break;
                    case 7: isRunning = false; Console.WriteLine("Exiting System..."); break;
                    default: Console.WriteLine("Choice out of range. Select 1-7."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }

    //--- End of Main ---

    //---  Start of Static Void Methods --
    static void DisplayMenu()
    {
        Console.WriteLine("\n=== E-Commerce Payment System ===");
        Console.WriteLine("1. Process Credit Card Payment");
        Console.WriteLine("2. Process Bank Transfer");
        Console.WriteLine("3. Process Mobile Money Payment");
        Console.WriteLine("4. Process Refund");
        Console.WriteLine("5. View Transaction History");
        Console.WriteLine("6. Test Exception Handling");
        Console.WriteLine("7. Exit");
        Console.Write("Enter your choice: ");
    }

    //1. Process Credit Card Payment
    static void HandleCreditCard()
    {
        // Collect inputs using GetValidatedString (to ensure they aren't empty)
        string name = GetValidatedString("Enter cardholder name: ");
        string cardNum = GetValidatedString("Enter card number (16 digits): ");
        decimal amount = GetValidatedAmount();
        string cardCVV = GetValidatedString("Enter 3-digit CVV: ");
        string dateInput = GetValidatedDate("Enter Expiry Date (dd/MM/yyyy): ");

        // Initialize the Object
        var card = new CreditCardPayment
        {
            CardNumber = cardNum,
            CardHolderName = name,
            ExpiryDate = DateTime.ParseExact(dateInput, "dd/MM/yyyy", null),
            CVV = cardCVV
        };

        // Pre-Processing Details (Display Fees First)
        decimal fee = card.CalculateTransactionFee(amount);
        Console.WriteLine("\n--- Transaction Preview ---");
        Console.WriteLine($"Payment Type:    {card.PaymentType}");
        Console.WriteLine($"Amount:          {amount:C}");
        Console.WriteLine($"Transaction Fee: {fee:C} (2.5%)");
        Console.WriteLine($"Total:           {(amount + fee):C}");
        Console.WriteLine("----------------------------");

        try
        {
            Console.WriteLine("\nValidating and Processing...");

            _processor.ProcessTransaction(card, amount);
        }
        catch (InvalidPaymentException ex)
        {
            // This catches the specific error in CreditCardPayment.cs
            Console.WriteLine($"\nPAYMENT ERROR: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SYSTEM ERROR: {ex.Message}");
        }
    }

    //2. Process Bank Transfer Payment
    static void HandleBankTransfer()
    {
        Console.WriteLine("\n--- Bank Transfer Details ---");
        string bank = GetValidatedString("Enter Bank Name: ");
        string accNum = GetValidatedString("Enter 10-Digit Account Number: ");
        string acctHolderName = GetValidatedString("Enter Account Holder Name: ");
        decimal amount = GetValidatedAmount();

        // Initialize Object
        var transfer = new BankTransferPayment
        {
            BankName = bank,
            AccountNumber = accNum,
            AccountHolderName = acctHolderName
        };

        // Show Fees (Transparency requirement)
        DisplayFees(transfer, amount);

        try
        {
            Console.WriteLine("Validating and Processing...");
            _processor.ProcessTransaction(transfer, amount);
        }
        catch (InvalidPaymentException ex)
        {
            // This catches the exact error
            Console.WriteLine($"\nBANK ERROR: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nSYSTEM ERROR: {ex.Message}");
        }
    }

    //3. Process Mobile Money Payment
    static void HandleMobileMoney()
    {
        Console.WriteLine("\n--- Mobile Money Details ---");

        string provider = GetValidatedString("Enter Provider (MTN/Airtel/Glo): ");
        string phone = GetValidatedString("Enter 11-digit Phone Number: ");
        string pin = GetValidatedString("Enter PIN: ");
        decimal amount = GetValidatedAmount();

        var mobile = new MobileMoneyPayment
        {
            Provider = provider,
            PhoneNumber = phone,
            PIN = pin
        };

        DisplayFees(mobile, amount);

        try
        {
            _processor.ProcessTransaction(mobile, amount);
        }
        catch (InvalidPaymentException ex)
        {
            Console.WriteLine($"\nMOBILE MONEY ERROR: {ex.Message}");
        }
    }


    //4. Handle Refund
    static void HandleRefund()
    {
        Console.WriteLine("\n--- Secure Refund Verification ---");

        // Initial Verification (ID and Date)
        string txId = GetValidatedString("Enter Transaction ID: ");
        string txDate = GetValidatedDate("Enter Transaction Date (dd/MM/yyyy): ");

        if (!_processor.VerifyForRefund(txId, txDate))
        {
            return; // Stop if verification fails
        }

        Console.WriteLine("\nVerification Successful. Proceeding with refund...");
        decimal amount = GetValidatedAmount();

        // Select Payment Method
        string type;
        while (true)
        {
            type = GetValidatedString("Select original payment method (1. Card / 2. Bank): ");
            if (type == "1" || type == "2") break;
            Console.WriteLine("Invalid choice. Please enter 1 or 2.");
        }

        IRefundable refundableMethod;
        if (type == "1")
        {
            Console.WriteLine("\n[Credit Card Verification]");
            string cardNum = GetValidatedString("Enter 16-digit Card Number: ");
            string name = GetValidatedString("Enter Card Holder Name: ");
            string cvv = GetValidatedString("Enter 3-digit CVV: ");

            refundableMethod = new CreditCardPayment
            {
                CardNumber = cardNum,
                CardHolderName = name,
                CVV = cvv,
                ExpiryDate = DateTime.Now.AddYears(1)
            };
        }
        else
        {
            Console.WriteLine("\n[Bank Transfer Verification]");
            string accNum = GetValidatedString("Enter 10-digit Account Number: ");
            string bank = GetValidatedString("Enter Bank Name: ");
            string holder = GetValidatedString("Enter Account Holder Name: ");

            refundableMethod = new BankTransferPayment
            {
                AccountNumber = accNum,
                BankName = bank,
                AccountHolderName = holder
            };
        }

        try
        {
            Console.WriteLine("\nVerifying details and initiating refund...");

            if (refundableMethod is IPaymentMethod payment)
            {
                payment.ValidatePaymentInfo(); // This will throw exception if details are wrong
            }

            bool refundSuccess = refundableMethod.ProcessRefund(txId, amount);

            if (refundSuccess)
            {
                Console.WriteLine($"Refund processed! Expected in {refundableMethod.GetRefundProcessingDays()} business days.");
            }
        }
        catch (InvalidPaymentException ex)
        {
            Console.WriteLine($"\nREFUND DENIED: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nSYSTEM ERROR: {ex.Message}");
        }
    }

    //6. Test Exception Handling
    static void TestExceptionHandling()
    {
        Console.WriteLine("\n=== TESTING CUSTOM EXCEPTIONS ===");

        // 1. InvalidPaymentException
        Console.WriteLine("\n[Test 1] Triggering InvalidPaymentException (Wrong Card Length)...");
        var badCard = new CreditCardPayment { CardNumber = "123", CardHolderName = "Emeka Ike", CVV = "345", ExpiryDate = DateTime.Now.AddYears(3) };
        _processor.ProcessTransaction(badCard, 100);

        // 2. PaymentDeclinedException
        Console.WriteLine("\n[Test 2] Triggering PaymentDeclinedException (Security Limit)...");
        var goodCard = new CreditCardPayment { CardNumber = "1234567890123456", ExpiryDate = DateTime.Now.AddYears(1), CardHolderName = "Loveth Ugo", CVV = "264" };
        _processor.ProcessTransaction(goodCard, 2000000); // Over 1 million limit

        // 3. InsufficientFundsException Simulation
        Console.WriteLine("\n[Test 3] Manually simulating InsufficientFundsException...");
        try { throw new InsufficientFundsException("Account balance too low.", 50000); }
        catch (InsufficientFundsException ex) { Console.WriteLine($"Caught: {ex.Message} (Deficit: {ex.Amount:C})"); }
    }
    static void DisplayFees(IPaymentMethod method, decimal amount)
    {
        decimal fee = method.CalculateTransactionFee(amount);
        Console.WriteLine($"\nProcessing via: {method.PaymentType}");
        Console.WriteLine($"Base Amount:     {amount:C}");
        Console.WriteLine($"Transaction Fee: {fee:C}");
        Console.WriteLine($"Final Total:     {(amount + fee):C}");
        Console.WriteLine("----------------------------");
    }

    //--- End of Static Void Methods  ---

    // --- Start of String Helpers ---
    static string GetValidatedString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? userInput = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(userInput))
            {
                return userInput;
            }

            Console.WriteLine("Input cannot be empty. Please try again.");
        }
    }
    static decimal GetValidatedAmount()
    {
        decimal amount;
        while (true)
        {
            Console.Write("Enter Transaction Amount: ");
            string? input = Console.ReadLine();

            // 1. Check if the input is a valid decimal
            // 2. Check if the amount is greater than zero
            if (decimal.TryParse(input, out amount) && amount > 0)
            {
                return amount; // Exit the loop and return the valid amount
            }

            Console.WriteLine("Invalid input. Please enter a positive decimal number (e.g., 500.50).");
        }
    }

    static string GetValidatedDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim() ?? "";

            // Try to parse the input specifically in the dd/MM/yyyy format
            if (DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {

                return parsedDate.ToString("dd/MM/yyyy");
            }

            Console.WriteLine("Invalid Date Format! Please use dd/MM/yyyy (e.g., 18/04/2026).");
        }
    }

    // --- End of String Helpers ---
}
