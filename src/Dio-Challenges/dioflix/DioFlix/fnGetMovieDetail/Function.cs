using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Functions.Worker.Http;

namespace fnGetMovieDetail
{
    public static class Function
    {
        [FunctionName("detail")]
        public static async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestData req,
            ILogger log)
        {
            var cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("CosmosDBConnection"));
            var container = cosmosClient.GetContainer("DioFlixDB", "movies");
            var id = req.Query["id"];
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id")
                .WithParameter("@id", id);
            var result = container.GetItemQueryIterator<MovieResult>(query);
            var results = new List<MovieResult>();
            while (result.HasMoreResults)
            {
                foreach (var item in await result.ReadNextAsync())
                {
                    results.Add(item);
                }
            }
            var responseMessage = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await responseMessage.WriteAsJsonAsync(results.FirstOrDefault());
            return responseMessage;
        }
    }
}
