using Google.Cloud.Storage.V1;

namespace API.Services
{
    public class StorageService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public StorageService(IConfiguration configuration)
        {
            _storageClient = StorageClient.Create(); // Usará la Service Account de la VM
            _bucketName = configuration["GoogleCloud:BucketName"]
                          ?? throw new Exception("Bucket name not configured");
        }

        public async Task<string> UploadFileAsync(IFormFile file, string objectName)
        {
            using var stream = file.OpenReadStream();

            var obj = await _storageClient.UploadObjectAsync(
                _bucketName,
                objectName,
                file.ContentType,
                stream
            );

            return $"gs://{_bucketName}/{obj.Name}";
        }
    }

}
