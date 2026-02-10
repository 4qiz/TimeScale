using TimeScale.Application.Entities;

namespace TimeScale.Application.Interfaces
{
    public interface IValueRecordRepository
    {
        IQueryable<ValueRecord> Query();
        Task RemoveRangeAsync(IEnumerable<ValueRecord> records, CancellationToken ct);
    }
}
