using RestApiWithDontNet.Data.VO;
using RestApiWithDontNet.Models;

namespace RestApiWithDontNet.Business
{
    public interface ILoginBusiness
    {
        TokenVO ValidateCredentials(UserAuthVO userAuth);
        TokenVO ValidateCredentials(TokenVO token);
        bool RevokeToken(string userName);
        UserAuthOutputVO? CreateUserAuth(UserAuthInputVO user);
    }
}
