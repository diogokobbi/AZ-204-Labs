## Manage Tiered Storage with Azure CLI
You can configure and manage Azure Storage Containers and Blobs by using command-line tools.

### Manage Containers
[Complete Azure CLI Reference for Storage Container](https://learn.microsoft.com/en-us/cli/azure/storage/container?view=azure-cli-latest)
### Manage Blobs
[Complete Azure CLI Reference for Storage Blob](https://learn.microsoft.com/en-us/cli/azure/storage/blob?view=azure-cli-latest)
| Cmdlet      | Description |
| ----------- | ----------- |
| az storage blob upload | Upload a file to a storage blob. |
| az storage blob upload-batch | Upload files from a local directory to a blob container. |
| az storage blob list | List blobs and blob subdirectories in a storage directory |
| az storage blob download | Download Blobs from Azure Storage to a file path. |
| az storage blob set-tier --name <code>BlobName</code> --container-name <code>BlobContainer</code> --account-name <code>StorageAccountName</code> --tier <code>Cool</code> | Change blob tier for storing infrequently accessed/modified data |

> ⚠️ You can switch between access tiers at any time, when and if your usage patterns change. The change between tiers is immediate, except when you wish to move data out of Archive.
> Archive data is offline, therefore, the blob must first be rehydrated. The rehydration starts when we change the blob tier from Archive to either Hot or Cool.

