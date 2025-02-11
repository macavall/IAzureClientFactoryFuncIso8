# Important Note
- Must add in the necessary libraries into `Program.cs`: `Azure.Identity` and `Microsoft.Extensions.Azure`
- Managed Identity must be enabled for the Function App
- Storage Account requires `Storage Blob Owner` and `Storage Blob Contributor` **Roles** assigned to the **Function App Managed Identity**


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
