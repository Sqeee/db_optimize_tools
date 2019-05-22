using System.Text;
using System.Security.Cryptography;

namespace DB_Optimize
{
    class DataProtection
    {
        public static string Encrypt(string data)
        {
            byte[] convertedData = Encoding.Default.GetBytes(data);
            byte[] encryptedData = Encrypt(convertedData);
            return Encoding.Default.GetString(encryptedData);
        }

        private static byte[] Encrypt(byte[] data)
        {
            return ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
        }

        public static string Decrypt(string data)
        {
            byte[] convertedData = Encoding.Default.GetBytes(data);
            byte[] decryptedData = Decrypt(convertedData);
            return Encoding.Default.GetString(decryptedData);
        }

        private static byte[] Decrypt(byte[] data)
        {
            return ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
        }
    }
}
