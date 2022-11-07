using Aggregation.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aggregation.Api.Data
{
    public class ElectricityDbContext : DbContext
    {
        public ElectricityDbContext(DbContextOptions options) : base(options)
        {

        }


        public DbSet<ElectricityAggregate> ElectricityAggregates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ElectricityAggregate>()
                        .HasKey(e => e.Id);

            modelBuilder.Entity<ElectricityAggregate>()
                        .Property(e => e.Tinklas)
                        .HasMaxLength(250);

            modelBuilder.Entity<ElectricityAggregate>()
                        .HasIndex(e => e.Tinklas)
                        .IsUnique();
        }
    }
}
