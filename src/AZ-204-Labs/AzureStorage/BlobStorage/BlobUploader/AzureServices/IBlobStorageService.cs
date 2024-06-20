namespace BlobUploader.AzureServices
{
    public interface IBlobStorageService
    {
        Task<IReadOnlyList<string>> GetBlobsNamesAsync();
        Task<bool> PostBlobAsync(Stream fileStream, string name, bool OverwriteIfExists = false);
        Task<bool?> AnyBlobAsync(string name);
    }
}
