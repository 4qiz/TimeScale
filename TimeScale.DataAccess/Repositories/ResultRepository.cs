
using TimeScale.Application.Entities;
using TimeScale.Application.Interfaces;

namespace TimeScale.DataAccess.Repositories
{
    public sealed class ResultRepository(AppDbContext db) : IResultRepository
    {
        public IQueryable<Result> Query()
        {
            return db.Results.AsQueryable();
        }

        public Task RemoveAsync(Result result, CancellationToken ct)
        {
            db.Results.Remove(result);
            return Task.CompletedTask;
        }
    }
}
