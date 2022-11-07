using Aggregation.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Electricity.Api.Data
{
    public class DbContextProvider : IDbContextProvider
    {
        private readonly ElectricityDbContext _dbContext;

        public DbContextProvider(ElectricityDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public IDbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }

        public async Task CommitAsync(IDbContextTransaction transaction, CancellationToken ct)
        {
            await transaction.CommitAsync(ct);
        }

        public async Task RollbackAsync(IDbContextTransaction transaction, CancellationToken ct)
        {
            await transaction.RollbackAsync(ct);
        }
    }
}
