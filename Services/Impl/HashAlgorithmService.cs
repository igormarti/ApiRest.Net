using System.Security.Cryptography;
using System.Text;

namespace RestApiWithDontNet.Services.Impl
{
    public static class HashAlgorithmService
    {
        public static string ComputerHash(string input, HashAlgorithm hashAlgorithm)
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
