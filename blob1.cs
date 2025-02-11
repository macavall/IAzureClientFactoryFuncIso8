using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;

namespace aspfunction56
{
    public class blob1
    {
        private readonly ILogger<blob1> _logger;
        private readonly BlobContainerClient _copyContainerClient;

        public blob1(ILogger<blob1> logger, IAzureClientFactory<BlobServiceClient> blobClientFactory)
        {
            _logger = logger;

            _copyContainerClient = blobClientFactory.CreateClient("copierOutputBlob").GetBlobContainerClient("container2");
            _copyContainerClient.CreateIfNotExists();
        }

        [Function(nameof(blob1))]
        public async Task Run([BlobTrigger("container1/{name}", Connection = "storconnstring")] Stream stream, string name)
        {
            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}");

            // Get a BlobClient for the new blob
            BlobClient blobClient = _copyContainerClient.GetBlobClient("container2");

            // Convert content to a stream
            using var stream2 = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("Hello there!"));

            // Upload the blob
            await blobClient.UploadAsync(stream2, new BlobHttpHeaders { ContentType = "text/plain" });

            // set blobName to a random set of characters to avoid conflicts
            string blobName = System.Guid.NewGuid().ToString();

            _logger.LogInformation($"Uploaded blob '{blobName}' to container '{_copyContainerClient.Name}'.");
        }
    }
}
