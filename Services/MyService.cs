using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Azure;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;
using System.Globalization;

namespace API.Services
{
    public class MyService
    {
        private readonly MySQLDbContext _mysqlContext;
        //private readonly LogService ApiLog;

        public MyService(MySQLDbContext MySQLContext)
        {
            _mysqlContext = MySQLContext;
        }



        public string GetDFO_AllUsers()
        {
            try
            {
                return String.Empty;
                //return _mysqlContext.DbVIEW_MS_USER_DATA.Where(u => u.WORKERSTATUS == 1).ToList();
            }
            catch (Exception ex)
            {
                //_ = ApiLog.HandleExceptionAsync(ex);
                throw;
            }
        }

    }
}
