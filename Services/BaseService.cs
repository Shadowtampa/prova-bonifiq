using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Interfaces;

namespace ProvaPub.Services
{
    public abstract class BaseService
    {
        protected readonly TestDbContext _ctx;
        protected const int _totalCount = 10;

        protected BaseService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        protected RecordList<T> CreatePaginatedList<T>(IQueryable<T> query, int page) where T: IHasId
        {
            var totalCount = query.Count();
            var records = query
                .OrderBy(record => record.Id) //Pra ordenar por ID
                .Skip((page - 1) * _totalCount) //Pra pular a pesquisa
                .Take(_totalCount) //determinação do número de itens que vou pegar
                .ToList(); //Pra converter pra lista

            return new RecordList<T>()
            {
                Records = records,
                TotalCount = totalCount,
                HasNext = (page * _totalCount) < totalCount
            };
        }
    }
}