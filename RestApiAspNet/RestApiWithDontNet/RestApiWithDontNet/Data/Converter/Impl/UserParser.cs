using RestApiWithDontNet.Data.Converter.Contracts;
using RestApiWithDontNet.Data.VO;
using RestApiWithDontNet.Models;

namespace RestApiWithDontNet.Data.Converter.Impl
{
    public class UserParser : IParser<User, UserVO>, IParser<UserVO, User>
    {
        public UserVO? Parse(User? input)
        {
            if (input == null) return null;
            return new UserVO() {
                CodUser = input.Id,
                NameUser = input.Name,
                EmailUser = input.Email,
                AddressUser = input.Address,
                GenderUser = input.Gender != null ? input.Gender: "Not Defined"
            };
        }

        public User? Parse(UserVO? input)
        {
            if (input == null) return null;
            return new User()
            {
                Id=input.CodUser,
                Name=input.NameUser,
                Email=input.EmailUser,
                Address=input.AddressUser,
                Gender=input.GenderUser,
            };
        }

        public List<User?> Parse(List<UserVO?> input)
        {
            if (input == null) return null;

            return input.Select(item => Parse(item)).ToList();
        }

        public List<UserVO?> Parse(List<User?> input)
        {
            if (input == null) return null;

            return input.Select(item => Parse(item)).ToList();
        }
    }
}
