using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace QL_CAFE
{
    internal class HashHelper
    {
        private const int Iterations = 100000;

        public static string HashPassword(string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(salt);
            using (var derive = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = derive.GetBytes(32);
                return $"PBKDF2${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
            }
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash)) return false;
            if (storedHash.StartsWith("PBKDF2$", StringComparison.Ordinal))
            {
                string[] parts = storedHash.Split('$');
                if (parts.Length != 4 || !int.TryParse(parts[1], out int iterations)) return false;
                try
                {
                    byte[] salt = Convert.FromBase64String(parts[2]);
                    byte[] expected = Convert.FromBase64String(parts[3]);
                    using (var derive = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
                        return SlowEquals(expected, derive.GetBytes(expected.Length));
                }
                catch (FormatException) { return false; }
            }
            return SlowEquals(Encoding.UTF8.GetBytes(storedHash), Encoding.UTF8.GetBytes(LegacySha256(password)))
                || SlowEquals(Encoding.UTF8.GetBytes(storedHash), Encoding.UTF8.GetBytes(password));
        }

        public static bool NeedsUpgrade(string storedHash) =>
            string.IsNullOrEmpty(storedHash) || !storedHash.StartsWith("PBKDF2$", StringComparison.Ordinal);

        private static string LegacySha256(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)))
                    .Replace("-", "").ToLowerInvariant();
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            int diff = a.Length ^ b.Length;
            int length = Math.Min(a.Length, b.Length);
            for (int i = 0; i < length; i++) diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}