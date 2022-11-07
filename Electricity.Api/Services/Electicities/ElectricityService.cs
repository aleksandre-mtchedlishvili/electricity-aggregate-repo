using Aggregation.Api.Dtos;
using Aggregation.Api.Repositories;
using Electricity.Api.Services.CSVProcessor;
using Electricity.Api.Services.HtmlParser;

namespace Electricity.Api.Services.Electicities
{
    public class ElectricityService : IElectricityService
    {
        private readonly IElectricityRepository _electricityRepository;
        private readonly IHtmlParserService _htmlParserService;
        private readonly ICSVProcessorService _csvProcessorService;

        public ElectricityService(IHtmlParserService htmlParserService, IElectricityRepository electricityRepository, ICSVProcessorService csvProcessorService)
        {
            _htmlParserService = htmlParserService;
            _electricityRepository = electricityRepository;
            _csvProcessorService = csvProcessorService;
        }

        public async Task DownloadAndAggregateCSVsAsync(int parseRowCountDesc, CancellationToken ct)
        {
            var csvUrls = _htmlParserService.GetCSVPublicUrlsFromParsedHtml(parseRowCountDesc);

            List<Task> csvWorderks = new();
            foreach (var url in csvUrls)
            {
                var task = Task.Run(async () =>
                {
                    var records = await _csvProcessorService.GetRecordsAsync(url, ct);

                    await _electricityRepository.SaveRecordsInNewScopeAsync(records, ct);

                }, ct);

                csvWorderks.Add(task);
            }

            await WaitForAlLCSVWorkersAsync(csvWorderks);
        }

        public async Task<IEnumerable<ElectricityAggregateDto>> GetAggregatedDataAsync(CancellationToken ct)
        {
            return (await _electricityRepository.GetAsync(ct))
                                                .Select(e => new ElectricityAggregateDto
                                                {
                                                    Count = e.Count,
                                                    PSum = e.PSum,
                                                    Tinklas = e.Tinklas
                                                });
        }

        private async Task WaitForAlLCSVWorkersAsync(List<Task> csvWorderks)
        {
            Task allTaskAwaiter = Task.WhenAll(csvWorderks);
            try
            {
                await allTaskAwaiter;
            }
            catch (Exception ex)
            {
                throw allTaskAwaiter?.Exception ?? ex;
            }
        }
    }
}
