namespace TimeScale.Api.Dtos
{
    /// <summary>
    /// Represents the multipart form payload for CSV uploads.
    /// </summary>
    public sealed class UploadCsvForm
    {
        /// <summary>
        /// The uploaded CSV file.
        /// </summary>
        public IFormFile File { get; init; } = null!;
    }
}
