using API.Dtos;
using API.Models;

namespace API.Mappers
{
    public static  class VIEW_MS_ALL_USERS_DATA_Mapper
    {
        public static VIEW_MS_ALL_USERS_DATADto ToDto(VIEW_MS_ALL_USERS_DATA _userData)
        {
            return new VIEW_MS_ALL_USERS_DATADto
            {
                PERSONNELNUMBER = _userData.PERSONNELNUMBER,
                NAME = _userData.NAME,
                JOBID = _userData.JOBID,
                TITLEID = _userData.TITLEID,
                SRVRRHHISDRIVER = _userData.SRVRRHHISDRIVER,
                IMAGE = _userData.IMAGE,
                IMAGEFILENAME = _userData.IMAGEFILENAME
            };
        }
    }
}
