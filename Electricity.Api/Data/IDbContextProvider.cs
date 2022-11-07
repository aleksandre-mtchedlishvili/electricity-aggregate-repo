using Aggregation.Api.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Electricity.Api.Data
{
    public interface IDbContextProvider
    {
        Task CommitAsync(IDbContextTransaction transaction, CancellationToken ct);
        Task RollbackAsync(IDbContextTransaction transaction, CancellationToken ct);
        IDbContextTransaction BeginTransaction();
    }
}
