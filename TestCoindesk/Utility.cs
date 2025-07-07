using Db;
using Db.Libraries;
using Microsoft.Extensions.Configuration;


namespace Application.UnitTest
{
    public static class Utility
    {
        public static DBContextFactory<EFContext> GetTestDb()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables();
            IConfiguration config = builder.Build();
            return new DBContextFactory<EFContext>(config);
        }
    }
}
