using RestApiWithDontNet.Models;
using RestApiWithDontNet.Repository;

namespace RestApiWithDontNet.Business.Impl
{
    public class UserBusiness : IUserBusiness
    {
        private IUserRepository _userRepository;

        public UserBusiness(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public User FindById(long id)
        {
            User user = _userRepository.FindById(id);
            if (user == null) {
                throw new Exception("Usuário não encontrado.");
            }
            return user;
        }

        public List<User> FindAll()
        {
            return _userRepository.FindAll();
        }

        public User Create(User user)
        {
            try
            {
                return _userRepository.Create(user);
            } catch (Exception)
            {
                throw;
            }
        }

        public User Update(long id, User user)
        {
            try
            {
                User userOld = _userRepository.FindById(id);

                if (userOld == null) { 
                    throw new Exception("Usuário não encontrado");
                }
                
                return _userRepository.Update(id, user, userOld);
            }
            catch (Exception) {
                throw;
            }
        }

        public void Delete(long id)
        {
            try
            {

                if (!_userRepository.Exists(id))
                {
                    throw new Exception("Usuário não encontrado");
                }

                _userRepository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
