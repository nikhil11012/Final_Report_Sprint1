using System;
using System.Security.Cryptography;
using System.Text;

namespace SecureUserApp.Helpers
{
    
    public class EncryptionHelper
    {
        private static readonly byte[] AesKey = Encoding.UTF8.GetBytes("MySecretKey12345");   // 16 bytes = 128-bit
        private static readonly byte[] AesIV  = Encoding.UTF8.GetBytes("MyInitVector1234");  // 16 bytes

        // Hashes a plain text password using SHA-256
        public static string HashPassword(string plainPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(plainPassword);
                byte[] hashBytes  = sha256.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }
        public static string EncryptData(string plainText)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = AesKey;
                    aes.IV  = AesIV;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                    byte[] inputBytes          = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes      = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

                    return Convert.ToBase64String(encryptedBytes);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Encryption failed.", ex);
            }
        }
        public static string DecryptData(string encryptedText)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = AesKey;
                    aes.IV  = AesIV;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    byte[] encryptedBytes      = Convert.FromBase64String(encryptedText);
                    byte[] decryptedBytes      = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Decryption failed.", ex);
            }
        }
    }
}
