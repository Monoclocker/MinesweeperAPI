using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinesweeperAPI.BusinessLogicLayer.Interfaces;
using MinesweeperAPI.BusinessLogicLayer.Services;
using MinesweeperAPI.DataAccessLayer;

namespace MinesweeperAPI.BusinessLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);

            services.AddScoped<IGameService, GameService>();

            return services;
        }
    }
}
