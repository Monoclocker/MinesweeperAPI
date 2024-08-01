using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinesweeperAPI.DataAccessLayer.Database;
using MinesweeperAPI.DataAccessLayer.Interfaces;
using MongoDB.Driver;

namespace MinesweeperAPI.DataAccessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IMongoClient>(new MongoClient(connectionString: configuration["connection"]));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
