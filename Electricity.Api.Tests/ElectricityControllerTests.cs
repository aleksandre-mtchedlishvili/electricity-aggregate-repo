using Aggregation.Api.Controllers;
using Electricity.Api.Services.Electicities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Electricity.Api.Tests
{
    public class ElectricityControllerTests
    {
        private readonly IElectricityService _electricityService = Substitute.For<IElectricityService>();
        private readonly ILogger<ElectricityController> _logger = Substitute.For<ILogger<ElectricityController>>();
        private readonly ElectricityController _sut;
        private readonly CancellationToken ct;
        public ElectricityControllerTests()
        {
            _sut = new ElectricityController(_logger, _electricityService);
            ct = new CancellationTokenSource().Token;
        }

        [Fact]
        public async Task AggregateCSVData__Should_Call_Service()
        {
            int parseRawCountDesc = 2;

            var result = await _sut.AggregateCSVData(ct);

            await _electricityService.Received(1)
                                     .DownloadAndAggregateCSVsAsync(Arg.Is(parseRawCountDesc), ct);

            result.Should().BeOfType<OkResult>();
        }
    }
}
