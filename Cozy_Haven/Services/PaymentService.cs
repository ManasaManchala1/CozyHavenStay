using Cozy_Haven.Interfaces;

namespace Cozy_Haven.Services
{
    public class PaymentService:IPaymentService
    {
        public async Task<bool> Refund(int userId, float amount)
        {
            // Assume this method communicates with a payment gateway or external service
            // to process the refund. For demonstration purposes, we'll simulate a successful refund.
            // In a real application, you would interact with the payment gateway API.

            // Simulate a successful refund
            Console.WriteLine($"Refunding {amount} to user with ID {userId}");
            return true;
        }
    }
}
