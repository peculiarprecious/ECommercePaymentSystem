## E-Commerce Payment System

# Author
Precious Nwajei

# Description
A professional C# console application simulating a comprehensive e-commerce checkout process. This project demonstrates advanced Object-Oriented Programming (OOP) concepts, focusing on Interface-driven design and Custom Exception Handling for secure and accurate payment transactions.

# Key Features
## 1. Unified Payment Interface (Part 1 & 2)
The system is built around a master IPaymentMethod interface, ensuring that all payment types (Credit Card, Bank Transfer, Mobile Money) provide standard logic for:

* Encapsulated Validation: Each class independently verifies its specific properties (e.g., 16-digit cards, 10-digit bank accounts, 11-digit mobile numbers starting with '0').
* Dynamic Fee Calculation: Automated processing fees calculated per method (2.5% for Credit Card, flat ₦100 for Bank, 1.5% for Mobile Money).
* Security-First Logging: Securely formatted transaction details featuring 3-4-3 data masking (e.g., 012****789) to protect sensitive user information.


## 2. Custom Exception Handling (Part 2)
The project replaces generic errors with a professional, centralized exception architecture.

* Custom Exception Classes: Implementation of InvalidPaymentException, InsufficientFundsException, PaymentDeclinedException, and TransactionTimeoutException.
* Contextual Feedback: Every exception carries relevant properties (like Amount or PaymentType) to provide specific user feedback.
* Try-Catch-Finally Logic: All transactions are wrapped in robust safety blocks, ensuring system stability and session-end logging even during failures.


## 3. Advanced Interface Logic (Part 3)
Demonstrates multi-interface implementation to enhance system capabilities:  

* IRefundable: Successfully implemented on CreditCardPayment and BankTransferPayment to provide specialized refund processing and timelines.
* ITransactionLogger: A dedicated TransactionLogger class that manages an internal collection of TransactionRecord objects for precise session auditing.


## 4. Menu System (Part 4)
A comprehensive, menu-driven interface with high-grade validation and method implementations:

* Defensive Input: Uses int.TryParse, decimal.TryParse, and DateTime.TryParseExact to eliminate crashes from "dirty" user input.
* Transparency: Displays exact transaction fees and total amounts before any processing occurs.
* Expert Verification: Includes a secure dual-factor refund check that cross-references Transaction IDs and Dates against the history list before allowing a refund.

## Project Structure

* Interfaces: Contains IPaymentMethod, IRefundable, and ITransactionLogger.
* Implementations: Contains the logic for Credit Card, Bank Transfer, and Mobile Money.
* Exceptions: Contains the custom Exception.cs file with specialized error classes.
* Logic: Contains the PaymentProcessor (core engine) and the TransactionLogger (data management).
* Program.cs: The main entry point handling UI logic and exception testing.

## Technical Skills Demonstrated

* Interface Implementation: Enforcing strict standards across multiple business modules.
* Multiple Interface Inheritance: Allowing single classes to fulfill multiple roles (Payment + Refund).
* Object-Oriented Validation: Moving business rules into implementation classes for reusability.
* Data Masking: Implementing security strings (e.g., showing only the first 3 and last 3 digits).
* Collection Management: Managing and searching a list of structured data for audit verification.

## How to Use the Application

   1. Startup: Run the application via Visual Studio or dotnet run.
   2. Payment: Select Options 1-3. Follow the specific validation prompts (e.g., 16 digits for Credit Card).
   3. Display Transaction Log: Select Option 5 to view the Global Transaction History in a column-aligned table.
   4. Process Refund: Select Option 4 for a Refund. You must provide a valid Transaction ID and Date (dd/MM/yyyy) from the history table to pass the security gate.
   5. Test Mode: Select Option 6 to see the system catch and describe custom exceptions in real-time.


## Method Location Table

| Category        | File/Class | Method/Property Name| Purpose |
|-----------------|------------|----------------------|---------|
| User Interface | Program.cs | DisplayMenu()         |Shows the 7-option main menu system. |
| Input Validation | Program.cs | GetValidatedAmount() | decimal.TryParse for numeric safety. |
| Input Validation | Program.cs | GetValidatedDate()   | Enforces dd/MM/yyyy to format date entry|
| Input Validation | Program.cs | GetValidatedString() | Prevents application crashes from empty or null inputs. |
| Logic Engine | PaymentProcessor.cs | ProcessTransaction() | Manages the try-catch flow for all payment methods. |
| Logic Bridge | PaymentProcessor.cs | VerifyForRefund() | Validates user-provided IDs against historical logs. |
| Business Rules | IPaymentMethod | ValidatePaymentInfo() | Implementation-side rules for card/bank/phone formats. |
| Refund Logic | IRefundable | ProcessRefund() | Handles returning funds for supported payment types. |
| Refund Logic | IRefundable | GetRefundProcessingDays() | Provides method-specific timelines (e.g., 3 days for cards). |
| Data Auditing | ITransactionLogger | LogTransaction() | Records attempt details, amounts, and success statuses. |
| Security Check | TransactionLogger.cs | IsTransactionValidForRefund() | Searches the List for successful IDs and matching dates. |



*Developed as part of the DotNet Cohort 5 Training.*

