namespace Cozy_Haven.Interfaces
{
    public interface IPaymentService
    {
        public Task<bool> Refund(int userId, float amount);
    }
}
