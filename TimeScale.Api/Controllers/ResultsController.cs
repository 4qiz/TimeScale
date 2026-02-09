using Microsoft.AspNetCore.Mvc;
using TimeScale.Application.Dtos;
using TimeScale.Application.Interfaces;

namespace TimeScale.Api.Controllers
{
    [ApiController]
    [Route("api/results")]
    public class ResultsController : ControllerBase
    {
        private readonly IResultsQueryService _service;

        public ResultsController(IResultsQueryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ResultFilterDto filter, CancellationToken ct)
            => Ok(await _service.GetResultsAsync(filter, ct));

        [HttpGet("{fileName}/last")]
        public async Task<IActionResult> GetLast(string fileName, CancellationToken ct)
            => Ok(await _service.GetLastValuesAsync(fileName, ct));
    }

}
