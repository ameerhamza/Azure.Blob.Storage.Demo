namespace StorageService.Contracts
{
  public interface IConfigurationService
  {
    string GetStorageConnectionString();
    string GetContainerName();
  }
}