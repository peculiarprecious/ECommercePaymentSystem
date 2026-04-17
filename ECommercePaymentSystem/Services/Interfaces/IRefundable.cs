namespace ECOMMERCEPAYMENTSYSTEM.Services.Interfaces
{
    public interface IRefundable
    {
        bool ProcessRefund(string transactionId, decimal amount);
        int GetRefundProcessingDays();
    }
}
