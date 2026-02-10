using FluentAssertions;
using TimeScale.Application.Entities;
using TimeScale.Application.Helpers;
using TimeScale.Application.Interfaces;

namespace TimeScale.Application.Tests
{
    public class ResultCalculatorTests
    {
        private readonly IResultCalculator _calculator = new ResultCalculator();

        [Fact]
        public void Calculate_EmptyCollection_ThrowsInvalidOperationException()
        {
            Action act = () => _calculator.Calculate(Array.Empty<ValueRecord>());

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("No records");
        }

        [Fact]
        public void Calculate_SingleRecord_CalculatesCorrectly()
        {
            var record = new ValueRecord
            {
                DateUtc = new DateTime(2023, 1, 1, 0, 0, 0),
                ExecutionTime = 5,
                Value = 10
            };

            var result = _calculator.Calculate(new[] { record });

            result.StartDateUtc.Should().Be(record.DateUtc);
            result.TimeDeltaSeconds.Should().Be(0);
            result.AvgExecutionTime.Should().Be(5);
            result.AvgValue.Should().Be(10);
            result.MedianValue.Should().Be(10);
            result.MinValue.Should().Be(10);
            result.MaxValue.Should().Be(10);
        }

        [Fact]
        public void Calculate_MultipleRecords_CalculatesMedianAndStatsCorrectly()
        {
            var records = new[]
            {
            new ValueRecord { DateUtc = DateTime.UtcNow.AddSeconds(1), ExecutionTime = 2, Value = 5 },
            new ValueRecord { DateUtc = DateTime.UtcNow.AddSeconds(2), ExecutionTime = 4, Value = 15 },
            new ValueRecord { DateUtc = DateTime.UtcNow.AddSeconds(3), ExecutionTime = 6, Value = 10 }
        };

            var result = _calculator.Calculate(records);

            result.MinValue.Should().Be(5);
            result.MaxValue.Should().Be(15);
            result.MedianValue.Should().Be(10);
            result.AvgValue.Should().Be(10);
            result.AvgExecutionTime.Should().Be(4);
            result.TimeDeltaSeconds.Should().BeApproximately(
                (records.Max(r => r.DateUtc) - records.Min(r => r.DateUtc)).TotalSeconds,
                0.001
            );
        }

        [Fact]
        public void Calculate_EvenNumberOfRecords_CalculatesMedianCorrectly()
        {
            var records = new[]
            {
            new ValueRecord { DateUtc = DateTime.UtcNow, ExecutionTime = 1, Value = 1 },
            new ValueRecord { DateUtc = DateTime.UtcNow, ExecutionTime = 1, Value = 3 }
        };

            var result = _calculator.Calculate(records);

            result.MedianValue.Should().Be(2); // (1+3)/2
        }
    }
}
