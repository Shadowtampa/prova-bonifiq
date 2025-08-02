using ProvaPub.Services;
using ProvaPub.Models;

namespace ProvaPub.Services.Payment
{
    public class CreditCardPaymentStrategy : IPaymentStrategy
    {
        public async Task<Order> ProcessPayment(decimal amount, int customerId)
        {
            return new Order
            {
                Value = amount,
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow
            };

        }
    }
}