

using System.Security.Cryptography;
using System.Text;

namespace VideoSummarizer.UseCases.Services;

public class HashService
{
    public async Task<(string hash, string salt)> HashPassword(string password)
    {
        byte[] saltBytes = new byte[4];
        new Random().NextBytes(saltBytes);
        string salt = Convert.ToBase64String(saltBytes);

        byte[] buffer = Encoding.UTF8.GetBytes(password + salt);

        using (var sha = SHA512.Create())
        {
            var hash = sha.ComputeHash(buffer);

            return (Convert.ToBase64String(hash), salt);
        }
    }

    public async Task<bool> VerifyPassword(string hashedPassword, string password, string salt)
    {
        if (string.IsNullOrEmpty(hashedPassword) ||
            string.IsNullOrEmpty(password) ||
            string.IsNullOrEmpty(salt))
        {
            return false;
        }

        byte[] buffer = Encoding.UTF8.GetBytes(password + salt);

        using (var sha = SHA512.Create())
        {
            var hash = sha.ComputeHash(buffer);

            return Convert.ToBase64String(hash) == hashedPassword;
        }
    }
}