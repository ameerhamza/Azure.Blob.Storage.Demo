using System.Threading.Tasks;

namespace StorageService.Contracts
{
  public interface IStorageHelperService
  {
    Task UploadVideoAsync(byte[] videoByteArray, string blobName);
    Task<bool> CheckIfBlobExistsAsync(string blobName);
  }
}