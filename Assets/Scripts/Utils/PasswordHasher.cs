using System;
using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher
{
    private const int SaltSize = 16;   // 128-bit
    private const int HashSize = 32;   // 256-bit
    private const int Iterations = 100000;

    public static void CreatePasswordHash(string password, out string hash, out string salt)
    {
        byte[] saltBytes = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        using (var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            saltBytes,
            Iterations,
            HashAlgorithmName.SHA256))
        {
            byte[] hashBytes = pbkdf2.GetBytes(HashSize);
            hash = Convert.ToBase64String(hashBytes);
            salt = Convert.ToBase64String(saltBytes);
        }
    }

    public static bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        byte[] saltBytes = Convert.FromBase64String(storedSalt);
        byte[] storedHashBytes = Convert.FromBase64String(storedHash);

        using (var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            saltBytes,
            Iterations,
            HashAlgorithmName.SHA256))
        {
            byte[] computedHash = pbkdf2.GetBytes(HashSize);
            return CryptographicOperations.FixedTimeEquals(
                computedHash,
                storedHashBytes);
        }
    }
}
