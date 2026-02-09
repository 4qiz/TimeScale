
using TimeScale.Application.Dtos;

namespace TimeScale.Application.Interfaces
{
    public interface ICsvParser
    {
        Task<IReadOnlyCollection<ValueRecordDto>> ParseAsync(
            Stream csvStream,
            CancellationToken ct);
    }
}
