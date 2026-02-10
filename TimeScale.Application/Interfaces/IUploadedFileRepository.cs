using TimeScale.Application.Entities;

namespace TimeScale.Application.Interfaces
{
    public interface IUploadedFileRepository
    {
        Task<UploadedFile?> GetByFileNameWithRelationsAsync(string fileName, CancellationToken ct);
        Task AddAsync(UploadedFile file, CancellationToken ct);
        Task RemoveAsync(UploadedFile file, CancellationToken ct);
    }
}
