using System.IO;
using Microsoft.Extensions.Configuration;
using StorageService.Contracts;

namespace StorageService.Services
{
  public class ConfigurationService : IConfigurationService
  {
    IConfiguration _configuration;
    public IConfiguration Configuration => _configuration;

    public ConfigurationService()
    {
      var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

      _configuration = builder.Build();

    }
    public string GetStorageConnectionString()
    {
      return _configuration["appsettings:connectionString"];
    }
  }
}