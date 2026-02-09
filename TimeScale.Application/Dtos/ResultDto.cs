

namespace TimeScale.Application.Dtos
{
    public class ResultDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = null!;
        public DateTime StartDateUtc { get; set; }
        public double TimeDeltaSeconds { get; set; }
        public double AvgExecutionTime { get; set; }
        public double AvgValue { get; set; }
        public double MedianValue { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
    }

}
