using Microsoft.AspNetCore.Mvc;
using TimeScale.Application.Dtos;
using TimeScale.Application.Interfaces;

namespace TimeScale.Api.Controllers
{
    /// <summary>
    /// Контроллер для получения результатов обработки файлов.
    /// </summary>
    [ApiController]
    [Route("api/results")]
    public class ResultsController(IResultsQueryService service) : ControllerBase
    {
        /// <summary>
        /// Получает отфильтрованные результаты обработки файлов.
        /// </summary>
        /// <response code="200">Результаты успешно получены</response>
        /// <response code="400">Неверные параметры фильтрации</response>
        /// <response code="500">Внутренняя ошибка сервера при получении результатов</response>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ResultFilterDto filter, CancellationToken ct)
        {
            return Ok(await service.GetResultsAsync(filter, ct));
        }

        /// <summary>
        /// Получает последние 10 значенияй обработки для указанного файла.
        /// </summary>
        /// <response code="200">Последние значения успешно получены</response>
        /// <response code="404">Файл с указанным именем не найден</response>
        /// <response code="500">Внутренняя ошибка сервера при получении значений</response>
        [HttpGet("{fileName}/last")]
        public async Task<IActionResult> GetLast(string fileName, CancellationToken ct)
        {
            return Ok(await service.GetLastValuesAsync(fileName, ct));
        }
    }

}
