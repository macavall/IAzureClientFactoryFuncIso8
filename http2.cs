using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace aspfunction56
{
    public class http2
    {
        private readonly ILogger<http2> _logger;

        public http2(ILogger<http2> logger)
        {
            _logger = logger;
        }

        [Function("http2")]
        [BlobOutput("container3/{rand-guid}-output.txt")]
        public string Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            //return new OkObjectResult("Welcome to Azure Functions!");
            // Blob Output
            return "blob-output content";
        }
    }

}
