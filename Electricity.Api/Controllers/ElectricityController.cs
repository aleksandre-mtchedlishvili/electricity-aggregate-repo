using Aggregation.Api.Data;
using Aggregation.Api.Dtos;
using Aggregation.Api.Entities;
using Aggregation.Api.Repositories;
using CsvHelper;
using Electricity.Api.Services.Electicities;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace Aggregation.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ElectricityController : ControllerBase
    {
        private readonly ILogger<ElectricityController> _logger;
        private readonly IElectricityService _electricityService;

        public ElectricityController(ILogger<ElectricityController> logger, IElectricityService electricityService)
        {
            _logger = logger;
            _electricityService = electricityService;
        }

        [HttpGet("AggregateCSVData")]
        public async Task<IActionResult> AggregateCSVData(CancellationToken ct)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            _logger.LogWarning("Started processing request");

            int parseRowsDescendingCount = 2;

            await _electricityService.DownloadAndAggregateCSVsAsync(parseRowsDescendingCount, ct);

            stopWatch.Stop();

            _logger.LogWarning($"Processing finished in:{stopWatch.ElapsedMilliseconds} milli seconds");
            return Ok();
        }

        [HttpGet()]
        public async Task<IActionResult> GetAggregatedData(CancellationToken ct)
        {
            var result = await _electricityService.GetAggregatedDataAsync(ct);

            return Ok(result);
        }
    }
}