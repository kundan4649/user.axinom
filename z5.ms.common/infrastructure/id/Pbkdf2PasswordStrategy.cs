using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace z5.ms.common.infrastructure.id
{
    /// <inheritdoc />
    /// <remarks>
    /// Generated hash has the following form: ${hash}${separator}${salt}
    /// </remarks>
    public class Pbkdf2PasswordStrategy : IPasswordEncryptionStrategy
    {
        private const int PasswordHashSize = 128 / 8;
        private const int SaltSize = 128 / 8;
        private const char Separator = ' ';
        
        /// <inheritdoc />
        public string HashPassword(string password)
        {
            var salt = GenerateSalt(SaltSize);
            var hash = GenerateSaltedHash(password, PasswordHashSize, salt);

            return EncodeHash(hash, salt);
        }

        /// <inheritdoc />
        public bool VerifyPassword(string password, string hash)
        {
            var (pwHash, salt) = DecodeHash(hash);

            if (pwHash == null || salt == null)
                return false;
            
            var inputHash = GenerateSaltedHash(password, PasswordHashSize, Convert.FromBase64String(salt));
            return pwHash == Convert.ToBase64String(inputHash);
        }
        
        private static byte[] GenerateSalt(int saltSize)
        {
            var salt = new byte[saltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private static byte[] GenerateSaltedHash(string password, int hashSize, byte[] salt) 
            => KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, 10000, hashSize);
        
        private static string EncodeHash(byte[] hash, byte[] salt)
            => $"{Convert.ToBase64String(hash)}{Separator}{Convert.ToBase64String(salt)}";

        private static (string, string) DecodeHash(string hash)
        {
            var splits = hash.Split(Separator);
            var pwHash = splits[0];
            string salt = null;
            if (splits.Length == 2)
                salt = splits[1];
            
            return (pwHash, salt);
        }
    }
}