using TimeScale.Application.Entities;

namespace TimeScale.Application.Interfaces
{
    public interface IResultRepository
    {
        IQueryable<Result> Query();
        Task RemoveAsync(Result result, CancellationToken ct);
    }
}
