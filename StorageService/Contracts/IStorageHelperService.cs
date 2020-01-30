using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace StorageService.Contracts
{
  public interface IStorageHelperService
  {
    Task UploadVideoAsync(byte[] videoByteArray, string blobName);
    Task<bool> CheckIfBlobExistsAsync(string blobName);
    Task<IEnumerable<BlobItem>> ListVideoBlobsAsync(string prefix = null);
    Task DownloadVideoAsync(BlobItem blob, Stream targetStream);
  }
}