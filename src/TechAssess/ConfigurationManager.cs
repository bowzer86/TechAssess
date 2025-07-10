using Microsoft.Extensions.Configuration;

namespace TechAssess;

public static class AppConfiguration
{
    private static IConfigurationRoot _configuration;

    static AppConfiguration()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public static IConfiguration AppSettings => _configuration;
}