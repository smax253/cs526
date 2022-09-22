using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;


namespace ImageSharingWithCloudStorage.DAL
{
    public class ImageStorage : IImageStorage
    {

        public const string ACCOUNT = "imagesharing";
        public const string CONTAINER = "images";

        protected readonly IWebHostEnvironment hostingEnvironment;

        protected ILogger<ImageStorage> logger;

        protected BlobServiceClient blobServiceClient;

        protected BlobContainerClient containerClient;


        public ImageStorage(IOptions<StorageOptions> storageOptions,
                            IWebHostEnvironment hostingEnvironment,
                            ILogger<ImageStorage> logger)
        {
            this.logger = logger;

            this.hostingEnvironment = hostingEnvironment;

            string connectionString = storageOptions.Value.ImageDb;

            logger.LogInformation("Using remote blob storage: "+connectionString);

            blobServiceClient = new BlobServiceClient(connectionString);

            containerClient = new BlobContainerClient(connectionString, CONTAINER);
        }

        /**
         * The name of a blob containing a saved image (id is key for metadata record).
         */
        protected static string BlobName (int imageId)
        {
            return "image-" + imageId + ".jpg";
        }

        protected string BlobUri (int imageId)
        {
            // TODO check this for correctness
            return containerClient.Uri + "/" + BlobName(imageId);
        }

        public async Task SaveFileAsync(IFormFile imageFile, int imageId)
        {
                logger.LogInformation("Saving image {0} to blob storage", imageId);

                BlobHttpHeaders headers = new BlobHttpHeaders();
                headers.ContentType = "image/jpeg";

            // TODO upload data to blob storage
            var blob = containerClient.GetBlobClient(BlobName(imageId));
            await blob.UploadAsync(imageFile.OpenReadStream(), headers);
        }

        public async Task DeleteFileAsync(int imageId)
        {
            logger.LogInformation("Deleting image {0} from blob storage", imageId);
            var blob = containerClient.GetBlobClient(BlobName(imageId));
            await blob.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }

        public string ImageUri(IUrlHelper urlHelper, int imageId)
        {
             return BlobUri(imageId);
        }

    }
}