
using RestApiWithDontNet.Hypermedia;
using RestApiWithDontNet.Hypermedia.Abstract;

namespace RestApiWithDontNet.Data.VO
{
    public class UserVO:ISupportsHyperMedia
    {
        public long CodUser {  get; set; }
        public string NameUser { get; set; }
        public string EmailUser { get; set; }
        public string AddressUser { get; set; }
        public string GenderUser { get; set; }
        public bool Enabled { get; set; }
        public List<HyperMediaLink> Links { get ; set ; } = new List<HyperMediaLink>();


        public UserVO() { }
        public UserVO(int codUser, string nameUser, string emailUser, string addressUser, string genderUser, bool enabled)
        {
            CodUser = codUser;
            NameUser = nameUser;
            EmailUser = emailUser;
            AddressUser = addressUser;
            GenderUser = genderUser;
            Enabled = enabled;
        }

        public override bool Equals(object? obj)
        {
            return obj is UserVO vO &&
                   CodUser == vO.CodUser;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CodUser, NameUser, EmailUser, AddressUser, GenderUser);
        }
    }
}
