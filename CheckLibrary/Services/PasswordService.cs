using System.Security.Cryptography;
using System.Text;

namespace CheckLibrary.Services
{
    public class PasswordService
    {

        public static string CriptographyPassword(string password)
        {
            var md5 = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(bytes);

            var sb = new StringBuilder();

            foreach (var caracter in hash)
            {
                sb.Append(caracter.ToString("X2"));
            }

            return sb.ToString();
        }

        public static Boolean VerifyPassword(string enterPassword, string userPassword)
        {
            string _enterPassword = CriptographyPassword(enterPassword);
            return _enterPassword.Equals(userPassword);
        }
    }
}