using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnitSense.Extensions.Extensions;

namespace UnitSense.Extensions.Encryption
{
    public class HashEncryption
    {
        /// <summary>
        /// Encrypts a string using the MD5 hash encryption algorithm.
        /// Message Digest is 128-bit and is commonly used to verify data, by checking
        /// the 'MD5 checksum' of the data. Information on MD5 can be found at:
        /// 
        /// http://www.faqs.org/rfcs/rfc1321.html
        /// </summary>
        /// <param name="Data">A string containing the data to encrypt.</param>
        /// <returns>A string containing the string, encrypted with the MD5 hash.</returns>
        public static string MD5Hash(string Data)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Computes an MD5 hash for the provided file.
        /// </summary>
        /// <param name="filename">The full path to the file</param>
        /// <returns>A hexadecimal encoded MD5 hash for the file.</returns>
        public static string MD5FileHash(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(stream);

                    StringBuilder builder = new StringBuilder();
                    foreach (byte b in hash)
                    {
                        builder.AppendFormat("{0:x2}", b);
                    }
                    return builder.ToString();
                }
            }
        }
        /// <summary>
        /// Encrypte une chaine de caractère en utilisant l'algorithme SHA1
        /// </summary>
        /// <param name="text">chaine à hasher</param>
        /// <returns></returns>
        public static string SHA1Hash(string text)
        {
            var hasher = SHA1.Create();
            var sigBytes = Encoding.ASCII.GetBytes(text);
            return hasher.ComputeHash(sigBytes).ToHexString();
        }
        /// <summary>
        /// Encrypts a string using the SHA256 (Secure Hash Algorithm) algorithm.
        /// Details: http://www.itl.nist.gov/fipspubs/fip180-1.htm
        /// This works in the same manner as MD5, providing however 256bit encryption.
        /// </summary>
        /// <param name="Data">A string containing the data to encrypt.</param>
        /// <returns>A string containing the string, encrypted with the SHA256 algorithm.</returns>
        public static string SHA256Hash(string Data)
        {
            SHA256 sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Encrypts a string using the SHA384(Secure Hash Algorithm) algorithm.
        /// This works in the same manner as MD5, providing 384bit encryption.
        /// </summary>
        /// <param name="Data">A string containing the data to encrypt.</param>
        /// <returns>A string containing the string, encrypted with the SHA384 algorithm.</returns>
        public static string SHA384Hash(string Data)
        {
            SHA384 sha = SHA384.Create();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Encrypts a string using the SHA512 (Secure Hash Algorithm) algorithm.
        /// This works in the same manner as MD5, providing 512bit encryption.
        /// </summary>
        /// <param name="Data">A string containing the data to encrypt.</param>
        /// <returns>A string containing the string, encrypted with the SHA512 algorithm.</returns>
        public static string SHA512Hash(string Data)
        {
            SHA512 sha = SHA512.Create();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        public static string SHA1Hash(byte[] temp)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                var hash = sha1.ComputeHash(temp);
                return ByteArrayToString(hash);
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }


        private static byte[] StringEncode(string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }

        private static byte[] HashHMAC(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }

        public static String HmacSha256(String stringToSign, String key)
        {
            return Convert.ToBase64String(HashHMAC(StringEncode(key), StringEncode(stringToSign)));
        }

        static public string EncryptRJ256(string plainText, string keyString, string ivString)
        {
            string sRet;


            var encoding = new UTF8Encoding();
            var key = encoding.GetBytes(keyString);
            var iv = encoding.GetBytes(ivString);

            using (var rj = new RijndaelManaged())
            {
                try
                {
                    rj.Padding = PaddingMode.Zeros;
                    rj.Mode = CipherMode.CBC;
                    rj.KeySize = 256;
                    rj.BlockSize = 128;
                    rj.Key = key;
                    rj.IV = iv;

                    var ms = new MemoryStream();

                    using (var cs = new CryptoStream(ms, rj.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        sRet = Convert.ToBase64String(ms.ToArray());
                    }
                }
                finally
                {
                    rj.Clear();
                }
            }

            return sRet;
        }

        static public string DecryptRJ256(string cypherText, string keyString, string ivString)
        {
            string sRet;

            byte[] cypher = Convert.FromBase64String(cypherText);

            var encoding = new UTF8Encoding();
            var key = encoding.GetBytes(keyString);
            var iv = encoding.GetBytes(ivString);

            using (var rj = new RijndaelManaged())
            {
                try
                {
                    rj.Padding = PaddingMode.Zeros;
                    rj.Mode = CipherMode.CBC;
                    rj.KeySize = 256;
                    rj.BlockSize = 128;
                    rj.Key = key;
                    rj.IV = iv;

                    var ms = new MemoryStream(cypher);

                    using (var cs = new CryptoStream(ms, rj.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            sRet = sr.ReadLine();
                        }
                    }
                }
                finally
                {
                    rj.Clear();
                }
            }

            return sRet;
        }
    }
}