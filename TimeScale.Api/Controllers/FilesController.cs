using Microsoft.AspNetCore.Mvc;
using TimeScale.Api.Dtos;
using TimeScale.Application.Dtos;
using TimeScale.Application.Interfaces;

namespace TimeScale.Api.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly IFileProcessingService _service;

        public FilesController(IFileProcessingService service)
        {
            _service = service;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(
            [FromForm] UploadCsvForm form,
            CancellationToken ct)
        {
            if (form.File == null || form.File.Length == 0)
                return BadRequest("File is empty");

            await _service.UploadAndProcessAsync(
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
