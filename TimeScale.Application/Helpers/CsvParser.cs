
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
            var records = new List<ValueRecordDto>(1024);

            using var reader = new StreamReader(csvStream, leaveOpen: true);

            var header = await reader.ReadLineAsync(ct);
            if (!string.Equals(header, "Date;ExecutionTime;Value", StringComparison.Ordinal))
                throw new ValidationException("Invalid CSV header");

            string? line;

            while ((line = await reader.ReadLineAsync(ct)) != null)
            {
                ct.ThrowIfCancellationRequested();

                var span = line.AsSpan();

                int firstSep = span.IndexOf(';');
                int secondSep = span.Slice(firstSep + 1).IndexOf(';');

                if (firstSep < 0 || secondSep < 0)
                    throw new ValidationException("Invalid CSV format");

                secondSep += firstSep + 1;

                var dateSpan = span.Slice(0, firstSep);
                var execSpan = span.Slice(firstSep + 1, secondSep - firstSep - 1);
                var valueSpan = span.Slice(secondSep + 1);

                if (!DateTime.TryParseExact(
                        dateSpan,
                        "yyyy-MM-dd'T'HH:mm:ss'Z'",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                        out var date))
                    throw new ValidationException("Invalid Date");

                if (!double.TryParse(execSpan, NumberStyles.Float, CultureInfo.InvariantCulture, out var executionTime))
                    throw new ValidationException("Invalid ExecutionTime");

                if (!double.TryParse(valueSpan, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
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
