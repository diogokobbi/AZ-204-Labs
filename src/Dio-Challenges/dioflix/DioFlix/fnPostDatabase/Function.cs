using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker;

namespace fnPostDatabase
{
    public static class fnPostDatabase
    {
        [FunctionName("movie")]
        [CosmosDBOutput("%DatabaseName%","movies", Connection = "CosmosDBConnection", CreateIfNotExists = true, PartitionKey = "id")]
        public static async Task<object?> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            MovieRequest movie = null;
            var content = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                movie = JsonConvert.DeserializeObject<MovieRequest>(content);
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Erro ao deserializar o objeto");
            }

            return JsonConvert.SerializeObject(movie);
        }
    }
}
