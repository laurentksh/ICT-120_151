using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ICT_151.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ICT_151.Repositories
{
    public interface IAzureBlobStorageRepository
    {
        /// <summary>Publication container name</summary>
        string PublicationMediaContainerName { get; }

        /// <summary>Profile pictures container name</summary>
        string ProfilePictureMediaContainerName { get; }

        /// <summary>Private messaging container name</summary>
        string PrivateMessagingContainerName { get; }

        /// <summary>Get the blob container for the specified container type.</summary>
        /// <param name="containerType"></param>
        /// <returns>BlobContainerClient instance, or null if not found.</returns>
        Task<BlobContainerClient> GetBlobContainer(MediaContainer containerType);

        /// <summary>
        /// Returns the Unified Resource Identifier (full path URL) for the specified blob.
        /// </summary>
        /// <param name="blobName">Blob name</param>
        /// <param name="containerType">Which container to use.</param>
        /// <returns></returns>
        Task<Uri> GetMediaUri(string blobName, MediaContainer containerType);

        /// <summary>Download a blob and it's metadata.</summary>
        /// <param name="blobName">Blob name</param>
        /// <param name="containerType">Which container to use.</param>
        /// <returns></returns>
        Task<BlobDownloadInfo> GetMediaContent(string blobName, MediaContainer containerType);

        /// <summary>Get a blob metadata wihout it's content.</summary>
        /// <param name="blobName">Blob name</param>
        /// <param name="containerType">Which container to use.</param>
        /// <returns></returns>
        Task<BlobDownloadDetails> GetMediaMetadata(string blobName, MediaContainer containerType);

        /// <summary>Upload binary data to Azure Blob Storage.</summary>
        /// <param name="blob">Stream to upload</param>
        /// <param name="blobName">Blob name (must be unique)</param>
        /// <param name="containerType">Which container to use.</param>
        /// <returns>Path to resource.</returns>
        Task<Uri> UploadMedia(Stream blob, string blobName, MediaContainer containerType);

        /// <summary>Check whether a blob exists or not.</summary>
        /// <param name="blobName">Blob name</param>
        /// <param name="containerType">Which container to use.</param>
        /// <returns></returns>
        Task<bool> Exists(string blobName, MediaContainer containerType);
    }

    public class AzureBlobStorageRepository : IAzureBlobStorageRepository
    {
        private BlobServiceClient Client;

        public string PublicationMediaContainerName { get; } = "publications-media";

        public string ProfilePictureMediaContainerName { get; } = "profilepicture-media";
        
        public string PrivateMessagingContainerName { get; } = "privatemessage-media";


        public AzureBlobStorageRepository(BlobServiceClient client)
        {
            Client = client;
        }

        public async Task<BlobContainerClient> GetBlobContainer(MediaContainer container)
        {
            BlobContainerClient containerClient = null;

            switch (container) {
                case MediaContainer.Publication:
                    containerClient = Client.GetBlobContainerClient(PublicationMediaContainerName);
                    await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
                    break;
                case MediaContainer.ProfilePicture:
                    containerClient = Client.GetBlobContainerClient(ProfilePictureMediaContainerName);
                    await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
                    break;
                case MediaContainer.PrivateMessage:
                    containerClient = Client.GetBlobContainerClient(PrivateMessagingContainerName);
                    await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);
                    break;
                case MediaContainer.Unknown:
                default:
                    break;
            }

            return containerClient;
        }


        public async Task<Uri> GetMediaUri(string blobName, MediaContainer containerType)
        {
            var container = await GetBlobContainer(containerType);

            return container.GetBlobClient(blobName).Uri;
        }

        public async Task<BlobDownloadInfo> GetMediaContent(string blobName, MediaContainer containerType)
        {
            var container = await GetBlobContainer(containerType);

            var client = container.GetBlobClient(blobName);
            return await client.DownloadAsync();
        }

        public async Task<BlobDownloadDetails> GetMediaMetadata(string blobName, MediaContainer containerType)
        {
            var container = await GetBlobContainer(containerType);
            
            var client = container.GetBlobClient(blobName);
            var result = await client.DownloadAsync(new Azure.HttpRange(length: 0));

            return result.Value.Details;
        }

        public async Task<Uri> UploadMedia(Stream blob, string blobName, MediaContainer containerType)
        {
            var container = await GetBlobContainer(containerType);
            await container.UploadBlobAsync(blobName, blob);
            
            return container.GetBlobClient(blobName).Uri;
        }

        public async Task<bool> Exists(string blobName, MediaContainer containerType)
        {
            var container = await GetBlobContainer(containerType);

            return await container.ExistsAsync();
        }
    }
}
