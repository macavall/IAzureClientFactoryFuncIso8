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

                 clientBuilder
                        .AddBlobServiceClient(new Uri("https://testfunc56563stor.blob.core.windows.net/"))
                        .WithName("copierOutputBlob")
                        .WithCredential(new DefaultAzureCredential());
             });

        //     .AddApplicationInsightsTelemetryWorkerService()
        //     .ConfigureFunctionsApplicationInsights();

        builder.Build().Run();
    }
}
