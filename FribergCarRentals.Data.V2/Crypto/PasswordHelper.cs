using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace FribergCarRentals.DataAccess.Crypto
{
    /// <summary>
    /// A helper class for creating and verifying hashed passwords.
    /// </summary>
    /// <remarks>Coden taken from: <see href="https://github.com/aspnet/AspNetIdentity/blob/main/src/Microsoft.AspNet.Identity.Core/Crypto.cs"/></remarks>
    internal static class PasswordHelper
    {
        #region Constants

        /// <summary>
        /// PBKDF2 iteration count.
        /// </summary>
        private const int PBKDF2IterationCount = 1000; // default for Rfc2898DeriveBytes

        /// <summary>
        /// PBKDF2 sub key length.
        /// </summary>
        private const int PBKDF2SubkeyLength = 256 / 8; // 256 bits

        /// <summary>
        /// Salt size.
        /// </summary>
        private const int SaltSize = 128 / 8; // 128 bits

        #endregion

        #region Methods

        /// <summary>
        /// Creates a hashed password.
        /// </summary>
        /// <param name="rawPassword">The raw password to hash.</param>
        /// <returns>A <see cref="string"/> containg the hashed password.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string HashPassword(string rawPassword)
        {
            #region Checks

            if (string.IsNullOrEmpty(rawPassword))
            {
                throw new ArgumentException($"The value of parameter '{rawPassword}' can't be empty.", nameof(rawPassword));
            }

            #endregion

            // Produce a version 0 (see comment above) text hash.
            byte[] salt;
            byte[] subkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(rawPassword, SaltSize, PBKDF2IterationCount))
            {
                salt = deriveBytes.Salt;
                subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }

            var outputBytes = new byte[1 + SaltSize + PBKDF2SubkeyLength];
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, PBKDF2SubkeyLength);
            return Convert.ToBase64String(outputBytes);
        }

        /// <summary>
        /// Verifies that a raw password matches a hashed password.
        /// </summary>
        /// <param name="hashedPassword">The hashed password to compare against. Must be of the format of HashWithPassword (salt + Hash(salt+input).</param>
        /// <param name="rawPassword">The raw password to verify.</param>
        /// <returns>True if the passwords matches.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool VerifyAgainstHashedPassword(string hashedPassword, string rawPassword)
        {
            #region Checks
            
            if (string.IsNullOrEmpty(hashedPassword))
            {
                throw new ArgumentException($"The value of parameter '{hashedPassword}' can't be empty.", nameof(hashedPassword));
            }

            if (string.IsNullOrEmpty(rawPassword))
            {
                throw new ArgumentException($"The value of parameter '{rawPassword}' can't be empty.", nameof(rawPassword));
            }

            #endregion

            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

            // Verify a version 0 (see comment above) text hash.
            if (hashedPasswordBytes.Length != 1 + SaltSize + PBKDF2SubkeyLength || hashedPasswordBytes[0] != 0x00)
            {
                // Wrong length or version header.
                return false;
            }

            var salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
            var storedSubkey = new byte[PBKDF2SubkeyLength];
            Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, PBKDF2SubkeyLength);

            byte[] generatedSubkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(rawPassword, salt, PBKDF2IterationCount))
            {
                generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }
            return ByteArraysEqual(storedSubkey, generatedSubkey);
        }

        /// <summary>
        /// Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
        /// </summary>
        /// <param name="firstArray">The first array.</param>
        /// <param name="secondArray">The second array.</param>
        /// <returns>True if <paramref name="firstArray"/> and <paramref name="secondArray"/> is equal.</returns>
        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] firstArray, byte[] secondArray)
        {
            if (ReferenceEquals(firstArray, secondArray))
            {
                return true;
            }

            if (firstArray == null || secondArray == null || firstArray.Length != secondArray.Length)
            {
                return false;
            }

            bool areSame = true;

            for (var i = 0; i < firstArray.Length; i++)
            {
                areSame &= firstArray[i] == secondArray[i];
            }

            return areSame;
        }

        #endregion
    }
}
