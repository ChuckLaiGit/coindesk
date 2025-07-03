using Contract.Repository;
using Infrastructure.CoinChart;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class InfrastructureRegistraction
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 先註冊 IDbConnection 的實現

            services.AddScoped<IBPIRepository, BPIRepository>();
            services.AddScoped<IChartRepository, ChartRepository>();
            return services;
        }
    }
}
