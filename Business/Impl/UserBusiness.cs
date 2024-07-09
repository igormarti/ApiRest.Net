using RestApiWithDontNet.Data.Converter.Impl;
using RestApiWithDontNet.Data.VO;
using RestApiWithDontNet.Models;
using RestApiWithDontNet.Repository;

namespace RestApiWithDontNet.Business.Impl
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly UserParser _userParser;

        public UserBusiness(IUserRepository userRepository, UserParser userParser) {
            _userRepository = userRepository;
            _userParser = userParser;
        }

        public UserVO FindById(long id)
        {
            User user = _userRepository.FindById(id);
            if (user == null) {
                throw new Exception("Usuário não encontrado.");
            }
            return _userParser.Parse(user);
        }

        public List<UserVO> FindAll()
        {
            return _userParser.Parse(_userRepository.FindAll());
        }

        public UserVO Create(UserVO userVo)
        {
            try
            {
                User userEntity = _userParser.Parse(userVo); 
                User user = _userRepository.Create(userEntity);
                return _userParser.Parse(user);
            } catch (Exception)
            {
                throw;
            }
        }

        public UserVO Update(long id, UserVO user)
        {
            try
            {
                User userOld = _userRepository.FindById(id);

                if (userOld == null) { 
                    throw new Exception("Usuário não encontrado");
                }

                User newUser = _userParser.Parse(user);
                
                User userEntity = _userRepository.Update(id, newUser, userOld);
                return _userParser.Parse(userEntity);
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
