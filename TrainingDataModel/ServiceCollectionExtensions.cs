using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TrainingDataModel.Data;

namespace TrainingDataModel
{
    /// <summary>
    /// Extension methods for configuring the Training Data Model with PostgreSQL
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the TrainingDbContext to the service collection with PostgreSQL configuration
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="connectionString">PostgreSQL connection string</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddTrainingDataModel(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<TrainingDbContext>(options =>
                options.UseNpgsql(connectionString, 
                    npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()));

            return services;
        }

        /// <summary>
        /// Adds the TrainingDbContext to the service collection with custom PostgreSQL configuration
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsAction">Action to configure DbContext options</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddTrainingDataModel(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction)
        {
            services.AddDbContext<TrainingDbContext>(optionsAction);

            return services;
        }
    }
}
