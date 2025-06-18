using BusinessLogicLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;       //DONT CHANGE
        private const int HashSize = 32;       //DONT CHANGE  
        private const int Iterations = 100000; //DONT CHANGE

        private static readonly HashAlgorithmName _algorithmName = HashAlgorithmName.SHA512;

        /// <summary>
        /// Consumes password, notice that it returns passsword with build in salt
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _algorithmName, HashSize);

            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}"; //storing salt in one column with hashed password
        }
        /// <summary>
        /// Gets input password and the hashed one. Splits password with built in salt by -
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public bool Verify(string password, string passwordHash)
        {
            string[] parts = passwordHash.Split('-');
            byte[] hash = Convert.FromHexString(parts[0]);
            byte[] salt = Convert.FromHexString(parts[1]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _algorithmName, HashSize);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }
    }
}
