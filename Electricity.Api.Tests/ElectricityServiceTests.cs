using Aggregation.Api.Dtos;
using Aggregation.Api.Repositories;
using Electricity.Api.Services.CSVProcessor;
using Electricity.Api.Services.Electicities;
using Electricity.Api.Services.HtmlParser;
using NSubstitute;

namespace Electricity.Api.Tests
{
    public class ElectricityServiceTests
    {
        private readonly IHtmlParserService _htmlParserService = Substitute.For<IHtmlParserService>();
        private readonly IElectricityRepository _electricityRepository = Substitute.For<IElectricityRepository>();
        private readonly ICSVProcessorService _csvProcessorService = Substitute.For<ICSVProcessorService>();
        private readonly ElectricityService _sut;
        public ElectricityServiceTests()
        {
            _sut = new(_htmlParserService, _electricityRepository, _csvProcessorService);
        }

        [Fact]
        public async Task DownloadAndAggregateCSVs_Should_Make_Calls()
        {
            var parseLastRowDesc = 2;
            var urls = new List<string>() {
                "https://data.gov.lt/dataset/1975/download/10766/2022-05.csv",
                "https://data.gov.lt/dataset/1975/download/10765/2022-04.csv"};

            CancellationToken ct = new CancellationTokenSource().Token;

            _htmlParserService.GetCSVPublicUrlsFromParsedHtml(Arg.Is(parseLastRowDesc))
                              .Returns(a => urls);

            await _sut.DownloadAndAggregateCSVsAsync(parseLastRowDesc, ct);

            _htmlParserService.Received(1)
                                       .GetCSVPublicUrlsFromParsedHtml(Arg.Is(parseLastRowDesc));

            await _csvProcessorService.Received(1)
                                        .GetRecordsAsync(Arg.Is(urls[0]), ct);

            await _csvProcessorService.Received(1)
                                        .GetRecordsAsync(Arg.Is(urls[1]), ct);

            await _electricityRepository.Received(parseLastRowDesc)
                                       .SaveRecordsInNewScopeAsync(Arg.Any<ICollection<ElectricityAggregateDto>>(), ct);
        }
    }
}
