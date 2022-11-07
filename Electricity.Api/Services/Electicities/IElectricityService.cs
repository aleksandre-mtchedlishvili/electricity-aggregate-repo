using Aggregation.Api.Dtos;

namespace Electricity.Api.Services.Electicities
{
    public interface IElectricityService
    {
        Task DownloadAndAggregateCSVsAsync(int parseRowCountDesc, CancellationToken ct);
        Task<IEnumerable<ElectricityAggregateDto>> GetAggregatedDataAsync(CancellationToken ct);
    }
}