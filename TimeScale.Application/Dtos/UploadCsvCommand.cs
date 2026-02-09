

namespace TimeScale.Application.Dtos
{
    public class UploadCsvCommand
    {
        public string FileName { get; init; } = null!;
        public Stream CsvStream { get; init; } = null!;
    }
}
