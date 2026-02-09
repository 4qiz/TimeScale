
using TimeScale.Application.Dtos;

namespace TimeScale.Application.Interfaces
{
    public interface ICsvDomainValidator
    {
        void Validate(IReadOnlyCollection<ValueRecordDto> records);
    }
}
