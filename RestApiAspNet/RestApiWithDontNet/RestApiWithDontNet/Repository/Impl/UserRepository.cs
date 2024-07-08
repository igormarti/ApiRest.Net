using RestApiWithDontNet.Models;
using RestApiWithDontNet.Models.Context;

namespace RestApiWithDontNet.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        private MySqlContext _context;

        public UserRepository(MySqlContext context) {
            _context = context;
        }

        public User FindById(long id)
        {
            return _context.Users.SingleOrDefault(u => u.Id.Equals(id));
        }

        public List<User> FindAll()
        {
            return _context.Users.ToList();
        }

        public User Create(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User Update(long id, User newUser, User oldUser)
        {
            newUser.Id = oldUser.Id;
            _context.Entry(oldUser).CurrentValues.SetValues(newUser);
            _context.SaveChanges();
            return newUser;
        }

        public void Delete(long id)
        {
            var result = FindById(id);
            _context.Users.Remove(result);
            _context.SaveChanges();
        }

        public bool Exists(long id)
        {
            return _context.Users.Any(u => u.Id.Equals(id));
        }
    }
}
