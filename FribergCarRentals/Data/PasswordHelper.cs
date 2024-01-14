using System.Security.Cryptography;
using System.Text;

namespace FribergCarRentals.Data
{
    internal static class PasswordHelper
    {
        #region Methods

        public static string CreateHashedPassword(string password)
        {
            var salt = new byte[16];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(20);
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);        
            string hashString = Encoding.UTF8.GetString(hashBytes, 0, hashBytes.Length);
            return hashString;
        }

        #endregion
    }
}
