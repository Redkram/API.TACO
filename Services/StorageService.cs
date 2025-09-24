using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace API.Services
{
    public class StorageService
    {
        private readonly StorageClient _storageClient;
        private readonly UrlSigner _urlSigner;
        private readonly string _bucketName;

        public StorageService(IConfiguration configuration)
        {
            _storageClient = StorageClient.Create(); // Usará la Service Account de la VM
            _bucketName = configuration["GoogleCloud:BucketName"]
                          ?? throw new Exception("Bucket name not configured");
            var serviceAccountPath = configuration["GoogleCloud:GOOGLE_APPLICATION_CREDENTIALS"];
            if (string.IsNullOrEmpty(serviceAccountPath))
                throw new Exception("Debe definir GOOGLE_APPLICATION_CREDENTIALS con la ruta al JSON");

            var credential = GoogleCredential.FromFile(serviceAccountPath)
                .UnderlyingCredential as ServiceAccountCredential;

            _urlSigner = UrlSigner.FromCredential(credential);
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

        public string GetSignedUrl(string objectName, TimeSpan expiration)
        {
            // GET para descargar
            return _urlSigner.Sign(_bucketName, objectName, expiration, HttpMethod.Get);
        }
    }

}