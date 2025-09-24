using API.Filters;
using API.Services;
using API.TACO.Class;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Security.Cryptography.Xml;

namespace API.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TacoController(MyService myService, StorageService storageService) : ControllerBase
    {

        private const string MissingUserInfoMessage = "Not user info";
        private const string MissingDriverIdMessage = "Driver Not Found";
        private const string InvalidDriverUserIdMessage = "This user is not registered as a driver.";
        private const string FailedUserDataMessage = "Failed to fetch user data.";


        private readonly MyService _myService = myService;
        private readonly StorageService _storageService = storageService;

        [HttpPost("UploadTacho")]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> UploadTacho([FromForm] FileUploadRequest request)
        {
            try
            { 
                //int _userId = (int)(HttpContext.Items["UserId"] ?? 0);

                //if (request.DDD == null || request.DDD.Length == 0)
                //    return BadRequest("No file was received in the request.");

                //var allowedContentTypes = new List<string>
                //{
                //    "application/octet-stream", "application/ddd", "application/tgd", "application/json"
                //};

                //if (!allowedContentTypes.Contains(request.DDD.ContentType))
                //    return BadRequest($"Unsupported file type: {request.DDD.ContentType}");

                //var allowedExtensions = new List<string> { ".ddd", ".tgd", ".json" };
                //var fileExtension = Path.GetExtension(request.DDD.FileName).ToLowerInvariant();

                //if (!allowedExtensions.Contains(fileExtension))
                //    return BadRequest($"Unsupported file extension: {fileExtension}");

                //// 👇 Guardar en el bucket con nombre fijo "0"
                //var objectName = $"originals/{_userId}/{request.DDD.FileName}";
                //await _storageService.UploadFileAsync(request.DDD, objectName);

                //var signedUrl = _storageService.GetSignedUrl(objectName, TimeSpan.FromDays(1));

                //return Ok(new { SignedUrl = signedUrl });
                return Ok(new { SignedUrl = "OK" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An unexpected error occurred",
                    details = ex.Message,
                    stack = ex.StackTrace
                });
            }
        }

    }
}
