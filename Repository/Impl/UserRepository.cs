using RestApiWithDontNet.Data.VO;
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

        public bool Disable(long id)
        {
            var user = this.FindById(id);
            if (user==null)return false;

            user.Enabled = false;
            _context.Entry(user).CurrentValues.SetValues(user);
            _context.SaveChanges();
            return true;
        }

        public List<User> findByName(string name)
        {
            return _context.Users.Where(u => u.Name.Contains(name)).ToList();
        }
    }
}
