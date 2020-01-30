using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorageService.Contracts;
using StorageService.Services;

namespace StorageDemo
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var serviceCollection = new ServiceCollection();

      var serviceProvider = BuildIoC();

      await serviceProvider.GetService<IAppHost>().Run();
    }

    private static ServiceProvider BuildIoC()
    {
      return new ServiceCollection()
            .AddSingleton<IAppHost, AppHost>()
            .AddSingleton<IConfigurationService, ConfigurationService>()
            .AddSingleton<IStorageHelperService, StorageHelperService>()
            .BuildServiceProvider();
    }
  }
}
