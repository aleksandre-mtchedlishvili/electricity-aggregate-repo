using Aggregation.Api.Dtos;
using Aggregation.Api.Entities;

namespace Aggregation.Api.Repositories
{
    public interface IElectricityRepository
    {
        Task<IEnumerable<ElectricityAggregate>> GetAsync(CancellationToken ct);
        Task SaveRecordsInNewScopeAsync(ICollection<ElectricityAggregateDto> records, CancellationToken ct);
    }
}
