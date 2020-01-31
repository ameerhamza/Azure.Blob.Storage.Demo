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
      Console.WriteLine("Enter 4 to delete a blob");

      var option = Console.ReadLine();
      Console.WriteLine($"option selected : {option}");

      if (option == "1")
      {
        await CreateBlobSeq();
      }
      else if (option == "2")
      {
        await ListBlobSeq();
      }
      else if (option == "3")
      {
        await DownloadBlobSeq();

      }
      else if (option == "4")
      {
        Console.WriteLine("Enter the name of the blob to delete");
        var blobName = Console.ReadLine();
        await _storageHelperService.DeleteVideoAsync(blobName);
      }

      Console.WriteLine("End of program");
      Console.ReadLine();

    }

    private async Task DownloadBlobSeq()
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

    private async Task ListBlobSeq()
    {
      foreach (var blob in await _storageHelperService.ListVideoBlobsAsync())
      {
        Console.WriteLine($"Blob Name : {blob.Name}");
      }
    }

    private async Task CreateBlobSeq()
    {
      Console.WriteLine("Enter File Path");
      var filePath = Console.ReadLine();

      Console.WriteLine("Enter Blob Name");
      var blobName = Console.ReadLine();

      Console.WriteLine("Enter Blob Title");
      var title = Console.ReadLine();

      Console.WriteLine("Enter Blob Description");
      var desc = Console.ReadLine();

      if (await _storageHelperService.CheckIfBlobExistsAsync(blobName))
      {
        Console.WriteLine("Blob already exists");
      }
      else
      {
        await _storageHelperService.UploadVideoAsync(await File.ReadAllBytesAsync(filePath), blobName, title, desc);
      }
    }
  }
}