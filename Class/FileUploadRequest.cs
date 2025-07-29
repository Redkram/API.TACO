using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.TACO.Class
{
    public class FileUploadRequest
    {
        public required IFormFile? DDD {  get; set; }
    }
}
