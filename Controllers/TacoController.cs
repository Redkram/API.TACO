using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using System.Security.Claims;
using API.Models;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using Asp.Versioning;
using API.Dtos;
using API.Mappers;
using API.Filters;

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
        [HttpPost("Test")]
        public IActionResult Test()
        {
            return Ok(new
            {
                data = "HOLA"
            });
        }

        
        //[ApiExplorerSettings(GroupName = "private")]
        //[HttpPost("GetContact")]
        //[Authorize(Roles = "Admin, Driver, User")]
        //[ValidateApiRequest]
        //public async Task<IActionResult> GetContact([FromBody] PersonnelNumberId _PersonnelNumberId)
        //{
        //    try
        //    {
        //        int _userId = (int)(HttpContext.Items["UserId"] ?? 0);
        //        if (_PersonnelNumberId == null || _PersonnelNumberId.PersonnelNumber == null || _PersonnelNumberId.PersonnelNumber == string.Empty) return Unauthorized("Usuario no encontrado");
        //        string _PersonnelNumber = Convert.ToInt32(_PersonnelNumberId.PersonnelNumber).ToString();
        //        VIEW_MS_ALL_USERS_DATA? _userData = _myService.GetDFO_UserByPersonnelNumber(_PersonnelNumber);
        //        if (_userData == null) return BadRequest(MissingUserInfoMessage);
        //        bool _isXofer = _userData.SRVRRHHISDRIVER == 1;

        //        StringContent _content = new StringContent(JsonConvert.SerializeObject(_PersonnelNumberId), Encoding.UTF8, "application/json");
        //        string? response = await _myService.CallExternalApiAsync("https://svt:4151/api/v1/SVT/GetOfficeStaffByPersonnelNumber", _content); //Error: Connection refused (svt:443)
        //        if (string.IsNullOrEmpty(response)) return BadRequest(FailedUserDataMessage);
        //        var outer = JObject.Parse(response);
        //        var innerJson = outer["data"]?.ToString();
        //        USUARIS? _user = JsonConvert.DeserializeObject<USUARIS>(innerJson ?? "");
        //        if (_user == null) return BadRequest(MissingUserInfoMessage);
        //        return Ok(new
        //        {
        //            data = _myService.Get3CX_User(_user)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Error: {ex.Message}");
        //    }
        //
    }
}
