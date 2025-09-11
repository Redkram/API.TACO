using API.Data;

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
