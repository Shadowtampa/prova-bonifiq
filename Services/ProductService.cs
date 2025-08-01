using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
	public class ProductService : BaseService
	{
		public ProductService(TestDbContext ctx) : base(ctx)
		{
		}

		public RecordList<Product> ListProducts(int page)
		{
			return CreatePaginatedList(_ctx.Products, page);
		}
	}
}
