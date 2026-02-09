
using TimeScale.Application.Dtos;

namespace TimeScale.Application.Interfaces
{
    public interface IResultsQueryService
    {
        Task<IReadOnlyCollection<ResultDto>> GetResultsAsync(ResultFilterDto filter, CancellationToken ct);
        Task<IReadOnlyCollection<ValueRecordDto>> GetLastValuesAsync(string fileName, CancellationToken ct);
    }
}
