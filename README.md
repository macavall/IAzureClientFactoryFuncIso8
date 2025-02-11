


``` csharp
using Azure.Identity;
using Microsoft.Extensions.Azure;

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

```
