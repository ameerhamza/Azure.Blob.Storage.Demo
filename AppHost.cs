using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StorageService.Contracts;

namespace StorageDemo
{
  public class AppHost : IAppHost
  {
    private readonly IStorageHelperService _storageHelperService;

    public AppHost(IStorageHelperService storageHelperService)
    {
      _storageHelperService = storageHelperService;
    }

    public async Task Run()
    {
      Console.Clear();

      Console.WriteLine("Enter 1 to create a blob");
      Console.WriteLine("Enter 2 to list a blob");
      Console.WriteLine("Enter 3 to download a blob");

      var option = Console.ReadLine();
      Console.WriteLine("option selected", option);

      if (option == "1")
      {
        Console.WriteLine("Enter File Path");
        var filePath = Console.ReadLine();

        Console.WriteLine("Enter Blob Name");
        var blobName = Console.ReadLine();
        if (await _storageHelperService.CheckIfBlobExistsAsync(blobName))
        {
          Console.WriteLine("Blob already exists");
        }
        else
        {
          await _storageHelperService.UploadVideoAsync(await File.ReadAllBytesAsync(filePath), blobName);
        }
      }
      else if (option == "2")
      {
        foreach (var blob in await _storageHelperService.ListVideoBlobsAsync())
        {
          Console.WriteLine($"Blob Name : {blob.Name}");
        }
      }
      else if (option == "3")
      {

        Console.WriteLine("Enter the blob name to download");
        var blobName = Console.ReadLine();
        var blob = await _storageHelperService.ListVideoBlobsAsync(blobName);
        if (blob.Any())
        {
          Console.WriteLine("Enter the file path including name");
          var filePath = Console.ReadLine();
          var fileStream = File.OpenWrite(filePath);

          await _storageHelperService.DownloadVideoAsync(blob.FirstOrDefault(), fileStream);
          await fileStream.FlushAsync();
          fileStream.Close();


        }
        else
        {
          Console.WriteLine($"No blob found with name {blobName}");
        }

      }

      Console.WriteLine("End of program");
      Console.ReadLine();

    }
  }
}