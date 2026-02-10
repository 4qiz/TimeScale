

namespace TimeScale.Application.Entities
{
    public class Result
    {
        public Guid Id { get; set; }

        public Guid UploadedFileId { get; set; }

        public UploadedFile UploadedFile { get; set; } = null!;

        public double TimeDeltaSeconds { get; set; }

        public DateTime StartDateUtc { get; set; }

        public double AvgExecutionTime { get; set; }

        public double AvgValue { get; set; }

        public double MedianValue { get; set; }

        public double MaxValue { get; set; }

        public double MinValue { get; set; }
    }
}
