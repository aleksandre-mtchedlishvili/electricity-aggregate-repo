using Aggregation.Api.Data;
using Aggregation.Api.Dtos;
using Aggregation.Api.Repositories;
using Electricity.Api.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Electricity.Api.Tests
{
    public class ElectricityRepositoryTests
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = Substitute.For<IServiceScopeFactory>();
        private readonly IDbContextProvider _dbContextProvider = Substitute.For<IDbContextProvider>();
        private readonly ElectricityDbContext _dbContext;
        private readonly ElectricityRepository _sut;

        public ElectricityRepositoryTests()
        {
            var builder = new DbContextOptionsBuilder<ElectricityDbContext>();
            builder.UseInMemoryDatabase(databaseName: "ElectricityDb");

            _dbContext = new ElectricityDbContext(builder.Options);

            _sut = new ElectricityRepository(_serviceScopeFactory, _dbContext, _dbContextProvider);
        }

        [Fact]
        public async Task SaveRecordsInNewScope_Should_Save_Records()
        {
            var records = new List<ElectricityAggregateDto>()
            {
                new ElectricityAggregateDto
                {
                    Count=1,
                    PSum=1,
                    Tinklas="Test"
                }
            };

            var ct = new CancellationTokenSource().Token;

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<ElectricityDbContext>(_dbContext);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _serviceScopeFactory.CreateScope().Returns(serviceProvider.CreateScope());

            await _sut.SaveRecordsInNewScopeAsync(records, ct);

            _dbContext.ElectricityAggregates.Count().Should().Be(1);

            var record = _dbContext.ElectricityAggregates.First();

            record.Tinklas.Should().Be("Test");
            record.Count.Should().Be(1);
            record.PSum.Should().Be(1);

        }
    }
}
