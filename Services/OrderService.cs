using ProvaPub.Models;
using ProvaPub.Models.Payment;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
	public class OrderService
	{
		private readonly TestDbContext _ctx;

		public OrderService(TestDbContext ctx)
		{
			_ctx = ctx;
		}

		public async Task<Order> PayOrder(PaymentMethod paymentMethod, decimal paymentValue, int customerId)
		{
			var order = await paymentMethod.ProcessPayment(paymentValue, customerId);

			return await InsertOrder(order);
		}

		public async Task<Order> InsertOrder(Order order)
		{
			//Insere pedido no banco de dados
			await _ctx.Orders.AddAsync(order);
			await _ctx.SaveChangesAsync(); 
			return order;
		}
	}
}
