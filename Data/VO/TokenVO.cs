namespace RestApiWithDontNet.Data.VO
{
    public class TokenVO
    {
        public TokenVO(string accessToken, string refreshToken, bool? authenticated, string? created, string? expiration)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Authenticated = authenticated;
            Created = created;
            Expiration = expiration;
        }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool ?Authenticated { get; set; }
        public string ?Created { get; set; }
        public string ?Expiration { get; set; }

    }
}
