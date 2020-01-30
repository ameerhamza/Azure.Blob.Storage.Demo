﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using StorageService.Contracts;

namespace StorageService.Services
{
  public class StorageHelperService : IStorageHelperService
  {
    private BlobServiceClient _storageServiceClient;
    private IConfigurationService _configurationService;
    private BlobContainerClient _containerClient;

    public StorageHelperService(IConfigurationService configurationService)
    {
      _configurationService = configurationService;
      _storageServiceClient = new BlobServiceClient(_configurationService.GetStorageConnectionString());
    }



    public async Task<bool> CheckIfBlobExistsAsync(string blobName)
    {
      var client = await GetContainerClient();
      var blobClient = client.GetBlobClient(blobName);

      return await blobClient.ExistsAsync();
    }

    public async Task UploadVideoAsync(byte[] videoByteArray, string blobName)
    {
      var client = await GetContainerClient();
      var blobs = client.GetBlobsAsync(BlobTraits.None, BlobStates.None, blobName);


      var blobClient = client.GetBlobClient(blobName);

      await blobClient.UploadAsync(new MemoryStream(videoByteArray), new BlobHttpHeaders()
      {
        ContentType = "video/mp4"
      });
    }

    public async Task<IEnumerable<BlobItem>> ListVideoBlobsAsync(string prefix = null)
    {
      var containerClient = await GetContainerClient();
      var blobs = containerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None, prefix);
      var videos = new List<BlobItem>();

      await foreach (var blobItem in blobs)
      {
        videos.Add(blobItem);
      }

      return videos;
    }


    public async Task DownloadVideoAsync(BlobItem blob, Stream targetStream)
    {
      var blobClient = _containerClient.GetBlobClient(blob.Name);
      var blobDownload = await blobClient.DownloadAsync();

      await blobDownload.Value.Content.CopyToAsync(targetStream);
    }

    public async Task DeleteVideoAsync(BlobItem blobItem)
    {
      // TODO: Delete the Blob from Blob Storage
    }


    public async Task<BlobContainerClient> GetContainerClient()
    {
      if (_containerClient == null)
      {
        _containerClient = _storageServiceClient.GetBlobContainerClient(_configurationService.GetContainerName());
        await _containerClient.CreateIfNotExistsAsync();
      }

      return _containerClient;
    }
  }
}