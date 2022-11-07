using Aggregation.Api.Dtos;
using CsvHelper;
using System.Globalization;

namespace Electricity.Api.Services.CSVProcessor
{
    public class CSVProcessorService : ICSVProcessorService
    {
        private readonly HttpClient _httpClient;

        public CSVProcessorService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<ICollection<ElectricityAggregateDto>> GetRecordsAsync(string url, CancellationToken ct)
        {
            using var stream = await _httpClient.GetStreamAsync(url, ct);

            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, ct);
            memoryStream.Position = 0;

            using var reader = new StreamReader(memoryStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<ElectricityDto>().Where(e => e.ObtPavadinimas == "Butas")
                                                          .GroupBy(e => e.Tinklas)
                                                          .Select(e => new ElectricityAggregateDto
                                                          {
                                                              Tinklas = e.Key,
                                                              Count = e.Count(),
                                                              PSum = e.Sum(t => (t.PPlus ?? 0) + (t.PMinus ?? 0))
                                                          })
                                                          .ToList();

            return records;
        }
    }
}
