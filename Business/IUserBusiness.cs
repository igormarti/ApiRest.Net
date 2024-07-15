using RestApiWithDontNet.Data.VO;
using RestApiWithDontNet.Hypermedia.Utils;

namespace RestApiWithDontNet.Business
{
    public interface IUserBusiness
    {
        UserVO Create(UserVO user);

        UserVO Update(long id, UserVO user);

        UserVO FindById(long id);

        List<UserVO> FindAll();

        void Delete(long id);

        void Disable(long id);

        List<UserVO> FindByName(string name);

        PagedSearchVO<UserVO> findWithPagedSearch(string name, string sortDirection, int pageSize, int page);
    }
}
