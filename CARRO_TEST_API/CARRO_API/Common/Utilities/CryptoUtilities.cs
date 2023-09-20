using System.Security.Cryptography;
using System.Text;

namespace CARRO_API.Common.Utilities
{
    public class CryptoUtilities
    {
        public static string EncryptionKey = "92{}oC9^E*pcj`OCk<tx5!vW1RaT8#b6\n";
        public static string Encrypt(string input, string _EncryptionKey = null)
        {
            if (_EncryptionKey == null) _EncryptionKey = EncryptionKey;
            byte[] inputArray = Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = Encoding.UTF8.GetBytes(_EncryptionKey);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string input, string _EncryptionKey = null)
        {
            if (_EncryptionKey == null) _EncryptionKey = EncryptionKey;
            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = Encoding.UTF8.GetBytes(_EncryptionKey);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Encoding.UTF8.GetString(resultArray);
        }
        public static string sha256_hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
