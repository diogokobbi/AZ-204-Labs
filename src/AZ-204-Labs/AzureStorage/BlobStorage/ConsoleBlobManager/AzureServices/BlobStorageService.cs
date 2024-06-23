﻿using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace ConsoleBlobManager.AzureServices
{
    internal class BlobStorageService: IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;
        private readonly BlobStorageConfiguration _blobStorageConfiguration;
        public BlobStorageService(IConfiguration configuration)
        {
            // _blobStorageConfiguration = configuration.GetRequiredSection("BlobStorageConfiguration").Get<BlobStorageConfiguration>();
            _blobStorageConfiguration = new BlobStorageConfiguration() {
                ConnectionString = configuration.GetSection("BlobStorageConfiguration:ConnectionString").Value,
                ContainerName = configuration.GetSection("BlobStorageConfiguration:ContainerName").Value
            };            
            _blobServiceClient = new BlobServiceClient(_blobStorageConfiguration.ConnectionString);
            _containerClient = _blobServiceClient.GetBlobContainerClient(_blobStorageConfiguration.ContainerName);
        }

        private async Task<bool> InitializeBlobContainerAsync()
        {
            if (_containerClient != null) { 
                await _containerClient.CreateIfNotExistsAsync();
                return await _containerClient.ExistsAsync();
            }
            else
            {
                return false;
            }
        }

        public async Task<string?> GetAccountNameAsync() {
            return _blobServiceClient != null
                ? _blobServiceClient.AccountName
                : null;
        }

        public async Task<IReadOnlyList<string>> GetContainersNamesAsync()
        {

            List<string> containersNames = new List<string>();
            if (_blobServiceClient != null) { 
                var containers = _blobServiceClient.GetBlobContainersAsync();
                await foreach (var container in containers)
                {
                    containersNames.Add(container.Name);
                }                
            }
            return containersNames.AsReadOnly();
        }

        public async Task<IReadOnlyList<string>> GetBlobsNamesAsync()
        {

            List<string> blobNames = new List<string>();
            if (_containerClient != null) { 
                var blobs = _containerClient.GetBlobsAsync();
                await foreach (var blob in blobs)
                {
                    blobNames.Add(blob.Name);
                }                
            }
            return blobNames.AsReadOnly();
        }

        public async Task<bool?> AnyBlobAsync(string name)
        {
            if (_containerClient != null) {
                BlobClient blobClient = _containerClient.GetBlobClient(name);
                if (blobClient != null)
                {
                    return await blobClient.ExistsAsync();
                }             
            }
            return null;
        }

        public async Task<bool> PostBlobAsync(Stream fileStream, string name, bool overwriteIfExists = false)
        {
            if (await InitializeBlobContainerAsync())
            {
                BlobClient blobClient = _containerClient.GetBlobClient(name);
                if (blobClient != null && (overwriteIfExists || !await blobClient.ExistsAsync()))
                {
                    var newBlob = await blobClient.UploadAsync(fileStream, overwrite: overwriteIfExists);
                    return newBlob != null;
                }
            }
            return false;
        }
            
    }
}
