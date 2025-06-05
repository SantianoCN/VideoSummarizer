
using System.Security.Cryptography;
using System.Text;

namespace VideoSummarizer.Persistence;

public class HashFunctions
{
    public (string hash, string salt) GetPasswordHash(string password)
    {
        using(SHA512 sha = SHA512.Create()) {
            byte[] saltBytes = new byte[8];
            new Random().NextBytes(saltBytes);
            string salt = Convert.ToBase64String(saltBytes);

            byte[] buffer = Encoding.UTF8.GetBytes(password + salt);

            byte[] hashBytes = sha.ComputeHash(buffer);

            return (Convert.ToBase64String(hashBytes), salt);
        }
    }

    public bool IsValidPasswordHash(string validHash, string password, string salt)
    {
        using(SHA512 sha = SHA512.Create()) {
            byte[] saltBytes = new byte[8];
            new Random().NextBytes(saltBytes);

            byte[] buffer = Encoding.UTF8.GetBytes(password + salt);

            byte[] hashBytes = sha.ComputeHash(buffer);

            if (Convert.ToBase64String(hashBytes) == validHash) {
                return true;
            }
            return false;
        }
    } 
}