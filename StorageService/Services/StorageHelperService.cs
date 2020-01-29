using System;
using System.Threading.Tasks;
using StorageService.Contracts;

namespace StorageService.Services
{
  public class StorageHelperService : IStorageHelperService
  {
    public StorageHelperService()
    {
    }

    public Task<bool> CheckIfBlobExistsAsync(string blobName)
    {
      throw new NotImplementedException();
    }

    public Task UploadVideoAsync(byte[] videoByteArray, string blobName)
    {
      throw new NotImplementedException();
    }
  }
}
