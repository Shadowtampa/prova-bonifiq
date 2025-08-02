using System.Collections.Generic;
using ProvaPub.Models;

namespace ProvaPub.Services.Payment
{
    public class PaymentContext
    {
        private readonly Dictionary<PaymentMethod, IPaymentStrategy> _strategies;

        public PaymentContext()
        {
            _strategies = new Dictionary<PaymentMethod, IPaymentStrategy>
            {
                { PaymentMethod.Pix, new PixPaymentStrategy() },
                { PaymentMethod.CreditCard, new CreditCardPaymentStrategy() },
                { PaymentMethod.PayPal, new PayPalPaymentStrategy() }
            };
        }

        //É se suma importância adicionar validação aqui
        public async Task<Order> ProcessPayment(PaymentMethod paymentMethod, decimal amount, int customerId)
        {
            if (!_strategies.ContainsKey(paymentMethod))
            {
                throw new ArgumentException($"Payment method '{paymentMethod}' is not supported.");
            }

            var strategy = _strategies[paymentMethod];
            return await strategy.ProcessPayment(amount, customerId);
        }
    }
} 