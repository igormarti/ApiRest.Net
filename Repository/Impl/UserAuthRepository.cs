using RestApiWithDontNet.Data.VO;
using RestApiWithDontNet.Models;
using RestApiWithDontNet.Models.Context;
using RestApiWithDontNet.Services.Impl;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace RestApiWithDontNet.Repository.Impl
{
    public class UserAuthRepository : IUserAuthRepository
    {

        private readonly MySqlContext _context;

        public UserAuthRepository(MySqlContext context)
        {
            _context = context;
        }

        public UserAuth? CreateUserAuth(UserAuth user)
        {
            _context.UsersAuth.Add(user);
            _context.SaveChanges();
            return user;
        }

        public UserAuth? FindUserAuth(long id)
        {
            var userFound = _context.UsersAuth
                .SingleOrDefault(u => u.Id == id);
            return userFound;
        }

        public UserAuth? FindUserAuthByUserName(string userName)
        {
            var userFound = _context.UsersAuth
                .SingleOrDefault(u => u.UserName.Equals(userName));
            return userFound;
        }

        public UserAuth? RefreshUserAuthInfo(UserAuth user)
        {
            if (user == null) return null;

            var result = FindUserAuth(user.Id);
            if (result == null) return null;

            try
            {
                _context.UsersAuth.Entry(result)
                    .CurrentValues.SetValues(user);
                _context.SaveChanges();
            }
            catch (Exception) {
                throw;
            }
           
            return result;
        }

        public UserAuth? ValidateCredentials(UserAuthVO user)
        {
            if(user == null) return null;

            string pass = HashAlgorithmService.ComputerHash(user.Password, SHA256.Create());
            return _context.UsersAuth.SingleOrDefault(u =>
            (u.UserName == user.UserName) && (u.Password == pass));

        }

        public UserAuth? ValidateCredentials(string userName)
        {
            return FindUserAuthByUserName(userName);
        }

        public bool RevokeToken(string userName)
        {
            var userFound = FindUserAuthByUserName(userName);
            if (userFound == null) return false;

            userFound.RefreshToken = null;
            _context.SaveChanges();
            return true;
        }

        private string ComputerHash(string input, HashAlgorithm hashAlgorithm)
        {
            byte[] hashedBytes = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashedBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

    }
}
