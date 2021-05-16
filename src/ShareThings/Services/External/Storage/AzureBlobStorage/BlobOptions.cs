namespace ShareThings.Services.External.Storage.AzureBlobStorage
{
    public class BlobOptions
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string DefaultContainer { get; set; }
        public string DefaultImage { get; set; }
    }
}