using Microsoft.AspNetCore.Mvc;
using TimeScale.Application.Dtos;
using TimeScale.Application.Interfaces;

namespace TimeScale.Api.Controllers
{
    /// <summary>
    /// Retrieves processed results and recent values.
    /// </summary>
    [ApiController]
    [Route("api/results")]
    public class ResultsController(IResultsQueryService service) : ControllerBase
    {
        /// <summary>
        /// Retrieves filtered processing results.
        /// </summary>
        /// <param name="filter">The filtering criteria.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <response code="200">Results were retrieved successfully.</response>
        /// <response code="400">The filter values are invalid.</response>
        /// <response code="500">An internal server error occurred while retrieving results.</response>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ResultFilterDto filter, CancellationToken ct)
        {
            return Ok(await service.GetResultsAsync(filter, ct));
        }

        /// <summary>
        /// Retrieves the latest ten values for a given file.
        /// </summary>
        /// <param name="fileName">The file name to inspect.</param>
        /// <param name="ct">A cancellation token.</param>
        /// <response code="200">The latest values were retrieved successfully.</response>
        /// <response code="404">The file could not be found.</response>
        /// <response code="500">An internal server error occurred while retrieving values.</response>
        [HttpGet("{fileName}/last")]
        public async Task<IActionResult> GetLast(string fileName, CancellationToken ct)
        {
            return Ok(await service.GetLastValuesAsync(fileName, ct));
        }
    }
}
