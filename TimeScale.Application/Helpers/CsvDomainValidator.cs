
using TimeScale.Application.Dtos;
using TimeScale.Application.Exceptions;
using TimeScale.Application.Interfaces;

namespace TimeScale.Application.Helpers
{
    public sealed class CsvDomainValidator : ICsvDomainValidator
    {
        private static readonly DateTime MinDate = new(2000, 1, 1);

        public void Validate(IReadOnlyCollection<ValueRecordDto> records)
        {
            if (records.Count is < 1 or > 10_000)
                throw new ValidationException("Rows count must be between 1 and 10000");

            foreach (var r in records)
            {
                if (r.DateUtc < MinDate || r.DateUtc > DateTime.UtcNow)
                    throw new ValidationException("Date out of allowed range");

                if (r.ExecutionTime < 0)
                    throw new ValidationException("ExecutionTime must be >= 0");

                if (r.Value < 0)
                    throw new ValidationException("Value must be >= 0");
            }
        }
    }

}
