﻿using Microsoft.AspNetCore.Mvc;
using ProvaPub.Models;
using ProvaPub.Services;
using ProvaPub.Requests;
using ProvaPub.Models.Payment;

namespace ProvaPub.Controllers
{

    /// <summary>
    /// Esse teste simula um pagamento de uma compra.
    /// O método PayOrder aceita diversas formas de pagamento. Dentro desse método é feita uma estrutura de diversos "if" para cada um deles.
    /// Sabemos, no entanto, que esse formato não é adequado, em especial para futuras inclusões de formas de pagamento.
    /// Como você reestruturaria o método PayOrder para que ele ficasse mais aderente com as boas práticas de arquitetura de sistemas?
    /// 
    /// Outra parte importante é em relação à data (OrderDate) do objeto Order. Ela deve ser salva no banco como UTC mas deve retornar para o cliente no fuso horário do Brasil. 
    /// Demonstre como você faria isso.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class Parte3Controller : ControllerBase
    {
        private readonly OrderService _orderService;

        public Parte3Controller(OrderService orderService)
        {
            _orderService = orderService;
        }

        // Eu acho que deveria ser feito com post. 
        [HttpPost("orders")]
        public async Task<Order> PlaceOrder([FromForm] PlaceOrderRequest request)
        {
            PaymentMethod payment = request.PaymentMethod.ToLower() switch
            {
                "pix" => new PixPayment(),
                "credit_card" => new CreditCardPayment(),
                "paypal" => new PaypalPayment(),
                _ => throw new ArgumentException("Método de pagamento inválido"),
            };

            var order = await _orderService.PayOrder(
                payment,
                request.PaymentValue,
                request.CustomerId
            );

            return order;
        }
    }
}
