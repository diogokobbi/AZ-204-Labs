using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Web.Http;

namespace fnPostDataStorage
{
    public static class fnPostDataStorage
    {
        [FunctionName("dataStorage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processando a image no Storage...");

            try
            {
                if (!req.Headers.TryGetValue("file-type", out var fileTypeHeader))
                {
                    return new BadRequestObjectResult("O cabeçalho 'file-type' é obrigatório.");
                }
                else
                {
                    var fileType = fileTypeHeader.ToString();
                    var form = await req.ReadFormAsync();
                    var file = form.Files["file"];
                    if (file == null || file.Length == 0)
                    {
                        return new BadRequestObjectResult("O cabeçalho 'file-type' é obrigatório.");
                    }
                    else
                    {
                        var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                        string containerName = fileType;
                        BlobClient blobClient = new BlobClient(connectionString, containerName, file.FileName);
                        BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);

                        await containerClient.CreateIfNotExistsAsync();
                        await containerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
                        
                        string blobName = file.FileName;
                        var blob = containerClient.GetBlobClient(blobName);

                        using (var stream = file.OpenReadStream())
                        {
                            await blob.UploadAsync(stream, true);
                        }

                        log.LogInformation($"Arquivo {file.FileName} enviado com sucesso para o container {containerName}.");
                        return new OkObjectResult( new
                        {
                            Message = $"Arquivo {file.FileName} enviado com sucesso para o container {containerName}.",
                            BlobUri = blob.Uri
                        });
                    }

                }
            }
            catch (Exception)
            {
                return new InternalServerErrorResult();
            }
        }
    }
}
