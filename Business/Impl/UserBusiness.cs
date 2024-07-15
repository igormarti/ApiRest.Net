using RestApiWithDontNet.Data.Converter.Impl;
using RestApiWithDontNet.Data.VO;
using RestApiWithDontNet.Hypermedia.Utils;
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

        public void Disable(long id)
        {
            try
            {
                if (!_userRepository.Disable(id)) throw new Exception("Error try disable user");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UserVO?> FindByName(string name)
        {
            var users = _userRepository.findByName(name);

            if (users.Count < 1||users==null) return null;

            return _userParser.Parse(users);
        }

        public PagedSearchVO<UserVO> findWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
        
            var sort = (!string.IsNullOrWhiteSpace(sortDirection)&&!sortDirection.Equals("desc")) 
                ? "asc" : "desc";
            var size = (pageSize < 1) ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            string conditionName = $" AND  u.name LIKE '%{name}%' ";
            string query = @"SELECT * FROM users u WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(name)) query += conditionName ;

            query += $" ORDER BY u.name ASC LIMIT {size} OFFSET {offset}";

            string countQuery = @"SELECT COUNT(*) FROM users u WHERE 1 = 1";
            if (!string.IsNullOrWhiteSpace(name)) countQuery += conditionName;

            var users = _userRepository.findWithPageSearch(query);
            int totalResults = _userRepository.GetCount(countQuery);

            return new PagedSearchVO<UserVO>
            {
                CurrentPage = page==0?1:page,
                TotalResults = totalResults,
                SortDirections = sort,
                List = _userParser.Parse(users),
                PageSize = size,
            };
        }
    }
}
