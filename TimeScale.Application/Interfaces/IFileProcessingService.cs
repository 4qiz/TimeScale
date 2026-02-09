

using TimeScale.Application.Dtos;

namespace TimeScale.Application.Interfaces
{
    public interface IFileProcessingService
    {
        Task UploadAndProcessAsync(UploadCsvCommand command, CancellationToken ct);
    }
}
