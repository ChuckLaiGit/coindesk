using Microsoft.AspNetCore.Hosting;

namespace 國泰.Utility
{
    public class Config
    {
        private static Config? _appSettings;

        public string appSettingValue { get; set; }

        public Config(IConfiguration config, string Key)
        {
            if (config != null)
            {
                this.appSettingValue = config.GetValue<string>(Key);
            }
        }


        private static Config GetCurrentSettings(string Key)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var settings = new Config(configuration, Key);

            return settings;
        }
    }
}
