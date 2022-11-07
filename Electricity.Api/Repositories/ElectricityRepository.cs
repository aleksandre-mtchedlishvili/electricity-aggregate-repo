using Aggregation.Api.Data;
using Aggregation.Api.Dtos;
using Aggregation.Api.Entities;
using Electricity.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Aggregation.Api.Repositories
{
    public class ElectricityRepository : IElectricityRepository
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ElectricityDbContext _dbContext;
        private readonly IDbContextProvider _dbContextProvider;
        public ElectricityRepository(IServiceScopeFactory serviceScopeFactory, ElectricityDbContext dbContext, IDbContextProvider dbContextProvider)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _dbContext = dbContext;
            _dbContextProvider = dbContextProvider;
        }
        public async Task SaveRecordsInNewScopeAsync(ICollection<ElectricityAggregateDto> records, CancellationToken ct)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ElectricityDbContext>()!;
                using var transaction = _dbContextProvider.BeginTransaction();

                try
                {
                    var tinklas = records.Select(e => e.Tinklas);
                    var existingData = await dbContext.ElectricityAggregates.Where(e => tinklas.Contains(e.Tinklas))
                                                                             .ToDictionaryAsync(e => e.Tinklas, ct);

                    foreach (var electricity in records)
                    {
                        if (existingData.ContainsKey(electricity.Tinklas))
                        {
                            var entity = existingData[electricity.Tinklas];
                            entity.Count += electricity.Count;
                            entity.PSum += electricity.PSum;
                            dbContext.Update(entity);
                        }
                        else
                        {
                            await dbContext.ElectricityAggregates.AddAsync(new ElectricityAggregate
                            {
                                Tinklas = electricity.Tinklas,
                                Count = electricity.Count,
                                PSum = electricity.PSum
                            }, ct);
                        }
                    }

                    await dbContext.SaveChangesAsync(ct);
                    await _dbContextProvider.CommitAsync(transaction, ct);
                    records.Clear();
                }
                catch (Exception ex)
                {
                    await _dbContextProvider.RollbackAsync(transaction, ct);
                    throw;
                }
            }
        }
        public async Task<IEnumerable<ElectricityAggregate>> GetAsync(CancellationToken ct)
        {
            return await _dbContext.ElectricityAggregates.ToListAsync(ct);
        }
    }
}
