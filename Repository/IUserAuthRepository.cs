using RestApiWithDontNet.Data.VO;
using RestApiWithDontNet.Models;

namespace RestApiWithDontNet.Repository
{
    public interface IUserAuthRepository
    {
        public UserAuth? ValidateCredentials(UserAuthVO user);

        public UserAuth? ValidateCredentials(string userName);

        public bool RevokeToken(string userName);

        public UserAuth? CreateUserAuth(UserAuth user);

        public UserAuth? RefreshUserAuthInfo(UserAuth user);

        public UserAuth? FindUserAuth(long id);
        public UserAuth? FindUserAuthByUserName(string userName);

    }
}
