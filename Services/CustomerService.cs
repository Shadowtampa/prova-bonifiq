using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Requests;

namespace ProvaPub.Services
{
    public class CustomerService : BaseService
    {
        public CustomerService(TestDbContext ctx) : base(ctx)
        {
        }

        public RecordList<Customer> ListCustomers(int page)
        {
            return CreatePaginatedList(_ctx.Customers, page);
        }

        public async Task<Customer> Get(int customerId)
        {
            return await _ctx.Customers.FirstAsync(c => c.Id == customerId);
        }
    }
}
