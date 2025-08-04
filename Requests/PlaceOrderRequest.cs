using ProvaPub.Enums;

namespace ProvaPub.Requests
{
    public class PlaceOrderRequest
    {
        public PaymentMethod PaymentMethod { get; set; }

        public decimal PaymentValue { get; set; }
        public int CustomerId { get; set; }
    }
}