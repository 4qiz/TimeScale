using Microsoft.EntityFrameworkCore;
using TimeScale.Application.Entities;
using TimeScale.Application.Interfaces;

namespace TimeScale.DataAccess.Repositories
{
    public sealed class UploadedFileRepository(AppDbContext db) : IUploadedFileRepository
    {
        public async Task<UploadedFile?> GetByFileNameWithRelationsAsync(string fileName, CancellationToken ct)
        {
            return await db.UploadedFiles
                .Include(x => x.ValueRecords)
                .Include(x => x.Result)
                .FirstOrDefaultAsync(x => x.FileName == fileName, ct);
        }

        public async Task AddAsync(UploadedFile file, CancellationToken ct)
        {
            await db.UploadedFiles.AddAsync(file, ct);
        }

        public Task RemoveAsync(UploadedFile file, CancellationToken ct)
        {
            db.UploadedFiles.Remove(file);
            return Task.CompletedTask;
        }
    }
}
