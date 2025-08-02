using ProvaPub.Models;

namespace ProvaPub.Services.Payment
{
    public interface IPaymentStrategy
    {
        Task<Order> ProcessPayment(decimal amount, int customerId);
    }
} 