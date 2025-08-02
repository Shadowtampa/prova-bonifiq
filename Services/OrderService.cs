using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services.Payment;

namespace ProvaPub.Services
{
	public class OrderService
	{
		private readonly TestDbContext _ctx;
		private readonly PaymentContext _paymentContext;

		public OrderService(TestDbContext ctx, PaymentContext paymentContext)
		{
			_ctx = ctx;
			_paymentContext = paymentContext;
		}

		public async Task<Order> PayOrder(PaymentMethod paymentMethod, decimal paymentValue, int customerId)
		{
			var order = await _paymentContext.ProcessPayment(paymentMethod, paymentValue, customerId);

			// Após o pagamento ser processado, insere o pedido
			return await InsertOrder(order);
		}

		public async Task<Order> InsertOrder(Order order)
		{
			//Insere pedido no banco de dados
			return (await _ctx.Orders.AddAsync(order)).Entity;
		}
	}
}
