# Important Note
- Possible to use the Release Version out of the box, ensure the ENVIRONMENT VARIABLE `STORAGE_NAME` is set in the Azure Portal for the Function App to know the name of the Storage Account

- Must add in the necessary libraries into `Program.cs`: `Azure.Identity` and `Microsoft.Extensions.Azure`
  - Both of the NuGet Packages above should be installed
- Managed Identity must be enabled for the Function App
- Storage Account requires `Storage Blob Owner` and `Storage Blob Contributor` **Roles** assigned to the **Function App Managed Identity**
d

``` csharp
using Azure.Identity;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = FunctionsApplication.CreateBuilder(args);

        builder.ConfigureFunctionsWebApplication();

        // Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
        builder.Services
            .AddAzureClients(clientBuilder =>
             {
                 // Add AddBlobServiceClient intended with managed identity and providing simply the storage URI

                 string storName = Environment.GetEnvironmentVariable("STORAGE_NAME") ?? "NO_STORAGE_NAME_FOUND";

                 clientBuilder
                        .AddBlobServiceClient(new Uri("https://" + storName + "blob.core.windows.net/"))
                        .WithName("copierOutputBlob")
                        .WithCredential(new DefaultAzureCredential());
             });

        //     .AddApplicationInsightsTelemetryWorkerService()
        //     .ConfigureFunctionsApplicationInsights();

        builder.Build().Run();
    }
}
```

# Finally, the Trigger Code

```csharp
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;

namespace aspfunction56
{
    public class http1
    {
        private readonly ILogger<http1> _logger;
        private readonly BlobContainerClient _copyContainerClient;

        public http1(ILogger<http1> logger, IAzureClientFactory<BlobServiceClient> blobClientFactory)
        {
            _logger = logger;

            _copyContainerClient = blobClientFactory.CreateClient("copierOutputBlob").GetBlobContainerClient("container2");
            _copyContainerClient.CreateIfNotExists();
        }

        [Function("http1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // random digits with 5 places
            string blobName = System.Guid.NewGuid().ToString().Substring(0, 5);

            await UploadBlobAsync(blobName + ".txt", "This is the content of the new blob.");

            return new OkObjectResult("Welcome to Azure Functions!");
        }

        private async Task UploadBlobAsync(string blobName, string content)
        {
            // Get a BlobClient for the new blob
            BlobClient blobClient = _copyContainerClient.GetBlobClient(blobName);

            // Convert content to a stream
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

            // Upload the blob
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = "text/plain" });

            _logger.LogInformation($"Uploaded blob '{blobName}' to container '{_copyContainerClient.Name}'.");
        }
    }
}
```
