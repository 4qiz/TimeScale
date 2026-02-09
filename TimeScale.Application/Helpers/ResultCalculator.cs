
using TimeScale.Application.Interfaces;
using TimeScale.DataAccess.Entities;

namespace TimeScale.Application.Helpers
{
    public sealed class ResultCalculator : IResultCalculator
    {
        public Result Calculate(IEnumerable<ValueRecord> records)
        {
            var recordList = records as IList<ValueRecord>
                ?? records.ToList();

            if (recordList.Count == 0)
                throw new InvalidOperationException("No records");

            var values = recordList
                .Select(x => x.Value)
                .OrderBy(x => x)
                .ToArray();

            var dates = recordList
                .Select(x => x.DateUtc)
                .ToArray();

            return new Result
            {
                Id = Guid.NewGuid(),
                StartDateUtc = dates.Min(),
                TimeDeltaSeconds = (dates.Max() - dates.Min()).TotalSeconds,
                AvgExecutionTime = recordList.Average(x => x.ExecutionTime),
                AvgValue = values.Average(),
                MedianValue = CalculateMedian(values),
                MaxValue = values[^1],
                MinValue = values[0]
            };
        }

        private static double CalculateMedian(double[] sorted)
        {
            var count = sorted.Length;
            return count % 2 == 0
                ? (sorted[count / 2 - 1] + sorted[count / 2]) / 2
                : sorted[count / 2];
        }
    }

}
