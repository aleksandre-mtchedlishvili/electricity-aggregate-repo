using Aggregation.Api.Dtos;

namespace Electricity.Api.Services.CSVProcessor
{
    public interface ICSVProcessorService
    {
        Task<ICollection<ElectricityAggregateDto>> GetRecordsAsync(string url, CancellationToken ct);
    }
}