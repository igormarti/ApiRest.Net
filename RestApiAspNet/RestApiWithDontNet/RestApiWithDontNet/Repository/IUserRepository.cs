using RestApiWithDontNet.Models;

namespace RestApiWithDontNet.Repository
{
    public interface IUserRepository
    {
        User Create(User user);

        User Update(long id, User newUser, User oldUser);

        User FindById(long id);

        List<User> FindAll();

        void Delete(long id);

        bool Exists(long id);
    }
}
