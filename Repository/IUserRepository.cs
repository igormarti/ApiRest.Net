using RestApiWithDontNet.Hypermedia.Utils;
using RestApiWithDontNet.Models;
using RestApiWithDontNet.Repository.Generic;

namespace RestApiWithDontNet.Repository
{
    public interface IUserRepository:IRepository<User>
    {
            
        bool Disable(long id);

        List<User> findByName(string name);

    }
}
