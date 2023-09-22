using Microsoft.Extensions.Configuration;
using System.IO;

namespace ConsoleSoccerApplication.Services
{
    public class ConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public string GetConnectionString()
        {
            return _configuration.GetConnectionString("DefaultConnection");
        }

        public string GetAuthToken()
        {
            return _configuration["AppSettings:AuthToken"];
        }

        public string GetAzureTranslatorApiKey()
        {
            return _configuration["Azure:TranslatorApiKey"];
        }
    }
}
