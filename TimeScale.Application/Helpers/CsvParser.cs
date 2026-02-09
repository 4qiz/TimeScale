
using System.Globalization;

using TimeScale.Application.Dtos;
using TimeScale.Application.Exceptions;
using TimeScale.Application.Interfaces;

namespace TimeScale.Application.Helpers
{
    public sealed class CsvParser : ICsvParser
    {
        public async Task<IReadOnlyCollection<ValueRecordDto>> ParseAsync(
            Stream csvStream,
            CancellationToken ct)
        {
            var records = new List<ValueRecordDto>(capacity: 1024);

            using var reader = new StreamReader(csvStream, leaveOpen: true);

            var header = await reader.ReadLineAsync(ct);
            if (header != "Date;ExecutionTime;Value")
                throw new ValidationException("Invalid CSV header");

            string? line;
            while ((line = await reader.ReadLineAsync(ct)) != null)
            {
                ct.ThrowIfCancellationRequested();

                var parts = line.Split(';');
                if (parts.Length != 3)
                    throw new ValidationException("Invalid CSV format");

                if (!DateTime.TryParse(
                        parts[0],
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AdjustToUniversal,
                        out var date))
                    throw new ValidationException("Invalid Date");

                if (!double.TryParse(
                        parts[1],
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out var executionTime))
                    throw new ValidationException("Invalid ExecutionTime");

                if (!double.TryParse(
                        parts[2],
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out var value))
                    throw new ValidationException("Invalid Value");

                records.Add(new ValueRecordDto
                {
                    DateUtc = date,
                    ExecutionTime = executionTime,
                    Value = value
                });
            }

            return records;
        }
    }

}
