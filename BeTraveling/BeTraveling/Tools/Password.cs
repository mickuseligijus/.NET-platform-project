using System.Security.Cryptography;
using System.Text;

namespace BeTraveling.Tools
{
    public class Password
    {
        public static string HashPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asByteArray);

            return Convert.ToBase64String(hashedPassword);
        }

        
    }
}
