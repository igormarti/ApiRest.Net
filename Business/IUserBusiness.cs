using RestApiWithDontNet.Data.VO;

namespace RestApiWithDontNet.Business
{
    public interface IUserBusiness
    {
        UserVO Create(UserVO user);

        UserVO Update(long id, UserVO user);

        UserVO FindById(long id);

        List<UserVO> FindAll();

        void Delete(long id);
    }
}
