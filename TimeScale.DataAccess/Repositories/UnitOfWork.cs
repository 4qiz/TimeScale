using Microsoft.EntityFrameworkCore.Storage;
using TimeScale.Application.Interfaces;

namespace TimeScale.DataAccess.Repositories
{
    public sealed class UnitOfWork(AppDbContext db) : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;

        public async Task BeginTransactionAsync(CancellationToken ct)
        {
            _transaction = await db.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitAsync(CancellationToken ct)
        {
            if (_transaction != null)
                await _transaction.CommitAsync(ct);
        }

        public async Task RollbackAsync(CancellationToken ct)
        {
            if (_transaction != null)
                await _transaction.RollbackAsync(ct);
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => db.SaveChangesAsync(ct);
    }

}
