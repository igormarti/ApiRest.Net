using RestApiWithDontNet.Models;
using RestApiWithDontNet.Models.Context;
using RestApiWithDontNet.Repository.Generic;

namespace RestApiWithDontNet.Repository.Impl
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private MySqlContext _context;

        public UserRepository(MySqlContext context):base(context) { 
            _context = context;
        } 
    }
}
