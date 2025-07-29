using API.Dtos;
using API.Filters;
using API.Mappers;
using API.Models;
using API.Services;
using API.TACO.Class;
using Asp.Versioning;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TacoController(MyService myService) : ControllerBase
    {

        private const string MissingUserInfoMessage = "Not user info";
        private const string MissingDriverIdMessage = "Driver Not Found";
        private const string InvalidDriverUserIdMessage = "This user is not registered as a driver.";
        private const string FailedUserDataMessage = "Failed to fetch user data.";


        private readonly MyService _myService = myService;

        
        [ApiExplorerSettings(GroupName = "public")]
        [HttpPost("UploadTacho")]
        [Consumes("multipart/form-data")]
        public IActionResult UploadTacho([FromForm] FileUploadRequest request)
        {
            try
            {
                if (request.DDD != null)
                {
                    var allowedContentTypes = new List<string>
                    {
                        "application/octet-stream", "application/ddd", "application/tgd"
                    };

                    if (!allowedContentTypes.Contains(request.DDD.ContentType))
                        return BadRequest($"Unsupported file type: {request.DDD.ContentType}");

                    var allowedExtensions = new List<string> { ".ddd", ".tgd" };
                    var fileExtension = Path.GetExtension(request.DDD.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(fileExtension))
                        return BadRequest($"Unsupported file extension: {fileExtension}");

                    using var stream = new MemoryStream();
                    request.DDD.CopyTo(stream);
                    var dddBytes = stream.ToArray();

                    var uploadsPath = "/app/uploads";
                    Directory.CreateDirectory(uploadsPath);

                    var fullPath = Path.Combine(uploadsPath, request.DDD.FileName);
                    using var fileStream = new FileStream(fullPath, FileMode.Create);
                    fileStream.Write(dddBytes);

                    var psi = new ProcessStartInfo
                    {
                        FileName = "python3",
                        Arguments = $"/app/scripts/asyncnodex.py \"{fullPath}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using var process = Process.Start(psi);
                    if (process == null)
                        return StatusCode(500, "Error processing the file. The processing script could not be started.");

                    string? output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    if (string.IsNullOrWhiteSpace(output))
                        return StatusCode(500, "Error processing the file. No output was returned from the processing script.");

                    var json = JObject.Parse(output);
                    var data = json["data"]?.ToString();

                    return Ok(new { data });


                }
                return BadRequest("No file was received in the request.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An unexpected error occurred. Please try again later."
                });
            }
        }
    }
}
