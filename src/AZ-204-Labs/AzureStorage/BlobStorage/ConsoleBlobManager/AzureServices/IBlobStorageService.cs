namespace ConsoleBlobManager.AzureServices
{
    public interface IBlobStorageService
    {
        Task<string?> GetAccountNameAsync();
        Task<IReadOnlyList<string>> GetContainersNamesAsync();
        Task<IReadOnlyList<string>> GetBlobsNamesAsync();
        Task<bool> PostBlobAsync(Stream fileStream, string name, bool OverwriteIfExists = false);
        Task<bool?> AnyBlobAsync(string name);
    }
}
