using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

//Maybe rename to (I)AzureBlobStorageRepository
namespace ICT_151.Services
{
    public interface IAzureBlobStorageService
    {
        string PublicationMediaContainerName { get; }

        string PrivateMessagingContainerName { get; }

        Task<BlobContainerClient> GetPublicationBlobContainer();

        Task<BlobContainerClient> GetPMBlobContainer();


        Task<BlobDownloadInfo> GetPublicationMedia(string blobName);

        Task<BlobDownloadDetails> GetPublicationMediaMetadata(string blobName);

        Task<BlobContentInfo> UploadPublicationMedia(Stream blob, string blobName);

        Task<BlobDownloadInfo> GetPMMedia(string blobName);

        Task<BlobDownloadDetails> GetPMMediaMetadata(string blobName);

        Task<BlobContentInfo> UploadPMMedia(Stream blob, string blobName);
    }

    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private BlobServiceClient Client; //Kinda acts like a repository ig

        public string PublicationMediaContainerName { get; } = "publications-media";

        public string PrivateMessagingContainerName { get; } = "pm-media";


        public AzureBlobStorageService(BlobServiceClient client)
        {
            Client = client;
        }

        public async Task<BlobContainerClient> GetPublicationBlobContainer()
        {
            var blobContainer = Client.GetBlobContainerClient(PublicationMediaContainerName);
            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.Blob);

            return blobContainer;
        }

        public async Task<BlobContainerClient> GetPMBlobContainer()
        {
            var blobContainer = Client.GetBlobContainerClient(PrivateMessagingContainerName);
            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.None);

            return blobContainer;
        }


        #region Publication
        public async Task<BlobDownloadInfo> GetPublicationMedia(string blobName)
        {
            var container = await GetPublicationBlobContainer();

            var client = container.GetBlobClient(blobName);
            return await client.DownloadAsync();
        }

        public async Task<BlobDownloadDetails> GetPublicationMediaMetadata(string blobName)
        {
            var container = await GetPublicationBlobContainer();
            
            var client = container.GetBlobClient(blobName);
            var result = await client.DownloadAsync(new Azure.HttpRange(length: 0));

            return result.Value.Details;
        }

        public async Task<BlobContentInfo> UploadPublicationMedia(Stream blob, string blobName)
        {
            var container = await GetPublicationBlobContainer();
            var result = await container.UploadBlobAsync(blobName, blob);

            return result.Value;
        }
        #endregion

        #region PrivateMessaging
        public async Task<BlobDownloadInfo> GetPMMedia(string blobName)
        {
            var container = await GetPMBlobContainer();

            var client = container.GetBlobClient(blobName);
            return await client.DownloadAsync();
        }

        public async Task<BlobDownloadDetails> GetPMMediaMetadata(string blobName)
        {
            var container = await GetPMBlobContainer();

            var client = container.GetBlobClient(blobName);
            var result = await client.DownloadAsync(new Azure.HttpRange(length: 0));

            return result.Value.Details;
        }

        public async Task<BlobContentInfo> UploadPMMedia(Stream blob, string blobName)
        {
            var container = await GetPMBlobContainer();
            var result = await container.UploadBlobAsync(blobName, blob);

            return result.Value;
        }
        #endregion
    }
}
