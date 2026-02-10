

namespace TimeScale.Application.Entities
{
    public class UploadedFile
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = null!;

        public DateTime UploadedAtUtc { get; set; }

        public int RowsCount { get; set; }

        public ICollection<ValueRecord> ValueRecords { get; set; } = [];

        public Result? Result { get; set; }
    }
}
