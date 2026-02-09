

namespace TimeScale.DataAccess.Entities
{
    public class ValueRecord
    {
        public long Id { get; set; }

        public Guid UploadedFileId { get; set; }

        public UploadedFile UploadedFile { get; set; } = null!;

        public DateTime DateUtc { get; set; }

        public double ExecutionTime { get; set; }

        public double Value { get; set; }
    }
}
