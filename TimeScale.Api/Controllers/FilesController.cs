using Microsoft.AspNetCore.Mvc;
using TimeScale.Api.Dtos;
using TimeScale.Application.Dtos;
using TimeScale.Application.Interfaces;

namespace TimeScale.Api.Controllers
{
    /// <summary>
    /// Контроллер для обработки файлов.
    /// Предоставляет эндпоинты для загрузки и обработки CSV файлов.
    /// </summary>
    [ApiController]
    [Route("api/files")]
    public class FilesController(IFileProcessingService service) : ControllerBase
    {

        /// <summary>
        /// Загружает и обрабатывает CSV файл.
        /// </summary>
        /// <response code="200">Файл успешно принят в обработку</response>
        /// <response code="400">Файл отсутствует или имеет нулевой размер</response>
        /// <response code="500">Внутренняя ошибка сервера при обработке файла</response>
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
