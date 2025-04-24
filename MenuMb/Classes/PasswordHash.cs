using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes
{
    internal static class PasswordHash
    {
        public static string GetPasswordHash(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Пароль не может быть пустым.");
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                string hashString = Convert.ToHexString(hashBytes);
                return hashString;
            }
        }
    }
}
