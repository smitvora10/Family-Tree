using FamilyTree.BL.Services;
using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;

public class TokenService : ITokenService
{
    private readonly string _encryptionKey;

    public TokenService(string encryptionKey)
    {
        _encryptionKey = encryptionKey;
    }

    public string GenerateToken(int userId, int roleId)
    {
        var plainText = $"{userId}:{roleId}:{DateTime.UtcNow.AddHours(24):O}";
        var plainBytes = Encoding.UTF8.GetBytes(plainText);

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_encryptionKey.PadRight(32).Substring(0, 32));
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        var combined = new byte[aes.IV.Length + cipherBytes.Length];
        Buffer.BlockCopy(aes.IV, 0, combined, 0, aes.IV.Length);
        Buffer.BlockCopy(cipherBytes, 0, combined, aes.IV.Length, cipherBytes.Length);

        return Convert.ToBase64String(combined)
            .Replace('+', '-').Replace('/', '_').Replace("=", ""); // URL-safe
    }

    public (int userId, int roleId) ValidateToken(string token)
    {
        try
        {
            byte[] tokenBytes = Convert.FromBase64String(PadBase64(token.Replace('-', '+').Replace('_', '/')));
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_encryptionKey.PadRight(32).Substring(0, 32));

            byte[] iv = new byte[16];
            Buffer.BlockCopy(tokenBytes, 0, iv, 0, iv.Length);
            aes.IV = iv;

            byte[] cipherBytes = new byte[tokenBytes.Length - iv.Length];
            Buffer.BlockCopy(tokenBytes, iv.Length, cipherBytes, 0, cipherBytes.Length);

            using ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            string decryptedText = Encoding.UTF8.GetString(decryptedBytes);
            string[] parts = decryptedText.Split(':');
            if (parts.Length < 3)
                throw new Exception("Token format invalid");

            int userId = int.Parse(parts[0]);
            int roleId = int.Parse(parts[1]);
            return (userId, roleId);
        }
        catch (Exception ex)
        {
            // You can log this or rethrow
            throw new SecurityException("Invalid token", ex);
        }
    }

    private string PadBase64(string base64)
    {
        while (base64.Length % 4 != 0)
            base64 += "=";
        return base64;
    }
}
