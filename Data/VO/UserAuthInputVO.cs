namespace RestApiWithDontNet.Data.VO
{
    public class UserAuthInputVO:UserAuthVO
    {
        public string FullName { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }

    }
}
