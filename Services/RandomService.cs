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
        public RandomService()
        {
            var contextOptions = new DbContextOptionsBuilder<TestDbContext>()
    .UseSqlServer(@"Server=localhost,1433;Database=Teste;User Id=sa;Password=Bonifiq123;Encrypt=True;TrustServerCertificate=True;")
    .Options;
            seed = Guid.NewGuid().GetHashCode();

            _ctx = new TestDbContext(contextOptions);

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
                if (exception.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
                {
                    Console.WriteLine("Número duplicado.");
                    return number;
                }
                throw; 
            }
        }

    }
}
