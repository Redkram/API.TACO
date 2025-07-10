namespace API.Dtos
{
    public class VIEW_MS_ALL_USERS_DATADto
    {
        public string? PERSONNELNUMBER { get; set; } = null;
        public string? LANGUAGEID { get; set; } = null;
        public string? NAME { get; set; } = null;
        public DateTime? SENIORITYDATE { get; set; } = null;
        public DateTime? BIRTHDATE { get; set; } = null;
        public int? GENDER { get; set; } = null;
        public string? FIRSTNAME { get; set; } = null;
        public string? LASTNAME { get; set; } = null;
        public string? EMPLOYMENTLEGALENTITYID { get; set; } = null;
        public string? JOBID { get; set; } = null;
        public string? TITLEID { get; set; } = null;
        public byte[]? IMAGE { get; set; } = null;
        public string? IMAGEFILENAME { get; set; } = null;
        public int SRVRRHHISDRIVER { get; set; }
    }
}