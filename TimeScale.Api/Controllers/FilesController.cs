using Microsoft.AspNetCore.Mvc;
using TimeScale.Api.Dtos;
using TimeScale.Application.Dtos;
using TimeScale.Application.Interfaces;

namespace TimeScale.Api.Controllers
{
    /// <summary>
    /// Handles CSV file uploads and processing requests.
    /// </summary>
    [ApiController]
    [Route("api/files")]
    public class FilesController(IFileProcessingService service) : ControllerBase
    {
        /// <summary>
        /// Uploads and processes a CSV file.
        /// </summary>
        /// <param name="form">The multipart form containing the upload payload.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <response code="200">The file was accepted for processing.</response>
        /// <response code="400">The file is missing or invalid.</response>
        /// <response code="500">An internal server error occurred while processing the file.</response>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(
            [FromForm] UploadCsvForm form,
            CancellationToken ct)
        {
            if (form.File == null || form.File.Length == 0)
                return BadRequest("File is empty");

            await service.UploadAndProcessAsync(
                new UploadCsvCommand
                {
                    FileName = form.File.FileName,
                    CsvStream = form.File.OpenReadStream()
                },
                ct);

            return Ok();
        }
    }
}
