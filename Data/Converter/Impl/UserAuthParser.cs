using RestApiWithDontNet.Data.Converter.Contracts;
using RestApiWithDontNet.Data.VO;
using RestApiWithDontNet.Models;

namespace RestApiWithDontNet.Data.Converter.Impl
{
    public class UserAuthParser : IParser<UserAuth, UserAuthOutputVO>, IParser<UserAuthInputVO, UserAuth>
    {
        public UserAuth? Parse(UserAuthInputVO? input)
        {
            if (input == null) return null;

            return new UserAuth
            {
                FullName = input.FullName,
                UserName = input.UserName,
                Password = input.Password,
                RefreshTokenExpireTime = input.RefreshTokenExpireTime,
            };
        }

        public List<UserAuth?> Parse(List<UserAuthInputVO?> input)
        {
            if (input == null) return null;

            return input.Select(item => Parse(item)).ToList();
        }

        public UserAuthOutputVO? Parse(UserAuth? input)
        {
            if (input == null) return null;

            return new UserAuthOutputVO
            {
                CodUser = input.Id,
                FullName = input.FullName,
                UserName = input.UserName
            };
        }

        public List<UserAuthOutputVO?> Parse(List<UserAuth?> input)
        {
            if (input == null) return null;

            return input.Select(item => Parse(item)).ToList();
        }
    }
}
