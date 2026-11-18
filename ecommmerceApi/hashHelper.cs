

using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

public static class HashHelper
{
    public static string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using (var provider = RandomNumberGenerator.Create())
        {
            provider.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }
    public static string ComputeSha256Hash(string password, string salt)
    {
        using(SHA256 sha256Hash = SHA256.Create())
        {
            string saltedPassword = password + salt;
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            StringBuilder builder = new StringBuilder();
            for(int i=0;i<bytes.Length;i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}