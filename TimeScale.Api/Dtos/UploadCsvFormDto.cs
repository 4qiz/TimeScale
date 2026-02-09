namespace TimeScale.Api.Dtos
{
    public sealed class UploadCsvForm
    {
        public IFormFile File { get; init; } = null!;
    }
}
