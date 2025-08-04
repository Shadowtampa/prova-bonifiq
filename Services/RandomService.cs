using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public class RandomService
    {
        private readonly int seed;
        private readonly TestDbContext _ctx;

        private readonly Random _random;
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
                await _ctx.Numbers.AddAsync(new RandomNumber() { Number = number });
                await _ctx.SaveChangesAsync();
                return number;
            }
            catch (DbUpdateException exception)
            {
                if (exception.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
                {
                    //Estou colocando console.log padrãozinho, mas poderia ser um sistema de logs mais arquitetado. 
                    Console.WriteLine("Número duplicado.");
                    return number;
                }
                throw;
            }
        }

    }
}
