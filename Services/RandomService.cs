using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public class RandomService
    {
        int seed;
        TestDbContext _ctx;

        Random _random;
        public RandomService(TestDbContext ctx)
        {

            seed = Guid.NewGuid().GetHashCode();

            _ctx = ctx;

            _random = new Random(seed);
        }
        public async Task<int> GetRandom()
        {
            var number = _random.Next(100);
            try
            {
                _ctx.Numbers.Add(new RandomNumber() { Number = number });
                _ctx.SaveChanges();
                return number;
            }
            catch (DbUpdateException exception)
            {
                Console.WriteLine("Número duplicado.");
                return number;

            }
        }

    }
}
