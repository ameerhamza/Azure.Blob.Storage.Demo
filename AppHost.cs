using System;
using System.Collections.Generic;
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
    private Dictionary<string, string> _leaseDic = new Dictionary<string, string>();

    public AppHost(IStorageHelperService storageHelperService)
    {
      _storageHelperService = storageHelperService;
    }

    public async Task Run()
    {

      var option = string.Empty;
      do
      {
        try
        {
          option = await options();
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }

      }
      while (option != "q");

      Console.WriteLine("End of program");
      Console.ReadLine();

    }

    private async Task<string> options()
    {
      string option = PrintOptions();

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
        await DeleteBlob();
      }
      else if (option == "5")
      {
        await UpdateMetadata();
      }
      else if (option == "6")
      {
        await AcquireLease();
      }
      else if (option == "7")
      {
        await ReleaseLease();
      }

      Console.ReadLine();
      return option;
    }

    private static string PrintOptions()
    {
      Console.Clear();

      Console.WriteLine("Enter 1 to create a blob");
      Console.WriteLine("Enter 2 to list a blob");
      Console.WriteLine("Enter 3 to download a blob");
      Console.WriteLine("Enter 4 to delete a blob");
      Console.WriteLine("Enter 5 to update metadata");
      Console.WriteLine("Enter 6 to acquire lease");
      Console.WriteLine("Enter 7 to release lease");

      var option = Console.ReadLine();
      Console.WriteLine($"option selected : {option}");
      return option;
    }

    private async Task ReleaseLease()
    {
      Console.WriteLine("Enter the name of the blob");
      var blobName = Console.ReadLine();
      if (_leaseDic.TryGetValue(blobName, out var leaseId))
      {

        await _storageHelperService.ReleaseLease(blobName, leaseId);
        _leaseDic.Remove(blobName);
        Console.WriteLine("Lease Released");
      }
      else
      {
        Console.WriteLine("No lease on blob");
      }
    }

    private async Task AcquireLease()
    {
      Console.WriteLine("Enter the name of the blob");
      var blobName = Console.ReadLine();

      var lease = await _storageHelperService.AcquireLease(blobName);
      if (lease != null)
      {
        Console.WriteLine(lease.ETag);
        Console.WriteLine(lease.LeaseId);
        Console.WriteLine(lease.LeaseTime);

        _leaseDic.Add(blobName, lease.LeaseId);
      }
    }

    private async Task UpdateMetadata()
    {
      Console.WriteLine("Enter the name of the blob");
      var blobName = Console.ReadLine();

      Console.WriteLine("Enter Blob Title");
      var title = Console.ReadLine();

      Console.WriteLine("Enter Blob Description");
      var desc = Console.ReadLine();

      await _storageHelperService.UpdateBlobMetadata(blobName, title, desc);
    }

    private async Task DeleteBlob()
    {
      Console.WriteLine("Enter the name of the blob to delete");
      var blobName = Console.ReadLine();
      await _storageHelperService.DeleteVideoAsync(blobName);
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
        var blobProperties = await _storageHelperService.GetBlobPropertiesAsync(blob.Name);
        Console.WriteLine($"Blob Name : {blob.Name}");
        if (blobProperties.Metadata.Count > 0)
        {
          Console.WriteLine($"Title {blobProperties.Metadata["Title"]}");
          Console.WriteLine($"Description : {blobProperties.Metadata["Description"]}");
        }
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