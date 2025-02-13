# Important Note
- Possible to use the Release Version out of the box, ensure the ENVIRONMENT VARIABLE `STORAGE_NAME` is set in the Azure Portal for the Function App to know the name of the Storage Account

- Must add in the necessary libraries into `Program.cs`: `Azure.Identity` and `Microsoft.Extensions.Azure`
  - Both of the NuGet Packages above should be installed
- Managed Identity must be enabled for the Function App
- Storage Account requires `Storage Blob Owner`, `Storage Queue Contributor` and `Storage Blob Contributor` **Roles** assigned to the **Function App Managed Identity**

![image](https://github.com/user-attachments/assets/d627298a-c5b5-4ff5-8896-a6fc4a7953f9)

![image](https://github.com/user-attachments/assets/64abee5b-b2f0-444b-9456-dbd20fd416a9)

![image](https://github.com/user-attachments/assets/b49e2533-2d36-46af-bf72-6e1b101bf276)


# Program.cs

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
        //builder.Services

        //     .AddApplicationInsightsTelemetryWorkerService()
        //     .ConfigureFunctionsApplicationInsights();

        builder.Build().Run();
    }
}

```

# Finally, the Trigger Code (_http2.cs_)

```csharp
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
```
