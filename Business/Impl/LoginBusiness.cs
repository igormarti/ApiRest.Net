using RestApiWithDontNet.Configurations;
using RestApiWithDontNet.Data.Converter.Impl;
using RestApiWithDontNet.Data.VO;
using RestApiWithDontNet.Models;
using RestApiWithDontNet.Repository;
using RestApiWithDontNet.Services.Contracts;
using RestApiWithDontNet.Services.Impl;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace RestApiWithDontNet.Business.Impl
{
    public class LoginBusiness : ILoginBusiness
    {

        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private TokenConfiguration _configuration;

        private readonly IUserAuthRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly UserAuthParser _userAuthParser;

        public LoginBusiness(TokenConfiguration configuration, IUserAuthRepository repository, 
            ITokenService tokenService, UserAuthParser userAuthParser)
        {
            _configuration = configuration;
            _repository = repository;
            _tokenService = tokenService;
            _userAuthParser = userAuthParser;
        }

        public TokenVO ValidateCredentials(UserAuthVO userAuth)
        {
            var user = _repository.ValidateCredentials(userAuth);
            if (user == null) throw new Exception("Invalid user.");
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N") ),
                new Claim(JwtRegisteredClaimNames.UniqueName, userAuth.UserName),
            };

            var accessToken = _tokenService.GeneratedAccessToken(claims);
            var refreshToken = _tokenService.GeneratedRefreshToken();
            UpdateRefreshToken(user, refreshToken);

            return GenerateOutPutToken(accessToken, refreshToken);
        }

        public TokenVO ValidateCredentials(TokenVO token)
        {
            var accessToken = token.AccessToken;
            var refreshToken = token.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            if (principal == null) throw new Exception("Fail to try refresh token");

            var userName = principal?.Identity?.Name;

            if (userName == null) throw new Exception("UserName not identified.");

            var userAuth = _repository.ValidateCredentials(userName);

            if (userAuth == null||
                userAuth.RefreshToken!=refreshToken||
                userAuth.RefreshTokenExpireTime<=DateTime.Now) throw new Exception("Token not renovated.");

            accessToken = _tokenService.GeneratedAccessToken(principal.Claims);
            refreshToken = _tokenService.GeneratedRefreshToken();
            UpdateRefreshToken(userAuth, refreshToken);

            return GenerateOutPutToken(accessToken, refreshToken);
        }
        public UserAuthOutputVO? CreateUserAuth(UserAuthInputVO user)
        {
            if(user== null) throw new Exception("Invalid user");    

            user.Password = HashAlgorithmService.ComputerHash(user.Password, SHA256.Create());
            user.RefreshTokenExpireTime = DateTime.Now;

            var userCreated = _repository.CreateUserAuth(_userAuthParser.Parse(user));

            if (userCreated == null) throw new Exception("Fail to try create user");

            return _userAuthParser.Parse(userCreated);
        }

        private void UpdateRefreshToken(UserAuth userAuth, string refreshToken)
        {
            userAuth.RefreshToken = refreshToken;
            userAuth.RefreshTokenExpireTime = DateTime.Now.AddDays(_configuration.DaysToExpire);
            _repository.RefreshUserAuthInfo(userAuth);
        }

        private TokenVO GenerateOutPutToken(string accessToken,string refreshToken)
        {
            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            return new TokenVO(
                accessToken,
                refreshToken,
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT)
             );
        }

        public bool RevokeToken(string userName)
        {
            return _repository.RevokeToken(userName);
        }
    }
}
