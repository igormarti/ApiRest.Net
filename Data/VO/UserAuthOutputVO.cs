namespace RestApiWithDontNet.Data.VO
{
    public class UserAuthOutputVO
    {
        
        public long CodUser { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }

        public UserAuthOutputVO()
        {
        }

        public UserAuthOutputVO(long codUser, string fullName, string userName)
        {
            CodUser = codUser;
            FullName = fullName;
            UserName = userName;
        }
    }
}
