using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TrainingDataModel.Data;

namespace TrainingDataModel.Migrations
{
    /// <summary>
    /// Factory for creating TrainingDbContext for EF Core CLI tools
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<TrainingDbContext>
    {
        public TrainingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TrainingDbContext>();
            // TODO: Update connection string for your environment
            optionsBuilder.UseNpgsql(
                "Host=localhost;Database=trainingdbExp;Username=postgres;Password=admin",
                b => b.MigrationsAssembly("TrainingDataModel.Migrations")
            );
            return new TrainingDbContext(optionsBuilder.Options);
        }
    }
}
