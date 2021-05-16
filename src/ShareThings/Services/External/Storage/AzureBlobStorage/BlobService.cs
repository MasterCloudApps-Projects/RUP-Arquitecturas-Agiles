using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShareThings.Services.External.Storage.AzureBlobStorage
{
    public class BlobService : IDocumentService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobOptions _options;

        public BlobService(IOptions<BlobOptions> options)
        {
            this._options = options.Value;
            this._blobServiceClient = new BlobServiceClient(_options.ConnectionString);
        }

        private BlobClient GetBlobClient(string container, string fileName)
        {
            var containerClient = GetContainerClient(container);
            var blobClient = containerClient.GetBlobClient(fileName);
            return blobClient;
        }

        public Uri LoadDefault()
        {
            BlobClient blobClient = this.GetBlobClient(this._options.DefaultContainer.ToLower(), this._options.DefaultImage);
            return blobClient.Uri;
        }

        public async Task DeleteAsync(string uri)
        {
            CloudBlockBlob blob_temp = new CloudBlockBlob(new Uri(uri));
            BlobClient blobClient = this.GetBlobClient(this._options.ContainerName.ToLower(), blob_temp.Name);
            await blobClient.DeleteAsync();
        }

        public async Task DeleteRangeAsync(List<string> uries)
        {
            if (uries == null || !uries.Any())
                return;

            foreach (string uri in uries)
                await DeleteAsync(uri);
        }

        public async Task<Uri> UploadAsync(Stream content, string contentType, string fileName)
        {
            BlobClient blobClient = this.GetBlobClient(this._options.ContainerName.ToLower(), fileName);
            await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType });
            return blobClient.Uri;
        }

        private BlobContainerClient GetContainerClient(string container)
        {
            var containerClient = this._blobServiceClient.GetBlobContainerClient(container);
            containerClient.CreateIfNotExists(PublicAccessType.Blob);
            return containerClient;
        }
    }
}