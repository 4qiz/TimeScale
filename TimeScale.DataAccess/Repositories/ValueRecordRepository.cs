
using TimeScale.Application.Entities;
using TimeScale.Application.Interfaces;

namespace TimeScale.DataAccess.Repositories
{
    public sealed class ValueRecordRepository(AppDbContext db) : IValueRecordRepository
    {
        public IQueryable<ValueRecord> Query()
        {
            return db.ValueRecords.AsQueryable();
        }

        public Task RemoveRangeAsync(IEnumerable<ValueRecord> records, CancellationToken ct)
        {
            db.ValueRecords.RemoveRange(records);
            return Task.CompletedTask;
        }
    }
}
