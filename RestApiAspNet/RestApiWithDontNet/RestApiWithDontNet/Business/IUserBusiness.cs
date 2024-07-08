using RestApiWithDontNet.Models;

namespace RestApiWithDontNet.Business
{
    public interface IUserBusiness
    {
        User Create(User user);

        User Update(long id, User user);

        User FindById(long id);

        List<User> FindAll();

        void Delete(long id);
    }
}
