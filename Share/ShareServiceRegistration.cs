using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Share.Utiity;
using System.Reflection;


namespace Share
{
    public static class ShareServiceRegistration
    {
        public static IServiceCollection AddShareServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            // 工具類 Service
            services.AddScoped<IRequestMethod, RequestMethod>();

            return services;
        }
    }
}
