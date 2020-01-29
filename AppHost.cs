using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StorageService.Contracts;

namespace StorageDemo
{
  public class AppHost : IAppHost
  {
    private readonly IConfigurationService _configurationService;

    public AppHost(IConfigurationService configurationService)
    {
      _configurationService = configurationService;
    }

    public async Task Run()
    {
      Console.WriteLine(_configurationService.GetStorageConnectionString());
    }
  }
}