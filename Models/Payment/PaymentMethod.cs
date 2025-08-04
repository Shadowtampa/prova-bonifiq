namespace ProvaPub.Models.Payment
{
    public abstract class PaymentMethod
    {
        public abstract Task<Order> ProcessPayment(decimal amount, int customerId);
    }
}