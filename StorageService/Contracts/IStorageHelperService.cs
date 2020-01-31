using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace StorageService.Contracts
{
  public interface IStorageHelperService
  {
    Task<bool> CheckIfBlobExistsAsync(string blobName);
    Task<IEnumerable<BlobItem>> ListVideoBlobsAsync(string prefix = null);
    Task DownloadVideoAsync(BlobItem blob, Stream targetStream);
    Task DeleteVideoAsync(string blobName);
    Task UploadVideoAsync(byte[] videoByteArray, string blobName, string title = null, string desc = null);
    Task<BlobProperties> GetBlobPropertiesAsync(string blobName);
    Task UpdateBlobMetadata(string blobName, string title, string desc);
    Task<BlobLease> AcquireLease(string blobName);
    Task ReleaseLease(string blobName, string leaseId);
  }
}