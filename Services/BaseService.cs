using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public abstract class BaseService
    {
        protected readonly TestDbContext _ctx;
        protected const int _TotalCount = 10;

        protected BaseService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        protected RecordList<T> CreatePaginatedList<T>(IQueryable<T> query, int page)
        {
            var totalCount = query.Count();
            var records = query
                .Skip((page - 1) * _TotalCount)
                .Take(_TotalCount)
                .ToList();

            return new RecordList<T>()
            {
                Records = records,
                TotalCount = totalCount,
                HasNext = (page * _TotalCount) < totalCount
            };
        }
    }
} 